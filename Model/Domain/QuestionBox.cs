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
                     quest = "What is your full name",
                     NextQuestionYes = 1
                },
                new question
                {
                    QuestionId = 1,
                    HasOptions =false,
                    quest = "What do you do? (profession)",
                    optionsNextId = "2".Split(','),
                    NextQuestionYes = 2
                },
                new question
                {
                    QuestionId = 2,
                    HasOptions =false,
                    quest = "What is your current location/where are you right now? - Area ",
                    optionsNextId = "3".Split(','),
                    NextQuestionYes = 3
                },

                 new question
                {
                    QuestionId = 3,
                    HasOptions =true,
                    quest = "Have you travelled within or outside country recently? ",
                    optionsNextId = "4,5".Split(','),
                    NextQuestionYes = 4,
                    NextQuestionNo = 5,
                    options = "Yes,No".Split(',')
                },
                 new question
                {
                    QuestionId = 4,
                    HasOptions =false,
                    quest = "where did you go? (city) ",
                    optionsNextId = "7".Split(','),
                    NextQuestionYes = 7,
                },

                 new question
                 {
                    QuestionId = 5,
                    HasOptions =true,
                    quest = "Have you been in contact with someone who just arrived Nigeria in the last one month?",
                    optionsNextId = "7,6,7".Split(','),
                    NextQuestionYes = 7,
                    NextQuestionNo = 6,
                    options = "Yes,No,Not to my knowledge".Split(','),

                 },
                  new question
                 {
                    QuestionId = 6,
                    HasOptions =true,
                    quest = "Have you been in contact with someone known to have corona virus (COVID-19)? ",
                    optionsNextId = "7,7,7".Split(','),
                    NextQuestionYes = 7,
                    NextQuestionNo = 7,
                    options = "Yes,No,Not to my knowledge".Split(','),

                 },
                   new question
                 {
                    QuestionId = 7,
                    HasOptions =true,
                    quest = "Have you been experiencing any of the following  ",
                    optionsNextId = "8,30".Split(','),
                    NextQuestionYes = 8,
                    NextQuestionNo = 30,
                    options = "Difficulty in breathing,fatigue/tiredness,Sore throat,Dry cough,Fever".Split(','),

                 },
                   new question
                   {
                       QuestionId = 8,
                       HasOptions = false,
                       quest = "when did they start?",
                       optionsNextId = "9".Split(','),
                       NextQuestionYes = 9,

                   },
                   new question
                   {
                       QuestionId = 9,
                       HasOptions =true,
                       quest = "Have you visited any public space since you first started to notice symptoms? ",
                       optionsNextId = "10,11".Split(','),
                       options = "Yes,No".Split(','),
                       NextQuestionNo = 11,
                       NextQuestionYes = 10

                   },
                   new question
                   {
                       QuestionId =10,
                       HasOptions = false,
                       quest = "Where did you go? address or location name",
                       optionsNextId = "11".Split(','),
                       NextQuestionYes = 11,


                   },
                   new question
                   {
                       QuestionId =11,
                       HasOptions = true,
                       quest = "Have you being in close physical contact with others",
                       optionsNextId = "35,35".Split(','),
                        NextQuestionYes = 35,
                        NextQuestionNo = 35,
                        options = "Yes,No".Split(',')
                   }


             };

                return Questions;
            }




        }
    
}
