using CarRental.Domain;

namespace CarRental.Pricing
{
    public class StandardPricing : IPricingStrategy
    {
        public string Name { get { return "Standard"; } }
        public string Description { get { return "Dagpris × antal dagar"; } }

        public decimal CalculateBasePrice(Vehicle vehicle, RentalPeriod period)
        {
            return vehicle.DailyRate * period.Days;
        }
    }

    public class LongTermPricing : IPricingStrategy
    {
        private const int MinDaysForDiscount = 7;
        private const decimal DiscountRate = 0.15m;

        public string Name { get { return "Veckohyra"; } }
        public string Description { get { return "15 % rabatt vid 7 dagar eller mer"; } }

        public decimal CalculateBasePrice(Vehicle vehicle, RentalPeriod period)
        {
            decimal fullPrice = vehicle.DailyRate * period.Days;
            if (period.Days < MinDaysForDiscount)
                return fullPrice;
            return Math.Round(fullPrice * (1 - DiscountRate), 2);
        }
    }

    public class CorporatePricing : IPricingStrategy
    {
        private const decimal DiscountRate = 0.10m;

        public string Name { get { return "Företagsavtal"; } }
        public string Description { get { return "10 % rabatt för avtalskunder"; } }

        public decimal CalculateBasePrice(Vehicle vehicle, RentalPeriod period)
        {
            decimal fullPrice = vehicle.DailyRate * period.Days;
            return Math.Round(fullPrice * (1 - DiscountRate), 2);
        }
    }
}
