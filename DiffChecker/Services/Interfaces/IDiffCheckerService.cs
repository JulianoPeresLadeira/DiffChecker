using DiffChecker.Model;

namespace DiffChecker.Services.Interfaces
{
    public interface IDiffCheckerService
    {
        ComparisonResponse FindDifference(string id);

        SetDataResponse SetLeft(string id, string data);

        SetDataResponse SetRight(string id, string data);
    }
}