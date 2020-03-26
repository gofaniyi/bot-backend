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
            Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Hello I'm Gloepid Bot ", 0 });
            Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Here to help you assess your health, answer your pressing questions about COVID-19and if necessary contact healthcare services", 0 });
            
            Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Let us start with some basic information", 0 });
            Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Welcome!, What's your Name ?", 0 });

            return System.Threading.Tasks.Task.CompletedTask;
        }


        public async System.Threading.Tasks.Task<System.Threading.Tasks.Task> SendResponse(string [] answers,string  message, int QuestionId)
        {
            try
            {

                //Check if question has options

                var question  = Questions[QuestionId];

                if (question.HasOptions)
                {
                    //don't send to LUIS

                    if (answers.Length > 0)//Check if response was returned 
                    {

                        if(QuestionId == 5) //If question is symptoms
                        {
                            //extract answer to report class
                        }
                        else
                        {
                           // answers[0] retrieve answer for report
                        }



                    }
                    else
                    {
                        //Resend question
                    }

                }
                else
                {

                    //send to LUIS
                }


                // Send Next Question based on Logic







                var msg = new Message
                {
                    data = message
                };
                var stringContent = new StringContent(JsonConvert.SerializeObject(msg), Encoding.UTF8, "application/json");


                var result = await ExternalService.MakeCallGet(message);
                
                var res = JsonConvert.DeserializeObject<LuisResponse>(await result.Content.ReadAsStringAsync());

            




                var quest = Questions[QuestionId];
                Response response = new Response();

                if (quest.IntentName.ToLower() == res.prediction.topIntent.ToLower())
                {
                    if(res.prediction.topIntent == "NoTravelHistoryProvider")
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
                   
                        await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "I didn't get that!" , QuestionId});
                        await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[QuestionId].quest, QuestionId });
                   
                       
                }
                    




                
            }
            catch (System.Exception ex)
            {

              await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { ex.Message,  });
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
        public string [] options { get; set; }

        public bool HasOptions { get; set; }

    }

    public class Response
    {
        public string message  { get; set; }
        public string QuestionId { get; set; }
    }
    public class LuisResponse
    {
        public string query { get; set; }
        public Prediction prediction { get; set; }
      
      //  public SentimentAnalysis sentimentAnalysis { get; set; }
    }

    public class Prediction
    {
        public string topIntent { get; set; }
        public LuisIntent  intent { get; set; }
        public entities entities { get; set; }
    }

    public class LuisIntent
    {
        public string name { get; set; }
        public double score { get; set; }
    }
    public class entities
    {
        public string [] entity { get; set; }
      
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
                      IntentName = "NameProvider",
                      HasOptions = false
                },
                new question
                {
                    quest = "What do you do?",
                     IntentName = "OccupationProvider",
                     HasOptions  = false
                },
                  new question
                {
                    quest = "What is your current location/where are you right now? Use this format area/city/state",
                     IntentName = "LocationProvider",
                     HasOptions = false
                },
                    new question
                {
                    quest = "Have you travelled within or outside country recently?",
                     IntentName = "TravelHistoryProvider",
                     options = "Yes, No But I know someone who has travelled out, No I haven't travelled out or know anyone who has travelled out".Split(','),
                     HasOptions = true
                },
                      new question
                {
                    quest = "Where is your House Address ",
                     IntentName = "HouseAddressProvider",
                     HasOptions = false
                },
                        new question
                {
                    quest = "Have you been experiencing any of the following ?",
                     IntentName = "SymptomsProvider",
                     options  = "Dry cough,Fever,Difficulty in breathing,fatigue/tiredness,Sore throat".Split(","),
                     HasOptions = true
                     
                },
                          new question
                {
                    quest = "when did the symptoms start?",
                     IntentName = "SymptomsStartProvider"
                },
                            new question
                {
                    quest = "Have you visited any public space since you first started to notice symptoms? ",
                     IntentName = "IsolationProvider",
                     options = "Yes,No".Split(','),
                     HasOptions = true
                },
                              new question
                {
                    quest = "Have you being in close physical contact with others?",
                     IntentName = "ContactProvider",
                       options = "Yes,No".Split(','),
                       HasOptions = true
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
