using CarRental.Domain;                                      // Behövs för Vehicle och VehicleType

namespace CarRental.Factories // Namnrymd för fabrikerna (Factory Method)
{
    /// <summary>
    /// DESIGNMÖNSTER: Factory Method (GoF).
    ///
    /// Abstrakt "skapare". Den gemensamma logiken för att skapa ett fordon
    /// (kontrollera registreringsnumrets format) finns här EN gång.
    /// Själva "new" av rätt subklass görs i fabriksmetoden CreateVehicle,
    /// som varje konkret skapare implementerar.
    ///
    /// Ny fordonstyp = ny Vehicle-subklass + ny Creator-subklass.
    /// Ingen befintlig kod behöver ändras (SOLID: OCP).
    /// </summary>
    public abstract class VehicleCreator
    {
        public abstract VehicleType Type { get; } // Vilken fordonstyp denna skapare tillverkar

        /// <summary>
        /// Fabriksmetoden (Factory Method). Subklassen bestämmer vilket
        /// konkret fordon som skapas. protected = bara synlig för subklasser.
        /// </summary>
        protected abstract Vehicle CreateVehicle(string regNumber, string model);

        /// <summary>
        /// Publik metod som används av resten av systemet (Fleet).
        /// Validerar först, anropar sedan fabriksmetoden.
        /// </summary>
        public Vehicle Create(string regNumber, string model)
        {
            if (regNumber == null) // Saknas registreringsnummer helt?
                regNumber = ""; // Gör om till tom text så att kontrollen nedan säger "ogiltigt"
            string normalized = regNumber.Trim().ToUpper(); // Ta bort mellanslag och gör versaler
            if (!IsValidRegNumber(normalized)) // Är formatet fel?
                throw new ArgumentException(
                    $"Ogiltigt registreringsnummer '{regNumber}'. Format: ABC123 eller ABC12D."); // Stoppa
            return CreateVehicle(normalized, model); // Låt subklassen skapa rätt sorts fordon
        }

        /// <summary>
        /// Svenskt format: 3 bokstäver, 2 siffror, 1 siffra eller bokstav.
        /// </summary>
        private static bool IsValidRegNumber(string reg)
        {
            if (reg.Length != 6) // Måste vara exakt 6 tecken
                return false; // Fel längd -> ogiltigt
            for (int i = 0; i < 3; i++) // Tecken 0, 1, 2
                if (!char.IsLetter(reg[i])) // ...måste vara bokstäver
                    return false; // Annars ogiltigt
            if (!char.IsDigit(reg[3]) || !char.IsDigit(reg[4])) // Tecken 3 och 4 måste vara siffror
                return false; // Annars ogiltigt
            return char.IsLetterOrDigit(reg[5]); // Sista tecknet får vara siffra eller bokstav
        }
    }
}
