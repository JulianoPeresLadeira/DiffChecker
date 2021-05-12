using System;
using System.Collections.Generic;

namespace DiffChecker.Model
{
    [Serializable]
    public class ComparisonResponse
    {
        public bool? Equal { get; set; }
        public bool? DifferentSize { get; set; }
        public IList<DiffPoint> DiffPoints { get; set; }
    }
}