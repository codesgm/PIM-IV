import logging
from datetime import datetime, timedelta
from typing import Optional, Tuple
from ..models import ProxySession, ChatState

logger = logging.getLogger(__name__)

class EscalationEngine:
    ESCALATION_KEYWORDS = [
        "técnico", "tecnico", "humano", "pessoa", "atendente",
        "falar com técnico", "falar com tecnico", 
        "quero técnico", "quero tecnico", "atendimento humano",
        "escalação", "escalacao"
    ]
    
    CONFIDENCE_THRESHOLD = 0.4
    MAX_AI_ATTEMPTS = 5
    TIMEOUT_MINUTES = 5
    
    def should_escalate(self, session: ProxySession, message: str, ai_confidence: float) -> Tuple[bool, str]:
        """
        Determina se o chat deve ser escalado para técnico humano
        Retorna (should_escalate, reason)
        """
        
        # 1. Verificar confidence baixa
        if ai_confidence < self.CONFIDENCE_THRESHOLD:
            logger.info(f"Escalação por baixa confidence: {ai_confidence}")
            return True, f"IA com baixa confiança ({ai_confidence:.2f})"
        
        # 2. Verificar keywords de escalação
        message_lower = message.lower()
        for keyword in self.ESCALATION_KEYWORDS:
            if keyword in message_lower:
                logger.info(f"Escalação por keyword: {keyword}")
                return True, f"Usuário solicitou: '{keyword}'"
        
        # 3. Verificar muitas tentativas
        if session.ai_attempts >= self.MAX_AI_ATTEMPTS:
            logger.info(f"Escalação por tentativas: {session.ai_attempts}")
            return True, f"Muitas tentativas da IA ({session.ai_attempts})"
        
        # 4. Verificar timeout
        if self._is_timeout(session):
            logger.info("Escalação por timeout")
            return True, "Timeout de resposta da IA"
        
        return False, ""
    
    def detect_escalation_intent(self, message: str) -> bool:
        """Detecta se a mensagem contém intenção de escalação"""
        message_lower = message.lower()
        return any(keyword in message_lower for keyword in self.ESCALATION_KEYWORDS)
    
    def get_escalation_reason(self, session: ProxySession, message: str, ai_confidence: float) -> str:
        """Retorna o motivo da escalação"""
        should_escalate, reason = self.should_escalate(session, message, ai_confidence)
        return reason if should_escalate else ""
    
    def _is_timeout(self, session: ProxySession) -> bool:
        """Verifica se houve timeout na sessão"""
        if session.state != ChatState.AI_ACTIVE:
            return False
        
        timeout_threshold = datetime.now() - timedelta(minutes=self.TIMEOUT_MINUTES)
        return session.created_at < timeout_threshold and session.ai_attempts > 0

# Instância global
escalation_engine = EscalationEngine()
