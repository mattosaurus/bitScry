using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using bitScry.Function.Models.PasswordGenerator;
using System.Collections.Generic;
using System.Linq;
using bitScry.Function.AppCode;
using Microsoft.Extensions.Logging;

namespace bitScry.Function
{
    public static class PasswordGeneratorString
    {
        [FunctionName("PasswordGeneratorString")]
        public static List<string> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "PasswordGenerator/String")]HttpRequest req, ILogger log, ExecutionContext executionContext)
        {
            log.LogInformation("PasswordGeneratorString function processed a request.");

            StringConfig config = new StringConfig();

            string body = req.ReadAsStringAsync().Result;

            if (!string.IsNullOrEmpty(body))
            {
                // Deserialize config object from body
                config = JsonConvert.DeserializeObject<StringConfig>(body);
            }
            else
            {
                // Deserialize config object from parameters
                config = JsonConvert.DeserializeObject<StringConfig>(JsonConvert.SerializeObject(req.GetQueryParameterDictionary()));
            }

            List<string> passwords = new List<string>();

            for (int i = 0; i < config.NumberOfPasswords; i++)
            {
                passwords.Add(Common.GenerateStringPassword(config));
            }

            return passwords;
        }
    }
}