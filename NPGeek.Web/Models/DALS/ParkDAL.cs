using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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


    }
}