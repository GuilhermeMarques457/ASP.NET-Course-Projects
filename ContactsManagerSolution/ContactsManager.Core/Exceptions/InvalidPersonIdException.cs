﻿namespace Exceptions
{
    public class InvalidPersonIdException : ArgumentException
    {
        public InvalidPersonIdException() : base()
        {
            
        }

        public InvalidPersonIdException(string? message) : base(message)
        {
            
        }

        public InvalidPersonIdException(string? messsage, Exception? innerException) : base(messsage, innerException)
        {
            
        }
    }
}