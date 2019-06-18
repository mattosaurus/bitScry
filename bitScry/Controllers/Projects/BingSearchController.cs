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
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace bitScry.Controllers.Projects
{
    [Route("Projects/[controller]/[action]")]
    public class BingSearchController : Controller
    {
        // https://github.com/Azure-Samples/cognitive-services-dotnet-sdk-samples/tree/master/BingSearchv7/BingImageSearch
        // https://westus.dev.cognitive.microsoft.com/docs/services/56f91f2d778daf23d8ec6739/operations/56f91f2e778daf14a499e1fa
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
                ImageSearchClient client = new ImageSearchClient(new Microsoft.Azure.CognitiveServices.Search.ImageSearch.ApiKeyServiceClientCredentials(_config["Keys:BingSearch"]));
                Images imageResults = client.Images.SearchAsync(query: imageSearchParameters.Query, offset: imageSearchParameters.Offset, count: imageSearchParameters.Count).Result;

                TempData.Put("Images", imageResults);
            }

            return View("~/Views/Projects/BingSearch/Index.cshtml", imageSearchParameters);
        }

        [HttpGet]
        [ActionName("Detail")]
        public async Task<IActionResult> DetailGet(string imageUrl, string imageSource)
        {
            ComputerVisionClient computerVision = new ComputerVisionClient(new Microsoft.Azure.CognitiveServices.Vision.ComputerVision.ApiKeyServiceClientCredentials(_config["Keys:ComputerVision"]));
            List<VisualFeatureTypes> visualFeatures = new List<VisualFeatureTypes>()
            {
                VisualFeatureTypes.Adult,
                VisualFeatureTypes.Categories,
                VisualFeatureTypes.Color,
                VisualFeatureTypes.Description,
                VisualFeatureTypes.Faces,
                VisualFeatureTypes.ImageType,
                VisualFeatureTypes.Tags
            };
            computerVision.Endpoint = "https://northeurope.api.cognitive.microsoft.com";

            ImageAnalysis imageAnalysis = await computerVision.AnalyzeImageAsync(imageUrl, visualFeatures);

            TempData["ImageUrl"] = imageUrl;
            TempData["ImageSource"] = imageSource;

            return View("~/Views/Projects/BingSearch/Detail.cshtml", imageAnalysis);
        }
    }
}