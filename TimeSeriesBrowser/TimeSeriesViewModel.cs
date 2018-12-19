using DataModel.TimeSeries;
using System.Collections.ObjectModel;

namespace TimeSeriesGUI
{
    public class TimeSeriesViewModel
    {
        public TimeSeriesViewModel(TimeSeries ts)
        {
            Series = ts;
            SubSeries = new ObservableCollection<TimeSeriesViewModel>();
        }

        public TimeSeries Series { get; private set; }
        public ObservableCollection<TimeSeriesViewModel> SubSeries { get; set; }
        public TimeSeriesViewModel Parent { get; set; }
    }
}
