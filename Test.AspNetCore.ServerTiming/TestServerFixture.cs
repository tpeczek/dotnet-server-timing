using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using Xunit;

namespace Test.AspNetCore.ServerTiming
{
	/// <summary>
	/// A test fixture which hosts the target project (project we wish to test) in an in-memory server.
	/// </summary>
	/// <typeparam name="TStartup"/>Target project's startup type</typeparam>
	public class TestServerFixture<TStartup> : IDisposable
	{
		private readonly TestServer _server;

		public HttpClient Client { get; }

		public TestServerFixture() : this(null)
		{
		}

		protected TestServerFixture(string relativeTargetProjectParentDir)
		{
			var startupAssembly = typeof(TStartup).GetTypeInfo().Assembly;
			var contentRoot = GetProjectPath(relativeTargetProjectParentDir, startupAssembly);

			Directory.SetCurrentDirectory(contentRoot);

			var builder = new WebHostBuilder()
				.UseContentRoot(contentRoot)
				.ConfigureServices(InitializeServices)
				.UseEnvironment("Development")
				.UseStartup(typeof(TStartup));

			_server = new TestServer(builder);

			Client = _server.CreateClient();
			Client.BaseAddress = new Uri("http://localhost");
		}

		protected virtual void InitializeServices(IServiceCollection services)
		{
			//var startupAssembly = typeof(TStartup).GetTypeInfo().Assembly;

			//// Inject a custom application part manager. 
			//// Overrides AddMvcCore() because it uses TryAdd().
			//var manager = new ApplicationPartManager();
			//manager.ApplicationParts.Add(new AssemblyPart(startupAssembly));
			//manager.FeatureProviders.Add(new ControllerFeatureProvider());
			//manager.FeatureProviders.Add(new ViewComponentFeatureProvider());

			//services.AddSingleton(manager);
		}

		/// <summary>
		/// Gets the full path to the target project that we wish to test
		/// </summary>
		/// <param name="projectRelativePath">
		/// The parent directory of the target project.
		/// e.g. src, samples, test, or test/Websites
		/// </param>
		/// <param name="startupAssembly">The target project's assembly.</param>
		/// <returns>The full path to the target project.</returns>
		private static string GetProjectPath(string projectRelativePath, Assembly startupAssembly)
		{
			// Get name of the target project which we want to test
			var projectName = startupAssembly.GetName().Name;

			projectRelativePath = projectRelativePath ?? projectName;

			// Get currently executing test project path
			var applicationBasePath = AppContext.BaseDirectory;

			// Find the path to the target project
			var directoryInfo = new DirectoryInfo(applicationBasePath);
			do
			{
				directoryInfo = directoryInfo.Parent;

				var projectDirectoryInfo = directoryInfo.EnumerateDirectories().FirstOrDefault(d => d.Name == projectRelativePath);
				if (projectDirectoryInfo != null)
				{
					var projectFileInfo = new FileInfo(Path.Combine(projectDirectoryInfo.FullName, $"{projectName}.csproj"));
					if (projectFileInfo.Exists)
					{
						return projectDirectoryInfo.FullName;
					}
				}
			}
			while (directoryInfo.Parent != null);

			throw new Exception($"Project root could not be located using the application root {applicationBasePath}.");
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
					Client.Dispose();
					_server.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~TestFixture() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion IDisposable Support
	}

	[CollectionDefinition(nameof(TestServerCollection))]
	public class TestServerCollection : ICollectionFixture<TestServerFixture<Startup>>
	{
		// This class has no code, and is never created. Its purpose is simply
		// to be the place to apply [CollectionDefinition] and all the
		// ICollectionFixture<> interfaces.
	}
}
