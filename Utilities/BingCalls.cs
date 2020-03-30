using BingMapsRESTToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
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

                return result.Address + " " + result.Name;
            }
            else
            {
                return "Location Not found";
            }
        }
    }
}
