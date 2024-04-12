using Grpc.Core;
using GrpcService;

namespace GrpcCoreServer
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var s = new Server()
			{
				Services = { Greeter.BindService(new MyService()) },
				Ports = { new ServerPort("localhost", 5000, ServerCredentials.Insecure) }
			};

			s.Start();

			await s.ShutdownTask;
		}
	}

	class MyService : Greeter.GreeterBase
	{
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