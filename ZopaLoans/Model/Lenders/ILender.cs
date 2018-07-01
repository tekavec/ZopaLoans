using ZopaLoans.Model.ExchangeMedium;
using ZopaLoans.Model.Interests;

namespace ZopaLoans.Model.Lenders
{
    public interface ILender
    {
        InterestRate GetMinInterestRate(Money loan, int numberOfMonthlyPayments);
        Repayment GetRepaymentBreakdown();
    }
}