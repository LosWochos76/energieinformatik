using DataModel.TimeSeries;
using System.Windows;

namespace TimeSeriesGUI
{
    public partial class TimeSeriesDialog : Window
    {
        private string name_backup;
        private TimeSeries series;

        public TimeSeriesDialog(TimeSeries series)
        {
            InitializeComponent();

            this.series = series;
            name_backup = series.Name;
            DataContext = series;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            series.Name = name_backup;
            Close();
        }
    }
}
