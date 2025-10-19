# Fix Checklist - Chat Widget IA

## 🎯 Objetivo
Corrigir problema onde "IA pensando" aparece mas resposta não é exibida.

## ✅ Checklist de Implementação

### 1. Corrigir Proxy Service - Resposta Inicial
**Arquivo**: `/proxy/app/main.py`

- [x] Modificar endpoint `start-chat` para processar mensagem inicial
- [x] Retornar `initial_response` no JSON de resposta
- [x] Testar endpoint isoladamente ✅ **FUNCIONANDO**

**Código**:
```python
# No método start_chat(), após criar sessão:
ai_response = await proxy_service.handle_user_message(session_id, request.initial_message)

return {
    "session_id": session_id,
    "status": "ai_active", 
    "initial_response": ai_response  # ← ADICIONAR
}
```

### 2. Ajustar Escalation Engine
**Arquivo**: `/proxy/app/services/escalation_service.py`

- [x] Aumentar `MAX_AI_ATTEMPTS` de 3 para 5 ✅
- [x] Ajustar `CONFIDENCE_THRESHOLD` de atual para 0.4 ✅
- [x] Verificar lógica de escalação por tentativas ✅

**Localizar e modificar**:
```python
MAX_AI_ATTEMPTS = 5  # Era 3
CONFIDENCE_THRESHOLD = 0.4  # Era maior
```

### 3. Melhorar Chat Widget - Processamento
**Arquivo**: `/faq/wwwroot/js/chat-widget.js`

- [x] Adicionar tratamento para `ai_with_escalation` no `sendMessage()` ✅
- [x] Verificar se `handleAIResponse()` está sendo chamado corretamente ✅
- [x] Adicionar logs de debug no console ✅

**Código**:
```javascript
// No método sendMessage(), após receber data:
if (data.type === 'ai_with_escalation') {
    this.handleAIResponse(data);
    this.addMessage('system', 'Posso escalar para um técnico se precisar.');
}
```

### 4. Adicionar Logs de Debug
**Múltiplos arquivos**

- [x] **Proxy Service**: Log da resposta sendo enviada ✅
- [x] **Chat Widget**: Console.log da resposta recebida ✅
- [x] **AI Service**: Log detalhado da comunicação ✅

**Exemplos**:
```python
# proxy_service.py
logger.info(f"Enviando resposta: {response}")
```

```javascript
// chat-widget.js
console.log('Resposta recebida:', data);
```

### 5. Testes de Validação

- [ ] **Teste 1**: Abrir chat, enviar "Olá" → IA deve responder visível
- [ ] **Teste 2**: Enviar pergunta complexa → IA responde + opção escalação
- [ ] **Teste 3**: Clicar "Falar com técnico" → Escalação funciona
- [ ] **Teste 4**: Verificar logs no console do navegador
- [ ] **Teste 5**: Verificar logs do Docker (proxy e bot-ia)

### 6. Deploy e Verificação

- [x] Parar containers: `docker compose down` ✅
- [x] Rebuild containers: `docker compose build` ✅
- [x] Iniciar containers: `docker compose up -d` ✅
- [x] Verificar logs: `docker compose logs -f proxy bot-ia` ✅
- [x] Testar no navegador: http://192.168.0.152:5030 ✅ **PRONTO PARA TESTE**

## 🔧 Comandos Úteis

```bash
# Rebuild e restart
cd /home/guilherme-araujo/Documentos/PIM
docker compose down
docker compose build proxy bot-ia
docker compose up -d

# Monitorar logs
docker compose logs -f proxy bot-ia

# Testar endpoints
curl -X POST http://192.168.0.152:9000/api/proxy/start-chat \
  -H "Content-Type: application/json" \
  -d '{"user_name":"Teste","user_email":"teste@email.com","initial_message":"Olá"}'
```

## 🎯 Critérios de Sucesso

- [ ] ✅ "IA pensando" aparece E resposta é exibida
- [ ] ✅ Resposta aparece em < 3 segundos
- [ ] ✅ Escalação só acontece quando necessário (confidence < 0.4)
- [ ] ✅ Logs mostram fluxo completo funcionando
- [ ] ✅ Não há erros no console do navegador

## 🚨 Rollback Plan

Se algo der errado:
```bash
# Voltar versão anterior
git checkout HEAD~1 -- proxy/ faq/
docker compose down
docker compose build
docker compose up -d
```

## 📝 Notas de Implementação

1. **Ordem de implementação**: Proxy → Escalation → Widget → Testes
2. **Testar incrementalmente**: Uma correção por vez
3. **Monitorar logs**: Sempre verificar logs após cada mudança
4. **Backup**: Fazer commit antes de começar as alterações

---
**Tempo estimado**: 1-2 horas
**Prioridade**: CRÍTICA
**Responsável**: Implementar na ordem do checklist
