using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using FreeGeoIPCore;
using MetOfficeDataPoint;
using MetOfficeDataPoint.Models;
using MetOfficeDataPoint.Models.GeoCoordinate;
using bitScry.Models.Projects.Weather;

namespace bitScry.Controllers.Projects
{
    [Route("Projects/[controller]")]
    public class WeatherController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;

        public WeatherController(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index(double longitude, double latitude)
        {
            FreeGeoIPClient ipClient = new FreeGeoIPClient();

            string ipAddress = AppCode.Projects.Weather.GetRequestIP(_httpContextAccessor);

            FreeGeoIPCore.Models.Location location = ipClient.GetLocation(ipAddress).Result;

            GeoCoordinate coordinate = new GeoCoordinate();

            // If location is provided then use over IP address
            if (longitude == 0 && latitude == 0)
            {
                coordinate.Longitude = location.Longitude;
                coordinate.Latitude = location.Latitude;
            }
            else
            {
                coordinate.Longitude = longitude;
                coordinate.Latitude = latitude;
            }

            MetOfficeDataPointClient metClient = new MetOfficeDataPointClient(_config["Keys:MetOfficeDataPoint"]);
            MetOfficeDataPoint.Models.Location site = metClient.GetClosestSite(coordinate).Result;
            ForecastResponse3Hourly forecastResponse = metClient.GetForecasts3Hourly(null, site.ID).Result;

            // Create list containing 5 days of forecasts for midday
            List<WeatherSummary> weatherSummaries = new List<WeatherSummary>();

            // Get current minutes after midnight
            int minutes = (int)(DateTime.Now.TimeOfDay.TotalSeconds / 60);

            foreach (ForecastLocation3Hourly forecastLocation in forecastResponse.SiteRep.DV.Location)
            {
                foreach (Period3Hourly period in forecastLocation.Period)
                {
                    if (DateTime.Parse(period.Value) == DateTime.Today)
                    {
                        WeatherSummary weatherSummary = new WeatherSummary();
                        weatherSummary.ForecastDay = DateTime.Parse(period.Value);

                        // Find closest forecast to now
                        Rep3Hourly forecast = period.Rep.Aggregate((x, y) => Math.Abs(x.MinutesAfterMidnight - minutes) < Math.Abs(y.MinutesAfterMidnight - minutes) ? x : y);
                        weatherSummary.Icon = AppCode.Projects.Weather.GetWeatherIcon(forecast.WeatherType);
                        // Find min temperature
                        weatherSummary.MinTemperature = (int)period.Rep.Where(x => x.MinutesAfterMidnight >= forecast.MinutesAfterMidnight).Min(x => x.Temperature);
                        // Find current temperature
                        weatherSummary.MaxTemperature = (int)period.Rep.Where(x => x.MinutesAfterMidnight >= forecast.MinutesAfterMidnight).Max(x => x.Temperature);
                        // Get remaing forecasts
                        weatherSummary.Forecasts = period.Rep.Where(x => x.MinutesAfterMidnight >= forecast.MinutesAfterMidnight).ToList();

                        weatherSummaries.Add(weatherSummary);
                    }
                    else
                    {
                        WeatherSummary weatherSummary = new WeatherSummary();
                        weatherSummary.ForecastDay = DateTime.Parse(period.Value);

                        // Get icon for midday
                        weatherSummary.Icon = AppCode.Projects.Weather.GetWeatherIcon(period.Rep.First(x => x.MinutesAfterMidnight == 720).WeatherType);
                        // Find min temperature
                        weatherSummary.MinTemperature = (int)period.Rep.Min(x => x.Temperature);
                        // Find max temperature
                        weatherSummary.MaxTemperature = (int)period.Rep.Max(x => x.Temperature);
                        // Get forecasts
                        weatherSummary.Forecasts = period.Rep;

                        weatherSummaries.Add(weatherSummary);
                    }
                }
            }

            ViewData["WeatherSummaries"] = weatherSummaries;
            ViewData["Location"] = location;
            ViewData["Site"] = site;

            return View("~/Views/Projects/Weather/Index.cshtml");
        }
    }
}