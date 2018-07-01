namespace ZopaLoans.Sys.Exceptions
{
    public class InsufficientOffersAmountException : ZopaLoansException
    {
        public InsufficientOffersAmountException(string message) : base(message)
        {
        }
    }
}