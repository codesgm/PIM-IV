import logging
import google.generativeai as genai
from typing import Optional, Dict, Any
from ..config import settings
from .knowledge_service import knowledge_service

logger = logging.getLogger(__name__)

class GeminiService:
    def __init__(self):
        self.model = None
        self._initialize_gemini()
    
    def _initialize_gemini(self):
        """Inicializa o cliente Gemini"""
        try:
            if not settings.GEMINI_API_KEY:
                logger.error("GEMINI_API_KEY não configurada")
                return
            
            genai.configure(api_key=settings.GEMINI_API_KEY)
            self.model = genai.GenerativeModel('gemini-2.0-flash')
            logger.info("Gemini inicializado com sucesso")
        except Exception as e:
            logger.error(f"Erro ao inicializar Gemini: {e}")
    
    def _create_prompt(self, question: str, user_name: str = None) -> str:
        """Cria o prompt contextualizado para o Gemini"""
        context = knowledge_service.get_context()
        
        user_greeting = f"O usuário se chama {user_name}. " if user_name else ""
        
    def _create_prompt(self, question: str, user_name: str = None) -> str:
        """Cria o prompt contextualizado para o Gemini"""
        context = knowledge_service.get_context()
        
        user_greeting = f"O usuário se chama {user_name}. " if user_name else ""
        
        prompt = f"""Você é um assistente virtual amigável e prestativo do sistema MidTalk.

PERSONALIDADE:
- Seja sempre simpático, cordial e acolhedor
- Use um tom conversacional e amigável
- Demonstre empatia e paciência
- Seja positivo e encorajador
- Use expressões como "Fico feliz em ajudar!", "Claro!", "Com prazer!"
- {user_greeting}Quando souber o nome, use-o de forma natural na conversa
- NÃO use emojis nas respostas

INSTRUÇÕES:
- Responda sempre em português brasileiro
- Seja claro e didático, explicando de forma simples
- Use apenas as informações da base de conhecimento fornecida
- Se não souber a resposta, seja honesto mas ofereça alternativas
- Mantenha respostas úteis e acessíveis
- Responda diretamente à pergunta feita, sem cumprimentos desnecessários
- Só cumprimente se for claramente uma saudação inicial
- Evite usar emojis ou símbolos especiais

BASE DE CONHECIMENTO:
{context}

PERGUNTA DO USUÁRIO: {question}

RESPOSTA (seja direto e útil, sem emojis):"""
        
        return prompt
    
    async def ask_question(self, question: str, user_name: str = None) -> Dict[str, Any]:
        """Faz uma pergunta para o Gemini e retorna a resposta"""
        try:
            if not self.model:
                return {
                    "answer": "Serviço de IA temporariamente indisponível. Entre em contato com o suporte.",
                    "confidence": 0.0,
                    "error": "Gemini não inicializado"
                }
            
            if not knowledge_service.is_loaded():
                return {
                    "answer": "Base de conhecimento não disponível. Entre em contato com o suporte.",
                    "confidence": 0.0,
                    "error": "Base de conhecimento não carregada"
                }
            
            prompt = self._create_prompt(question, user_name)
            
            response = self.model.generate_content(
                prompt,
                generation_config=genai.types.GenerationConfig(
                    max_output_tokens=settings.MAX_TOKENS,
                    temperature=settings.TEMPERATURE,
                )
            )
            
            if response.text:
                return {
                    "answer": response.text.strip(),
                    "confidence": 0.85,  # Valor fixo por enquanto
                    "error": None
                }
            else:
                return {
                    "answer": "Não consegui gerar uma resposta. Tente reformular sua pergunta.",
                    "confidence": 0.0,
                    "error": "Resposta vazia do Gemini"
                }
                
        except Exception as e:
            logger.error(f"Erro ao consultar Gemini: {e}")
            return {
                "answer": "Ocorreu um erro ao processar sua pergunta. Entre em contato com o suporte.",
                "confidence": 0.0,
                "error": str(e)
            }

# Instância global
gemini_service = GeminiService()
