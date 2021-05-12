using System;
using DiffChecker.DataAccess.Error;
using DiffChecker.DataAccess.Model;
using DiffChecker.Domain.Model;
using DiffChecker.Domain.Services;
using MongoDB.Driver;

namespace DiffChecker.DataAccess.Service
{
    public class MongoRepository : IRepository
    {
        private readonly IMongoCollection<MongoDiffData> _leftData;
        private readonly IMongoCollection<MongoDiffData> _rightData;

        public MongoRepository(IMongoDbConfiguration settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _leftData = database.GetCollection<MongoDiffData>(settings.LeftDataCollection);
            _rightData = database.GetCollection<MongoDiffData>(settings.RightDataCollection);
        }

        public DiffData GetLeft(string id)
        {
            return FindData(_leftData, id);
        }

        public DiffData GetRight(string id)
        {
            return FindData(_rightData, id);
        }

        public DiffData SetLeft(string id, string data)
        {
            return UpsertData(_leftData, id, data);
        }

        public DiffData SetRight(string id, string data)
        {
            return UpsertData(_rightData, id, data);
        }

        private DiffData FindData(IMongoCollection<MongoDiffData> collection, string id)
        {
            var filter = Builders<MongoDiffData>.Filter.Eq("RequestId", id);
            var result = collection.Find(filter);
            return result.FirstOrDefault();
        }

        private DiffData UpsertData(IMongoCollection<MongoDiffData> collection, string id, string data)
        {
            try
            {
                var filter = Builders<MongoDiffData>.Filter.Eq("RequestId", id);
                var update = Builders<MongoDiffData>.Update.Set("Data", data);
                collection.UpdateOne(filter, update, new UpdateOptions { IsUpsert = true });
                return new DiffData { RequestId = id, Data = data };
            }
            catch (Exception)
            {
                throw new PersistenceException();
            }
        }
    }
}