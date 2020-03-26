using System;
using System.ComponentModel.DataAnnotations;

namespace GloEpidBot.Model.Parameters
{
    public class ReportParameters : QueryStringParameters
    {
        public string RiskStatus { get; set; }
        
        public int MinAge { get; set; }

        public int MaxAge { get; set; }
 
	    public bool ValidAgeRange => MaxAge > MinAge; 

        public string SearchBy { get; set; } = "";  

    }
}
