namespace PCShop.Exceptions
{
    public class InvalidDiscountException : Exception
    {
        private const string message = "Discount must be between 0 and 100!";

        public InvalidDiscountException() : base(message)
        {

        }

        public InvalidDiscountException(string externalMessage) : base(externalMessage)
        {

        }
    }
}
