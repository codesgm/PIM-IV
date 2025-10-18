# MidTalk Proxy

Proxy em Python/FastAPI que faz ponte entre FAQ e sistema de chats do backend.

## 🎯 Funcionalidades

- **Gerenciamento de sessões** em memória
- **Ponte FAQ ↔ Backend** para chats
- **API REST** para comunicação
- **Limpeza automática** de sessões expiradas

## 🚀 Como Executar

### Via Docker (Recomendado)
```bash
# Na raiz do projeto
./start.sh
```

### Localmente
```bash
cd proxy
pip install -r requirements.txt
uvicorn app.main:app --host 0.0.0.0 --port 9000
```

## 🔧 Variáveis de Ambiente

```env
BACKEND_API_URL=http://api:5000
PROXY_PORT=9000
SESSION_TIMEOUT=3600
```

## 📡 Endpoints

### Iniciar Chat
```bash
POST /api/proxy/start-chat
{
  "user_name": "João",
  "initial_message": "Preciso de ajuda"
}
```

### Enviar Mensagem
```bash
POST /api/proxy/send-message
{
  "session_id": "proxy_abc123",
  "message": "Minha mensagem"
}
```

### Buscar Mensagens (Polling)
```bash
GET /api/proxy/messages/{session_id}
```

### Health Check
```bash
GET /health
GET /api/proxy/sessions
```

## 🧪 Testes

```bash
# Iniciar chat
curl -X POST http://localhost:9000/api/proxy/start-chat \
  -H "Content-Type: application/json" \
  -d '{"user_name":"Teste","initial_message":"Olá"}'

# Enviar mensagem
curl -X POST http://localhost:9000/api/proxy/send-message \
  -H "Content-Type: application/json" \
  -d '{"session_id":"proxy_123","message":"Preciso de ajuda"}'

# Buscar mensagens
curl http://localhost:9000/api/proxy/messages/proxy_123
```

## 📊 Arquitetura

```
FAQ Site → Proxy (Port 9000) → Backend API (Port 5000)
```

- **Sessões**: Armazenadas em memória com timeout
- **Mapeamento**: session_id → chat_id do backend
- **Polling**: FAQ busca mensagens periodicamente
- **Limpeza**: Sessões expiradas removidas automaticamente
