using TransactionAnalyzerService.Models;
using TransactionAnalyzerService.Repositories;

namespace TransactionAnalyzerService.DomainServices
{
    public class TransactionAnalyzer : ITransactionAnalyzer
    {
        private readonly ITransactionRepository _transactionRepository;
        public TransactionAnalyzer(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public async Task<bool> IsValidTransaction(Transaction transaction)
        {
            var blockedMerchants = await _transactionRepository.GetBlockedMerchantsAsync() ?? new List<int>(); 
            var usersTransactions = await _transactionRepository.GetTransactionAsync(transaction.TransactionId.ToString()) ?? new List<Transaction>();            
            if(blockedMerchants.Any(x => x == transaction.MerchantId))
            {
                return false;
            }

            if(transaction.Amount > 10000
            && !usersTransactions.Any(x => x.MerchantId == transaction.MerchantId 
                                            && x.State == TransactionState.VALID))
            {
                return false;
            }

            if(transaction.Amount > 50000)
            {
                return false;
            }
            
            return true;
        }
    }
}