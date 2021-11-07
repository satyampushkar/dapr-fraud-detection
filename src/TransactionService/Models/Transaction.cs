namespace TransactionService.Models
{
    public class Transaction
    {
        public Guid TransactionId { get; set; }
        public TransactionState State { get; set; }
        public int UserId { get; set; }
        public int MerchantId { get; set; }
        public int Amount { get; set; }        
        public DateTime Timestamp { get; set; }
    }
}