using DiffChecker.Api.Services;
using DiffChecker.Domain.Error;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace DiffChecker.UnitTests.Services
{
    public class DecodeServiceTests
    {
        private Mock<ILogger<DecodeService>> _loggerMock;
        private DecodeService _service;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<DecodeService>>();
            _service = InstantiateService();
        }

        [Test]
        public void CorrectlyDecodesBase64String()
        {
            var encodedString = "VGhpcyBpcyBhIHRlc3Q=";
            var decodedString = "This is a test";

            var serviceDecodedString = _service.DecodeString(encodedString);

            Assert.AreEqual(serviceDecodedString, decodedString);
        }

        [Test]
        public void ThrowsExpectedExceptionOnDecodingError()
        {
            Assert.Throws<DataDecodingException>(() => _service.DecodeString("Hello"));
        }

        private DecodeService InstantiateService()
        {
            return new DecodeService(_loggerMock.Object);
        }
    }
}