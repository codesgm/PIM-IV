# Frontend Web - Sistema PIM

## Estrutura
```
frontend/
├── Controllers/
│   ├── AuthController.cs     # Login/logout
│   └── HomeController.cs     # Dashboard
├── Services/
│   ├── ApiService.cs         # HTTP client para API
│   ├── AuthService.cs        # Gerenciamento de sessão
│   └── UsuarioService.cs     # CRUD usuários
├── Models/
│   ├── DTOs/                 # Data Transfer Objects
│   ├── ViewModels/           # ViewModels para views
│   └── Enums/                # StatusUsuario, PerfilAcesso
├── Views/
│   ├── Auth/Login.cshtml     # Página de login
│   └── Home/Index.cshtml     # Dashboard
└── Program.cs                # Configuração MVC + sessão
```

## Funcionalidades
- **Login/Logout**: Autenticação via API
- **Dashboard**: Página inicial pós-login
- **Gerenciamento de Usuários**: CRUD completo
- **Sessão**: 8 horas de duração
- **Responsivo**: Bootstrap 5.1.3

## Configuração
- **Porta**: 5028 (host) → 8080 (container)
- **API**: Configurável via appsettings.json (ApiSettings:BaseUrl)
- **Sessão**: Armazena JWT token
- **Cores**: Paleta azul (#03045e a #caf0f8)

## Docker
- Dockerfile configurado para .NET 9.0
- Mapeia porta 5028:8080
- Conecta com API via rede docker (api:5000)

## Acesso
- URL: http://localhost:5028
- Credenciais: guilhermetts0@gmail.com / admin123
