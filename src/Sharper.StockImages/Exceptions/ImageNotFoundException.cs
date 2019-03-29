using System;

namespace Sharper.StockImages.Exceptions
{
    public class ImageNotFoundException : Exception
    {
        public string ImageId { get; set; }

        public string[] ServiceErrors { get; set; }

        public ImageNotFoundException()
        {
        }

        public ImageNotFoundException(string message) : base(message)
        {
        }

        public ImageNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}