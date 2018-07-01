using System;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Xunit;
using ZopaLoans.Model.DataSource;
using ZopaLoans.Model.Lenders;
using ZopaLoans.Sys.Exceptions;

namespace ZopaLoans.Tests.Model.DataSource
{
    public class CsvFileMarketDataSourceShould
    {
        [Fact]
        public void get_lender_offers_from_a_csv_file()
        {
            var csvFileFullPath = Path.Combine(Path.GetDirectoryName(typeof(CsvFileMarketDataSourceShould).GetTypeInfo().Assembly.Location), "market.csv");
            var csvFileMarketDataSource = new CsvFileMarketDataSource();

            var offers = csvFileMarketDataSource.GetAllOffers(csvFileFullPath);

            offers.Should().BeOfType<LoanOffers>();
        }

        [Fact]
        public void throw_an_exception_if_source_file_is_not_found()
        {
            var csvFileMarketDataSource = new CsvFileMarketDataSource();

            Action action = () => csvFileMarketDataSource.GetAllOffers("abc");

            action.ShouldThrow<MarketFileDoesNotExistException>();
        }
    }
}