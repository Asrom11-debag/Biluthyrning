namespace CarRental.Domain
{
    /// <summary>
    /// Elbil. Ärver grundegenskaper och validering från Vehicle (Arv).
    /// </summary>
    public class ElectricCar : Vehicle
    {
        // Skickar vidare regnummer och modell till basklassens konstruktor för validering
        public ElectricCar(string regNumber, string model)
            : base(regNumber, model)
        {
        }

        // Ger värden till basklassens abstrakta egenskaper (Polymorfism)
        public override VehicleType Type { get { return VehicleType.Electric; } }
        public override string TypeName { get { return "Elbil"; } }
        public override decimal DailyRate { get { return 595m; } } // m = decimal, rätt typ för pengar
        public override int Seats { get { return 5; } }

        // Elbilens egna implementation av basklassens metod
        public override string GetFeatures()
        {
            return "Laddkabel ingår, räckvidd ca 400 km.";
        }
    }
}