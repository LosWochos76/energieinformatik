using DataModel.TimeSeries;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Server
{
    public class MySqlStore : IDisposable
    {
        private MySqlConnection connection;

        public MySqlStore(string connection_string)
        {
            connection = new MySqlConnection(connection_string);
            connection.Open();
        }

        public TimeSeries[] GetAllTimeSeries()
        {
            var result = new List<TimeSeries>();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM timeseries order by ID";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(FromReader(reader));
                }
            }

            return result.ToArray();
        }

        public TimeSeries GetTimeSeriesById(int id)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM timeseries where id=@id";
            command.Parameters.AddWithValue("@id", id);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return FromReader(reader);
                }
            }

            return null;
        }

        private static TimeSeries FromReader(MySqlDataReader reader)
        {
            var ts = new TimeSeries();
            ts.ID = Convert.ToInt32(reader["id"]);
            ts.ParentID = reader["parent_id"] != DBNull.Value ? Convert.ToInt32(reader["parent_id"]) : 0;
            ts.Name = reader["name"].ToString();
            ts.Unit = (Unit)Convert.ToInt32(reader["unit"]);
            return ts;
        }

        public TimeSeriesData GetTimeSeriesData(int time_series_id, TimeRange range)
        {
            var series = GetTimeSeriesById(time_series_id);
            var result = new TimeSeriesData(series);

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM timeseries_data where id=@id and delivery>=@from and delivery<=@to order by delivery";
            command.Parameters.AddWithValue("@id", time_series_id);
            command.Parameters.AddWithValue("@from", range.From.ToString("yyyy-MM-dd hh:MM"));
            command.Parameters.AddWithValue("@to", range.To.ToString("yyyy-MM-dd hh:MM"));

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var v = new TimeSeriesValue();
                    v.Delivery = Convert.ToDateTime(reader["delivery"]);
                    v.Value = Convert.ToDouble(reader["value"]);
                    result.Add(v);
                }
            }

            return result;
        }

        public TimeSeriesData GetTimeSeriesData(TimeSeries series, TimeRange range)
        {
            return GetTimeSeriesData(series.ID, range);
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}