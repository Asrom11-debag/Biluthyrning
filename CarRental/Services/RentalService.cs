using CarRental.Algorithms;                                 // Behövs för VehicleSorter
using CarRental.Domain;                                     // Domänklasser
using CarRental.Pricing;                                    // Prisberäkning
using CarRental.Registries;                                 // Register

namespace CarRental.Services                               
{
    
    /// GRASP Controller: tar emot alla systemhändelser från användargränssnittet
    /// ("sök", "boka", "avboka" ...) och samordnar arbetet. Själva arbetet görs
    /// av experterna: Fleet, BookingRegistry, PriceCalculator och domänobjekten.
    ///
    /// I ett riktigt system skulle ett webb-API anropa just denna klass.
    /// ConsoleUI vet därför ingenting om register eller algoritmer (Low Coupling).
    
    public class RentalService
    {
        public const int MaxRentalDays = 60;                // Affärsregel: längsta hyrperiod

        private readonly Fleet fleet;                       // Fordonsflottan
        private readonly BookingRegistry bookings;          // Bokningarna
        private readonly CustomerRegistry customers;        // Kunderna
        private readonly PriceCalculator priceCalculator;   // Bygger priskedjan

        
        
        public RentalService(Fleet fleet, BookingRegistry bookings, CustomerRegistry customers, PriceCalculator priceCalculator)
        {
            this.fleet = fleet;                             // Spara the fleet
            this.bookings = bookings;                       // Spara bokningsregister
            this.customers = customers;                     // Spara kundregister
            this.priceCalculator = priceCalculator;         // Spara priskalkylatorn
        }

        // Fordon

        public Vehicle AddVehicle(VehicleType type, string regNumber, string model) 
        {
            return fleet.AddVehicle(type, regNumber, model); 
        }

        public List<Vehicle> GetAllVehicles()               // Alla fordon, billigast först
        {
            List<Vehicle> all = fleet.GetAllVehicles();     
            VehicleSorter.SortByDailyRate(all);             
            return all;                                     
        }

        public List<VehicleType> GetSupportedVehicleTypes() { return fleet.GetSupportedTypes(); } 

        // Kunder 

        public Customer RegisterCustomer(string name, string phone) { return customers.RegisterCustomer(name, phone); } // Ny kund
        public Customer GetCustomer(int customerId) { return customers.GetCustomer(customerId); } // Hämta kund
        public List<Customer> GetAllCustomers() { return customers.GetAllCustomers(); }           // Alla kunder

    

      
        /// Use Case 1: Sök lediga fordon av ALLA typer.
        
        public List<Vehicle> SearchAvailableVehicles(RentalPeriod period)
        {
            return Search(period, false, VehicleType.Compact); 
        }

        
        /// Use Case 1: Sök lediga fordon av EN viss typ (överlagrad metod: samma namn, andra parametrar).
        
        public List<Vehicle> SearchAvailableVehicles(RentalPeriod period, VehicleType type)
        {
            return Search(period, true, type);              
        }

        ///Gemensam sökning för båda metoderna ovan.
        private List<Vehicle> Search(RentalPeriod period, bool filterOnType, VehicleType type)
        {
            ValidatePeriod(period);                      

            List<Vehicle> result = new List<Vehicle>();     
            foreach (Vehicle vehicle in fleet.GetAllVehicles()) 
            {
                if (filterOnType && vehicle.Type != type)   
                    continue;                               
                if (bookings.IsVehicleAvailable(vehicle.RegNumber, period)) 
                    result.Add(vehicle);                    
            }

            VehicleSorter.SortByDailyRate(result);          
            return result;                                 
        }

        
        /// Prisförslag utan att boka (kunden ser kvittot innan hen bekräftar).
        
        public IRentalCost GetQuote(string regNumber, RentalPeriod period, IPricingStrategy strategy, List<AddOnType> addOns)
        {
            Vehicle vehicle = fleet.GetVehicle(regNumber); 
            return priceCalculator.BuildCost(vehicle, period, strategy, addOns); 
        }

        
        /// Use Case 2: Boka fordon. Kontrollerar allt i rätt ordning och
        
        public Booking CreateBooking(int customerId, string regNumber, RentalPeriod period,
                                     IPricingStrategy strategy, List<AddOnType> addOns)
        {
            ValidatePeriod(period);                         // Är perioden tillåten?
            Customer customer = customers.GetCustomer(customerId); // Finns kunden?
            Vehicle vehicle = fleet.GetVehicle(regNumber);  // Finns fordonet?

            if (!bookings.IsVehicleAvailable(vehicle.RegNumber, period)) // Är fordonet ledigt? (förhindrar dubbelbokning)
                throw new InvalidOperationException($"{vehicle.RegNumber} är redan bokat under {period}."); 

            IRentalCost cost = priceCalculator.BuildCost(vehicle, period, strategy, addOns); // Räkna pris
            return bookings.CreateBooking(customer, vehicle, period, cost); // Skapa och spara bokningen
        }

        /// Use Case 3: Avboka. Fordonet blir ledigt igen.
        public Booking CancelBooking(int bookingId)
        {
            return bookings.CancelBooking(bookingId);       
        }

        public List<Booking> GetBookingsForCustomer(int customerId) // Kundens bokningar
        {
            customers.GetCustomer(customerId);              // Kontrollera att kunden finns (kastar annars fel)
            return bookings.GetBookingsForCustomer(customerId); 
        } bokningarna

        public List<Booking> GetActiveBookingsForVehicle(string regNumber) // Ett fordons schema
        {
            Vehicle vehicle = fleet.GetVehicle(regNumber);  // Kontrollera att fordonet finns
            return bookings.GetActiveBookingsForVehicle(vehicle.RegNumber); 
        }

        /// Affärsregler för en hyrperiod.
        private static void ValidatePeriod(RentalPeriod period)
        {
            if (period == null)                             // Saknas perioden helt?
                throw new ArgumentNullException("period"); // Programmeringsfel
            if (period.Start < DateTime.Today)              
                throw new ArgumentException("Startdatum kan inte vara i det förflutna."); 
            if (period.Days > MaxRentalDays)                
                throw new ArgumentException($"Längsta hyrperiod är {MaxRentalDays} dagar."); 
        }
    }
}
