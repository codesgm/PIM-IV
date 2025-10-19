import logging
import httpx
from typing import Dict, Any
from ..models import AIResponse
from ..config import BACKEND_API_URL

logger = logging.getLogger(__name__)

class AIService:
    def __init__(self):
        self.bot_ia_url = "http://bot-ia:8001"  # URL interna do container
        self.client = httpx.AsyncClient(timeout=30.0)
    
    async def ask_ai(self, question: str, user_name: str = None) -> AIResponse:
        """
        Faz uma pergunta para a IA e retorna a resposta
        """
        try:
            logger.info(f"Enviando pergunta para IA: {question[:100]}...")
            
            payload = {"question": question}
            if user_name:
                payload["user_name"] = user_name
            
            response = await self.client.post(
                f"{self.bot_ia_url}/api/ai/ask",
                json=payload,
                headers={"Content-Type": "application/json"}
            )
            
            if response.status_code == 200:
                data = response.json()
                ai_response = AIResponse(
                    answer=data.get("answer", ""),
                    confidence=data.get("confidence", 0.0),
                    error=data.get("error")
                )
                
                logger.info(f"IA respondeu com confidence: {ai_response.confidence}")
                return ai_response
            else:
                logger.error(f"Erro na API da IA: {response.status_code}")
                return self._fallback_response("Erro na comunicação com IA")
                
        except httpx.TimeoutException:
            logger.error("Timeout ao consultar IA")
            return self._fallback_response("IA demorou para responder")
        except Exception as e:
            logger.error(f"Erro ao consultar IA: {e}")
            return self._fallback_response("IA temporariamente indisponível")
    
    def _fallback_response(self, error_msg: str) -> AIResponse:
        """Resposta de fallback quando IA não está disponível"""
        return AIResponse(
            answer="Desculpe, estou com dificuldades técnicas. Um técnico humano irá te ajudar em breve.",
            confidence=0.0,
            error=error_msg
        )
    
    async def is_ai_available(self) -> bool:
        """Verifica se a IA está disponível"""
        try:
            response = await self.client.get(f"{self.bot_ia_url}/health")
            return response.status_code == 200
        except:
            return False

# Instância global
ai_service = AIService()
