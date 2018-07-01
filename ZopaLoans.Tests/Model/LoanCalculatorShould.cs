using Moq;
using Xunit;
using ZopaLoans.Model;
using ZopaLoans.Model.DataSource;
using ZopaLoans.Model.ExchangeMedium;
using ZopaLoans.Model.InterestCalculations;
using ZopaLoans.Model.Lenders;
using ZopaLoans.Model.Printer;
using ZopaLoans.Model.Validation;
using ZopaLoans.Sys.Exceptions;
using ZopaLoans.Sys.IO;

namespace ZopaLoans.Tests.Model
{
    public class LoanCalculatorShould
    {
        private readonly Mock<IConsole> console = new Mock<IConsole>();
        private readonly Mock<IMarketDataSource> marketDataSource = new Mock<IMarketDataSource>();
        private readonly Mock<IInputValidator> inputValidator = new Mock<IInputValidator>();
        private readonly Mock<IQuotePrinter> quotePrinterMock = new Mock<IQuotePrinter>();
        private readonly Mock<IInterestCalculation> interestCalculationMock = new Mock<IInterestCalculation>();
        private readonly Mock<ILenderMarket> marketLenders = new Mock<ILenderMarket>();

        private readonly string[] inputParameters = {"market.csv", "1000"};
        private const int MonthlyPayments = 36;

        [Fact]
        public void display_a_loan_quote_for_monthly_compunding_interests()
        {
            var quotePrinter = new QuotePrinter(console.Object);
            var interestCalculation = new MonthlyCompoundingInterest();
            var lenderMarket = new LenderMarket(interestCalculation);
            var loanCalculator = new LoanCalculator(inputValidator.Object, quotePrinter, new CsvFileMarketDataSource(),
                lenderMarket, interestCalculation);

            loanCalculator.GetQuoteFor(MonthlyPayments, inputParameters);

            console.Verify(a => a.WriteLine("Requested amount: £1000"));
            console.Verify(a => a.WriteLine("Rate: 7.0%"));
            console.Verify(a => a.WriteLine("Monthly repayment: £30.88"));
            console.Verify(a => a.WriteLine("Total repayment: £1111.64"));
        }

        [Fact]
        public void display_an_eror_message_if_exception_was_caught()
        {
            var loanCalculator = new LoanCalculator(inputValidator.Object, quotePrinterMock.Object, marketDataSource.Object,
                marketLenders.Object, interestCalculationMock.Object);
            var errorMessage = "an error message";
            marketLenders.Setup(a => a.GetMinInterestRate(new LoanOffers(), It.IsAny<Money>(), It.IsAny<int>()))
                .Throws(new ZopaLoansException(errorMessage));

            loanCalculator.GetQuoteFor(MonthlyPayments, inputParameters);

            quotePrinterMock.Verify(a => a.PrintError(errorMessage), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData("file")]
        [InlineData("file 1000 3rdParam")]
        public void display_an_eror_message_if_the_number_of_input_parameters_is_incorrect(params string[] input)
        {
            var loanCalculator = new LoanCalculator(inputValidator.Object, quotePrinterMock.Object, marketDataSource.Object,
                marketLenders.Object, interestCalculationMock.Object);

            loanCalculator.GetQuoteFor(MonthlyPayments, input);

            quotePrinterMock.Verify(a => a.PrintError(It.IsAny<string>()), Times.Once);
        }
    }
}
