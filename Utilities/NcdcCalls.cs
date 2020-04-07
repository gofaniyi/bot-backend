using GloEpidBot.Model.Domain;
using GloEpidBot.Persistence.Contexts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GloEpidBot.Utilities
{
    public static class NcdcCalls
    {
        

       

        public static async Task SendToNCDCAsync(List<AssessmentResponsesModel> questions, string RiskLevel, string Channel, string State, string Phone, string[] symptoms, string FullName)
        {
            var statesCode = "FC,AB,AD,AK,AN,BA,BY,BE,BO,CR,DE,EB,ED,EK,EN,GO,IM,JI,KD,KN,KT,KE,KO,KW,LA,NA,NI,OG,ON,OS,OY,PL,RI,SO,TA,YO,ZA".Split(',');
            var states = "Abuja,Abia,Adamawa,Akwa Ibom,Anambra,Bauchi,Bayelsa,Benue,Borno,Cross River,Delta,Ebonyi,Edo,Ekiti,Enugu,Gombe,Imo,Jigawa,Kaduna,Kano,Katsina,Kebbi,Kogi,Kwara,Lagos,Nasarawa,Niger,Ogun,Ondo,Osun,Oyo,Plateau,Rivers,Sokoto,Taraba,Yobe,Zamfara".Split(',');
            string LocationCode = string.Empty;
            if (states.Contains(State))
            {
                int i = Array.IndexOf(states, State);
                LocationCode = statesCode[i];

            }
               



            var AssData = new AssessmentModel
            {
                assessmentResponses = questions,
              
                createdAt = DateTime.Now,
                State = LocationCode,
                PhoneNumber = Phone,
                Symptoms = symptoms,
                FullName = FullName,
                sourcePartnerId = 1
            };
            if (RiskLevel.ToLower() == "high")
                AssData.assessmentResult = AssessmentStatus.HighRisk;
            else if (RiskLevel.ToLower() == "medium")
                AssData.assessmentResult = AssessmentStatus.MediumRisk;
            else if (RiskLevel.ToLower() == "low")
                AssData.assessmentResult = AssessmentStatus.LowRisk;

            if (Channel.ToLower() == "mobile")
                AssData.assessmentChannel = Utilities.Channel.GloEpidMobile;
            else if (Channel.ToLower() == "web")
                AssData.assessmentChannel = Utilities.Channel.Web;


            var myContent = JsonConvert.SerializeObject(AssData);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
           
            ExternalService.MakeCallNCDCAsync(byteContent);

        }
    }
    public class AssessmentModel
    {
        public long? gloEpidUserId { get; set; }
        public IEnumerable<AssessmentResponsesModel> assessmentResponses { get; set; }
        public long sourcePartnerId { get; set; }
        public Channel assessmentChannel { get; set; }
        public DateTime createdAt { get; set; }
        public AssessmentStatus assessmentResult { get; set; }
        public string State { get; set; }
        public string PhoneNumber { get; set; }
        public string [] Symptoms { get; set; }
        public string FullName { get; set; }

    }
    public enum Channel
    {
        [Description("USSD")]
        USSD = 1,
        [Description("Mobile")]
        Mobile,
        [Description("GloEpid Mobile")]
        GloEpidMobile,
        Web
    }
    public class AssessmentResponsesModel
    {

        public string question { get; set; }
        public string response { get; set; }
        public int score { get; set; }
    }
    public enum AssessmentStatus
    {
        [Description("Not Assessed")]
        NotAssessed = 0,
        [Description("Low Risk")]
        LowRisk,
        [Description("Medium Risk")]
        MediumRisk,
        [Description("High Risk")]
        HighRisk,
        [Description("Confirmed")]
        Confirmed,
        [Description("Healed")]
        Healed
    }
}
