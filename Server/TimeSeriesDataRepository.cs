using DataModel.Messages;
using DataModel.TimeSeries;
using MySql.Data.MySqlClient;
using System;

namespace Server
{
    public class TimeSeriesDataRepository
    {
        private MySqlConnection connection;

        public TimeSeriesDataRepository(MySqlConnection connection)
        {
            this.connection = connection;
        }

        public TimeSeriesData GetTimeSeriesData(int time_series_id, TimeRange range)
        {
            var result = new TimeSeriesData(time_series_id);

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM timeseries_data where id=@id and delivery>=@from and delivery<=@to order by delivery";
                command.Parameters.AddWithValue("@id", time_series_id);
                command.Parameters.AddWithValue("@from", range.From.ToString("yyyy-MM-dd hh:MM"));
                command.Parameters.AddWithValue("@to", range.To.ToString("yyyy-MM-dd hh:MM"));

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var v = new TimeSeriesValue();
                        v.Delivery = Convert.ToDateTime(reader["delivery"]); ;
                        v.Value = Convert.ToDouble(reader["value"]);
                        result.Add(v);
                    }
                }
            }

            return result;
        }

        public SuccessMessage SaveTimeSeriesData(TimeSeriesData data)
        {
            foreach (var d in data.Values)
            {
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO timeseries_data (id, delivery, value) VALUES(@id, @delivery, @value) ON DUPLICATE KEY UPDATE value=@value";
                    command.Parameters.AddWithValue("@id", data.TimeSeriesID);
                    command.Parameters.AddWithValue("@delivery", d.Delivery);
                    command.Parameters.AddWithValue("@value", d.Value);
                    command.ExecuteNonQuery();
                }
            }

            return new SuccessMessage(true);
        }

        public SuccessMessage DeleteTimeSeriesData(int time_series_id, DateTime from, DateTime to)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "Delete from timeseries_data where id=@id and delivery>=@from and delivery<=@to";
                command.Parameters.AddWithValue("@id", time_series_id);
                command.Parameters.AddWithValue("@from", from);
                command.Parameters.AddWithValue("@to", to);
                command.ExecuteNonQuery();
            }

            return new SuccessMessage(true);
        }
    }
}
