using TransactionAnalyzerService.Models;

namespace TransactionAnalyzerService.Repositories
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetTransactionAsync(string userId);  
        Task<IEnumerable<int>> GetBlockedMerchantsAsync();
        Task BlockMerchant(int merchantId);
    }
}