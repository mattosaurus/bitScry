using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bitScry.ViewComponents
{
    public class BingSearchImageGrid : ViewComponent
    {
        public IViewComponentResult Invoke(List<ImageObject> images)
        {
            return View(images);
        }
    }
}