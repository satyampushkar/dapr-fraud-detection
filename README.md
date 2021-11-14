# dapr-fraud-detection
 Sample app to Understand DAPR development using dotnet in self-hosted mode.
 
 * Initialize dapr

   Run the command 
   
   `dapr init`

    `dapr --version`

 * Start Infrastructure
 
   Change directory to 'src\Infrastructure' and run 

   `start-all.ps1`
 
 * Start services in self-hosted mode
 
   Change directory to 'src\TransactionService' and run

   `start-selfhosted.ps1`

    Change directory to 'src\TransactionAnalyzerService' and run

   `start-selfhosted.ps1`

    Change directory to 'src\NotifierService' and run

   `start-selfhosted.ps1`

  Change directory to 'src\TransactionSimulation' and run

   `dotnet run`
