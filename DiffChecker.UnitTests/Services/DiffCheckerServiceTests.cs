using System.Collections.Generic;
using DiffChecker.Api.Model;
using DiffChecker.Api.Services;
using DiffChecker.Api.Services.Interfaces;
using DiffChecker.Domain.Error;
using DiffChecker.Domain.Model;
using DiffChecker.Domain.Services;
using Moq;
using NUnit.Framework;

namespace DiffChecker.UnitTests.Services
{
    public class DiffCheckerServiceTests
    {
        private Mock<IRepository> _repositoryMock;
        private Mock<IDecodeService> _decodeServiceMock;
        private DiffCheckerService _service;

        private readonly string TestId = "1";

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IRepository>();
            _decodeServiceMock = new Mock<IDecodeService>();

            _decodeServiceMock
                .Setup(ds => ds.DecodeString(It.IsAny<string>()))
                .Returns<string>(val => val);

            _service = InstantiateService();
        }

        [Test]
        public void SetLeftCallsRepository()
        {
            var testData = "123";
            _service.SetLeft(TestId, testData);

            _repositoryMock.Verify(
                r => r.SetLeft(
                    It.Is<string>(id => id.Equals(TestId)),
                    It.Is<string>(data => data.Equals(testData))
                ),
                Times.Once
            );
        }

        [Test]
        public void SetRightCallsRepository()
        {
            var testData = "123";
            _service.SetRight(TestId, testData);

            _repositoryMock.Verify(
                r => r.SetRight(
                    It.Is<string>(id => id.Equals(TestId)),
                    It.Is<string>(data => data.Equals(testData))
                ),
                Times.Once
            );
        }

        [Test]
        public void DiffForEqualData()
        {
            var response = SetupDiffTest("abc", "abc");
            Assert.IsTrue(response.Equal);
            AssertDecodeServiceWasCalled();
        }

        [Test]
        public void DiffForDifferentSizes()
        {
            var response = SetupDiffTest("abc", "ab");
            Assert.IsTrue(response.DifferentSize);
            AssertDecodeServiceWasCalled();
        }

        [Test]
        public void DiffForDifferentValues_LastChar()
        {
            var response = SetupDifferentDiffTest("abc", "abd");

            var expectedDiffPoints = new List<DiffPoint> { new DiffPoint { Offset = 2, Length = 1 } };

            AssertEqualDiffPoints(expectedDiffPoints, response.DiffPoints);
            AssertDecodeServiceWasCalled();
        }

        [Test]
        public void DiffForDifferentValues_FirstChar()
        {
            var response = SetupDifferentDiffTest("abc", "bbc");

            var expectedDiffPoints = new List<DiffPoint> { new DiffPoint { Offset = 0, Length = 1 } };

            AssertEqualDiffPoints(expectedDiffPoints, response.DiffPoints);
            AssertDecodeServiceWasCalled();
        }

        [Test]
        public void DiffForDifferentValues_MultipleSingleCharDiffs()
        {
            var response = SetupDifferentDiffTest("123456", "113355");

            var expectedDiffPoints = new List<DiffPoint> {
                new DiffPoint { Offset = 1, Length = 1 },
                new DiffPoint { Offset = 3, Length = 1 },
                new DiffPoint { Offset = 5, Length = 1 }
            };

            AssertEqualDiffPoints(expectedDiffPoints, response.DiffPoints);
            AssertDecodeServiceWasCalled();
        }

        [Test]
        public void DiffForDifferentValues_MultipleMultipleCharDiffs()
        {
            var response = SetupDifferentDiffTest("111222333444", "000222111444");

            var expectedDiffPoints = new List<DiffPoint> {
                new DiffPoint { Offset = 0, Length = 3 },
                new DiffPoint { Offset = 6, Length = 3 }
            };

            AssertEqualDiffPoints(expectedDiffPoints, response.DiffPoints);
            AssertDecodeServiceWasCalled();
        }

        [Test]
        public void DiffForDifferentValues_RandomDiffs()
        {
            var response = SetupDifferentDiffTest(
                "111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111",
                "000001110011111110001110111100000000001111111100011111111111111111011111000010000011110001");

            var expectedDiffPoints = new List<DiffPoint> {
                new DiffPoint { Offset = 0, Length = 5 },
                new DiffPoint { Offset = 8, Length = 2 },
                new DiffPoint { Offset = 17, Length = 3 },
                new DiffPoint { Offset = 23, Length = 1 },
                new DiffPoint { Offset = 28, Length = 10 },
                new DiffPoint { Offset = 46, Length = 3 },
                new DiffPoint { Offset = 66, Length = 1 },
                new DiffPoint { Offset = 72, Length = 4 },
                new DiffPoint { Offset = 77, Length = 5 },
                new DiffPoint { Offset = 86, Length = 3}
            };

            AssertEqualDiffPoints(expectedDiffPoints, response.DiffPoints);
            AssertDecodeServiceWasCalled();
        }

        [Test]
        public void DiffForDifferentValues_SingleMultipleCharDiff()
        {
            var response = SetupDifferentDiffTest("12345", "67890");

            var expectedDiffPoints = new List<DiffPoint> {
                new DiffPoint { Offset = 0, Length = 5 }
            };

            AssertEqualDiffPoints(expectedDiffPoints, response.DiffPoints);
            AssertDecodeServiceWasCalled();
        }

        [Test]
        public void NoDataSetThrowsError()
        {
            Assert.Throws<MissingInputException>(() => _service.FindDifference(TestId));
        }

        private ComparisonResponse SetupDiffTest(string plainLeftData, string plainRightData)
        {
            _repositoryMock
                .Setup(r => r.GetLeft(It.IsAny<string>()))
                .Returns(new DiffData { Data = plainLeftData });

            _repositoryMock
                .Setup(r => r.GetRight(It.IsAny<string>()))
                .Returns(new DiffData { Data = plainRightData });

            return _service.FindDifference(TestId);
        }

        private ComparisonResponse SetupDifferentDiffTest(string plainLeftData, string plainRightData)
        {
            var response = SetupDiffTest(plainLeftData, plainRightData);
            Assert.IsFalse(response.Equal);
            return response;
        }

        private void AssertEqualDiffPoints(IList<DiffPoint> expected, IList<DiffPoint> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count, $"Expected {expected.Count} DiffPoints, got {actual.Count}");

            for (var diffCounter = 0; diffCounter < expected.Count; diffCounter++)
            {
                var expectedDiff = expected[diffCounter];
                var actualDiff = actual[diffCounter];

                Assert.AreEqual(expectedDiff.Length, actualDiff.Length, $"Expected Length = {expectedDiff.Length}, got {actualDiff.Length} for DiffPoint {diffCounter}");
                Assert.AreEqual(expectedDiff.Offset, actualDiff.Offset, $"Expected Offset = {expectedDiff.Offset}, got {actualDiff.Offset} for DiffPoint {diffCounter}");
            }
        }

        private void AssertDecodeServiceWasCalled()
        {
            _decodeServiceMock.Verify(
                ds => ds.DecodeString(It.IsAny<string>()),
                Times.Exactly(2));
        }

        private DiffCheckerService InstantiateService()
        {
            return new DiffCheckerService(
                _repositoryMock.Object,
                _decodeServiceMock.Object);
        }
    }
}