# Checklist - Implementação IA-First Chat Flow

## 📋 Sprint 1: Proxy Service (2 dias)

### 1.1 Novos Models e Enums
- [ ] Criar `ChatState` enum (AI_ACTIVE, AI_ESCALATING, HUMAN_ASSIGNED, RESOLVED)
- [ ] Criar `MessageType` enum (USER, AI, SYSTEM, TECHNICIAN)
- [ ] Atualizar `ProxySession` model com novos campos:
  - [ ] `state: ChatState = ChatState.AI_ACTIVE`
  - [ ] `ai_attempts: int = 0`
  - [ ] `last_ai_confidence: float = 0.0`
  - [ ] `escalation_reason: Optional[str] = None`
  - [ ] `escalated_at: Optional[datetime] = None`

### 1.2 Escalation Engine
- [ ] Criar `EscalationEngine` class
- [ ] Definir `ESCALATION_KEYWORDS` list
- [ ] Implementar `should_escalate()` method:
  - [ ] Verificar confidence < 0.5
  - [ ] Verificar keywords de escalação
  - [ ] Verificar tentativas >= 3
  - [ ] Verificar timeout (5 min)
- [ ] Implementar `detect_escalation_intent()` method
- [ ] Implementar `get_escalation_reason()` method

### 1.3 Modified Message Flow
- [ ] Atualizar `start_chat()` para iniciar com IA
- [ ] Criar `handle_user_message()` method:
  - [ ] Verificar estado do chat
  - [ ] Decidir: IA ou Técnico
  - [ ] Aplicar lógica de escalação
- [ ] Criar `send_to_ai()` method:
  - [ ] Chamar bot-ia API
  - [ ] Processar resposta
  - [ ] Verificar confidence
  - [ ] Incrementar ai_attempts
- [ ] Criar `escalate_to_human()` method:
  - [ ] Mudar estado para AI_ESCALATING
  - [ ] Criar chat no backend
  - [ ] Atribuir técnico
  - [ ] Mudar estado para HUMAN_ASSIGNED
  - [ ] Notificar usuário

### 1.4 New Endpoints
- [ ] Atualizar `POST /api/proxy/start-chat`:
  - [ ] Não criar chat no backend imediatamente
  - [ ] Manter sessão em estado AI_ACTIVE
- [ ] Atualizar `POST /api/proxy/send-message`:
  - [ ] Implementar roteamento IA vs Técnico
  - [ ] Aplicar escalation logic
- [ ] Criar `POST /api/proxy/escalate`:
  - [ ] Escalação manual pelo usuário
- [ ] Criar `GET /api/proxy/chat-state/{session_id}`:
  - [ ] Retornar estado atual do chat

### 1.5 AI Integration
- [ ] Criar `AIService` class no proxy
- [ ] Implementar `ask_ai()` method:
  - [ ] Chamar bot-ia API
  - [ ] Tratar timeouts
  - [ ] Tratar erros
  - [ ] Retornar resposta + confidence
- [ ] Implementar retry logic para IA
- [ ] Implementar fallback para IA indisponível

### 1.6 Testes Proxy
- [ ] Testar criação de sessão IA
- [ ] Testar envio de mensagem para IA
- [ ] Testar escalação automática (baixa confidence)
- [ ] Testar escalação manual (keywords)
- [ ] Testar escalação por timeout
- [ ] Testar fallback IA indisponível

## 📋 Sprint 2: Backend (2 dias)

### 2.1 Database Migration
- [ ] Criar migration `AddChatStateAndAI`:
  - [ ] Adicionar `State` int NOT NULL DEFAULT 1
  - [ ] Adicionar `AIAttempts` int NOT NULL DEFAULT 0
  - [ ] Adicionar `EscalationReason` nvarchar(500) NULL
  - [ ] Adicionar `EscalatedAt` datetime2 NULL
- [ ] Atualizar `ChatMessage` table:
  - [ ] Adicionar `MessageType` int NOT NULL DEFAULT 1
  - [ ] Adicionar `AIConfidence` float NULL
- [ ] Executar migration

### 2.2 Models Update
- [ ] Atualizar `Chat.cs`:
  - [ ] Adicionar `ChatState State { get; set; } = ChatState.AI_ACTIVE`
  - [ ] Adicionar `int AIAttempts { get; set; } = 0`
  - [ ] Adicionar `string? EscalationReason { get; set; }`
  - [ ] Adicionar `DateTime? EscalatedAt { get; set; }`
- [ ] Atualizar `ChatMessage.cs`:
  - [ ] Adicionar `MessageType MessageType { get; set; }`
  - [ ] Adicionar `float? AIConfidence { get; set; }`
- [ ] Criar enums:
  - [ ] `ChatState` enum
  - [ ] `MessageType` enum

### 2.3 DTOs Update
- [ ] Atualizar `ChatResponseDto`:
  - [ ] Adicionar `string State`
  - [ ] Adicionar `int AIAttempts`
  - [ ] Adicionar `string? EscalationReason`
  - [ ] Adicionar `DateTime? EscalatedAt`
- [ ] Atualizar `ChatMessageResponseDto`:
  - [ ] Adicionar `string MessageType`
  - [ ] Adicionar `float? AIConfidence`
- [ ] Criar `EscalateRequestDto`:
  - [ ] `string Reason`
  - [ ] `string? UserMessage`
- [ ] Criar `AIResponseRequestDto`:
  - [ ] `string Message`
  - [ ] `float Confidence`

### 2.4 New Endpoints
- [ ] Implementar `POST /api/chats/{id}/escalate`:
  - [ ] Validar chat existe
  - [ ] Atualizar estado para HUMAN_ASSIGNED
  - [ ] Atribuir técnico
  - [ ] Registrar escalation_reason
  - [ ] Retornar chat atualizado
- [ ] Implementar `POST /api/chats/{id}/ai-response`:
  - [ ] Adicionar mensagem tipo AI
  - [ ] Salvar confidence score
  - [ ] Incrementar ai_attempts
  - [ ] Retornar mensagem criada
- [ ] Implementar `GET /api/chats/ai-queue`:
  - [ ] Listar chats com estado AI_ACTIVE
  - [ ] Filtros e paginação
- [ ] Atualizar `GET /api/chats`:
  - [ ] Incluir novos campos na resposta
  - [ ] Filtrar por estado se necessário

### 2.5 Service Updates
- [ ] Atualizar `ChatAssignmentService`:
  - [ ] Modificar para não atribuir automaticamente
  - [ ] Criar método `AssignTechnicianOnEscalation()`
- [ ] Criar `ChatStateService`:
  - [ ] `UpdateChatState()`
  - [ ] `EscalateChat()`
  - [ ] `GetChatsByState()`

### 2.6 Testes Backend
- [ ] Testar criação de chat com estado AI_ACTIVE
- [ ] Testar adição de mensagem AI
- [ ] Testar escalação de chat
- [ ] Testar atribuição de técnico na escalação
- [ ] Testar queries por estado
- [ ] Testar migration de dados existentes

## 📋 Sprint 3: Frontend + FAQ Widget (2 dias)

### 3.1 Frontend - Models Update
- [ ] Atualizar `ChatDetailViewModel`:
  - [ ] Adicionar propriedades de estado IA
- [ ] Criar `ChatStateViewModel`
- [ ] Atualizar `MessageViewModel`:
  - [ ] Adicionar `MessageType`
  - [ ] Adicionar `AIConfidence`

### 3.2 Frontend - Chat Interface
- [ ] Atualizar `Views/Chat/Details.cshtml`:
  - [ ] Adicionar indicador de estado do chat
  - [ ] Mostrar badge IA/Técnico
  - [ ] Exibir confidence score para mensagens IA
  - [ ] Adicionar botão "Assumir Chat" para técnicos
- [ ] Criar partial view `_ChatStateIndicator.cshtml`
- [ ] Criar partial view `_AIMessage.cshtml`
- [ ] Atualizar CSS para estilos IA vs Técnico

### 3.3 Frontend - Dashboard Updates
- [ ] Atualizar `Views/Chat/Index.cshtml`:
  - [ ] Separar chats IA vs Técnico
  - [ ] Mostrar contadores por estado
  - [ ] Filtros por estado
- [ ] Criar `Views/Chat/AIQueue.cshtml`:
  - [ ] Lista de chats sendo atendidos pela IA
  - [ ] Métricas de performance IA
- [ ] Atualizar controller `ChatsController`:
  - [ ] Método `AIQueue()`
  - [ ] Método `AssumeChat()`
  - [ ] Filtros por estado

### 3.4 FAQ Widget - IA First Flow
- [ ] Atualizar `chat-widget.js`:
  - [ ] Modificar `startChat()` para não criar backend chat
  - [ ] Implementar `sendToAI()` method
  - [ ] Implementar `handleAIResponse()` method
  - [ ] Implementar `requestEscalation()` method
  - [ ] Adicionar indicadores visuais IA vs Técnico
- [ ] Criar `displayAIMessage()` method:
  - [ ] Ícone de robô
  - [ ] Confidence indicator
  - [ ] Styling diferenciado
- [ ] Criar `showEscalationOptions()` method:
  - [ ] Botão "Falar com técnico"
  - [ ] Botão "Isso resolveu?"
- [ ] Implementar `displayEscalationNotice()`:
  - [ ] Notificação de transferência
  - [ ] Loading state durante escalação

### 3.5 FAQ Widget - UX Improvements
- [ ] Adicionar typing indicator para IA
- [ ] Implementar confidence visual (barra/estrelas)
- [ ] Adicionar quick actions:
  - [ ] "Sim, resolveu"
  - [ ] "Não, preciso de mais ajuda"
  - [ ] "Falar com técnico"
- [ ] Implementar chat state persistence
- [ ] Adicionar reconnection logic

### 3.6 CSS/Styling
- [ ] Criar estilos para mensagens IA:
  - [ ] `.ai-message` class
  - [ ] `.confidence-indicator` class
  - [ ] `.escalation-notice` class
- [ ] Criar estilos para estados:
  - [ ] `.chat-state-ai` class
  - [ ] `.chat-state-human` class
  - [ ] `.chat-state-escalating` class
- [ ] Adicionar animações de transição
- [ ] Responsive design para mobile

### 3.7 Testes Frontend
- [ ] Testar interface IA vs Técnico
- [ ] Testar indicadores de estado
- [ ] Testar botões de escalação
- [ ] Testar dashboard separado
- [ ] Testar responsividade
- [ ] Testar acessibilidade

## 📋 Sprint 4: Testes e Deploy (1 dia)

### 4.1 Testes de Integração
- [ ] Testar fluxo completo IA → Técnico:
  - [ ] Usuário inicia chat
  - [ ] IA responde pergunta
  - [ ] Usuário solicita escalação
  - [ ] Técnico recebe chat com histórico
  - [ ] Chat é finalizado
- [ ] Testar escalação automática:
  - [ ] IA com baixa confidence
  - [ ] Timeout de resposta IA
  - [ ] IA indisponível
- [ ] Testar múltiplos chats simultâneos
- [ ] Testar reconexão durante chat IA

### 4.2 Testes de Performance
- [ ] Tempo de resposta IA < 3s
- [ ] Escalação < 1s
- [ ] Suporte a 50+ chats simultâneos
- [ ] Memory usage aceitável
- [ ] Database performance

### 4.3 Testes de Edge Cases
- [ ] IA retorna erro
- [ ] Bot-ia container down
- [ ] Proxy service restart
- [ ] Backend indisponível
- [ ] Múltiplas tentativas de escalação
- [ ] Chat abandonado pelo usuário

### 4.4 Configuration & Feature Flags
- [ ] Adicionar configurações no .env:
  - [ ] `ENABLE_AI_FIRST=true`
  - [ ] `AI_CONFIDENCE_THRESHOLD=0.5`
  - [ ] `AI_MAX_ATTEMPTS=3`
  - [ ] `AI_TIMEOUT_SECONDS=30`
  - [ ] `ESCALATION_KEYWORDS=técnico,humano,pessoa`
- [ ] Implementar feature toggles
- [ ] Configurar rollout gradual

### 4.5 Monitoring & Logging
- [ ] Adicionar logs estruturados:
  - [ ] Chat iniciado com IA
  - [ ] Resposta IA (confidence)
  - [ ] Escalação (motivo)
  - [ ] Técnico atribuído
- [ ] Implementar métricas:
  - [ ] Taxa de resolução IA
  - [ ] Tempo médio de resposta
  - [ ] Motivos de escalação
  - [ ] Satisfação do usuário
- [ ] Configurar alertas:
  - [ ] IA indisponível
  - [ ] Alta taxa de escalação
  - [ ] Baixa confidence média

### 4.6 Documentation
- [ ] Atualizar README com novo fluxo
- [ ] Documentar APIs de escalação
- [ ] Criar guia de troubleshooting
- [ ] Documentar configurações
- [ ] Criar manual do técnico

### 4.7 Deploy Preparation
- [ ] Backup do banco de dados
- [ ] Testar migration em staging
- [ ] Preparar rollback plan
- [ ] Configurar feature flags
- [ ] Testar em ambiente de produção

## ✅ Checklist Final de Validação

### Fluxo IA-First
- [ ] Usuário inicia chat → IA responde primeiro
- [ ] IA mantém contexto da conversa
- [ ] Confidence score é calculado e exibido
- [ ] Escalação automática funciona (baixa confidence)
- [ ] Escalação manual funciona (botão/keywords)
- [ ] Histórico completo é preservado na escalação
- [ ] Técnico vê todo contexto anterior
- [ ] Estados do chat são atualizados corretamente

### Interface e UX
- [ ] Indicadores visuais funcionam (IA vs Técnico)
- [ ] Botão "Falar com técnico" sempre disponível
- [ ] Notificações de escalação são claras
- [ ] Dashboard mostra chats IA vs Humano
- [ ] Confidence score é visível e compreensível
- [ ] Animações e transições funcionam
- [ ] Interface é responsiva

### Performance e Confiabilidade
- [ ] Tempo de resposta IA < 3 segundos
- [ ] Escalação < 1 segundo
- [ ] Sistema suporta múltiplos chats
- [ ] Fallback funciona se IA falhar
- [ ] Reconexão funciona corretamente
- [ ] Logs e métricas estão funcionando

### Configuração e Deploy
- [ ] Feature flags funcionam
- [ ] Configurações são aplicadas
- [ ] Migration executou sem erros
- [ ] Rollback plan está pronto
- [ ] Monitoramento está ativo
- [ ] Documentação está atualizada

---

## 📝 Comandos Úteis

### Desenvolvimento
```bash
# Rebuild todos containers
docker compose down && docker compose up --build -d

# Logs do bot-ia
docker logs pim-bot-ia -f

# Logs do proxy
docker logs pim-proxy -f

# Testar IA
curl -X POST http://192.168.0.152:8001/api/ai/ask \
  -H "Content-Type: application/json" \
  -d '{"question": "Como criar um chamado?"}'

# Testar proxy
curl -X POST http://192.168.0.152:9000/api/proxy/start-chat \
  -H "Content-Type: application/json" \
  -d '{"user_name": "Teste", "user_email": "teste@teste.com", "initial_message": "Olá"}'
```

### Database
```sql
-- Verificar estados dos chats
SELECT State, COUNT(*) FROM Chats GROUP BY State;

-- Verificar mensagens por tipo
SELECT MessageType, COUNT(*) FROM ChatMessages GROUP BY MessageType;

-- Chats com IA ativa
SELECT * FROM Chats WHERE State = 1;
```

### Monitoramento
```bash
# Status dos containers
docker ps

# Uso de recursos
docker stats

# Health checks
curl http://192.168.0.152:8001/health
curl http://192.168.0.152:9000/health
```

---

**Total de itens**: 150+ checkboxes organizados em 4 sprints
**Tempo estimado**: 7 dias
**Complexidade**: Alta (integração completa IA + Chat)
