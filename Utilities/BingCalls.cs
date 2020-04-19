using BingMapsRESTToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GloEpidBot.Utilities
{
    public static  class BingCalls
    {
        private static string APIKEY = Environment.GetEnvironmentVariable("BING_API_KEY");

        public static async Task<string> GetLocation(double longitude, double latitude)
        {
           


            var req = new ReverseGeocodeRequest()
            {
                Point = new Coordinate
                {
                    Latitude = latitude,
                    Longitude = longitude
                },
                BingMapsKey = APIKEY,
                IncludeNeighborhood = true,
                IncludeIso2 = true


            };

            var response = await req.Execute();

            if (response != null &&
      response.ResourceSets != null &&
      response.ResourceSets.Length > 0 &&
      response.ResourceSets[0].Resources != null &&
      response.ResourceSets[0].Resources.Length > 0)
            {
                var result = response.ResourceSets[0].Resources[0] as BingMapsRESTToolkit.Location;

                return result.Name;
            }
            else
            {
                return "Location Not found";
            }
        }

        public static string ValidateLocation(string location)
        {

            
            string dstate = string.Empty;
            if (!location.Contains(','))
                return "false";
            var local = location.Split(',');

            
            
            if(local.Length != 2)
            {
                return "false";
            }
            else
            {
                var States = "FCT,Abia,Adamawa,Akwa Ibom,Anambra,Bauchi,Bayelsa,Benue,Borno,Cross River,Delta,Ebonyi,Edo,Ekiti,Enugu,Abuja,Gombe,Imo,Jigawa,Kaduna,Kano,Katsina,Kebbi,Kogi,Kwara,Lagos,Nasarawa,Niger,Ogun,Ondo,Osun,Oyo,Plateau,Rivers,Sokoto,Taraba,Yobe,Zamfara".Split(',');

                dstate = local[1];
                dstate = dstate.Trim();
                var localwithstate = dstate.Split()[0];         
               // dstate =  Regex.Replace(dstate, @"\s+", "");
                foreach ( var states in States)
                {
                    if(localwithstate.ToString().ToLower() == states.ToLower())
                    {
                        return localwithstate;
                    }
                    
                }

                return "false";
            }
        }

        public static string ValidateDate(string date)
        {
            DateTime datey;
            var r = DateTime.TryParse(date, out datey);
            if (!r)
                return "Date format not recognized, Try again";


            if (datey.Date > DateTime.Today.Date)
                return "Symptoms start date can not be in the future";

            return datey.ToString();
                     
        }
    }
}
