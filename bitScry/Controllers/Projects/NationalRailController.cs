using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using bitScry.Extensions;
using bitScry.Models.Projects.NationalRail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NationalRail;
using NationalRail.Models.HistoricalServicePerformance;

namespace bitScry.Controllers.Projects
{
    [Route("Projects/[controller]")]
    public class NationalRailController : Controller
    {
        private readonly IConfiguration _config;
        private readonly List<Station> _stations;
        private readonly List<Delay> _delayReasons;

        public NationalRailController(IConfiguration config)
        {
            _config = config;
            _stations = AppCode.Projects.NationalRail.GetStations(_config.GetConnectionString("NationalRailConnection"));
            _delayReasons = AppCode.Projects.NationalRail.GetDelayReasons(_config.GetConnectionString("NationalRailConnection"));
        }

        [HttpGet]
        [ActionName("Index")]
        public IActionResult IndexGet()
        {
            TempData.Put("Stations", _stations);

            return View("~/Views/Projects/NationalRail/Index.cshtml");
        }

        [HttpPost]
        [ActionName("Index")]
        public IActionResult IndexPost(HistoricalQuery query)
        {
            // Historical Data
            HistoricalServicePerformanceClient historicalClient = new HistoricalServicePerformanceClient(_config["Keys:NationalRail:Email"], _config["Keys:NationalRail:Password"]);

            List<ServiceDetailsResponse> details = new List<ServiceDetailsResponse>();
            List<ServiceMetricResponse> historicalResponses = new List<ServiceMetricResponse>();
            List<Service> services = new List<Service>();

            DateTime date;

            DateTime.TryParseExact(query.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

            DateTime endDate = date.AddDays(1);

            while (date < endDate && date < DateTime.Now)
            {
                string fromTime = date.TimeOfDay.ToString("hhmm");
                string toTime = date.AddHours(1).TimeOfDay.ToString("hhmm");

                if (toTime == "0000")
                {
                    toTime = "2359";
                }

                ServiceMetric metric = new ServiceMetric()
                {
                    FromLocation = query.FromCrs,
                    ToLocation = query.ToCrs,
                    FromTime = fromTime,
                    ToTime = toTime,
                    FromDate = date.ToString("yyyy-MM-dd"),
                    ToDate = date.ToString("yyyy-MM-dd")
                };

                if (date.DayOfWeek == DayOfWeek.Saturday)
                {
                    metric.Days = "SATURDAY";
                }
                else if (date.DayOfWeek == DayOfWeek.Sunday)
                {
                    metric.Days = "SUNDAY";
                }
                else
                {
                    metric.Days = "WEEKDAY";
                }

                ServiceMetricResponse historicalResponse = historicalClient.GetServiceMetrics(metric).Result;
                historicalResponses.Add(historicalResponse);

                foreach (Service service in historicalResponse.Services)
                {
                    services.Add(service);

                    foreach (string rid in service.ServiceAttributesMetrics.RIDs)
                    {
                        ServiceDetailsResponse detail = historicalClient.GetServiceDetails(new ServiceDetailsRID(rid)).Result;
                        details.Add(detail);
                    }
                }

                date = date.AddHours(1);
            }

            TempData.Put("Stations", _stations);
            TempData.Put("Services", services);
            TempData.Put("Details", details);
            TempData.Put("DelayReasons", _delayReasons);

            return View("~/Views/Projects/NationalRail/Index.cshtml", query);
        }
    }
}