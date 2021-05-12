using System;

namespace DiffChecker.Model
{
    [Serializable]
    public class SetDataResponse
    {
        public string Id { get; set; }
        public string Data { get; set; }

        public override string ToString()
        {
            return $"Id = {Id}, Data = {Data}";
        }
    }
}