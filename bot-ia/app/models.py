from pydantic import BaseModel, Field
from typing import Optional

class QuestionRequest(BaseModel):
    question: str = Field(..., min_length=1, max_length=1000, description="Pergunta do usuário")

class AnswerResponse(BaseModel):
    answer: str = Field(..., description="Resposta da IA")
    confidence: float = Field(..., ge=0.0, le=1.0, description="Nível de confiança da resposta")
    error: Optional[str] = Field(None, description="Mensagem de erro, se houver")

class ErrorResponse(BaseModel):
    detail: str = Field(..., description="Detalhes do erro")
    error_code: Optional[str] = Field(None, description="Código do erro")

class HealthResponse(BaseModel):
    status: str = Field(..., description="Status do serviço")
    gemini_available: bool = Field(..., description="Se o Gemini está disponível")
    knowledge_loaded: bool = Field(..., description="Se a base de conhecimento está carregada")

class ReloadResponse(BaseModel):
    success: bool = Field(..., description="Se o reload foi bem-sucedido")
    message: str = Field(..., description="Mensagem de status")
