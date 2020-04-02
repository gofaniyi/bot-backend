using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloEpidBot.Model.Domain
{
    public class SelfAssesment 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TravelHistory { get; set; }
        public string PublicPlace { get; set; }
        public string PublicPlaces { get; set; }
        public string TravelPlaces { get; set; }
        public string CloseContact { get; set; }
        public string Ocupation { get; set; }
        public string Location { get; set; }
        public string HouseAddress { get; set; }
        public string Symptoms { get; set; }
        public string SymptomsStart { get; set; }

    }
}
