using Dapr.Client;
using TransactionService.Models;

namespace TransactionService.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private const string DAPR_STORE_NAME = "statestore";
        private readonly DaprClient _daprClient;
        public TransactionRepository(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }
        public async Task DeleteTransactionAsync(Transaction transaction)
        {
            var tansactions = await _daprClient.GetStateAsync<IEnumerable<Transaction>>(DAPR_STORE_NAME, transaction.UserId.ToString());
            var toDeleteTransaction = tansactions.ToList().FirstOrDefault(x => x.TransactionId == transaction.TransactionId);
            if(toDeleteTransaction != null)
            {
                toDeleteTransaction.State = TransactionState.INVALID;
            }
            await _daprClient.SaveStateAsync<IEnumerable<Transaction>>(DAPR_STORE_NAME, transaction.UserId.ToString(), tansactions);
        }

        public async Task SaveTransactionAsync(Transaction transaction)
        {
            var tansactions = await _daprClient.GetStateAsync<IEnumerable<Transaction>>(DAPR_STORE_NAME, transaction.UserId.ToString());
            if (tansactions == null)
            {
                tansactions = new List<Transaction>();
            }
            tansactions.ToList().Add(transaction);
            await _daprClient.SaveStateAsync<IEnumerable<Transaction>>(DAPR_STORE_NAME, transaction.UserId.ToString(), tansactions);
        }
    }
}