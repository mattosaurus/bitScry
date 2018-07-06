using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace bitScry.Function
{
    public static class LetsEncrypt
    {
        [FunctionName("LetsEncrypt")]
        public static async Task<HttpResponseMessage> Run(HttpRequest req, string code, TraceWriter log)
        {
            log.Info($"C# HTTP trigger function processed a request. {code}");

            var content = File.ReadAllText(@"D:\home\site\wwwroot\.well-known\acme-challenge\" + code);
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(content, Encoding.UTF8, "text/plain");
            return resp;
        }
    }
}
