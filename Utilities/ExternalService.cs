using GloEpidBot.Model.Domain;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GloEpidBot.Utilities
{
    public static class ExternalService
    {
        private static string ACCESS_TOKEN = Environment.GetEnvironmentVariable("ACCESS_TOKEN");
        private static string DATABASE_API_URL = Environment.GetEnvironmentVariable("DATABASE_API_URL");
     

        public static async Task<HttpResponseMessage> MakeCallNCDCAsync(HttpContent content)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(DATABASE_API_URL);
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            try
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                content.Headers.Add("token", ACCESS_TOKEN);
                
            }
            catch (Exception ex)
            {

                throw;
            }
          
            var response = client.PostAsync("api/v1/Assessment/AddAssessment", content).Result;
            var responseAsString  = await response.Content.ReadAsStringAsync();
            return response;
        }


        public async static Task<HttpResponseMessage> MakeCallGet(string message)
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://gloepid.cognitiveservices.azure.com/luis/prediction/v3.0/apps/dae3036e-5f57-40fb-9cbf-aec04fb45ee7/slots/production/predict?subscription-key=a12c0ccff307458abb1200c0e37e6b20&verbose=true&show-all-intents=true&log=true");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "a12c0ccff307458abb1200c0e37e6b20");
            HttpResponseMessage response = await client.GetAsync($"?query={message}");
            return response;
        }
    }
}
