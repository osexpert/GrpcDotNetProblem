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

		const bool GrpcDotnetBidirStreamNotClosedHacks = true;

		public override async Task EchoBidir(IAsyncStreamReader<EchoRequest> requestStream, IServerStreamWriter<EchoReply> responseStream, ServerCallContext context)
		{
			await requestStream.MoveNext();

			var reply = new EchoReply() { Reply = "Via server : " + requestStream.Current.Request };
			await responseStream.WriteAsync(reply);

			if (GrpcDotnetBidirStreamNotClosedHacks)
			{
				var hdr = context.RequestHeaders;
				var agent = hdr.GetValue("user-agent");
				if (agent != null)
				{
					if (agent.StartsWith("grpc-dotnet/"))
					{
						// dotnet client: needs hangup hack
						var reply3 = new EchoReply() { Reply = "quit" };
						await responseStream.WriteAsync(reply3);
					}
					else if (agent.StartsWith("grpc-csharp/"))
					{
						// native client: needs very dirty hack
						var ctx = context.GetHttpContext();
						var http2stream = ctx.Features.Get<IHttp2StreamIdFeature>();
						var meht = http2stream?.GetType().GetMethod("OnEndStreamReceived", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
						meht?.Invoke(http2stream, null);
					}
				}
			}
		}

		public override async Task<EchoReply> EchoUnary(EchoRequest request, ServerCallContext context)
		{
			return new EchoReply { Reply = "Echoed from server: " + request.Request };
		}
	}
}
