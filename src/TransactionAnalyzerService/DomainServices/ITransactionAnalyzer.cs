using TransactionAnalyzerService.Models;

namespace TransactionAnalyzerService.DomainServices
{
    public interface ITransactionAnalyzer
    {
        public Task<bool> IsValidTransaction(Transaction transaction);
    }
}