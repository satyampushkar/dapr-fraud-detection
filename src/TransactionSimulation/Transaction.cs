namespace TransactionSimulation
{
    public class Transaction
    {
        public Guid TransactionId { get; set; }
        public int UserId { get; set; }
        public int MerchantId { get; set; }
        public int Amount { get; set; }        
        public DateTime Timestamp { get; set; }
    }
}