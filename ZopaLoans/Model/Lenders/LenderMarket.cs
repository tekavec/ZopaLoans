using ZopaLoans.Model.ExchangeMedium;
using ZopaLoans.Model.InterestCalculations;
using ZopaLoans.Model.Interests;
using ZopaLoans.Sys.Exceptions;

namespace ZopaLoans.Model.Lenders
{
    public class LenderMarket : ILenderMarket
    {
        private readonly IMonthlyCompoundingInterest monthlyCompoundingInterest;

        public LenderMarket(
            IMonthlyCompoundingInterest monthlyCompoundingInterest)
        {
            this.monthlyCompoundingInterest = monthlyCompoundingInterest;
        }

        public InterestRate GetMinInterestRate(LoanOffers loanOffers, Money loan, int numberOfMonthlyPayments)
        {
            if (!loanOffers.HasSufficientOffersFor(loan))
            {
                throw new InsufficientOffersAmountException("It is not possible to provide a quote at that time.");
            }
            var offersTotal = 0m;
            var totalRepayment = 0m;
            var minInterestRate = double.MaxValue;
            var maxInterestRate = double.MinValue;
            foreach (var offer in loanOffers.GetSufficientSortedLoanOffers(loan))
            {
                var currentLoanAmout = offer.Available;
                if (offersTotal + currentLoanAmout > loan.Amount)
                {
                    currentLoanAmout = loan.Amount - offersTotal;
                }
                totalRepayment += monthlyCompoundingInterest.GetMonthlyPayment(
                        new Money(currentLoanAmout),
                        new InterestRate(offer.Rate),
                        numberOfMonthlyPayments
                    ).Amount * numberOfMonthlyPayments;
                offersTotal += currentLoanAmout;
                if (offer.Rate > maxInterestRate)
                {
                    maxInterestRate = offer.Rate;
                }
                if (offer.Rate < minInterestRate)
                {
                    minInterestRate = offer.Rate;
                }
            }
            return monthlyCompoundingInterest.FindCompoundInterestRate(
                loan,
                new Money(totalRepayment),
                numberOfMonthlyPayments,
                new InterestRate(minInterestRate),
                new InterestRate(maxInterestRate));
        }
    }
}