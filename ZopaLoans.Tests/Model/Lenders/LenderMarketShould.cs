using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Xunit;
using ZopaLoans.Model.ExchangeMedium;
using ZopaLoans.Model.InterestCalculations;
using ZopaLoans.Model.Interests;
using ZopaLoans.Model.Lenders;
using ZopaLoans.Sys.Exceptions;
using ZopaLoans.Tests.TestUtils;

namespace ZopaLoans.Tests.Model.Lenders
{
    public class MarketShould
    {
        private readonly Mock<IMonthlyCompoundingInterest> interestCalculation = new Mock<IMonthlyCompoundingInterest>();
        private readonly Money aMoney = new Money(0m);
        private const int NumberOfMonthlyPayments = 36;
        private readonly Money aPrincipal = new Money(1000m);
        private readonly InterestRate anInterestRate = new InterestRate(0.07d);
        private readonly InterestRate bobInterestRate = new InterestRate(0.075d);
        private readonly Money bobOffer = new Money(640m);
        private readonly InterestRate janeInterestRate = new InterestRate(0.069d);
        private readonly Money janeOffer = new Money(480m);
        private readonly Money janeMonthlyRepayment = new Money(14m);
        private readonly InterestRate fredInterestRate = new InterestRate(0.071d);
        private readonly Money fredOffer = new Money(520m);
        private readonly Money fredMonthlyRepayment = new Money(16m);
        private readonly Money janePlusFredRepayment = new Money(1080m);

        [Fact]
        public void throw_an_exception_if_there_are_no_offers()
        {
            var marketLenders = new LenderMarket(interestCalculation.Object);

            Action action = () => marketLenders.GetMinInterestRate(Fakes.EmptyOffers, new Money(1000m), NumberOfMonthlyPayments);

            action.ShouldThrow<InsufficientOffersAmountException>();
        }

        [Fact]
        public void throw_an_exception_if_offers_amount_not_sufficient()
        {
            var marketLenders = new LenderMarket(interestCalculation.Object);
            interestCalculation
                .Setup(a => a.GetMonthlyPayment(It.IsAny<Money>(), It.IsAny<InterestRate>(), It.IsAny<int>()))
                .Returns(aMoney);
            interestCalculation.Setup(a => a.FindCompoundInterestRate(It.IsAny<Money>(), It.IsAny<Money>(),
                    It.IsAny<int>(), It.IsAny<InterestRate>(), It.IsAny<InterestRate>()))
                .Returns(anInterestRate);

            Action action = () => marketLenders.GetMinInterestRate(Fakes.OneOfferOnly, new Money(1000m), NumberOfMonthlyPayments);

            action.ShouldThrow<InsufficientOffersAmountException>();
        }

        [Fact]
        public void cut_the_last_included_lenders_offer_if_it_would_overflow_loan_amount()
        {
            var marketLenders = new LenderMarket(interestCalculation.Object);
            var loanOffers = new LoanOffers(new List<LoanOffer> {new LoanOffer("Jane", janeInterestRate.Annual, 1100)});

            marketLenders.GetMinInterestRate(loanOffers, aPrincipal, NumberOfMonthlyPayments);

            interestCalculation.Verify(a => a.GetMonthlyPayment(aPrincipal, janeInterestRate, NumberOfMonthlyPayments), Times.Once);
        }

        [Fact]
        public void use_the_most_competitive_lenders_offers_for_calculating_min_interest_rate()
        {
            var marketLenders = new LenderMarket(interestCalculation.Object);
            var offers =new LoanOffers(new List<LoanOffer>
            {
                new LoanOffer("Bob", bobInterestRate.Annual, (int) bobOffer.Amount),
                new LoanOffer("Jane", janeInterestRate.Annual, (int)janeOffer.Amount),
                new LoanOffer("Fred", fredInterestRate.Annual, (int)fredOffer.Amount)
            });
            interestCalculation.Setup(a => a.GetMonthlyPayment(janeOffer, janeInterestRate, NumberOfMonthlyPayments)).Returns(janeMonthlyRepayment);
            interestCalculation.Setup(a => a.GetMonthlyPayment(fredOffer, fredInterestRate, NumberOfMonthlyPayments)).Returns(fredMonthlyRepayment);
            interestCalculation.Setup(a => a.FindCompoundInterestRate(It.IsAny<Money>(), It.IsAny<Money>(),
                    It.IsAny<int>(), It.IsAny<InterestRate>(), It.IsAny<InterestRate>()))
                .Returns(anInterestRate);

            var interestRate = marketLenders.GetMinInterestRate(offers, aPrincipal, NumberOfMonthlyPayments);

            interestRate.ShouldBeEquivalentTo(anInterestRate);
            interestCalculation.Verify(a => a.FindCompoundInterestRate(aPrincipal, janePlusFredRepayment,
                NumberOfMonthlyPayments, janeInterestRate, fredInterestRate), Times.Once);
        }
    }
}