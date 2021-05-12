using DiffChecker.Api.Services;
using DiffChecker.Domain.Services;

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