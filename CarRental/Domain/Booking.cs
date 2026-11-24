
using System.Security.Cryptography.X509Certificates;

///<summary>
/// Representerar en elbil.Ärver grundegenskaper från Vechicle (Arv).
/// </summary>
public class ElectricCar : Vehicle
{
   // Skickar vidare renummer och modell till basklassen konstruktor för validering
  public ElectricCar(string regNumber, string model) 
     : base(regNumber, model)
  {
  }

  /// Överskuggar (override) basklassens egenskaper med värden specifika för en elbil
  public override VehicleType Type { get { return VehicleType.Electric; } } 
  public override string TypeName { get {return "Elbil"; } }
  public override decimal DailyRate { get { return 595m; } }
  public override int seats{ get { return 5; } }
        
 
  //Polymorfism: Ger elbilen en egen implementation av Getfetures()
  public override string GetFeatures()
  {
        return "Laddkabel ingår, räckvidd ca 400km.";
  }
}