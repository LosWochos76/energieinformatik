using System.Collections.Generic;

namespace DataModel.TimeSeries
{
    public class TimeSeriesData
    {
        public TimeSeries Series { get; private set; }
        private List<TimeSeriesValue> values = new List<TimeSeriesValue>();

        public TimeSeriesData(TimeSeries series)
        {
            Series = series;
        }

        public void Add(TimeSeriesValue v)
        {
            values.Add(v);
        }

        public List<TimeSeriesValue> Values
        {
            get { return values; }
        }
    }
}
