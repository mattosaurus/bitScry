using bitScry.Models.Projects.EventHubSAS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bitScry.Controllers.Projects
{
    [Route("Projects/[controller]")]
    public class EventHubSASController : Controller
    {
        private readonly IConfiguration _config;

        public EventHubSASController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        [ActionName("Index")]
        public IActionResult IndexGet()
        {
            return View("~/Views/Projects/EventHubSAS/Index.cshtml");
        }

        [HttpPost]
        [ActionName("Index")]
        public IActionResult IndexPost(EventHubSASDetails eventHubSASDetails)
        {
            eventHubSASDetails.SecurityToken = EventHubSASToken.CreateSecurityToken(
                eventHubSASDetails.SenderKeyName,
                eventHubSASDetails.SenderKey,
                eventHubSASDetails.ServiceNamespace,
                eventHubSASDetails.HubName,
                eventHubSASDetails.PublisherName,
                eventHubSASDetails.TokenTimeToLive
                );

            return View("~/Views/Projects/EventHubSAS/Index.cshtml", eventHubSASDetails);
        }
    }
}
