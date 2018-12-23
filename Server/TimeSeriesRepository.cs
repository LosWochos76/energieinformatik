using DataModel.Messages;
using DataModel.TimeSeries;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
    public class TimeSeriesRepository
    {
        private MySqlConnection connection;
        private List<TimeSeries> cache = new List<TimeSeries>();

        public TimeSeriesRepository(MySqlConnection connection)
        {
            this.connection = connection;
            FillCache();
        }

        private List<TimeSeries> ReadTimeSeriesObjectsFromDb()
        {
            var result = new List<TimeSeries>();

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM timeseries order by name,id";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(FromReader(reader));
                    }
                }
            }

            return result;
        }

        private void FillCache()
        {
            cache = ReadTimeSeriesObjectsFromDb();
        }

        public TimeSeries[] GetAllTimeSeries()
        {
            return cache.ToArray();
        }

        public TimeSeries GetTimeSeriesById(int id)
        {
            return (from s in cache where s.ID == id select s).SingleOrDefault();
        }

        public TimeSeries GetTimeSeriesByName(string name)
        {
            return (from s in cache where s.Name == name select s).SingleOrDefault();
        }

        public SuccessMessage DeleteTimeSeries(int time_series_id)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "Delete from timeseries_data where id=@id";
                command.Parameters.AddWithValue("@id", time_series_id);
                command.ExecuteNonQuery();
            }

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "Delete from timeseries where id=@id";
                command.Parameters.AddWithValue("@id", time_series_id);
                command.ExecuteNonQuery();
            }

            FillCache();
            return new SuccessMessage(true);
        }

        private static TimeSeries FromReader(MySqlDataReader reader)
        {
            var ts = new TimeSeries();
            ts.ID = Convert.ToInt32(reader["id"]);
            ts.Name = reader["name"].ToString();
            ts.Unit = (Unit)Convert.ToInt32(reader["unit"]);
            return ts;
        }

        public SuccessMessage SaveTimeSeries(TimeSeries series)
        {
            var result = GetTimeSeriesByName(series.Name);

            if (series.ID == 0)
            {
                if (result == null)
                {
                    return SaveNewTimeSeries(series);
                }
                else
                {
                    return new SuccessMessage(false) { ErrorMessage = "Series with that name already exists!" };
                }
            }
            else
            {
                if (result != null && result.ID != series.ID)
                {
                    return new SuccessMessage(false) { ErrorMessage = "Series with that name already exists!" };
                }
                else
                {
                    return UpdateTimeSeries(series);
                }
            }
        }

        private SuccessMessage SaveNewTimeSeries(TimeSeries series)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO timeseries (name, unit) VALUES(@name, @unit)";
                command.Parameters.AddWithValue("@name", series.Name);
                command.Parameters.AddWithValue("@unit", series.Unit);
                command.ExecuteNonQuery();
                series.ID = (int)command.LastInsertedId;
            }

            FillCache();
            return new SuccessMessage(true);
        }

        private SuccessMessage UpdateTimeSeries(TimeSeries series)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "update timeseries set name=@name, unit=@unit where id=@id";
                command.Parameters.AddWithValue("@id", series.ID);
                command.Parameters.AddWithValue("@name", series.Name);
                command.Parameters.AddWithValue("@unit", series.Unit);
                command.ExecuteNonQuery();
            }

            FillCache();
            return new SuccessMessage(true);
        }
    }
}