using CarRental.Pricing;

namespace CarRental.Domain
{
    // Bokning som kopplar ihop kund, bil, hyrperiod och pris
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

        // Avbokar bokningen om den fortfarande är aktiv
        public void Cancel()
        {
            if (!IsActive)
                throw new InvalidOperationException($"Bokning {Id} är redan avbokad.");
            Status = BookingStatus.Cancelled;
        }

        // Skriver ut ett kvitto med alla detaljer och totalpris
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