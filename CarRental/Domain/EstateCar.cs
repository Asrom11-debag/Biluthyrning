namespace CarRental.Domain
{
    /// <summary>
    /// Representerar en kombi. Ärver grundegenskaper från basklassen Vehicle.
    /// </summary>
    public class EstateCar : Vehicle
    {
        // Vidarebefordrar registreringsnummer och modell till basklassen för validering
        public EstateCar(string regNumber, string model)
            : base(regNumber, model)
        {
        }

        // Överskuggar basklassens egenskaper med specifika värden för en kombi
        public override VehicleType Type { get { return VehicleType.Estate; } }
        public override string TypeName { get { return "Kombi"; } }
        public override decimal DailyRate { get { return 495m; } } // decimal används för exakta belopp
        public override int Seats { get { return 5; } }

        // Returnerar en egen beskrivning av kombins egenskaper (polymorfism)
        public override string GetFeatures()
        {
            return "Stort bagageutrymme, dragkrok.";
        }
    }
}