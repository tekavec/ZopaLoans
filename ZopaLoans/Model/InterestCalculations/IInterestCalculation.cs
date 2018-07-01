using ZopaLoans.Model.ExchangeMedium;
using ZopaLoans.Model.Interests;

namespace ZopaLoans.Model.InterestCalculations
{
    public interface IInterestCalculation
    {
        Money GetMonthlyPayment(Money principal, InterestRate interestRate, int numberOfMonthlyPayments);
    }
}