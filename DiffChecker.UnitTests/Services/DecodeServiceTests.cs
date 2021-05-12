using DiffChecker.Api.Services;
using DiffChecker.Domain.Error;
using NUnit.Framework;

namespace DiffChecker.UnitTests.Services
{
    public class DecodeServiceTests
    {
        private DecodeService _service;

        [SetUp]
        public void SetUp()
        {
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
            return new DecodeService();
        }
    }
}