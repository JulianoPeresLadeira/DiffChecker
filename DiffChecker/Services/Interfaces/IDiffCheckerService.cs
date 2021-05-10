using DiffChecker.Model;

namespace DiffChecker.Services.Interfaces
{
    public interface IDiffCheckerService
    {
        ServiceResponse FindDifference(string id);

        void SetLeft(string id, string data);

        void SetRight(string id, string data);
    }
}