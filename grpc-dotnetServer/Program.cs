using GrpcService.Services;

namespace grpcdotnetServer
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Logging make us slower and then the leak will not always (as easily) manifest
			builder.Logging.ClearProviders();

			// Additional configuration is required to successfully run gRPC on macOS.
			// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

			// Add services to the container.
			builder.Services.AddGrpc();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			app.MapGrpcService<GreeterService>();
			app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

			app.Run();
		}
	}
}