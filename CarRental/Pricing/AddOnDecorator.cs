// Hämtar in klasser och gränssnitt från CarRental.Domain så vi kommer åt bl.a. IRentalCost
using CarRental.Domain;
// Namnrymden som den här filen tillhör
namespace CarRental.Pricing
{
    // Abstrakt basklass för tillägg (kan inte skapas direkt, bara ärvas)
    public abstract class AddOnDecorator : IRentalCost
    {
        // Sparar objektet vi ska bygga vidare på (readonly = kan inte ändras efteråt)
        private readonly IRentalCost inner;
        // Konstruktor som tar emot objektet vi lägger tillägg på (bara synlig för underklasser)
        protected AddOnDecorator(IRentalCost inner)
        {
            // Kollar om det inskickade objektet är tomt
            if (inner == null)
                // Kastar ett fel direkt om inner är null
                throw new ArgumentNullException("inner");
            // Sparar det inskickade objektet i vår privata variabel
            this.inner = inner;
        }

        // Namnet på tillägget – måste fyllas i av underklasserna
        protected abstract string AddOnName { get; }
        // Metod för tilläggets kostnad – måste skapas av underklasserna
        protected abstract decimal GetAddOnPrice();
        // Metod för prisdetaljer – måste skapas av underklasserna
        protected abstract string GetPriceDetails();

        // Hämtar det totala priset
        public decimal GetPrice()
        {
            // Tar priset från det inre objektet och plussar på priset för det här tillägget
            return inner.GetPrice() + GetAddOnPrice();
        }

        // Hämtar hela beskrivningen
        public string GetDescription()
        {
            // Tar tidigare beskrivning och lägger till namn, detaljer och pris (med 2 decimaler) för tillägget
            return inner.GetDescription() + $"\n  + {AddOnName}: {GetPriceDetails()} = {GetAddOnPrice():0.00} kr";
        }

    }
} 