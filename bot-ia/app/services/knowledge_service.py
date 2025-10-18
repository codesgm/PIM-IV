import os
import logging
from typing import Optional
from ..config import settings

logger = logging.getLogger(__name__)

class KnowledgeService:
    def __init__(self):
        self._knowledge_content: Optional[str] = None
        self.load_knowledge()
    
    def load_knowledge(self) -> bool:
        """Carrega a base de conhecimento do arquivo markdown"""
        try:
            if not os.path.exists(settings.KNOWLEDGE_FILE_PATH):
                logger.error(f"Arquivo de conhecimento não encontrado: {settings.KNOWLEDGE_FILE_PATH}")
                return False
            
            with open(settings.KNOWLEDGE_FILE_PATH, 'r', encoding='utf-8') as file:
                self._knowledge_content = file.read()
            
            logger.info("Base de conhecimento carregada com sucesso")
            return True
        except Exception as e:
            logger.error(f"Erro ao carregar base de conhecimento: {e}")
            return False
    
    def get_context(self) -> str:
        """Retorna o contexto da base de conhecimento"""
        if self._knowledge_content is None:
            self.load_knowledge()
        
        return self._knowledge_content or "Base de conhecimento não disponível."
    
    def reload_knowledge(self) -> bool:
        """Recarrega a base de conhecimento"""
        logger.info("Recarregando base de conhecimento...")
        return self.load_knowledge()
    
    def is_loaded(self) -> bool:
        """Verifica se a base de conhecimento está carregada"""
        return self._knowledge_content is not None

# Instância global
knowledge_service = KnowledgeService()
