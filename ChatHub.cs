using GloEpidBot.Model.Domain;
using GloEpidBot.Persistence.Contexts;
using GloEpidBot.Utilities;
using Google.Cloud.Dialogflow.V2;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace GloEpidBot
{
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {
        readonly List<question> Questions = DetectIntents.ReturnQuestions();
        private readonly AppDbContext db;
        public ChatHub(AppDbContext db)
        {
            this.db = db;
        }


        override
      public System.Threading.Tasks.Task OnConnectedAsync()
        {


            Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Hello I'm Gloepid Bot ", Questions[0] });
            Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Here to help you assess your health, answer your pressing questions about COVID-19and if necessary contact healthcare services", Questions[0] });

            Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Let us start with some basic information", Questions[0] });
            Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Welcome!, What's your Full name ?", Questions[0] });



            return System.Threading.Tasks.Task.CompletedTask;
        }


        public System.Threading.Tasks.Task SendResponse(string[] answers, string message, int QuestionId, int NextQuestionId)
        {


            if (NextQuestionId == 30)
            {
                Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "You seem to be doing fine at the moment. But stay alert and be cautious.", Questions[0] });
                Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "You can run the assessment test anytime (we recommend daily if you have not been staying indoors). ", Questions[0] });
                Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { " =>> Wash hands regularly and sanitize", Questions[0] });
                Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { " =>> Avoid touching your face especially nose, mouth and eyes", Questions[0] });
                Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { " =>> Practice social distancing and stay Indoors", Questions[0] });
                Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { " =>> Turn on your Bluetooth when you go out", Questions[0] });
                Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { " =>> Wipe and disinfect regularly touched surfaces(door knobs, phone, counter tops etc.)", Questions[0] });
                Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { " =>> Eat healthy and do not self-medicate", Questions[0] });
                Clients.Client(Context.ConnectionId).SendCoreAsync("CloseConnection", new object[] { "Terminate connection" });
                return System.Threading.Tasks.Task.CompletedTask;
            }
            else if (NextQuestionId == 35)
            {

                Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Ok, I am sending your details to the health care authorities for a follow-up", Questions[0] });
               
                Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "In the meantime kindly do the following", Questions[0] });
                Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Remain calm ", Questions[0] });
                Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Grant Gloepid permission to upload your data so that I can track and notify those you may have been in contact with", Questions[0] });
                Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Stay indoors and if you live with others isolate yourself in a room. ", Questions[0] });
                Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Wait for healthcare services to contact you and safely guide you to the nearest treatment center", Questions[0] });
               

                //var assesment = new SelfAssesment
                //{
                //{
                //    Name = Context.Items["name"].ToString(),
                //    CloseContact = Context.Items["closecontact"].ToString(),
                //    Location = Context.Items["location"].ToString(),
                //    Ocupation = Context.Items["occupation"].ToString(),
                //    Symptoms = Context.Items["symptoms"].ToString(),
                //    SymptomsStart = Context.Items["symptomstart"].ToString(),
                //    PublicPlace = Context.Items["publicplaces"].ToString(),
                //    TravelHistory = Context.Items["travelhistory"].ToString(),
                //    Id = Guid.NewGuid().ToString(),
                //  //  PublicPlaces = Context.Items["visitedpublicplace"].ToString()
                //};
                var ass = new SelfAssesment();
                ass.Name = Context.Items["name"].ToString();
                ass.Location = Context.Items["location"].ToString();
                ass.Ocupation = Context.Items["occupation"].ToString();
                ass.SymptomsStart = Context.Items["symptomstart"].ToString();
                ass.Symptoms = Context.Items["symptoms"].ToString();
                ass.Id = Guid.NewGuid().ToString();
                db.Assesments.Add(ass);
                db.SaveChanges();
                Clients.Client(Context.ConnectionId).SendCoreAsync("CloseConnection", new object[] { "Terminate connection" });
                return System.Threading.Tasks.Task.CompletedTask;
                
            }
            else
            {


                // 

                try
                {

                    //Check if question has options

                    var question = Questions[QuestionId];

                    if (question.HasOptions)
                    {
                        //don't send to LUIS

                        if (answers.Length > 0)//Check if response was returned 
                        {

                            if (QuestionId == 7) //If question is symptoms
                            {
                                //extract answer to report class
                                string Symptoms = String.Empty;
                                foreach (var item in answers)
                                {
                                    Symptoms = Symptoms + "," + item;
                                }
                                Context.Items.Add("symptoms", Symptoms);
                                if (answers.Length >= 2)
                                {
                                    Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Gotcha", Questions[NextQuestionId] });
                                    Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });

                                    return System.Threading.Tasks.Task.CompletedTask;
                                }
                            }
                            else
                            {
                                // answers[0] retrieve answer for report

                                if (QuestionId == 3)
                                {
                                    Context.Items.Add("istravelled", answers[0]);
                                }



                                else if (QuestionId == 9)
                                {
                                   

                                    Context.Items.Add("publicplaces", answers[0]);
                                    if (answers[0] == "No")
                                    {
                                        Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Great job, the chances of the virus spreading is reduced ", Questions[NextQuestionId] });
                                        Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });

                                        return System.Threading.Tasks.Task.CompletedTask;
                                    }
                                }
                                else if (QuestionId == 5)
                                {
                                    if(answers[0] == "Yes")
                                    {
                                        Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "You have done a great job so far, now I will ask some health related questions ", Questions[NextQuestionId] });
                                    }
                                    else
                                    {
                                        Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Gotcha!", Questions[NextQuestionId] });
                                    }
                                   
                                    Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });
                                    return System.Threading.Tasks.Task.CompletedTask;
                                }
                                else if (QuestionId == 6)
                                {
                                    Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "You have done a great job so far, now I will ask some health related questions ", Questions[NextQuestionId] });
                                    Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });
                                    return System.Threading.Tasks.Task.CompletedTask;
                                }
                                else if (QuestionId == 11)
                                {
                                    Context.Items.Add("closecontact", answers[0]);
                                }
                                // assesment.CloseContact = answers[0];


                            }
                        }
                        else
                        {
                            //Resend question, No answers sent

                            Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "OOps, didn't catch that, come again?!", Questions[QuestionId] });
                            Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[QuestionId].quest, Questions[QuestionId] });
                            return System.Threading.Tasks.Task.CompletedTask;
                        }

                    }
                    else
                    {

                        //send to LUIS


                        if (QuestionId == 2)
                        {
                          
                            Context.Items.Add("location", message);
                            Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Gotcha", Questions[NextQuestionId] });
                            Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });
                            return System.Threading.Tasks.Task.CompletedTask;
                        }
                        else if (QuestionId == 8)
                        {
                            Context.Items.Add("symptomstart", message);
                            Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Gotcha", Questions[NextQuestionId] });
                            Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });
                            return System.Threading.Tasks.Task.CompletedTask;
                        }

                        else if(QuestionId == 10)
                        {
                           // Context.Items.Add("symptomstart", message);
                            Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Gotcha", Questions[NextQuestionId] });
                            Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });
                            return System.Threading.Tasks.Task.CompletedTask;
                        }
                      





                        var predictionResult = Luiscalls.GetPredictionAsync(message).Result;

                        var res = predictionResult.Prediction;



                        //Answer matches question
                        //Get the question type and fix answer into the assesment variable
                        if (QuestionId == 0)
                        {
                            if (res.Entities.ContainsKey("personName"))
                            {
                                var NameData = (JArray)res.Entities["personName"];
                                string PersonName = String.Empty;
                                foreach (var data in NameData)
                                {
                                    PersonName += data;
                                }
                                Context.Items.Add("name", PersonName);
                            }
                            else if (message.Split().Length <= 2)
                            {
                                Context.Items.Add("name", message);
                            }
                            else
                            {

                                Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "I didn't get that!, Say that again?", Questions[QuestionId] });
                                return System.Threading.Tasks.Task.CompletedTask;
                            }

                        }

                        else if (QuestionId == 1)
                        {

                            if (res.Entities.ContainsKey("occupation"))
                            {
                                var OccupationData = (JArray)res.Entities["occupation"];
                                string Occupation = String.Empty;
                                foreach (var data in OccupationData)
                                {
                                    Occupation += data;
                                }
                                Context.Items.Add("occupation", Occupation);
                            }
                            else if (message.Split().Length <= 2)
                            {
                                Context.Items.Add("occupation", message);
                            }
                            else
                            {
                                Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "I didn't get that!, Say that again?", Questions[QuestionId] });
                                return System.Threading.Tasks.Task.CompletedTask;
                            }

                        }

                        else if (QuestionId == 4)
                        {

                            if (res.Entities.ContainsKey("geographyV2"))
                            {
                                var GeoData = (JArray)res.Entities["geographyV2"];
                                var d = GeoData.ToObject<List<LuisIntent>>();
                                string Location = String.Empty;
                                foreach (var item in d)
                                {
                                    Location =Location + " " +  item.value;
                                }

                                Context.Items.Add("travelhistory", Location);
                                Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "You have done a great job so far, now I will ask some health related questions ", Questions[NextQuestionId] });
                                Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });
                                return System.Threading.Tasks.Task.CompletedTask;
                            }
                            else
                            {
                                Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "I didn't get that!, Say that again?", Questions[QuestionId] });
                                return System.Threading.Tasks.Task.CompletedTask;
                            }

                        }


                        else
                        {
                            Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "I didn't get that!, Say that again?", Questions[QuestionId] });
                            return System.Threading.Tasks.Task.CompletedTask;

                        }

                    }

                   



                    // Send Next Question based on Logic
                    Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Gotcha", Questions[NextQuestionId] });
                    Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });



















                }
                catch (System.Exception ex)
                {

                    Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { ex.Message, Questions[QuestionId] });
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

        public string quest { get; set; }
        public string[] options { get; set; }
        public string[] optionsNextId { get; set; }
        public int NextQuestionYes { get; set; }
        public int NextQuestionNo { get; set; }
        public bool HasOptions { get; set; }

    }

    public class Response
    {
        public string message { get; set; }
        public string QuestionId { get; set; }
    }

    public class LuisIntent
    {
        public string value { get; set; }
        public string type { get; set; }
    }
    public class entities
    {
        public string[] entity { get; set; }

    }
    public class SentimentAnalysis
    {

    }
    public static class DetectIntents
    {

        public static List<question> ReturnQuestions()
        {
            var Questions = new List<question> {

                new question
                {
                     QuestionId = 0,
                     HasOptions = false,
                     optionsNextId = "1".Split(','),
                     quest = "What is your full name",
                     NextQuestionYes = 1
                },
                new question
                {
                    QuestionId = 1,
                    HasOptions =false,
                    quest = "What do you do? (profession)",
                    optionsNextId = "2".Split(','),
                    NextQuestionYes = 2
                },
                new question
                {
                    QuestionId = 2,
                    HasOptions =false,
                    quest = "What is your current location/where are you right now? - Area ",
                    optionsNextId = "3".Split(','),
                    NextQuestionYes = 3
                },

                 new question
                {
                    QuestionId = 3,
                    HasOptions =true,
                    quest = "Have you travelled within or outside country recently? ",
                    optionsNextId = "4,5".Split(','),
                    NextQuestionYes = 4,
                    NextQuestionNo = 5,
                    options = "Yes,No".Split(',')
                },
                 new question
                {
                    QuestionId = 4,
                    HasOptions =false,
                    quest = "where did you go? (city) ",
                    optionsNextId = "7".Split(','),
                    NextQuestionYes = 7,
                },

                 new question
                 {
                    QuestionId = 5,
                    HasOptions =true,
                    quest = "Have you been in contact with someone who just arrived Nigeria in the last one month?",
                    optionsNextId = "7,6,7".Split(','),
                    NextQuestionYes = 7,
                    NextQuestionNo = 6,
                    options = "Yes,No,Not to my knowledge".Split(','),
                    
                 },
                  new question
                 {
                    QuestionId = 6,
                    HasOptions =true,
                    quest = "Have you been in contact with someone known to have corona virus (COVID-19)? ",
                    optionsNextId = "7,7,7".Split(','),
                    NextQuestionYes = 7,
                    NextQuestionNo = 7,
                    options = "Yes,No,Not to my knowledge".Split(','),

                 },
                   new question
                 {
                    QuestionId = 7,
                    HasOptions =true,
                    quest = "Have you been experiencing any of the following  ",
                    optionsNextId = "8,30".Split(','),
                    NextQuestionYes = 8,
                    NextQuestionNo = 30,
                    options = "Difficulty in breathing,fatigue/tiredness,Sore throat,Dry cough,Fever".Split(','),

                 },
                   new question
                   {
                       QuestionId = 8,
                       HasOptions = false,
                       quest = "when did they start?",
                       optionsNextId = "9".Split(','),
                       NextQuestionYes = 9,

                   },
                   new question
                   {
                       QuestionId = 9,
                       HasOptions =true,
                       quest = "Have you visited any public space since you first started to notice symptoms? ",
                       optionsNextId = "10,11".Split(','),
                       options = "Yes,No".Split(','), 
                       NextQuestionNo = 11,
                       NextQuestionYes = 10

                   },
                   new question
                   {
                       QuestionId =10,
                       HasOptions = false,
                       quest = "Where did you go? address or location name",
                       optionsNextId = "11".Split(','),
                       NextQuestionYes = 11,
                          

                   },
                   new question
                   {
                       QuestionId =11,
                       HasOptions = true,
                       quest = "Have you being in close physical contact with others",
                       optionsNextId = "35,35".Split(','),
                        NextQuestionYes = 35,
                        NextQuestionNo = 35,
                        options = "Yes,No".Split(',')
                   }


             };

            return Questions;
        }




    }
}
