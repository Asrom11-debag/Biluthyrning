using CarRental.Domain;                                      // Behövs för fordonsklasserna

namespace CarRental.Factories                               // Fabriker
{
    // De fyra konkreta skaparna ligger i samma fil eftersom de är mycket små
    // och ser likadana ut. Varje klass gör EN sak: skapar EN fordonstyp (SRP).

    /// <summary>Skapar kompaktbilar.</summary>
    public class CompactCarCreator : VehicleCreator         // Ärver den abstrakta skaparen
    {
        public override VehicleType Type { get { return VehicleType.Compact; } } // Talar om vilken typ den skapar

        protected override Vehicle CreateVehicle(string regNumber, string model) // Implementerar fabriksmetoden
        {
            return new CompactCar(regNumber, model);        // Här – och bara här – skapas en CompactCar
        }
    }

    /// <summary>Skapar kombibilar.</summary>
    public class EstateCarCreator : VehicleCreator          // Ärver den abstrakta skaparen
    {
        public override VehicleType Type { get { return VehicleType.Estate; } } // Typen kombi

        protected override Vehicle CreateVehicle(string regNumber, string model) // Fabriksmetoden
        {
            return new EstateCar(regNumber, model);         // Skapar en kombi
        }
    }

    /// <summary>Skapar elbilar.</summary>
    public class ElectricCarCreator : VehicleCreator        // Ärver den abstrakta skaparen
    {
        public override VehicleType Type { get { return VehicleType.Electric; } } // Typen elbil

        protected override Vehicle CreateVehicle(string regNumber, string model) // Fabriksmetoden
        {
            return new ElectricCar(regNumber, model);       // Skapar en elbil
        }
    }

    /// <summary>Skapar skåpbilar.</summary>
    public class VanCreator : VehicleCreator                // Ärver den abstrakta skaparen
    {
        public override VehicleType Type { get { return VehicleType.Van; } } // Typen skåpbil

        protected override Vehicle CreateVehicle(string regNumber, string model) // Fabriksmetoden
        {
            return new Van(regNumber, model);               // Skapar en skåpbil
        }
    }
}

