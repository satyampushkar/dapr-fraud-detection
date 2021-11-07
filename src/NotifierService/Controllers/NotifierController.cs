using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using NotifierService.Helpers;
using NotifierService.Models;

namespace NotifierService.Controllers
{
    [ApiController]
    [Route("")]
    public class NotifierController : ControllerBase
    {
        public readonly ILogger<NotifierController> _logger;
        public NotifierController(ILogger<NotifierController> logger)
        {
            _logger = logger;
        }

        [Topic("pubsub", "fraudtransactions")]
        [Route("notify")]
        [HttpPost]
        public async Task<IActionResult> Notify(Transaction transaction, [FromServices] DaprClient daprClient)
        {
            try
            {
                _logger.LogInformation($"Notifying User: {transaction.UserId}");
                var body = EmailUtils.CreateEmailBody(transaction);
                var metadata = new Dictionary<string, string>
                {
                    ["emailFrom"] = "noreply@creditcard.mybank.com",
                    ["emailTo"] = $"{transaction.UserId}@creditcard.mybank.com",
                    ["subject"] = $"Fraud Transaction detected on your card for amount: {transaction.Amount} at merchant: {transaction.MerchantId}"
                };
                await daprClient.InvokeBindingAsync("sendmail", "create", body, metadata);
                
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing new Transaction");
                return StatusCode(500);
            }
        }
    }
}