from pydantic import BaseModel
from typing import List, Optional
from datetime import datetime

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

class MessagesListResponse(BaseModel):
    messages: List[MessageResponse]

class ProxySession(BaseModel):
    session_id: str
    chat_id: int
    user_name: str
    created_at: datetime
    last_activity: datetime
    last_message_id: int = 0
