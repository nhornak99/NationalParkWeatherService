using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

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
                        p.InspirationalQuote = Convert.ToString(reader["inpirationQuote"]);
                        p.InspirationalQuoteSource = Convert.ToString(reader["inpirationQuoteSource"]);
                        p.Description = Convert.ToString(reader["parkDescription"]);
                        p.EntryFee = Convert.ToDecimal(reader["entreeFee"]);
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
    }
}