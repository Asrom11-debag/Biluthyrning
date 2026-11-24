// Hämtar in klasser från CarRental.Domain som t.ex. Vehicle och RentalPeriod
using CarRental.Domain;
// Namnrymden som klassen ligger i
namespace CarRental.Pricing
{
    // Klass som bygger ihop hela prisberäkningen med grundpris och tillägg
    public class PriceCalculator
    {
        // ...samt vald prisstrategi och en lista med valda tillägg
        public IRentalCost BuildCost(Vehicle vehicle, RentalPeriod period,
                                     IPricingStrategy strategy, List<AddOnType> addOns)
        {
            
            IRentalCost cost = new BaseRentalCost(vehicle, period, strategy); // Steg 1: grundpris via vald strategi

            foreach (AddOnType addOn in addOns)             // Gå igenom varje valt tillägg
            {
                cost = WrapWithAddOn(cost, addOn, period.Days); // Steg 2: linda in det nuvarande priset i ett nytt tillägg
            }

            return cost;
    }

        // Privat hjälpmetod som väljer rätt tilläggsklass och lägger den runt cost
        private static IRentalCost WrapWithAddOn(IRentalCost inner, AddOnType type, int days)
        {
            // Kollar vilken typ av tillägg som skickades in
            switch (type)
            {
                // Om tillägget är bilbarnstol
                case AddOnType.ChildSeat:
                    // Skapar och returnerar ett bilbarnstol-tillägg med antal dagar
                    return new ChildSeatAddOn(inner, days);
                case AddOnType.Gps:
                    return new GpsAddOn(inner, days);
                case AddOnType.ExtraDriver:
                    return new ExtraDriverAddOn(inner);
                // Om inget av fallen ovan matchar
                default:
                    // Kastar ett fel som säger att tillägget är okänt
                    throw new ArgumentException($"Okänt tillägg: {type}");
            }

        }
    }
}

