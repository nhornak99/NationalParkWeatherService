using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace NPGeek.Web.Models.DALS
{
    public class ParkDAL : IParkDAL
    {

        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public List<Park> GetAllParks()
        {
            List<Park> parks = new List<Park>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM park", conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Park p = new Park();

                        p.ParkCode = Convert.ToString(reader["parkCode"]);
                        p.Name = Convert.ToString(reader["parkName"]);
                        p.State = Convert.ToString(reader["state"]);
                        p.Acreage = Convert.ToInt32(reader["acreage"]);
                        p.ElevationInFeet = Convert.ToInt32(reader["elevationInFeet"]);
                        p.MilesOfTrail = Convert.ToDouble(reader["milesOfTrail"]);
                        p.NumberOfCampsites = Convert.ToInt32(reader["numberOfCampsites"]);
                        p.Climate = Convert.ToString(reader["climate"]);
                        p.YearFounded = Convert.ToInt32(reader["yearFounded"]);
                        p.AnnualVisitorCount = Convert.ToInt32(reader["annualVisitorCount"]);
                        p.InspirationalQuote = Convert.ToString(reader["inspirationalQuote"]);
                        p.InspirationalQuoteSource = Convert.ToString(reader["inspirationalQuoteSource"]);
                        p.Description = Convert.ToString(reader["parkDescription"]);
                        p.EntryFee = Convert.ToDecimal(reader["entryFee"]);
                        p.NumberOfCampsites = Convert.ToInt32(reader["numberOfAnimalSpecies"]);

                        parks.Add(p);
                    }
                }
            }
            catch (SqlException)
            {
            }

            return parks;
        }

        public string GetWeatherData(string parkCode)
        {
            var weatherData = new List<object>();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT fiveDayForecastValue, low, high, forecast FROM weather WHERE parkCode = @parkCode;", conn);
                    cmd.Parameters.AddWithValue("@parkCode", parkCode);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        weatherData.Add(new
                        {
                            Day = Convert.ToInt32(reader["fiveDayForecastValue"]),
                            Low = Convert.ToInt32(reader["low"]),
                            High = Convert.ToInt32(reader["high"]),
                            Forecast = Convert.ToString(reader["forecast"])
                        });
                    }
                }
            }
            catch (SqlException ex)
            {

            }
            return jss.Serialize(weatherData);
        }

        public int[] GetLatitudeAndLongitude(string parkCode)
        {
            Dictionary<string, int[]> location = new Dictionary<string, int[]>()
            {
                {"GTNP" ,new int[]{44, 111 } },
                {"MRNP" , new int[]{47, 122 }},
                {"RMNP" ,new int[]{ 40, 106} },
                {"CVNP" ,new int[]{41, 82 } },
                {"ENP" ,new int[]{ 26, 81} },
                { "GCNP",new int[]{ 36, 112} },
                {"GNP" ,new int[]{49, 114 } },
                {"GSMNP" ,new int[]{36, 84 } },
                {"YNP2" ,new int[]{38 , 120 } },
                {"YNP" ,new int[]{44 , 111 } }
            };

            var weatherData = new List<object>();
            dynamic json = null;
            using (WebClient wc = new WebClient())
            {
                json = Newtonsoft.Json.JsonConvert.DeserializeObject(wc.DownloadString("https://api.darksky.net/forecast/81d9e91d0dd5e5e49eb5d6d739e0d9c3/" + location[parkCode][0] + ", " + location[parkCode][1]));
            }
            var sfsd = json.daily.data[0].time;
            weatherData.Add(new
            {
                Day = UnixTimeStampToDateTime(Convert.ToDouble(json.daily.data[0].time))

            });


            return location[parkCode];
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

    }
}