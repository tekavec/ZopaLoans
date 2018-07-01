using ZopaLoans.Model.ExchangeMedium;
using ZopaLoans.Model.Interests;

namespace ZopaLoans.Model.InterestCalculations
{
    public interface IMonthlyCompoundingInterest : IInterestCalculation
    {
        InterestRate FindCompoundInterestRate(
            Money principal,
            Money totalRepayment,
            int numberOfMonthlyPayments,
            InterestRate lowerBoundInterestRate,
            InterestRate upperBoundInterestRate);
    }
}