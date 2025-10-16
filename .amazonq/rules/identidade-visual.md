# Identidade Visual - MidTalk

## 🎨 Conceito Visual

O MidTalk adota uma identidade visual **moderna, minimalista e profissional**, focada em:

- **Clareza e legibilidade** para facilitar o uso diário
- **Design limpo** com espaços em branco estratégicos
- **Hierarquia visual** bem definida
- **Consistência** em todos os componentes
- **Acessibilidade** e usabilidade

## 🎯 Filosofia de Design

### Minimalismo Funcional
- Elementos essenciais em destaque
- Redução de ruído visual
- Foco na experiência do usuário

### Profissionalismo
- Cores sóbrias e confiáveis
- Tipografia legível e moderna
- Interface corporativa

### Modernidade
- Tendências atuais de UI/UX
- Micro-interações suaves
- Design responsivo

## 🎨 Paleta de Cores

### Cores Primárias

| Cor | Hex | RGB | Uso |
|-----|-----|-----|-----|
| **Primary** | `#2563eb` | `rgb(37, 99, 235)` | Botões principais, links, elementos de destaque |
| **Primary Dark** | `#1d4ed8` | `rgb(29, 78, 216)` | Hover states, elementos ativos |

### Cores Secundárias

| Cor | Hex | RGB | Uso |
|-----|-----|-----|-----|
| **Secondary** | `#64748b` | `rgb(100, 116, 139)` | Textos secundários, badges |
| **Success** | `#10b981` | `rgb(16, 185, 129)` | Status positivos, confirmações |
| **Danger** | `#ef4444` | `rgb(239, 68, 68)` | Alertas, exclusões, erros |
| **Warning** | `#f59e0b` | `rgb(245, 158, 11)` | Avisos, informações importantes |

### Cores Neutras

| Cor | Hex | RGB | Uso |
|-----|-----|-----|-----|
| **Light Gray** | `#f8fafc` | `rgb(248, 250, 252)` | Background principal, áreas de destaque |
| **Medium Gray** | `#e2e8f0` | `rgb(226, 232, 240)` | Bordas, separadores |
| **Dark Gray** | `#334155` | `rgb(51, 65, 85)` | Textos escuros |

### Cores de Texto

| Cor | Hex | RGB | Uso |
|-----|-----|-----|-----|
| **Text Primary** | `#1e293b` | `rgb(30, 41, 59)` | Títulos, textos principais |
| **Text Secondary** | `#64748b` | `rgb(100, 116, 139)` | Subtítulos, textos auxiliares |
| **Border Color** | `#e2e8f0` | `rgb(226, 232, 240)` | Bordas de elementos |

## 🔤 Tipografia

### Fonte Principal
- **Família**: Inter (Google Fonts)
- **Fallback**: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif
- **Pesos**: 400 (Regular), 500 (Medium), 600 (Semibold), 700 (Bold)

### Hierarquia Tipográfica
- **H1**: 2rem, font-weight: 700 (Títulos principais)
- **H2**: 1.5rem, font-weight: 600 (Subtítulos)
- **Body**: 1rem, font-weight: 400 (Texto padrão)
- **Small**: 0.875rem, font-weight: 400 (Textos auxiliares)

## 🎭 Componentes Visuais

### Sombras
- **Shadow SM**: `0 1px 2px 0 rgb(0 0 0 / 0.05)` - Elementos sutis
- **Shadow MD**: `0 4px 6px -1px rgb(0 0 0 / 0.1), 0 2px 4px -2px rgb(0 0 0 / 0.1)` - Cards
- **Shadow LG**: `0 10px 15px -3px rgb(0 0 0 / 0.1), 0 4px 6px -4px rgb(0 0 0 / 0.1)` - Modais

### Bordas
- **Radius**: 8px (padrão), 12px (cards), 16px (elementos especiais)
- **Border**: 1px solid var(--border-color)

### Espaçamentos
- **Base**: 1rem (16px)
- **Pequeno**: 0.5rem (8px)
- **Médio**: 1.5rem (24px)
- **Grande**: 2rem (32px)
- **Extra Grande**: 3rem (48px)

## 🎨 Aplicação das Cores

### Estados dos Elementos

| Estado | Cor | Uso |
|--------|-----|-----|
| **Normal** | Primary | Botões, links |
| **Hover** | Primary Dark | Interações |
| **Active** | Primary Dark | Elementos ativos |
| **Disabled** | Medium Gray | Elementos desabilitados |

### Badges e Status

| Status | Cor | Contexto |
|--------|-----|----------|
| **Ativo** | Success | Usuários ativos |
| **Inativo** | Danger | Usuários inativos |
| **Administrador** | Primary | Perfil de acesso |
| **Técnico** | Secondary | Perfil de acesso |
| **Info** | Warning | Informações gerais |

## 🖼️ Ícones

- **Biblioteca**: Font Awesome 6.4.0
- **Estilo**: Solid (fas)
- **Tamanhos**: 0.8rem, 1rem, 1.5rem, 3rem
- **Cor**: Herda do elemento pai ou cores específicas do contexto

## 📱 Responsividade

### Breakpoints
- **Mobile**: < 768px
- **Tablet**: 768px - 1024px
- **Desktop**: > 1024px

### Adaptações
- **Mobile**: Padding reduzido, botões maiores
- **Tablet**: Layout intermediário
- **Desktop**: Layout completo com sidebar

## 🎯 Diretrizes de Uso

### ✅ Fazer
- Usar cores da paleta oficial
- Manter consistência visual
- Respeitar hierarquia tipográfica
- Aplicar espaçamentos padronizados

### ❌ Evitar
- Cores fora da paleta
- Misturar diferentes pesos de fonte
- Espaçamentos inconsistentes
- Elementos sem contraste adequado

## 🔧 Implementação Técnica

### CSS Variables
```css
:root {
  --primary-color: #2563eb;
  --primary-dark: #1d4ed8;
  --secondary-color: #64748b;
  --success-color: #10b981;
  --danger-color: #ef4444;
  --warning-color: #f59e0b;
  --light-gray: #f8fafc;
  --medium-gray: #e2e8f0;
  --dark-gray: #334155;
  --text-primary: #1e293b;
  --text-secondary: #64748b;
  --border-color: #e2e8f0;
}
```

### Arquivos
- **CSS Principal**: `/wwwroot/css/site.css`
- **Layout**: `/Views/Shared/_Layout.cshtml`
- **Componentes**: Seguem as classes Bootstrap 5 + customizações

---

*Esta identidade visual foi desenvolvida para o MidTalk - Sistema de Gestão de Chamados e Suporte Técnico, priorizando usabilidade, profissionalismo e modernidade.*
