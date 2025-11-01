# Diagrama de Classes - Sistema MidTalk

## Visão Geral

Este documento apresenta o diagrama de classes do Sistema MidTalk - Gestão de Chamados e Suporte Técnico com IA. O diagrama está organizado em camadas seguindo a arquitetura do sistema implementado.

## Arquitetura em Camadas

O sistema segue uma arquitetura em camadas bem definida:

- **Camada de Apresentação**: Controllers e ViewModels
- **Camada de Aplicação**: Services e DTOs  
- **Camada de Domínio**: Models e Entities
- **Camada de Infraestrutura**: Data Access e External Services

---

## Diagrama de Classes Principal

```mermaid
classDiagram
    %% ===== CAMADA DE DOMÍNIO (MODELS) =====
    
    %% Entidade principal que representa técnicos e administradores do sistema
    class Usuario {
        +int Id
        +string Nome
        +string Email
        +string Senha
        +StatusUsuario Status
        +PerfilAcesso PerfilAcesso
        +DateTime DataCadastro
        +DateTime? DataUltimaModificacao
        +List~Chat~ ChatsAtribuidos
        +List~ChatMessage~ MensagensEnviadas
    }
    
    %% Entidade que representa um chamado de suporte técnico criado via FAQ ou escalação
    class Chat {
        +int Id
        +string UserName
        +string UserContact
        +string UserEmail
        +string InitialMessage
        +ChatStatus Status
        +string Source
        +int? AssignedTechnicianId
        +Usuario? AssignedTechnician
        +DateTime CreatedAt
        +DateTime UpdatedAt
        +DateTime? ResolvedAt
        +List~ChatMessage~ Messages
        +int GetMessagesCount()
        +void UpdateStatus(ChatStatus newStatus)
        +bool CanBeAssignedTo(Usuario technician)
    }
    
    %% Representa uma mensagem individual dentro de um chat (usuário ou técnico)
    class ChatMessage {
        +int Id
        +int ChatId
        +Chat Chat
        +string SenderType
        +int? SenderId
        +Usuario? Sender
        +string Message
        +DateTime CreatedAt
        +bool IsFromUser()
        +bool IsFromTechnician()
    }
    
    %% Entidade que armazena perguntas frequentes exibidas no portal público
    class Faq {
        +int Id
        +string Pergunta
        +string Resposta
        +CategoriaFaq Categoria
        +bool Ativo
        +DateTime DataCriacao
        +DateTime? DataAtualizacao
        +void Ativar()
        +void Desativar()
        +bool MatchesSearch(string term)
    }
    
    %% ===== ENUMERAÇÕES =====
    
    %% Define os possíveis status de um usuário no sistema
    class StatusUsuario {
        <<enumeration>>
        Ativo
        Inativo
    }
    
    %% Define os níveis de acesso dos usuários no sistema
    class PerfilAcesso {
        <<enumeration>>
        Administrador
        Tecnico
    }
    
    %% Define os possíveis status de um chamado durante seu ciclo de vida
    class ChatStatus {
        <<enumeration>>
        Active
        Resolved
        Closed
    }
    
    %% Define as categorias para organização das perguntas frequentes
    class CategoriaFaq {
        <<enumeration>>
        GestaoContas
        Relatorios
        Configuracoes
        ProblemasTecnicos
    }
    
    %% ===== CAMADA DE APLICAÇÃO (SERVICES) =====
    
    %% Interface que define contrato para operações de gestão de usuários
    class IUsuarioService {
        <<interface>>
        +Task~List~UsuarioResponseDto~~ ListarUsuariosAsync()
        +Task~UsuarioResponseDto?~ ObterUsuarioPorIdAsync(int id)
        +Task~UsuarioResponseDto~ CriarUsuarioAsync(CadastroUsuarioRequestDto request)
        +Task~UsuarioResponseDto~ AtualizarUsuarioAsync(int id, EditarUsuarioRequestDto request)
        +Task~bool~ DesativarUsuarioAsync(int id)
    }
    
    %% Implementação concreta da lógica de negócio para gestão de usuários técnicos
    class UsuarioService {
        -AppDbContext _context
        +Task~List~UsuarioResponseDto~~ ListarUsuariosAsync()
        +Task~UsuarioResponseDto?~ ObterUsuarioPorIdAsync(int id)
        +Task~UsuarioResponseDto~ CriarUsuarioAsync(CadastroUsuarioRequestDto request)
        +Task~UsuarioResponseDto~ AtualizarUsuarioAsync(int id, EditarUsuarioRequestDto request)
        +Task~bool~ DesativarUsuarioAsync(int id)
        -string GerarHashSenha(string senha)
        -bool ValidarEmail(string email)
    }
    
    %% Interface que define contrato para operações de autenticação e autorização
    class IAuthService {
        <<interface>>
        +Task~LoginResponseDto?~ LoginAsync(LoginRequestDto request)
        +string GerarToken(Usuario usuario)
        +bool ValidarToken(string token)
    }
    
    %% Implementação do serviço de autenticação com JWT e validação de credenciais
    class AuthService {
        -AppDbContext _context
        -IConfiguration _configuration
        +Task~LoginResponseDto?~ LoginAsync(LoginRequestDto request)
        +string GerarToken(Usuario usuario)
        +bool ValidarToken(string token)
        -bool VerificarSenha(string senha, string hash)
    }
    
    %% Interface para algoritmo de atribuição inteligente de chamados a técnicos
    class IChatAssignmentService {
        <<interface>>
        +Task~Usuario?~ SelectBestTechnicianAsync()
        +Task~bool~ AssignChatAsync(int chatId, int technicianId)
        +Task~List~Usuario~~ GetAvailableTechniciansAsync()
    }
    
    %% Implementação do algoritmo de balanceamento de carga para atribuição de chamados
    class ChatAssignmentService {
        -AppDbContext _context
        +Task~Usuario?~ SelectBestTechnicianAsync()
        +Task~bool~ AssignChatAsync(int chatId, int technicianId)
        +Task~List~Usuario~~ GetAvailableTechniciansAsync()
        -int CalculateWorkload(Usuario technician)
        -bool IsTechnicianAvailable(Usuario technician)
    }
    
    %% ===== CAMADA DE APRESENTAÇÃO (CONTROLLERS) =====
    
    %% Controller REST para endpoints de gestão de usuários técnicos
    class UsuariosController {
        -IUsuarioService _usuarioService
        +Task~IActionResult~ Get()
        +Task~IActionResult~ Get(int id)
        +Task~IActionResult~ Post(CadastroUsuarioRequestDto request)
        +Task~IActionResult~ Put(int id, EditarUsuarioRequestDto request)
        +Task~IActionResult~ Delete(int id)
    }
    
    %% Controller REST para endpoints de gestão de chamados e mensagens


    
    %% Controller REST para endpoints de gestão de perguntas frequentes
    class FaqController {
        -AppDbContext _context
        +Task~IActionResult~ GetFaqs()
        +Task~IActionResult~ GetFaq(int id)
        +Task~IActionResult~ SearchFaqs(string term)
        +Task~IActionResult~ GetFaqsByCategory(CategoriaFaq category)
        +Task~IActionResult~ CreateFaq(CreateFaqRequestDto request)
        +Task~IActionResult~ UpdateFaq(int id, UpdateFaqRequestDto request)
    }
    
    %% Controller REST para endpoints de autenticação e autorização
    class AuthController {
        -IAuthService _authService
        +Task~IActionResult~ Login(LoginRequestDto request)
        +IActionResult Logout()
        +IActionResult ValidateToken()
    }
    
    %% ===== DTOs (DATA TRANSFER OBJECTS) =====
    
    %% DTO para receber dados de login do usuário
    class LoginRequestDto {
        +string Email
        +string Senha
    }
    
    %% DTO para retornar dados de login bem-sucedido com token JWT
    class LoginResponseDto {
        +string Token
        +UsuarioResponseDto Usuario
    }
    
    %% DTO para retornar dados públicos de um usuário (sem senha)
    class UsuarioResponseDto {
        +int Id
        +string Nome
        +string Email
        +string Status
        +string PerfilAcesso
        +DateTime DataCadastro
    }
    
    %% DTO para receber dados de cadastro de novo usuário técnico
    class CadastroUsuarioRequestDto {
        +string Nome
        +string Email
        +PerfilAcesso PerfilAcesso
    }
    
    %% DTO para receber dados de edição de usuário existente
    class EditarUsuarioRequestDto {
        +string Nome
        +string Email
        +StatusUsuario Status
        +PerfilAcesso PerfilAcesso
    }
    
    %% DTO para retornar dados completos de um chamado
    class ChatResponseDto {
        +int Id
        +string UserName
        +string UserContact
        +string UserEmail
        +string InitialMessage
        +string Status
        +string Source
        +int? AssignedTechnicianId
        +string? AssignedTechnicianName
        +DateTime CreatedAt
        +DateTime UpdatedAt
        +DateTime? ResolvedAt
        +int MessagesCount
    }
    
    %% DTO para receber dados de criação de novo chamado
    class CreateChatRequestDto {
        +string UserName
        +string UserEmail
        +string UserContact
        +string InitialMessage
    }
    
    %% DTO para retornar dados de uma pergunta frequente
    class FaqResponseDto {
        +int Id
        +string Pergunta
        +string Resposta
        +string Categoria
        +bool Ativo
        +DateTime DataCriacao
    }
    
    %% DTO genérico para padronizar respostas da API com sucesso/erro
    class ApiResponseDto~T~ {
        +bool Success
        +string Message
        +T? Data
        +List~string~ Errors
    }
    
    %% ===== CAMADA DE INFRAESTRUTURA =====
    
    %% Contexto do Entity Framework Core para acesso ao banco SQL Server
    class AppDbContext {
        +DbSet~Usuario~ Usuarios
        +DbSet~Chat~ Chats
        +DbSet~ChatMessage~ ChatMessages
        +DbSet~Faq~ Faqs
        +void OnModelCreating(ModelBuilder modelBuilder)
        +Task~int~ SaveChangesAsync()
    }
    
    %% ===== RELACIONAMENTOS =====
    
    %% Relacionamentos de Domínio
    Usuario ||--o{ Chat : "AssignedTechnician"
    Usuario ||--o{ ChatMessage : "Sender"
    Chat ||--o{ ChatMessage : "Messages"
    Usuario }o--|| StatusUsuario : "Status"
    Usuario }o--|| PerfilAcesso : "PerfilAcesso"
    Chat }o--|| ChatStatus : "Status"
    Faq }o--|| CategoriaFaq : "Categoria"
    
    %% Relacionamentos de Serviços
    IUsuarioService <|.. UsuarioService : implements
    IAuthService <|.. AuthService : implements
    IChatAssignmentService <|.. ChatAssignmentService : implements
    
    %% Dependências dos Serviços
    UsuarioService --> AppDbContext : uses
    AuthService --> AppDbContext : uses
    ChatAssignmentService --> AppDbContext : uses
    
    %% Dependências dos Controllers
    UsuariosController --> IUsuarioService : uses
    ChatsController --> IChatAssignmentService : uses
    ChatsController --> AppDbContext : uses
    FaqController --> AppDbContext : uses
    AuthController --> IAuthService : uses
    
    %% Relacionamentos com DTOs
    UsuarioService --> UsuarioResponseDto : creates
    UsuarioService --> CadastroUsuarioRequestDto : uses
    UsuarioService --> EditarUsuarioRequestDto : uses
    AuthService --> LoginResponseDto : creates
    AuthService --> LoginRequestDto : uses
    
    %% DbContext relacionamentos
    AppDbContext --> Usuario : manages
    AppDbContext --> Chat : manages
    AppDbContext --> ChatMessage : manages
    AppDbContext --> Faq : manages
```

---

## Descrição das Classes Principais

### **Camada de Domínio**

#### Usuario
Representa os usuários técnicos do sistema (Administradores e Técnicos).

**Responsabilidades**:
- Armazenar dados de identificação e autenticação
- Controlar perfil de acesso e status
- Manter relacionamento com chats atribuídos

**Atributos Principais**:
- `Id`: Identificador único
- `Nome`: Nome completo do usuário
- `Email`: Email único para login
- `Senha`: Hash da senha para autenticação
- `Status`: Ativo ou Inativo
- `PerfilAcesso`: Administrador ou Técnico

#### Chat
Representa um chamado de suporte técnico.

**Responsabilidades**:
- Armazenar dados do usuário final
- Controlar status do atendimento
- Manter relacionamento com técnico responsável
- Gerenciar histórico de mensagens

**Atributos Principais**:
- `Id`: Identificador único do chamado
- `UserName/UserEmail`: Dados do usuário final
- `InitialMessage`: Primeira mensagem do usuário
- `Status`: Active, Resolved, Closed
- `AssignedTechnician`: Técnico responsável

#### ChatMessage
Representa uma mensagem dentro de um chat.

**Responsabilidades**:
- Armazenar conteúdo da mensagem
- Identificar remetente (usuário ou técnico)
- Manter timestamp da mensagem

#### Faq
Representa uma pergunta frequente no sistema.

**Responsabilidades**:
- Armazenar pergunta e resposta
- Categorizar por tipo de problema
- Controlar status ativo/inativo

### **Camada de Aplicação**

#### UsuarioService
Implementa a lógica de negócio para gestão de usuários.

**Responsabilidades**:
- CRUD completo de usuários
- Validação de regras de negócio
- Transformação entre entidades e DTOs
- Geração de hash de senhas

#### AuthService
Gerencia autenticação e autorização.

**Responsabilidades**:
- Validação de credenciais
- Geração e validação de tokens JWT
- Controle de sessões

#### ChatAssignmentService
Implementa algoritmo de atribuição de chamados.

**Responsabilidades**:
- Seleção do melhor técnico disponível
- Balanceamento de carga de trabalho
- Verificação de disponibilidade

### **Camada de Apresentação**

#### Controllers
Expõem endpoints da API REST.

**Responsabilidades**:
- Receber requisições HTTP
- Validar dados de entrada
- Chamar serviços apropriados
- Retornar respostas padronizadas

### **Camada de Infraestrutura**

#### AppDbContext
Contexto do Entity Framework Core.

**Responsabilidades**:
- Mapeamento objeto-relacional
- Configuração de relacionamentos
- Execução de queries no banco
- Controle de transações

---

## Padrões de Design Aplicados

### **Repository Pattern**
- Implementado através do Entity Framework Core
- `AppDbContext` atua como Unit of Work
- `DbSet<T>` atua como Repository genérico

### **Service Layer Pattern**
- Camada de serviços encapsula lógica de negócio
- Interfaces definem contratos
- Implementações concretas isolam dependências

### **DTO Pattern**
- Objetos de transferência de dados
- Isolam modelo de domínio da API
- Facilitam versionamento e evolução

### **Dependency Injection**
- Inversão de controle via DI container
- Interfaces definem abstrações
- Facilita testes unitários

### **MVC Pattern**
- Controllers gerenciam requisições
- Models representam dados
- Views (no frontend) apresentam informação

---

## Relacionamentos Principais

### **Usuario ↔ Chat**
- **Tipo**: One-to-Many
- **Relacionamento**: Um técnico pode ter vários chats atribuídos
- **Chave Estrangeira**: `Chat.AssignedTechnicianId`
- **Navegação**: `Usuario.ChatsAtribuidos` ↔ `Chat.AssignedTechnician`

### **Chat ↔ ChatMessage**
- **Tipo**: One-to-Many
- **Relacionamento**: Um chat possui várias mensagens
- **Chave Estrangeira**: `ChatMessage.ChatId`
- **Navegação**: `Chat.Messages` ↔ `ChatMessage.Chat`

### **Usuario ↔ ChatMessage**
- **Tipo**: One-to-Many (opcional)
- **Relacionamento**: Um técnico pode enviar várias mensagens
- **Chave Estrangeira**: `ChatMessage.SenderId` (nullable)
- **Navegação**: `Usuario.MensagensEnviadas` ↔ `ChatMessage.Sender`

---

## Validações e Constraints

### **Usuario**
- `Email`: Único, formato válido, máximo 150 caracteres
- `Nome`: Obrigatório, máximo 100 caracteres
- `Senha`: Obrigatória, armazenada como hash
- `Status`: Enum válido (Ativo/Inativo)
- `PerfilAcesso`: Enum válido (Administrador/Técnico)

### **Chat**
- `UserName`: Obrigatório, máximo 100 caracteres
- `UserEmail`: Obrigatório, formato válido, máximo 255 caracteres
- `InitialMessage`: Obrigatória
- `Status`: Enum válido (Active/Resolved/Closed)
- `Source`: Máximo 20 caracteres, padrão "faq"

### **ChatMessage**
- `Message`: Obrigatória
- `SenderType`: Obrigatório, máximo 20 caracteres
- `ChatId`: Obrigatório, deve existir
- `SenderId`: Opcional, deve existir se informado

### **Faq**
- `Pergunta`: Obrigatória, máximo 500 caracteres
- `Resposta`: Obrigatória
- `Categoria`: Enum válido
- `Ativo`: Padrão true

---

## Considerações de Performance

### **Índices Recomendados**
- `Usuario.Email` (único)
- `Chat.AssignedTechnicianId`
- `Chat.Status`
- `Chat.CreatedAt`
- `ChatMessage.ChatId`
- `ChatMessage.CreatedAt`
- `Faq.Categoria`
- `Faq.Ativo`

### **Lazy Loading**
- Relacionamentos carregados sob demanda
- `Include()` explícito quando necessário
- Projeções para DTOs otimizam queries

### **Paginação**
- Implementada em listagens grandes
- `Skip()` e `Take()` para eficiência
- Contadores separados quando necessário

---

## Extensibilidade

### **Novos Tipos de Usuário**
- Adicionar valores ao enum `PerfilAcesso`
- Implementar validações específicas
- Ajustar controle de acesso

### **Novos Status de Chat**
- Adicionar valores ao enum `ChatStatus`
- Implementar transições válidas
- Atualizar lógica de negócio

### **Novas Categorias de FAQ**
- Adicionar valores ao enum `CategoriaFaq`
- Implementar filtros específicos
- Ajustar interface de usuário

### **Auditoria**
- Implementar interfaces `IAuditable`
- Adicionar campos de auditoria
- Interceptar mudanças no `SaveChanges()`

---

## Mapeamento para Banco de Dados

### **Tabelas Principais**
- `Usuarios` ← Usuario
- `Chats` ← Chat  
- `ChatMessages` ← ChatMessage
- `Faqs` ← Faq

### **Relacionamentos FK**
- `Chats.AssignedTechnicianId` → `Usuarios.Id`
- `ChatMessages.ChatId` → `Chats.Id`
- `ChatMessages.SenderId` → `Usuarios.Id`

### **Configurações EF Core**
```csharp
// Usuario
entity.HasIndex(u => u.Email).IsUnique();
entity.Property(u => u.Status).HasConversion<int>();
entity.Property(u => u.PerfilAcesso).HasConversion<int>();

// Chat
entity.HasOne(c => c.AssignedTechnician)
      .WithMany(u => u.ChatsAtribuidos)
      .HasForeignKey(c => c.AssignedTechnicianId);

// ChatMessage
entity.HasOne(m => m.Chat)
      .WithMany(c => c.Messages)
      .HasForeignKey(m => m.ChatId);
```

---

*Documento completo - Sistema MidTalk v1.0*  
*Total de Classes: 25+ classes principais*  
*Última atualização: 29/10/2024*
