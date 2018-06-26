using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace bitScry.Controllers.Projects
{
    [Route("Projects/[controller]")]
    public class BingSearchController : Controller
    {
        private readonly IConfiguration _config;

        public BingSearchController(IConfiguration config)
        {
            _config = config;
        }
    }
}