﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bitScry.ViewComponents
{
    public class BingSearchImageRow : ViewComponent
    {
        public IViewComponentResult Invoke(Images images)
        {
            return View(images);
        }
    }
}
