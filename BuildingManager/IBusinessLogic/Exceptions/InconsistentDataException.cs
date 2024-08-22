
namespace IBusinessLogic.Exceptions
{
    [Serializable]
    public class InconsistentDataException : Exception
    {
        public InconsistentDataException(string? message) : base(message)
        {

        }

    }
}