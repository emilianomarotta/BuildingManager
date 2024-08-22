﻿namespace IImporter.Exceptions
{
    [Serializable]
    public class InvalidFormatException : Exception
    {

        public InvalidFormatException(string? message) : base(message)
        {

        }

    }
}