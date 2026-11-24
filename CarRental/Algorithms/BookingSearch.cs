using CarRental.Domain;                                      // Behövs för Booking och RentalPeriod

namespace CarRental.Algorithms                              // Namnrymd för egna algoritmer
{
    /// <summary>
    /// ALGORITM: Binärsökning för att kontrollera dubbelbokning.
    ///
    /// Förutsättning: listan innehåller ETT fordons AKTIVA bokningar,
    /// sorterade på startdatum, och bokningarna överlappar aldrig varandra
    /// (det garanterar systemet). Då är även slutdatumen sorterade.
    ///
    /// Idé: hitta den sista bokningen som startar FÖRE den nya periodens slut.
    /// Bara den bokningen kan krocka – alla tidigare slutar ännu tidigare,
    /// och alla senare startar för sent.
    ///
    /// Tidskomplexitet: O(log n) i stället för O(n) för linjär sökning.
    /// GRASP Pure Fabrication: en teknisk hjälpklass utan egen data.
    /// </summary>
    public static class BookingSearch                       // static = klassen har bara metoder, inga objekt skapas
    {
        /// <summary>
        /// Binärsökning ("lower bound"): returnerar index för den första
        /// bokningen vars startdatum är &gt;= date. Om ingen finns: listans längd.
        /// Samma index används när en ny bokning ska sättas in på rätt plats.
        /// </summary>
        public static int FindFirstStartingOnOrAfter(List<Booking> sortedBookings, DateTime date)
        {
            int low = 0;                                    // Vänstra gränsen för sökområdet
            int high = sortedBookings.Count;                // Högra gränsen (exklusive) – hela listan till att börja med

            while (low < high)                              // Så länge sökområdet inte är tomt
            {
                int mid = low + (high - low) / 2;           // Mitten av området (skrivs så för att undvika överflöd)
                if (sortedBookings[mid].Period.Start < date) // Startar mitten-bokningen före datumet?
                    low = mid + 1;                          // Ja -> svaret ligger till höger om mitten
                else
                    high = mid;                             // Nej -> svaret är mitten eller till vänster
            }

            return low;                                     // low == high: första positionen där Start >= date
        }

        /// <summary>
        /// Sant om den nya perioden krockar med någon bokning i listan. O(log n).
        /// </summary>
        public static bool HasOverlap(List<Booking> sortedBookings, RentalPeriod newPeriod)
        {
            int index = FindFirstStartingOnOrAfter(sortedBookings, newPeriod.End); // Alla från index startar för sent för att krocka
            if (index == 0)                                 // Ingen bokning startar före den nya periodens slut
                return false;                               // -> ingen krock möjlig

            Booking candidate = sortedBookings[index - 1];  // Den sista bokningen som startar före vårt slut
            return candidate.Period.End > newPeriod.Start;  // Krock om den slutar efter att vår period börjar
        }

        /// <summary>
        /// Linjär sökning – det enkla alternativet. Går igenom alla bokningar. O(n).
        /// Finns kvar för att jämföra mot binärsökningen i tester och demo.
        /// </summary>
        public static bool HasOverlapLinear(List<Booking> bookings, RentalPeriod newPeriod)
        {
            foreach (Booking booking in bookings)           // Titta på varje bokning en i taget
            {
                if (booking.Period.Overlaps(newPeriod))     // Krockar den? (RentalPeriod är Information Expert)
                    return true;                            // Ja -> klart, vi behöver inte leta mer
            }
            return false;                                   // Ingen krockade
        }
    }
}


