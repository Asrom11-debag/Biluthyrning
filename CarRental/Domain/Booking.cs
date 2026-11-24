
using System.Security.Cryptography.X509Certificates;

using CarRental.Pricing;

namespace CarRental.Domain
{
    // Representerar en bokning som kopplar ihop kund, fordon, period och pris
    public class Booking
    {
        public int Id { get; }
        public Customer Customer { get; }
        public Vehicle Vehicle { get; }
        public RentalPeriod Period { get; }
        public IRentalCost Cost { get; }
        public BookingStatus Status { get; private set; } // Kan bara ändras via Cancel()

        public Booking(int id, Customer customer, Vehicle vehicle, RentalPeriod period, IRentalCost cost)
        {
            Id = id;
            if (customer == null)
                throw new ArgumentNullException("customer");
            Customer = customer;
            if (vehicle == null)
                throw new ArgumentNullException("vehicle");
            Vehicle = vehicle;
            if (period == null)
                throw new ArgumentNullException("period");
            Period = period;
            if (cost == null)
                throw new ArgumentNullException("cost");
            Cost = cost;
            Status = BookingStatus.Active;
        }

        public bool IsActive { get { return Status == BookingStatus.Active; } }

        // Hämtar totalpriset från prisobjektet
        public decimal TotalPrice { get { return Cost.GetPrice(); } }

        // Avbokar bokningen om den inte redan är avbokad
        public void Cancel()
        {
            if (!IsActive)
                throw new InvalidOperationException($"Bokning {Id} är redan avbokad.");
            Status = BookingStatus.Cancelled;
        }

        // Skapar ett formaterat kvitto för bokningen
        public string GetReceipt()
        {
            string statusText = GetStatusText();
            return $"Bokning {Id} [{statusText}]\n" +
                   $"  {Customer}\n" +
                   $"  Period: {Period}\n" +
                   $"  {Cost.GetDescription()}\n" +
                   $"  TOTALT: {TotalPrice:0.00} kr";
        }

        private string GetStatusText()
        {
            if (IsActive)
                return "Aktiv";
            return "Avbokad";
        }

        public override string ToString()
        {
            string statusText = GetStatusText();
            return $"#{Id,-3} {Vehicle.RegNumber,-7} {Period}  {TotalPrice,9:0.00} kr  {statusText}";
        }
    }
}