namespace CarRental.Domain
{
   ///<summary>
   ///Representerar en boknings status. En enum används istället för State-mönsstret
   ///eftersom systemet endast har två fasta lägen utan komplex övergångslogik
   ///</summary>
   public enum BookingStatus
    {
        Active,
        Cancelled
    }
}