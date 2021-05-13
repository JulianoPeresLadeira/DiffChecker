using DiffChecker.Api.Controllers.v1;
using DiffChecker.Api.Model;
using DiffChecker.Api.Services.Interfaces;
using DiffChecker.Domain.Error;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace DiffChecker.UnitTests.Controllers.v1
{
    public class DiffControllerTests
    {
        private Mock<IDiffCheckerService> _diffCheckerServiceMock;
        private Mock<ILogger<DiffController>> _loggerMock;
        private DiffController _controller;

        private readonly string TestId = "1";
        private readonly string TestData = "Test";

        [SetUp]
        public void SetUp()
        {
            _diffCheckerServiceMock = new Mock<IDiffCheckerService>();
            _loggerMock = new Mock<ILogger<DiffController>>();
            _controller = InstantiateController();
        }

        [Test]
        public void CallsServiceWhenSetLeftIsCalledCorrectly()
        {
            var request = new SetDataRequest
            {
                Data = TestData
            };

            _controller.SetLeft(TestId, request);

            _diffCheckerServiceMock
                .Verify(
                    dcs => dcs.SetLeft(
                        It.Is<string>(id => id.Equals(TestId)),
                        It.Is<string>(dataRequest => dataRequest.Equals(TestData)))
                    );
        }

        [Test]
        public void CallsServiceWhenSetRightIsCalledCorrectly()
        {
            var request = new SetDataRequest
            {
                Data = TestData
            };

            _controller.SetRight(TestId, request);

            _diffCheckerServiceMock
                .Verify(
                    dcs => dcs.SetRight(
                        It.Is<string>(id => id.Equals(TestId)),
                        It.Is<string>(dataRequest => dataRequest.Equals(TestData)))
                    );
        }

        [Test]
        public void ThrowsExpectedExceptionWhenSetLeftIsCalledIncorrectly()
        {
            Assert.Throws<InvalidInputException>(() => _controller.SetLeft(TestId, null));
        }

        [Test]
        public void ThrowsExpectedExceptionWhenSetRightIsCalledIncorrectly()
        {
            Assert.Throws<InvalidInputException>(() => _controller.SetRight(TestId, null));
        }

        [Test]
        public void CallsServiceWhenFindDiffIsCalled()
        {
            _controller.FindDiff(TestId);

            _diffCheckerServiceMock.Verify(dcs => dcs.FindDifference(It.Is<string>(id => id.Equals(TestId))));
        }

        private DiffController InstantiateController()
        {
            return new DiffController(_diffCheckerServiceMock.Object, _loggerMock.Object);
        }
    }
}