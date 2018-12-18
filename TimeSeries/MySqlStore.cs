using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace TimeSeries
{
    public class MySqlStore
    {
        private MySqlConnection connection;

        public MySqlStore(string connection_string)
        {
            connection = new MySqlConnection(connection_string);
            connection.Open();
        }

        public List<TimeSeries> GetAllTimeSeries()
        {
            var result = new List<TimeSeries>();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM timeseries order by ID";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var ts = new TimeSeries();
                    ts.ID = Convert.ToInt32(reader["id"]);
                    ts.ParentID = reader["parent_id"] != DBNull.Value ? Convert.ToInt32(reader["parent_id"]) : 0;
                    ts.Name = reader["name"].ToString();
                    ts.Unit = (Unit)Convert.ToInt32(reader["unit"]);
                    result.Add(ts);
                }
            }

            return result;
        }

        public TimeSeriesData GetData(TimeSeries series, TimeRange range)
        {
            var result = new TimeSeriesData(series);

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM timeseries_data where id=@id and delivery>=@from and delivery<=@to order by delivery";
            command.Parameters.AddWithValue("@id", series.ID);
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
    }
}