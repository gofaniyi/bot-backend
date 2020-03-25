using GloEpidBot.Model.Domain;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GloEpidBot.Utilities
{
    public static class ExternalService
    {
        public async static Task<HttpResponseMessage> MakeCallPost(HttpContent content, IOptions<LuisConfig> options)
        {
            HttpClient client = new HttpClient();
            //var SessionId = await GetSessionId(config);

            
            client.BaseAddress = new Uri($"https://westus.api.cognitive.microsoft.com");
            client.DefaultRequestHeaders.Accept.Clear();
         //   client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "2f119a4df88249fe9968478e442f0c5c");
            

            var response = await client.PostAsync("/luis/v2.0/apps/9d3e37ca-a828-4f8a-bac2-0dd87a93193d", content);
             
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
