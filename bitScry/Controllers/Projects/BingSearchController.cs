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
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;

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
                ImageSearchAPI client = new ImageSearchAPI(new ApiKeyServiceClientCredentials(_config["Keys:BingSearch"]));
                Images imageResults = client.Images.SearchAsync(query: imageSearchParameters.Query, offset: imageSearchParameters.Offset, count: imageSearchParameters.Count).Result;

                TempData.Put("Images", imageResults);
            }

            return View("~/Views/Projects/BingSearch/Index.cshtml", imageSearchParameters);
        }

        [HttpGet]
        [ActionName("Detail")]
        public IActionResult DetailGet(string imageUrl, string imageSource)
        {
            Images images = TempData.Get<Images>("Images");

            VisionServiceClient visionServiceClient = new VisionServiceClient(_config["Keys:ComputerVision"], "https://northeurope.api.cognitive.microsoft.com/vision/v1.0");
            VisualFeature[] visualFeatures = new VisualFeature[] { VisualFeature.Adult, VisualFeature.Categories, VisualFeature.Color, VisualFeature.Description, VisualFeature.Faces, VisualFeature.ImageType, VisualFeature.Tags };

            AnalysisResult analysisResult = visionServiceClient.AnalyzeImageAsync(imageUrl, visualFeatures).Result;

            TempData["ImageUrl"] = imageUrl;
            TempData["ImageSource"] = imageSource;

            return View("~/Views/Projects/BingSearch/Detail.cshtml", analysisResult);
        }
    }
}