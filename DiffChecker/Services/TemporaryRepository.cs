using System.Collections.Generic;
using DiffChecker.Model;
using DiffChecker.Services.Interfaces;

namespace DiffChecker.Services
{
    public class TemporaryRepository : IRepository
    {
        private readonly Dictionary<string, string> _leftMap = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _rightMap = new Dictionary<string, string>();

        public DiffData GetLeft(string id)
        {
            string data;
            _leftMap.TryGetValue(id, out data);
            return new DiffData
            {
                Id = id,
                Data = data
            };
        }

        public DiffData GetRight(string id)
        {
            string data;
            _rightMap.TryGetValue(id, out data);
            return new DiffData
            {
                Id = id,
                Data = data
            };
        }

        public DiffData SetLeft(string id, string data)
        {
            _leftMap[id] = data;
            return new DiffData
            {
                Id = id,
                Data = _leftMap[id]
            };
        }

        public DiffData SetRight(string id, string data)
        {
            _rightMap[id] = data;
            return new DiffData
            {
                Id = id,
                Data = _rightMap[id]
            };
        }
    }
}