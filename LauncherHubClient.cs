using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalR_Test
{
    public class LauncherHubClient
    {
        const int MaxSendRetryCount = 5;
        readonly HubConnection _connection;
        readonly string _url;

        public LauncherHubClient()
        {
            _url = "https://account.cryproxy.ru/LaunchersHub";
            _connection = new HubConnectionBuilder().
                WithUrl(_url).
                Build();
            _connection.Closed += _connection_Closed;
        }

        public void Start()
        {
            _ = Connect();
        }

        async Task Connect()
        {
            Console.WriteLine("Connecting to SignalR hub");
            await _connection.StartAsync();
            Console.WriteLine("Connected to hub");
        }

        async Task _connection_Closed(Exception ex)
        {
            Console.WriteLine("Hub closed: " + ex);

            //get exception thrown
            GC.Collect();
            GC.WaitForPendingFinalizers();

            await Reconnect();
        }

        async Task Reconnect()
        {
            await Task.Delay(TimeSpan.FromMinutes(1));
            try
            {
                await Connect();
            }
            catch
            {
                await Reconnect();
            }
        }
    }
}