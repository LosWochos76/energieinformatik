namespace DataModel.TimeSeries
{
    public class TimeSeries
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Unit Unit { get; set; }

        public string RootName
        {
            get
            {
                return Name.Substring(0, Name.IndexOf('.'));
            }
        }
    }
}