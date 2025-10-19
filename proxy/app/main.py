from fastapi import FastAPI, HTTPException
from fastapi.middleware.cors import CORSMiddleware
import logging
import httpx
from .models import (
    StartChatRequest, SendMessageRequest, StartChatResponse, 
    MessagesListResponse, EscalateRequest, ChatStateResponse
)
from .proxy_service import ProxyService

logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

app = FastAPI(title="MidTalk Proxy", version="2.0.0")

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

proxy_service = ProxyService()

@app.get("/health")
async def health_check():
    return {"status": "healthy", "service": "midtalk-proxy", "version": "2.0.0"}

@app.get("/api/proxy/sessions")
async def get_sessions_info():
    count = proxy_service.get_active_sessions_count()
    return {"active_sessions": count}

@app.post("/api/proxy/start-chat")
async def start_chat(request: StartChatRequest):
    """Inicia chat com IA e retorna resposta inicial"""
    try:
        logger.info(f"Iniciando chat IA para: {request.user_name}")
        
        # Criar sessão IA
        session_id = await proxy_service.create_session(
            request.user_name,
            request.user_email
        )
        
        # Processar mensagem inicial com IA
        ai_response = await proxy_service.handle_user_message(session_id, request.initial_message)
        logger.info(f"Resposta inicial da IA: {ai_response}")
        
        # Retornar sessão + resposta inicial
        return {
            "session_id": session_id,
            "status": "ai_active",
            "initial_response": ai_response
        }
        
    except Exception as e:
        logger.error(f"Erro ao iniciar chat: {e}")
        raise HTTPException(status_code=500, detail=str(e))

@app.post("/api/proxy/send-message")
async def send_message(request: SendMessageRequest):
    """Envia mensagem (IA ou técnico baseado no estado)"""
    try:
        response = await proxy_service.handle_user_message(
            request.session_id,
            request.message
        )
        
        if "error" in response:
            raise HTTPException(status_code=404, detail=response["error"])
        
        return response
        
    except HTTPException:
        raise
    except Exception as e:
        logger.error(f"Erro ao enviar mensagem: {e}")
        raise HTTPException(status_code=500, detail=str(e))

@app.post("/api/proxy/escalate")
async def escalate_chat(request: EscalateRequest):
    """Escalação manual para técnico"""
    try:
        response = await proxy_service.manual_escalate(
            request.session_id,
            request.reason,
            request.user_message or ""
        )
        
        if "error" in response:
            raise HTTPException(status_code=404, detail=response["error"])
        
        return response
        
    except HTTPException:
        raise
    except Exception as e:
        logger.error(f"Erro na escalação: {e}")
        raise HTTPException(status_code=500, detail=str(e))

@app.get("/api/proxy/chat-state/{session_id}", response_model=ChatStateResponse)
async def get_chat_state(session_id: str):
    """Retorna estado atual do chat"""
    state = proxy_service.get_chat_state(session_id)
    if not state:
        raise HTTPException(status_code=404, detail="Sessão não encontrada")
    
    return ChatStateResponse(**state)

@app.get("/api/proxy/messages/{session_id}", response_model=MessagesListResponse)
async def get_messages(session_id: str):
    """Busca mensagens (apenas para chats escalados)"""
    session = proxy_service.get_session(session_id)
    if not session:
        raise HTTPException(status_code=404, detail="Sessão não encontrada")
    
    # Só buscar mensagens se chat foi escalado
    if session.chat_id is None:
        return MessagesListResponse(messages=[])
    
    try:
        messages = await proxy_service.get_messages_from_backend(
            session.chat_id,
            session.last_message_id
        )
        
        # Atualizar último message_id apenas para mensagens de técnico
        if messages:
            technician_messages = [msg for msg in messages if msg.sender_type.lower() == 'technician']
            if technician_messages:
                session.last_message_id = max(msg.id for msg in messages)
        
        return MessagesListResponse(messages=messages)
        
    except Exception as e:
        logger.error(f"Erro ao buscar mensagens: {e}")
        raise HTTPException(status_code=500, detail=str(e))

# Endpoints de debug/teste (manter por compatibilidade)
@app.post("/api/proxy/debug-request")
async def debug_request(request: dict):
    logger.info(f"Debug - Request raw: {request}")
    return {"received": request}

@app.get("/api/proxy/test-backend")
async def test_backend():
    try:
        url = f"http://api:5000/api/chats"
        data = {
            "userName": "Teste",
            "userContact": "teste@email.com",
            "initialMessage": "Teste",
            "source": "FAQ"
        }
        
        async with httpx.AsyncClient() as client:
            response = await client.post(url, json=data)
            result = response.json()
            return {"status": "success", "response": result}
    except Exception as e:
        return {"status": "error", "error": str(e)}

if __name__ == "__main__":
    import uvicorn
    from .config import PROXY_PORT
    uvicorn.run(app, host="0.0.0.0", port=PROXY_PORT)
