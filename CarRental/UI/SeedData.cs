using CarRental.Domain;                                     
using CarRental.Factories;                                  
using CarRental.Pricing;                                    
using CarRental.Registries;                                 
using CarRental.Services;                                  

namespace CarRental.UI                                      
{
  
    /// Bygger ihop systemet och fyller det med exempeldata.
    /// Här (och bara här) väljs vilka konkreta klasser som används 
    
    
    public static class SeedData
    {
        /// skapar en färdig RentalService. withSampleData = fyll med fordon, kunder och bokningar.
        public static RentalService CreateService(bool withSampleData)
        {
            Fleet fleet = new Fleet();                      // Tom flotta
            fleet.RegisterCreator(new CompactCarCreator()); // Registrera fabrik för kompaktbilar
            fleet.RegisterCreator(new EstateCarCreator());  // för kombi
            fleet.RegisterCreator(new ElectricCarCreator()); // för elbilar
            fleet.RegisterCreator(new VanCreator());        // för skåpbilar

            RentalService service = new RentalService(      // Koppla ihop tjänsten med sina beroenden
                fleet, new BookingRegistry(), new CustomerRegistry(), new PriceCalculator());

            if (withSampleData)                             // Ska exempeldata läggas in?
                AddSampleData(service);                     

            return service;                                 // Returnera det färdiga systemet
        }

        private static void AddSampleData(RentalService service)
        {
            // Fordonen läggs medvetet in i "blandad" prisordning så att sorteringen syns.
            service.AddVehicle(VehicleType.Electric, "FGH678", "Tesla Model 3");     // Elbil
            service.AddVehicle(VehicleType.Compact, "ABC123", "VW Polo");            // Kompakt
            service.AddVehicle(VehicleType.Van, "HIJ890", "VW Transporter");         // Skåpbil
            service.AddVehicle(VehicleType.Estate, "DEF456", "Volvo V60");           // Kombi
            service.AddVehicle(VehicleType.Compact, "BCD234", "Toyota Yaris");       // Kompakt
            service.AddVehicle(VehicleType.Electric, "GHI789", "Volkswagen ID.4");   // Elbil
            service.AddVehicle(VehicleType.Estate, "EFG567", "Skoda Octavia Kombi"); // Kombi
            service.AddVehicle(VehicleType.Van, "IJK901", "Ford Transit");           // Skåpbil

            Customer sara = service.RegisterCustomer("Sara Ali", "070-1234567");       
            Customer johan = service.RegisterCustomer("Johan Berg", "073-7654321");   
            service.RegisterCustomer("Dalarna Bygg AB", "023-112233");                 

            DateTime today = DateTime.Today;                
            service.CreateBooking(sara.Id, "ABC123",        
                new RentalPeriod(today.AddDays(3), today.AddDays(7)),
                new StandardPricing(), new List<AddOnType> { AddOnType.ChildSeat });
            service.CreateBooking(johan.Id, "DEF456",       
                new RentalPeriod(today.AddDays(5), today.AddDays(13)),
                new LongTermPricing(), new List<AddOnType> { AddOnType.Gps, AddOnType.ExtraDriver });
        }
    }
}
