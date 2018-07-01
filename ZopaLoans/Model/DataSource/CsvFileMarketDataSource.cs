using System.IO;
using System.Linq;
using CsvHelper;
using ZopaLoans.Model.Lenders;
using ZopaLoans.Sys.Exceptions;

namespace ZopaLoans.Model.DataSource
{
    public class CsvFileMarketDataSource : IMarketDataSource
    {
        public LoanOffers GetAllOffers(string path)
        {
            if (!File.Exists(path))
            {
                throw new MarketFileDoesNotExistException("No market file in the specified location.");
            }
            using (var streamReader = new StreamReader(path))
            {
                var reader = new CsvReader(streamReader);
                var offers = reader.GetRecords<LoanOffer>();
                return new LoanOffers(offers.ToList());
            }
        }
    }
}