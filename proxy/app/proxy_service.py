import httpx
import uuid
import logging
from datetime import datetime, timedelta
from typing import Dict, Optional, List
from .models import ProxySession, MessageResponse
from .config import BACKEND_API_URL, SESSION_TIMEOUT

logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

class ProxyService:
    def __init__(self):
        self.sessions: Dict[str, ProxySession] = {}
        self.client = httpx.AsyncClient(timeout=30.0)
    
    def generate_session_id(self) -> str:
        return f"proxy_{uuid.uuid4().hex[:8]}"
    
    def create_session(self, chat_id: int, user_name: str) -> str:
        session_id = self.generate_session_id()
        now = datetime.now()
        
        session = ProxySession(
            session_id=session_id,
            chat_id=chat_id,
            user_name=user_name,
            created_at=now,
            last_activity=now
        )
        
        self.sessions[session_id] = session
        logger.info(f"Sessão criada: {session_id} -> Chat {chat_id}")
        return session_id
    
    def get_session(self, session_id: str) -> Optional[ProxySession]:
        session = self.sessions.get(session_id)
        if session:
            session.last_activity = datetime.now()
        return session
    
    def cleanup_expired_sessions(self):
        now = datetime.now()
        expired = []
        
        for session_id, session in self.sessions.items():
            if now - session.last_activity > timedelta(seconds=SESSION_TIMEOUT):
                expired.append(session_id)
        
        for session_id in expired:
            del self.sessions[session_id]
            logger.info(f"Sessão expirada removida: {session_id}")
    
    async def create_backend_chat(self, user_name: str, user_email: str, initial_message: str) -> int:
        url = f"{BACKEND_API_URL}/api/chats"
        data = {
            "UserName": user_name,
            "UserContact": "FAQ",
            "UserEmail": user_email,
            "InitialMessage": initial_message,
            "Source": "FAQ"
        }
        
        logger.info(f"Enviando request para: {url}")
        logger.info(f"Dados: {data}")
        
        response = await self.client.post(url, json=data)
        response.raise_for_status()
        result = response.json()
        
        logger.info(f"Resposta completa: {result}")
        
        # Verificar se a resposta tem a estrutura esperada
        if isinstance(result, dict) and "data" in result:
            data_obj = result["data"]
            if isinstance(data_obj, dict) and "id" in data_obj:
                chat_id = data_obj["id"]
                logger.info(f"Chat criado com sucesso - ID: {chat_id}")
                return chat_id
        
        logger.error(f"Estrutura de resposta inválida: {result}")
        raise Exception("Resposta do backend não contém data.id")
    
    async def send_message_to_backend(self, chat_id: int, message: str):
        url = f"{BACKEND_API_URL}/api/chats/{chat_id}/messages"
        data = {
            "senderType": "User",
            "message": message
        }
        
        try:
            response = await self.client.post(url, json=data)
            response.raise_for_status()
            logger.info(f"Mensagem enviada para chat {chat_id}")
        except Exception as e:
            logger.error(f"Erro ao enviar mensagem: {e}")
            raise
    
    async def get_messages_from_backend(self, chat_id: int, after_id: int = 0) -> List[MessageResponse]:
        url = f"{BACKEND_API_URL}/api/chats/{chat_id}/messages"
        
        try:
            response = await self.client.get(url)
            response.raise_for_status()
            result = response.json()
            
            logger.info(f"Tipo da resposta: {type(result)}")
            logger.info(f"Resposta das mensagens: {result}")
            
            # Extrair mensagens do objeto data
            messages_data = result.get("data", [])
            logger.info(f"Messages data: {messages_data}")
            
            messages = []
            for msg in messages_data:
                logger.info(f"Processando mensagem: {msg}")
                if msg["id"] > after_id:
                    # Simplificar parsing da data
                    created_at = datetime.now()  # Temporário para debug
                    try:
                        created_at = datetime.fromisoformat(msg["createdAt"])
                    except:
                        logger.warning(f"Erro ao parsear data: {msg['createdAt']}")
                    
                    messages.append(MessageResponse(
                        id=msg["id"],
                        sender_type=msg["senderType"],
                        message=msg["message"],
                        created_at=created_at
                    ))
            
            logger.info(f"Mensagens processadas: {len(messages)}")
            return messages
        except Exception as e:
            logger.error(f"Erro detalhado ao buscar mensagens: {type(e).__name__}: {str(e)}")
            import traceback
            logger.error(f"Traceback: {traceback.format_exc()}")
            raise
    
    def get_active_sessions_count(self) -> int:
        self.cleanup_expired_sessions()
        return len(self.sessions)
