using CarRental.Domain;

namespace CarRental.Algorithms
{
    // Insättningssortering: billigaste bilen först.
    public static class VehicleSorter
    {
        public static void SortByDailyRate(List<Vehicle> vehicles)
        {
            for (int i = 1; i < vehicles.Count; i++)
            {
                Vehicle current = vehicles[i];              // Bilen vi ska placera
                int j = i - 1;

                while (j >= 0 && vehicles[j].DailyRate > current.DailyRate)
                {
                    vehicles[j + 1] = vehicles[j];          // Flytta dyrare bil åt höger
                    j--;
                }

                vehicles[j + 1] = current;                  // Sätt in bilen
            }
        }
    }
}