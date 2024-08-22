namespace Domain.Exceptions
{
    [Serializable]
    public class InvalidAttributeException : Exception
    {
        public InvalidAttributeException(string? message) : base(message)
        {

        }

    }
}