using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloEpidBot.Model.Domain
{
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

}
