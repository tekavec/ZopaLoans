using System;
using ZopaLoans.Model.DataSource;
using ZopaLoans.Model.ExchangeMedium;
using ZopaLoans.Model.InterestCalculations;
using ZopaLoans.Model.Interests;
using ZopaLoans.Model.Lenders;
using ZopaLoans.Model.Printer;
using ZopaLoans.Model.Validation;
using ZopaLoans.Sys.Exceptions;

namespace ZopaLoans.Model
{
    public class LoanCalculator
    {
        private const int NumberOfRequiredCmdLineParameters = 2;
        private readonly IInputValidator inputValidator;
        private readonly IQuotePrinter quotePrinter;
        private readonly IMarketDataSource marketDataSource;
        private readonly ILenderMarket lenderMarket;
        private readonly IInterestCalculation interestCalculation;
        private readonly string invalidParameterInputErrorMessage = $"The number of required parameters is {NumberOfRequiredCmdLineParameters}. Usage: dotnet ZopaLoans.dll <filename> <loan amount>";

        public LoanCalculator(
            IInputValidator inputValidator,
            IQuotePrinter quotePrinter, 
            IMarketDataSource marketDataSource,
            ILenderMarket lenderMarket,
            IInterestCalculation interestCalculation)
        {
            this.inputValidator = inputValidator;
            this.quotePrinter = quotePrinter;
            this.marketDataSource = marketDataSource;
            this.lenderMarket = lenderMarket;
            this.interestCalculation = interestCalculation;
        }

        public void GetQuoteFor(int monthlyPayments, string[] inputParameters)
        {
            if (inputParameters.Length != NumberOfRequiredCmdLineParameters)
            {
                quotePrinter.PrintError(invalidParameterInputErrorMessage);
            }
            else
            {
                try
                {
                    inputValidator.Validate(inputParameters[1]);

                    var loan = new Money(Decimal.Parse(inputParameters[1]));
                    var offers = marketDataSource.GetAllOffers(inputParameters[0]);

                    var interestRate = lenderMarket.GetMinInterestRate(offers, loan, monthlyPayments);
                    var monthlyPayment = interestCalculation.GetMonthlyPayment(loan, interestRate, monthlyPayments);

                    quotePrinter.PrintQuote(new Repayment(
                        loan,
                        monthlyPayment,
                        new Money(monthlyPayment.Amount * monthlyPayments),
                        interestRate));
                }
                catch (ZopaLoansException ex)
                {
                    quotePrinter.PrintError(ex.Message);
                }                
            }
        }
    }
}