using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPGeek.Web.Models.DALS
{
    public interface IParkDAL
    {
        List<Park> GetAllParks();
        string GetWeatherData(string parkCode);
        string GetWeatherApiDataFromParkCode(string parkCode);
    }
}
