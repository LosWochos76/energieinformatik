using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TimeSeries;

namespace TimeSeriesGUI
{
    public class ViewModel
    {
        private MySqlStore store;
        private TimeSeriesViewModel current_series;
        private DateTime from;
        private DateTime to;

        public ObservableCollection<TimeSeriesValue> CurrentData { get; private set; }
        public ObservableCollection<TimeSeriesViewModel> AllSeries { get; private set; }

        public ViewModel()
        {
            To = DateTime.Now.Date.AddDays(1);
            From = To.AddDays(-7);
            AllSeries = new ObservableCollection<TimeSeriesViewModel>();
            CurrentData = new ObservableCollection<TimeSeriesValue>();

            store = new MySqlStore("SERVER=localhost;DATABASE=timeseries;UID=root");
            CreateTimeSeriesViewModel();
        }

        public TimeSeriesViewModel CurrentSeries
        {
            get { return current_series; }
            set
            {
                current_series = value;
                UpdateData();
            }
        }

        private void CreateTimeSeriesViewModel()
        {
            AllSeries.Clear();
            CurrentData.Clear();

            var original = store.GetAllTimeSeries();
            var root = from i in original where i.ParentID == 0 select i;

            foreach (var s in root)
            {
                var vms = new TimeSeriesViewModel(s);
                AllSeries.Add(vms);
                AddSubs(vms, original);
            }
        }

        private void AddSubs(TimeSeriesViewModel tsvm, List<TimeSeries.TimeSeries> original)
        {
            var subs = from i in original where i.ParentID == tsvm.Series.ID select new TimeSeriesViewModel(i);
            tsvm.SubSeries = subs.ToList<TimeSeriesViewModel>();

            foreach (var u in tsvm.SubSeries)
            {
                u.Parent = tsvm;
                AddSubs(u, original);
            }
        }

        public DateTime From
        {
            get { return from; }
            set
            {
                from = value;
                UpdateData();
            }
        }

        public DateTime To
        {
            get { return to; }
            set
            {
                to = value;
                UpdateData();
            }
        }

        private void UpdateData()
        {
            if (current_series == null)
                return;

            var data = store.GetData(current_series.Series, new TimeRange(From, To));
            CurrentData.Clear();

            foreach (var d in data.Values)
                CurrentData.Add(d);
        }
    }
}
