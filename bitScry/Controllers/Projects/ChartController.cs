using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace bitScry.Controllers.Projects
{
    [Route("Projects/[controller]")]
    public class ChartController : Controller
    {
        private readonly IConfiguration _config;

        public ChartController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}