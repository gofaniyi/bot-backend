using GloEpidBot.Model.Domain;
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
       
        public ChatHub()
        {
           
        }


        override
      public  System.Threading.Tasks.Task OnConnectedAsync()
        {


            Clients.Client(Context.ConnectionId).SendCoreAsync("WelcomeMessage", new object[] { new string[] { "Hello I am NCDC Bot powered by GloEpid", "Here to help you assess your COVID-19 risk factor and know if you need to contact the NCDC.", "Please note that I am an assessment tool and should not be used for diagnostic purposes", "Let’s begin when you are ready" }, Questions[0] });
        

            return System.Threading.Tasks.Task.CompletedTask;
        }

        public System.Threading.Tasks.Task SelfIdentify(string channel)
        {
            if (channel != null)
                Context.Items.Add("channel", channel);
            return System.Threading.Tasks.Task.CompletedTask;
        }

        public async System.Threading.Tasks.Task<System.Threading.Tasks.Task> ProvideLocation(double longitude, double latitude)
        {
            string location  = await BingCalls.GetLocation(longitude, latitude);
            Context.Items.Add("location", location);
            return System.Threading.Tasks.Task.CompletedTask;
        }

        public  System.Threading.Tasks.Task ProvidePhone(string phone)
        {
            Luiscalls.IsPhoneNumber(phone);
            Context.Items.Add("phone", phone);
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
                  await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "NCDC Number - 0800-970000-10 Toll Free Call Center", Questions[0] });
                await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Send a DM to Twitter @NCDCGov", Questions[0] });
                await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Email address to reach NCDC - info@ndcd.gov.ng", Questions[0] });

                  await  Clients.Client(Context.ConnectionId).SendCoreAsync("CloseConnection", new object[] { "Terminate connection" });
                }
                else
                {
                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "**Medium Risk**", Questions[0] });
                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "You may have been exposed. Please self isolate and monitor your health status", Questions[0] });
                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { " if there are any changes to your symptoms, please call a doctor or take the assessment test again", Questions[0] });
                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "NCDC Number - 0800-970000-10 Toll Free Call Center", Questions[0] });
                await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Send a DM to Twitter @NCDCGov", Questions[0] });
                await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Email address to reach NCDC - info@ndcd.gov.ng", Questions[0] });
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

                            if (QuestionId == 10) //If question is symptoms
                            {
                               
                                string Symptoms = String.Empty;
                                foreach (var item in answers)
                                {
                                    Symptoms += item + ",";
                                }
                                Context.Items.TryAdd("symptoms", Symptoms);
                             //   Context.Items.Add("symptoms", Symptoms);

                                int r = calculate();
                                if(r == 0)
                                {
                                    return System.Threading.Tasks.Task.CompletedTask;
                                }
                                await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });
                                return System.Threading.Tasks.Task.CompletedTask;

                            }
                            else if(QuestionId == 5 || QuestionId == 6 || QuestionId == 7)
                            {
                                string ContactForms = String.Empty;
                                foreach(var item in answers)
                                {
                                    ContactForms = ContactForms + "," + item;
                                }
                                Context.Items.TryAdd("contactforms", ContactForms);
                            }
                           
                            else
                            {
                                // answers[0] retrieve answer for report

                                if (QuestionId == 2)
                                {
                                    Context.Items.TryAdd("istravelled", answers[0]);
                                }

                                else if(QuestionId == 1)
                                {
                                    Context.Items.TryAdd("age", answers[0]);
                                }
            
                                else if (QuestionId == 12)
                                {
                                   

                                  Context.Items.TryAdd("selfisolating", answers[0]);    
                                  await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });
                                    return System.Threading.Tasks.Task.CompletedTask;

                                }


                                else if (QuestionId == 3)
                                {


                                    Context.Items.TryAdd("closecontactcorona", answers[0]);
                                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });
                                    return System.Threading.Tasks.Task.CompletedTask;

                                }

                                else if (QuestionId == 8)
                                {
                                    Context.Items.TryAdd("closecontactnigeria", answers[0]);
                                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "Okay", Questions[NextQuestionId] });
                                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });

                                    return System.Threading.Tasks.Task.CompletedTask;
                                }
                                else if (QuestionId == 9)
                                {
                                    Context.Items.TryAdd("contactsick", answers[0]);
                                   
                                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });

                                    return System.Threading.Tasks.Task.CompletedTask;
                                }
                                else if (QuestionId == 13)
                                {
                                    Context.Items.TryAdd("publicplaces", answers[0]);
                                       
                                        await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });

                                        return System.Threading.Tasks.Task.CompletedTask;
                                   
                                   
                                }
                                else if(QuestionId == 4)
                                {
                                    Context.Items.TryAdd("contacttype", answers[0]);
                                    await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });

                                    return System.Threading.Tasks.Task.CompletedTask;
                                }
                                


                            }
                        }
                        else
                        {
                            //Resend question, No answers sent

                            await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { "OOps, didn't catch that, come again?!", Questions[QuestionId] });
                            return System.Threading.Tasks.Task.CompletedTask;
                        }

                    }
                    else
                    {

                        
                        if (QuestionId == 11)
                        {
                            string response = BingCalls.ValidateDate(message);
                            if (response != "Date format not recognized, Try again" && response != "Date format not recognized, Try again")
                            {
                                Context.Items.TryAdd("symptomstart", response);
                              
                                await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { Questions[NextQuestionId].quest, Questions[NextQuestionId] });
                                return System.Threading.Tasks.Task.CompletedTask;
                            }
                            else
                            {
                                await Clients.Client(Context.ConnectionId).SendCoreAsync("ReceiveResponse", new object[] { response, Questions[QuestionId] });
                              
                                return System.Threading.Tasks.Task.CompletedTask;
                            }
                          
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
                                Context.Items.TryAdd("name", PersonName);
                            }
                            else if (message.Split().Length <= 2)
                            {
                                Context.Items.TryAdd("name", message);
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
            object contactforms = string.Empty;
            object contacttype = string.Empty;
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
            Context.Items.TryGetValue("name", out name);
            Context.Items.TryGetValue("channel", out channel);
            Context.Items.TryGetValue("contactforms", out contactforms);
            Context.Items.TryGetValue("contacttype", out contacttype);


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
                     question = "when did the symptoms start?",
                      response = symptomstart == null ? "" : symptomstart.ToString(),

                },
                new AssessmentResponsesModel
                {
                     question = "What kind of contact have you had with a confirmed case of coronavirus (COVID-19)",
                      response = contacttype == null ? "" : contacttype.ToString(),
                },
                 new AssessmentResponsesModel
                {
                     question = "Forms of contact",
                      response = contactforms == null ? "" : contactforms.ToString(),
                }

           };
            string RiskLevel = risklevel == null ? "" : risklevel.ToString();
            string state = location == null ? "" : location.ToString();
            string homeAddress = homeaddress == null ? "" : homeaddress.ToString();
            if (channel == null)
                channel = string.Empty;
            await NcdcCalls.SendToNCDCAsync(questions, RiskLevel,channel.ToString(), state, phone.ToString(), symptoms.ToString().Split(','), name.ToString(),homeaddress.ToString());
            
             

             
        }
        public int calculate()
        {
            object travelhistory = string.Empty, symptoms =string.Empty, closecontact = string.Empty, location =  string.Empty, contactsick = string.Empty;
            object contacttype = string.Empty;
            Context.Items.TryGetValue("closecontactcorona", out closecontact);
            Context.Items.TryGetValue("istravelled", out travelhistory);
            Context.Items.TryGetValue("contactsick", out contactsick);
            Context.Items.TryGetValue("location", out location);
            Context.Items.TryGetValue("symptoms", out symptoms);
            Context.Items.TryGetValue("contacttype", out contacttype);
            if (contactsick == null)
            {
                contactsick = string.Empty;
            }
            if (closecontact == null)
            {
                closecontact = string.Empty;
            }
            if(location == null)
            {
                location = string.Empty;
            }
            if (contacttype == null)
                contacttype = string.Empty;

            if (travelhistory != null && travelhistory.ToString() != "No" && (symptoms.ToString().Contains("Cough") || symptoms.ToString().Contains("Difficulty in breathing") || symptoms.ToString().Contains("Fever")))
            {

                Context.Items.TryAdd("risklevel", "high");
                return 2;

            }else if(closecontact.ToString() == "Yes" && (contacttype.ToString().Contains("Personal Contact and Accomodation") || contacttype.ToString().Contains("Medical and Healthcare") ) && (symptoms.ToString().Contains("Cough") || symptoms.ToString().Contains("Difficulty in breathing") || symptoms.ToString().Contains("Fever")))
            {
                Context.Items.TryAdd("risklevel", "high");
                return 2;
            }
            else if((symptoms.ToString().Contains("Cough") || symptoms.ToString().Contains("Difficulty in breathing") || symptoms.ToString().Contains("Fever")) && (location.ToString().Contains("Lagos")|| location.ToString().Contains("Oyo")|| location.ToString().Contains("FCT") || location.ToString().Contains("Osun")|| location.ToString().Contains("Kastina")|| location.ToString().Contains("Abuja")|| location.ToString().Contains("Ogun") || location.ToString().Contains("Edo")|| location.ToString().Contains("Kano") || location.ToString().Contains("Kwara") || location.ToString().Contains("Kaduna") || location.ToString().Contains("Akwa Ibom"))) {
                Context.Items.TryAdd("risklevel", "high");
                return 2;
            }

            else if(travelhistory.ToString() == "Yes" && symptoms.ToString().Contains("None of the symptoms"))
            {
                Context.Items.TryAdd("risklevel", "medium");
                return 1;
            }
            else if(closecontact.ToString() == "Yes" && symptoms.ToString().Contains("None of the symptoms") && contacttype.ToString().Contains("Social interaction"))
            {
                Context.Items.TryAdd("risklevel", "medium");
                return 1;
            }
            else if(contactsick.ToString() == "Yes" && symptoms.ToString().Contains("None of the symptoms"))
            {
                Context.Items.TryAdd("risklevel", "medium");
                return 1;
            }
            else if(contactsick.ToString() == "Not to my knowledge" && (symptoms.ToString().Contains("Cough") || symptoms.ToString().Contains("Difficulty in breathing") || symptoms.ToString().Contains("Fever")))
            {
                Context.Items.TryAdd("risklevel", "medium");
                return 1;
            }
            else if((symptoms.ToString().Contains("Cough") || symptoms.ToString().Contains("Difficulty in breathing") || symptoms.ToString().Contains("Fever")))
            {
                Context.Items.TryAdd("risklevel", "medium");
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
