using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace bitScry.Models.Home
{
    public class ReCaptcha
    {
        public static async Task<bool> ValidateAsync(string response, string secretKey)
        {
            bool valid = false;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage result = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify?secret=" + secretKey + "&response=" + response, null);
                ReCaptchaResponse reCaptchaResponse = JsonConvert.DeserializeObject<ReCaptchaResponse>(await result.Content.ReadAsStringAsync());
                valid = reCaptchaResponse.Success;
            }

            return valid;
        }
    }

    public class ReCaptchaRequest
    {
        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("response ")]
        public string Response { get; set; }

        [JsonProperty("remoteip")]
        public string RemoteIp { get; set; }
    }

    public class ReCaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public DateTime ChallengeTimeStamp { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}
