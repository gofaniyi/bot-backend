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


            Clients.Client(Context.ConnectionId).SendCoreAsync("WelcomeMessage", new object[] { new string[] { "Hello I'm Gloepid Bot", "Here to help you assess your COVID-19 risk factor and know if you need to contact the NCDC.", "Please note that I am an assessment tool and should not be used for diagnostic purposes", "Let’s begin when you are ready" }, Questions[0] });
        
            return System.Threading.Tasks.Task.CompletedTask;
        }

        public System.Threading.Tasks.Task SelfIdentify(string channel)
        {
            if (channel != null)
                Context.Items.Add("channel", channel);
            return System.Threading.Tasks.Task.CompletedTask;
        }
        public async System.Threading.Tasks.Task<System.Threading.Tasks.Task> SendResponse(string[] answers, string message, int QuestionId, int NextQuestionId)
        {


           
            if (NextQuestionId == 35)
            {
                Context.Items.Add("home", message);
                object risklevel = string.Empty;
                Context.Items.TryGetValue("risklevel", out risklevel);

                if (risklevel.ToString() == "high")
                {
                   await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "**High Risk**", Questions[0] });
                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Please be patient and wait for NCDC to contact you", Questions[0] });
                  await  Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "In the meantime kindly do the following", Questions[0] });
                 await   Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Remain calm ", Questions[0] });
                 await   Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Self-Isolate", Questions[0] });
                 await   Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Wait for healthcare services to contact you for further information and next steps", Questions[0] });


                  await  Clients.Client(Context.ConnectionId).SendCoreAsync("CloseConnection", new object[] { "Terminate connection" });
                }
                else
                {
                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "**Medium Risk**", Questions[0] });
                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "You may have been exposed. Please self isolate and monitor your health status", Questions[0] });
                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { " if there are any changes to your symptoms, please call a doctor or take the assessment test again", Questions[0] });
                    await Clients.Client(Context.ConnectionId).SendCoreAsync("CloseConnection", new object[] { "Terminate connection" });


                }


                














                await Clients.Client(Context.ConnectionId).SendCoreAsync("CloseConnection", new object[] { "Terminate connection" });


                await SendResultAsync();





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
                               
                                string Symptoms = String.Empty;
                                foreach (var item in answers)
                                {
                                    Symptoms = Symptoms + "," + item;
                                }
                                Context.Items.Add("symptoms", Symptoms);

                                int r = calculate();
                                if(r == 0)
                                {
                                    return System.Threading.Tasks.Task.CompletedTask;
                                }
                                await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });
                                return System.Threading.Tasks.Task.CompletedTask;

                            }
                           
                            else
                            {
                                // answers[0] retrieve answer for report

                                if (QuestionId == 3)
                                {
                                    Context.Items.Add("istravelled", answers[0]);
                                }

                                else if(QuestionId == 1)
                                {
                                    Context.Items.Add("age", answers[0]);
                                }
            
                                else if (QuestionId == 9)
                                {
                                   

                                  Context.Items.Add("selfisolating", answers[0]);    
                                  await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });
                                    return System.Threading.Tasks.Task.CompletedTask;

                                }


                                else if (QuestionId == 4)
                                {


                                    Context.Items.Add("closecontactcorona", answers[0]);
                                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });
                                    return System.Threading.Tasks.Task.CompletedTask;

                                }

                                else if (QuestionId == 5)
                                {
                                    Context.Items.Add("closecontactnigeria", answers[0]);
                                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Okay", Questions[NextQuestionId] });
                                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });

                                    return System.Threading.Tasks.Task.CompletedTask;
                                }
                                else if (QuestionId == 6)
                                {
                                    Context.Items.Add("contactsick", answers[0]);
                                   
                                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });

                                    return System.Threading.Tasks.Task.CompletedTask;
                                }
                                else if (QuestionId == 10)
                                {
                                    Context.Items.Add("publicplaces", answers[0]);
                                       
                                        await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });

                                        return System.Threading.Tasks.Task.CompletedTask;
                                   
                                   
                                }
                                


                            }
                        }
                        else
                        {
                            //Resend question, No answers sent

                            await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "OOps, didn't catch that, come again?!", Questions[QuestionId] });
                            await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[QuestionId].quest, Questions[QuestionId] });
                            return System.Threading.Tasks.Task.CompletedTask;
                        }

                    }
                    else
                    {

                        //send to LUIS


                        if (QuestionId == 2)
                        {
                          //TODO : Check location with bing api
                          if(answers.Length == 0)
                            {
                                string r = BingCalls.ValidateLocation(message);
                                if (r != "false")
                                {

                                    Context.Items.Add("location", r);
                             
                                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });
                                    return System.Threading.Tasks.Task.CompletedTask;
                                }
                                else
                                {
                                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "please use this format Area,State", Questions[QuestionId] });
                                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[QuestionId].quest, Questions[QuestionId] });
                                    return System.Threading.Tasks.Task.CompletedTask;
                                }
                              
                            }
                            else
                            {
                                double longitude, latitude;
                                double.TryParse(answers[0], out longitude);
                                double.TryParse(answers[1], out latitude);

                                string location  = await BingCalls.GetLocation(longitude, latitude);
                                await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { location, Questions[NextQuestionId] });
                            }
                            
                        
                         
                        }
                        else if (QuestionId == 8)
                        {
                            if (BingCalls.ValidateDate(message))
                            {
                                Context.Items.Add("symptomstart", message);
                              
                                await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });
                                return System.Threading.Tasks.Task.CompletedTask;
                            }
                            else
                            {
                                await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "please use this format day/month", Questions[QuestionId] });
                                await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[QuestionId].quest, Questions[QuestionId] });
                                return System.Threading.Tasks.Task.CompletedTask;
                            }
                          
                        }
                      
                        else if(QuestionId == 12)
                        {
                           
                            if(Luiscalls.IsPhoneNumber(message))
                            {
                                Context.Items.Add("phone", message);
                         
                                await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });
                                return System.Threading.Tasks.Task.CompletedTask;
                            }
                            else
                            {
                               
                                await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Didn't get that, try again?", Questions[NextQuestionId] });
                                await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[QuestionId].quest, Questions[QuestionId] });
                                return System.Threading.Tasks.Task.CompletedTask;
                            }
                           
                        }
                        else if (QuestionId == 13)
                        {

                            Context.Items.Add("homeaddress", message);
                     
                            await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });
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

                                await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "I didn't get that!, Say that again?", Questions[QuestionId] });
                                return System.Threading.Tasks.Task.CompletedTask;
                            }

                        }

                     

                     


                        else
                        {
                            await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "I didn't get that!, Say that again?", Questions[QuestionId] });
                            return System.Threading.Tasks.Task.CompletedTask;

                        }

                    }





                    // Send Next Question based on Logic
                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Okay", Questions[NextQuestionId] });
                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });








                }
                catch (System.Exception ex)
                {

                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { ex.Message, Questions[QuestionId] });
                }
            }



            return System.Threading.Tasks.Task.CompletedTask;
        }

        public async System.Threading.Tasks.Task SendResultAsync()
        {
            object symptoms = string.Empty, istravelled = string.Empty, age = string.Empty, selfisolating = string.Empty, closecontactcorona = string.Empty, closecontactnigeria = string.Empty, contactsick = string.Empty, publicplaces = string.Empty, location = string.Empty, symptomstart = string.Empty, phone = string.Empty, homeaddress = string.Empty, name = string.Empty, risklevel = string.Empty;
            object channel = string.Empty;
            Context.Items.TryGetValue("risklevel", out risklevel);
            Context.Items.TryGetValue("symptoms", out symptoms);
            Context.Items.TryGetValue("istravelled", out istravelled);
            Context.Items.TryGetValue("age", out age);
            Context.Items.TryGetValue("closecontactcorona", out closecontactcorona);
            Context.Items.TryGetValue("closecontactnigeria", out closecontactnigeria);
            Context.Items.TryGetValue("contactsick", out contactsick);
            Context.Items.TryGetValue("publicplaces", out publicplaces);
            Context.Items.TryGetValue("location", out location);
            Context.Items.TryGetValue("symptomstart", out symptomstart);
            Context.Items.TryGetValue("phone", out phone);
            Context.Items.TryGetValue("home", out homeaddress);
            Context.Items.TryGetValue("name", out name);
            Context.Items.TryGetValue("channel", out channel);

            List<AssessmentResponsesModel> questions = new List<AssessmentResponsesModel>()
           {
                new AssessmentResponsesModel
                {
                     question = "What is your name",
                      response = name == null ? "" : name.ToString(),

                },
                new AssessmentResponsesModel
                {
                     question = "What is your age?",
                      response = age == null ? "" : age.ToString(),

                },
                new AssessmentResponsesModel
                {
                     question = "Where are you right now? (Area,State)",
                      response = location == null ? "" : location.ToString(),

                },
                new AssessmentResponsesModel
                {
                     question = "Have you travelled outside the country within the last 14 days? ",
                      response = istravelled == null ? "" : istravelled.ToString(),

                },
                new AssessmentResponsesModel
                {
                     question = "Have you been in contact with a confirmed case of coronavirus (COVID-19)?",
                      response = closecontactcorona == null ? "" : closecontactcorona.ToString(),

                },
                new AssessmentResponsesModel
                {
                     question = "Have you been in contact with someone who just arrived in Nigeria in the last one month?",
                      response = closecontactnigeria == null ? "" : closecontactnigeria.ToString(),

                },
                new AssessmentResponsesModel
                {
                     question = "Is the individual sick or indicating any COVID-19 symptoms?",
                      response = contactsick == null ? "" : contactsick.ToString(),

                },
                new AssessmentResponsesModel
                {
                     question = "Have you been experiencing any of the following (Select all that apply)",
                      response = symptoms == null ? "" : symptoms.ToString(),

                },
                new AssessmentResponsesModel
                {
                     question = "when did the symptoms start? (day/month) - (01/10)",
                      response = symptomstart == null ? "" : symptomstart.ToString(),

                },
                new AssessmentResponsesModel
                {
                     question = "Kindly provide your phone number",
                      response = phone == null ? "" : phone.ToString(),

                },
                new AssessmentResponsesModel
                {
                     question = "Kindly provide your  contact address",
                      response = homeaddress == null ? "" : homeaddress.ToString(),

                }
           };
            string RiskLevel = risklevel == null ? "" : risklevel.ToString();
            string state = location == null ? "" : location.ToString();
            if (channel == null)
                channel = string.Empty;
            await NcdcCalls.SendToNCDCAsync(questions, RiskLevel,channel.ToString(), state, phone.ToString(), symptoms.ToString().Split(','), name.ToString());
            
             

             
        }
        public int calculate()
        {
            object travelhistory = string.Empty, symptoms =string.Empty, closecontact = string.Empty, location =  string.Empty, contactsick = string.Empty;

            Context.Items.TryGetValue("closecontactcorona", out closecontact);
            Context.Items.TryGetValue("istravelled", out travelhistory);
            Context.Items.TryGetValue("contactsick", out contactsick);
            Context.Items.TryGetValue("location", out location);
            Context.Items.TryGetValue("symptoms", out symptoms);
            if(contactsick == null)
            {
                contactsick = string.Empty;
            }
            if (closecontact == null)
            {
                closecontact = string.Empty;
            }

            if (travelhistory != null && travelhistory.ToString() != "No" && (symptoms.ToString().Contains("Cough") || symptoms.ToString().Contains("Difficulty in breathing") || symptoms.ToString().Contains("Fever")))
            {

                Context.Items.Add("risklevel", "high");
                return 2;

            }else if(closecontact.ToString() == "Yes" && (symptoms.ToString().Contains("Cough") || symptoms.ToString().Contains("Difficulty in breathing") || symptoms.ToString().Contains("Fever")))
            {
                Context.Items.Add("risklevel", "high");
                return 2;
            }
            else if((symptoms.ToString().Contains("Cough") || symptoms.ToString().Contains("Difficulty in breathing") || symptoms.ToString().Contains("Fever")) && (location.ToString().Contains("Lagos")|| location.ToString().Contains("Oyo")|| location.ToString().Contains("Abuja"))) {
                Context.Items.Add("risklevel", "high");
                return 2;
            }

            else if(travelhistory.ToString() == "Yes" && symptoms.ToString().Contains("None of the symptoms"))
            {
                Context.Items.Add("risklevel", "medium");
                return 1;
            }
            else if(closecontact.ToString() == "Yes" && symptoms.ToString().Contains("None of the symptoms"))
            {
                Context.Items.Add("risklevel", "medium");
                return 1;
            }
            else if(contactsick.ToString() == "Yes" && symptoms.ToString().Contains("None of the symptoms"))
            {
                Context.Items.Add("risklevel", "medium");
                return 1;
            }
            else if(contactsick.ToString() == "Not to my knowledge" && (symptoms.ToString().Contains("Cough") || symptoms.ToString().Contains("Difficulty in breathing") || symptoms.ToString().Contains("Fever")))
            {
                Context.Items.Add("risklevel", "medium");
                return 1;
            }
            else if((symptoms.ToString().Contains("Cough") || symptoms.ToString().Contains("Difficulty in breathing") || symptoms.ToString().Contains("Fever")))
            {
                Context.Items.Add("risklevel", "medium");
                return 1;
            }
            else
            {
                Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "**Low Risk**", Questions[0] });
                Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "You seem to be doing fine at the moment. But stay alert and practice social distancing.", Questions[0] });
                 Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "You can call a doctor if you have any unrelated health issues or questions.", Questions[0] });
                 
                Clients.Client(Context.ConnectionId).SendCoreAsync("CloseConnection", new object[] { "Terminate connection" });
                return 0;

            }
        }
    }

  
   
   


    public class LuisIntent
    {
        public string value { get; set; }
        public string type { get; set; }
    }
    
   
}
