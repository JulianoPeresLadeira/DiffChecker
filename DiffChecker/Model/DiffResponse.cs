using System;
using System.Collections.Generic;

namespace DiffChecker.Model
{
    [Serializable]
    public class DiffResponse
    {
        public Nullable<bool> Equal { get; set; }
        public Nullable<bool> DifferentSize { get; set; }
        public IList<DiffPoint> DiffPoints { get; set; }
    }
}