import os
import logging
import requests
from typing import Optional
from ..config import settings

logger = logging.getLogger(__name__)

class KnowledgeService:
    def __init__(self):
        self._knowledge_content: Optional[str] = None
        self.knowledge_dir = os.path.dirname(settings.KNOWLEDGE_FILE_PATH)
        self.load_knowledge()
    
    def load_knowledge(self) -> bool:
        """Carrega a base de conhecimento de múltiplos arquivos"""
        try:
            knowledge_files = [
                'base_conhecimento.md',
                'faq_knowledge.md'
            ]
            
            combined_knowledge = ""
            files_loaded = 0
            
            for filename in knowledge_files:
                file_path = os.path.join(self.knowledge_dir, filename)
                
                if os.path.exists(file_path):
                    with open(file_path, 'r', encoding='utf-8') as file:
                        content = file.read()
                        combined_knowledge += content + "\n\n"
                        files_loaded += 1
                        logger.info(f"Carregado: {filename}")
                else:
                    logger.warning(f"Arquivo não encontrado: {filename}")
            
            if files_loaded == 0:
                logger.error("Nenhum arquivo de conhecimento encontrado")
                return False
            
            self._knowledge_content = combined_knowledge
            logger.info(f"Base de conhecimento carregada: {files_loaded} arquivos")
            return True
            
        except Exception as e:
            logger.error(f"Erro ao carregar base de conhecimento: {e}")
            return False
    
    def update_faq_knowledge(self) -> bool:
        """Atualiza conhecimento das FAQs do backend"""
        try:
            logger.info("Atualizando conhecimento das FAQs...")
            
            # Executar script de atualização
            import subprocess
            script_path = '/app/scripts/update_knowledge.py'
            
            if os.path.exists(script_path):
                result = subprocess.run(['python3', script_path], 
                                      capture_output=True, text=True, timeout=30)
                
                if result.returncode == 0:
                    logger.info("Script de atualização executado com sucesso")
                    return self.reload_knowledge()
                else:
                    logger.error(f"Erro no script: {result.stderr}")
                    return False
            else:
                logger.error(f"Script não encontrado: {script_path}")
                return False
                
        except Exception as e:
            logger.error(f"Erro ao atualizar FAQs: {e}")
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
