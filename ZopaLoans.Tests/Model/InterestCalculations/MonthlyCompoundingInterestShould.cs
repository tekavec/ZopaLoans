using System;
using FluentAssertions;
using Xunit;
using ZopaLoans.Model.ExchangeMedium;
using ZopaLoans.Model.InterestCalculations;
using ZopaLoans.Model.Interests;
using ZopaLoans.Sys.Exceptions;

namespace ZopaLoans.Tests.Model.InterestCalculations
{
    public class MonthlyCompoundingInterestShould
    {
        private const double DesiredInterestRatePrecision = 0.000001d;
        private const decimal DesiredAmountPrecision = 0.00001m;

        [Theory]
        [InlineData(360, 0.0, 36, 10)]
        [InlineData(1000, 0.07, 36, 30.7788788)]
        [InlineData(10000, 0.045, 360, 50.133826)]
        public void get_loan_repayment_breakdown(
            decimal principal, 
            double annualInterestRate, 
            int monthlyRepayments,
            decimal expectedMonthlyRepayment)
        {
            var monthlyCompoundingInterest = new MonthlyCompoundingInterest();

            var monthlyPayment = monthlyCompoundingInterest.GetMonthlyPayment(
                new Money(principal), new InterestRate(annualInterestRate), monthlyRepayments
            );

            monthlyPayment.Amount.Should().BeApproximately(expectedMonthlyRepayment, DesiredAmountPrecision);
        }

        [Theory]
        [InlineData(480, 532.7665201535, 36, 0.065, 0.075, 0.069)]
        [InlineData(1000, 1111.57549, 36, 0.068, 0.075, 0.07)]
        [InlineData(1000, 1000, 12, -0.01, 0.1, 0.0)]
        public void calculate_interest_rate_from(
            decimal principal, 
            decimal totalRepayment, 
            int monthlyPayments, 
            double lowerBoundInterestRate, 
            double upperBoundInterestRate,
            double expectedInterestRate)
        {
            var monthlyCompoundingInterest = new MonthlyCompoundingInterest();

            var rate = monthlyCompoundingInterest.FindCompoundInterestRate(
                new Money(principal), new Money(totalRepayment), monthlyPayments, new InterestRate(lowerBoundInterestRate), new InterestRate(upperBoundInterestRate));

            rate.Annual.Should().BeApproximately(expectedInterestRate, DesiredInterestRatePrecision);
        }

        [Theory]
        [InlineData(400, 300, 36, 0.068, 0.07)]
        public void throw_an_exception_if_algorithm_cannot_converge(
            decimal principal, 
            decimal totalRepayment, 
            int monthlyPayments, 
            double lowerBoundInterestRate, 
            double upperBoundInterestRate)
        {
            var monthlyCompoundingInterest = new MonthlyCompoundingInterest();

            Action action = () => monthlyCompoundingInterest.FindCompoundInterestRate(
                new Money(principal), new Money(totalRepayment), monthlyPayments, new InterestRate(lowerBoundInterestRate), new InterestRate(upperBoundInterestRate));

            action.ShouldThrow<CompoundInterestRateConvergenceException>();
        }
    }
}