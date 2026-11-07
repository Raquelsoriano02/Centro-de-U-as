namespace CentroUnas.Infrastructure.Exceptions
{
    public class TurnoException : Exception
    {
        public TurnoException()
        {
        }

        public TurnoException(string? message) : base(message)
        {
        }

        public TurnoException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
