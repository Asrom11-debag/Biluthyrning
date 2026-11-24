// Namnrymden som det här gränssnittet tillhör
namespace CarRental.Pricing
{
    // Gränssnitt (kontrakt) som bestämmer vad alla kostnadsklasser måste ha
    public interface IRentalCost
    {
        // Metod som hämtar och returnerar priset som ett decimaltal
        decimal GetPrice();
        // Metod som hämtar och returnerar en textbeskrivning av kostnaden
        string GetDescription();
        
    }
}

