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
    public static class PasswordGeneratorWord
    {
        [FunctionName("PasswordGeneratorWord")]
        public static List<string> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "PasswordGenerator/Word")]WordConfig config, HttpRequest req, TraceWriter log, ExecutionContext executionContext)
        {
            log.Info("PasswordGeneratorWord function processed a request.");

            List<string> passwords = new List<string>();

            List<string> words = File.ReadAllLines(executionContext.FunctionAppDirectory + "/Data/PasswordGenerator/google-10000-english-no-swears.txt").ToList();

            for (int i = 0; i < config.NumberOfPasswords; i++)
            {
                passwords.Add(Common.GenerateWordPassword(config, words));
            }

            return passwords;
        }
    }
}