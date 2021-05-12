using System.Net.Http;
using System.Text;
using DiffChecker.Model;
using Newtonsoft.Json;

namespace DiffChecker.IntegrationTests.Util
{
    public static class IntegrationTestsHelper
    {
        public static string GetSetLeftUrl(string id)
        {
            return $"v1/diff/{id}/left";
        }

        public static string GetSetRightUrl(string id)
        {
            return $"v1/diff/{id}/right";
        }

        public static string GetDiffUrl(string id)
        {
            return $"v1/diff/{id}";
        }

        public static HttpContent BuildSetDataRequest(string data)
        {
            return new StringContent(
                JsonConvert.SerializeObject(new SetDataRequest { Data = data }),
                Encoding.UTF8,
                "application/json");
        }
    }
}