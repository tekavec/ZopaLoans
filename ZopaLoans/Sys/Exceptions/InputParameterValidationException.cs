namespace ZopaLoans.Sys.Exceptions
{
    public class InputParameterValidationException : ZopaLoansException
    {
        public InputParameterValidationException(string message) : base(message)
        {
        }
    }
}