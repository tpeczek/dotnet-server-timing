using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using Xunit;

namespace Test.Azure.Functions.Worker.ServerTiming.Infrastructure
{
    public class AzureFunctionsTestcontainersFixture : IAsyncLifetime
    {
        private readonly IFutureDockerImage _azureFunctionsDockerImage;

        public IContainer AzureFunctionsContainerInstance { get; private set; }

        public AzureFunctionsTestcontainersFixture()
        {
            _azureFunctionsDockerImage = new ImageFromDockerfileBuilder()
                .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), String.Empty)
                .WithDockerfile("AzureFunctions-Testcontainers.Dockerfile")
                .WithBuildArgument("RESOURCE_REAPER_SESSION_ID", ResourceReaper.DefaultSessionId.ToString("D"))
                .Build();
        }

        public async Task InitializeAsync()
        {
            await _azureFunctionsDockerImage.CreateAsync();

            AzureFunctionsContainerInstance = new ContainerBuilder()
                .WithImage(_azureFunctionsDockerImage)
                .WithPortBinding(80, true)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(80)))
                .Build();
            await AzureFunctionsContainerInstance.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await AzureFunctionsContainerInstance.DisposeAsync();

            await _azureFunctionsDockerImage.DisposeAsync();
        }
    }
}
