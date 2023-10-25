FROM mcr.microsoft.com/dotnet/sdk:7.0 AS installer-env
ARG RESOURCE_REAPER_SESSION_ID="00000000-0000-0000-0000-000000000000"
LABEL "org.testcontainers.resource-reaper-session"=$RESOURCE_REAPER_SESSION_ID

WORKDIR /src
COPY src/Lib.Azure.Functions.Worker.ServerTiming/ ./Lib.Azure.Functions.Worker.ServerTiming/
COPY src/Lib.ServerTiming.Abstractions/ ./Lib.ServerTiming.Abstractions/

WORKDIR /demos
COPY demos/Demo.Azure.Functions.Worker.ServerTiming/ ./Demo.Azure.Functions.Worker.ServerTiming/

RUN dotnet publish Demo.Azure.Functions.Worker.ServerTiming \
    --output /home/site/wwwroot \

FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated7.0

ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

COPY --from=installer-env ["/home/site/wwwroot", "/home/site/wwwroot"]