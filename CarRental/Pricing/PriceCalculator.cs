using CarRental.Domain;

namespace CarRental.Pricing
{
    public class PriceCalculator
    {
        public IRentalCost BuildCost(Vehicle vehicle, RentalPeriod period,
                                     IPricingStrategy strategy, List<AddOnType> addOns)
        {
            IRentalCost cost = new BaseRentalCost(vehicle, period, strategy); // Steg 1: grundpris via vald strategi

            foreach (AddOnType addOn in addOns)             // Gå igenom varje valt tillägg
            {
                cost = WrapWithAddOn(cost, addOn, period.Days); // Steg 2: linda in det nuvarande priset i ett nytt tillägg
            }

            return cost;)
    }

        private IRentalCost WrapWithAddOn(IRentalCost cost, AddOnType addOn, int days)
        {
            switch (addOn)
            {
                case AddOnType.ChildSeat:
                    return new ChildSeatAddOn(inner, days);
                case AddOnType.Gps:
                    return new GpsAddOn(inner, days);
                case AddOnType.ExtraDriver:
                    return new ExtraDriverAddOn(cost, days);
                default:
                    throw new ArgumentException($"Okänt tillägg: {type}");
            }

        }
    }
}

