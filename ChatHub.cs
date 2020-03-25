using GloEpidBot.Model.Domain;
using GloEpidBot.Utilities;
using Google.Cloud.Dialogflow.V2;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace GloEpidBot
{
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IOptions<LuisConfig> options;
        public ChatHub(IOptions<LuisConfig> option)
        {
            options = option;
        }
        string key = string.Empty;
        List<question> Questions = DetectIntents.ReturnQuestions();
        Report report = new Report();
        
        override
      public System.Threading.Tasks.Task OnConnectedAsync()
        {
             key = Context.ConnectionId;

            Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Welcome!, What's your Name ?" });
            return System.Threading.Tasks.Task.CompletedTask;
        }


        public async System.Threading.Tasks.Task<System.Threading.Tasks.Task> SendResponse(string message, int QuestionId)
        {
            try
            {
               
                var msg = new Message
                {
                    data = message
                };
                var stringContent = new StringContent(JsonConvert.SerializeObject(msg), Encoding.UTF8, "application/json");


                var result = await ExternalService.MakeCallPost(stringContent,options);

                var res = JsonConvert.DeserializeObject<LuisResponse>(await result.Content.ReadAsStringAsync());






                var quest = Questions[QuestionId];
                Response response = new Response();

                if (quest.IntentName.ToLower() == res.topScoringIntent.intent.ToLower())
                {
                    if(res.topScoringIntent.intent == "NoTravelHistoryProvider")
                    {
                        await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Gotcha!" });
                        await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[QuestionId + 2].quest, QuestionId + 2 });
                    }



                    else if (Questions.Count > QuestionId)
                    {
                        await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] {"Gotcha!" });
                        await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[QuestionId + 1].quest, QuestionId + 1 });
                        
                    }
                    else
                    {
                        //Response 
                        await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "All good!" });
                    }
                       

                }
                   
                else
                {
                   
                        await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "I didn't get that!" });
                        await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[QuestionId].quest, QuestionId + 1 });
                   
                       
                }
                    




                
            }
            catch (System.Exception ex)
            {

              await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { ex.Message });
            }
          

            return System.Threading.Tasks.Task.CompletedTask;
        }
    }

    public class Report
    {
        public string Symptoms { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string TravelHistory { get; set; }
    }

    public class Message
    {
        public string data;
    }
    public class question
    {
        public string IntentName { get; set; }
        public string quest { get; set; }

    }

    public class Response
    {
        public string message  { get; set; }
        public string QuestionId { get; set; }
    }
    public class LuisResponse
    {
        public string query { get; set; }
        public TopScoringIntent topScoringIntent { get; set; }
        public List<entities> entities { get; set; }
        public SentimentAnalysis sentimentAnalysis { get; set; }
    }

    public class TopScoringIntent
    {
        public string intent { get; set; }
        public double score { get; set; }
    }
    public class entities
    {
        public string entity { get; set; }
        public string type { get; set; }
        public string startIndex { get; set; }
        public string endIndex { get; set; }
    }
    public class SentimentAnalysis
    {

    }
    public static class DetectIntents
    {

        public static List<question> ReturnQuestions ()
        {
            var Questions = new List<question> {
                new question
                {
                     quest = "What's your name",
                      IntentName = "NameProvider"
                },
                new question
                {
                    quest = "Where is your location (General location)",
                     IntentName = "LocationProvider"
                },
                  new question
                {
                    quest = "What is your occupation?",
                     IntentName = "OccupationProvider"
                },
                    new question
                {
                    quest = "What is your recent travel history?",
                     IntentName = "TravelHistoryProvider"
                },
                      new question
                {
                    quest = "Where is your House Address ",
                     IntentName = "HouseAddressProvider"
                },
                        new question
                {
                    quest = "Do you have any of these symptoms ? Cough, Fever, Difficulty breathing",
                     IntentName = "SymptomsProvider"
                },
                          new question
                {
                    quest = "when did the symptoms start?",
                     IntentName = "SymptomsStartProvider"
                },
                            new question
                {
                    quest = "Have you been self isolating?",
                     IntentName = "IsolationProvider"
                },
                              new question
                {
                    quest = "Have people been visiting you?",
                     IntentName = "VisitingProvider"
                },
                                new question
                {
                    quest = "Have you been in contact with someone who just arrived Nigeria?",
                     IntentName = "ContactProvider"
                },
                                  new question
                {
                    quest = "Where do you work?",
                     IntentName = "WorkProvider"
                }

           };

            return Questions;
        }






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
