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

namespace bitScry.Function
{
    public static class ImageProxy
    {
        [FunctionName("ImageProxy")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("ImageProxy function processed a request.");

            string url = req.Query["url"];

            if (!string.IsNullOrEmpty(url))
            {
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(url);

                return response;
            }
            else
            {
                log.Info("No url parameter provided.");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
    }
}
