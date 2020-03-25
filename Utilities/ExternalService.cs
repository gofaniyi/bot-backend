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
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", options.Value.Key);
            

            var response = await client.PostAsync($"/luis/v2.0/apps/{options.Value.AppId}", content);
             
            return response; 

        }
    }
}
