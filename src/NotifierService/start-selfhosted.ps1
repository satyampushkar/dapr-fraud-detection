dapr run `
    --app-id notifierservice `
    --app-port 6003 `
    --dapr-http-port 3603 `
    --dapr-grpc-port 60003 `
    --config ../dapr/config/config.yaml `
    --components-path ../dapr/components `
    dotnet run