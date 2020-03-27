using System;
using System.ComponentModel.DataAnnotations;

namespace GloEpidBot.Model.Parameters
{
    public class ReportParameters : QueryStringParameters
    {
        public string SearchString { get; set; }
       // public string Location { get; set; }
/*         public int MinAge { get; set; }

        public int MaxAge { get; set; }
 
	    public bool ValidAgeRange => MaxAge > MinAge;  */

       // public bool HaveFilter => !string.IsNullOrEmpty(RiskStatus) || !string.IsNullOrEmpty(Location);

        

    }
}
