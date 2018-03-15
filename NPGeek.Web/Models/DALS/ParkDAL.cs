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

        public string GetWeatherApiDataFromParkCode(string parkCode)
        {

            JavaScriptSerializer jss = new JavaScriptSerializer();

            Dictionary<string, double[]> location = new Dictionary<string, double[]>()
            {
                {"GTNP" ,new double[]{43.7904, -110.6818 } },
                {"MRNP" , new double[]{46.8800, -121.7269 }},
                {"RMNP" ,new double[]{ 40.3428, -105.6836 } },
                {"CVNP" ,new double[]{41.2808, -81.5678 } },
                {"ENP" ,new double[]{ 25.2866, -80.8987} },
                { "GCNP",new double[]{ 36.1070, -112.1130 } },
                {"GNP" ,new double[]{ 48.7596, -113.7870 } },
                {"GSMNP" ,new double[]{35.6118, -83.4895 } },
                {"YNP2" ,new double[]{37.8651 , -119.5383 } },
                {"YNP" ,new double[]{44.4280 , -110.5885 } }
            };

            var weatherData = new List<object>();
            dynamic json = null;
            using (WebClient wc = new WebClient())
            {
                json = Newtonsoft.Json.JsonConvert.DeserializeObject(wc.DownloadString("https://api.darksky.net/forecast/81d9e91d0dd5e5e49eb5d6d739e0d9c3/" + location[parkCode][0] + ", " + location[parkCode][1]));
            }

            for (int i = 0; i < 5; i++)
            {
                weatherData.Add(new
                {
                    Day = UnixTimeStampToDateTime(Convert.ToDouble(json.daily.data[i].time)),
                    Low = Convert.ToInt32(json.daily.data[i].temperatureLow),
                    High = Convert.ToInt32(json.daily.data[i].temperatureHigh),
                    Forecast = Convert.ToString(json.daily.data[i].icon)
                });
            }
            

            return jss.Serialize(weatherData);
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