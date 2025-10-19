import httpx
import uuid
import logging
from datetime import datetime, timedelta
from typing import Dict, Optional, List
from .models import ProxySession, MessageResponse, ChatState, MessageType
from .config import BACKEND_API_URL, SESSION_TIMEOUT
from .services.ai_service import ai_service
from .services.escalation_service import escalation_engine

logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

class ProxyService:
    def __init__(self):
        self.sessions: Dict[str, ProxySession] = {}
        self.client = httpx.AsyncClient(timeout=30.0)
    
    def generate_session_id(self) -> str:
        return f"proxy_{uuid.uuid4().hex[:8]}"
    
    async def create_session(self, user_name: str, user_email: str) -> str:
        """Cria uma nova sessão iniciando com IA"""
        session_id = self.generate_session_id()
        now = datetime.now()
        
        session = ProxySession(
            session_id=session_id,
            chat_id=None,  # Não cria chat no backend ainda
            user_name=user_name,
            user_email=user_email,
            created_at=now,
            last_activity=now,
            state=ChatState.AI_ACTIVE
        )
        
        self.sessions[session_id] = session
        logger.info(f"Sessão IA criada: {session_id} para {user_name}")
        return session_id
    
    async def handle_user_message(self, session_id: str, message: str) -> Dict:
        """Processa mensagem do usuário baseado no estado do chat"""
        session = self.get_session(session_id)
        if not session:
            return {"error": "Sessão não encontrada"}
        
        session.last_activity = datetime.now()
        
        if session.state == ChatState.AI_ACTIVE:
            return await self._handle_ai_message(session, message)
        elif session.state == ChatState.ESCALATION_PENDING:
            return await self._handle_escalation_confirmation(session, message)
        elif session.state == ChatState.HUMAN_ASSIGNED:
            return await self._handle_human_message(session, message)
        else:
            return {"error": f"Estado inválido: {session.state}"}
    
    async def _handle_ai_message(self, session: ProxySession, message: str) -> Dict:
        """Processa mensagem quando IA está ativa"""
        
        # Verificar se deve perguntar sobre escalação
        if escalation_engine.detect_escalation_intent(message):
            return await self._ask_escalation_confirmation(session, message)
        
        # Enviar para IA
        ai_response = await ai_service.ask_ai(message)
        session.ai_attempts += 1
        session.last_ai_confidence = ai_response.confidence
        
        # Verificar se deve escalar após resposta da IA
        should_escalate, reason = escalation_engine.should_escalate(session, message, ai_response.confidence)
        
        if should_escalate:
            # Primeiro retornar resposta da IA, depois perguntar sobre escalação
            response = {
                "type": "ai_with_escalation_question",
                "message": ai_response.answer,
                "confidence": ai_response.confidence,
                "escalation_reason": reason
            }
            logger.info(f"Enviando resposta com pergunta de escalação: {response}")
            return response
        
        response = {
            "type": "ai",
            "message": ai_response.answer,
            "confidence": ai_response.confidence
        }
        logger.info(f"Enviando resposta IA: {response}")
        return response
    
    async def _ask_escalation_confirmation(self, session: ProxySession, message: str) -> Dict:
        """Pergunta se usuário quer escalar para técnico"""
        session.state = ChatState.ESCALATION_PENDING
        session.escalation_reason = "Usuário mencionou palavra-chave"
        
        return {
            "type": "escalation_question",
            "message": "Você deseja falar com um técnico?",
            "options": ["Sim", "Não"]
        }
    
    async def _handle_escalation_confirmation(self, session: ProxySession, message: str) -> Dict:
        """Processa resposta da confirmação de escalação"""
        message_lower = message.lower().strip()
        
        if message_lower in ["sim", "s", "yes", "y", "quero", "ok"]:
            # Usuário confirmou escalação
            session.state = ChatState.AI_ACTIVE  # Reset para escalar
            return await self._escalate_to_human(session, "Escalação confirmada pelo usuário", "Usuário confirmou escalação")
        else:
            # Usuário não quer escalar, voltar para IA
            session.state = ChatState.AI_ACTIVE
            return {
                "type": "ai",
                "message": "Ok, vou continuar te ajudando. Em que posso ajudar?",
                "confidence": 1.0
            }
        """Processa mensagem quando técnico está atribuído"""
        if not session.chat_id:
            return {"error": "Chat não foi criado no backend"}
        
        await self.send_message_to_backend(session.chat_id, message)
        return {"type": "human", "status": "sent"}
    
    async def _escalate_to_human(self, session: ProxySession, message: str, reason: str) -> Dict:
        """Escala chat para técnico humano"""
        try:
            logger.info(f"Escalando sessão {session.session_id}: {reason}")
            
            session.state = ChatState.AI_ESCALATING
            session.escalation_reason = reason
            session.escalated_at = datetime.now()
            
            # Criar chat no backend
            chat_id = await self.create_backend_chat(
                session.user_name,
                session.user_email,
                f"[IA→Técnico] {message}"
            )
            
            session.chat_id = chat_id
            session.state = ChatState.HUMAN_ASSIGNED
            
            logger.info(f"Chat escalado: sessão {session.session_id} → chat {chat_id}")
            
            return {
                "type": "escalated",
                "chat_id": chat_id,
                "reason": reason,
                "message": "Transferindo para um técnico humano. Aguarde..."
            }
            
        except Exception as e:
            logger.error(f"Erro ao escalar: {e}")
            session.state = ChatState.AI_ACTIVE  # Voltar para IA
            return {"error": f"Erro na escalação: {str(e)}"}
    
    async def manual_escalate(self, session_id: str, reason: str, user_message: str = "") -> Dict:
        """Escalação manual solicitada pelo usuário"""
        session = self.get_session(session_id)
        if not session:
            return {"error": "Sessão não encontrada"}
        
        if session.state != ChatState.AI_ACTIVE:
            return {"error": "Chat já foi escalado"}
        
        return await self._escalate_to_human(session, user_message or "Escalação manual", reason)
    
    def get_session(self, session_id: str) -> Optional[ProxySession]:
        session = self.sessions.get(session_id)
        if session:
            session.last_activity = datetime.now()
        return session
    
    def get_chat_state(self, session_id: str) -> Optional[Dict]:
        """Retorna estado atual do chat"""
        session = self.get_session(session_id)
        if not session:
            return None
        
        return {
            "session_id": session_id,
            "state": session.state,
            "ai_attempts": session.ai_attempts,
            "last_confidence": session.last_ai_confidence,
            "escalation_reason": session.escalation_reason,
            "chat_id": session.chat_id
        }
    
    def cleanup_expired_sessions(self):
        now = datetime.now()
        expired = []
        
        for session_id, session in self.sessions.items():
            if now - session.last_activity > timedelta(seconds=SESSION_TIMEOUT):
                expired.append(session_id)
        
        for session_id in expired:
            del self.sessions[session_id]
            logger.info(f"Sessão expirada removida: {session_id}")
    
    # Métodos existentes do backend
    async def create_backend_chat(self, user_name: str, user_email: str, initial_message: str) -> int:
        url = f"{BACKEND_API_URL}/api/chats"
        data = {
            "UserName": user_name,
            "UserContact": "FAQ",
            "UserEmail": user_email,
            "InitialMessage": initial_message,
            "Source": "FAQ"
        }
        
        logger.info(f"Criando chat no backend: {user_name}")
        
        response = await self.client.post(url, json=data)
        response.raise_for_status()
        result = response.json()
        
        if isinstance(result, dict) and "data" in result:
            chat_id = result["data"]["id"]
            logger.info(f"Chat criado no backend: {chat_id}")
            return chat_id
        
        raise Exception("Resposta inválida do backend")
    
    async def send_message_to_backend(self, chat_id: int, message: str):
        url = f"{BACKEND_API_URL}/api/chats/{chat_id}/messages"
        data = {
            "senderType": "User",
            "message": message
        }
        
        response = await self.client.post(url, json=data)
        response.raise_for_status()
        logger.info(f"Mensagem enviada para chat {chat_id}")
    
    async def get_messages_from_backend(self, chat_id: int, after_id: int = 0) -> List[MessageResponse]:
        url = f"{BACKEND_API_URL}/api/chats/{chat_id}/messages"
        
        response = await self.client.get(url)
        response.raise_for_status()
        result = response.json()
        
        messages_data = result.get("data", [])
        messages = []
        
        for msg in messages_data:
            if msg["id"] > after_id:
                created_at = datetime.now()
                try:
                    created_at = datetime.fromisoformat(msg["createdAt"])
                except:
                    pass
                
                messages.append(MessageResponse(
                    id=msg["id"],
                    sender_type=msg["senderType"],
                    message=msg["message"],
                    created_at=created_at
                ))
        
        return messages
    
    def get_active_sessions_count(self) -> int:
        self.cleanup_expired_sessions()
        return len(self.sessions)
