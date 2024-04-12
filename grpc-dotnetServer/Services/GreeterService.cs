using Grpc.AspNetCore.Server;
using Grpc.Core;
using GrpcService;
using Microsoft.AspNetCore.Http;
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

		public override async Task EchoBidir(IAsyncStreamReader<EchoRequest> requestStream, IServerStreamWriter<EchoReply> responseStream, ServerCallContext context)
		{
			await requestStream.MoveNext();

			var reply = new EchoReply() { Reply = "echo test: " + requestStream.Current.Request };
			await responseStream.WriteAsync(reply);
		}

		public override async Task EchoBidirHangup(IAsyncStreamReader<EchoRequest> requestStream, IServerStreamWriter<EchoReply> responseStream, ServerCallContext context)
		{
			await requestStream.MoveNext();

			var reply = new EchoReply() { Reply = "echo hangup test: " + requestStream.Current.Request };
			await responseStream.WriteAsync(reply);

			var reply33 = new EchoReply() { Reply = "quit" };
			await responseStream.WriteAsync(reply33);
		}

	

	
	}
}
