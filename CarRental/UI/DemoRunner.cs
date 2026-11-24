using System.Diagnostics;                                   
using CarRental.Algorithms;                                
using CarRental.Domain;                                     
using CarRental.Factories;                                  
using CarRental.Pricing;                                    
using CarRental.Registries;                                 
using CarRental.Services;                                   

namespace CarRental.UI                                      
{
    /// Färdiga demonstrationer för redovisningen.
    /// 1–6 är FRISTÅENDE demonstrationer (en del i taget).
    /// 7 är ett INTEGRERAT SCENARIO där alla delar samverkar.
    /// Varje demo använder ett nytt, eget system så att den alltid ger samma resultat.
    
    public class DemoRunner
    {
        private readonly DateTime today = DateTime.Today;   // Alla datum räknas från idag

        public void Run()                                   // Demomenyn
        {
            while (true)                                    // Visa menyn tills användaren väljer 0
            {
                Console.WriteLine("\n========== DEMONSTRATIONER =========="); // Rubrik
                Console.WriteLine("1. Factory Method – skapa fordon");      // Demo 1
                Console.WriteLine("2. Strategy – byt prismodell");          // Demo 2
                Console.WriteLine("3. Decorator – lägg till tillägg");      // Demo 3
                Console.WriteLine("4. Binärsökning – stoppa dubbelbokning"); // Demo 4
                Console.WriteLine("5. Insättningssortering – billigast först"); // Demo 5
                Console.WriteLine("6. Robusthet – felhantering och edge cases"); // Demo 6
                Console.WriteLine("7. INTEGRERAT SCENARIO: sök → boka → krock → avboka → boka"); // Demo 7
                Console.WriteLine("0. Tillbaka");                           // Tillbaka till huvudmenyn
                int choice = InputHelper.ReadInt("Val: ", 0, 7); // Läs val

                switch (choice)                             // Kör vald demo
                {
                    case 1: DemoFactoryMethod(); break;     // Demo 1
                    case 2: DemoStrategy(); break;          // Demo 2
                    case 3: DemoDecorator(); break;         // Demo 3
                    case 4: DemoBinarySearch(); break;      // Demo 4
                    case 5: DemoSorting(); break;           // Demo 5
                    case 6: DemoRobustness(); break;        // Demo 6
                    case 7: DemoIntegratedScenario(); break; // Demo 7
                    case 0: return;                         // Tillbaka
                }
                InputHelper.Pause();                        // Vänta innan menyn visas igen
            }
        }

        // ---------- Demo 1 ----------
        private void DemoFactoryMethod()
        {
            Header("DEMO 1: Factory Method + Polymorfism");  // Rubrik
            VehicleCreator[] creators =                     // En array med en skapare per fordonstyp
            {
                new CompactCarCreator(), new EstateCarCreator(), new ElectricCarCreator(), new VanCreator()
            };
            string[] regs = { "AAA111", "BBB222", "CCC333", "DDD444" }; // Ett regnr per skapare

            for (int i = 0; i < creators.Length; i++)       
            {
                Vehicle v = creators[i].Create(regs[i], "Demobil"); 
                Console.WriteLine($"Skaparen för {creators[i].Type,-8} skapade: {v.TypeName,-8} {v.DailyRate} kr/dag. {v.GetFeatures()}"); // Visa resultatet
            }

            Console.WriteLine("\nFabriken validerar regnr på ett ställe för alla typer:"); 
            try                                             
            {
                new VanCreator().Create("12ABC", "Fel");    
                Console.WriteLine("OK");                    
            }
            catch (ArgumentException ex)                    
            {
                PrintCaught(ex);                           
            }
            Console.WriteLine("\nVAD: Factory Method. VAR: VehicleCreator + Fleet.AddVehicle. VARFÖR: ny fordonstyp utan att ändra Fleet (OCP)."); 
        }

        // Demo 2
        private void DemoStrategy()
        {
            Header("DEMO 2: Strategy – samma bil, olika prismodeller"); // Rubrik
            Vehicle car = new EstateCar("DEF456", "Volvo V60"); // En kombi (495 kr/dag)
            RentalPeriod shortRent = new RentalPeriod(today.AddDays(1), today.AddDays(4)); // 3 dagar
            RentalPeriod longRent = new RentalPeriod(today.AddDays(1), today.AddDays(11)); // 10 dagar
            IPricingStrategy[] strategies = { new StandardPricing(), new LongTermPricing(), new CorporatePricing() }; // Alla strategier

            foreach (IPricingStrategy s in strategies)      
            {
                decimal p3 = s.CalculateBasePrice(car, shortRent); // Pris för 3 dagar
                decimal p10 = s.CalculateBasePrice(car, longRent); // Pris för 10 dagar
                Console.WriteLine($"{s.Name,-14} 3 dagar: {p3,8:0.00} kr   10 dagar: {p10,8:0.00} kr"); 
            }
            Console.WriteLine("\nNotera: Veckohyra ger vanligt pris vid 3 dagar i stället för att kasta fel – därför kan den ersätta de andra (LSP)."); 
        }

        // Demo 3 
        private void DemoDecorator()
        {
            Header("DEMO 3: Decorator – tillägg lindas runt grundpriset"); // Rubrik
            Vehicle car = new CompactCar("ABC123", "VW Polo"); // Kompaktbil 395 kr/dag
            RentalPeriod period = new RentalPeriod(today.AddDays(1), today.AddDays(4)); // 3 dagar

            IRentalCost cost = new BaseRentalCost(car, period, new StandardPricing()); // Kärnobjekt: grundpris
            Print("Grundpris", cost);                       // 1185 kr
            cost = new ChildSeatAddOn(cost, period.Days);   // Linda in i barnstol
            Print("+ Barnstol", cost);                      // +150 kr
            cost = new GpsAddOn(cost, period.Days);         // Linda in i GPS
            Print("+ GPS", cost);                           // +120 kr
            cost = new ExtraDriverAddOn(cost);              // Linda in i extra förare
            Print("+ Extra förare", cost);                  // +200 kr

            Console.WriteLine("\nKvitto (varje lager lägger till sin egen rad):"); // Rubrik
            Console.WriteLine("  " + cost.GetDescription()); // Hela kedjans beskrivning
            Console.WriteLine($"  TOTALT: {cost.GetPrice():0.00} kr"); // Totalpris
        }

        // Demo 4
        private void DemoBinarySearch()
        {
            Header("DEMO 4: Binärsökning – är fordonet ledigt?"); // Rubrik
            BookingRegistry registry = new BookingRegistry(); // Eget register för demon
            Vehicle car = new CompactCar("ABC123", "VW Polo"); // Ett fordon
            Customer c = new Customer(1, "Demo", "000");    // En kund
            AddDemoBooking(registry, c, car, 10, 14);       // Bokad dag 10–14
            AddDemoBooking(registry, c, car, 20, 25);       // Bokad dag 20–25
            AddDemoBooking(registry, c, car, 2, 5);         // Bokad dag 2–5 (läggs in sist men hamnar först – binärsök insättning)

            Console.WriteLine("Fordonets schema (sorterat på startdatum):"); // Rubrik
            foreach (Booking b in registry.GetActiveBookingsForVehicle("ABC123")) // Visa schemat
                Console.WriteLine("  " + b.Period);         // En rad per bokning

            Console.WriteLine("\nTestar nya perioder:");    // Rubrik
            CheckPeriod(registry, 12, 16, "överlappar bokning 2");          
            CheckPeriod(registry, 14, 20, "precis mellan bokning 2 och 3"); 
            CheckPeriod(registry, 1, 30, "omsluter hela schemat");          
            CheckPeriod(registry, 26, 28, "efter sista bokningen");         

            Console.WriteLine("\nPrestanda: 10 000 bokningar på ett fordon, 2 000 kontroller."); 
            BookingRegistry big = new BookingRegistry();    // Stort register
            for (int i = 0; i < 10000; i++)                 
                AddDemoBooking(big, c, car, 1 + i * 2, 2 + i * 2); 
            List<Booking> schedule = big.GetActiveBookingsForVehicle("ABC123"); 

            Stopwatch watch = Stopwatch.StartNew();         // Starta tidtagning
            for (int i = 0; i < 2000; i++)                  
                BookingSearch.HasOverlapLinear(schedule, Period(19000 + i, 19001 + i)); 
            long linearMs = watch.ElapsedMilliseconds;      

            watch.Restart();                                
            for (int i = 0; i < 2000; i++)                 
                BookingSearch.HasOverlap(schedule, Period(19000 + i, 19001 + i)); 
            long binaryMs = watch.ElapsedMilliseconds;      

            Console.WriteLine($"  Linjär sökning O(n):     {linearMs} ms  (upp till 10 000 jämförelser per kontroll)"); 
            Console.WriteLine($"  Binärsökning O(log n):   {binaryMs} ms  (ca 14 jämförelser per kontroll)");       // Resultat
        }

        // Demo 5 
        private void DemoSorting()
        {
            Header("DEMO 5: Insättningssortering – billigast först"); // Rubrik
            List<Vehicle> list = new List<Vehicle>          // Osorterad lista
            {
                new Van("HIJ890", "VW Transporter"),        
                new CompactCar("ABC123", "VW Polo"),        
                new ElectricCar("FGH678", "Tesla Model 3"),
                new EstateCar("DEF456", "Volvo V60"),       
                new CompactCar("BCD234", "Toyota Yaris")   
            };
            Console.WriteLine("Före sortering:");           // Rubrik
            foreach (Vehicle v in list) Console.WriteLine("  " + v); // Visa osorterat
            VehicleSorter.SortByDailyRate(list);            // Sortera
            Console.WriteLine("\nEfter sortering:");        // Rubrik
            foreach (Vehicle v in list) Console.WriteLine("  " + v); // Visa sorterat
            Console.WriteLine("\nPolo står före Yaris (samma pris) – sorteringen är stabil."); 
        }

        //Demo 6
        private void DemoRobustness()
        {
            Header("DEMO 6: Robusthet – inget av detta kraschar programmet"); // Rubrik
            RentalService service = SeedData.CreateService(true); 
            Console.Write("Slutdatum före startdatum:       "); // Fall 1
            try { new RentalPeriod(today.AddDays(5), today.AddDays(2)); Console.WriteLine("OK"); } // Slut före start
            catch (Exception ex) { PrintCaught(ex); }       // Fånga och visa felet

            Console.Write("Startdatum i det förflutna:      "); // Fall 2
            try { service.SearchAvailableVehicles(Period(-3, 2)); Console.WriteLine("OK"); } // Start bakåt i tiden
            catch (Exception ex) { PrintCaught(ex); }

            Console.Write("För lång hyrperiod (90 dagar):   "); // Fall 3
            try { service.SearchAvailableVehicles(Period(1, 91)); Console.WriteLine("OK"); } // Över 60 dagar
            catch (Exception ex) { PrintCaught(ex); }

            Console.Write("Fordon som inte finns:           "); // Fall 4
            try { service.CreateBooking(1, "ZZZ999", Period(1, 2), new StandardPricing(), new List<AddOnType>()); Console.WriteLine("OK"); }
            catch (Exception ex) { PrintCaught(ex); }

            Console.Write("Kund som inte finns:             "); // Fall 5
            try { service.CreateBooking(99, "ABC123", Period(1, 2), new StandardPricing(), new List<AddOnType>()); Console.WriteLine("OK"); }
            catch (Exception ex) { PrintCaught(ex); }

            Console.Write("Dubblett av regnr:               "); // Fall 6
            try { service.AddVehicle(VehicleType.Compact, "ABC123", "Kopia"); Console.WriteLine("OK"); }
            catch (Exception ex) { PrintCaught(ex); }

            Console.Write("Avboka samma bokning två gånger: "); // Fall 7
            service.CancelBooking(1);                       // Första avbokningen går bra
            try { service.CancelBooking(1); Console.WriteLine("OK"); } // Andra ska nekas
            catch (Exception ex) { PrintCaught(ex); }
        }

        // Demo 7
        private void DemoIntegratedScenario()
        {
            Header("DEMO 7: INTEGRERAT SCENARIO");          // Rubrik
            RentalService service = SeedData.CreateService(true); 
            Customer sara = service.GetCustomer(1);         // Kund 1
            Customer johan = service.GetCustomer(2);        // Kund 2
            RentalPeriod period = Period(20, 27);           

            Step(1, $"{sara.Name} söker lediga elbilar {period}"); // Steg 1: sök
            List<Vehicle> electric = service.SearchAvailableVehicles(period, VehicleType.Electric); 
            foreach (Vehicle v in electric) Console.WriteLine("   " + v); // Visa resultat
            Vehicle chosen = electric[0];                  

            Step(2, "Hon väljer Veckohyra + barnstol + extra förare och ser ett prisförslag"); // Steg 2: pris
            List<AddOnType> addOns = new List<AddOnType> { AddOnType.ChildSeat, AddOnType.ExtraDriver }; // Tillägg
            IRentalCost quote = service.GetQuote(chosen.RegNumber, period, new LongTermPricing(), addOns); 
            Console.WriteLine(Indent(quote.GetDescription())); // Prisrader
            Console.WriteLine($"   TOTALT: {quote.GetPrice():0.00} kr"); // Totalt

            Step(3, "Hon bekräftar – bokningen skapas");    // Steg 3: boka
            Booking saraBooking = service.CreateBooking(sara.Id, chosen.RegNumber, period, new LongTermPricing(), addOns); // Boka
            Console.WriteLine(Indent(saraBooking.GetReceipt())); // Kvitto

            Step(4, $"{johan.Name} försöker boka SAMMA bil med överlappande datum"); // Steg 4: krock
            try                                             // Försök boka den redan bokade bilen
            {
                service.CreateBooking(johan.Id, chosen.RegNumber, Period(25, 30), new StandardPricing(), new List<AddOnType>());
                Console.WriteLine("   OK");                 
            }
            catch (InvalidOperationException ex)            
            {
                PrintCaught(ex);                            // Visa varför
            }

            Step(5, "Systemet visar i stället vad som är ledigt för Johan"); // Steg 5: alternativ
            foreach (Vehicle v in service.SearchAvailableVehicles(Period(25, 30), VehicleType.Electric)) 
                Console.WriteLine("   " + v);               

            Step(6, $"{sara.Name} avbokar");                // Steg 6: avboka
            service.CancelBooking(saraBooking.Id);          // Avboka
            Console.WriteLine($"   Bokning {saraBooking.Id} avbokad."); // Bekräfta

            Step(7, "Nu kan Johan boka bilen han ville ha"); // Steg 7: boka igen
            Booking johanBooking = service.CreateBooking(johan.Id, chosen.RegNumber, Period(25, 30), new StandardPricing(), new List<AddOnType> { AddOnType.Gps }); // Boka
            Console.WriteLine(Indent(johanBooking.GetReceipt())); // Kvitto

            Console.WriteLine("\nI scenariot samverkar: Controller (RentalService), binärsökning, insättningssortering,"); 
            Console.WriteLine("Strategy (prismodell), Decorator (tillägg), Information Expert (RentalPeriod.Overlaps) och felhantering."); // Sammanfattning
        }

        //Små hjälpmetoder för demonstrationerna

        private RentalPeriod Period(int fromDay, int toDay) // Period räknad i dagar från idag
        {
            return new RentalPeriod(today.AddDays(fromDay), today.AddDays(toDay)); 
        }

        private void AddDemoBooking(BookingRegistry registry, Customer c, Vehicle v, int fromDay, int toDay) 
        {
            RentalPeriod p = Period(fromDay, toDay);        // Perioden
            registry.CreateBooking(c, v, p, new BaseRentalCost(v, p, new StandardPricing())); 
        }

        private void CheckPeriod(BookingRegistry registry, int fromDay, int toDay, string note) 
        {
            RentalPeriod p = Period(fromDay, toDay);        // Perioden som testas
            bool free = registry.IsVehicleAvailable("ABC123", p); // Binärsökning
            string result = "UPPTAGET";                     // Anta att perioden är upptagen
            if (free)                                       // Var den ledig?
                result = "LEDIGT";                          
            Console.WriteLine($"  {p,-42} {result,-9} ({note})"); // Skriv ut resultatet
        }

        private static void PrintCaught(Exception ex)       // Skriver ut ett fångat fel
        {
            Console.WriteLine($"Fångat fel -> {ex.Message}"); // Visa felmeddelandet
        }

        private static void Print(string label, IRentalCost cost) 
        {
            Console.WriteLine($"  {label,-16} totalt {cost.GetPrice(),8:0.00} kr"); 
        }

        private static void Header(string title)            // Rubrik för en demo
        {
            Console.WriteLine("\n" + new string('=', 70));  // Linje
            Console.WriteLine(title);                       // Titel
            Console.WriteLine(new string('=', 70));         // Linje
        }

        private static void Step(int number, string text)   
        {
            Console.WriteLine($"\n[Steg {number}] {text}"); 
        }

        private static string Indent(string text)           
        {
            return "   " + text.Replace("\n", "\n   ");     
        }
    }
}
