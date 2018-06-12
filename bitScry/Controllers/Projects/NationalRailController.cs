using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace bitScry.Controllers.Projects
{
    public class NationalRailController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Projects/NationalRail/Index.cshtml");
        }
    }
}