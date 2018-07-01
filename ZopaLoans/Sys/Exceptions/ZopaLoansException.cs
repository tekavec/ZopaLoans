using System;

namespace ZopaLoans.Sys.Exceptions
{
    public class ZopaLoansException : Exception
    {
        public ZopaLoansException(string message) : base(message)
        {
        }
    }
}