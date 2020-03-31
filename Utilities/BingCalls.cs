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

        public static bool ValidateLocation(string location)
        {
          
            if (!location.Contains(','))
                return false;
            var local = location.Split(',');
            if(local.Length != 2)
            {
                return false;
            }
            else
            {
                var States = "Abia,Adamawa,Akwa Ibom,Anambra,Bauchi,Bayelsa,Benue,Borno,Cross River,Delta,Ebonyi,Edo,Ekiti,Enugu,Abuja,Gombe,Imo,Jigawa,Kaduna,Kano,Katsina,Kebbi,Kogi,Kwara,Lagos,Nasarawa,Niger,Ogun,Ondo,Osun,Oyo,Plateau,Rivers,Sokoto,Taraba,Yobe,Zamfara".Split(',');

                string dstate = string.Empty;
            dstate =     Regex.Replace(dstate, @"\s+", "");
              foreach ( var states in States)
                {
                    if(dstate.ToString().ToLower() == states.ToLower())
                    {
                        return true;
                    }
                    
                }

                return false;
            }
        }

        public static bool ValidateDate(string date)
        {
            var days = date.Split('/');
            if (days.Length != 2)
                return false;
            int day = 0;
            int month = 0;
            int.TryParse(days[0], out day);
            int.TryParse(days[1], out month);
            if (day > 0 && day < 31)
            {
                if(month > 0 && month < 12)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;

            }
        }
    }
}
