using Newtonsoft.Json;
using PimWeb.Models;

namespace PimWeb.Services
{
    public class AuthService
    {
        private readonly ApiService _apiService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(ApiService apiService, IHttpContextAccessor httpContextAccessor)
        {
            _apiService = apiService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponseDto<LoginResponseDto>?> LoginAsync(LoginRequestDto loginRequest)
        {
            return await _apiService.PostAsync<LoginResponseDto>("api/auth/login", loginRequest);
        }

        public void SaveUserSession(LoginResponseDto loginResponse)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                session.SetString("Token", loginResponse.Token);
                session.SetString("UserData", JsonConvert.SerializeObject(loginResponse.Usuario));
            }
        }

        public void Logout()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.Clear();
        }

        public bool IsAuthenticated()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("Token");
            return !string.IsNullOrEmpty(token);
        }

        public UsuarioResponseDto? GetCurrentUser()
        {
            var userData = _httpContextAccessor.HttpContext?.Session.GetString("UserData");
            if (!string.IsNullOrEmpty(userData))
            {
                return JsonConvert.DeserializeObject<UsuarioResponseDto>(userData);
            }
            return null;
        }

        public bool IsAdmin()
        {
            var user = GetCurrentUser();
            return user?.PerfilAcesso == PerfilAcesso.Administrador;
        }

        public bool IsTechnician()
        {
            var user = GetCurrentUser();
            return user?.PerfilAcesso == PerfilAcesso.Tecnico;
        }
    }
}
