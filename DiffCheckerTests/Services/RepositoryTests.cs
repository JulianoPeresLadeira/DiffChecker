using DiffChecker.Services.Interfaces;
using NUnit.Framework;

namespace DiffCheckerTests.Services
{
    public abstract class RepositoryTests
    {
        protected readonly string testId = "1";
        protected readonly string altTestId = "2";
        protected readonly string testData = "eyJhIjogMSwgImIiOiAyfQ==";

        protected IRepository repository;

        protected abstract IRepository InstantiateRepository();

        [SetUp]
        public void SetUp()
        {
            repository = InstantiateRepository();
        }

        [Test]
        public void CorrectlySetsAndFetchesLeftData()
        {
            repository.SetLeft(testId, testData);

            var fetchedData = repository.GetLeft(testId);
            Assert.AreEqual(testData, fetchedData);
        }

        [Test]
        public void CorrectlySetsAndFetchesRightData()
        {
            repository.SetRight(testId, testData);

            var fetchedData = repository.GetRight(testId);
            Assert.AreEqual(testData, fetchedData);
        }

        [Test]
        public void DoesNotFetchInexistentLeftData()
        {
            repository.SetLeft(testId, testData);

            var fetchedData = repository.GetLeft(altTestId);
            Assert.IsNull(fetchedData);
        }

        [Test]
        public void DoesNotFetchInexistentRightData()
        {
            repository.SetRight(testId, testData);

            var fetchedData = repository.GetRight(altTestId);
            Assert.IsNull(fetchedData);
        }

        [Test]
        public void NoIdSetLeft()
        {
            var data = repository.GetLeft(testId);
            Assert.IsNull(data);
        }

        [Test]
        public void NoIdSetRight()
        {
            var data = repository.GetRight(testId);
            Assert.IsNull(data);
        }
    }
}