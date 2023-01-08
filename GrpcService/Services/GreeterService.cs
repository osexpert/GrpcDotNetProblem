using Grpc.Core;
using GrpcService;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core.Features;

namespace GrpcService.Services
{
	public class GreeterService : Greeter.GreeterBase
	{
		private readonly ILogger<GreeterService> _logger;
		public GreeterService(ILogger<GreeterService> logger)
		{
			_logger = logger;
		}

		public override async Task SayHello(IAsyncStreamReader<HelloRequest> requestStream, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
		{
			await requestStream.MoveNext();

			var reply = new HelloReply() { Message = "Via server : " + requestStream.Current.Name };
			await responseStream.WriteAsync(reply);

			var reply3 = new HelloReply() { Message = "quit" };
			await responseStream.WriteAsync(reply3);
		}
	}
}
