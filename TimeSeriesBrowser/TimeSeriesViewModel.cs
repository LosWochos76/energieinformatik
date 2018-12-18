using System.Collections.Generic;

namespace TimeSeriesGUI
{
    public class TimeSeriesViewModel
    {
        private TimeSeries.TimeSeries ts;

        public TimeSeriesViewModel(TimeSeries.TimeSeries ts)
        {
            this.ts = ts;
        }

        public TimeSeries.TimeSeries Series
        {
            get { return ts; }
        }

        public List<TimeSeriesViewModel> SubSeries { get; set; }
        public TimeSeriesViewModel Parent { get; set; }
    }
}
