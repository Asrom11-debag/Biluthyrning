using CarRental.Domain;                                      // Det vi testar
using CarRental.Factories;                                  // Fabrikerna
using CarRental.Registries;                                 // Flottan
using Xunit;                                                // Testverktyget

namespace CarRental.Tests                                   // Alla tester ligger här
{
    /// <summary>Testar hyrperioder, fabrikerna och flottan.</summary>
    public class DomainTests
    {
        private static readonly DateTime Today = DateTime.Today; // Dagens datum

        [Fact]                                              // Ett vanligt test
        public void RentalPeriod_CountsDaysCorrectly()      // Blir antalet dagar rätt?
        {
            RentalPeriod period = new RentalPeriod(Today.AddDays(1), Today.AddDays(4)); // Dag 1 till 4
            Assert.Equal(3, period.Days);                   // Det ska bli 3 dagar
        }

        [Fact]
        public void RentalPeriod_EndBeforeStart_Throws()    // Slutar innan den börjar
        {
            Assert.Throws<ArgumentException>(() => new RentalPeriod(Today.AddDays(5), Today.AddDays(2))); // Ska ge fel
        }

        [Fact]
        public void RentalPeriod_SameDay_Throws()           // Noll dagar
        {
            Assert.Throws<ArgumentException>(() => new RentalPeriod(Today.AddDays(5), Today.AddDays(5))); // Ska ge fel
        }

        [Fact]
        public void RentalPeriod_Overlapping_ReturnsTrue()  // Två perioder som krockar
        {
            RentalPeriod a = new RentalPeriod(Today.AddDays(1), Today.AddDays(5)); // Dag 1–5
            RentalPeriod b = new RentalPeriod(Today.AddDays(4), Today.AddDays(8)); // Dag 4–8
            Assert.True(a.Overlaps(b));                     // De krockar
            Assert.True(b.Overlaps(a));                     // Åt båda hållen
        }

        [Fact]
        public void RentalPeriod_BackToBack_DoesNotOverlap() // Lämnas och hämtas samma dag
        {
            RentalPeriod a = new RentalPeriod(Today.AddDays(1), Today.AddDays(5)); // Dag 1–5
            RentalPeriod b = new RentalPeriod(Today.AddDays(5), Today.AddDays(8)); // Dag 5–8
            Assert.False(a.Overlaps(b));                    // Det är okej, ingen krock
        }

        [Fact]
        public void Creators_CreateCorrectSubclass()        // Varje fabrik gör rätt bil
        {
            Assert.IsType<CompactCar>(new CompactCarCreator().Create("ABC123", "Polo")); // Kompaktbil
            Assert.IsType<EstateCar>(new EstateCarCreator().Create("ABC124", "V60"));    // Kombi
            Assert.IsType<ElectricCar>(new ElectricCarCreator().Create("ABC125", "ID.4")); // Elbil
            Assert.IsType<Van>(new VanCreator().Create("ABC126", "Transit"));          // Skåpbil
        }

        [Fact]
        public void Creator_NormalizesRegNumber()           // Små bokstäver och mellanslag
        {
            Vehicle v = new CompactCarCreator().Create(" abc123 ", "Polo"); // Slarvigt inskrivet
            Assert.Equal("ABC123", v.RegNumber);            // Ska bli snyggt ändå
        }

        [Fact]
        public void Creator_InvalidRegNumber_Throws()       // Konstigt regnummer
        {
            Assert.Throws<ArgumentException>(() => new VanCreator().Create("12ABC", "Transit")); // Ska ge fel
        }

        [Fact]
        public void Fleet_DuplicateRegNumber_Throws()       // Samma bil två gånger
        {
            Fleet fleet = new Fleet();                      // Tom flotta
            fleet.RegisterCreator(new CompactCarCreator()); // Lägg till en fabrik
            fleet.AddVehicle(VehicleType.Compact, "ABC123", "Polo"); // Första gången går bra
            Assert.Throws<InvalidOperationException>(() => fleet.AddVehicle(VehicleType.Compact, "abc123", "Yaris")); // Andra gången nekas
        }

        [Fact]
        public void Fleet_TypeWithoutCreator_Throws()       // Ingen fabrik för typen
        {
            Fleet fleet = new Fleet();                      // Flotta utan fabriker
            Assert.Throws<InvalidOperationException>(() => fleet.AddVehicle(VehicleType.Van, "ABC123", "Transit")); // Ska ge fel
        }
    }
}