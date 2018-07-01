using System.Collections.Generic;
using ZopaLoans.Model.Lenders;

namespace ZopaLoans.Tests.TestUtils
{
    public class Fakes
    {
        public static LoanOffers EmptyOffers = new LoanOffers(new List<LoanOffer>());

        public static LoanOffers OneOfferOnly = new LoanOffers(new List<LoanOffer>
        {
            new LoanOffer("Bob", 0.07d, 640)
        });

        public static LoanOffers Offers = new LoanOffers(new List<LoanOffer>
        {
            new LoanOffer("Bob", 0.075d, 640),
            new LoanOffer("Jane", 0.069d, 480),
            new LoanOffer("Fred", 0.071d, 520),
            new LoanOffer("Mary", 0.104d, 170),
            new LoanOffer("John", 0.081d, 320),
            new LoanOffer("Dave", 0.074d, 140),
            new LoanOffer("Ange", 0.071d, 60)
        });
    }
}