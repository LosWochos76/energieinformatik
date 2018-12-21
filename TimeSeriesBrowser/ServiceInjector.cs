using System;

namespace TimeSeriesGUI
{
    public class ServiceInjector : IDisposable
    {
        private RabbitClient client = null;
        private static ServiceInjector instance = null;

        private ServiceInjector()
        {
            this.client = new RabbitClient();
        }

        public static ServiceInjector GetInstance()
        {
            if (instance == null)
            {
                instance = new ServiceInjector();
            }

            return instance;
        }

        public RabbitClient GetRabbitClient()
        {
            return client;
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
