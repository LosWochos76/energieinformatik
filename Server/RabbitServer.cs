using DataModel.Messages;
using DataModel.TimeSeries;
using EasyNetQ;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server
{
    class RabbitServer : IDisposable
    {
        private MySqlConnection connection;
        private IBus bus;
        private TimeSeriesRepository ts_repository;
        private TimeSeriesDataRepository tsd_repository;

        public RabbitServer(string connection_string)
        {
            connection = new MySqlConnection(connection_string);
            connection.Open();

            this.ts_repository = new TimeSeriesRepository(connection);
            this.tsd_repository = new TimeSeriesDataRepository(connection);

            this.bus = RabbitHutch.CreateBus("host=localhost");
        }

        public void Serve()
        {
            bus.RespondAsync<GetAllTimeSeriesRequest, TimeSeries[]>(GetAllTimeSeriesAsync);
            bus.RespondAsync<GetTimeSeriesDataRequest, TimeSeriesData>(GetTimeSeriesDataAsync);
            bus.RespondAsync<SaveTimeSeriesDataRequest, SuccessMessage>(SaveTimeSeriesData);
            bus.RespondAsync<DeleteTimeSeriesDataRequest, SuccessMessage>(DeleteTimeSeriesData);
            bus.RespondAsync<DeleteTimeSeriesRequest, SuccessMessage>(DeleteTimeSeries);
            bus.RespondAsync<SaveTimeSeriesRequest, SuccessMessage>(SaveTimeSeries);
        }

        private Task<TimeSeries[]> GetAllTimeSeriesAsync(GetAllTimeSeriesRequest r)
        {
            return Task.Factory.StartNew(() =>
            {
                return ts_repository.GetAllTimeSeries();
            });
        }

        private Task<TimeSeriesData> GetTimeSeriesDataAsync(GetTimeSeriesDataRequest r)
        {
            return Task.Factory.StartNew(() =>
            {
                return tsd_repository.GetTimeSeriesData(r.TimeSeriesID, r.Range);
            });
        }

        public Task<SuccessMessage> SaveTimeSeriesData(SaveTimeSeriesDataRequest r)
        {
            return Task.Factory.StartNew(() =>
            {
                return tsd_repository.SaveTimeSeriesData(r.Data);
            });
        }

        public Task<SuccessMessage> DeleteTimeSeriesData(DeleteTimeSeriesDataRequest r)
        {
            return Task.Factory.StartNew(() =>
            {
                return tsd_repository.DeleteTimeSeriesData(r.TimeSeriesID, r.From, r.To);
            });
        }

        public Task<SuccessMessage> DeleteTimeSeries(DeleteTimeSeriesRequest r)
        {
            return Task.Factory.StartNew(() =>
            {
                return ts_repository.DeleteTimeSeries(r.TimeSeriesID);
            });
        }

        public Task<SuccessMessage> SaveTimeSeries(SaveTimeSeriesRequest r)
        {
            return Task.Factory.StartNew(() =>
            {
                return ts_repository.SaveTimeSeries(r.Series);
            });
        }

        public void Dispose()
        {
            this.connection.Close();
            this.bus.Dispose();
        }
    }
}
