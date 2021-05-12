using DiffChecker.Api.Model;
using DiffChecker.Domain.Model;

namespace DiffChecker.Api.Services.Interfaces
{
    public interface IDiffCheckerService
    {
        ComparisonResponse FindDifference(string id);

        DiffData SetLeft(string id, string data);

        DiffData SetRight(string id, string data);
    }
}