using System;
namespace HostingService.Application.Response
{
    public class BaseResponse<T> 
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string Error { get; set; } = "Algo deu errado...";

        public static BaseResponse<T> SuccessResponse(T data)
        {
            return new BaseResponse<T>
            {
                Success = true,
                Data = data,
                Error = string.Empty
            };
        }
    }
}

