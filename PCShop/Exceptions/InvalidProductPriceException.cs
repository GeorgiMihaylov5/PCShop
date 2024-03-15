namespace PCShop.Exceptions
{
    public class InvalidProductPriceException : Exception
    {
        private const string message = "Price must be more than 0";

        public InvalidProductPriceException() : base(message)
        {

        }

        public InvalidProductPriceException(string externalMessage) : base(externalMessage)
        {

        }
    }
}
