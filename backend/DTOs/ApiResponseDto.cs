namespace PimApi.DTOs
{
    public class ApiResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public int? TotalRecords { get; set; }
        public int? Page { get; set; }
        
        public static ApiResponseDto<T> SuccessResult(T data, string message = "")
        {
            return new ApiResponseDto<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }
        
        public static ApiResponseDto<T> ErrorResult(string message)
        {
            return new ApiResponseDto<T>
            {
                Success = false,
                Message = message
            };
        }
    }
}
