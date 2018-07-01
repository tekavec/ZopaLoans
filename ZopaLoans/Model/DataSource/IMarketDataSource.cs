using ZopaLoans.Model.Lenders;

namespace ZopaLoans.Model.DataSource
{
    public interface IMarketDataSource
    {
        LoanOffers GetAllOffers(string path);
    }
}