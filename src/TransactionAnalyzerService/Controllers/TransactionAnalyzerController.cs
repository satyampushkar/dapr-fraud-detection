using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using TransactionAnalyzerService.DomainServices;
using TransactionAnalyzerService.Models;

namespace TransactionAnalyzerService.Controllers
{
    [ApiController]
    [Route("")]
    public class TransactionAnalyzerController : ControllerBase
    {
        private readonly ILogger<TransactionAnalyzerController> _logger;
        private readonly ITransactionAnalyzer _transactionAnalyzer;
        public TransactionAnalyzerController(ILogger<TransactionAnalyzerController> logger, ITransactionAnalyzer transactionAnalyzer)
        {
            _logger = logger;
            _transactionAnalyzer = transactionAnalyzer;
        }

        [HttpPost("analyze")]
        public async Task<IActionResult> Analyze(Transaction transaction,
         [FromServices] DaprClient daprClient)
        {
            try
            {
                if(!await _transactionAnalyzer.IsValidTransaction(transaction))
                {
                    _logger.LogInformation($"Fraud Transaction Detected :: For user {transaction.UserId}" 
                    +$" of amount {transaction.Amount} at merchant {transaction.MerchantId}");

                    await daprClient.PublishEventAsync("kafkapubsub", "fraudtransactions", transaction);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing Analysis");
                return StatusCode(500);
            }
        }
    } 
}