using System.Collections.Generic;
using System.Linq;
using ZopaLoans.Model.ExchangeMedium;

namespace ZopaLoans.Model.Lenders
{
    public struct LoanOffers
    {
        private readonly IEnumerable<LoanOffer> offers;

        public LoanOffers(IEnumerable<LoanOffer> offers)
        {
            this.offers = offers;
        }

        public IEnumerable<LoanOffer> GetSufficientSortedLoanOffers(Money loan)
        {
            var result = new List<LoanOffer>();
            foreach (var loanOffer in offers.Select(a => a).OrderBy(a => a.Rate))
            {
                if (result.Sum(a => a.Available) < loan.Amount)
                {
                    result.Add(loanOffer);
                }
            }
            return result;
        }

        public bool HasSufficientOffersFor(Money loanRequested)
        {
            return offers.Sum(a => a.Available) >= loanRequested.Amount;
        }
    }
}