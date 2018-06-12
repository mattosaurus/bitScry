using bitScry.Models.Projects.Weather;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bitScry.AppCode.Projects
{
    public static class Weather
    {
        public static string GetRequestIP(IHttpContextAccessor httpContextAccessor, bool tryUseXForwardHeader = true)
        {
            string ip = null;

            if (tryUseXForwardHeader)
                ip = GetHeaderValueAs<string>(httpContextAccessor, "X-Forwarded-For").SplitCsv().FirstOrDefault();

            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (ip.IsNullOrWhitespace() && httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress != null)
                ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (ip.IsNullOrWhitespace())
                ip = GetHeaderValueAs<string>(httpContextAccessor, "REMOTE_ADDR");

            // _httpContextAccessor.HttpContext?.Request?.Host this is the local host.

            if (ip.IsNullOrWhitespace())
                throw new Exception("Unable to determine caller's IP.");

            // Remove port if on IP address
            ip = ip.Substring(0, ip.IndexOf(":"));

            return ip;
        }

        public static T GetHeaderValueAs<T>(IHttpContextAccessor httpContextAccessor, string headerName)
        {
            StringValues values;

            if (httpContextAccessor.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!rawValues.IsNullOrWhitespace())
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }

        public static List<string> SplitCsv(this string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable<string>()
                .Select(s => s.Trim())
                .ToList();
        }

        public static bool IsNullOrWhitespace(this string s)
        {
            return String.IsNullOrWhiteSpace(s);
        }

        public static WeatherIcon GetWeatherIcon(int weatherTypeId)
        {
            WeatherIcon icon = new WeatherIcon();

            if (weatherTypeId == 0)
            {
                icon.MetId = 0;
                icon.Description = "Clear night";
                icon.Icon = "wi-night-clear";
            }
            else if (weatherTypeId == 1)
            {
                icon.MetId = 1;
                icon.Description = "Sunny day";
                icon.Icon = "wi-day-sunny";
            }
            else if (weatherTypeId == 2)
            {
                icon.MetId = 2;
                icon.Description = "Partly cloudy (night)";
                icon.Icon = "wi-night-cloudy";
            }
            else if (weatherTypeId == 3)
            {
                icon.MetId = 3;
                icon.Description = "Partly cloudy (day)";
                icon.Icon = "wi-day-cloudy";
            }
            else if (weatherTypeId == 4)
            {
                icon.MetId = 4;
                icon.Description = "Not used";
                icon.Icon = "wi-na";
            }
            else if (weatherTypeId == 5)
            {
                icon.MetId = 5;
                icon.Description = "Mist";
                icon.Icon = "wi-fog";
            }
            else if (weatherTypeId == 6)
            {
                icon.MetId = 6;
                icon.Description = "Fog";
                icon.Icon = "wi-fog";
            }
            else if (weatherTypeId == 7)
            {
                icon.MetId = 7;
                icon.Description = "Cloudy";
                icon.Icon = "wi-cloud";
            }
            else if (weatherTypeId == 8)
            {
                icon.MetId = 8;
                icon.Description = "Overcast";
                icon.Icon = "wi-cloudy";

            }
            else if (weatherTypeId == 9)
            {
                icon.MetId = 9;
                icon.Description = "Light rain shower (night)";
                icon.Icon = "wi-night-showers";
            }
            else if (weatherTypeId == 10)
            {
                icon.MetId = 10;
                icon.Description = "Light rain shower (day)";
                icon.Icon = "wi-day-showers";
            }
            else if (weatherTypeId == 11)
            {
                icon.MetId = 11;
                icon.Description = "Drizzle";
                icon.Icon = "wi-sprinkle";
            }
            else if (weatherTypeId == 12)
            {
                icon.MetId = 12;
                icon.Description = "Light rain";
                icon.Icon = "wi-rain-mix";
            }
            else if (weatherTypeId == 13)
            {
                icon.MetId = 13;
                icon.Description = "Heavy rain shower (night)";
                icon.Icon = "wi-night-rain";
            }
            else if (weatherTypeId == 14)
            {
                icon.MetId = 14;
                icon.Description = "Heavy rain shower (day)";
                icon.Icon = "wi-day-rain";
            }
            else if (weatherTypeId == 15)
            {
                icon.MetId = 15;
                icon.Description = "Heavy rain";
                icon.Icon = "wi-rain";
            }
            else if (weatherTypeId == 16)
            {
                icon.MetId = 16;
                icon.Description = "Sleet shower (night)";
                icon.Icon = "wi-night-sleet";
            }
            else if (weatherTypeId == 17)
            {
                icon.MetId = 17;
                icon.Description = "Sleet shower (day)";
                icon.Icon = "wi-day-sleet";
            }
            else if (weatherTypeId == 18)
            {
                icon.MetId = 18;
                icon.Description = "Sleet";
                icon.Icon = "wi-sleet";
            }
            else if (weatherTypeId == 19)
            {
                icon.MetId = 19;
                icon.Description = "Hail shower (night)";
                icon.Icon = "wi-night-hail";
            }
            else if (weatherTypeId == 20)
            {
                icon.MetId = 20;
                icon.Description = "Hail shower (day)";
                icon.Icon = "wi-day-hail";
            }
            else if (weatherTypeId == 21)
            {
                icon.MetId = 21;
                icon.Description = "Hail";
                icon.Icon = "wi-hail";
            }
            else if (weatherTypeId == 22)
            {
                icon.MetId = 22;
                icon.Description = "Light snow shower (night)";
                icon.Icon = "wi-night-snow";
            }
            else if (weatherTypeId == 23)
            {
                icon.MetId = 23;
                icon.Description = "Light snow shower (day)";
                icon.Icon = "wi-day-snow";
            }
            else if (weatherTypeId == 24)
            {
                icon.MetId = 24;
                icon.Description = "Light snow";
                icon.Icon = "wi-snow";
            }
            else if (weatherTypeId == 25)
            {
                icon.MetId = 25;
                icon.Description = "Heavy snow shower (night)";
                icon.Icon = "wi-night-snow";
            }
            else if (weatherTypeId == 26)
            {
                icon.MetId = 26;
                icon.Description = "Heavy snow shower (day)";
                icon.Icon = "wi-day-snow";
            }
            else if (weatherTypeId == 27)
            {
                icon.MetId = 27;
                icon.Description = "Heavy snow";
                icon.Icon = "wi-snow";
            }
            else if (weatherTypeId == 28)
            {
                icon.MetId = 28;
                icon.Description = "Thunder shower (night)";
                icon.Icon = "wi-night-storm-showers";
            }
            else if (weatherTypeId == 29)
            {
                icon.MetId = 29;
                icon.Description = "Thunder shower (day)";
                icon.Icon = "wi-day-storm-showers";
            }
            else if (weatherTypeId == 30)
            {
                icon.MetId = 30;
                icon.Description = "Thunder";
                icon.Icon = "wi-storm-showers";
            }

            return icon;
        }
    }
}
