using DiffChecker.Domain.Model;
using DiffChecker.Model;

namespace DiffChecker.Services.Interfaces
{
    public interface IDiffCheckerService
    {
        ComparisonResponse FindDifference(string id);

        DiffData SetLeft(string id, string data);

        DiffData SetRight(string id, string data);
    }
}