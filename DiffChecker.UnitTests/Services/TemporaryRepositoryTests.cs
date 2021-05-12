using DiffChecker.Domain.Services;
using DiffChecker.Services;

namespace DiffChecker.UnitTests.Services
{
    public class TemporaryRepositoryTests : RepositoryTests
    {
        protected override IRepository InstantiateRepository()
        {
            return new TemporaryRepository();
        }
    }
}