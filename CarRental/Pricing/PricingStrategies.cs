// Hämtar in klasser från CarRental.Domain så vi kommer åt Vehicle och RentalPeriod
using CarRental.Domain;
// Namnrymden som prisstrategierna tillhör
namespace CarRental.Pricing
{
    // Klass för standardpris som implementerar IPricingStrategy
    public class StandardPricing : IPricingStrategy
    {
        // Returnerar namnet på den här prismodellen
        public string Name { get { return "Standard"; } }
        // Returnerar en kort beskrivning av hur priset räknas ut
        public string Description { get { return "Dagpris × antal dagar"; } }

        // Metod som räknar ut grundpriset utifrån fordon och period
        public decimal CalculateBasePrice(Vehicle vehicle, RentalPeriod period)
        {
            // Multiplicerar fordonets dagspris med antal dagar och returnerar svaret
            return vehicle.DailyRate * period.Days;
        }
    }

    // Klass för veckohyra/långtidshyra som implementerar IPricingStrategy
    public class LongTermPricing : IPricingStrategy
    {
        // Konstant som anger att man måste hyra minst 7 dagar för rabatt
        private const int MinDaysForDiscount = 7;
        // Konstant för rabatten på 15 % (0,15)
        private const decimal DiscountRate = 0.15m;

        // Returnerar namnet på prismodellen
        public string Name { get { return "Veckohyra"; } }
        // Returnerar beskrivningen av rabatten
        public string Description { get { return "15 % rabatt vid 7 dagar eller mer"; } }

        // Metod som räknar ut grundpriset för långtidshyra
        public decimal CalculateBasePrice(Vehicle vehicle, RentalPeriod period)
        {
            // Räknar först ut fullpris utan rabatt
            decimal fullPrice = vehicle.DailyRate * period.Days;
            // Kollar om hyresperioden är kortare än 7 dagar
            if (period.Days < MinDaysForDiscount)
                // Om det är färre än 7 dagar returneras fullpriset direkt utan rabatt
                return fullPrice;
            // Annars dras 15 % av (gånger 0,85) och priset avrundas till 2 decimaler
            return Math.Round(fullPrice * (1 - DiscountRate), 2);
        }
    }

    // Klass för företagspris som implementerar IPricingStrategy
    public class CorporatePricing : IPricingStrategy
    {
        // Konstant för företagsrabatten på 10 % (0,10)
        private const decimal DiscountRate = 0.10m;
        // Returnerar namnet på prismodellen
        public string Name { get { return "Företagsavtal"; } }
        // Returnerar beskrivningen av företagsrabatten
        public string Description { get { return "10 % rabatt för avtalskunder"; } }
        // Metod som räknar ut grundpriset för företagskunder
        public decimal CalculateBasePrice(Vehicle vehicle, RentalPeriod period)
        {
            // Räknar ut ordinarie pris (dagspris gånger antal dagar)
            decimal fullPrice = vehicle.DailyRate * period.Days;
            // Drar av 10 % rabatt (gånger 0,90) och avrundar till 2 decimaler
            return Math.Round(fullPrice * (1 - DiscountRate), 2);
        }
    }
}
