using System;

namespace TimeSeries
{
    public class TimeRange
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public TimeRange(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }
    }
}
