using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace bitScry.Controllers.Projects
{
    [Route("Projects/[controller]")]
    public class WeatherController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}