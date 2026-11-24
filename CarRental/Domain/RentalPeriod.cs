namespace CarRental.Domain
{
    /// <summary>
    /// Representerar en hyrperiod [Start, End) och sköter beräkningar av dagar och överlapp (Information Expert).
    /// </summary>
    public class RentalPeriod
    {
        public DateTime Start { get; }
        public DateTime End { get; }

        public RentalPeriod(DateTime start, DateTime end)
        {
            // Säkerställer att perioden är minst en dag lång
            if (end.Date <= start.Date)
                throw new ArgumentException("Slutdatum måste vara efter startdatum (minst 1 dag).");

            // .Date nollställer klockslaget så beräkningen alltid sker på hela dygn
            Start = start.Date;
            End = end.Date;
        }

        // Räknar ut antal hyrdagar (återlämningsdagen räknas inte)
        public int Days { get { return (End - Start).Days; } }

        // Kontrollerar om två halvöppna perioder krockar (samma slut- och startdag tillåts)
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