# Checklist - Implementação Bot IA com Gemini

## 📋 Fase 1: Setup Inicial

### 1.1 Estrutura de Diretórios
- [ ] Criar diretório `app/`
- [ ] Criar diretório `app/services/`
- [ ] Criar diretório `knowledge/`
- [ ] Criar arquivos `__init__.py` necessários

### 1.2 Configuração Base
- [ ] Criar `requirements.txt` com dependências
- [ ] Criar `Dockerfile` para containerização
- [ ] Criar `.env.example` com variáveis necessárias
- [ ] Criar `app/config.py` para configurações
- [ ] Criar `README.md` básico

### 1.3 Docker Setup
- [ ] Configurar Dockerfile com Python 3.11
- [ ] Definir WORKDIR e COPY adequados
- [ ] Configurar EXPOSE para porta 8001
- [ ] Testar build do container

## 📚 Fase 2: Base de Conhecimento

### 2.1 Arquivo de Conhecimento
- [ ] Criar `knowledge/base_conhecimento.md`
- [ ] Adicionar informações sobre MidTalk
- [ ] Incluir FAQ comum do sistema
- [ ] Documentar procedimentos de suporte
- [ ] Adicionar políticas e diretrizes

### 2.2 Serviço de Conhecimento
- [ ] Criar `app/services/knowledge_service.py`
- [ ] Implementar função `load_knowledge()`
- [ ] Implementar função `get_context()`
- [ ] Adicionar cache em memória
- [ ] Implementar reload dinâmico

### 2.3 Testes Base Conhecimento
- [ ] Testar carregamento do arquivo
- [ ] Validar parsing do markdown
- [ ] Verificar cache funcionando

## 🤖 Fase 3: Integração Gemini

### 3.1 Configuração Gemini
- [ ] Obter API Key do Google Gemini
- [ ] Instalar `google-generativeai`
- [ ] Configurar cliente Gemini
- [ ] Testar conexão básica

### 3.2 Serviço Gemini
- [ ] Criar `app/services/gemini_service.py`
- [ ] Implementar `GeminiService` class
- [ ] Criar método `ask_question()`
- [ ] Implementar prompt engineering
- [ ] Adicionar tratamento de erros

### 3.3 Prompt Engineering
- [ ] Definir sistema de instruções
- [ ] Criar template de prompt
- [ ] Configurar parâmetros (temperature, max_tokens)
- [ ] Testar diferentes tipos de pergunta

## 🚀 Fase 4: API FastAPI

### 4.1 Models Pydantic
- [ ] Criar `app/models.py`
- [ ] Implementar `QuestionRequest`
- [ ] Implementar `AnswerResponse`
- [ ] Implementar `ErrorResponse`
- [ ] Adicionar validações necessárias

### 4.2 Aplicação Principal
- [ ] Criar `app/main.py`
- [ ] Configurar FastAPI app
- [ ] Adicionar middleware CORS
- [ ] Implementar health check
- [ ] Configurar logging

### 4.3 Endpoints
- [ ] Implementar `POST /api/ai/ask`
- [ ] Implementar `GET /health`
- [ ] Implementar `GET /api/ai/knowledge/reload`
- [ ] Adicionar documentação automática
- [ ] Testar todos endpoints

### 4.4 Testes API
- [ ] Testar endpoint de pergunta
- [ ] Validar respostas da IA
- [ ] Testar health check
- [ ] Verificar reload de conhecimento

## 🔗 Fase 5: Integração Docker

### 5.1 Docker Compose
- [ ] Atualizar `docker-compose.yml` principal
- [ ] Adicionar serviço `bot-ia`
- [ ] Configurar variáveis de ambiente
- [ ] Definir rede e dependências
- [ ] Configurar porta 8001

### 5.2 Variáveis de Ambiente
- [ ] Atualizar `.env` principal
- [ ] Adicionar `GEMINI_API_KEY`
- [ ] Configurar `BOT_IA_URL`
- [ ] Definir configurações do bot

### 5.3 Testes Container
- [ ] Build do container bot-ia
- [ ] Testar inicialização
- [ ] Verificar conectividade na rede
- [ ] Testar API via container

## 🔌 Fase 6: Integração Sistema

### 6.1 Proxy Service
- [ ] Atualizar `proxy/app/models.py`
- [ ] Adicionar `AskAIRequest`
- [ ] Criar método `ask_ai()` no proxy
- [ ] Implementar detecção de perguntas para IA
- [ ] Integrar resposta no fluxo de chat

### 6.2 Backend Integration
- [ ] Atualizar `ChatMessage` model
- [ ] Adicionar tipo "AI" para mensagens
- [ ] Implementar logging de IA
- [ ] Atualizar DTOs se necessário

### 6.3 Frontend Updates
- [ ] Identificar mensagens de IA no chat
- [ ] Adicionar ícone/badge para IA
- [ ] Implementar styling diferenciado
- [ ] Testar exibição no frontend

## 🧪 Fase 7: Testes e Validação

### 7.1 Testes Unitários
- [ ] Testar knowledge_service
- [ ] Testar gemini_service
- [ ] Testar endpoints da API
- [ ] Validar models Pydantic

### 7.2 Testes Integração
- [ ] Testar fluxo FAQ → Proxy → Bot IA
- [ ] Validar resposta IA → Backend → Frontend
- [ ] Testar reload de conhecimento
- [ ] Verificar tratamento de erros

### 7.3 Testes Performance
- [ ] Medir tempo de resposta
- [ ] Testar múltiplas requisições
- [ ] Verificar uso de memória
- [ ] Validar rate limiting

## 📊 Fase 8: Monitoramento

### 8.1 Logging
- [ ] Configurar logs estruturados
- [ ] Implementar níveis de log
- [ ] Adicionar métricas de uso
- [ ] Configurar rotação de logs

### 8.2 Health Checks
- [ ] Implementar health check completo
- [ ] Verificar status Gemini API
- [ ] Monitorar base de conhecimento
- [ ] Alertas de falha

### 8.3 Métricas
- [ ] Contador de perguntas
- [ ] Tempo médio de resposta
- [ ] Taxa de sucesso/erro
- [ ] Uso de tokens Gemini

## 🚀 Fase 9: Deploy e Documentação

### 9.1 Deploy
- [ ] Rebuild todos containers
- [ ] Testar sistema completo
- [ ] Validar integração end-to-end
- [ ] Verificar logs de todos serviços

### 9.2 Documentação
- [ ] Atualizar README principal
- [ ] Documentar API endpoints
- [ ] Criar guia de uso da IA
- [ ] Documentar troubleshooting

### 9.3 Configuração Produção
- [ ] Configurar rate limiting
- [ ] Implementar cache Redis (opcional)
- [ ] Configurar backup da base conhecimento
- [ ] Definir monitoramento contínuo

## ✅ Checklist Final

### Validação Completa
- [ ] FAQ widget detecta perguntas complexas
- [ ] Proxy encaminha para bot IA
- [ ] Bot IA responde baseado no conhecimento
- [ ] Resposta aparece no chat do usuário
- [ ] Técnico vê interação com IA
- [ ] Logs registram todas interações
- [ ] Sistema funciona em todos dispositivos da rede

### Testes de Aceitação
- [ ] Usuário faz pergunta no FAQ
- [ ] IA responde corretamente
- [ ] Resposta é contextualizada
- [ ] Tempo de resposta < 5 segundos
- [ ] Fallback funciona se IA falhar
- [ ] Base conhecimento pode ser atualizada

---

## 📝 Notas de Implementação

### Ordem de Execução
1. Executar fases 1-4 para ter API funcionando
2. Testar API isoladamente
3. Executar fases 5-6 para integração
4. Executar fases 7-9 para finalização

### Pontos Críticos
- **API Key Gemini**: Obter antes de começar fase 3
- **Base Conhecimento**: Criar conteúdo relevante na fase 2
- **Docker Network**: Garantir conectividade entre serviços
- **Error Handling**: Implementar fallbacks em todas fases

### Comandos Úteis
```bash
# Build e test do bot-ia
cd bot-ia && docker build -t bot-ia .

# Rebuild sistema completo
docker compose down && docker compose up --build -d

# Logs do bot-ia
docker logs pim-bot-ia -f

# Test API
curl -X POST http://192.168.0.152:8001/api/ai/ask \
  -H "Content-Type: application/json" \
  -d '{"question": "Como criar um chamado?"}'
```
