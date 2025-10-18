# Planejamento - Bot IA com Gemini

## 🎯 Objetivo
Criar uma API que utiliza o Google Gemini para responder perguntas baseadas em um arquivo markdown de conhecimento, integrando com o sistema de chat existente.

## 📋 Requisitos Funcionais

### 1. API de IA
- **Endpoint**: `/api/ai/ask`
- **Método**: POST
- **Input**: `{"question": "pergunta do usuário"}`
- **Output**: `{"answer": "resposta da IA", "confidence": 0.95}`

### 2. Base de Conhecimento
- Arquivo markdown com informações da empresa/produto
- Carregamento automático na inicialização
- Possibilidade de atualização sem restart

### 3. Integração Gemini
- Configuração de API Key
- Prompt engineering para respostas contextualizadas
- Tratamento de erros e fallbacks

## 🏗️ Arquitetura Técnica

### Tecnologias
- **Framework**: FastAPI (Python)
- **IA**: Google Gemini API
- **Containerização**: Docker
- **Configuração**: Environment variables

### Estrutura de Diretórios
```
bot-ia/
├── app/
│   ├── __init__.py
│   ├── main.py              # FastAPI app
│   ├── models.py            # Pydantic models
│   ├── services/
│   │   ├── __init__.py
│   │   ├── gemini_service.py    # Integração Gemini
│   │   └── knowledge_service.py # Gerenciamento do markdown
│   └── config.py            # Configurações
├── knowledge/
│   └── base_conhecimento.md # Base de conhecimento
├── requirements.txt
├── Dockerfile
├── .env.example
└── README.md
```

## 📝 Passos de Implementação

### Fase 1: Setup Inicial
1. **Criar estrutura de diretórios**
2. **Configurar requirements.txt** com dependências:
   - fastapi
   - uvicorn
   - google-generativeai
   - python-dotenv
   - pydantic
3. **Criar Dockerfile** para containerização
4. **Setup de configurações** (.env, config.py)

### Fase 2: Base de Conhecimento
1. **Criar base_conhecimento.md** com:
   - Informações sobre o MidTalk
   - FAQ comum
   - Procedimentos de suporte
   - Políticas da empresa
2. **Implementar knowledge_service.py**:
   - Carregamento do markdown
   - Parsing e estruturação
   - Cache em memória

### Fase 3: Integração Gemini
1. **Configurar Google Gemini API**:
   - Obter API Key
   - Configurar cliente
2. **Implementar gemini_service.py**:
   - Conexão com Gemini
   - Prompt engineering
   - Tratamento de respostas
3. **Criar prompts contextualizados**:
   - Sistema de instruções
   - Contexto da base de conhecimento
   - Formatação de respostas

### Fase 4: API FastAPI
1. **Implementar main.py**:
   - Configuração FastAPI
   - Middleware CORS
   - Health check
2. **Criar models.py**:
   - QuestionRequest
   - AnswerResponse
   - ErrorResponse
3. **Implementar endpoints**:
   - POST /api/ai/ask
   - GET /health
   - GET /api/ai/knowledge/reload

### Fase 5: Integração com Sistema Existente
1. **Atualizar docker-compose.yml**:
   - Adicionar serviço bot-ia
   - Configurar rede
   - Variáveis de ambiente
2. **Modificar proxy service**:
   - Detectar perguntas que precisam de IA
   - Chamar API do bot
   - Integrar resposta no chat
3. **Atualizar backend**:
   - Novo tipo de mensagem: "AI"
   - Logging de interações com IA

### Fase 6: Melhorias e Otimizações
1. **Cache de respostas**:
   - Redis para cache
   - TTL configurável
2. **Métricas e monitoramento**:
   - Logs estruturados
   - Métricas de uso
3. **Rate limiting**:
   - Controle de requisições
   - Proteção contra spam

## 🔧 Configurações Necessárias

### Variáveis de Ambiente
```env
GEMINI_API_KEY=sua_api_key_aqui
KNOWLEDGE_FILE_PATH=/app/knowledge/base_conhecimento.md
API_PORT=8001
LOG_LEVEL=INFO
CACHE_TTL=3600
MAX_TOKENS=1000
TEMPERATURE=0.7
```

### Docker Compose Integration
```yaml
bot-ia:
  build:
    context: ./bot-ia
  environment:
    - GEMINI_API_KEY=${GEMINI_API_KEY}
  ports:
    - "${HOST_IP}:8001:8001"
  networks:
    - pim-network
```

## 🎨 Prompt Engineering

### Sistema de Instruções
```
Você é um assistente de suporte técnico especializado no sistema MidTalk.
Responda sempre em português brasileiro, de forma clara e profissional.
Use apenas as informações fornecidas na base de conhecimento.
Se não souber a resposta, diga que não tem essa informação.
```

### Estrutura do Prompt
1. **Contexto**: Base de conhecimento
2. **Instrução**: Como responder
3. **Pergunta**: Questão do usuário
4. **Formato**: Estrutura da resposta

## 🧪 Testes e Validação

### Testes Unitários
- Carregamento da base de conhecimento
- Integração com Gemini
- Validação de respostas

### Testes de Integração
- API endpoints
- Integração com chat
- Fluxo completo

### Testes de Performance
- Tempo de resposta
- Uso de memória
- Limite de requisições

## 📊 Métricas de Sucesso

### KPIs Técnicos
- Tempo de resposta < 3 segundos
- Disponibilidade > 99%
- Taxa de erro < 1%

### KPIs de Negócio
- Redução de tickets manuais
- Satisfação do usuário
- Precisão das respostas

## 🚀 Cronograma Estimado

- **Fase 1-2**: 1 dia (Setup + Base conhecimento)
- **Fase 3**: 1 dia (Integração Gemini)
- **Fase 4**: 1 dia (API FastAPI)
- **Fase 5**: 1 dia (Integração sistema)
- **Fase 6**: 1 dia (Melhorias)

**Total**: 5 dias de desenvolvimento

## 🔒 Considerações de Segurança

### API Key Management
- Nunca commitar API keys
- Usar variáveis de ambiente
- Rotação periódica

### Rate Limiting
- Limite por IP
- Limite por usuário
- Proteção DDoS

### Validação de Input
- Sanitização de entrada
- Limite de caracteres
- Validação de formato

## 📈 Próximos Passos

1. **Obter API Key do Google Gemini**
2. **Criar base de conhecimento inicial**
3. **Implementar MVP da API**
4. **Testar integração básica**
5. **Expandir funcionalidades**

---

*Este planejamento serve como guia para implementação da API de IA com Gemini, integrando ao sistema MidTalk existente.*
