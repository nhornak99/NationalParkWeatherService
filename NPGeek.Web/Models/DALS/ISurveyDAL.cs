using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPGeek.Web.Models.DALS
{
    public interface ISurveyDAL
    {
        void SaveSurvey(SurveyResult model);

        List<Park> GetFavoriteParks();
     }
}
