using DataModel.TimeSeries;
using System;
using System.Collections.Generic;
using System.Windows;

namespace TimeSeriesGUI
{
    public class TimeSeriesDataImporter
    {
        public static List<TimeSeriesValue> FromClipboard()
        {
            var text = Clipboard.GetText(TextDataFormat.CommaSeparatedValue);
            var data = new List<TimeSeriesValue>();

            foreach (var line in text.Split('\n'))
            {
                try
                {
                    var parts = line.Split(';');
                    var delivery = Convert.ToDateTime(parts[0]);
                    var value = Convert.ToDouble(parts[1]);
                    data.Add(new TimeSeriesValue() { Delivery = delivery, Value = value });
                }
                catch { }
            }

            return data;
        }
    }
}
