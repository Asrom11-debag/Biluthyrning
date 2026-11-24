using System.Globalization;                                 

namespace CarRental.UI                                      
{
    
    /// Säker inläsning från tangentbordet (Robusthet).
    /// Alla metoder frågar om och om igen tills användaren skriver något giltigt,
    /// så programmet kraschar aldrig på felaktig inmatning.
    
    public static class InputHelper
    {
        public const string DateFormat = "yyyy-MM-dd";      

        //Läser en rad. Om inmatningen tar slut avslutas programmet snyggt.
        public static string ReadLine(string prompt)
        {
            Console.Write(prompt);                          // Skriv frågan utan radbrytning
            string line = Console.ReadLine();              
            if (line == null)                               
            {
                Console.WriteLine("\nInmatningen tog slut – programmet avslutas."); 
                Environment.Exit(0);                        // Avsluta programmet utan krasch
            }
            return line.Trim();                             // Returnera svaret utan mellanslag runt
        }

        //Läser ett heltal mellan min och max (inklusive).
        public static int ReadInt(string prompt, int min, int max)
        {
            while (true)                                    // Fråga tills svaret är giltigt
            {
                string text = ReadLine(prompt);             
                if (int.TryParse(text, out int value) && value >= min && value <= max) 
                    return value;                           
                Console.WriteLine($"  Ange ett heltal mellan {min} och {max}."); 
            }
        }

        /// Läser ett datum i formatet ÅÅÅÅ-MM-DD
        public static DateTime ReadDate(string prompt)
        {
            while (true)                                    // Fråga tills datumet är giltigt
            {
                string text = ReadLine(prompt);             
                if (DateTime.TryParseExact(text, DateFormat, CultureInfo.InvariantCulture,
                                           DateTimeStyles.None, out DateTime date)) 
                    return date;                           
                Console.WriteLine($"  Felaktigt datum. Använd formatet {DateFormat}, t.ex. {DateTime.Today.AddDays(1):yyyy-MM-dd}."); 
            }
        }

        /// Läser en text som inte får vara tom.
        public static string ReadText(string prompt)
        {
            while (true)                                    
            {
                string text = ReadLine(prompt);             
                if (text.Length > 0)                        
                    return text;                          
                Console.WriteLine("  Fältet får inte vara tomt.");
            }
        }

        /// Läser j/n och returnerar true för ja.
        public static bool ReadYesNo(string prompt)
        {
            while (true)                                    // Fråga tills svaret är j eller n
            {
                string text = ReadLine(prompt + " (j/n): ").ToLower(); 
                if (text == "j" || text == "ja")            
                    return true;                            
                if (text == "n" || text == "nej")           
                    return false;                           
                Console.WriteLine("  Svara j eller n.");    
            }
        }
        
        public static void Pause()
        {
            ReadLine("\nTryck Enter för att fortsätta...");  
        }
    }
}
