using GloEpidBot.Model.Domain;
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
        

       

        public static async Task SendToNCDCAsync(List<AssessmentResponsesModel> questions, string RiskLevel, string Channel, string State, string Phone, string[] symptoms, string FullName, string homeAddress)
        {
            var statesCode = "NG-FC,NG-FC,NG-FC,NG-FC,NG-FC,NG-AB,NG-AD,NG-AK,NG-AN,NG-BA,NG-BY,NG-BE,NG-BO,NG-CR,NG-DE,NG-EB,NG-ED,NG-EK,NG-EN,NG-GO,NG-IM,NG-JI,NG-KD,NG-KN,NG-KT,NG-KE,NG-KO,NG-KW,NG-LA,NG-NA,NG-NI,NG-OG,NG-ON,NG-OS,NG-OY,NG-PL,NG-RI,NG-SO,NG-TA,NG-YO,NG-ZA".Split(',');
            var states = "FCT Abuja,Abuja FCT,Federal Capital Territory,FCT,Abuja,Abia,Adamawa,Akwa Ibom,Anambra,Bauchi,Bayelsa,Benue,Borno,Cross River,Delta,Ebonyi,Edo,Ekiti,Enugu,Gombe,Imo,Jigawa,Kaduna,Kano,Katsina,Kebbi,Kogi,Kwara,Lagos,Nasarawa,Niger,Ogun,Ondo,Osun,Oyo,Plateau,Rivers,Sokoto,Taraba,Yobe,Zamfara".Split(',');
            string LocationCode = string.Empty;
            if (states.Contains(State))
            {
                int i = Array.IndexOf(states, State);
                LocationCode = statesCode[i];

            }

            /*
              TECHNICAL DEBT
              1.
             
             */

            List<AssessmentResponsesModel> ResponseBox = new List<AssessmentResponsesModel>();
            foreach (var item in questions)
            {
                if (item.response != "")
                    ResponseBox.Add(item);
            }



                var AssData = new AssessmentModel
            {
                assessmentResponses = ResponseBox,
                createdAt = DateTime.Now,
                phoneNumber = Phone,
                symptoms = symptoms,
                fullName = FullName,
                stateCode = LocationCode,
                location = homeAddress
               
            };

           

            if (RiskLevel.ToLower() == "high")
                AssData.assessmentResult = AssessmentStatus.HighRisk;
            else if (RiskLevel.ToLower() == "medium")
                AssData.assessmentResult = AssessmentStatus.MediumRisk;
            else if (RiskLevel.ToLower() == "low")
                AssData.assessmentResult = AssessmentStatus.LowRisk;

            if (Channel.ToLower() == "mobile")
                AssData.assessmentChannel = Utilities.Channel.Mobile;
            else if (Channel.ToLower() == "web")
                AssData.assessmentChannel = Utilities.Channel.Web;

           
            var myContent = JsonConvert.SerializeObject(AssData);
            Console.WriteLine(myContent);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
           
            await ExternalService.MakeCallNCDCAsync(byteContent);

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
        public string location { get; set; }
        public string phoneNumber { get; set; }
        public string [] symptoms { get; set; }
        public string fullName { get; set; }
        public string email { get; set; } = "taiwo.insight@gmail.com";
        public string stateCode { get; set; }

    }
    public enum Channel
    {
        [Description("USSD")]
        USSD = 1,
        [Description("Mobile")]
        Mobile,
        [Description("Bot")]
        Bot,
        [Description("Web")]
        Web,
        [Description("Others")]
        Others
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
        NotAssessed = 1,
        [Description("Low Risk")]
        LowRisk,
        [Description("Medium Risk")]
        MediumRisk,
        [Description("High Risk")]
        HighRisk,
        [Description("Confirmed")]
        Confirmed,
        [Description("Discharged")]
        Discharged
    }
}
