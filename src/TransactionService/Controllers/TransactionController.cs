using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Models;
using TransactionService.Proxies;
using TransactionService.Repositories;

namespace TransactionService.Controllers
{
    [ApiController]
    [Route("")]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly ITransactionRepository _transactionRepository;
        private readonly TransactionAnalyzerService _transactionAnalyzerService;

        public TransactionController(
            ITransactionRepository transactionRepository,
            TransactionAnalyzerService transactionAnalyzerService,
            ILogger<TransactionController> logger)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
            _transactionAnalyzerService = transactionAnalyzerService;
        }

        [HttpPost("newtransaction")]
        public async Task<ActionResult> Post(Transaction transaction,
        [FromServices] DaprClient daprClient)
        {
            try
            {
                _logger.LogInformation($"New transaction :: For user {transaction.UserId}" 
                    +$" of amount {transaction.Amount} at merchant {transaction.MerchantId}");
                transaction.State = TransactionState.NEW;
                await _transactionRepository.SaveTransactionAsync(transaction);

                await _transactionAnalyzerService.AnalyzeTransaction(transaction);

                // var request = daprClient.CreateInvokeMethodRequest("transactionanalyzerservice", "analyze", transaction);
                // var response = await daprClient.InvokeMethodWithResponseAsync(request);
                // response.EnsureSuccessStatusCode();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing new Transaction");
                return StatusCode(500);
            }
        }

        [HttpDelete("deleteTransaction")]
        public async Task<ActionResult> Delete(Transaction transaction)
        {
            try
            {
                _logger.LogInformation($"Deleting fraud transaction :: For user {transaction.UserId}" 
                    +$"for amount {transaction.Amount} at merchant {transaction.MerchantId}");
                transaction.State= TransactionState.INVALID;
                await _transactionRepository.DeleteTransactionAsync(transaction);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing delete Transaction");
                return StatusCode(500);
            }
        }
    }
}