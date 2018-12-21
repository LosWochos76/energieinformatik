using EasyNetQ.Logging;
using System;

namespace Server
{
    class Program
    {
        private static string connection_string = "SERVER=localhost;DATABASE=timeseries;UID=root";

        static void Main(string[] args)
        {
            LogProvider.SetCurrentLogProvider(ConsoleLogProvider.Instance);

            using (var handler = new RabbitServer(connection_string))
            {
                handler.Serve();
                Console.WriteLine("Server started. Press Enter to stop...");
                Console.ReadLine();
            }
        }
    }
}
