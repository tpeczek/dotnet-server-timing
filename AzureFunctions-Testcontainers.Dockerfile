FROM mcr.microsoft.com/dotnet/sdk:7.0 AS installer-env
ARG RESOURCE_REAPER_SESSION_ID="00000000-0000-0000-0000-000000000000"
LABEL "org.testcontainers.resource-reaper-session"=$RESOURCE_REAPER_SESSION_ID
COPY ./demos/Demo.Azure.Functions.Worker.ServerTiming /demos/Demo.Azure.Functions.Worker.ServerTiming
COPY ./src/Lib.Azure.Functions.Worker.ServerTiming /src/Lib.Azure.Functions.Worker.ServerTiming
COPY ./src/Lib.ServerTiming.Abstractions /src/Lib.ServerTiming.Abstractions
RUN cd /demos/Demo.Azure.Functions.Worker.ServerTiming && \
    mkdir -p /home/site/wwwroot && \
    dotnet publish *.csproj --output /home/site/wwwroot

FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated7.0
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

COPY --from=installer-env ["/home/site/wwwroot", "/home/site/wwwroot"]