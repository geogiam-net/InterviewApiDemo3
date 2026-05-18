namespace Demo.Business.Exceptions
{
    public class NotFoundException : Exception
    {
        public readonly string[] Errors;

        public NotFoundException(string error)
        {
            Errors = [error];
        }

        public NotFoundException(string[] errors)
        {
            Errors = errors;
        }
    }
}
