using bitScry.Models.Projects.Weather;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bitScry.ViewComponents
{
    public class DayForecast : ViewComponent
    {
        public IViewComponentResult Invoke(WeatherSummary weatherSummary)
        {
            return View(weatherSummary);
        }
    }
}
