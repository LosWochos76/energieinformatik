using System;

namespace DataModel.Messages
{
    public class DeleteTimeSeriesDataRequest
    {
        public int TimeSeriesID { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
