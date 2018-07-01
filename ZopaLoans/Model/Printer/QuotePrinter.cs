using ZopaLoans.Model.Interests;
using ZopaLoans.Sys.IO;

namespace ZopaLoans.Model.Printer
{
    public class QuotePrinter : IQuotePrinter
    {
        private readonly IConsole console;

        public QuotePrinter(IConsole console)
        {
            this.console = console;
        }

        public void PrintQuote(Repayment repayment)
        {
            console.WriteLine($"Requested amount: £{repayment.Principal.Amount:#}");
            console.WriteLine($"Rate: {repayment.AnnualInterestRate.Annual:P1}");
            console.WriteLine($"Monthly repayment: £{repayment.MonthlyRepayment.Amount:#.00}");
            console.WriteLine($"Total repayment: £{repayment.TotalRepayment.Amount:#.00}");
        }

        public void PrintError(string errorMessage)
        {
            console.WriteLine(errorMessage);
        }
    }
}