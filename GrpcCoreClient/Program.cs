using Grpc.Core;
using GrpcService;

namespace GrpcCoreClient
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var channel = new Channel("localhost:5000", ChannelCredentials.Insecure);

			var client = new Greeter.GreeterClient(channel);

			await EchoBidirHangupTest(client);
		}

		private static async Task<int> EchoBidirHangupTest(Greeter.GreeterClient client)
		{
			int i = 1;
			while (true)
			{

				using (var streaming = client.EchoBidirHangup())
				{
					await streaming.RequestStream.WriteAsync(new EchoRequest() { Request = "bidir hangup hello " + i++ });

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