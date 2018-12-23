using DataModel.TimeSeries;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TimeSeriesGUI
{
    public class MainWindowViewModel : IDisposable, INotifyPropertyChanged
    {
        private RabbitClient client;
        private TimeSeries[] original_series;
        private TimeSeries current_series;
        private DateTime from;
        private DateTime to;

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<TimeSeries> AllSeries { get; private set; }
        public ObservableCollection<TimeSeriesValue> CurrentData { get; private set; }

        public MainWindowViewModel()
        {
            To = DateTime.Now.Date;
            From = To.AddDays(-7);

            AllSeries = new ObservableCollection<TimeSeries>();
            CurrentData = new ObservableCollection<TimeSeriesValue>();

            client = ServiceInjector.GetInstance().GetRabbitClient();
            client.TimeSeriesChanged += Client_TimeSeriesChanged;
            LoadTimeSeriesFromServer();
        }

        private void Client_TimeSeriesChanged()
        {
            System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
            {
                LoadTimeSeriesFromServer();
            });
        }

        public TimeSeries CurrentSeries 
        {
            get { return current_series; }
            set
            {
                current_series = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("HasSeriesSelected");
                LoadTimeSeriesDataFromServer();
            }
        }

        public bool HasSeriesSelected
        {
            get { return current_series != null; }
        }

        private async void LoadTimeSeriesFromServer()
        {
            original_series = await client.GetAllTimeSeries();

            AllSeries.Clear();
            foreach (var s in original_series)
            {
                AllSeries.Add(s);
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

        public DateTime From
        {
            get { return from; }
            set
            {
                from = value;
                NotifyPropertyChanged();
                LoadTimeSeriesDataFromServer();
            }
        }

        public DateTime To
        {
            get { return to; }
            set
            {
                to = value;
                NotifyPropertyChanged();
                LoadTimeSeriesDataFromServer();
            }
        }

        public async void LoadTimeSeriesDataFromServer()
        {
            if (CurrentSeries == null)
                return;

            var data = await client.GetTimeSeriesData(CurrentSeries, new TimeRange(From, To));
            CurrentData.Clear();

            foreach (var d in data.Values)
                CurrentData.Add(d);
        }

        public void Save()
        {
            var data = new TimeSeriesData(CurrentSeries.ID);
            foreach (var d in CurrentData)
            {
                data.Add(d);
            }

            client.SaveTimeSeriesData(data);
        }

        public void Delete()
        {
            client.DeleteTimeSeriesData(CurrentSeries.ID, from, to);
            CurrentData.Clear();
        }

        public void ExportToClipboard()
        {
            TimeSeriesDataExporter.ToClipboard(CurrentData);
        }

        public void ImportFromClipboard()
        {
            CurrentData.Clear();
            foreach (var d in TimeSeriesDataImporter.FromClipboard())
                CurrentData.Add(d);
        }
    }
}