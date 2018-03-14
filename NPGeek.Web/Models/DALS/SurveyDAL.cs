using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NPGeek.Web.Models.DALS
{
    public class SurveyDAL : ISurveyDAL
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public List<Park> GetFavoriteParks()
        {
            List<Park> parks = new List<Park>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"SELECT park.*, COUNT(survey_result.parkCode) AS parkCount
                                                    FROM park
                                                    JOIN survey_result ON park.parkCode = survey_result.parkCode
                                                    GROUP BY park.parkCode, park.parkName, park.state, park.acreage,
                                                    park.elevationInFeet, park.milesOfTrail, park.numberOfCampsites,
                                                    park.climate, park.yearFounded, park.annualVisitorCount, park.inspirationalQuote,
                                                    park.inspirationalQuoteSource, park.parkDescription, park.entryFee, park.numberOfAnimalSpecies
                                                    ORDER BY COUNT(survey_result.parkCode) DESC", conn);

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
                        p.NumberOfFavorites = Convert.ToInt32(reader["parkCount"]);

                        parks.Add(p);
                    }
                }
            }
            catch (SqlException)
            {
            }

            return parks;
        }

        public void SaveSurvey(SurveyResult model)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"INSERT INTO survey_result (parkCode, emailAddress, state, activityLevel)
                                                    VALUES (@parkCode, @emailAddress, @state, @activityLevel)", conn);

                    cmd.Parameters.AddWithValue("@parkCode", model.FavoriteParkCode);
                    cmd.Parameters.AddWithValue("@emailAddress", model.Email);
                    cmd.Parameters.AddWithValue("@state", model.State);
                    cmd.Parameters.AddWithValue("@activityLevel", model.ActivityLevel);

                    cmd.ExecuteNonQuery();
                }
            }
            catch(SqlException)
            {

            }
        }
    }
}