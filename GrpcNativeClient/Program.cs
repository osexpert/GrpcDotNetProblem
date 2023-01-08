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

			while (true)
			{
				using (var streaming = client.SayHello())
				{
					await streaming.RequestStream.WriteAsync(new HelloRequest() { Name = "hello " + i++ });

					while (await streaming.ResponseStream.MoveNext())
					{
						if (streaming.ResponseStream.Current.Message == "quit")
						{
							break;
						}
						else
							Console.WriteLine("Message from server: " + streaming.ResponseStream.Current.Message);
					}

					await streaming.RequestStream.CompleteAsync();
				}
			}
		}
	}
}