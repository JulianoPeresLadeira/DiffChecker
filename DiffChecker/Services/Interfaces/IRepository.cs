using DiffChecker.Model;

namespace DiffChecker.Services.Interfaces
{
    public interface IRepository
    {
        SetDataResponse SetRight(string id, string data);

        SetDataResponse GetRight(string id);

        SetDataResponse SetLeft(string id, string data);

        SetDataResponse GetLeft(string id);
    }
}