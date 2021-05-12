namespace DiffChecker.Domain.Model
{
    public class DiffData
    {
        public string RequestId { get; set; }
        public string Data { get; set; }

        public override string ToString()
        {
            return $"RequestId = {RequestId}, Data = {Data}";
        }
    }
}