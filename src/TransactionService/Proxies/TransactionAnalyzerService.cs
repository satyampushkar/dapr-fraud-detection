using TransactionService.Models;

namespace TransactionService.Proxies
{
    public class TransactionAnalyzerService
    {
        private HttpClient _httpClient;
        public TransactionAnalyzerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AnalyzeTransaction(Transaction transaction)
        {
            await _httpClient.PostAsJsonAsync("/analyze", transaction);
        }
    }
}