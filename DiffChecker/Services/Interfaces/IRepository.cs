namespace DiffChecker.Services.Interfaces
{
    public interface IRepository
    {
        void SetRight(string id, string data);

        string GetRight(string id);

        void SetLeft(string id, string data);

        string GetLeft(string id);
    }
}