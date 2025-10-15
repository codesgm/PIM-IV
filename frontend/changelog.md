# Changelog - Frontend Web

## 2025-01-14

### ✅ 1.1 Estrutura do Projeto - CONCLUÍDO
- Criado projeto ASP.NET Core MVC (.NET 8)
- Configurada estrutura de pastas (Controllers, Views, Models, Services, wwwroot)
- Adicionado pacote Newtonsoft.Json

### ✅ 1.2 Pacotes NuGet - CONCLUÍDO
- Adicionado Microsoft.AspNetCore.Session
- HttpClient já incluído no .NET 8

### ✅ 2.1 DTOs - CONCLUÍDO
- Criados todos os DTOs de request e response
- LoginRequestDto, CadastroUsuarioRequestDto, EditarUsuarioRequestDto
- UsuarioResponseDto, LoginResponseDto, ApiResponseDto

### ✅ 2.3 Enums - CONCLUÍDO
- Criados StatusUsuario e PerfilAcesso

### ✅ 2.2 ViewModels - CONCLUÍDO
- Criados LoginViewModel, UsuarioListViewModel, UsuarioCadastroViewModel, DashboardViewModel
- Incluídas validações e display names

### ✅ 3.1 ApiService Base - CONCLUÍDO
- Criado ApiService com HttpClient configurado
- Base URL apontando para API (localhost:5000)
- Métodos GET, POST, PUT, DELETE implementados
- Gerenciamento automático de token JWT via sessão
- Tratamento de erros HTTP

### Próximo: Criar AuthService e UsuarioService
