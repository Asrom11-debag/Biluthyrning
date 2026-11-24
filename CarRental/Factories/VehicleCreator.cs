using CarRental.Domain;

namespace CarRental.Factories
{
    // Factory Method: basklass för alla bilfabriker.
    public abstract class VehicleCreator
    {
        public abstract VehicleType Type { get; }

        // Varje fabrik bestämmer vilken bil som skapas.
        protected abstract Vehicle CreateVehicle(string regNumber, string model);

        // Kollar regnumret och skapar bilen.
        public Vehicle Create(string regNumber, string model)
        {
            if (regNumber == null)
                regNumber = "";
            string normalized = regNumber.Trim().ToUpper();
            if (!IsValidRegNumber(normalized))
                throw new ArgumentException($"Ogiltigt registreringsnummer '{regNumber}'. Format: ABC123 eller ABC12D.");
            return CreateVehicle(normalized, model);
        }

        // Format: ABC123 eller ABC12D.
        private static bool IsValidRegNumber(string reg)
        {
            if (reg.Length != 6)
                return false;
            for (int i = 0; i < 3; i++)
                if (!char.IsLetter(reg[i]))                 // 3 bokstäver
                    return false;
            if (!char.IsDigit(reg[3]) || !char.IsDigit(reg[4])) // 2 siffror
                return false;
            return char.IsLetterOrDigit(reg[5]);            // Siffra eller bokstav
        }
    }
}