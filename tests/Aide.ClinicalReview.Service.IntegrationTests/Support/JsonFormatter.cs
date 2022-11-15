using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aide.ClinicalReview.Service.IntegrationTests.Support
{
    static class JsonFormatter
    {
        public static string FormatJson(string json)
        {
            return JToken.Parse(json).ToString(Formatting.Indented);
        }
    }
}
