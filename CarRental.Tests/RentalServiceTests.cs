using CarRental.Domain;                                      // Bilar, kunder, bokningar
using CarRental.Pricing;                                    // Prismodeller och tillägg
using CarRental.Services;                                   // RentalService
using CarRental.UI;                                         // SeedData bygger systemet
using Xunit;                                                // Testverktyget

namespace CarRental.Tests                                   // Alla tester ligger här
{
    /// <summary>
    /// Testar hela systemet via RentalService: sök, boka och avboka.
    /// Varje test börjar med ett nytt, rent system.
    /// </summary>
    public class RentalServiceTests
    {
        private readonly RentalService service = SeedData.CreateService(withSampleData: false); // Tomt system
        private readonly Customer customer;                 // Vår testkund
        private static readonly List<AddOnType> NoAddOns = new List<AddOnType>(); // Inga tillägg

        public RentalServiceTests()                         // Körs före varje test
        {
            service.AddVehicle(VehicleType.Compact, "ABC123", "VW Polo");    // Billig bil
            service.AddVehicle(VehicleType.Electric, "FGH678", "Tesla");     // Dyrare elbil
            customer = service.RegisterCustomer("Sara Ali", "070-1234567");  // En kund
        }

        private static RentalPeriod P(int from, int to) { return new RentalPeriod(DateTime.Today.AddDays(from), DateTime.Today.AddDays(to)); } // Period räknat från idag

        [Fact]
        public void UC1_Search_ReturnsAvailableVehicles_CheapestFirst() // Sök visar billigast först
        {
            List<Vehicle> result = service.SearchAvailableVehicles(P(1, 3)); // Sök alla bilar
            Assert.Equal(2, result.Count);                  // Båda är lediga
            Assert.Equal("ABC123", result[0].RegNumber);    // Polon kommer först
        }

        [Fact]
        public void UC1_Search_FiltersOnType()              // Sök bara en typ
        {
            List<Vehicle> result = service.SearchAvailableVehicles(P(1, 3), VehicleType.Electric); // Bara elbilar
            Assert.Single(result);                          // En träff
            Assert.Equal("FGH678", result[0].RegNumber);    // Det är Teslan
        }

        [Fact]
        public void UC2_Book_ThenVehicleIsNotInSearch()     // Bokad bil syns inte i sökningen
        {
            service.CreateBooking(customer.Id, "ABC123", P(1, 5), new StandardPricing(), NoAddOns); // Boka Polon
            List<Vehicle> result = service.SearchAvailableVehicles(P(3, 4)); // Sök mitt i bokningen
            Assert.Single(result);                          // Bara Teslan kvar
            Assert.Equal("FGH678", result[0].RegNumber);
        }

        [Fact]
        public void UC2_DoubleBooking_IsRejected()          // Samma bil får inte bokas två gånger
        {
            service.CreateBooking(customer.Id, "ABC123", P(1, 5), new StandardPricing(), NoAddOns); // Första bokningen
            Assert.Throws<InvalidOperationException>(() =>  // Den andra ska nekas
                service.CreateBooking(customer.Id, "ABC123", P(4, 8), new StandardPricing(), NoAddOns));
        }

        [Fact]
        public void UC2_BackToBackBooking_IsAllowed()       // Lämnas och hämtas samma dag
        {
            service.CreateBooking(customer.Id, "ABC123", P(1, 5), new StandardPricing(), NoAddOns); // Dag 1–5
            Booking second = service.CreateBooking(customer.Id, "ABC123", P(5, 8), new StandardPricing(), NoAddOns); // Dag 5–8
            Assert.True(second.IsActive);                   // Det gick bra
        }

        [Fact]
        public void UC2_PastStartDate_IsRejected()          // Kan inte boka bakåt i tiden
        {
            Assert.Throws<ArgumentException>(() =>
                service.CreateBooking(customer.Id, "ABC123", P(-2, 3), new StandardPricing(), NoAddOns));
        }

        [Fact]
        public void UC2_TooLongPeriod_IsRejected()          // Max 60 dagar
        {
            Assert.Throws<ArgumentException>(() =>
                service.CreateBooking(customer.Id, "ABC123", P(1, 62), new StandardPricing(), NoAddOns));
        }

        [Fact]
        public void UC2_UnknownCustomerOrVehicle_IsRejected() // Kund eller bil som inte finns
        {
            Assert.Throws<KeyNotFoundException>(() => service.CreateBooking(99, "ABC123", P(1, 2), new StandardPricing(), NoAddOns)); // Ingen sådan kund
            Assert.Throws<KeyNotFoundException>(() => service.CreateBooking(customer.Id, "ZZZ999", P(1, 2), new StandardPricing(), NoAddOns)); // Ingen sådan bil
        }

        [Fact]
        public void UC3_Cancel_MakesVehicleAvailableAgain() // Avbokad bil blir ledig igen
        {
            Booking booking = service.CreateBooking(customer.Id, "ABC123", P(1, 5), new StandardPricing(), NoAddOns); // Boka
            service.CancelBooking(booking.Id);              // Avboka
            Assert.Equal(BookingStatus.Cancelled, booking.Status); // Nu är den avbokad
            Booking again = service.CreateBooking(customer.Id, "ABC123", P(1, 5), new StandardPricing(), NoAddOns); // Boka samma igen
            Assert.True(again.IsActive);                    // Det går nu
        }

        [Fact]
        public void UC3_CancelTwice_IsRejected()            // Kan inte avboka två gånger
        {
            Booking booking = service.CreateBooking(customer.Id, "ABC123", P(1, 5), new StandardPricing(), NoAddOns); // Boka
            service.CancelBooking(booking.Id);              // Första gången går bra
            Assert.Throws<InvalidOperationException>(() => service.CancelBooking(booking.Id)); // Andra gången nekas
        }

        [Fact]
        public void Booking_TotalPrice_IncludesAddOns()     // Tilläggen syns i priset
        {
            List<AddOnType> addOns = new List<AddOnType> { AddOnType.Gps }; // Med GPS
            Booking booking = service.CreateBooking(customer.Id, "ABC123", P(1, 4), new StandardPricing(), addOns); // 3 dagar
            Assert.Equal(1185m + 120m, booking.TotalPrice); // Hyra plus GPS
        }
    }
}