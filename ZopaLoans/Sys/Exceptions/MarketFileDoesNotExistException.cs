namespace ZopaLoans.Sys.Exceptions
{
    public class MarketFileDoesNotExistException : ZopaLoansException
    {
        public MarketFileDoesNotExistException(string message) : base(message)
        {
        }
    }
}