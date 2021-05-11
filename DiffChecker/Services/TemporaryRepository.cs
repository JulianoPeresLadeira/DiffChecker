using System.Collections.Generic;
using DiffChecker.Model;
using DiffChecker.Services.Interfaces;

namespace DiffChecker.Services
{
    public class TemporaryRepository : IRepository
    {
        private readonly Dictionary<string, string> LeftMap = new Dictionary<string, string>();
        private readonly Dictionary<string, string> RightMap = new Dictionary<string, string>();

        public DiffData GetLeft(string id)
        {
            string data;
            LeftMap.TryGetValue(id, out data);
            return new DiffData
            {
                Id = id,
                Data = data
            };
        }

        public DiffData GetRight(string id)
        {
            string data;
            RightMap.TryGetValue(id, out data);
            return new DiffData
            {
                Id = id,
                Data = data
            };
        }

        public DiffData SetLeft(string id, string data)
        {
            LeftMap[id] = data;
            return new DiffData
            {
                Id = id,
                Data = LeftMap[id]
            };
        }

        public DiffData SetRight(string id, string data)
        {
            RightMap[id] = data;
            return new DiffData
            {
                Id = id,
                Data = RightMap[id]
            };
        }
    }
}