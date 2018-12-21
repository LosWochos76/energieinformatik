using DataModel.TimeSeries;
using System.Collections.Generic;
using System.Windows;

namespace TimeSeriesGUI
{
    public class TimeSeriesDataExporter
    {
        public static void ToClipboard(IEnumerable<TimeSeriesValue> values)
        {
            var text = "";
            foreach (var d in values)
            {
                text += d.Delivery.ToString("dd.MM.yyyy HH:mm") + ";" + d.Value + "\n";
            }

            Clipboard.SetText(text, TextDataFormat.CommaSeparatedValue);
        }
    }
}