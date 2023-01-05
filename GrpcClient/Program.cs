using Grpc.Core;
using Grpc.Net.Client;
using GrpcService;

namespace GrpcClient
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions { Credentials = ChannelCredentials.SecureSsl });


			var client = new Greeter.GreeterClient(channel);

			int i = 0;

			while (true)
			{
				using (var streaming = client.SayHello())
				{
					await streaming.RequestStream.WriteAsync(new HelloRequest() { Name = "hello " + i++ });

					while (await streaming.ResponseStream.MoveNext())
					{
						Console.WriteLine("Message from server: " + streaming.ResponseStream.Current.Message);
					}

					await streaming.RequestStream.CompleteAsync();
				}
			}
		}
	}
}