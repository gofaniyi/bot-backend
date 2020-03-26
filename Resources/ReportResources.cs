using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GloEpidBot.Resources
{
    public class ReportResource
    {
        public int Id { get; set; }

        [Required]
        public string Symptoms { get; set; }

        [Required]
        public string RiskStatus{ get; set; }

        [Required]
        public string Location { get; set; }

        public string ReporterName{ get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateReported { get; set; }
    }

    public class ReportOutput1
    {  
      public int Count { get; set; }  
      public List<ReportResource> Items { get; set; }  
    }
}