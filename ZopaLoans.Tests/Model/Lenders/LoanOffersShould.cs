using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using ZopaLoans.Model.ExchangeMedium;
using ZopaLoans.Model.Lenders;

namespace ZopaLoans.Tests.Model.Lenders
{
    public class LoanOffersShould
    {
        [Fact]
        public void get_sufficient_loan_offers_sorted_by_rate_in_ascending_order()
        {
            var bobOffer = new LoanOffer("Bob", 0.075d, 640);
            var janeOffer = new LoanOffer("Jane", 0.069d, 480);
            var fredOffer = new LoanOffer("Fred", 0.071d, 520);
            var loanOffers = new LoanOffers(new List<LoanOffer> {bobOffer, janeOffer, fredOffer});

            var sortedLoanOffers = loanOffers.GetSufficientSortedLoanOffers(new Money(900m));

            sortedLoanOffers.Should().ContainInOrder(janeOffer, fredOffer);
        }

        [Theory]
        [InlineData(100, 100, 1000, false)]
        [InlineData(50, 50, 100, true)]
        [InlineData(1000, 100, 200, true)]
        public void check_if_there_are_sufficient_offers(int available1, int available2, decimal loanRequested, bool expectedResult)
        {
            var bobOffer = new LoanOffer("Bob", 0.075d, available1);
            var janeOffer = new LoanOffer("Jane", 0.069d, available2);
            var loanOffers = new LoanOffers(new List<LoanOffer> { bobOffer, janeOffer });

            var result = loanOffers.HasSufficientOffersFor(new Money(loanRequested));

            result.Should().Be(expectedResult);
        }
    }
}