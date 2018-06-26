using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bitScry.ViewComponents
{
    public class BingSearchImage : ViewComponent
    {
        public IViewComponentResult Invoke(ImageObject image)
        {
            return View(image);
        }
    }
}
