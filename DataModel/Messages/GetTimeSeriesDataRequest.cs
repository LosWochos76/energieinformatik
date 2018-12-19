using DataModel.TimeSeries;

namespace DataModel.Messages
{
    public class GetTimeSeriesDataRequest
    {
        public int TimeSeriesID { get; set; }
        public TimeRange Range { get; set; }

        public GetTimeSeriesDataRequest(int time_series_id, TimeRange range)
        {
            TimeSeriesID = time_series_id;
            Range = range;
        }
    }
}
