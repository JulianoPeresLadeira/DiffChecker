using DiffChecker.Domain.Model;

namespace DiffChecker.Domain.Services
{
    public interface IRepository
    {
        DiffData SetRight(string id, string data);

        DiffData GetRight(string id);

        DiffData SetLeft(string id, string data);

        DiffData GetLeft(string id);
    }
}