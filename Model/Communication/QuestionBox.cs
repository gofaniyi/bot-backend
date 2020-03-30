using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloEpidBot.Model.Domain
{
   
        public static class QuestionBox
        {

            public static List<question> ReturnQuestions()
            {
                var Questions = new List<question> {

                new question
                {
                     QuestionId = 0,
                     HasOptions = false,
                     optionsNextId = "1".Split(','),
                     quest = "What is your name",
                     NextQuestionYes = 1,
                     IsMultipleChoice = false

                },
                new question
                {
                    QuestionId = 1,
                    HasOptions =true,
                    options = "Below 0,10-19,20-29,30-39,40-49,50-59,60+".Split(','),
                    quest = "What is your age?",
                    optionsNextId = "2".Split(','),
                    NextQuestionYes = 2
                },
                new question
                {
                    QuestionId = 2,
                    HasOptions =false,
                    quest = "Where are you right now? - Area ",
                    optionsNextId = "3".Split(','),
                    NextQuestionYes = 3
                },

                 new question
                {
                    QuestionId = 3,
                    HasOptions =true,
                    quest = "Have you travelled outside country recently? ",
                    optionsNextId = "4,5".Split(','),
                    NextQuestionYes = 4,
                    NextQuestionNo = 5,
                    options = "Yes,No".Split(',')
                },
                 new question
                {
                    QuestionId = 4,
                    HasOptions =false,
                    quest = "What date did you arrive in Nigeria",
                    optionsNextId = "7".Split(','),
                    NextQuestionYes = 7,
                },

                 new question
                 {
                    QuestionId = 5,
                    HasOptions =true,
                    quest = "Have you been in contact with a confirmed case of coronavirus (COVID-19)?",
                    optionsNextId = "7,6,6".Split(','),
                    NextQuestionYes = 7,
                    NextQuestionNo = 6,
                    options = "Yes,No,Not to my knowledge".Split(','),

                 },
                  new question
                 {
                    QuestionId = 6,
                    HasOptions =true,
                    quest = "Have you been in contact with someone who just arrived in Nigeria in the last one month? ",
                    optionsNextId = "7,7,7".Split(','),
                    NextQuestionYes = 7,
                    NextQuestionNo = 7,
                    options = "Yes,No,Not to my knowledge".Split(','),

                 },
                   new question
                 {
                    QuestionId = 7,
                    HasOptions =true,
                    quest = "Have you been experiencing any of the following (Select all that apply)",
                    optionsNextId = "8,8".Split(','),
                    NextQuestionYes = 8,
                    NextQuestionNo = 8,
                    options = "Difficulty in breathing,fatigue/tiredness,Sore throat,Cough,Fever, None of the symptoms".Split(','),
                    IsMultipleChoice = true

                 },
                   new question
                   {
                       QuestionId = 8,
                       HasOptions = true,
                       quest = "Do you have any of the following risk factors?",
                       optionsNextId = "9".Split(','),
                       NextQuestionYes = 9,
                       IsMultipleChoice = true,
                       options  = "Fever > 100.0 F, Age >= 60 years,Chronic heart disease, Chronic lung disease, Chronic kidney disease,Receiving immunosuppressive medication, None of the risk factors".Split(',')

                   },
                   new question
                   {
                       QuestionId = 9,
                       HasOptions =false,
                       quest = "when did the symptoms start? (insert days)",
                       optionsNextId = "10".Split(','),
                       NextQuestionNo = 10,
                       NextQuestionYes = 10

                   },
                   new question
                   {
                       QuestionId =10,
                       HasOptions = true,
                       quest = "Have you been self-isolating?",
                       optionsNextId = "13,11".Split(','),
                       NextQuestionYes = 13,
                       NextQuestionNo  = 11,
                       options = "Yes,No".Split(',')


                   },
                   new question
                   {
                       QuestionId =11,
                       HasOptions = true,
                       quest = "Have you visited any public space since you first started to notice symptoms?",
                       optionsNextId = "12,13".Split(','),
                        NextQuestionYes = 12,
                        NextQuestionNo = 35,
                        options = "Yes,No".Split(',')
                   },
                    new question
                   {
                       QuestionId =12,
                       HasOptions = false,
                       quest = "Where did you go? (use this format) address or location name (use comma to separate if listing multiple locations) ",
                      optionsNextId = "13".Split(','),
                       NextQuestionNo = 13,
                       
                   },
                     new question
                   {
                       QuestionId =13,
                       HasOptions = false,
                      quest = "Kindly provide your phone number and contact address",
                      optionsNextId = "35".Split(','),
                      NextQuestionNo = 35,

                   }


             };

                return Questions;
            }




        }
    
}
