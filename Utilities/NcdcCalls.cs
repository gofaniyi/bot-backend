using GloEpidBot.Model.Domain;
using GloEpidBot.Persistence.Contexts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GloEpidBot.Utilities
{
    public static class NcdcCalls
    {
        

       

        public static async Task SendToNCDCAsync(List<questionsModel> questions, string RiskLevel)
        {
            EvaluatedRiskLevel evaluatedRiskLevel = EvaluatedRiskLevel.MediumRisk;
            if (RiskLevel.ToLower() == "high")
                evaluatedRiskLevel = EvaluatedRiskLevel.HighRisk;
            else if (RiskLevel.ToLower() == "low")
                evaluatedRiskLevel = EvaluatedRiskLevel.LowRisk;
            else if (RiskLevel.ToLower() == "medium")
                evaluatedRiskLevel = EvaluatedRiskLevel.MediumRisk;


            var AssData = new assesmentModel
            {
                questions = questions,
                evaluationOutcome = RiskLevel,
                evaluationTime = DateTime.Now,
                source = "GLOEPID-BOT",
                channel = "WEB",
                publicKey = "",
                EvaluatedRiskLevel = evaluatedRiskLevel

            };
            var myContent = JsonConvert.SerializeObject(AssData);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
           
            ExternalService.MakeCallNCDC(byteContent);

        }
    }
}
