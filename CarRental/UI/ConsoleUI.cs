using CarRental.Domain;                                     
using CarRental.Pricing;                                    
using CarRental.Services;                                   

namespace CarRental.UI                                      
{
  
    /// Konsolgränssnittet: visar menyer, läser inmatning och skriver ut resultat.
    /// SRP: här finns ingen affärslogik – allt skickas vidare till RentalService
    /// (GRASP Controller). Byter man till ett webbgränssnitt behöver bara denna
    /// klass bytas ut.
   
    public class ConsoleUI
    {
        private readonly RentalService service;             
        private readonly List<IPricingStrategy> strategies; // Prismodellerna användaren kan välja mellan

        public ConsoleUI(RentalService service)             
        {
            this.service = service;                         // Spara tjänsten
            strategies = new List<IPricingStrategy>         
            {
                new StandardPricing(),                      // 1. Standard
                new LongTermPricing(),                      // 2. Veckohyra
                new CorporatePricing()                      // 3. Företagsavtal
            };
        }

        /// Huvudloopen: visar menyn tills användaren väljer 0.
        public void Run()
        {
            bool running = true;                            
            while (running)                                 
            {
                PrintMainMenu();                            // Visa menyn
                int choice = InputHelper.ReadInt("Val: ", 0, 8); // Läs ett giltigt menyval

                try                                         // Fånga fel så att programmet aldrig kraschar
                {
                    switch (choice)                         
                    {
                        case 1: SearchVehicles(); break;    // Sök lediga fordon
                        case 2: BookVehicle(); break;       // Boka (integrerat flöde)
                        case 3: CancelBooking(); break;     // Avboka
                        case 4: ShowCustomerBookings(); break; // Visa en kunds bokningar
                        case 5: ShowAllVehicles(); break;   // Visa hela flottan
                        case 6: AddVehicle(); break;        // Lägg till fordon (Factory Method)
                        case 7: RegisterCustomer(); break;  // Registrera kund
                        case 8: new DemoRunner().Run(); break; // Demonstrationer för redovisningen
                        case 0: running = false; break;     // Avsluta loopen
                    }
                }
                catch (ArgumentException ex)                
                {
                    PrintError(ex.Message);               
                }
                catch (InvalidOperationException ex)        // Otillåten handling (t.ex. dubbelbokning)
                {
                    PrintError(ex.Message);                 // Visa felet
                }
                catch (KeyNotFoundException ex)             // Något som inte finns (kund, fordon, bokning)
                {
                    PrintError(ex.Message);                 // Visa felet
                }

                if (running)                                
                    InputHelper.Pause();                    
            }
            Console.WriteLine("Hej då!");                   // Avslutningstext
        }

        private static void PrintMainMenu()                 // Skriver ut huvudmenyn
        {
            Console.WriteLine();                           
            Console.WriteLine("========== BILUTHYRNING =========="); // Rubrik
            Console.WriteLine("1. Sök lediga fordon");      // Menyval 1
            Console.WriteLine("2. Boka fordon");            // Menyval 2
            Console.WriteLine("3. Avboka bokning");         // Menyval 3
            Console.WriteLine("4. Visa kundens bokningar"); // Menyval 4
            Console.WriteLine("5. Visa alla fordon");       // Menyval 5
            Console.WriteLine("6. Lägg till fordon");       // Menyval 6
            Console.WriteLine("7. Registrera kund");        // Menyval 7
            Console.WriteLine("8. Demonstrationer (redovisning)"); // Menyval 8
            Console.WriteLine("0. Avsluta");                // Menyval 0
        }

        //Use Case 1: Sök lediga fordon

        private void SearchVehicles()
        {
            RentalPeriod period = ReadPeriod();             // Fråga efter period
            List<Vehicle> available = AskTypeAndSearch(period); 
            PrintVehicleList(available, period);            // Visa resultatet
        }

        //Use Case 2: Boka fordon (integrerat flöde)

        private void BookVehicle()
        {
            Customer customer = ChooseCustomer();           // Steg 1: välj kund
            RentalPeriod period = ReadPeriod();             // Steg 2: välj period
            List<Vehicle> available = AskTypeAndSearch(period); // Steg 3–4: välj typ och sök lediga
            if (available.Count == 0)                       // Edge case: inget ledigt
            {
                Console.WriteLine("Tyvärr finns inga lediga fordon för perioden."); 
                return;                                     // Avbryt bokningen
            }
            PrintVehicleList(available, period);            // Visa de lediga fordonen
            int vehicleChoice = InputHelper.ReadInt("Välj fordon (nummer, 0 = avbryt): ", 0, available.Count); 
            if (vehicleChoice == 0)                         // Ångrade sig användaren?
                return;                                     // Avbryt
            Vehicle vehicle = available[vehicleChoice - 1]; 

            IPricingStrategy strategy = ChooseStrategy();   // Steg 5: välj prismodell (Strategy)
            List<AddOnType> addOns = ChooseAddOns();        // Steg 6: välj tillägg (Decorator)

            IRentalCost quote = service.GetQuote(vehicle.RegNumber, period, strategy, addOns); // Steg 7: prisförslag
            Console.WriteLine("\n--- Prisförslag ---");     // Rubrik
            Console.WriteLine("  " + quote.GetDescription()); 
            Console.WriteLine($"  TOTALT: {quote.GetPrice():0.00} kr"); // Totalpris

            if (!InputHelper.ReadYesNo("Bekräfta bokningen?")) // Steg 8: bekräfta
            {
                Console.WriteLine("Bokningen avbröts.");    // Användaren sa nej
                return;                                     // Avbryt
            }

            Booking booking = service.CreateBooking(customer.Id, vehicle.RegNumber, period, strategy, addOns); // Steg 9: boka
            PrintSuccess("Bokningen är klar!");             // Bekräftelse
            Console.WriteLine(booking.GetReceipt());        // Visa kvittot
        }

        //Use Case 3: Avboka

        private void CancelBooking()
        {
            int bookingId = InputHelper.ReadInt("Bokningsnummer att avboka: ", 1, int.MaxValue); 
            Booking booking = service.CancelBooking(bookingId); // Avboka via controllern
            PrintSuccess($"Bokning {booking.Id} är avbokad. {booking.Vehicle.RegNumber} är ledigt igen under {booking.Period}."); 
        }

        //Övriga menyval

        private void ShowCustomerBookings()
        {
            Customer customer = ChooseCustomer();           // Välj kund
            List<Booking> list = service.GetBookingsForCustomer(customer.Id); // Hämta bokningar
            if (list.Count == 0)                            // Inga bokningar?
            {
                Console.WriteLine($"{customer.Name} har inga bokningar."); // Informera
                return;                                    
            }
            foreach (Booking booking in list)               // Skriv ut varje bokning
                Console.WriteLine("\n" + booking.GetReceipt()); // Hela kvittot
        }

        private void ShowAllVehicles()
        {
            Console.WriteLine("\nHela fordonsflottan (sorterad på dagpris):"); // Rubrik
            foreach (Vehicle vehicle in service.GetAllVehicles()) // Alla fordon, sorterade
            {
                Console.WriteLine("  " + vehicle);          
                Console.WriteLine("          " + vehicle.GetFeatures()); 
            }
        }

        private void AddVehicle()
        {
            List<VehicleType> types = service.GetSupportedVehicleTypes(); 
            for (int i = 0; i < types.Count; i++)           
                Console.WriteLine($"{i + 1}. {TypeToText(types[i])}"); 
            int choice = InputHelper.ReadInt("Typ: ", 1, types.Count); // Välj typ
            string reg = InputHelper.ReadText("Registreringsnummer (t.ex. ABC123): "); // Läs regnr
            string model = InputHelper.ReadText("Modell: "); // Läs modell
            Vehicle vehicle = service.AddVehicle(types[choice - 1], reg, model); // Skapa via Factory Method
            PrintSuccess("Fordonet är tillagt: " + vehicle); // Bekräfta
        }

        private void RegisterCustomer()
        {
            string name = InputHelper.ReadText("Namn: ");   // Läs namn
            string phone = InputHelper.ReadText("Telefon: "); // Läs telefon
            Customer customer = service.RegisterCustomer(name, phone); // Registrera
            PrintSuccess("Registrerad: " + customer);       // Bekräfta
        }

        //Hjälpmetoder för inmatning

        private RentalPeriod ReadPeriod()
        {
            DateTime start = InputHelper.ReadDate($"Startdatum ({InputHelper.DateFormat}): "); 
            DateTime end = InputHelper.ReadDate($"Återlämningsdatum ({InputHelper.DateFormat}): "); 
            return new RentalPeriod(start, end);            // Skapa perioden (kastar fel om slut <= start)
        }

        private List<Vehicle> AskTypeAndSearch(RentalPeriod period)
        {
            List<VehicleType> types = service.GetSupportedVehicleTypes(); 
            Console.WriteLine("0. Alla typer");             // Första valet: ingen filtrering
            for (int i = 0; i < types.Count; i++)           // Visa varje typ
                Console.WriteLine($"{i + 1}. {TypeToText(types[i])}");
            int choice = InputHelper.ReadInt("Fordonstyp: ", 0, types.Count); // Läs val
            if (choice == 0)                                // Alla typer?
                return service.SearchAvailableVehicles(period); // Sök utan typfilter
            return service.SearchAvailableVehicles(period, types[choice - 1]); // Sök på den valda typen
        }

        private Customer ChooseCustomer()
        {
            Console.WriteLine("\nKunder:");                 // Rubrik
            foreach (Customer c in service.GetAllCustomers()) // Visa alla kunder
                Console.WriteLine("  " + c);                // En rad per kund
            int id = InputHelper.ReadInt("Kundnummer: ", 1, int.MaxValue); // Läs kundnummer
            return service.GetCustomer(id);                 // Hämta (kastar fel om kunden saknas)
        }

        private IPricingStrategy ChooseStrategy()
        {
            Console.WriteLine("\nPrismodell:");             // Rubrik
            for (int i = 0; i < strategies.Count; i++)      
                Console.WriteLine($"{i + 1}. {strategies[i].Name} – {strategies[i].Description}"); // Namn och förklaring
            int choice = InputHelper.ReadInt("Val: ", 1, strategies.Count); // Läs val
            return strategies[choice - 1];                  // Den valda strategin
        }

        private static List<AddOnType> ChooseAddOns()
        {
            List<AddOnType> addOns = new List<AddOnType>(); // Tom lista med tillägg
            while (true)                                    // Fråga tills användaren är klar
            {
                Console.WriteLine("\nTillägg: 1. Barnstol (50 kr/dag)  2. GPS (40 kr/dag)  3. Extra förare (200 kr)  0. Klar"); // Alternativ
                int choice = InputHelper.ReadInt("Lägg till: ", 0, 3); 
                if (choice == 0)                            // Klar?
                    return addOns;                          // Returnera valda tillägg
                if (choice == 1)                            // 1 = barnstol
                    addOns.Add(AddOnType.ChildSeat);
                else if (choice == 2)                       // 2 = GPS
                    addOns.Add(AddOnType.Gps);
                else                                        // 3 = extra förare
                    addOns.Add(AddOnType.ExtraDriver);
                Console.WriteLine($"  Tillagt. Antal valda tillägg: {addOns.Count}"); // Bekräfta
            }
        }

        //Hjälpmetoder för utskrift

        private static void PrintVehicleList(List<Vehicle> vehicles, RentalPeriod period)
        {
            Console.WriteLine($"\nLediga fordon {period}, billigast först:"); // Rubrik
            if (vehicles.Count == 0)                        // Tomt resultat?
                Console.WriteLine("  (inga lediga fordon)"); // Informera
            for (int i = 0; i < vehicles.Count; i++)        // Skriv varje fordon med nummer
                Console.WriteLine($"  {i + 1,2}. {vehicles[i]}"); // T.ex. " 1. ABC123 VW Polo ..."
        }

        public static string TypeToText(VehicleType type)   
        {
            switch (type)                                   
            {
                case VehicleType.Compact: return "Kompakt"; 
                case VehicleType.Estate: return "Kombi";    
                case VehicleType.Electric: return "Elbil";  
                case VehicleType.Van: return "Skåpbil";     
                default: return type.ToString();           
            }
        }

        private static void PrintError(string message)      // Skriver ett felmeddelande i rött
        {
            Console.ForegroundColor = ConsoleColor.Red;     // Byt textfärg till röd
            Console.WriteLine("Fel: " + message);           // Skriv felet
            Console.ResetColor();                           
        }

        private static void PrintSuccess(string message)    // Skriver ett lyckat meddelande i grönt
        {
            Console.ForegroundColor = ConsoleColor.Green;   // Byt textfärg till grön
            Console.WriteLine(message);                     // Skriv meddelandet
            Console.ResetColor();                           
        }
    }
}
