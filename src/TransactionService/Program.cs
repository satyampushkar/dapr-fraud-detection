using Dapr.Client;
using TransactionService.Proxies;
using TransactionService.Repositories;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers().AddDapr();
builder.Services.AddSingleton<ITransactionRepository, TransactionRepository>();

var daprHttpPort = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3600";
builder.Services.AddSingleton<TransactionAnalyzerService>(_ => 
                new TransactionAnalyzerService(DaprClient.CreateInvokeHttpClient(
                    "transactionanalyzerservice", $"http://localhost:{daprHttpPort}")));
// var daprHttpPort = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3600";
// var daprGrpcPort = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT") ?? "60000";
// builder.Services.AddDaprClient(builder => builder
//     .UseHttpEndpoint($"http://localhost:{daprHttpPort}")
//     .UseGrpcEndpoint($"http://localhost:{daprGrpcPort}"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
