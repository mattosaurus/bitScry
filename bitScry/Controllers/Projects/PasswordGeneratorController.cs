using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bitScry.Models.Projects.PasswordGenerator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace bitScry.Controllers.Projects
{
    [Route("Projects/[controller]")]
    public class PasswordGeneratorController : Controller
    {
        private readonly IConfiguration _config;

        public PasswordGeneratorController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/Projects/PasswordGenerator/Index.cshtml");
        }
    }
}