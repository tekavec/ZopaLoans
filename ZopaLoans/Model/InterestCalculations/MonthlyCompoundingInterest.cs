using System;
using MathNet.Numerics.RootFinding;
using ZopaLoans.Model.ExchangeMedium;
using ZopaLoans.Model.Interests;
using ZopaLoans.Sys.Exceptions;

namespace ZopaLoans.Model.InterestCalculations
{
    public class MonthlyCompoundingInterest : IMonthlyCompoundingInterest
    {
        private readonly double findRootAccuracy;
        private readonly int maxFindRootIterations;
        private const double DefaultFindRootAccuracy = 1E-05d;
        private const int DefaultMaxFindRootIterations = 10000;

        public MonthlyCompoundingInterest(
            double findRootAccuracy = DefaultFindRootAccuracy, 
            int maxFindRootIterations = DefaultMaxFindRootIterations)
        {
            this.findRootAccuracy = findRootAccuracy;
            this.maxFindRootIterations = maxFindRootIterations;
        }

        public Money GetMonthlyPayment(
            Money principal, 
            InterestRate interestRate, 
            int numberOfMonthlyPayments)
        {
            var monthlyPayment = Convert.ToDouble(principal.Amount) / Convert.ToDouble(numberOfMonthlyPayments); //in case of zero interest
            if (Math.Abs(interestRate.Monthly) > Double.Epsilon)
            {
                monthlyPayment = Convert.ToDouble(principal.Amount) * interestRate.Monthly /
                                 (1 - 1 / Math.Pow(1d + interestRate.Monthly,
                                           Convert.ToDouble(numberOfMonthlyPayments)));
            }
            return new Money(Convert.ToDecimal(monthlyPayment));
        }

        public InterestRate FindCompoundInterestRate(
            Money principal, 
            Money totalRepayment, 
            int numberOfMonthlyPayments,
            InterestRate lowerBoundInterestRate, 
            InterestRate upperBoundInterestRate)
        {
            double CompoundInterestFunction(double x) =>
                Convert.ToDouble(principal.Amount) * x / (1 - 1 / Math.Pow(1d + x, numberOfMonthlyPayments)) -
                Convert.ToDouble(totalRepayment.Amount) / Convert.ToDouble(numberOfMonthlyPayments);

            var findRootSuccess = Brent.TryFindRoot(CompoundInterestFunction, lowerBoundInterestRate.Monthly,
                upperBoundInterestRate.Monthly, findRootAccuracy, maxFindRootIterations, out var monthlyInterestRate);
            if (findRootSuccess)
            {
                return new InterestRate(monthlyInterestRate * 12d);
            }
            throw new CompoundInterestRateConvergenceException("No root for compond interest rate can be found by the algorithm.");
        }
    }
}