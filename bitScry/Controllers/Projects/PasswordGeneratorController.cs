using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bitScry.Extensions;
using bitScry.Models.Projects.PasswordGenerator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace bitScry.Controllers.Projects
{
    [Route("Projects/[controller]")]
    public class PasswordGeneratorController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PasswordGeneratorController(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Try and get cookies
            string wordConfigCookie = _httpContextAccessor.HttpContext.Request.Cookies["wordConfig"];
            string stringConfigCookie = _httpContextAccessor.HttpContext.Request.Cookies["stringConfig"];

            WordConfig wordConfig = new WordConfig();

            if (!string.IsNullOrEmpty(wordConfigCookie))
            {
                wordConfig = JsonConvert.DeserializeObject<WordConfig>(wordConfigCookie);
            }

            StringConfig stringConfig = new StringConfig();

            if (!string.IsNullOrEmpty(stringConfigCookie))
            {
                stringConfig = JsonConvert.DeserializeObject<StringConfig>(stringConfigCookie);
            }

            TempData.Put("WordConfig", wordConfig);
            TempData.Put("StringConfig", stringConfig);

            return View("~/Views/Projects/PasswordGenerator/Index.cshtml");
        }
    }
}