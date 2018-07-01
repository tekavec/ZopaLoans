using ZopaLoans.Model.ExchangeMedium;

namespace ZopaLoans.Model.Interests
{
    public struct Repayment
    {
        public Repayment(Money principal, Money monthlyRepayment, Money totalRepayment, InterestRate annualInterestRate)
        {
            Principal = principal;
            MonthlyRepayment = monthlyRepayment;
            TotalRepayment = totalRepayment;
            AnnualInterestRate = annualInterestRate;
        }

        public Money Principal { get; }
        public Money MonthlyRepayment { get; }
        public Money TotalRepayment { get; }
        public InterestRate AnnualInterestRate { get; }
    }
}