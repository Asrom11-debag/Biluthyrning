// Namnrymden som klasserna tillhör
namespace CarRental.Pricing
{
    // Klass för bilbarnstol som ärver från AddOnDecorator
    public class ChildSeatAddOn : AddOnDecorator
    {
        // Fast dagspris på 50 kr (const = kan aldrig ändras, m = decimal)
        private const decimal PricePerDay = 50m;
        private readonly int days;
        // Sparar antal dagar (readonly = sätts bara i konstruktorn
        // Konstruktor som skickar vidare inner till basklassen och tar emot antal dagar
        public ChildSeatAddOn(IRentalCost inner, int days) : base(inner)
        {
            // Sparar inskickat antal dagar i variabeln days
            this.days = days;
        }
        
        // Skriver över namnet på tillägget från basklassen
        protected override string AddOnName { get { return "Barnstol"; } }
        // Räknar ut och returnerar kostnaden (dagspris gånger antal dagar)
        protected override decimal GetAddOnPrice() { return PricePerDay * days; }
        // Returnerar en text som visar hur priset räknats ut
        protected override string GetPriceDetails() { return $"{days} dagar x {PricePerDay} kr"; }
    }
    // Klass för GPS-tillägg som ärver från AddOnDecorator
    public class GpsAddOn : AddOnDecorator
    {
        // Fast dagspris på 40 kr
        private const decimal PricePerDay = 40m;
        // Sparar antal dagar för hyran
        private readonly int days;
        // Konstruktor som skickar inner till basklassen och tar emot dagar
        public GpsAddOn(IRentalCost inner, int days) : base(inner)
        {
            // Sparar antal dagar i fältet
            this.days = days;
        }

        // Returnerar namnet "GPS" för det här tillägget
        protected override string AddOnName { get { return "GPS"; } }
        // Räknar ut totala priset för GPS-tillägget
        protected override decimal GetAddOnPrice() { return PricePerDay * days; }
        // Returnerar prisdetaljerna som text
        protected override string GetPriceDetails() { return $"{days} dagar x {PricePerDay} kr"; }
    }

    // Klass för extra förare som ärver från AddOnDecorator
    public class ExtraDriverAddOn : AddOnDecorator
    {
        // Fast engångspris på 200 kr
        private const decimal FixedPrice = 200m;
        // Konstruktor som bara skickar vidare inner till basklassen (behöver inga dagar)
        public ExtraDriverAddOn(IRentalCost inner) : base(inner)
        {
        }
        // Returnerar namnet på tillägget
        protected override string AddOnName { get { return "Extra förare"; } }
        // Returnerar den fasta kostnaden direkt
        protected override decimal GetAddOnPrice() { return FixedPrice; }
        // Returnerar texten "engångsavgift" som prisdetalj
        protected override string GetPriceDetails() { return "engångsavgift"; }
    }
}