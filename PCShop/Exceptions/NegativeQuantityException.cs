namespace PCShop.Exceptions
{
    public class NegativeQuantityException : Exception
    {
        private const string message = "The quantity cannot be a negative number!";

        public NegativeQuantityException() : base(message)
        {
            
        }

        public NegativeQuantityException(string externalMessage) : base(externalMessage)
        {

        }
    }
}
