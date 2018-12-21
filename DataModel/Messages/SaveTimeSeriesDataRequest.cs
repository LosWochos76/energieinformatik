using DataModel.TimeSeries;

namespace DataModel.Messages
{
    public class SaveTimeSeriesDataRequest
    {
        public TimeSeriesData Data { get; set; }

        public SaveTimeSeriesDataRequest(TimeSeriesData data)
        {
            this.Data = data;
        }
    }
}