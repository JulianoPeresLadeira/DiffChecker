using System;
using System.Collections.Generic;
using System.Text;

namespace DiffChecker.Api.Model
{
    [Serializable]
    public class ComparisonResponse
    {
        public bool? Equal { get; set; }
        public bool? DifferentSize { get; set; }
        public IList<DiffPoint> DiffPoints { get; set; }

        public override string ToString()
        {
            var values = new StringBuilder();

            if (Equal.HasValue)
            {
                values.Append($"Equal = {Equal}, ");
            }

            if (DifferentSize.HasValue)
            {
                values.Append($"DifferentSize = {DifferentSize}, ");
            }

            values.Append($"DiffPoints = {DiffPoints}");
            return values.ToString();
        }
    }
}