using System;

namespace ZopaLoans.Model.Interests
{
    public struct InterestRate
    {
        public InterestRate(double annual)
        {
            Annual = annual;
        }

        public double Annual { get; }

        public double Monthly => Math.Pow(1 + Annual, 1/12d) - 1d;
    }
}