namespace CarRental.Domain
 {
     // Kombi för familj och last, ärver grundegenskaper från Vehicle
     public class EstateCar : Vehicle
     {
         // Skickar vidare regnummer och modell till basklassen för validering
         public EstateCar(string regNumber, string model)
             : base(regNumber, model)
         {
         }
 
         // Sätter kombins specifika värden
         public override VehicleType Type { get { return VehicleType.Estate; } }
         public override string TypeName { get { return "Kombi"; } }
         public override decimal DailyRate { get { return 495m; } }
         public override int Seats { get { return 5; } }
 
         public override string GetFeatures()
         {
             return "Stort bagageutrymme, dragkrok.";
         }
     }
 }