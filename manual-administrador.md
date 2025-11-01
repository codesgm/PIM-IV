# Manual de Uso - Sistema MidTalk
## Guia para Administradores Iniciantes

### 🎯 **Bem-vindo ao MidTalk!**

Este manual irá guiá-lo através dos primeiros passos para configurar e utilizar o Sistema MidTalk - Gestão de Chamados e Suporte Técnico com IA.

---

## 📋 **Pré-requisitos**

- Acesso à URL do sistema: `http://[IP_DO_SERVIDOR]:5028`
- Credenciais de administrador fornecidas pela equipe técnica
- Navegador web atualizado (Chrome, Firefox, Edge)

---

## 🚀 **Primeiros Passos**

### **Passo 1: Fazer Login no Sistema**

1. **Acesse a URL do sistema** no seu navegador
2. **Tela de Login** será exibida
3. **Digite suas credenciais**:
   - **Email**: `guilhermetts0@gmail.com` (padrão inicial)
   - **Senha**: `admin123` (padrão inicial)
4. **Clique em "Entrar"**
5. **Você será redirecionado** para o Dashboard Administrativo

> ⚠️ **IMPORTANTE**: Altere a senha padrão imediatamente após o primeiro login!

---

### **Passo 2: Alterar Senha Padrão (OBRIGATÓRIO)**

1. **No menu superior**, clique no seu nome
2. **Selecione "Meu Perfil"**
3. **Clique em "Alterar Senha"**
4. **Digite**:
   - Senha atual: `admin123`
   - Nova senha: (escolha uma senha forte)
   - Confirme a nova senha
5. **Clique em "Salvar"**
6. **Faça logout e login** com a nova senha

---

### **Passo 3: Cadastrar Primeiro Técnico**

1. **No menu lateral**, clique em **"Usuários"**
2. **Clique no botão "Novo Usuário"**
3. **Preencha os dados**:
   - **Nome**: Nome completo do técnico
   - **Email**: Email único do técnico
   - **Perfil**: Selecione "Técnico"
4. **Clique em "Salvar"**
5. **O sistema gerará senha padrão "123"**
6. **Informe ao técnico** suas credenciais de acesso

> 💡 **Dica**: O técnico deve alterar a senha no primeiro login!

---

### **Passo 4: Configurar FAQs Iniciais**

1. **No menu lateral**, clique em **"FAQs"**
2. **Clique no botão "Nova FAQ"**
3. **Preencha**:
   - **Pergunta**: Pergunta frequente dos clientes
   - **Resposta**: Resposta clara e objetiva
   - **Categoria**: Selecione categoria apropriada
4. **Clique em "Salvar"**
5. **Repita** para criar pelo menos 5-10 FAQs iniciais

**Categorias Disponíveis**:
- Gestão de Contas
- Relatórios
- Configurações
- Problemas Técnicos

---

### **Passo 5: Testar o Portal FAQ**

1. **Abra nova aba** no navegador
2. **Acesse**: `http://[IP_DO_SERVIDOR]:5030`
3. **Verifique se**:
   - FAQs estão sendo exibidas
   - Busca funciona corretamente
   - Chat IA está disponível
4. **Teste o chat IA**:
   - Clique no widget de chat
   - Digite nome e email de teste
   - Faça uma pergunta relacionada às FAQs

---

## 🎛️ **Funcionalidades Principais**

### **Dashboard Administrativo**

**O que você verá**:
- Resumo de chamados ativos
- Técnicos online/offline
- Métricas de atendimento
- Gráficos de performance

**Como usar**:
- Monitore indicadores em tempo real
- Clique nos números para ver detalhes
- Use filtros para análises específicas

---

### **Gestão de Usuários**

#### **Listar Usuários**
- **Menu**: Usuários → Listar
- **Funcionalidades**:
  - Ver todos os técnicos cadastrados
  - Filtrar por status (Ativo/Inativo)
  - Buscar por nome ou email
  - Editar dados dos usuários

#### **Cadastrar Novo Usuário**
- **Menu**: Usuários → Novo
- **Campos obrigatórios**:
  - Nome completo
  - Email único
  - Perfil (Administrador/Técnico)
- **Senha**: Gerada automaticamente como "123"

#### **Editar Usuário**
- **Na listagem**, clique no ícone de edição
- **Pode alterar**: Nome, email, status, perfil
- **Não pode alterar**: Senha (usuário deve alterar)

#### **Desativar Usuário**
- **Na listagem**, clique no ícone de desativação
- **Confirme a ação**
- **Usuário fica inativo** mas dados são preservados
- **Chamados ativos** são reatribuídos automaticamente

---

### **Gestão de Chamados**

#### **Visualizar Chamados**
- **Menu**: Chamados → Listar
- **Informações exibidas**:
  - Número do protocolo
  - Nome do cliente
  - Status atual
  - Técnico responsável
  - Data de criação

#### **Filtros Disponíveis**:
- **Por Status**: Ativo, Resolvido, Fechado
- **Por Técnico**: Chamados de técnico específico
- **Por Período**: Data de criação
- **Busca**: Por nome do cliente ou protocolo

#### **Detalhes do Chamado**
- **Clique no protocolo** para ver detalhes
- **Visualize**:
  - Dados completos do cliente
  - Histórico de mensagens
  - Tempo de atendimento
  - Ações realizadas

---

### **Gestão de FAQs**

#### **Listar FAQs**
- **Menu**: FAQs → Listar
- **Funcionalidades**:
  - Ver todas as perguntas cadastradas
  - Filtrar por categoria
  - Buscar por palavra-chave
  - Ativar/desativar FAQs

#### **Criar Nova FAQ**
- **Menu**: FAQs → Nova
- **Preencha**:
  - Pergunta clara e objetiva
  - Resposta completa e útil
  - Categoria apropriada
- **Status**: Ativa por padrão

#### **Editar FAQ Existente**
- **Na listagem**, clique no ícone de edição
- **Pode alterar**: Pergunta, resposta, categoria, status
- **Salve as alterações**

---

## 📊 **Relatórios e Métricas**

### **Dashboard Principal**
- **Chamados por Status**: Gráfico em tempo real
- **Performance dos Técnicos**: Ranking de atendimento
- **Satisfação dos Clientes**: Média de avaliações
- **Efetividade da IA**: Taxa de resolução automática

### **Relatórios Detalhados**
- **Menu**: Relatórios
- **Tipos disponíveis**:
  - Relatório de Performance por Técnico
  - Relatório de Chamados por Período
  - Métricas de Uso da IA
  - Análise de Satisfação

---

## ⚙️ **Configurações do Sistema**

### **Configurações Gerais**
- **Menu**: Configurações → Geral
- **Opções**:
  - Timeout de sessão
  - Configurações de email
  - Parâmetros da IA
  - Backup automático

### **Configurações de Notificação**
- **Menu**: Configurações → Notificações
- **Configure**:
  - Alertas por email
  - Notificações push
  - Escalação automática
  - Lembretes de SLA

---

## 🔧 **Solução de Problemas Comuns**

### **Não Consigo Fazer Login**
1. **Verifique** se a URL está correta
2. **Confirme** email e senha
3. **Limpe** cache do navegador
4. **Tente** em modo anônimo/privado
5. **Contate** suporte técnico se persistir

### **Técnico Não Recebe Chamados**
1. **Verifique** se técnico está ativo
2. **Confirme** se técnico fez login
3. **Verifique** configurações de atribuição
4. **Teste** atribuição manual

### **IA Não Está Respondendo**
1. **Verifique** se serviço de IA está ativo
2. **Confirme** configurações da API
3. **Teste** conectividade de rede
4. **Reinicie** serviços se necessário

### **FAQs Não Aparecem no Portal**
1. **Verifique** se FAQs estão ativas
2. **Confirme** se portal FAQ está funcionando
3. **Limpe** cache do navegador
4. **Teste** em dispositivo diferente

---

## 📞 **Suporte Técnico**

### **Contatos de Emergência**
- **Email**: suporte@midtalk.com
- **Telefone**: (11) 9999-9999
- **WhatsApp**: (11) 9999-9999

### **Horário de Atendimento**
- **Segunda a Sexta**: 8h às 18h
- **Sábados**: 8h às 12h
- **Emergências**: 24h (apenas críticas)

### **Documentação Adicional**
- **Manual Técnico**: Para configurações avançadas
- **API Documentation**: Para integrações
- **Troubleshooting**: Para problemas específicos

---

## ✅ **Checklist de Configuração Inicial**

### **Configuração Básica**
- [ ] Login realizado com sucesso
- [ ] Senha padrão alterada
- [ ] Primeiro técnico cadastrado
- [ ] FAQs iniciais criadas (mínimo 5)
- [ ] Portal FAQ testado
- [ ] Chat IA testado

### **Configuração Avançada**
- [ ] Configurações de email definidas
- [ ] Parâmetros de IA ajustados
- [ ] Notificações configuradas
- [ ] Backup automático ativado
- [ ] Relatórios testados
- [ ] Equipe treinada

### **Testes de Funcionamento**
- [ ] Login de técnico testado
- [ ] Criação de chamado testada
- [ ] Atribuição automática funcionando
- [ ] Chat em tempo real funcionando
- [ ] Escalação IA → Técnico testada
- [ ] Relatórios sendo gerados

---

## 🎓 **Próximos Passos**

### **Após Configuração Inicial**
1. **Treine sua equipe** nos procedimentos
2. **Configure alertas** personalizados
3. **Ajuste parâmetros** da IA conforme uso
4. **Monitore métricas** regularmente
5. **Colete feedback** dos usuários
6. **Otimize processos** continuamente

### **Manutenção Regular**
- **Diária**: Verificar dashboard e chamados pendentes
- **Semanal**: Revisar relatórios de performance
- **Mensal**: Atualizar FAQs e configurações
- **Trimestral**: Análise completa e otimizações

---

**🎉 Parabéns! Você está pronto para usar o MidTalk!**

*Este manual cobre os aspectos essenciais para começar. Para funcionalidades avançadas, consulte a documentação técnica completa.*

---

**Versão do Manual**: 1.0  
**Data**: 29/10/2024  
**Sistema**: MidTalk v1.0
