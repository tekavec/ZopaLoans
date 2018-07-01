using System;
using ZopaLoans.Sys.Exceptions;

namespace ZopaLoans.Model.Validation
{
    public class LoanAmountValidator : IInputValidator
    {
        private readonly decimal lowerBoundary;
        private readonly decimal upperBoundary;
        private readonly decimal increment;

        public LoanAmountValidator(decimal lowerBoundary, decimal upperBoundary, decimal increment)
        {
            this.lowerBoundary = lowerBoundary;
            this.upperBoundary = upperBoundary;
            this.increment = increment;
        }

        public void Validate(string loanAmountParam)
        {
            var success = Decimal.TryParse(loanAmountParam, out var loanAmount);
            if (!success)
            {
                throw new InputParameterValidationException("The loan amount is not in the required numeric format.");
            }
            if (loanAmount < lowerBoundary || loanAmount > upperBoundary || loanAmount % increment != 0)
            {
                throw new InputParameterValidationException($"The loan amount must be of any {increment} increment between {lowerBoundary} and {upperBoundary} (inclusive).");
            }
        }
    }
}