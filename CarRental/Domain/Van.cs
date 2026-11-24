namespace CarRental.Domain
{
    // Skåpbil för flytt och transporter, ärver grundegenskaper från Vehicle
    public class Van : Vehicle
    {
        // Skickar vidare regnummer och modell till basklassen för validering
        public Van(string regNumber, string model)
            : base(regNumber, model)
        {
        }

        // Sätter skåpbilens specifika värden
        public override VehicleType Type { get { return VehicleType.Van; } }
        public override string TypeName { get { return "Skåpbil"; } }
        public override decimal DailyRate { get { return 695m; } }
        public override int Seats { get { return 3; } }

        public override string GetFeatures()
        {
            return "Lastvolym ca 10 m³, B-körkort räcker.";
        }
    }
}