import os
from dotenv import load_dotenv

load_dotenv()

BACKEND_API_URL = os.getenv("BACKEND_API_URL", "http://api:5000")
PROXY_PORT = int(os.getenv("PROXY_PORT", "9000"))
SESSION_TIMEOUT = int(os.getenv("SESSION_TIMEOUT", "3600"))
