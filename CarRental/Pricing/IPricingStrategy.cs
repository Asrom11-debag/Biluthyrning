// Hämtar in CarRental.Domain så vi kommer åt Vehicle och RentalPeriod
using CarRental.Domain;
// Namnrymden som det här gränssnittet ligger i
namespace CarRental.Pricing
{
    // Gränssnitt (kontrakt) som bestämmer vad alla prisstrategier måste innehålla
    public interface IPricingStrategy
    {
        // Namnet på prismodellen (går bara att läsa)
        string Name { get; }
        // Kort beskrivning av prismodellen (går bara att läsa)
        string Description { get; }
        // Metod som räknar ut och returnerar grundpriset utifrån fordon och hyresperiod
        decimal CalculateBasePrice(Vehicle vehicle, RentalPeriod period);
    }
}
