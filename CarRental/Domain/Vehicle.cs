namespace CarRental.Domain
{
    /// <summary>
    /// Abstrakt basklass för alla fordon (Abstraktion och Arv).
    /// Kan inte instansieras direkt, utan agerar mall för subklasser som fyller i de abstrakta delarna.
    /// </summary>
    public abstract class Vehicle
    {
        // Endast get = skrivskyddade egenskaper (Inkapsling)
        public string RegNumber { get; }
        public string Model { get; }

        // Abstrakta egenskaper som varje subklass måste implementera (Polymorfism)
        public abstract VehicleType Type { get; }
        public abstract string TypeName { get; }
        public abstract decimal DailyRate { get; }
        public abstract int Seats { get; }

        // Protected så att bara subklasser kan anropa konstruktorn och validera indata
        protected Vehicle(string regNumber, string model)
        {
            if (string.IsNullOrWhiteSpace(regNumber))
                throw new ArgumentException("Registreringsnummer får inte vara tomt.");
            if (string.IsNullOrWhiteSpace(model))
                throw new ArgumentException("Modell får inte vara tom.");

            RegNumber = regNumber.Trim().ToUpper();
            Model = model.Trim();
        }

        public abstract string GetFeatures();

        // Gemensam utskrift som hämtar värden från subklassens egenskaper (Template Method-liknande)
        public override string ToString()
        {
            return $"{RegNumber,-7} {Model,-20} {TypeName,-10} {Seats} platser  {DailyRate,5} kr/dag";
        }
    }
}