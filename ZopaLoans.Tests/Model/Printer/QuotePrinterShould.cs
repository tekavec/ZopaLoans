using Moq;
using Xunit;
using ZopaLoans.Model.ExchangeMedium;
using ZopaLoans.Model.Interests;
using ZopaLoans.Model.Printer;
using ZopaLoans.Sys.IO;

namespace ZopaLoans.Tests.Model.Printer
{
    public class QuotePrinterShould
    {
        private readonly Mock<IConsole> console = new Mock<IConsole>();
        private readonly Money principal = new Money(1m);
        private readonly Money monthlyRepayment = new Money(2.22m);
        private readonly Money totalRepayment = new Money(3333.33m);
        private readonly InterestRate annualInterestRate = new InterestRate(0.1d);
        private readonly string anErrorMessage = "error message";

        [Fact]
        public void print_a_repayment_details_quote()
        {
            var repayment = new Repayment(principal, monthlyRepayment, totalRepayment, annualInterestRate);
            var quotePrinter = new QuotePrinter(console.Object);

            quotePrinter.PrintQuote(repayment);

            console.Verify(a => a.WriteLine("Requested amount: £1"));
            console.Verify(a => a.WriteLine("Rate: 10.0%"));
            console.Verify(a => a.WriteLine("Monthly repayment: £2.22"));
            console.Verify(a => a.WriteLine("Total repayment: £3333.33"));
        }

        [Fact]
        public void print_an_error_message()
        {
            var quotePrinter = new QuotePrinter(console.Object);

            quotePrinter.PrintError(anErrorMessage);

            console.Verify(a => a.WriteLine(anErrorMessage));
        }
    }
}