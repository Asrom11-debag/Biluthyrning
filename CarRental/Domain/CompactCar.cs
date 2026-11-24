namespace CarRental.Domain
{
    ///<summary>
    ///kompaktbil - liten och billig stadsbil. Ärver från Vehicle (APIE: Arv).
    ///</summary>
    public class CompactCar : Vehicle
    {
        // Skickar vidare till Vehicle-konstruktorn som gör valderingen
        public CompactCar(string regNumber, string model)
            : base(regNumber, model)
        {

        }

        //Ger värden till basklassens abstrakta egenskaper (Polymorfism)
        public override VehicleType Type { get { return VehicleType.Compact; } }
        public override string TypeName { get { return "Kompakt"; } }
        public override decimal DailyRate { get { return 395m; } } //m = decimal, rätt typ för pengar
        public override int Seats { get { return 5; } }

        // Kompaktbilens egna implementation av basklassens metod
        public override string GetFeatures()
        {
            return "låg förbrukning,lätt att parkera i stan.";

        }
    }

}



