using System;

namespace Sharper.StockImages.Exceptions
{
    public class NoValidServiceException : Exception
    {
        public NoValidServiceException()
        {
        }

        public NoValidServiceException(string message) : base(message)
        {
        }

        public NoValidServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}