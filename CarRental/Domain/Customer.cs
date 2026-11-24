/// <summary>
/// Representerar en kund som kan hyra fordon. 
/// Validerar indata i konstruktorn för att garantera ett giltigt tillstånd (Inkapsling).
/// </summary>
public class Customer
{
    // Get-only egenskaper: värdena kan bara sättas när kunden skapas (immutability)
    public int Id { get; }
    public string Name { get; }
    public string Phone { get; }

    public Customer(int id, string name, string phone)
    {
        // Skyddar objektet från att skapas med ogiltig eller tom data
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Kundens namn får inte vara tomt.");
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Telefonnummer får inte vara tomt.");

        Id = id;
        Name = name.Trim(); // Tar bort oavsiktliga mellanslag i början/slutet
        Phone = phone.Trim();
    }

    // Överskuggar basklassen Objects ToString() för att ge en läsbar textrepresentation
    public override string ToString()
    {
        return $"Kund {Id}: {Name} ({Phone})";
    }
}
}
   
       
       