namespace ZopaLoans.Model.Lenders
{
    public struct LoanOffer
    {
        public LoanOffer(string lender, double rate, int available)
        {
            Lender = lender;
            Rate = rate;
            Available = available;
        }

        public string Lender { get; set; }
        public double Rate { get; set; }
        public decimal Available { get; set; }
    }
}