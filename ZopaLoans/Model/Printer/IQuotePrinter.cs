using ZopaLoans.Model.Interests;

namespace ZopaLoans.Model.Printer
{
    public interface IQuotePrinter
    {
        void PrintQuote(Repayment repayment);
        void PrintError(string errorMessage);
    }
}