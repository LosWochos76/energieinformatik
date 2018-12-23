using DataModel.TimeSeries;
using System;
using System.Windows;

namespace TimeSeriesGUI
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel view_model;

        public MainWindow()
        {
            InitializeComponent();

            view_model = new MainWindowViewModel();
            DataContext = view_model;
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var ts = e.NewValue as TimeSeries;
            if (ts == null)
                return;

            view_model.CurrentSeries = ts;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ServiceInjector.GetInstance().Dispose();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ToClipboard_Click(object sender, RoutedEventArgs e)
        {
            view_model.ExportToClipboard();
        }

        private void FromClipboard_Click(object sender, RoutedEventArgs e)
        {
            view_model.ImportFromClipboard();
        }

        private void Save_Data_Click(object sender, RoutedEventArgs e)
        {
            view_model.Save();
        }

        private void Delete_Data_Click(object sender, RoutedEventArgs e)
        {
            view_model.Delete();
        }

        private async void New_TimeSeries_Click(object sender, RoutedEventArgs e)
        {
            var series = new TimeSeries();
            var dialog = new TimeSeriesDialog(series);
            var result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                var client_result = await ServiceInjector.GetInstance().GetRabbitClient().SaveTimeSeries(series);
                if (client_result.Success)
                {
                    view_model.CurrentSeries = series;
                }
                else
                {
                    MessageBox.Show(this, "Error: " + client_result.ErrorMessage);
                }
            }
        }

        private async void Edit_TimeSeries_Click(object sender, RoutedEventArgs e)
        {
            var series = view_model.CurrentSeries;
            var dialog = new TimeSeriesDialog(series);
            var result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                var client_result = await ServiceInjector.GetInstance().GetRabbitClient().SaveTimeSeries(series);
                if (!client_result.Success)
                { 
                    MessageBox.Show(this, "Error: " + client_result.ErrorMessage);
                }
            }
        }

        private async void Delete_TimeSeries_Click(object sender, RoutedEventArgs e)
        {
            var series = view_model.CurrentSeries;
            var client_result = await ServiceInjector.GetInstance().GetRabbitClient().DeleteTimeSeries(series.ID);

            if (client_result.Success)
            {
                view_model.CurrentSeries = null;
            }
            else
            {
                MessageBox.Show(this, "Error: " + client_result.ErrorMessage);
            }
        }
    }
}