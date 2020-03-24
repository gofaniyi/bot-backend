using Google.Cloud.Dialogflow.V2;
namespace GloEpidBot
{
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {

        string key = string.Empty;

        override
      public System.Threading.Tasks.Task OnConnectedAsync()
        {
             key = Context.ConnectionId;
          

            return System.Threading.Tasks.Task.CompletedTask;
        }


        public System.Threading.Tasks.Task SendResponse(string message)
        {
            try
            {
                var result = DetectIntents.DetectIntentFromTexts("covidboi-puwabf", key, message, "en-US");

                Clients.Client(key).SendCoreAsync("ReceiveResponse", new object[] { result.FulfillmentMessages, result.FulfillmentText, result.Intent.DisplayName, result.Parameters });
            }
            catch (System.Exception ex)
            {

              Clients.All.SendCoreAsync("ReceiveResponse", new object[] { ex.Message });
            }
          

            return System.Threading.Tasks.Task.CompletedTask;
        }
    }


    public static class DetectIntents
    {
        public static QueryResult DetectIntentFromTexts(string projectId,
                                                string sessionId,
                                                string text,
                                                string languageCode = "en-US")
        {
            var client = SessionsClient.Create();
            
                var response = client.DetectIntent(
                    session:  SessionName.FormatProjectSession(projectId,sessionId),
                    queryInput: new QueryInput()
                    {
                        Text = new TextInput()
                        {
                            Text = text,
                            LanguageCode = languageCode
                        }
                    }
                );

                var queryResult = response.QueryResult;

                //Console.WriteLine($"Query text: {queryResult.QueryText}");
                //if (queryResult.Intent != null)
                //{
                //    Console.WriteLine($"Intent detected: {queryResult.Intent.DisplayName}");
                //}
                //Console.WriteLine($"Intent confidence: {queryResult.IntentDetectionConfidence}");
                //Console.WriteLine($"Fulfillment text: {queryResult.FulfillmentText}");
                //Console.WriteLine();


            

            return queryResult;
        }
    }
}
