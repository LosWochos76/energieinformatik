using DataModel.Messages;
using DataModel.TimeSeries;
using EasyNetQ;
using EasyNetQ.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimeSeriesGUI
{
    public class RabbitClient : IDisposable
    {
        private IBus bus;

        public RabbitClient()
        {
            LogProvider.SetCurrentLogProvider(ConsoleLogProvider.Instance);
            this.bus = RabbitHutch.CreateBus("host=localhost");
        }

        public void Dispose()
        {
            bus.Dispose();
        }

        public Task<TimeSeries[]> GetAllTimeSeries()
        {
            var request = new GetAllTimeSeriesRequest();
            return bus.RequestAsync<GetAllTimeSeriesRequest, TimeSeries[]>(request);
        }

        public Task<TimeSeriesData> GetTimeSeriesData(TimeSeries series, TimeRange range)
        {
            var request = new GetTimeSeriesDataRequest(series.ID, range);
            return bus.RequestAsync<GetTimeSeriesDataRequest, TimeSeriesData>(request);
        }
    }
}
