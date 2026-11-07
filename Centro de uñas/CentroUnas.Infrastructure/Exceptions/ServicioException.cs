namespace CentroUnas.Infrastructure.Exceptions
{
    public class ServicioException : Exception
    {
        public ServicioException()
        {
        }

        public ServicioException(string? message) : base(message)
        {
        }

        public ServicioException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
