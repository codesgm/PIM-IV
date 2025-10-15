using Newtonsoft.Json;
using System.Text;
using PimWeb.Models;

namespace PimWeb.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            var baseUrl = configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5000";
            _httpClient.BaseAddress = new Uri($"{baseUrl}/");
        }

        private void SetAuthorizationHeader()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<ApiResponseDto<T>?> GetAsync<T>(string endpoint)
        {
            try
            {
                SetAuthorizationHeader();
                var response = await _httpClient.GetAsync(endpoint);
                var content = await response.Content.ReadAsStringAsync();
                
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<ApiResponseDto<T>>(content);
                }
                
                return new ApiResponseDto<T> 
                { 
                    Success = false, 
                    Message = $"Erro HTTP: {response.StatusCode}" 
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<T> 
                { 
                    Success = false, 
                    Message = $"Erro: {ex.Message}" 
                };
            }
        }

        public async Task<ApiResponseDto<T>?> PostAsync<T>(string endpoint, object data)
        {
            try
            {
                SetAuthorizationHeader();
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync(endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                return JsonConvert.DeserializeObject<ApiResponseDto<T>>(responseContent);
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<T> 
                { 
                    Success = false, 
                    Message = $"Erro: {ex.Message}" 
                };
            }
        }

        public async Task<ApiResponseDto<T>?> PutAsync<T>(string endpoint, object data)
        {
            try
            {
                SetAuthorizationHeader();
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PutAsync(endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                return JsonConvert.DeserializeObject<ApiResponseDto<T>>(responseContent);
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<T> 
                { 
                    Success = false, 
                    Message = $"Erro: {ex.Message}" 
                };
            }
        }

        public async Task<ApiResponseDto<object>?> DeleteAsync(string endpoint)
        {
            try
            {
                SetAuthorizationHeader();
                var response = await _httpClient.DeleteAsync(endpoint);
                var content = await response.Content.ReadAsStringAsync();
                
                return JsonConvert.DeserializeObject<ApiResponseDto<object>>(content);
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<object> 
                { 
                    Success = false, 
                    Message = $"Erro: {ex.Message}" 
                };
            }
        }
    }
}
