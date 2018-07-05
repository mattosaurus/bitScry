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

namespace bitScry.Function
{
    public static class PasswordGeneratorString
    {
        [FunctionName("PasswordGeneratorString")]
        public static List<string> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "PasswordGenerator/String")]StringConfig config, HttpRequest req, TraceWriter log, ExecutionContext executionContext)
        {
            log.Info("PasswordGeneratorString function processed a request.");

            List<string> passwords = new List<string>();

            for (int i = 0; i < config.NumberOfPasswords; i++)
            {
                passwords.Add(Common.GenerateStringPassword(config));
            }

            return passwords;
        }
    }
}