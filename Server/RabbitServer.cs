using DataModel.Messages;
using DataModel.TimeSeries;
using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server
{
    class RabbitServer : IDisposable
    {
        private MySqlStore store;
        private IBus bus;

        public RabbitServer(MySqlStore store)
        {
            this.store = store;
            this.bus = RabbitHutch.CreateBus("host=localhost");
        }

        public void Serve()
        {
            bus.RespondAsync<GetAllTimeSeriesRequest, TimeSeries[]>(GetAllTimeSeriesAsync);
            bus.RespondAsync<GetTimeSeriesDataRequest, TimeSeriesData>(GetTimeSeriesDataAsync);
        }

        private Task<TimeSeries[]> GetAllTimeSeriesAsync(GetAllTimeSeriesRequest r)
        {
            return Task.Factory.StartNew(() =>
            {
                return store.GetAllTimeSeries();
            });
        }

        private Task<TimeSeriesData> GetTimeSeriesDataAsync(GetTimeSeriesDataRequest r)
        {
            return Task.Factory.StartNew(() =>
            {
                return store.GetTimeSeriesData(r.TimeSeriesID, r.Range);
            });
        }

        public void Dispose()
        {
            this.bus.Dispose();
        }
    }
}
