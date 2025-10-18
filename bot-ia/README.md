# MidTalk AI Bot

API de IA que utiliza Google Gemini para responder perguntas baseadas na base de conhecimento do MidTalk.

## Funcionalidades

- **Perguntas e Respostas**: Responde perguntas usando IA contextualizada
- **Base de Conhecimento**: Carrega informações de arquivo markdown
- **Health Check**: Monitora status do serviço
- **Reload Dinâmico**: Atualiza base de conhecimento sem restart

## Endpoints

### POST /api/ai/ask
Faz uma pergunta para a IA.

**Request:**
```json
{
  "question": "Como criar um chamado?"
}
```

**Response:**
```json
{
  "answer": "Para criar um chamado, acesse o site FAQ...",
  "confidence": 0.85,
  "error": null
}
```

### GET /health
Verifica status do serviço.

**Response:**
```json
{
  "status": "healthy",
  "gemini_available": true,
  "knowledge_loaded": true
}
```

### GET /api/ai/knowledge/reload
Recarrega a base de conhecimento.

**Response:**
```json
{
  "success": true,
  "message": "Base de conhecimento recarregada com sucesso"
}
```

## Configuração

Copie `.env.example` para `.env` e configure:

```env
GEMINI_API_KEY=sua_api_key_aqui
KNOWLEDGE_FILE_PATH=/app/knowledge/base_conhecimento.md
API_PORT=8001
LOG_LEVEL=INFO
MAX_TOKENS=1000
TEMPERATURE=0.7
```

## Docker

```bash
# Build
docker build -t bot-ia .

# Run
docker run -p 8001:8001 --env-file .env bot-ia
```

## Desenvolvimento

```bash
# Instalar dependências
pip install -r requirements.txt

# Executar
uvicorn app.main:app --reload --port 8001
```
