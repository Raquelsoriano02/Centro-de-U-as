namespace CentroUnas.Infrastructure.Exceptions
{
    public class HorarioException : Exception
    {
        public HorarioException()
        {
        }

        public HorarioException(string? message) : base(message)
        {
        }

        public HorarioException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
