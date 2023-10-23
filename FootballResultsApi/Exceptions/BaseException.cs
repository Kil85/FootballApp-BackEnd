namespace FootballResultsApi.Exceptions
{
    public abstract class BaseException : Exception
    {
        public abstract int statusCode { get; }

        public BaseException(string message)
            : base(message) { }
    }
}
