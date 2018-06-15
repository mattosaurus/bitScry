using bitScry.Models.Projects.NationalRail;
using NationalRail.Models.HistoricalServicePerformance;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace bitScry.AppCode.Projects
{
    public static class NationalRail
    {
        public static List<Station> GetStations(string connectionString)
        {
            List<Station> stations = new List<Station>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("national_rail.sp__get_stations"))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Connection = connection;

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            stations.Add(new Station() { CrsCode = reader[0].ToString(), StationName = reader[1].ToString() });
                        }
                    }

                    reader.Close();
                    connection.Close();
                }
            }

            return stations;
        }

        public static List<Delay> GetDelayReasons(string connectionString)
        {
            List<Delay> delays = new List<Delay>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("national_rail.sp__get_delay_reasons"))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Connection = connection;

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            delays.Add(new Delay() { DelayCode = reader[0].ToString(), DelayReason = reader[1].ToString() });
                        }
                    }

                    reader.Close();
                    connection.Close();
                }
            }

            return delays;
        }

        public static bool? IsDelayed(ServiceDetailsLocation location, string dateOfService, int delayMinutes = 15)
        {
            bool? delayed = null;

            DateTime predictedDeparture;
            DateTime actualDeparture;
            DateTime predictedArrival;
            DateTime actualArrival;

            DateTime.TryParseExact(dateOfService + " " + location.PredictedTimeOfDeparture, "yyyy-MM-dd HHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out predictedDeparture);
            DateTime.TryParseExact(dateOfService + " " + location.ActualTimeOfDeparture, "yyyy-MM-dd HHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out actualDeparture);
            DateTime.TryParseExact(dateOfService + " " + location.PredictedTimeOfArrival, "yyyy-MM-dd HHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out predictedArrival);
            DateTime.TryParseExact(dateOfService + " " + location.ActualTimeOfArrival, "yyyy-MM-dd HHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out actualArrival);

            if (location.PredictedTimeOfDeparture != "" && location.ActualTimeOfDeparture != "")
            {
                if (actualDeparture >= predictedDeparture.AddMinutes(delayMinutes))
                {
                    delayed = true;
                }
                else
                {
                    delayed = false;
                }
            }

            if (delayed == null || delayed == false)
            {
                if (location.PredictedTimeOfArrival != "" && location.ActualTimeOfArrival != "")
                {
                    if (actualArrival >= predictedArrival.AddMinutes(delayMinutes))
                    {
                        delayed = true;
                    }
                    else
                    {
                        delayed = false;
                    }
                }
            }

            if (location.LateCancelationReason != "")
            {
                delayed = true;
            }

            return delayed;
        }
    }
}
