namespace PCShop.Exceptions
{
    public class OrderNotFoundException : Exception
    {
        private const string message = "Cannot find the order!";

        public OrderNotFoundException() : base(message)
        {

        }

        public OrderNotFoundException(string externalMessage) : base(externalMessage)
        {

        }
    }
}
