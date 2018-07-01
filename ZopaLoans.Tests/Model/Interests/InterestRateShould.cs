using FluentAssertions;
using Xunit;
using ZopaLoans.Model.Interests;

namespace ZopaLoans.Tests.Model.Interests
{
    public class InterestRateShould
    {
        [Fact]
        public void provide_annual_interest_rate()
        {
            var interestRate = new InterestRate(0.07d);

            interestRate.Annual.ShouldBeEquivalentTo(0.07d);
        }

        [Fact]
        public void provide_monthly_interest_rate()
        {
            var interestRate = new InterestRate(0.012d);

            interestRate.Monthly.ShouldBeEquivalentTo(0.001d);
        }
    }
}