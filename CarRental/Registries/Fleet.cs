
using CarRental.Domain;                                      // Behövs för Vehicle och VehicleType
using CarRental.Factories;                                  

namespace CarRental.Registries                              
{
    
    /// Fordonsflottan: håller reda på alla fordon i minnet.
    ///
    /// GRASP Creator: flottan "äger" fordonen och är därför den som ber
    /// fabrikerna skapa nya fordon.
    ///
    /// Datastrukturer:
    ///  - Dictionary&lt;string, Vehicle&gt;: snabb uppslagning på regnr, O(1).
    ///  - Dictionary&lt;VehicleType, VehicleCreator&gt;: väljer rätt fabrik
    ///    utan if/switch (GRASP Polymorphism, SOLID OCP).
    
    public class Fleet
    {
        private readonly Dictionary<string, Vehicle> vehicles = new Dictionary<string, Vehicle>(); 
        private readonly Dictionary<VehicleType, VehicleCreator> creators = new Dictionary<VehicleType, VehicleCreator>(); 

        /// Registrerar en fabrik för en fordonstyp.
        public void RegisterCreator(VehicleCreator creator)
        {
            creators[creator.Type] = creator;              
        }

        /// Skapar ett nytt fordon via rätt fabrik och lägger in det i flottan.
        public Vehicle AddVehicle(VehicleType type, string regNumber, string model)
        {
            if (!creators.ContainsKey(type))               
                throw new InvalidOperationException($"Ingen fabrik registrerad för {type}."); 

            VehicleCreator creator = creators[type];       
            Vehicle vehicle = creator.Create(regNumber, model); 

            if (vehicles.ContainsKey(vehicle.RegNumber))    
                throw new InvalidOperationException($"Fordonet {vehicle.RegNumber} finns redan."); 

            vehicles.Add(vehicle.RegNumber, vehicle);       
            return vehicle;                                 
        }

        /// Hämtar ett fordon på regnr. Kastar tydligt fel om det inte finns.
        public Vehicle GetVehicle(string regNumber)
        {
            if (regNumber == null)                          // Saknas regnr helt?
                regNumber = "";                             
            string key = regNumber.Trim().ToUpper();        // Normalisera så att "abc123" hittar "ABC123"
            if (!vehicles.ContainsKey(key))                 
                throw new KeyNotFoundException($"Hittar inget fordon med regnr '{regNumber}'."); 
            return vehicles[key];                           // Hämta och returnera fordonet
        }

        /// Alla fordon som en ny lista (så att flottan inte kan ändras utifrån).
        public List<Vehicle> GetAllVehicles()
        {
            return new List<Vehicle>(vehicles.Values);      
        }

        /// De fordonstyper som har en fabrik, t.ex. till menyn.
        public List<VehicleType> GetSupportedTypes()
        {
            return new List<VehicleType>(creators.Keys);    
        }
    }
}
