using CarRental.Domain;                                      // Behövs för Vehicle

namespace CarRental.Algorithms                              // Egna algoritmer
{
    /// <summary>
    /// ALGORITM: Insättningssortering (Insertion Sort) av sökresultat,
    /// billigast dagpris först.
    ///
    /// Varför insättningssortering?
    ///  - Sökresultatet är litet (ett tiotal fordon) – då är den snabb i praktiken.
    ///  - Den är STABIL: fordon med samma pris behåller sin ordning (regnr-ordning).
    ///  - Den sorterar på plats och är lätt att förstå och förklara.
    /// Tidskomplexitet: O(n²) i värsta fall, O(n) om listan redan är nästan sorterad.
    /// </summary>
    public static class VehicleSorter
    {
        public static void SortByDailyRate(List<Vehicle> vehicles) // Sorterar listan direkt (inget returvärde)
        {
            for (int i = 1; i < vehicles.Count; i++)        // Börja på element 1 – element 0 är "sorterat" redan
            {
                Vehicle current = vehicles[i];              // Fordonet som ska sättas in på rätt plats
                int j = i - 1;                              // Börja jämföra med elementet till vänster

                while (j >= 0 && vehicles[j].DailyRate > current.DailyRate) // Så länge vänstra är dyrare
                {
                    vehicles[j + 1] = vehicles[j];          // Flytta det dyrare fordonet ett steg åt höger
                    j--;                                    // Fortsätt ett steg åt vänster
                }

                vehicles[j + 1] = current;                  // Sätt in fordonet i luckan som blev kvar
            }
        }
    }
}