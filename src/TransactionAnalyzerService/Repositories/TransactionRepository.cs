using Dapr.Client;
using TransactionAnalyzerService.Models;

namespace TransactionAnalyzerService.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        //private const string DAPR_STORE_NAME = "statestore";
        private const string DAPR_STORE_NAME = "mongostatestore";
        private const string DAPR_BLOCKED_MERCHANT_IDS = "blockedMerchantIds";
        private readonly DaprClient _daprClient;

        public TransactionRepository(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        public async Task BlockMerchant(int merchantId)
        {
            var blockedMerchants = await  _daprClient.GetStateAsync<IEnumerable<int>>(DAPR_STORE_NAME, DAPR_BLOCKED_MERCHANT_IDS);
            blockedMerchants.ToList().Add(merchantId);
            await _daprClient.SaveStateAsync(DAPR_STORE_NAME, DAPR_BLOCKED_MERCHANT_IDS, blockedMerchants);
        }

        public async Task<IEnumerable<int>> GetBlockedMerchantsAsync()
        {
            return await _daprClient.GetStateAsync<IEnumerable<int>>(DAPR_STORE_NAME, DAPR_BLOCKED_MERCHANT_IDS);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionAsync(string userId)
        {
            return await _daprClient.GetStateAsync<IEnumerable<Transaction>>(DAPR_STORE_NAME, userId);
        }
    }
}