using System;
using FluentAssertions;
using Xunit;
using ZopaLoans.Model.Validation;
using ZopaLoans.Sys.Exceptions;

namespace ZopaLoans.Tests.Model.Validation
{
    public class LoanAmountValidatorShould
    {
        [Theory]
        [InlineData("1000")]
        [InlineData("1100")]
        [InlineData("14900")]
        [InlineData("15000")]
        public void validate_if_loan_amount_is_in_range_and_within_increments(string loan)
        {
            var loanAmountValidator = new LoanAmountValidator(1000m, 15000m, 100m);

            Action action = () => loanAmountValidator.Validate(loan);

            action.ShouldNotThrow();
        }

        [Theory]
        [InlineData("900")]
        [InlineData("1001")]
        [InlineData("15100")]
        public void throw_an_exception_if_loan_amount_is_not_range_or_within_increments(string loan)
        {
            var loanAmountValidator = new LoanAmountValidator(1000m, 15000m, 100m);

            Action action = () => loanAmountValidator.Validate(loan);

            action.ShouldThrow<InputParameterValidationException>();
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("10.1.1")]
        public void throw_an_exception_if_loan_amount_is_not_in_numeric_format(string loan)
        {
            var loanAmountValidator = new LoanAmountValidator(1000m, 15000m, 100m);

            Action action = () => loanAmountValidator.Validate(loan);

            action.ShouldThrow<InputParameterValidationException>();
        }
    }
}