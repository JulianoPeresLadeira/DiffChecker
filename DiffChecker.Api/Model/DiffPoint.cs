using System;

namespace DiffChecker.Api.Model
{
    [Serializable]
    public class DiffPoint
    {
        public int Offset { get; set; }
        public int Length { get; set; }

        public override string ToString()
        {
            return $"Offset = {Offset}, Length = {Length}";
        }
    }
}