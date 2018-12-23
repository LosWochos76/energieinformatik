using DataModel.Messages;
using DataModel.TimeSeries;
using EasyNetQ;
using EasyNetQ.Logging;
using System;
using System.Threading.Tasks;

namespace TimeSeriesGUI
{
    public delegate void TimeSeriesChangedHandler();

    public class RabbitClient : IDisposable
    {
        private IBus bus;

        public event TimeSeriesChangedHandler TimeSeriesChanged;

        public RabbitClient()
        {
            LogProvider.SetCurrentLogProvider(ConsoleLogProvider.Instance);
            this.bus = RabbitHutch.CreateBus("host=localhost");

            bus.SubscribeAsync<TimeSeriesChanged>("client", message => Task.Factory.StartNew(() =>
            {
                RaiseTimeSeriesChangedEvent();
            }));
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

        public Task<SuccessMessage> SaveTimeSeriesData(TimeSeriesData data)
        {
            var request = new SaveTimeSeriesDataRequest(data);
            return bus.RequestAsync<SaveTimeSeriesDataRequest, SuccessMessage>(request);
        }

        public Task<SuccessMessage> DeleteTimeSeriesData(int time_series_id, DateTime from, DateTime to)
        {
            var request = new DeleteTimeSeriesDataRequest() { TimeSeriesID = time_series_id, From = from, To = to };
            return bus.RequestAsync<DeleteTimeSeriesDataRequest, SuccessMessage>(request);
        }

        public Task<SuccessMessage> SaveTimeSeries(TimeSeries series)
        {
            var request = new SaveTimeSeriesRequest() { Series = series };
            return bus.RequestAsync<SaveTimeSeriesRequest, SuccessMessage>(request);
        }

        public Task<SuccessMessage> DeleteTimeSeries(int time_series_id)
        {
            var request = new DeleteTimeSeriesRequest() { TimeSeriesID = time_series_id };
            return bus.RequestAsync<DeleteTimeSeriesRequest, SuccessMessage>(request);
        }

        private void RaiseTimeSeriesChangedEvent()
        {
            TimeSeriesChanged?.Invoke();
        }
    }
}