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
        

       

        public static async Task SendToNCDCAsync(List<AssessmentResponsesModel> questions, string RiskLevel, string Channel, string State)
        {
            EvaluatedRiskLevel evaluatedRiskLevel = EvaluatedRiskLevel.MediumRisk;
            if (RiskLevel.ToLower() == "high")
                evaluatedRiskLevel = EvaluatedRiskLevel.HighRisk;
            else if (RiskLevel.ToLower() == "low")
                evaluatedRiskLevel = EvaluatedRiskLevel.LowRisk;
            else if (RiskLevel.ToLower() == "medium")
                evaluatedRiskLevel = EvaluatedRiskLevel.MediumRisk;


            var AssData = new AssessmentModel
            {
                assessmentResponses = questions,
              
                createdAt = DateTime.Now,
                Location = State
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
           
            ExternalService.MakeCallNCDC(byteContent);

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
        public string Location { get; set; }

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
