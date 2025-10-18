import os
from dotenv import load_dotenv

load_dotenv()

class Settings:
    GEMINI_API_KEY: str = os.getenv("GEMINI_API_KEY", "")
    KNOWLEDGE_FILE_PATH: str = os.getenv("KNOWLEDGE_FILE_PATH", "/app/knowledge/base_conhecimento.md")
    API_PORT: int = int(os.getenv("API_PORT", "8001"))
    LOG_LEVEL: str = os.getenv("LOG_LEVEL", "INFO")
    MAX_TOKENS: int = int(os.getenv("MAX_TOKENS", "1000"))
    TEMPERATURE: float = float(os.getenv("TEMPERATURE", "0.7"))

settings = Settings()
