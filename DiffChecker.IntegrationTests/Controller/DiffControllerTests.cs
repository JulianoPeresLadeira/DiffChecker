using System.Net.Http;
using System.Threading.Tasks;
using DiffChecker.IntegrationTests.Util;
using DiffChecker.Model;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace DiffChecker.IntegrationTests.Controller
{
    public class DiffControllerTests : IClassFixture<WebApplicationFactory<DiffChecker.Startup>>
    {
        private readonly HttpClient _client;

        private readonly string TestId = "10000";

        private readonly string TestString1_10Chars = "MDEyMzQ1Njc4OQ==";
        private readonly string TestString2_10Chars = "MDEyMzQ1NjQ1Ng==";
        private readonly string TestString_9Chars = "MDEyMzQ1Njc4";

        public DiffControllerTests(WebApplicationFactory<DiffChecker.Startup> fixture)
        {
            _client = fixture.CreateClient();
        }

        [Fact]
        public async Task ShouldReturnEqualStrings()
        {
            var diffResponse = await EndToEndTest(TestId, TestString1_10Chars, TestString1_10Chars);
            Assert.True(diffResponse.Equal);
        }

        [Fact]
        public async Task ShouldReturnDifferentSizes()
        {
            var diffResponse = await EndToEndTest(TestId, TestString1_10Chars, TestString_9Chars);
            Assert.True(diffResponse.DifferentSize);
        }

        [Fact]
        public async Task ShouldReturnDiffPoints()
        {
            var diffResponse = await EndToEndTest(TestId, TestString1_10Chars, TestString2_10Chars);
            Assert.False(diffResponse.Equal);
            Assert.Equal(1, diffResponse.DiffPoints.Count);
            Assert.Equal(7, diffResponse.DiffPoints[0].Offset);
            Assert.Equal(3, diffResponse.DiffPoints[0].Length);
        }

        [Fact]
        public async Task ShouldReturnErrorWhenUsingInvalidString()
        {
            var errorResponse = await EndToEndTestWithError(TestId, "123", "123");
            Assert.Equal(500, errorResponse.StatusCode);
        }

        private async Task<ComparisonResponse> EndToEndTest(string id, string leftData, string rightData)
        {
            await SetupData(id, leftData, rightData);
            return await CallDiff(id);
        }

        private async Task<ErrorResponse> EndToEndTestWithError(string id, string leftData, string rightData)
        {
            await SetupData(id, leftData, rightData);
            var diffResponse = await _client.GetAsync(IntegrationTestsHelper.GetDiffUrl(id));
            return JsonConvert.DeserializeObject<ErrorResponse>(await diffResponse.Content.ReadAsStringAsync());
        }

        private async Task SetupData(string id, string leftData, string rightData)
        {
            var leftResponse = await CallSetData(IntegrationTestsHelper.GetSetLeftUrl(id), leftData);
            var rightResponse = await CallSetData(IntegrationTestsHelper.GetSetRightUrl(id), rightData);

            Assert.Equal(leftData, leftResponse.Data);
            Assert.Equal(id, leftResponse.Id);

            Assert.Equal(rightData, rightResponse.Data);
            Assert.Equal(id, rightResponse.Id);
        }

        private async Task<SetDataResponse> CallSetData(string url, string data)
        {
            var requestBody = IntegrationTestsHelper.BuildSetDataRequest(data);

            var response = await _client.PostAsync(url, requestBody);
            return JsonConvert.DeserializeObject<SetDataResponse>(await response.Content.ReadAsStringAsync());
        }

        private async Task<ComparisonResponse> CallDiff(string id)
        {
            var diffResponse = await _client.GetAsync(IntegrationTestsHelper.GetDiffUrl(id));
            return JsonConvert.DeserializeObject<ComparisonResponse>(await diffResponse.Content.ReadAsStringAsync());
        }
    }
}