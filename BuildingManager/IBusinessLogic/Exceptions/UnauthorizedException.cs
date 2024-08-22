
namespace IBusinessLogic.Exceptions
{
    [Serializable]
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string? message) : base(message)
        {

        }

    }
}