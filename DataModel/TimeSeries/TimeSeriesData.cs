using System;
using System.Collections.Generic;
using System.Linq;

namespace DataModel.TimeSeries
{
    public class TimeSeriesData
    {
        public int TimeSeriesID { get; set; }
        public List<TimeSeriesValue> Values { get; set; }

        public TimeSeriesData(int time_series_id)
        {
            TimeSeriesID = time_series_id;
            Values = new List<TimeSeriesValue>();
        }

        public void Add(TimeSeriesValue v)
        {
            Values.Add(v);
        }

        public void Set(DateTime d, double value)
        {
            foreach (var v in Values)
            {
                if (v.Delivery == d)
                {
                    v.Value = value;
                    return;
                }
            }

            Values.Add(new TimeSeriesValue() { Delivery = d, Value = value });
        }

        public void Add(IEnumerable<TimeSeriesValue> values)
        {
            foreach (var d in values)
            {
                Set(d.Delivery, d.Value);
            }
        }
    }
}