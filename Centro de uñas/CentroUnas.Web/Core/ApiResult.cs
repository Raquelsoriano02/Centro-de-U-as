namespace CentroUnas.Web.Core
{
    public class ApiResult<T>
    {
        public bool Success { get; private set; }
        public string? Message { get; private set; }
        public T? Data { get; private set; }

        public static ApiResult<T> Ok(T? data)
        {
            return new ApiResult<T> { Success = true, Data = data };
        }

        public static ApiResult<T> Fail(string message)
        {
            return new ApiResult<T> { Message = message };
        }
    }
}
