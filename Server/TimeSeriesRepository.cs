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
                command.CommandText = "SELECT * FROM timeseries order by ID";
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
            RestoreFullNames();
        }

        private void RestoreFullNames()
        {
            foreach (var t in from s in cache where s.ParentID == 0 select s)
            {
                t.FullName = t.Name;
                RestoreFullNames(t);
            }
        }

        private void RestoreFullNames(TimeSeries t)
        {
            var roots = from s in cache where s.ParentID == t.ID select s;
            foreach (var u in roots)
            {
                u.FullName = t.FullName + "." + u.Name;
                RestoreFullNames(u);
            }
        }

        public TimeSeries[] GetAllTimeSeries()
        {
            return cache.ToArray();
        }

        public TimeSeries GetTimeSeriesById(int id)
        {
            return (from s in cache where s.ID == id select s).SingleOrDefault();
        }

        public TimeSeries GetTimeSeriesByFullName(string full_name)
        {
            return (from s in cache where s.FullName == full_name select s).SingleOrDefault();
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

            return new SuccessMessage(true);
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
    }
}
