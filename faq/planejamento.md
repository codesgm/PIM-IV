# Planejamento - FAQ MinhasContas

## 🎯 Objetivo
Criar site público de FAQ para o sistema MinhasContas, seguindo o mesmo padrão de design do sistema interno, mas adaptado para usuários externos que buscam suporte.

## 🎨 Design e Layout

### Identidade Visual
- **Mesma paleta de cores** do sistema principal
- **Logo MinhasContas** no header
- **Tipografia Inter** mantida
- **Componentes** similares mas adaptados para público

### Layout Público
- **Header fixo** com logo e navegação simples
- **Hero section** com busca em destaque
- **Seções organizadas** por categorias
- **Footer** com informações de contato
- **Sem autenticação** necessária

## 📱 Estrutura da Página Inicial

### 1. Header
- Logo MinhasContas (completa)
- Menu: Início | Contato
- Botão "Acessar Sistema" (link para sistema interno)

### 2. Hero Section com Busca Principal
- Título: "Como podemos ajudar você?"
- Subtítulo: "Digite sua dúvida e encontre a resposta"
- **Barra de pesquisa** em destaque (grande e centralizada)
- Placeholder: "Ex: Como cadastrar uma nova conta?"
- **Busca por título** das perguntas
- Botão "Pesquisar" com ícone de lupa

### 3. Perguntas Frequentes (Seção Principal)
- Título: "Perguntas Frequentes"
- **Lista de perguntas** mais comuns
- **Formato accordion** (expandir/recolher respostas)
- **10-15 perguntas** principais
- Organizadas por relevância/popularidade

### 4. Ação de Contato (Simplificada)
- Seção compacta: "Não encontrou sua resposta?"
- Botão para **contatar suporte**
- Link para **acessar o sistema**

### 5. Footer (Minimalista)
- Copyright MinhasContas
- Links de contato

## 🛠️ Estrutura Técnica

### Projeto ASP.NET Core MVC
```
faq/
├── Controllers/
│   ├── HomeController.cs           # Página inicial com FAQ
│   └── BuscaController.cs          # Busca por perguntas
├── Models/
│   ├── HomeViewModel.cs            # Dados da home + FAQ
│   ├── PerguntaFrequenteViewModel.cs # Perguntas e respostas
│   └── BuscaViewModel.cs           # Resultados de busca
├── Views/
│   ├── Home/
│   │   └── Index.cshtml            # Página inicial com FAQ
│   └── Shared/
│       ├── _Layout.cshtml          # Layout público
│       └── _ViewStart.cshtml       # Configuração de layout
├── Services/
│   └── FaqApiService.cs            # Consumo da API (futuro)
├── wwwroot/
│   ├── css/
│   │   └── site.css                # Estilos customizados
│   ├── js/
│   │   └── faq.js                  # Scripts para accordion e busca
│   └── logo/                       # Logos (copiadas)
└── Program.cs                      # Configuração
```

### Configurações
- **Porta**: 5030
- **Layout**: Responsivo (mobile-first)
- **SEO**: Meta tags otimizadas
- **Performance**: CSS/JS minificados

## 🎨 Componentes Visuais

### Barra de Busca Principal
- **Input grande** e centralizado (hero section)
- **Ícone de lupa** integrado
- **Placeholder** sugestivo
- **Busca em tempo real** por título das perguntas
- **Resultados instantâneos** abaixo da barra

### Accordion de Perguntas Frequentes
- **Layout limpo** com bordas sutis
- **Ícone +/-** para expandir/recolher
- **Animação suave** de abertura/fechamento
- **Destaque** na pergunta ativa
- **Texto da resposta** bem formatado

### Resultados de Busca
- **Lista filtrada** das perguntas
- **Highlight** do termo pesquisado
- **Ordenação** por relevância
- **Mensagem** quando não encontrar resultados

## 📊 Dados Mockados (Inicial)

### Perguntas Frequentes (15 principais)

#### Gestão de Contas
1. **"Como cadastrar uma nova conta?"**
   - Resposta: Passo a passo para adicionar contas no sistema...

2. **"Como editar informações de uma conta existente?"**
   - Resposta: Instruções para modificar dados das contas...

3. **"Como excluir uma conta do sistema?"**
   - Resposta: Processo para remover contas indesejadas...

#### Relatórios
4. **"Como gerar relatório mensal de gastos?"**
   - Resposta: Tutorial para criar relatórios mensais...

5. **"Onde encontro o relatório de receitas?"**
   - Resposta: Localização e geração de relatórios de receita...

6. **"Como exportar relatórios para Excel?"**
   - Resposta: Processo de exportação de dados...

#### Configurações
7. **"Como alterar minha senha?"**
   - Resposta: Passo a passo para redefinir senha...

8. **"Como configurar categorias de gastos?"**
   - Resposta: Criação e organização de categorias...

9. **"Como fazer backup dos meus dados?"**
   - Resposta: Processo de backup e restauração...

#### Problemas Técnicos
10. **"O sistema não está carregando, o que fazer?"**
    - Resposta: Soluções para problemas de carregamento...

11. **"Problemas de sincronização de dados"**
    - Resposta: Como resolver falhas de sincronização...

12. **"Como limpar cache do navegador?"**
    - Resposta: Instruções para limpeza de cache...

#### Gerais
13. **"Como entrar em contato com o suporte?"**
    - Resposta: Canais de atendimento disponíveis...

14. **"Qual navegador é recomendado?"**
    - Resposta: Compatibilidade e recomendações...

15. **"Como recuperar dados perdidos?"**
    - Resposta: Procedimentos de recuperação...

## 🔗 Integração Futura

### Com Backend
- **Endpoints**: `/api/faq/categorias`, `/api/faq/artigos`
- **Busca**: `/api/faq/buscar`
- **Analytics**: Tracking de acessos

### Com Sistema Principal
- **Link "Abrir Chamado"** → Redirect para login
- **Dados compartilhados** via API
- **Sessão unificada** (futuro)

## 📱 Responsividade

### Mobile (< 768px)
- Menu hambúrguer
- Busca em tela cheia
- Cards empilhados
- Footer simplificado

### Tablet (768px - 1024px)
- Layout intermediário
- 2 cards por linha
- Navegação completa

### Desktop (> 1024px)
- Layout completo
- 4 cards por linha
- Sidebar (futuro)

## 🚀 Fases de Desenvolvimento

### Fase 1 (Atual)
- ✅ Estrutura do projeto
- ✅ Layout base
- ✅ Página inicial com dados mockados
- ✅ Design responsivo

### Fase 2 (Futura)
- Páginas de categoria
- Página de artigo individual
- Sistema de busca
- Integração com backend

### Fase 3 (Futura)
- Analytics e métricas
- Sistema de avaliação
- Sugestões automáticas
- SEO avançado

## ❓ Pontos para Revisão

1. **Design**: A identidade visual está alinhada?
2. **Estrutura**: A organização das seções faz sentido?
3. **Categorias**: As categorias mockadas são adequadas?
4. **Navegação**: O fluxo do usuário está claro?
5. **Responsividade**: Os breakpoints estão corretos?
6. **Integração**: A estratégia de integração está adequada?

---

**Aguardando revisão para prosseguir com a implementação do FAQ MinhasContas.** 🔍
