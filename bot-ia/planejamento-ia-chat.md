# Planejamento - IA First Chat Flow

## 🎯 Objetivo
Implementar um fluxo onde a IA responde primeiro as perguntas dos usuários, e apenas quando necessário, o chat é escalado para um técnico humano.

## 📋 Fluxo Atual vs Novo Fluxo

### Fluxo Atual
```
Usuário → FAQ Chat → Proxy → Backend → Técnico
```

### Novo Fluxo (IA First)
```
Usuário → FAQ Chat → Proxy → Bot IA → [Resolvido OU Escalação] → Técnico
```

## 🔄 Detalhamento do Novo Fluxo

### 1. Início do Chat
- Usuário abre chat no FAQ
- Identificação (nome + email)
- **Primeira mensagem vai para IA**
- IA responde baseada na base de conhecimento

### 2. Interação com IA
- IA tenta resolver a dúvida
- Usuário pode fazer perguntas de follow-up
- IA mantém contexto da conversa
- **Todas mensagens ficam registradas**

### 3. Critérios de Escalação para Técnico
- **Automática**: IA não consegue responder (confidence < 0.5)
- **Manual**: Usuário digita palavras-chave como:
  - "quero falar com técnico"
  - "não resolveu"
  - "preciso de ajuda humana"
  - "escalação"
- **Timeout**: Após X tentativas sem resolução
- **Complexidade**: IA detecta problema técnico complexo

### 4. Transição IA → Técnico
- Chat é transferido para fila de técnicos
- **Histórico completo** é preservado
- Técnico vê toda conversa anterior
- Usuário é notificado da transferência

## 🏗️ Arquitetura Técnica

### Componentes Necessários

#### 1. Chat State Management
```
Estados do Chat:
- AI_ACTIVE: IA respondendo
- AI_ESCALATING: Transferindo para técnico
- HUMAN_ASSIGNED: Técnico atribuído
- RESOLVED: Chat finalizado
```

#### 2. Message Types
```
Tipos de Mensagem:
- USER: Mensagem do usuário
- AI: Resposta da IA
- SYSTEM: Notificações do sistema
- TECHNICIAN: Mensagem do técnico
```

#### 3. Escalation Engine
```
Critérios de Escalação:
- Confidence Score < 0.5
- Keywords de escalação
- Timeout (5 min sem resolução)
- Comando manual do usuário
```

## 📝 Implementação Detalhada

### Fase 1: Modificar Proxy Service

#### 1.1 Novos Models
```python
class ChatState(Enum):
    AI_ACTIVE = "ai_active"
    AI_ESCALATING = "ai_escalating" 
    HUMAN_ASSIGNED = "human_assigned"
    RESOLVED = "resolved"

class MessageType(Enum):
    USER = "user"
    AI = "ai"
    SYSTEM = "system"
    TECHNICIAN = "technician"

class ProxySession:
    session_id: str
    chat_id: int
    state: ChatState = ChatState.AI_ACTIVE
    ai_attempts: int = 0
    last_ai_confidence: float = 0.0
    escalation_reason: Optional[str] = None
```

#### 1.2 Escalation Logic
```python
class EscalationEngine:
    ESCALATION_KEYWORDS = [
        "técnico", "humano", "pessoa", "atendente",
        "não resolveu", "não funcionou", "escalação",
        "quero falar com", "preciso de ajuda"
    ]
    
    def should_escalate(self, message: str, confidence: float, attempts: int) -> bool:
        # Confidence baixa
        if confidence < 0.5:
            return True
            
        # Keywords de escalação
        if any(keyword in message.lower() for keyword in self.ESCALATION_KEYWORDS):
            return True
            
        # Muitas tentativas
        if attempts >= 3:
            return True
            
        return False
```

#### 1.3 Modified Chat Flow
```python
async def handle_user_message(session_id: str, message: str):
    session = get_session(session_id)
    
    if session.state == ChatState.AI_ACTIVE:
        # Verificar se deve escalar
        if escalation_engine.should_escalate(message, session.last_ai_confidence, session.ai_attempts):
            await escalate_to_human(session, message)
        else:
            await send_to_ai(session, message)
    
    elif session.state == ChatState.HUMAN_ASSIGNED:
        await send_to_backend(session, message)
```

### Fase 2: Modificar Backend

#### 2.1 Chat Model Updates
```csharp
public class Chat
{
    // Campos existentes...
    public ChatState State { get; set; } = ChatState.AI_ACTIVE;
    public int AIAttempts { get; set; } = 0;
    public string? EscalationReason { get; set; }
    public DateTime? EscalatedAt { get; set; }
}

public enum ChatState
{
    AI_ACTIVE = 1,
    AI_ESCALATING = 2,
    HUMAN_ASSIGNED = 3,
    RESOLVED = 4
}
```

#### 2.2 Message Model Updates
```csharp
public class ChatMessage
{
    // Campos existentes...
    public MessageType MessageType { get; set; }
    public float? AIConfidence { get; set; }
}

public enum MessageType
{
    USER = 1,
    AI = 2,
    SYSTEM = 3,
    TECHNICIAN = 4
}
```

#### 2.3 New Endpoints
```csharp
[HttpPost("{id}/escalate")]
public async Task<IActionResult> EscalateToHuman(int id, EscalateRequest request)

[HttpGet("ai-queue")]
public async Task<IActionResult> GetAIActiveChats()

[HttpPost("{id}/ai-response")]
public async Task<IActionResult> AddAIResponse(int id, AIResponseRequest request)
```

### Fase 3: Modificar Frontend

#### 3.1 Chat Interface Updates
```html
<!-- Indicador de estado do chat -->
<div class="chat-state-indicator">
    <span class="badge badge-ai" *ngIf="chat.state === 'AI_ACTIVE'">
        <i class="fas fa-robot"></i> IA Ativa
    </span>
    <span class="badge badge-human" *ngIf="chat.state === 'HUMAN_ASSIGNED'">
        <i class="fas fa-user"></i> Técnico Atribuído
    </span>
</div>

<!-- Histórico com tipos de mensagem -->
<div class="message ai-message" *ngIf="message.type === 'AI'">
    <div class="ai-avatar">🤖</div>
    <div class="message-content">{{ message.content }}</div>
    <div class="confidence-score">Confiança: {{ message.confidence }}%</div>
</div>
```

#### 3.2 Technician Dashboard
```html
<!-- Fila de chats IA vs Humano -->
<div class="chat-queues">
    <div class="ai-queue">
        <h3>Chats com IA ({{ aiChats.length }})</h3>
        <!-- Lista de chats sendo atendidos pela IA -->
    </div>
    
    <div class="human-queue">
        <h3>Aguardando Técnico ({{ humanChats.length }})</h3>
        <!-- Lista de chats escalados -->
    </div>
</div>
```

### Fase 4: Modificar FAQ Chat Widget

#### 4.1 AI-First Messaging
```javascript
class ChatWidget {
    async sendMessage(message) {
        // Sempre enviar para proxy primeiro
        const response = await this.sendToProxy(message);
        
        if (response.type === 'AI') {
            this.displayAIMessage(response);
        } else if (response.type === 'ESCALATED') {
            this.displayEscalationNotice();
        }
    }
    
    displayEscalationNotice() {
        this.addSystemMessage("Transferindo para um técnico humano. Aguarde...");
    }
    
    displayAIMessage(response) {
        this.addMessage('ai', response.message);
        if (response.confidence < 0.7) {
            this.showEscalationOption();
        }
    }
    
    showEscalationOption() {
        const escalateBtn = `
            <button onclick="chatWidget.requestEscalation()" class="escalate-btn">
                Falar com técnico humano
            </button>
        `;
        this.addSystemMessage("Não consegui resolver? " + escalateBtn);
    }
}
```

## 🎨 UX/UI Considerations

### 1. Indicadores Visuais
- **IA Ativa**: Ícone de robô, cor azul
- **Escalando**: Ícone de transferência, cor amarela
- **Técnico**: Ícone de pessoa, cor verde
- **Confidence Score**: Barra de progresso para respostas IA

### 2. Mensagens do Sistema
- "🤖 IA está analisando sua pergunta..."
- "🔄 Transferindo para técnico humano..."
- "👨‍💻 Técnico João foi atribuído ao seu chat"
- "✅ Chat finalizado"

### 3. Botões de Ação
- "Falar com técnico" (sempre visível)
- "Isso resolveu?" (após resposta IA)
- "Tentar novamente" (se IA falhar)

## 📊 Métricas e Analytics

### 1. Métricas de IA
- Taxa de resolução por IA (sem escalação)
- Confidence score médio
- Tempo médio de resposta IA
- Principais tópicos resolvidos pela IA

### 2. Métricas de Escalação
- Taxa de escalação (% de chats que vão para humano)
- Motivos de escalação mais comuns
- Tempo médio até escalação
- Satisfação pós-escalação

### 3. Métricas de Eficiência
- Redução de carga para técnicos
- Tempo total de resolução
- Satisfação geral do usuário

## 🔧 Configurações

### 1. Parâmetros Ajustáveis
```env
# IA Configuration
AI_CONFIDENCE_THRESHOLD=0.5
AI_MAX_ATTEMPTS=3
AI_TIMEOUT_MINUTES=5

# Escalation
AUTO_ESCALATE_LOW_CONFIDENCE=true
ESCALATION_KEYWORDS="técnico,humano,pessoa,atendente"
SHOW_ESCALATION_BUTTON=true
```

### 2. Feature Flags
```env
# Feature toggles
ENABLE_AI_FIRST=true
ENABLE_AUTO_ESCALATION=true
ENABLE_CONFIDENCE_DISPLAY=false
ENABLE_AI_LEARNING=false
```

## 🧪 Testes Necessários

### 1. Testes de Fluxo
- ✅ IA responde pergunta simples
- ✅ IA escala automaticamente (baixa confidence)
- ✅ Usuário solicita escalação manual
- ✅ Técnico recebe histórico completo
- ✅ Chat é finalizado corretamente

### 2. Testes de Edge Cases
- IA indisponível → Escalação automática
- Múltiplas tentativas de escalação
- Timeout de resposta IA
- Reconexão durante chat IA

### 3. Testes de Performance
- Tempo de resposta IA < 3s
- Escalação < 1s
- Suporte a múltiplos chats simultâneos

## 📅 Cronograma de Implementação

### Sprint 1 (2 dias)
- ✅ Modificar proxy service (estados, escalação)
- ✅ Atualizar models e enums
- ✅ Implementar escalation engine

### Sprint 2 (2 dias)
- ✅ Modificar backend (novos campos, endpoints)
- ✅ Atualizar DTOs e controllers
- ✅ Implementar migration de banco

### Sprint 3 (2 dias)
- ✅ Modificar frontend (indicadores, dashboard)
- ✅ Atualizar chat widget (IA first)
- ✅ Implementar UX de escalação

### Sprint 4 (1 dia)
- ✅ Testes integrados
- ✅ Ajustes finais
- ✅ Deploy e monitoramento

**Total: 7 dias**

## 🚀 Rollout Strategy

### Fase 1: Soft Launch
- Ativar apenas para 10% dos usuários
- Monitorar métricas de perto
- Coletar feedback

### Fase 2: Gradual Rollout
- 50% dos usuários
- Ajustar parâmetros baseado em dados
- Treinar base de conhecimento

### Fase 3: Full Rollout
- 100% dos usuários
- Monitoramento contínuo
- Otimizações baseadas em uso

## 🔍 Critérios de Sucesso

### Objetivos Primários
- **Taxa de resolução IA**: > 60%
- **Redução carga técnicos**: > 40%
- **Satisfação usuário**: > 4.0/5.0
- **Tempo médio resolução**: < 5 minutos

### Objetivos Secundários
- **Confidence score médio**: > 0.7
- **Taxa escalação desnecessária**: < 10%
- **Uptime IA**: > 99%
- **Tempo resposta IA**: < 3 segundos

---

*Este planejamento garante uma transição suave para o modelo IA-first, mantendo a qualidade do atendimento e melhorando a eficiência geral do sistema.*
