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

        }

