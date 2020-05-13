using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Jeopardy.Utilities
{
    public static class Convert 
    {
        public static string Base64Encode(this string plainText) {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string base64EncodedData) {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string ToJsonBase64String(this object obj)
        {
            return JsonConvert
                .SerializeObject(obj, 
                    new JsonSerializerSettings{
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }
                )
                .Base64Encode();
        }

        public static T FromBase64JsonString<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str.Base64Decode());
        }
    }
}