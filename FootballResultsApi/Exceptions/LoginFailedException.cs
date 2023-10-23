namespace FootballResultsApi.Exceptions
{
    public class LoginFailedException : BaseException
    {
        public override int statusCode => 401;

        public LoginFailedException(string message)
            : base(message) { }
    }
}
