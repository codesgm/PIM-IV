# Proxy - Ponte FAQ ↔ Backend - Checklist

## 🎯 Objetivo
Implementar proxy em Python/FastAPI que faz ponte entre FAQ e sistema de chats do backend.

## 📋 Checklist de Implementação

### 1. Estrutura do Projeto

#### 1.1 Criar Estrutura de Pastas
- [x] **Criar pasta `app/`** para código Python
- [x] **Criar `requirements.txt`** com dependências
- [x] **Criar `Dockerfile`** para containerização
- [x] **Criar `.env.example`** com variáveis de ambiente

#### 1.2 Arquivos Necessários
```
proxy/
├── app/
│   ├── main.py              # FastAPI app principal ✅
│   ├── proxy_service.py     # Lógica da ponte ✅
│   ├── models.py            # Modelos Pydantic ✅
│   └── config.py            # Configurações ✅
├── requirements.txt         # Dependências Python ✅
├── Dockerfile              # Container ✅
├── .env.example            # Exemplo de env vars ✅
└── checklist.md            # Este arquivo ✅
```

### 2. Dependências e Configuração

#### 2.1 Requirements.txt
- [x] **FastAPI** para API REST
- [x] **Uvicorn** para servidor ASGI
- [x] **httpx** para chamadas HTTP ao backend
- [x] **pydantic** para validação de dados
- [x] **python-dotenv** para variáveis de ambiente

#### 2.2 Variáveis de Ambiente
- [x] **BACKEND_API_URL** (http://api:5000)
- [x] **PROXY_PORT** (9000)
- [x] **SESSION_TIMEOUT** (3600 segundos)

### 3. Modelos de Dados

#### 3.1 Request Models (Pydantic)
- [x] **StartChatRequest** - dados para iniciar chat
- [x] **SendMessageRequest** - dados para enviar mensagem
- [x] **ProxySession** - estrutura da sessão em memória

#### 3.2 Response Models
- [x] **StartChatResponse** - resposta ao iniciar chat
- [x] **MessageResponse** - resposta de mensagem
- [x] **MessagesListResponse** - lista de mensagens

### 4. Gerenciamento de Sessões

#### 4.1 Armazenamento em Memória
- [x] **Dicionário global** para sessões ativas
- [x] **Estrutura de sessão** com chat_id, user_name, timestamps
- [x] **Geração de session_id** único
- [x] **Limpeza automática** de sessões expiradas

#### 4.2 Mapeamento Session ↔ Chat
- [x] **session_id** → **chat_id** do backend
- [x] **Controle de estado** da conversa
- [x] **Último message_id** para polling eficiente

### 5. Integração com Backend

#### 5.1 HTTP Client para Backend
- [x] **Cliente httpx** configurado
- [x] **Base URL** do backend (http://api:5000)
- [x] **Timeout** configurado
- [x] **Tratamento de erros** de conexão

#### 5.2 Endpoints do Backend Consumidos
- [x] **POST /api/chats** - criar novo chat
- [x] **POST /api/chats/{id}/messages** - enviar mensagem
- [x] **GET /api/chats/{id}/messages** - buscar mensagens
- [ ] **PUT /api/chats/{id}/resolve** - resolver chat (futuro)

### 6. API Endpoints do Proxy

#### 6.1 Iniciar Chat
- [x] **POST /api/proxy/start-chat**
- [x] **Receber**: user_name, initial_message
- [x] **Criar chat** no backend via API
- [x] **Gerar session_id** único
- [x] **Armazenar mapeamento** session → chat_id
- [x] **Retornar**: session_id, status

#### 6.2 Enviar Mensagem
- [x] **POST /api/proxy/send-message**
- [x] **Receber**: session_id, message
- [x] **Validar sessão** existe
- [x] **Enviar mensagem** para backend
- [x] **Retornar**: confirmação de envio

#### 6.3 Buscar Mensagens (Polling)
- [x] **GET /api/proxy/messages/{session_id}**
- [x] **Validar sessão** existe
- [x] **Buscar mensagens** do backend
- [x] **Filtrar mensagens novas** (após last_message_id)
- [x] **Atualizar last_message_id** da sessão
- [x] **Retornar**: lista de mensagens

#### 6.4 Health Check
- [x] **GET /health** - verificar se proxy está funcionando
- [x] **GET /api/proxy/sessions** - debug (quantas sessões ativas)

### 7. Lógica do Proxy Service

#### 7.1 Gerenciamento de Sessões
- [x] **create_session()** - criar nova sessão
- [x] **get_session()** - buscar sessão por ID
- [x] **update_session()** - atualizar última atividade
- [x] **cleanup_expired_sessions()** - limpar sessões antigas

#### 7.2 Comunicação com Backend
- [x] **create_backend_chat()** - criar chat via API
- [x] **send_message_to_backend()** - enviar mensagem
- [x] **get_messages_from_backend()** - buscar mensagens
- [x] **handle_backend_errors()** - tratar erros da API

### 8. Docker e Deploy

#### 8.1 Dockerfile
- [x] **Base image** Python 3.11-slim
- [x] **Instalar dependências** do requirements.txt
- [x] **Copiar código** da aplicação
- [x] **Expor porta** 9000
- [x] **CMD** para iniciar uvicorn

#### 8.2 Docker Compose Integration
- [x] **Adicionar serviço proxy** no docker-compose.yml
- [x] **Porta 9000:9000** mapeada
- [x] **Dependência** do serviço api
- [x] **Variáveis de ambiente** configuradas
- [x] **Rede** pim-network

### 9. Tratamento de Erros

#### 9.1 Validações
- [x] **Sessão não encontrada** (404)
- [x] **Dados inválidos** (400)
- [x] **Backend indisponível** (503)
- [x] **Timeout** de requisições

#### 9.2 Logs
- [x] **Log de sessões** criadas/expiradas
- [x] **Log de mensagens** enviadas/recebidas
- [x] **Log de erros** de comunicação
- [x] **Estrutura JSON** para logs

### 10. Testes e Validação

#### 10.1 Testes Manuais
- [x] **Iniciar chat** via curl/Postman
- [x] **Enviar mensagem** e verificar no backend
- [x] **Buscar mensagens** e verificar retorno
- [x] **Testar sessões expiradas**
- [x] **Testar erros** (backend offline, dados inválidos)

#### 10.2 Comandos de Teste
```bash
# Iniciar chat ✅
curl -X POST http://localhost:9000/api/proxy/start-chat \
  -H "Content-Type: application/json" \
  -d '{"user_name":"Teste","initial_message":"Olá"}'

# Enviar mensagem ✅
curl -X POST http://localhost:9000/api/proxy/send-message \
  -H "Content-Type: application/json" \
  -d '{"session_id":"proxy_123","message":"Preciso de ajuda"}'

# Buscar mensagens ✅
curl http://localhost:9000/api/proxy/messages/proxy_123
```

#### 10.3 Validar Integração
- [x] **Proxy cria chat** no backend corretamente
- [x] **Mensagens fluem** FAQ → Backend
- [x] **Respostas fluem** Backend → FAQ
- [x] **Sessões são limpas** automaticamente
- [x] **Erros são tratados** adequadamente

### 11. Performance e Otimização

#### 11.1 Otimizações
- [x] **Cache de sessões** eficiente
- [x] **Polling otimizado** (apenas mensagens novas)
- [x] **Timeout adequado** para requests
- [x] **Limpeza periódica** de sessões

#### 11.2 Monitoramento
- [x] **Endpoint /health** funcionando
- [x] **Logs estruturados** para debug
- [x] **Métricas básicas** (sessões ativas, requests/min)

### 12. Documentação

#### 12.1 API Documentation
- [x] **FastAPI Swagger** automático
- [x] **Exemplos de request/response**
- [x] **Códigos de erro** documentados

#### 12.2 README
- [x] **Como executar** o proxy
- [x] **Variáveis de ambiente** necessárias
- [x] **Endpoints disponíveis**
- [x] **Como testar** manualmente

---

## ✅ Critérios de Conclusão

### Funcionalidades Mínimas
- [x] **Proxy inicia** e responde na porta 9000
- [x] **FAQ pode criar chat** via proxy
- [x] **Mensagens são enviadas** para backend
- [x] **Mensagens são recebidas** do backend
- [x] **Sessões são gerenciadas** corretamente

### Integração
- [x] **Docker container** funciona
- [x] **Comunicação com backend** estável
- [x] **Tratamento de erros** adequado
- [x] **Logs para debug** disponíveis

### Qualidade
- [x] **Código limpo** e organizado
- [x] **Documentação** clara
- [x] **Testes manuais** passando
- [x] **Pronto para FAQ** consumir

**Status**: ✅ **CONCLUÍDO**
**Porta**: 9000
**URL**: http://localhost:9000

## 🎉 Resultado Final

O proxy está **100% funcional** e pronto para uso:

- ✅ **Criação de chats** via FAQ funcionando
- ✅ **Envio de mensagens** funcionando
- ✅ **Polling de mensagens** funcionando
- ✅ **Gerenciamento de sessões** funcionando
- ✅ **Integração com backend** funcionando
- ✅ **Docker container** funcionando
- ✅ **Logs e monitoramento** funcionando

**Próximo passo**: Integrar o proxy com o site FAQ público.
