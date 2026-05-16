namespace Demo.Domain.Exceptions
{
    public class ConflictException : Exception
    {
        public readonly string[] Errors;

        public ConflictException(string error)
        {
            Errors = [error];
        }

        public ConflictException(string[] errors)
        {
            Errors = errors;
        }
    }
}
