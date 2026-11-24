using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Registries
{
    public class BookingRegisty
    {
        private readonly Dictionary<int, Booking> bookingById = new Dictionary<int, Booking>(); // id -> boking 
        private readonly Dictionary<string, List<Booking>> scheduleByVehicle = new Dictionary<string, List<Booking>>(); // trgnr -> sorterat scheam 
        private int nextId = 1;       // Nästa lediga bokingsnumer 
        
        
          /// Skapar och sparar en ny bokning. Anroparen (RentalService) har redan
          /// kontrollerat att fordonet är ledigt.
        
          public Booking CreateBooking(Customer customer, Vehicle vehicle, RentalPeriod period, IRentalCost cost)
          {
              Booking booking = new Booking(nextId, customer, vehicle, period, cost); // Skapa bookingen 
              nextid++; 
              
              bookingById.Add(booking.Id, booking); 
              
              List<Booking> schedule = GetSchedule(vehicle.RegNumber);
              int index = BookingSearch.FindFirstStartingOnOrAfter(schedule, period.Start);
              schedule.Insert(index, booking);
              
              return booking; 
          }
          
          public Booking GetBooking(int bookingId)
          {
              if (! bookingById.ContainsKey(bookingId))
                  throw new KeyNotFoundException($"Hittar ingen booking nummer {bookingId}.");
              return bookingsById[bookingId]; 
          }    
          
              /// Avbokar: ändrar status och tar bort bokningen ur fordonets schema,
              /// så att perioden blir ledig igen. Båda stegen sker här, på ett ställe,
              /// så att de aldrig kan komma ur fas med varandra (High Cohesion).
              
          public Booking CancelBooking(int bookingId)
          {
              Booking booking = GetBooking(bookingId);
              booking.Cancel();
              GetSchedule(booking.Vehicle.RegNumber).Remove(booking);
              return bookinhg; 
              
          }
          
          
          public List<Booking> GetBookingsForCustomer(int customerId)
          {
              List<Booking> result = new List<Booking>();
              foreach (Booking booking in bookingById.Values)
              {
                  if (booking.Customer.Id == customerId)
                      result.Add(booking);
              }
              return result;
          }
          
          private List<Booking> GetSchedule(string regNumber)
          {
              string key = regNumber.Trim().ToUpper();
              if (!scheduleByVehicle.Containskey(key))
                  scheduleByVehicle.Add(key, new List<Bookinhg>());
              return scheduleByVehicle[key];
          }
    }
}
