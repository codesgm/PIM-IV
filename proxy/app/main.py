from fastapi import FastAPI, HTTPException
from fastapi.middleware.cors import CORSMiddleware
import logging
import httpx
from .models import StartChatRequest, SendMessageRequest, StartChatResponse, MessagesListResponse
from .proxy_service import ProxyService

logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

app = FastAPI(title="MidTalk Proxy", version="1.0.0")

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
    return {"status": "healthy", "service": "midtalk-proxy"}

@app.get("/api/proxy/sessions")
async def get_sessions_info():
    count = proxy_service.get_active_sessions_count()
    return {"active_sessions": count}

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

@app.post("/api/proxy/debug-request")
async def debug_request(request: dict):
    logger.info(f"Debug - Request raw: {request}")
    return {"received": request}

@app.post("/api/proxy/start-chat-simple")
async def start_chat_simple(request: dict):
    logger.info(f"Request dict recebido: {request}")
    
    user_name = request.get('user_name', '')
    user_email = request.get('user_email', '')
    initial_message = request.get('initial_message', '')
    
    logger.info(f"Dados extraídos - Nome: {user_name}, Email: {user_email}, Mensagem: {initial_message}")
    
    try:
        # Criar chat no backend
        chat_id = await proxy_service.create_backend_chat(
            user_name,
            user_email,
            initial_message
        )
        
        # Criar sessão no proxy
        session_id = proxy_service.create_session(chat_id, user_name)
        
        return {
            "session_id": session_id,
            "status": "created"
        }
    except Exception as e:
        logger.error(f"Erro: {str(e)}")
        raise HTTPException(status_code=500, detail=str(e))

@app.post("/api/proxy/start-chat", response_model=StartChatResponse)
async def start_chat(request: StartChatRequest):
    logger.info(f"Request completo recebido: {request.dict()}")
    logger.info(f"User name: {request.user_name}")
    logger.info(f"User email: {request.user_email}")
    logger.info(f"Initial message: {request.initial_message}")
    
    try:
        # Criar chat no backend
        chat_id = await proxy_service.create_backend_chat(
            request.user_name,
            request.user_email,
            request.initial_message
        )
        
        logger.info(f"Chat criado com ID: {chat_id}")
        
        # Criar sessão no proxy
        session_id = proxy_service.create_session(chat_id, request.user_name)
        
        logger.info(f"Sessão criada: {session_id}")
        
        return StartChatResponse(
            session_id=session_id,
            status="created"
        )
    except Exception as e:
        logger.error(f"Erro detalhado ao iniciar chat: {type(e).__name__}: {str(e)}")
        import traceback
        logger.error(f"Traceback: {traceback.format_exc()}")
        raise HTTPException(status_code=500, detail=f"Erro interno: {str(e)}")

@app.post("/api/proxy/send-message")
async def send_message(request: SendMessageRequest):
    # Validar sessão
    session = proxy_service.get_session(request.session_id)
    if not session:
        raise HTTPException(status_code=404, detail="Sessão não encontrada")
    
    try:
        # Enviar mensagem para o backend
        await proxy_service.send_message_to_backend(
            session.chat_id, 
            request.message
        )
        
        return {"status": "sent"}
    except Exception as e:
        logger.error(f"Erro ao enviar mensagem: {e}")
        raise HTTPException(status_code=500, detail="Erro interno do servidor")

@app.get("/api/proxy/messages/{session_id}", response_model=MessagesListResponse)
async def get_messages(session_id: str):
    # Validar sessão
    session = proxy_service.get_session(session_id)
    if not session:
        raise HTTPException(status_code=404, detail="Sessão não encontrada")
    
    try:
        # Buscar mensagens do backend
        messages = await proxy_service.get_messages_from_backend(
            session.chat_id, 
            session.last_message_id
        )
        
        # Atualizar último message_id
        if messages:
            session.last_message_id = max(msg.id for msg in messages)
        
        return MessagesListResponse(messages=messages)
    except Exception as e:
        logger.error(f"Erro detalhado ao buscar mensagens: {type(e).__name__}: {str(e)}")
        import traceback
        logger.error(f"Traceback: {traceback.format_exc()}")
        raise HTTPException(status_code=500, detail=f"Erro interno: {str(e)}")

if __name__ == "__main__":
    import uvicorn
    from .config import PROXY_PORT
    uvicorn.run(app, host="0.0.0.0", port=PROXY_PORT)
