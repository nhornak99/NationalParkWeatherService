using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace NPGeek.Web.Models
{
    public class Park
    {
        public string ParkCode { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public int Acreage { get; set; }
        public int ElevationInFeet { get; set; }
        public double MilesOfTrail { get; set; }
        public int NumberOfCampsites { get; set; }
        public string Climate { get; set; }
        public int YearFounded { get; set; }
        public int AnnualVisitorCount { get; set; }
        public string InspirationalQuote { get; set; }
        public string InspirationalQuoteSource { get; set; }
        public string Description { get; set; }
        public decimal EntryFee { get; set; }
        public int NumberOfAnimalSpecies { get; set; }

        public string Weather { get; set; }
        public int NumberOfFavorites { get; set; }

        public void ConvertTemperature()
        {
            var weatherJson = Json.Decode(Weather);
            foreach(var day in weatherJson)
            {
                day.Low = (((double)day.Low - 32) / 1.8).ToString();
                day.High = (((double)day.High - 32) / 1.8).ToString();
            }
            Weather = weatherJson;
        }
    }
}