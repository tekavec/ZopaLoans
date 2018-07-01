using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using ZopaLoans.Model;
using ZopaLoans.Model.DataSource;
using ZopaLoans.Model.InterestCalculations;
using ZopaLoans.Model.Lenders;
using ZopaLoans.Model.Printer;
using ZopaLoans.Model.Validation;
using Console = ZopaLoans.Sys.IO.Console;

namespace ZopaLoans
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(typeof(Program).GetTypeInfo().Assembly.Location))
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            var findRootAccuracy = Double.Parse(configuration["FindRootAlgorithm:Accuracy"]);
            var maxFindRootIterations = Int32.Parse(configuration["FindRootAlgorithm:MaxIterations"]);
            var monthlyPayments = Int32.Parse(configuration["MonthlyPayments"]);
            var loanLowerBoundary = decimal.Parse(configuration["loanAmountValidation:lowerBoundary"]);
            var loanUpperBoundary = decimal.Parse(configuration["loanAmountValidation:upperBoundary"]);
            var loanIncrement = decimal.Parse(configuration["loanAmountValidation:increment"]);

            var loanCalculator = new LoanCalculator(
                new LoanAmountValidator(loanLowerBoundary, loanUpperBoundary, loanIncrement), 
                new QuotePrinter(new Console()),
                new CsvFileMarketDataSource(),
                new LenderMarket(new MonthlyCompoundingInterest()), 
                new MonthlyCompoundingInterest(
                    findRootAccuracy, 
                    maxFindRootIterations));
            loanCalculator.GetQuoteFor(monthlyPayments, args);
        }
    }
}
