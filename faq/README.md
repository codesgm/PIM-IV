# FAQ - MidTalk

## 🎯 Objetivo
Site público de FAQ (Frequently Asked Questions) para suporte técnico, permitindo que usuários encontrem respostas para dúvidas comuns antes de abrir um chamado.

## 🛠️ Tecnologia Escolhida
**ASP.NET Core MVC (.NET 9.0)**

### Justificativa:
- **Consistência**: Mesma stack do frontend principal
- **Integração**: Consumirá o mesmo backend API
- **Performance**: Otimizado para sites públicos
- **SEO**: Renderização server-side para melhor indexação
- **Manutenção**: Equipe já familiarizada com a tecnologia

## 🏗️ Arquitetura Planejada

### Frontend FAQ (ASP.NET Core MVC)
- **Porta**: 5030
- **Público**: Acesso sem autenticação
- **Layout**: Design responsivo e otimizado para SEO
- **Funcionalidades**:
  - Busca de artigos
  - Categorização de dúvidas
  - Avaliação de utilidade
  - Sugestão de novos tópicos

### Backend (Compartilhado)
- **API**: Mesma API do sistema principal (porta 5000)
- **Endpoints**: Novos endpoints para FAQ
- **Dados**: Tabelas específicas para artigos e categorias

## 📊 Estrutura Planejada

```
faq/
├── Controllers/
│   ├── HomeController.cs      # Página inicial
│   ├── ArtigosController.cs   # Listagem e visualização
│   └── BuscaController.cs     # Sistema de busca
├── Models/
│   ├── ArtigoViewModel.cs     # Artigos do FAQ
│   ├── CategoriaViewModel.cs  # Categorias
│   └── BuscaViewModel.cs      # Resultados de busca
├── Views/
│   ├── Home/Index.cshtml      # Página inicial
│   ├── Artigos/               # Views dos artigos
│   └── Shared/_Layout.cshtml  # Layout público
├── Services/
│   └── FaqApiService.cs       # Consumo da API
└── wwwroot/                   # Assets públicos
```

## 🎨 Design
- **Identidade**: Baseada no MidTalk mas adaptada para público
- **Cores**: Paleta similar com foco em legibilidade
- **Layout**: Limpo e focado no conteúdo
- **Mobile**: Totalmente responsivo

## 🔗 Integração com Backend
- **Endpoints**: `/api/faq/artigos`, `/api/faq/categorias`
- **Dados**: Artigos, categorias, avaliações
- **Cache**: Implementação de cache para performance
- **Analytics**: Tracking de artigos mais acessados

## 📈 Funcionalidades Futuras
- **IA**: Sugestões automáticas baseadas em chamados
- **Analytics**: Métricas de uso e efetividade
- **Feedback**: Sistema de avaliação dos artigos
- **Integração**: Link direto para abertura de chamados

## 🚀 Próximos Passos
1. Criar projeto ASP.NET Core MVC
2. Configurar layout e identidade visual
3. Implementar endpoints no backend
4. Desenvolver sistema de busca
5. Adicionar ao docker-compose.yml
