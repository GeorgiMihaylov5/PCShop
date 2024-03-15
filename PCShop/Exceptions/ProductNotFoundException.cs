namespace PCShop.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        private const string message = "Cannot find the product!";

        public ProductNotFoundException() : base(message)
        {

        }

        public ProductNotFoundException(string externalMessage) : base(externalMessage)
        {

        }
    }
}
