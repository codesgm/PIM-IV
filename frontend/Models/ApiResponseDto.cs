namespace PimWeb.Models
{
    public class ApiResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public int? TotalRecords { get; set; }
        public int? Page { get; set; }
    }
}
