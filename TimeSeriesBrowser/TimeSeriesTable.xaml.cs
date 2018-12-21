using DataModel.TimeSeries;
using System;
using System.Collections.Generic;
using System.Windows;

namespace TimeSeriesGUI
{
    public partial class TimeSeriesTable : Window
    {
        private TimeSeriesTableViewModel view_model;

        public TimeSeriesTable(TimeSeriesViewModel series)
        {
            InitializeComponent();

            Title = series.Series.FullName;
            view_model = new TimeSeriesTableViewModel(series);
            DataContext = view_model;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            view_model.Save();
        }

        private void ToClipboard_Click(object sender, RoutedEventArgs e)
        {
            TimeSeriesDataExporter.ToClipboard(view_model.CurrentData);
        }

        private void FromClipboard_Click(object sender, RoutedEventArgs e)
        {
            var data = TimeSeriesDataImporter.FromClipboard();
            view_model.Add(data);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            ServiceInjector.GetInstance().GetRabbitClient().DeleteTimeSeriesData(view_model.Series.ID, view_model.From, view_model.To);
            view_model.ReloadDataFromServer();
        }
    }
}
