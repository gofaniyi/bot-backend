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
        BotAssesment assesment = new BotAssesment();
        
        override
      public System.Threading.Tasks.Task OnConnectedAsync()
        {
             key = Context.ConnectionId;
            Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Hello I'm Gloepid Bot ", Questions[0] });
            Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Here to help you assess your health, answer your pressing questions about COVID-19and if necessary contact healthcare services", Questions[0] });
            
            Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Let us start with some basic information", Questions[0] });
            Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Welcome!, What's your Name ?", Questions[0] });

            return System.Threading.Tasks.Task.CompletedTask;
        }


        public async System.Threading.Tasks.Task<System.Threading.Tasks.Task> SendResponse(string [] answers,string  message, int QuestionId, int NextQuestionId)
        {
            if(NextQuestionId == 30)
            {
               await  Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse,", new object[] { "You seem to be doing fine at the moment. But stay alert and be cautious." , Questions[0]});
                string t = @"Remember to

 =>> Wash hands regularly and sanitize

=>> Avoid touching your face especially nose, mouth and eyes

=>> Practice social distancing and stay Indoors.

=>> Turn on your Bluetooth when you go out 

=>> Wipe and disinfect regularly touched surfaces(door knobs, phone, counter tops etc.)

=>> Eat healthy  and do not self-medicate";
                await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse,", new object[] { t, Questions[0] });
                await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse,", new object[] { "You can run the assessment test anytime (we recommend daily if you have not been staying indoors). " ,Questions[0]});
                return System.Threading.Tasks.Task.CompletedTask;
            }
            else if(NextQuestionId  == 35)
            {

                await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse,", new object[] { "Ok, I am sending your details to the health care authorities for a follow-up" });
                string t = @"In the meantime kindly do the following 

Remain calm  

Grant Gloepid permission to upload your data so that I can track and notify those you may have been in contact with 

Stay indoors and if you live with others isolate yourself in a room. 

Wait for healthcare services to contact you and safely guide you to the nearest treatment center ";
                await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse,", new object[] { t });
                return System.Threading.Tasks.Task.CompletedTask;
            }
            else
            {
                try
                {

                    //Check if question has options

                    var question = Questions[QuestionId];

                    if (question.HasOptions)
                    {
                        //don't send to LUIS

                        if (answers.Length > 0)//Check if response was returned 
                        {

                            if (QuestionId == 5) //If question is symptoms
                            {
                                //extract answer to report class
                                foreach(var item in answers)
                                {
                                    assesment.Symptoms += item;
                                }

                                if(answers.Length > 2)
                                {
                                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Gotcha", Questions[NextQuestionId] });
                                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId] });
                                }
                            }
                            else
                            {
                                // answers[0] retrieve answer for report

                                if (QuestionId == 3)
                                    assesment.TravelHistory = answers[0];
                                else if (QuestionId == 7)
                                    assesment.PublicPlace = answers[0];
                                else if (QuestionId == 8)
                                    assesment.CloseContact = answers[0];
                               

                            }



                        }
                        else
                        {
                            //Resend question, No answers sent

                            await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "OOps, didn't catch that, come again?!", QuestionId });
                            await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[QuestionId].quest, QuestionId });
                        }

                    }
                    else
                    {

                        //send to LUIS


                        if(QuestionId == 10)
                        {
                            if (!message.Contains('/'))
                            {
                                await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "OOps, didn't catch that, come again?!", Questions[QuestionId] });
                                return System.Threading.Tasks.Task.CompletedTask;
                            }
                                

                            assesment.PublicPlaces = message;
                            return System.Threading.Tasks.Task.CompletedTask;
                        }else if(QuestionId == 6)
                        {
                            assesment.SymptomsStart = message;

                            await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Gotcha", QuestionId });
                            await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId] });







                            return System.Threading.Tasks.Task.CompletedTask;
                        }







                        var predictionResult = Luiscalls.GetPredictionAsync(message).Result;

                        var res = predictionResult.Prediction;
                      
                        await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { res});
                        if (question.IntentName.ToLower() == res.TopIntent.ToLower())
                        {

                            //Answer matches question
                            //Get the question type and fix answer into the assesment variable
                            if (QuestionId == 0)
                            {
                                if (res.Entities.ContainsKey("personName"))
                                    assesment.Name = res.Entities["personName"].ToString();
                            }
                              
                            else if (QuestionId == 1)
                            {
                                //  assesment.Ocupation = res?.prediction?.entities["occupation"][0];
                                if (res.Entities.ContainsKey("occupation"))
                                {
                                    assesment.Ocupation = res.Entities["occupation"].ToString();
                                }
                                   
                            }

                            else if (QuestionId == 2)
                            {
                                //    assesment.Location = res?.prediction?.entities["geographyV2"][0];
                                if (res.Entities.ContainsKey("geographyV2"))
                                {
                                    var GeoData = (IEnumerable<LuisIntent>)res.Entities["geographyV2"];
                                    foreach (var item in GeoData)
                                    {
                                        assesment.Location += item.value + " ";
                                    }
                                }
                               
                            }

                           
                               
                          




                        }
                        else
                        {
                            await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "I didn't get that!", QuestionId });
                            await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[QuestionId].quest, QuestionId });

                        }

                    }




                    // Send Next Question based on Logic
                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Gotcha", QuestionId });
                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId]});



















                }
                catch (System.Exception ex)
                {

                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { ex.Message, });
                }
            }
          
          

            return System.Threading.Tasks.Task.CompletedTask;
        }

        public static void FixAnswer(int QuestionId, string Answer)
        {
            //var question = Questions
        }
    }

    public class Reporting
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
        public int QuestionId { get; set; }
        public string IntentName { get; set; }
        public string quest { get; set; }
        public string [] options { get; set; }
        public int NextQuestionYes { get; set; }
        public int NextQuestionNo { get; set; }
        public bool HasOptions { get; set; }

    }
    public class BotAssesment
    {
        public string Name { get; set; }
        public string TravelHistory { get; set; }
        public string PublicPlace { get; set; }
        public string PublicPlaces { get; set; }
        public string TravelPlaces { get; set; }
        public string CloseContact { get; set; }
        public string Ocupation { get; set; }
        public string Location { get; set; }
        public string HouseAddress { get; set; }
        public string Symptoms { get; set; }
        public string SymptomsStart { get; set; }

    }
    public class Response
    {
        public string message  { get; set; }
        public string QuestionId { get; set; }
    }
   
    public class LuisIntent
    {
        public string value { get; set; }
        public string type  { get; set; }
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
                      HasOptions = false,
                      NextQuestionYes = 1,
                      QuestionId = 0
                },
                new question
                {
                    quest = "What do you do?",
                     IntentName = "OccupationProvider",
                     HasOptions  = false,
                     NextQuestionYes = 2,
                     QuestionId =1
                },
                  new question
                {
                    quest = "What is your current location/where are you right now? Use this format area/city/state",
                     IntentName = "LocationProvider",
                     HasOptions = false,
                     NextQuestionYes =3,
                     QuestionId =2
                     
                },
                    new question
                {
                    quest = "Have you travelled within or outside country recently?",
                     IntentName = "TravelHistoryProvider",
                     options = "Yes, No But I know someone who has travelled out, No I haven't travelled out or know anyone who has travelled out".Split(','),
                     HasOptions = true,
                     NextQuestionYes = 35,
                     QuestionId =3,
                     NextQuestionNo =  5

                },
                      new question
                {
                    quest = "Where is your House Address ",
                     IntentName = "HouseAddressProvider",
                     HasOptions = false,
                     NextQuestionYes =5,
                     QuestionId = 4
                },
                        new question
                {
                    quest = "Have you been experiencing any of the following ?",
                     IntentName = "SymptomsProvider",
                     options  = "Dry cough,Fever,Difficulty in breathing,fatigue/tiredness,Sore throat".Split(","),
                     HasOptions = true,
                     NextQuestionYes =6,
                     NextQuestionNo = 30,
                     QuestionId =5
                     
                     
                },
                          new question
                {
                    quest = "when did the symptoms start?",
                     IntentName = "SymptomsStartProvider",
                     NextQuestionYes = 7,
                     HasOptions = false,
                     QuestionId = 6
                },
                            new question
                {
                    quest = "Have you visited any public space since you first started to notice symptoms? ",
                     IntentName = "IsolationProvider",
                     options = "Yes,No".Split(','),
                     HasOptions = true,
                     NextQuestionYes = 8,
                     NextQuestionNo = 9,
                     QuestionId = 7
                },
                new question
                {
                    quest = "What is your current location/where are you right now? Use this format area/city/state",
                     IntentName = "LocationProvider",
                     HasOptions = false,
                     NextQuestionYes =3,
                     QuestionId = 8

                },
                              new question
                {
                    quest = "Have you being in close physical contact with others?",
                     IntentName = "ContactProvider",
                       options = "Yes,No".Split(','),
                       HasOptions = true,
                       NextQuestionYes = 35,
                       NextQuestionNo = 35,
                       QuestionId = 9

                },
                              new question
                              {
                                  quest = "Where did you go (use this format) location/state, location/state",
                                  IntentName = "TravelHistoryProvider",
                                  HasOptions = false,
                                   QuestionId = 10,
                                    NextQuestionYes = 5
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
