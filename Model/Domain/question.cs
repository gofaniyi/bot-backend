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
        public bool IsMultipleChoice { get; set; }

    }
    public class questions
    {
        public string questionsId { get; set; }
        public string question { get; set; }
        public string response { get; set; }
        public string score { get; set; }
    }
    public class questionsModel
    {
     
        public string question { get; set; }
        public string response { get; set; }
        public string score { get; set; }
    }
}
