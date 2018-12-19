namespace DataModel.TimeSeries
{
    public class TimeSeries
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Name { get; set; }
        public Unit Unit { get; set; }
    }
}