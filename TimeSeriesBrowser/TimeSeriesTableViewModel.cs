using DataModel.TimeSeries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TimeSeriesGUI
{
    public class TimeSeriesTableViewModel : INotifyPropertyChanged
    {
        private RabbitClient client;
        private DateTime from;
        private DateTime to;
        private TimeSeriesData data;

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<TimeSeriesValue> CurrentData { get; private set; }

        public TimeSeriesTableViewModel(TimeSeriesViewModel series)
        {
            this.client = ServiceInjector.GetInstance().GetRabbitClient();
            Series = series.Series;
            CurrentData = new ObservableCollection<TimeSeriesValue>();

            to = DateTime.Now.Date.AddDays(1);
            from = To.AddDays(-7);
            ReloadDataFromServer();
        }

        public TimeSeries Series { get; private set; }

        public DateTime From
        {
            get { return from; }
            set
            {
                from = value;
                NotifyPropertyChanged();
                ReloadDataFromServer();
            }
        }

        public DateTime To
        {
            get { return to; }
            set
            {
                to = value;
                NotifyPropertyChanged();
                ReloadDataFromServer();
            }
        }

        public async void ReloadDataFromServer()
        {
            data = await client.GetTimeSeriesData(Series, new TimeRange(From, To));
            CopyDataFromSeriesIntoViewModel();
        }

        private void CopyDataFromSeriesIntoViewModel()
        {
            CurrentData.Clear();

            foreach (var d in data.Values)
                CurrentData.Add(d);
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Save()
        {
            var ts_data = new TimeSeriesData(Series.ID);
            foreach (var d in CurrentData)
                ts_data.Add(d);

            client.SaveTimeSeriesData(ts_data);
        }

        public void Add(IEnumerable<TimeSeriesValue> other_data)
        {
            data.Add(other_data);
            CopyDataFromSeriesIntoViewModel();
        }
    }
}
