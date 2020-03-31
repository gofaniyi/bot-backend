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
                    options = "Below 10,10-19,20-29,30-39,40-49,50-59,60+".Split(','),
                    quest = "What is your age?",
                    optionsNextId = "2,2,2,2,2,2,2".Split(','),
                    NextQuestionYes = 2,
                    NextQuestionNo = 2
                },
                new question
                {
                    QuestionId = 2,
                    HasOptions =false,
                    quest = "Where are you right now? (Area,State) ",
                    optionsNextId = "3".Split(','),
                    NextQuestionYes = 3
                },

                 new question
                {
                    QuestionId = 3,
                    HasOptions =true,
                    quest = "Have you travelled outside the country within the last 14 days? ",
                    optionsNextId = "7,4".Split(','),
                    NextQuestionYes = 7,
                    NextQuestionNo = 4,
                    options = "Yes,No".Split(',')
                },

                 new question
                 {
                    QuestionId = 4,
                    HasOptions =true,
                    quest = "Have you been in contact with a confirmed case of coronavirus (COVID-19)?",
                    optionsNextId = "7,5,5".Split(','),
                    NextQuestionYes = 7,
                    NextQuestionNo = 6,
                    options = "Yes,No,Not to my knowledge".Split(','),

                 },
                  new question
                 {
                    QuestionId = 5,
                    HasOptions =true,
                    quest = "Have you been in contact with someone who just arrived in Nigeria in the last one month? ",
                    optionsNextId = "6,7,7".Split(','),
                    NextQuestionYes = 6,
                    NextQuestionNo = 7,
                    options = "Yes,No,Not to my knowledge".Split(','),

                 },
                   new question
                 {
                    QuestionId = 6,
                    HasOptions =true,
                    quest = "Is the individual sick or indicating any COVID-19 symptoms?",
                    optionsNextId = "7,7".Split(','),
                    NextQuestionYes = 7,
                    NextQuestionNo = 7,
                    options = "Yes,No".Split(','),
                    IsMultipleChoice = false

                 },
                   new question
                 {
                    QuestionId = 7,
                    HasOptions =true,
                    quest = "Have you been experiencing any of the following (Select all that apply)",
                    optionsNextId = "8,8,8,8,8,12".Split(','),
                    NextQuestionYes = 8,
                    NextQuestionNo = 12,
                    options = "Difficulty in breathing,fatigue/tiredness,Sore throat,Dry Cough,Fever > 37.8 C,None of the symptoms".Split(','),
                    IsMultipleChoice = true

                 },

                   new question
                   {
                       QuestionId = 8,
                       HasOptions =false,
                       quest = "when did the symptoms start? (day/month)",
                       optionsNextId = "10".Split(','),
                       NextQuestionNo = 10,
                       NextQuestionYes = 10

                   },
                   
                   new question
                   {
                       QuestionId =9,
                       HasOptions = true,
                       quest = "Have you been self-isolating?",
                       optionsNextId = "12,10".Split(','),
                       NextQuestionYes = 12,
                       NextQuestionNo  = 10,
                       options = "Yes,No".Split(',')


                   },
                   new question
                   {
                       QuestionId =10,
                       HasOptions = true,
                       quest = "Have you visited any public space since you first started to notice symptoms?",
                       optionsNextId = "12,12".Split(','),
                        NextQuestionYes = 11,
                        NextQuestionNo = 12,
                        options = "Yes,No".Split(',')
                   },
                    new question
                   {
                       QuestionId =11,
                       HasOptions = false,
                       quest = "Where did you go? (use this format) Area,State ",
                      optionsNextId = "12".Split(','),
                       NextQuestionNo = 12,
                       
                   },
                     new question
                   {
                       QuestionId =12,
                       HasOptions = false,
                      quest = "Kindly provide your phone number",
                      optionsNextId = "13".Split(','),
                      NextQuestionNo = 13,

                   },
                      new question
                   {
                       QuestionId =13,
                       HasOptions = false,
                      quest = "Kindly provide your  contact address",
                      optionsNextId = "35".Split(','),
                      NextQuestionNo = 35,

                   }
                    


             };

                return Questions;
            }




        }
    
}
