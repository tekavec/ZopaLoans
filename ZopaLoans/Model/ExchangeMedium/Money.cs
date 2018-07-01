namespace ZopaLoans.Model.ExchangeMedium
{
    public struct Money
    {
        public Money(decimal amount) 
        {
            Amount = amount;
        }

        public decimal Amount { get; }
    }
}