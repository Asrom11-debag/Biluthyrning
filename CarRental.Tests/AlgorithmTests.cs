using CarRental.Algorithms;                                 // Det vi testar
using CarRental.Domain;                                     // Bilar, kunder, bokningar
using CarRental.Pricing;                                    // Behövs för att skapa ett pris
using CarRental.Registries;                                 // Bokningsregistret
using Xunit;                                                // Testverktyget

namespace CarRental.Tests                                   // Alla tester ligger här
{
    /// <summary>Testar binärsökningen och sorteringen.</summary>
    public class AlgorithmTests
    {
        private static readonly DateTime Today = DateTime.Today;           // Dagens datum
        private readonly Vehicle car = new CompactCar("ABC123", "VW Polo"); // En testbil
        private readonly Customer customer = new Customer(1, "Test", "000"); // En testkund

        private RentalPeriod P(int from, int to) { return new RentalPeriod(Today.AddDays(from), Today.AddDays(to)); } // Period räknat från idag

        private void AddBooking(BookingRegistry registry, int from, int to) // Lägger in en bokning
        {
            RentalPeriod period = P(from, to);              // Vilka dagar
            registry.CreateBooking(customer, car, period, new BaseRentalCost(car, period, new StandardPricing())); // Boka bilen
        }

        private BookingRegistry StandardRegistry()          // Tre bokningar att testa mot
        {
            BookingRegistry registry = new BookingRegistry(); // Tomt från början
            AddBooking(registry, 20, 25);                   // Lägger in dem huller om buller
            AddBooking(registry, 2, 5);                     // för att se att de
            AddBooking(registry, 10, 14);                   // ändå hamnar i rätt ordning
            return registry;                                // Klart
        }

        [Fact]
        public void Insert_KeepsScheduleSorted()            // Schemat ska bli sorterat
        {
            BookingRegistry registry = StandardRegistry();     // Hämta testbokningarna
            List<Booking> schedule = registry.GetActiveBookingsForVehicle("ABC123"); // Bilens schema
            Assert.Equal(Today.AddDays(2), schedule[0].Period.Start);  // Dag 2 först
            Assert.Equal(Today.AddDays(10), schedule[1].Period.Start); // Dag 10 sen
            Assert.Equal(Today.AddDays(20), schedule[2].Period.Start); // Dag 20 sist
        }

        [Fact]
        public void HasOverlap_EmptySchedule_ReturnsFalse() // Inga bokningar alls
        {
            Assert.False(BookingSearch.HasOverlap(new List<Booking>(), P(1, 3))); // Då kan inget krocka
        }

        [Theory]                                            // Samma test, flera fall
        [InlineData(12, 16, true)]                          // Krockar med 10–14
        [InlineData(14, 20, false)]                         // Precis emellan, okej
        [InlineData(1, 30, true)]                           // Täcker allt
        [InlineData(26, 28, false)]                         // Efter allt
        [InlineData(1, 2, false)]                           // Före allt
        [InlineData(3, 4, true)]                            // Mitt i 2–5
        public void BinaryAndLinear_GiveSameAnswer(int from, int to, bool expectedOverlap) // Båda sökningarna ska svara lika
        {
            List<Booking> schedule = StandardRegistry().GetActiveBookingsForVehicle("ABC123"); // Bilens schema
            Assert.Equal(expectedOverlap, BookingSearch.HasOverlap(schedule, P(from, to)));       // Binär sökning
            Assert.Equal(expectedOverlap, BookingSearch.HasOverlapLinear(schedule, P(from, to))); // Vanlig sökning
        }

        [Fact]
        public void InsertionSort_SortsByDailyRate_AndIsStable() // Billigast först
        {
            Vehicle van = new Van("HIJ890", "Transit");     // 695 kr
            Vehicle polo = new CompactCar("ABC123", "Polo"); // 395 kr
            Vehicle yaris = new CompactCar("BCD234", "Yaris"); // 395 kr, samma som Polo
            Vehicle v60 = new EstateCar("DEF456", "V60");   
            List<Vehicle> list = new