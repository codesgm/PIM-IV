# Base to IA - Integração da Base de Conhecimento

## 🎯 Objetivo
Integrar a base de conhecimento do FAQ (perguntas e respostas) com a IA para que ela tenha acesso às informações específicas do sistema.

## 🔍 Problema Atual
- IA não conhece as FAQs específicas do sistema
- Respostas genéricas em vez de informações precisas
- Base de conhecimento do FAQ não está sendo utilizada pela IA

## 📋 Análise da Situação

### Base de Conhecimento Atual
**Localização**: `/bot-ia/knowledge/base_conhecimento.md`
**Conteúdo**: Informações genéricas sobre o MidTalk

### Base de FAQ (não integrada)
**Localização**: Banco de dados do backend
**Conteúdo**: Perguntas e respostas específicas do sistema

## 🔧 Plano de Implementação

### Etapa 1: Extrair FAQs do Banco
**Objetivo**: Buscar todas as FAQs do banco de dados

**Implementação**:
1. Criar endpoint no backend: `GET /api/faq/export`
2. Retornar todas as FAQs em formato estruturado
3. Incluir pergunta, resposta, categoria, tags

**Arquivo**: `/backend/Controllers/FaqController.cs`
```csharp
[HttpGet("export")]
public async Task<IActionResult> ExportFaqs()
{
    var faqs = await _context.Faqs
        .Where(f => f.IsActive)
        .Select(f => new {
            f.Question,
            f.Answer,
            f.Category,
            f.Tags
        })
        .ToListAsync();
    
    return Ok(faqs);
}
```

### Etapa 2: Gerar Markdown das FAQs
**Objetivo**: Converter FAQs em formato markdown para a IA

**Implementação**:
1. Criar script Python para buscar FAQs
2. Gerar arquivo markdown estruturado
3. Atualizar base de conhecimento da IA

**Arquivo**: `/bot-ia/scripts/update_knowledge.py`
```python
import requests
import json

def fetch_faqs():
    response = requests.get('http://api:5000/api/faq/export')
    return response.json()

def generate_markdown(faqs):
    markdown = "# Base de Conhecimento MidTalk\n\n"
    
    for faq in faqs:
        markdown += f"## {faq['question']}\n\n"
        markdown += f"{faq['answer']}\n\n"
        if faq['tags']:
            markdown += f"**Tags**: {faq['tags']}\n\n"
        markdown += "---\n\n"
    
    return markdown

def update_knowledge_base():
    faqs = fetch_faqs()
    markdown = generate_markdown(faqs)
    
    with open('/app/knowledge/faq_knowledge.md', 'w', encoding='utf-8') as f:
        f.write(markdown)
```

### Etapa 3: Atualizar KnowledgeService
**Objetivo**: Carregar múltiplos arquivos de conhecimento

**Implementação**:
1. Modificar KnowledgeService para carregar múltiplos arquivos
2. Combinar base genérica + FAQs específicas
3. Reload automático quando FAQs são atualizadas

**Arquivo**: `/bot-ia/app/services/knowledge_service.py`
```python
def load_knowledge(self):
    knowledge_files = [
        'base_conhecimento.md',
        'faq_knowledge.md'
    ]
    
    combined_knowledge = ""
    for file in knowledge_files:
        file_path = os.path.join(self.knowledge_path, file)
        if os.path.exists(file_path):
            with open(file_path, 'r', encoding='utf-8') as f:
                combined_knowledge += f.read() + "\n\n"
    
    self.knowledge_base = combined_knowledge
```

### Etapa 4: Endpoint de Atualização
**Objetivo**: Permitir atualização da base de conhecimento

**Implementação**:
1. Endpoint para trigger de atualização
2. Webhook quando FAQ é criada/editada
3. Reload automático da IA

**Arquivo**: `/bot-ia/app/main.py`
```python
@app.post("/api/ai/update-knowledge")
async def update_knowledge():
    # Buscar FAQs atualizadas
    # Regenerar markdown
    # Recarregar knowledge service
    success = knowledge_service.reload_from_api()
    return {"success": success}
```

### Etapa 5: Webhook no Backend
**Objetivo**: Notificar IA quando FAQ é alterada

**Implementação**:
1. Trigger após CRUD de FAQ
2. Chamar endpoint de atualização da IA
3. Log de sincronização

**Arquivo**: `/backend/Services/FaqService.cs`
```csharp
private async Task NotifyIAUpdate()
{
    try
    {
        var client = new HttpClient();
        await client.PostAsync("http://bot-ia:8001/api/ai/update-knowledge", null);
    }
    catch (Exception ex)
    {
        _logger.LogError($"Erro ao notificar IA: {ex.Message}");
    }
}
```

## 📊 Estrutura do Markdown Gerado

```markdown
# Base de Conhecimento MidTalk

## Como alterar o perfil de um usuário?

Para alterar o perfil de um usuário no MidTalk:

1. Acesse o menu "Usuários"
2. Clique no usuário desejado
3. Selecione "Editar Perfil"
4. Altere as informações necessárias
5. Clique em "Salvar"

**Tags**: usuário, perfil, editar

---

## Como criar um chamado?

Para criar um novo chamado:

1. Clique em "Novo Chamado"
2. Preencha o título e descrição
3. Selecione a categoria
4. Defina a prioridade
5. Clique em "Criar Chamado"

**Tags**: chamado, criar, novo

---
```

## 🚀 Cronograma de Implementação

### Fase 1 (1-2 horas)
- [x] Criar endpoint de export no backend
- [x] Implementar script de geração de markdown
- [x] Testar extração de FAQs

### Fase 2 (1 hora)
- [x] Modificar KnowledgeService para múltiplos arquivos
- [x] Implementar endpoint de atualização na IA
- [x] Testar carregamento combinado

### Fase 3 (30 min)
- [x] Implementar webhook no backend
- [x] Testar sincronização automática
- [x] Validar respostas da IA

## 🧪 Testes de Validação

### Teste 1: Extração de FAQs
```bash
curl http://localhost:5000/api/faq/export
```
**Esperado**: JSON com todas as FAQs ativas

### Teste 2: Geração de Markdown
```bash
python /bot-ia/scripts/update_knowledge.py
```
**Esperado**: Arquivo `faq_knowledge.md` gerado

### Teste 3: IA com Conhecimento
**Pergunta**: "Como alterar o perfil de um usuário?"
**Esperado**: Resposta específica baseada na FAQ

### Teste 4: Sincronização
1. Criar nova FAQ no admin
2. Verificar se IA foi notificada
3. Testar resposta da IA com nova informação

## 📈 Benefícios Esperados

- ✅ **Respostas precisas**: IA conhece FAQs específicas
- ✅ **Sincronização automática**: Sempre atualizada
- ✅ **Redução de escalações**: Mais perguntas respondidas pela IA
- ✅ **Experiência melhor**: Usuários recebem informações corretas

## 🔄 Manutenção

### Automática
- Webhook sincroniza quando FAQ é alterada
- IA recarrega conhecimento automaticamente
- Logs de sincronização para debug

### Manual
- Endpoint `/api/ai/update-knowledge` para forçar atualização
- Reload manual via `/api/ai/knowledge/reload`
- Verificação de integridade dos arquivos

---

**Prioridade**: ALTA
**Impacto**: Melhora significativa na qualidade das respostas da IA
**Esforço**: 2-3 horas de implementação
