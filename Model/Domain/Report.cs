using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GloEpidBot.Model.Domain
{
    public class Report
    {
        public int Id { get; set; }

        [Required]
        public string Symptoms { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string RiskStatus{ get; set; }

        public string ReporterName{ get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateReported { get; set; }
    }
}
