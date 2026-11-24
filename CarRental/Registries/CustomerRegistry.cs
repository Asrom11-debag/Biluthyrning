using CarRental.Domain;                                      // Behövs för Customer

namespace CarRental.Registries                              // Register i minnet
{
    /// Håller reda på alla kunder. Dictionary ger O(1) uppslagning på kundnummer.
    /// GRASP Creator: registret skapar Customer-objekt och delar ut kundnummer.
    
    public class CustomerRegistry
    {
        private readonly Dictionary<int, Customer> customers = new Dictionary<int, Customer>(); // id -> kund
        private int nextId = 1;                             // Nästa lediga kundnummer

        public Customer RegisterCustomer(string name, string phone) // Skapar och sparar en kund
        {
            Customer customer = new Customer(nextId, name, phone); // Skapa (konstruktorn validerar)
            customers.Add(customer.Id, customer);           
            nextId++;                                       
            return customer;                              
        }

        public Customer GetCustomer(int customerId)         
        {
            if (!customers.ContainsKey(customerId))         
                throw new KeyNotFoundException($"Hittar ingen kund med nummer {customerId}."); 
            return customers[customerId];                   
        }

        public List<Customer> GetAllCustomers()             
        {
            return new List<Customer>(customers.Values);    
        }
    }
}