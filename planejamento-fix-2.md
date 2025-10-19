# Planejamento Fix 2 - Análise Detalhada do Problema

## 🔍 Problema Real Identificado

**Sintoma**: IA escala automaticamente no `start-chat` sem mostrar resposta
**Evidência**: "Conectando com nossa IA..." → "Transferindo para um técnico humano"

## 📊 Análise do Fluxo Atual

### O que está acontecendo:
1. **Chat Widget** → `start-chat` → **Proxy Service**
2. **Proxy Service** → `create_session()` → Cria sessão IA
3. **Proxy Service** → `handle_user_message()` → Chama IA
4. **IA** → Responde com confidence 0.85 ✅
5. **Escalation Engine** → `should_escalate()` → **ESCALA IMEDIATAMENTE** ❌

### Causa Raiz:
O `start-chat` está chamando `handle_user_message()` que **sempre escala** porque:
- Mensagem inicial: "Olá, sou Guilherme Mello e preciso de ajuda"
- Contém palavra "ajuda" → Detecta como escalação
- OU está escalando por outro critério automático

## 🔧 Diagnóstico Detalhado

### 1. Verificar Escalation Engine
**Arquivo**: `/proxy/app/services/escalation_service.py`

**Suspeita 1**: Keywords muito amplas
```python
ESCALATION_KEYWORDS = [
    "técnico", "tecnico", "humano", "pessoa", "atendente",
    "não resolveu", "nao resolveu", "não funcionou", "nao funcionou",
    "escalação", "escalacao", "quero falar com", "preciso de ajuda",  # ← PROBLEMA
    "falar com técnico", "falar com tecnico", "atendimento humano"
]
```

**"preciso de ajuda"** está nas keywords → Escalação automática!

### 2. Verificar Lógica de Escalação
**Arquivo**: `/proxy/app/proxy_service.py`

**Suspeita 2**: Ordem de verificação
```python
# Verificar se deve escalar antes de enviar para IA
if escalation_engine.detect_escalation_intent(message):
    return await self._escalate_to_human(session, message, "Usuário solicitou técnico")
```

**Problema**: Verifica escalação ANTES da IA responder!

### 3. Verificar Mensagem Inicial
**Arquivo**: `/faq/wwwroot/js/chat-widget.js`

```javascript
initial_message: `Olá, sou ${this.userData.name} e preciso de ajuda`
```

**"preciso de ajuda"** → Keyword de escalação → Escala imediatamente!

## 🎯 Solução Definitiva

### Correção 1: Remover Keywords Genéricas
**Arquivo**: `/proxy/app/services/escalation_service.py`

```python
ESCALATION_KEYWORDS = [
    "técnico", "tecnico", "humano", "pessoa", "atendente",
    "falar com técnico", "falar com tecnico", "atendimento humano",
    "quero técnico", "quero tecnico", "escalação", "escalacao"
]
# REMOVER: "preciso de ajuda", "quero falar com", "não resolveu", etc.
```

### Correção 2: Mudar Mensagem Inicial
**Arquivo**: `/faq/wwwroot/js/chat-widget.js`

```javascript
initial_message: `Olá, sou ${this.userData.name}. Como posso ser ajudado?`
// OU
initial_message: `Usuário ${this.userData.name} iniciou conversa`
```

### Correção 3: Ajustar Ordem de Verificação
**Arquivo**: `/proxy/app/proxy_service.py`

```python
async def _handle_ai_message(self, session: ProxySession, message: str) -> Dict:
    # PRIMEIRO: Enviar para IA
    ai_response = await ai_service.ask_ai(message)
    session.ai_attempts += 1
    session.last_ai_confidence = ai_response.confidence
    
    # DEPOIS: Verificar se deve escalar (só keywords explícitas)
    if escalation_engine.detect_escalation_intent(message):
        return await self._escalate_to_human(session, message, "Usuário solicitou técnico")
    
    # Verificar escalação por confidence/tentativas
    should_escalate, reason = escalation_engine.should_escalate(session, message, ai_response.confidence)
    
    if should_escalate:
        return {
            "type": "ai_with_escalation",
            "message": ai_response.answer,
            "confidence": ai_response.confidence,
            "escalation_reason": reason
        }
    
    return {
        "type": "ai", 
        "message": ai_response.answer,
        "confidence": ai_response.confidence
    }
```

## 📋 Checklist de Implementação

### Prioridade CRÍTICA
- [ ] **1. Corrigir Keywords**: Remover "preciso de ajuda" e similares
- [ ] **2. Mudar Mensagem Inicial**: Remover palavras que triggam escalação
- [ ] **3. Testar Isoladamente**: Verificar se IA responde sem escalar

### Prioridade ALTA  
- [ ] **4. Ajustar Ordem**: IA primeiro, escalação depois
- [ ] **5. Logs Detalhados**: Mostrar exatamente por que está escalando
- [ ] **6. Teste Completo**: Fluxo end-to-end

## 🧪 Testes de Validação

### Teste 1: Mensagem Inicial Neutra
```
Entrada: "Usuário João iniciou conversa"
Esperado: IA responde, NÃO escala
```

### Teste 2: Pergunta Simples
```
Entrada: "Olá, como criar um chamado?"
Esperado: IA responde, NÃO escala
```

### Teste 3: Escalação Explícita
```
Entrada: "quero falar com técnico"
Esperado: Escala imediatamente
```

### Teste 4: Escalação por Confidence
```
Entrada: Pergunta muito complexa
Esperado: IA responde + opção escalação
```

## 🎯 Implementação Sequencial

### Passo 1: Keywords (CRÍTICO)
```python
# Manter apenas keywords explícitas
ESCALATION_KEYWORDS = [
    "técnico", "tecnico", "humano", "pessoa", "atendente",
    "falar com técnico", "falar com tecnico", 
    "quero técnico", "quero tecnico"
]
```

### Passo 2: Mensagem Inicial (CRÍTICO)
```javascript
initial_message: `Usuário iniciou conversa via FAQ`
```

### Passo 3: Teste Básico
- Rebuild containers
- Testar mensagem simples
- Verificar se IA responde SEM escalar

### Passo 4: Refinamentos
- Ajustar ordem de verificação
- Melhorar logs
- Testes completos

## 🚨 Validação de Sucesso

**Critério**: IA deve responder pelo menos 1 mensagem antes de qualquer escalação

**Teste Mínimo**:
1. Abrir chat
2. Enviar "Olá"
3. **DEVE**: Ver resposta da IA
4. **NÃO DEVE**: Escalar automaticamente

---

**Foco**: Corrigir keywords e mensagem inicial PRIMEIRO, depois refinamentos.
**Objetivo**: IA responde pelo menos uma vez antes de escalar.
**Validação**: Teste simples "Olá" deve funcionar.
