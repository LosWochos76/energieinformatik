using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using DataModel.TimeSeries;

namespace TimeSeriesGUI
{
    public class MainWindowViewModel : IDisposable, INotifyPropertyChanged
    {
        private RabbitClient client;
        private TimeSeriesViewModel current_series;

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<TimeSeriesViewModel> AllSeries { get; private set; }

        public MainWindowViewModel()
        {
            AllSeries = new ObservableCollection<TimeSeriesViewModel>();
            client = ServiceInjector.GetInstance().GetRabbitClient();
            CreateTimeSeriesViewModel();
        }

        public TimeSeriesViewModel CurrentSeries 
        {
            get { return current_series; }
            set
            {
                current_series = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("HasSeriesSelected");
            }
        }

        public bool HasSeriesSelected
        {
            get { return current_series != null; }
        }

        private async void CreateTimeSeriesViewModel()
        {
            AllSeries.Clear();

            var original = await client.GetAllTimeSeries();
            var root = from i in original where i.ParentID == 0 select i;

            foreach (var s in root)
            {
                var vms = new TimeSeriesViewModel(s);
                AllSeries.Add(vms);
                AddSubs(vms, original);
            }
        }

        private void AddSubs(TimeSeriesViewModel tsvm, TimeSeries[] original)
        {
            var subs = from i in original where i.ParentID == tsvm.Series.ID select new TimeSeriesViewModel(i);

            foreach (var s in subs)
            {
                tsvm.SubSeries.Add(s);
            }

            foreach (var u in tsvm.SubSeries)
            {
                u.Parent = tsvm;
                AddSubs(u, original);
            }
        }

        public void Dispose()
        {
            this.client.Dispose();
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}