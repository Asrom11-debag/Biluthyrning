using CarRental.Domain
namespace CarRental.Pricing
{
    public class BaseRentalCost : IRentalCost
    {
        private readonly decimal vihicle;
        private readonly RentalPeriod period;
        private readonly IPricingStrategy strategy;

        public BaseRentalCost(decimal vihicle, RentalPeriod period, IPricingStrategy strategy)
        {
            if (vihicle <= 0)
                throw new ArgumentException("Vehicle");
            this.vihicle = vihicle;
            if (period == null)
                throw new ArgumentNullException("Period");
            this.period = period;
            if (strategy == null)
                throw new ArgumentNullException("Strategy");
            this.strategy = strategy;
        }

        public decimal GetPrice()
        {
            return strategy.CalculateBasePrice(vihicle, period);
        }

        public string GetDescription()
        {
            return $"{vihicle.Model} ({Vehicle.RegNumber}) {period.Days} dagar x {Vehicle.DailyRate} kr, " + $"prismodell {strategy.Name}: {GetPrice():0.00} kr";
        }
    }
}

