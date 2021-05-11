using DiffChecker.Services;
using DiffChecker.Services.Interfaces;

namespace DiffCheckerTests.Services
{
    public class TemporaryRepositoryTests : RepositoryTests
    {
        protected override IRepository InstantiateRepository()
        {
            return new TemporaryRepository();
        }
    }
}