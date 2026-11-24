using CarRental.Domain;

namespace CarRental.Factories
{
    // En fabrik per biltyp.

    public class CompactCarCreator : VehicleCreator
    {
        public override VehicleType Type { get { return VehicleType.Compact; } }

        protected override Vehicle CreateVehicle(string regNumber, string model)
        {
            return new CompactCar(regNumber, model);
        }
    }

    public class EstateCarCreator : VehicleCreator
    {
        public override VehicleType Type { get { return VehicleType.Estate; } }

        protected override Vehicle CreateVehicle(string regNumber, string model)
        {
            return new EstateCar(regNumber, model);
        }
    }

    public class ElectricCarCreator : VehicleCreator
    {
        public override VehicleType Type { get { return VehicleType.Electric; } }

        protected override Vehicle CreateVehicle(string regNumber, string model)
        {
            return new ElectricCar(regNumber, model);
        }
    }

    public class VanCreator : VehicleCreator
    {
        public override VehicleType Type { get { return VehicleType.Van; } }

        protected override Vehicle CreateVehicle(string regNumber, string model)
        {
            return new Van(regNumber, model);
        }
    }
}