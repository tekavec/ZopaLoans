using ZopaLoans.Model.ExchangeMedium;
using ZopaLoans.Model.Interests;

namespace ZopaLoans.Model.Lenders
{
    public interface ILenderMarket
    {
        InterestRate GetMinInterestRate(LoanOffers loanOffers, Money loan, int numberOfMonthlyPayments);
    }
}