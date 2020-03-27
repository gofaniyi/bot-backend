using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;

namespace GloEpidBot.Utilities
{
    public static class Luiscalls
    {

        private static string predictionKey = Environment.GetEnvironmentVariable("LUIS_PREDICTION_KEY");

        
        private static string predictionEndpoint = Environment.GetEnvironmentVariable("LUIS_ENDPOINT_NAME");

     
        private static string appId = Environment.GetEnvironmentVariable("LUIS_APP_ID");

        static LUISRuntimeClient CreateClient()
        {
            var credentials = new ApiKeyServiceClientCredentials(predictionKey);
            var luisClient = new LUISRuntimeClient(credentials, new System.Net.Http.DelegatingHandler[] { })
            {
                Endpoint = predictionEndpoint
            };

            return luisClient;

        }




        public static async Task<PredictionResponse> GetPredictionAsync(string query)
        {

            // Get client 
            using (var luisClient = CreateClient())
            {

                var requestOptions = new PredictionRequestOptions
                {
                    DatetimeReference = DateTime.Parse("2019-01-01"),
                    PreferExternalEntities = true
                };

                var predictionRequest = new PredictionRequest
                {
                    Query =query,
                    Options = requestOptions
                };

                // get prediction
                return await luisClient.Prediction.GetSlotPredictionAsync(
                    Guid.Parse(appId),
                    slotName: "production",
                    predictionRequest,
                    verbose: true,
                    showAllIntents: true,
                    log: true);
            }
        }
    }
}
