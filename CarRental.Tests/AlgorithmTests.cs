using CarRental.Algorithms;       // Koderna vi ska testa
using CarRental.Domain;           // Grundklasser som bil och kund 
using CarRental.Pricing;           //Prisregler 
using CarRental.Registries;        // Bokningsregister
using Xunit;                      // Testramverk

namespace CarRental.Tests;     // Samling för testfiler
{ 
    // Testar Binärsökning och insättning i sorterad lista
    public class AlgorithmTests
    {
    private static readonly DateTime Today = DateTime.Today;   // Dagens datum eller startdatum
    private readonly VehicleType car = new CompactCar("ABC123", "VW polo"); // Testfordon
    private readonly Customer customer = new Customer(1, "Test", "000"); // Testkund
    private RentalPeriod P(int from, int to) { return new Domain.RentalPeriod(Today.AddDays(from), Today.AddDays(to)); }  // datumintervall från idag
    private void AddBooking(BookingRegistry registry, int from, int to )


    }
}