using Grpc.Core;
using GrpcService;

namespace GrpcNativeClient
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var channel = new Channel("localhost:5000", ChannelCredentials.Insecure);

			var client = new Greeter.GreeterClient(channel);

			int i = 0;

			//while (true)
			//{
			//	var res = client.EchoUnary(new EchoRequest { Request = "unary hello " + i++ });
			//	Console.WriteLine("Message from server: " + res.Reply);
			//}

			while (true)
			{
				using (var streaming = client.EchoBidir())
				{
					await streaming.RequestStream.WriteAsync(new EchoRequest() { Request = "bidir hello " + i++ });

					while (await streaming.ResponseStream.MoveNext())
					{
						if (streaming.ResponseStream.Current.Reply == "quit")
						{
							break;
						}
						else
							Console.WriteLine("Message from server: " + streaming.ResponseStream.Current.Reply);
					}

					await streaming.RequestStream.CompleteAsync();
				}
			}
		}
	}
}