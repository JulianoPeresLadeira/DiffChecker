using System.Collections.Generic;
using DiffChecker.Services.Interfaces;

namespace DiffChecker.Services
{
    public class TemporaryRepository : IRepository
    {
        private readonly Dictionary<string, string> LeftMap = new Dictionary<string, string>();
        private readonly Dictionary<string, string> RightMap = new Dictionary<string, string>();

        public string GetLeft(string id)
        {
            string data;
            LeftMap.TryGetValue(id, out data);
            return data;
        }

        public string GetRight(string id)
        {
            string data;
            RightMap.TryGetValue(id, out data);
            return data;
        }

        public void SetLeft(string id, string data)
        {
            LeftMap[id] = data;
        }

        public void SetRight(string id, string data)
        {
            RightMap[id] = data;
        }
    }
}