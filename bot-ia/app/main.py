import logging
from fastapi import FastAPI, HTTPException
from fastapi.middleware.cors import CORSMiddleware
from .models import QuestionRequest, AnswerResponse, HealthResponse, ReloadResponse, ErrorResponse
from .services.gemini_service import gemini_service
from .services.knowledge_service import knowledge_service
from .config import settings

# Configurar logging
logging.basicConfig(level=getattr(logging, settings.LOG_LEVEL))
logger = logging.getLogger(__name__)

app = FastAPI(
    title="MidTalk AI Bot",
    description="API de IA para responder perguntas baseadas na base de conhecimento do MidTalk",
    version="1.0.0"
)

# Configurar CORS
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

@app.get("/health", response_model=HealthResponse)
async def health_check():
    """Verifica o status do serviço"""
    return HealthResponse(
        status="healthy",
        gemini_available=gemini_service.model is not None,
        knowledge_loaded=knowledge_service.is_loaded()
    )

@app.post("/api/ai/ask", response_model=AnswerResponse)
async def ask_question(request: QuestionRequest):
    """Faz uma pergunta para a IA"""
    try:
        logger.info(f"Pergunta recebida: {request.question[:100]}...")
        
        result = await gemini_service.ask_question(request.question, request.user_name)
        
        return AnswerResponse(
            answer=result["answer"],
            confidence=result["confidence"],
            error=result["error"]
        )
    except Exception as e:
        logger.error(f"Erro ao processar pergunta: {e}")
        raise HTTPException(status_code=500, detail="Erro interno do servidor")

@app.get("/api/ai/knowledge/reload", response_model=ReloadResponse)
async def reload_knowledge():
    """Recarrega a base de conhecimento"""
    try:
        success = knowledge_service.reload_knowledge()
        
        return ReloadResponse(
            success=success,
            message="Base de conhecimento recarregada com sucesso" if success else "Erro ao recarregar base de conhecimento"
        )
    except Exception as e:
        logger.error(f"Erro ao recarregar conhecimento: {e}")
        return ReloadResponse(
            success=False,
            message=f"Erro: {str(e)}"
        )

@app.on_event("startup")
async def startup_event():
    """Evento executado na inicialização"""
    logger.info("Iniciando MidTalk AI Bot...")
    logger.info(f"Gemini disponível: {gemini_service.model is not None}")
    logger.info(f"Base de conhecimento carregada: {knowledge_service.is_loaded()}")

if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=settings.API_PORT)
