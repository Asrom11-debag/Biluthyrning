using System.Text;                                          // Behövs för Encoding (å, ä, ö i konsolen)
using CarRental.Services;                                   
using CarRental.UI;                                         

namespace CarRental                                         
{
   
    /// Startpunkt för programmet. Bygger ihop systemet och startar menyn.
    
    public class Program
    {
        public static void Main(string[] args)              // Main körs först när programmet startar
        {
            Console.OutputEncoding = Encoding.UTF8;         // Så att å, ä, ö och × visas rätt
            RentalService service = SeedData.CreateService(withSampleData: true); 
            ConsoleUI ui = new ConsoleUI(service);          
            ui.Run();                                       // Starta huvudmenyn
        }
    }
}