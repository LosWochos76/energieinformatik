using System.Windows;

namespace TimeSeriesGUI
{
    public partial class MainWindow : Window
    {
        private ViewModel view_model;

        public MainWindow()
        {
            InitializeComponent();

            view_model = new ViewModel();
            DataContext = view_model;
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var tsvm = e.NewValue as TimeSeriesViewModel;
            if (tsvm == null)
                return;

            view_model.CurrentSeries = tsvm;
        }
    }
}
