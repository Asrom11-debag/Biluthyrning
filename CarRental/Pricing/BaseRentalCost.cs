// Hämtar in klasser och gränssnitt från CarRental.Domain
using CarRental.Domain;
// Namnrymden som klassen ligger i
namespace CarRental.Pricing
{
    // Klass för grundkostnaden av hyran som implementerar IRentalCost
    public class BaseRentalCost : IRentalCost
    {
        // Sparar fordonet/värdet (readonly = kan bara sättas i konstruktorn)
        private readonly Vehicle vehicle;
        // Sparar hyresperioden
        private readonly RentalPeriod period;
        // Sparar prisstrategin som ska räkna ut priset
        private readonly IPricingStrategy strategy;

        // Konstruktor som tar emot fordon, hyresperiod och prisstrategi
        public BaseRentalCost(decimal vehicle, RentalPeriod period, IPricingStrategy strategy)
        {
            // Kollar om värdet på vihicle är 0 eller mindre
            if (vehicle == null)
                // Kastar ett fel om värdet inte är giltigt
                throw new ArgumentException("vehicle");
            // Sparar inskickat värde i fältet vihicle
            this.vehicle = vehicle;
            // Kollar om period är tom (null)
            if (period == null)
                // Kastar ett fel om period saknas
                throw new ArgumentNullException("period");
            // Sparar inskickad period i fältet
            this.period = period;
            // Kollar om strategy är tom (null)
            if (strategy == null)
                // Kastar ett fel om prisstrategi saknas
                throw new ArgumentNullException("strategy");
            // Sparar inskickad prisstrategi i fältet
            this.strategy = strategy;
        }

        // Metod som hämtar grundpriset
        public decimal GetPrice()
        {
            // Låter prisstrategin räkna ut och returnera grundpriset utifrån fordon och period
            return strategy.CalculateBasePrice(vehicle, period);
        }

        // Metod som hämtar en beskrivning av hyran
        public string GetDescription()
        {
            // Sätter ihop och returnerar en text med modell, regnummer, dagar, dagspris, prismodell och totalpris (med 2 decimaler)
            return $"{vehicle.Model} ({vehicle.RegNumber}), {period.Days} dagar x {vehicle.DailyRate} kr, " + $"prismodell {strategy.Name}: {GetPrice():0.00} kr";
        }
    }
}

