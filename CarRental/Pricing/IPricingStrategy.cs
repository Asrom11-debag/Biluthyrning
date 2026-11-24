using CarRental.Domain;

namespace CarRental.Pricing
{
    public interface IPricingStrategy
    {
        string Name { get; }
        string Description { get; }
        decimal CalculateBasePrice(Vehicle vehicle, RentalPeriod period);
    }
}
