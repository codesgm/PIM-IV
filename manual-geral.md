# Manual Geral - Sistema MidTalk

## 📋 Visão Geral do Projeto

O **MidTalk** é um sistema integrado de gestão de chamados e suporte técnico desenvolvido como Projeto Integrado Multidisciplinar (PIM) do curso de Análise e Desenvolvimento de Sistemas da UNIP. O sistema foi projetado para empresas que precisam de um atendimento eficiente e organizado, incorporando tecnologias modernas como Inteligência Artificial para otimizar o suporte ao usuário.

## 🎯 Objetivo Principal

Desenvolver um sistema de suporte técnico inteligente que utiliza IA para responder automaticamente às dúvidas dos usuários, com escalação inteligente para técnicos humanos quando necessário, promovendo autonomia dos usuários e otimizando o atendimento.

## 🏗️ Arquitetura do Sistema

### Tecnologias Utilizadas
- **Backend**: ASP.NET Core Web API (.NET 9.0)
- **Frontend**: ASP.NET Core MVC (.NET 9.0)
- **Banco de Dados**: SQL Server 2022
- **Containerização**: Docker Compose
- **Inteligência Artificial**: Google Gemini API
- **Chat Widget**: JavaScript vanilla com integração IA

### Estrutura de Componentes
```
MidTalk/
├── Backend API          # Gerenciamento de dados e lógica de negócio
├── Frontend Web         # Interface administrativa (CORE)
├── FAQ Público          # Portal público com chat IA
├── Proxy Service        # Orquestração IA ↔ Técnico
├── Bot-IA              # Serviço de Inteligência Artificial
└── Banco de Dados      # SQL Server com dados do sistema
```

## 🎯 Módulos do Sistema

### 1. 📚 FAQ PÚBLICO (Portal de Atendimento)
**URL**: http://localhost:5030  
**Acesso**: Público (sem login necessário)

#### Recursos do FAQ:
- **Portal Público**: Interface acessível a qualquer usuário
- **Base de Conhecimento**: Perguntas frequentes organizadas por categoria
- **Busca Inteligente**: Localização rápida de informações
- **Interface Responsiva**: Adaptável a desktop, tablet e mobile
- **Design Moderno**: Identidade visual consistente

#### Chat Widget Integrado:
- **Posicionamento**: Canto inferior direito (não intrusivo)
- **Tamanho Otimizado**: 420x600px (desktop), responsivo (mobile)
- **Estados Visuais**: Minimizado, expandido, loading
- **Persistência**: Mantém conversa durante navegação

### 2. 💬 CHAT INTELIGENTE (Sistema de Conversação)
**Integrado**: FAQ Público + Core Administrativo  
**Tecnologia**: IA Google Gemini + Proxy Service

#### Recursos do Chat:
- **IA-First Approach**: Assistente virtual responde primeiro
- **Identificação de Usuário**: Nome e email obrigatórios
- **Histórico de Conversa**: Mantém contexto da sessão
- **Estados de Chat**: AI_ACTIVE, ESCALATION_PENDING, HUMAN_ASSIGNED
- **Polling em Tempo Real**: Mensagens instantâneas (3s)

#### Funcionalidades da IA:
- **Base de Conhecimento**: Integrada com FAQs do sistema
- **Personalidade Amigável**: Tom conversacional e prestativo
- **Respostas Contextuais**: Baseadas na pergunta específica
- **Confidence Score**: Avalia qualidade da resposta (0-100%)
- **Sem Emojis**: Comunicação profissional e limpa

#### Sistema de Escalação:
- **Escalação Inteligente**: Baseada em múltiplos critérios
- **Confirmação Obrigatória**: "Você deseja falar com um técnico?"
- **Botões de Ação**: Sim/Não para confirmar escalação
- **Transferência Suave**: Contexto preservado para técnico
- **Notificação Visual**: Loading discreto no topo do chat

#### Critérios de Escalação:
- **Keywords Específicas**: "técnico", "humano", "pessoa", "atendente"
- **Baixa Confiança**: Respostas com confidence < 40%
- **Muitas Tentativas**: Após 10 interações sem resolução
- **Timeout**: Sessão inativa por mais de 5 minutos
- **Solicitação Direta**: Usuário pede explicitamente

#### Interface do Chat:
- **Header**: Título "Suporte MidTalk" com botão fechar
- **Loading Bar**: Indicador "IA está pensando..." no topo
- **Área de Mensagens**: Scroll automático, timestamps locais
- **Input Inteligente**: Auto-resize, máximo 120px altura
- **Botões de Escalação**: Aparecem quando necessário
- **Indicadores Visuais**: Confidence score, tipos de mensagem

### 3. 🏢 CORE ADMINISTRATIVO (Sistema Principal)
**URL**: http://localhost:5028  
**Acesso**: Requer login (credenciais específicas)

#### Módulos do Core:

##### 👥 Gestão de Usuários
- **CRUD Completo**: Criar, visualizar, editar, desativar usuários
- **Perfis de Acesso**: Administrador, Técnico, Usuário
- **Informações Pessoais**: Nome, email, telefone, cargo
- **Controle de Status**: Ativo/Inativo
- **Histórico de Ações**: Log de atividades do usuário
- **Reset de Senhas**: Funcionalidade administrativa
- **Validações**: Email único, campos obrigatórios

##### 🎫 Sistema de Chamados
- **Abertura de Chamados**: Interface intuitiva com campos obrigatórios
- **Gestão de Status**: Aberto, Em Andamento, Resolvido, Fechado
- **Atribuição**: Manual ou automática para técnicos
- **Priorização**: Baixa, Média, Alta, Crítica (com SLA)
- **Categorização**: Tipos de problema organizados
- **Histórico Completo**: Timeline de todas as interações
- **Anexos**: Upload de arquivos e imagens
- **Comentários**: Comunicação interna entre técnicos

##### 💬 Chat Administrativo
- **Interface Técnico**: Painel para atender chats escalados
- **Fila de Atendimento**: Chats aguardando resposta
- **Contexto Completo**: Histórico da conversa com IA
- **Resposta Rápida**: Templates de respostas comuns
- **Transferência**: Entre técnicos quando necessário
- **Status de Atendimento**: Online, Ocupado, Ausente
- **Métricas Individuais**: Tempo médio, satisfação

##### 📊 Relatórios e Dashboards
- **Dashboard Principal**: Visão geral em tempo real
- **Chamados por Status**: Gráficos de distribuição
- **Produtividade**: Métricas por técnico e período
- **Tempo de Resolução**: SLA e performance
- **Satisfação**: Avaliações dos usuários
- **Uso da IA**: Taxa de resolução vs escalação
- **Exportação**: Excel, PDF, CSV

##### ❓ Gestão de FAQs
- **CRUD de FAQs**: Criar, editar, desativar perguntas
- **Categorização**: Organização por temas
- **Editor Rico**: Formatação de texto avançada
- **Preview**: Visualização antes da publicação
- **Versionamento**: Histórico de alterações
- **Sincronização IA**: Atualização automática da base
- **Estatísticas**: FAQs mais acessadas

##### ⚙️ Configurações do Sistema
- **Configurações Gerais**: Nome da empresa, logo, cores
- **Parâmetros de IA**: Confidence threshold, max tentativas
- **Configurações de Email**: SMTP, templates
- **Backup**: Configuração automática
- **Logs do Sistema**: Auditoria e troubleshooting
- **Integrações**: APIs externas, webhooks

#### Recursos Administrativos:
- **Controle de Acesso**: Permissões por perfil
- **Auditoria**: Log de todas as ações
- **Backup Automático**: Proteção de dados
- **Monitoramento**: Status dos serviços
- **Configurações**: Personalização do sistema
- **Relatórios Gerenciais**: Métricas executivas

#### Interface do Core:
- **Layout Responsivo**: Bootstrap 5 + identidade visual
- **Sidebar**: Navegação principal com ícones
- **Breadcrumbs**: Localização na aplicação
- **Modais**: Ações rápidas sem sair da página
- **Tabelas Dinâmicas**: Ordenação, filtros, paginação
- **Formulários Inteligentes**: Validação em tempo real
- **Notificações**: Toast messages para feedback

## 🌟 Funcionalidades Principais

### 1. Sistema de Chamados
- **Criação de Chamados**: Interface intuitiva para abertura de solicitações
- **Gestão de Status**: Acompanhamento completo do ciclo de vida
- **Atribuição Automática**: Distribuição inteligente para técnicos
- **Histórico Completo**: Rastreamento de todas as interações
- **Priorização**: Sistema de prioridades (Baixa, Média, Alta, Crítica)

### 2. Chat Inteligente com IA
- **IA First**: Assistente virtual responde primeiro
- **Base de Conhecimento**: Integrada com FAQs do sistema
- **Escalação Inteligente**: Transferência automática quando necessário
- **Confirmação de Escalação**: Usuário confirma antes da transferência
- **Polling em Tempo Real**: Mensagens instantâneas

### 3. Portal FAQ Público
- **Acesso Público**: Disponível sem necessidade de login
- **Chat Widget**: Integrado em todas as páginas
- **Perguntas Frequentes**: Base de conhecimento organizada
- **Interface Responsiva**: Funciona em desktop e mobile

### 4. Sistema Administrativo
- **Gestão de Usuários**: CRUD completo com perfis de acesso
- **Gestão de FAQs**: Criação e edição de base de conhecimento
- **Relatórios**: Dashboards com métricas de atendimento
- **Configurações**: Personalização do sistema

## 👥 Perfis de Usuário

### 1. Administrador
- **Acesso Total**: Todas as funcionalidades do sistema
- **Gestão de Usuários**: Criar, editar, desativar usuários
- **Configurações**: Ajustes gerais do sistema
- **Relatórios Gerenciais**: Visão completa das métricas

### 2. Técnico
- **Gestão de Chamados**: Atender e resolver solicitações
- **Chat com Usuários**: Comunicação direta via sistema
- **Atualização de Status**: Controle do andamento dos chamados
- **Relatórios Técnicos**: Métricas de produtividade

### 3. Usuário Final (Público)
- **Abertura de Chamados**: Via sistema ou chat
- **Acompanhamento**: Status e histórico das solicitações
- **Chat com IA**: Suporte automatizado 24/7
- **FAQ**: Acesso à base de conhecimento

## 🤖 Sistema de Inteligência Artificial

### Características da IA
- **Modelo**: Google Gemini 2.0 Flash
- **Personalidade**: Amigável, prestativa e profissional
- **Base de Conhecimento**: Integrada com FAQs do sistema
- **Idioma**: Português brasileiro
- **Atualização**: Sincronização automática com base de dados

### Fluxo de Escalação
1. **IA Responde**: Primeira tentativa de resolução
2. **Análise de Confiança**: Avalia qualidade da resposta
3. **Detecção de Keywords**: Identifica solicitações de técnico
4. **Confirmação**: Pergunta se usuário quer falar com técnico
5. **Escalação**: Transfere para técnico humano se confirmado

### Critérios de Escalação
- **Keywords Específicas**: "técnico", "humano", "pessoa"
- **Baixa Confiança**: Respostas com confidence < 40%
- **Muitas Tentativas**: Após 10 interações sem resolução
- **Solicitação Direta**: Usuário pede explicitamente

## 🌐 URLs e Acessos dos Módulos

### 📚 FAQ Público
- **URL**: http://localhost:5030
- **Acesso**: Público (sem login)
- **Funcionalidade**: Portal de FAQs + Chat IA
- **Usuários**: Qualquer pessoa

### 🏢 Core Administrativo  
- **URL**: http://localhost:5028
- **Acesso**: Requer login
- **Funcionalidade**: Gestão completa do sistema
- **Usuários**: Administradores e Técnicos

### 🔧 APIs e Serviços (Interno)
- **Backend API**: http://localhost:5000
- **Proxy Service**: http://localhost:9000  
- **Bot IA**: http://localhost:8001
- **Banco SQL Server**: localhost:1433

### Credenciais Padrão (Core)
- **Email**: guilhermetts0@gmail.com
- **Senha**: admin123
- **Perfil**: Administrador

## 🔧 Configuração e Deploy

### Requisitos do Sistema
- **Docker**: Para containerização
- **Docker Compose**: Para orquestração
- **Portas**: 5000, 5028, 5030, 8001, 9000, 1433
- **API Key**: Google Gemini (configurada no .env)

### Scripts de Automação
- **start.sh**: Inicia toda a aplicação
- **stop.sh**: Para todos os serviços
- **change-ip.sh**: Altera IP de rede automaticamente

### Configuração de Rede
- **Arquivo**: `.env` na raiz do projeto
- **Variável Principal**: `HOST_IP`
- **Mudança de IP**: Script automatizado disponível

## 📊 Métricas e Relatórios

### Dashboards Disponíveis
- **Chamados por Status**: Abertos, em andamento, resolvidos
- **Produtividade por Técnico**: Chamados resolvidos por período
- **Tempo de Resolução**: Métricas de SLA
- **Satisfação do Cliente**: Avaliações dos atendimentos
- **Uso da IA**: Taxa de resolução automática vs escalação

### Relatórios Exportáveis
- **Excel**: Dados tabulares para análise
- **PDF**: Relatórios formatados para apresentação
- **Gráficos**: Visualizações interativas

## 🔒 Segurança e Conformidade

### LGPD (Lei Geral de Proteção de Dados)
- **Dados Pessoais**: Tratamento adequado conforme legislação
- **Consentimento**: Coleta transparente de informações
- **Direitos do Titular**: Acesso, correção, exclusão de dados
- **Segurança**: Criptografia e controle de acesso

### Controle de Acesso
- **Autenticação**: Login seguro com hash de senhas
- **Autorização**: Perfis de acesso diferenciados
- **Sessões**: Controle de tempo e segurança
- **Logs**: Auditoria de ações do sistema

## 🚀 Inovações Implementadas

### IA-First Approach
- **Primeira Resposta**: IA atende antes de técnicos
- **Aprendizado Contínuo**: Base de conhecimento atualizada
- **Escalação Inteligente**: Decisão baseada em múltiplos critérios
- **Experiência Fluida**: Transição suave IA → Humano

### Integração Completa
- **Sincronização Automática**: FAQs → Base de Conhecimento IA
- **Polling em Tempo Real**: Mensagens instantâneas
- **Estados de Chat**: Controle preciso do fluxo de atendimento
- **Confirmação de Escalação**: Usuário decide quando escalar

### Interface Moderna
- **Design Responsivo**: Funciona em todos os dispositivos
- **Chat Widget**: Integração não intrusiva
- **Loading Inteligente**: Feedback visual adequado
- **Identidade Visual**: Consistente em todo o sistema

## 📈 Benefícios Esperados

### Para a Empresa
- **Redução de Custos**: Menos chamados para técnicos humanos
- **Disponibilidade 24/7**: IA atende fora do horário comercial
- **Métricas Precisas**: Dashboards para tomada de decisão
- **Escalabilidade**: Sistema cresce com a demanda

### Para os Técnicos
- **Foco em Casos Complexos**: IA resolve questões simples
- **Contexto Completo**: Histórico da conversa com IA
- **Produtividade**: Menos interrupções desnecessárias
- **Satisfação**: Trabalho mais estratégico

### Para os Usuários
- **Resposta Imediata**: IA disponível instantaneamente
- **Resolução Rápida**: Muitas dúvidas resolvidas automaticamente
- **Escalação Fácil**: Acesso a técnicos quando necessário
- **Experiência Consistente**: Atendimento padronizado

## 🔄 Fluxo Típico de Uso

### Cenário 1: FAQ Público - Resolução pela IA
1. Usuário acessa portal FAQ público
2. Navega pelas perguntas frequentes ou abre chat widget
3. IA responde baseada na base de conhecimento integrada
4. Usuário fica satisfeito e encerra atendimento

### Cenário 2: Chat - Escalação para Técnico
1. Usuário faz pergunta complexa no chat
2. IA tenta responder mas confidence é baixa
3. Sistema pergunta "Você deseja falar com um técnico?"
4. Usuário confirma escalação clicando "Sim"
5. Chat é transferido para técnico no Core Administrativo
6. Técnico atende via interface administrativa

### Cenário 3: Core - Gestão Administrativa
1. Administrador/Técnico acessa Core Administrativo
2. Gerencia usuários, chamados, FAQs e configurações
3. Atende chats escalados pela IA
4. Gera relatórios e monitora métricas do sistema

### Cenário 4: Integração Completa
1. Usuário cria FAQ no Core Administrativo
2. Sistema sincroniza automaticamente com base da IA
3. IA passa a responder com nova informação
4. Usuários do FAQ público recebem respostas atualizadas

## 📝 Considerações para o Manual do Usuário

### Público-Alvo
- **Usuários Finais**: Foco na simplicidade e clareza
- **Técnicos**: Procedimentos operacionais detalhados
- **Administradores**: Configurações e gestão avançada

### Estrutura Sugerida
1. **Introdução**: Visão geral e objetivos
2. **Primeiros Passos**: Login e navegação básica
3. **Funcionalidades por Perfil**: Guias específicos
4. **Chat com IA**: Como usar e quando escalar
5. **Resolução de Problemas**: FAQ técnico
6. **Contatos**: Suporte e informações adicionais

### Pontos de Atenção
- **Screenshots**: Interfaces atualizadas
- **Fluxos Visuais**: Diagramas do processo de escalação
- **Exemplos Práticos**: Casos de uso reais
- **Troubleshooting**: Problemas comuns e soluções

---

**Versão**: 1.0  
**Data**: Outubro 2025  
**Desenvolvido por**: Equipe PIM - UNIP  
**Tecnologia**: ASP.NET Core + Google Gemini AI
