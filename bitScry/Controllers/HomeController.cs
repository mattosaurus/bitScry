using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bitScry.Models;
using bitScry.Models.Home;
using Microsoft.Extensions.Configuration;
using bitScry.AppCode;

namespace bitScry.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Index()
        {
            List<Blog> blogs = Home.GetBlogSummaries(_config.GetConnectionString("BlogConnection"));
            ViewData["Blogs"] = blogs;
            return View();
        }

        [HttpGet]
        public IActionResult About()
        {
            ViewData["Message"] = "A bit about bitScry.";

            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactForm contact)
        {
            string contactMessage;

            try
            {
                Home.SendEmail(contact.FromEmail, _config["SendGrid:To"], contact.Message, "bitScry Conatct Form - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), _config["SendGrid:Templates:Contact"], _config["SendGrid:APIKey"]).Wait();
                contactMessage = "Message sent :)";
            }
            catch (Exception ex)
            {
                contactMessage = ex.Message;
            }

            ViewData["ContactMessage"] = contactMessage;
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
