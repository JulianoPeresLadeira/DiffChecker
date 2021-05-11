using DiffChecker.Model;

namespace DiffChecker.Services.Interfaces
{
    public interface IDiffCheckerService
    {
        DiffResponse FindDifference(string id);

        DiffData SetLeft(string id, string data);

        DiffData SetRight(string id, string data);
    }
}