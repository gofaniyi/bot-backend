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
        readonly List<question> Questions = QuestionBox.ReturnQuestions();
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
               

                
                
                var ass = new SelfAssesment();
                object Symptoms = String.Empty;
              Context.Items.TryGetValue("symptoms", out Symptoms);
              ass.Symptoms = Symptoms.ToString();



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

        
    }

  
   
   


    public class LuisIntent
    {
        public string value { get; set; }
        public string type { get; set; }
    }
    
   
}
