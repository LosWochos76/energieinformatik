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
            var tsvm = e.NewValue as TimeSeriesViewModel;
            if (tsvm == null)
                return;

            view_model.CurrentSeries = tsvm;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ServiceInjector.GetInstance().Dispose();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Table_Click(object sender, RoutedEventArgs e)
        {
            var table = new TimeSeriesTable(view_model.CurrentSeries);
            table.Show();
        }

        private void Graph_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
