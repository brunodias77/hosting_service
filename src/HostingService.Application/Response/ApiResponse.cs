using System;
namespace HostingService.Application.Response
{
    public class ApiResponse<T>
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public T Data { get; private set; }

        // Construtor para sucesso
        public ApiResponse(T data, string message = "")
        {
            Success = true;
            Message = message;
            Data = data;
        }

        // Construtor para falha
        public ApiResponse(string message)
        {
            Success = false;
            Message = message;
            Data = default(T);
        }

        // Métodos estáticos de fábrica para facilitar a criação de respostas
        public static ApiResponse<T> SuccessResponse(T data, string message = "")
        {
            return new ApiResponse<T>(data, message);
        }

        public static ApiResponse<T> FailureResponse(string message)
        {
            return new ApiResponse<T>(message);
        }
    }
}

