using DiffChecker.Model;

namespace DiffChecker.Services.Interfaces
{
    public interface IRepository
    {
        DiffData SetRight(string id, string data);

        DiffData GetRight(string id);

        DiffData SetLeft(string id, string data);

        DiffData GetLeft(string id);
    }
}