# Atualizar Chat Widget - IA First Flow

## 🎯 Objetivo
Atualizar o chat widget do FAQ para usar o novo fluxo IA-first onde a IA responde primeiro e escala para técnico quando necessário.

## 📋 Mudanças Necessárias

### 1. Fluxo Atual vs Novo Fluxo

#### Fluxo Atual
```
startChat() → Cria chat no backend → Polling mensagens técnico
```

#### Novo Fluxo IA-First
```
startChat() → Sessão IA → IA responde → [Escalação] → Técnico
```

### 2. Modificações no JavaScript

#### 2.1 Método `startChat()`
**Atual:**
- Chama `/api/proxy/start-chat`
- Cria chat no backend imediatamente
- Inicia polling

**Novo:**
- Chama `/api/proxy/start-chat` (novo endpoint)
- Cria apenas sessão IA
- Processa resposta inicial da IA
- Não inicia polling ainda

#### 2.2 Método `sendMessage()`
**Atual:**
- Envia para `/api/proxy/send-message`
- Sempre vai para técnico

**Novo:**
- Envia para `/api/proxy/send-message`
- Processa resposta baseada no tipo:
  - `type: "ai"` → Exibe resposta da IA
  - `type: "escalated"` → Inicia polling técnico
  - `type: "human"` → Mensagem enviada para técnico

#### 2.3 Novo Método `handleAIResponse()`
- Exibir mensagem da IA com ícone 🤖
- Mostrar confidence score (opcional)
- Adicionar botão "Falar com técnico" se confidence baixa

#### 2.4 Novo Método `handleEscalation()`
- Mostrar mensagem de transferência
- Iniciar polling para mensagens do técnico
- Mudar interface para modo "técnico"

#### 2.5 Indicadores Visuais
- **IA Ativa**: Ícone 🤖, cor azul
- **Escalando**: Mensagem "Transferindo..."
- **Técnico**: Ícone 👨‍💻, cor verde
- **Botão**: "Falar com técnico" sempre disponível

### 3. Estrutura de Resposta da API

#### Resposta IA
```json
{
  "type": "ai",
  "message": "Resposta da IA...",
  "confidence": 0.85
}
```

#### Resposta Escalação
```json
{
  "type": "escalated",
  "chat_id": 1046,
  "reason": "Usuário solicitou técnico",
  "message": "Transferindo para um técnico humano. Aguarde..."
}
```

#### Resposta Técnico
```json
{
  "type": "human",
  "status": "sent"
}
```

### 4. Estados do Chat Widget

#### Estado: `AI_ACTIVE`
- IA está respondendo
- Botão "Falar com técnico" disponível
- Não faz polling

#### Estado: `ESCALATING`
- Mostra mensagem de transferência
- Interface em loading

#### Estado: `HUMAN_ASSIGNED`
- Técnico atribuído
- Inicia polling normal
- Interface igual ao atual

### 5. Modificações Específicas

#### 5.1 Variáveis de Estado
```javascript
this.chatState = 'AI_ACTIVE'; // AI_ACTIVE, ESCALATING, HUMAN_ASSIGNED
this.chatId = null; // Só definido após escalação
```

#### 5.2 Método `addMessage()` Atualizado
```javascript
addMessage(type, text, timestamp = new Date()) {
    // type: 'user', 'ai', 'system', 'technician'
    const messageDiv = document.createElement('div');
    
    if (type === 'ai') {
        // Ícone de robô, cor azul
        messageDiv.innerHTML = `🤖 ${text}`;
    } else if (type === 'technician') {
        // Ícone de técnico, cor verde
        messageDiv.innerHTML = `👨‍💻 ${text}`;
    }
    // ...resto do código
}
```

#### 5.3 Botão "Falar com Técnico"
```javascript
showEscalationButton() {
    const button = `
        <button onclick="chatWidget.requestEscalation()" class="escalate-btn">
            👨‍💻 Falar com técnico
        </button>
    `;
    this.addSystemMessage(button);
}

requestEscalation() {
    this.sendMessage("quero falar com técnico");
}
```

#### 5.4 Polling Condicional
```javascript
startPolling() {
    // Só inicia polling se chat foi escalado
    if (this.chatState === 'HUMAN_ASSIGNED' && this.chatId) {
        setInterval(() => this.pollMessages(), this.config.pollingInterval);
    }
}
```

### 6. CSS Adicional

#### 6.1 Estilos para IA
```css
.message.ai {
    background-color: #e3f2fd;
    border-left: 4px solid #2196f3;
}

.message.ai::before {
    content: "🤖";
    margin-right: 8px;
}
```

#### 6.2 Botão de Escalação
```css
.escalate-btn {
    background: #4caf50;
    color: white;
    border: none;
    padding: 8px 16px;
    border-radius: 20px;
    cursor: pointer;
    margin: 8px 0;
}

.escalate-btn:hover {
    background: #45a049;
}
```

#### 6.3 Indicador de Estado
```css
.chat-state-indicator {
    padding: 4px 8px;
    border-radius: 12px;
    font-size: 12px;
    margin-bottom: 8px;
}

.state-ai {
    background: #e3f2fd;
    color: #1976d2;
}

.state-human {
    background: #e8f5e8;
    color: #388e3c;
}
```

### 7. Fluxo de Implementação

#### Passo 1: Atualizar `startChat()`
- Modificar para não criar chat no backend
- Processar resposta inicial da IA
- Definir estado inicial como `AI_ACTIVE`

#### Passo 2: Atualizar `sendMessage()`
- Processar diferentes tipos de resposta
- Implementar lógica de escalação
- Gerenciar estados do chat

#### Passo 3: Implementar Métodos IA
- `handleAIResponse()`
- `handleEscalation()`
- `showEscalationButton()`

#### Passo 4: Atualizar Interface
- Indicadores visuais
- Botões de ação
- CSS para IA vs Técnico

#### Passo 5: Polling Condicional
- Só fazer polling após escalação
- Manter compatibilidade com fluxo atual

### 8. Compatibilidade

#### Manter Funcionalidades Existentes
- ✅ Identificação do usuário (nome + email)
- ✅ Validação de formulário
- ✅ Persistência de sessão
- ✅ Polling de mensagens (após escalação)
- ✅ Interface responsiva

#### Adicionar Novas Funcionalidades
- 🆕 Resposta IA imediata
- 🆕 Indicadores visuais de estado
- 🆕 Botão de escalação manual
- 🆕 Confidence score (opcional)
- 🆕 Mensagens de sistema

### 9. Testes Necessários

#### Cenário 1: IA Resolve
1. Usuário abre chat
2. IA responde pergunta
3. Usuário satisfeito
4. Chat permanece com IA

#### Cenário 2: Escalação Manual
1. Usuário abre chat
2. IA responde
3. Usuário clica "Falar com técnico"
4. Chat escalado para técnico
5. Polling iniciado

#### Cenário 3: Escalação Automática
1. Usuário abre chat
2. IA não consegue responder (baixa confidence)
3. Escalação automática
4. Técnico atribuído

#### Cenário 4: Keywords de Escalação
1. Usuário digita "quero técnico"
2. Escalação imediata
3. Chat transferido

### 10. Configurações

#### Variáveis Configuráveis
```javascript
this.config = {
    proxyUrl: 'http://192.168.0.152:9000',
    showConfidenceScore: false,
    autoEscalateThreshold: 0.5,
    escalationKeywords: ['técnico', 'humano', 'pessoa'],
    showEscalationButton: true
};
```

---

## 📝 Resumo das Mudanças

### Arquivos a Modificar
- ✅ `/faq/wwwroot/js/chat-widget.js` (principal)
- ✅ `/faq/wwwroot/css/chat-widget.css` (estilos)

### Métodos a Atualizar
- ✅ `startChat()` - Fluxo IA-first
- ✅ `sendMessage()` - Processar tipos de resposta
- ✅ `addMessage()` - Suporte a mensagens IA
- ✅ `startPolling()` - Condicional após escalação

### Métodos a Criar
- 🆕 `handleAIResponse()`
- 🆕 `handleEscalation()`
- 🆕 `showEscalationButton()`
- 🆕 `requestEscalation()`

### Estados a Gerenciar
- 🆕 `AI_ACTIVE` → `ESCALATING` → `HUMAN_ASSIGNED`

**Resultado:** Chat widget totalmente integrado com fluxo IA-first, mantendo compatibilidade e adicionando nova experiência do usuário.
