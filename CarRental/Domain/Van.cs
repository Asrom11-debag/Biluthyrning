namespace CarRental.Domain
{
    /// <summary>
    /// Representerar en halvöppen hyrperiod [Start, End) och räknar ut dagar samt överlapp (Information Expert).
    /// </summary>
    public class RentalPeriod
    {
        public DateTime Start { get; }
        public DateTime End { get; }

        public RentalPeriod(DateTime start, DateTime end)
        {
            if (end.Date <= start.Date)
                throw new ArgumentException("Slutdatum måste vara efter startdatum (minst 1 dag).");

            // .Date tar bort klockslaget så vi alltid räknar hela dagar
            Start = start.Date;
            End = end.Date;
        }

        // Återlämningsdagen (End) räknas inte som hyrdag
        public int Days { get { return (End - Start).Days; } }

        // Kollar om två halvöppna intervall krockar (tillåter byte samma dag)
        public bool Overlaps(RentalPeriod other)
        {
            return Start < other.End && other.Start < End;
        }

        public override string ToString()
        {
            return $"{Start:yyyy-MM-dd} – {End:yyyy-MM-dd} ({Days} dagar)";
        }
    }
}