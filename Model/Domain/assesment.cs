using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloEpidBot.Model.Domain
{
    public class assesment
    {
        public string assesmentId { get; set; }
        public string source { get; set; }
        public questions questions { get; set; }
        public string evaluationScore { get; set; }
        public DateTime evaluationTime { get; set; }
        public string evaluationOutcome { get; set; }
    }

    public class assesmentModel
    {
        public string source { get; set; }
        public List<questionsModel> questions { get; set; }
        public EvaluatedRiskLevel EvaluatedRiskLevel { get; set; }
        public string evaluationScore { get; set; }
        public DateTime evaluationTime { get; set; }
        public string evaluationOutcome { get; set; }
    }

    public enum EvaluatedRiskLevel
    {
        LowRisk,
        MediumRisk,
        HighRisk
    }
}
