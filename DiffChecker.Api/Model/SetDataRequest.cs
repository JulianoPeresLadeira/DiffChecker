using System;

namespace DiffChecker.Api.Model
{
    [Serializable]
    public class SetDataRequest
    {
        public string Data { get; set; }

        public override string ToString()
        {
            return $"Data = {Data}";
        }
    }
}