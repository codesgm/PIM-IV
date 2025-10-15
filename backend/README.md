# Backend API - Sistema PIM

## Estrutura
```
backend/
├── Controllers/
│   ├── AuthController.cs      # Login/logout
│   └── UsuariosController.cs  # CRUD usuários
├── Models/
│   ├── Usuario.cs            # Entidade usuário
│   ├── DTOs/                 # Data Transfer Objects
│   └── Enums/                # StatusUsuario, PerfilAcesso
├── Data/
│   └── AppDbContext.cs       # Entity Framework context
└── Program.cs                # Configuração da aplicação
```

## Endpoints Principais
- `POST /api/auth/login` - Login (email/senha)
- `POST /api/auth/logout` - Logout
- `GET /api/usuarios` - Listar usuários
- `POST /api/usuarios` - Criar usuário
- `PUT /api/usuarios/{id}` - Editar usuário
- `DELETE /api/usuarios/{id}` - Deletar usuário

## Configuração
- **Porta**: 5000
- **Banco**: SQL Server (connection string no appsettings.json)
- **JWT**: Configurado para autenticação
- **CORS**: Habilitado para frontend

## Docker
- Dockerfile configurado para .NET 9.0
- Expõe porta 5000
- Conecta com SQL Server via docker-compose

## Credenciais Padrão
- Email: guilhermetts0@gmail.com
- Senha: admin123
- Perfil: Administrador
