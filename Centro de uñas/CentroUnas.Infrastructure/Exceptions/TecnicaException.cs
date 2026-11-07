namespace CentroUnas.Infrastructure.Exceptions
{
    public class TecnicaException : Exception
    {
        public TecnicaException()
        {
        }

        public TecnicaException(string? message) : base(message)
        {
        }

        public TecnicaException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
