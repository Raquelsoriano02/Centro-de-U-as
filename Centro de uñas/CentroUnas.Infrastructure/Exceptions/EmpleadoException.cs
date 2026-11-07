namespace CentroUnas.Infrastructure.Exceptions
{
    public class EmpleadoException : Exception
    {
        public EmpleadoException()
        {
        }

        public EmpleadoException(string? message) : base(message)
        {
        }

        public EmpleadoException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
