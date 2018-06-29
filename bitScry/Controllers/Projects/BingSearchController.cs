using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bitScry.Models.Projects.BingSearch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch.Models;
using Microsoft.Extensions.Configuration;
using bitScry.Extensions;

namespace bitScry.Controllers.Projects
{
    [Route("Projects/[controller]/[action]")]
    public class BingSearchController : Controller
    {
        // https://github.com/Azure-Samples/cognitive-services-dotnet-sdk-samples/tree/master/BingSearchv7/BingImageSearch
        private readonly IConfiguration _config;

        public BingSearchController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("/Projects/[controller]")]
        [ActionName("Index")]
        public IActionResult IndexGet(ImageSearchParameters imageSearchParameters)
        {
            if (!string.IsNullOrEmpty(imageSearchParameters.Query))
            {
                var client = new ImageSearchAPI(new ApiKeyServiceClientCredentials(_config["Keys:BingSearch"]));
                var imageResults = client.Images.SearchAsync(query: imageSearchParameters.Query, offset: imageSearchParameters.Offset, count: imageSearchParameters.Count).Result;

                TempData.Put("Images", imageResults);
            }

            return View("~/Views/Projects/BingSearch/Index.cshtml", imageSearchParameters);
        }

        [HttpGet]
        [ActionName("Detail")]
        public IActionResult DetailGet(string query, string insightsToken)
        {
            var client = new ImageSearchAPI(new ApiKeyServiceClientCredentials(_config["Keys:BingSearch"]));
            var insights = client.Images.DetailsAsync(query, insightsToken: insightsToken, modules: new List<string>() { "all" }).Result;

            TempData.Put("Insights", insights);

            return View("~/Views/Projects/BingSearch/Details.cshtml");
        }
    }
}