using CarRental.Domain;

namespace CarRental.Algorithms
{
    // Binärsökning: kollar om en bil redan är bokad. O(log n).
    public static class BookingSearch
    {
        // Index för första bokningen som startar på eller efter datumet.
        public static int FindFirstStartingOnOrAfter(List<Booking> sortedBookings, DateTime date)
        {
            int low = 0;                                    // Början
            int high = sortedBookings.Count;                // Slutet

            while (low < high)
            {
                int mid = low + (high - low) / 2;           // Mitten
                if (sortedBookings[mid].Period.Start < date)
                    low = mid + 1;                          // Sök till höger
                else
                    high = mid;                             // Sök till vänster
            }

            return low;
        }

        // True om den nya perioden krockar med en bokning.
        public static bool HasOverlap(List<Booking> sortedBookings, RentalPeriod newPeriod)
        {
            int index = FindFirstStartingOnOrAfter(sortedBookings, newPeriod.End);
            if (index == 0)                                 // Ingen startar före vårt slut
                return false;

            Booking candidate = sortedBookings[index - 1];  // Enda som kan krocka
            return candidate.Period.End > newPeriod.Start;
        }

        // Linjär sökning, O(n). Används för att jämföra.
        public static bool HasOverlapLinear(List<Booking> bookings, RentalPeriod newPeriod)
        {
            foreach (Booking booking in bookings)
            {
                if (booking.Period.Overlaps(newPeriod))
                    return true;
            }
            return false;
        }
    }
}
