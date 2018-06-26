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
    [Route("Projects/[controller]")]
    public class BingSearchController : Controller
    {
        // https://github.com/Azure-Samples/cognitive-services-dotnet-sdk-samples/tree/master/BingSearchv7/BingImageSearch
        private readonly IConfiguration _config;

        public BingSearchController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        [ActionName("Index")]
        public IActionResult IndexGet()
        {
            return View("~/Views/Projects/BingSearch/Index.cshtml");
        }

        [HttpPost]
        [ActionName("Index")]
        public IActionResult IndexPost(ImageSearchParameters imageSearchParameters)
        {
            var client = new ImageSearchAPI(new ApiKeyServiceClientCredentials(_config["Keys:BingSearch"]));
            var imageResults = client.Images.SearchAsync(query: imageSearchParameters.Query).Result;

            TempData.Put("Images", imageResults);

            return View("~/Views/Projects/BingSearch/Index.cshtml");
        }
    }
}