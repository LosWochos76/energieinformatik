namespace DataModel.Messages
{
    public class SuccessMessage
    {
        public bool Success { get; set; }

        public SuccessMessage(bool success)
        {
            this.Success = success;
        }
    }
}
