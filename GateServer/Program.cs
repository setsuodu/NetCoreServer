using System;
using System.Threading.Tasks;
using Orleans;
using Orleans.Configuration;

namespace GateServer
{
    public class Program
    {
        private static IClusterClient client;

        //private static TcpServer tcpServer;

        static async void Main(string[] args)
        {
            //await ConnectClient();

            //tcpServer = new TcpServer(client);

            //await tcpServer.StartAsync();
        }
        /*
        private static async Task<IClusterClient> ConnectClient()
        {
            client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "ClusterId";
                    options.ServiceId = "ServiceId";
                });
            await client.Connect();
            return client;
        }*/
    }
}