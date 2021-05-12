namespace DiffChecker.Domain.Model
{
    public class MongoDbConfiguration : IMongoDbConfiguration
    {
        public string LeftDataCollection { get; set; }
        public string RightDataCollection { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IMongoDbConfiguration
    {
        public string LeftDataCollection { get; set; }
        public string RightDataCollection { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}