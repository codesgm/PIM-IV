from pydantic import BaseModel
from typing import List, Optional
from datetime import datetime
from enum import Enum

class ChatState(str, Enum):
    AI_ACTIVE = "ai_active"
    AI_ESCALATING = "ai_escalating"
    ESCALATION_PENDING = "escalation_pending"
    HUMAN_ASSIGNED = "human_assigned"
    RESOLVED = "resolved"

class MessageType(str, Enum):
    USER = "user"
    AI = "ai"
    SYSTEM = "system"
    TECHNICIAN = "technician"

class StartChatRequest(BaseModel):
    user_name: str
    user_email: str
    initial_message: str

class SendMessageRequest(BaseModel):
    session_id: str
    message: str

class StartChatResponse(BaseModel):
    session_id: str
    status: str

class MessageResponse(BaseModel):
    id: int
    sender_type: str
    message: str
    created_at: datetime
    confidence: Optional[float] = None

class MessagesListResponse(BaseModel):
    messages: List[MessageResponse]

class ProxySession(BaseModel):
    session_id: str
    chat_id: Optional[int] = None
    user_name: str
    user_email: str
    created_at: datetime
    last_activity: datetime
    last_message_id: int = 0
    state: ChatState = ChatState.AI_ACTIVE
    ai_attempts: int = 0
    last_ai_confidence: float = 0.0
    escalation_reason: Optional[str] = None
    escalated_at: Optional[datetime] = None

class EscalateRequest(BaseModel):
    session_id: str
    reason: str
    user_message: Optional[str] = None

class ChatStateResponse(BaseModel):
    session_id: str
    state: ChatState
    ai_attempts: int
    last_confidence: float
    escalation_reason: Optional[str] = None

class AIResponse(BaseModel):
    answer: str
    confidence: float
    error: Optional[str] = None
