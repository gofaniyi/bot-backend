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
                     quest = "What is your Full name",
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
                    HasOptions =true,
                    quest = "Have you travelled (either inter-state or outside the country) in the last 30 days? ",
                    optionsNextId = "10,3".Split(','),
                    NextQuestionYes = 10,
                    NextQuestionNo = 3,
                    options = "Yes,No".Split(',')
                },

                 new question
                 {
                    QuestionId = 3,
                    HasOptions =true,
                    quest = "Have you been in contact with a confirmed case of coronavirus (COVID-19)?",
                    optionsNextId = "4,8,4".Split(','),
                    NextQuestionYes = 4,
                    NextQuestionNo = 8,
                    options = "Yes,No,Not to my knowledge".Split(','),

                 },
                 new question
                 {
                     QuestionId = 4,
                     HasOptions = true,
                     quest = "What kind of contact have you had",
                     IsMultipleChoice = false,
                     optionsNextId = "5,6,7".Split(','),
                     options = "Personal Contact and Accomodation,Medical and Healthcare,Social interaction".Split(','),
                   

                 },
                 new question
                 {
                     QuestionId  = 5,
                     HasOptions = true,
                     IsMultipleChoice = true,
                     quest = "Personal Contact (please select all that apply and press send)",
                     options = "Greater than 15 minutes face-to-face (<2 meters distance) direct physical contact with a probable or confirmed case,Living or sleeping in the same home with confirmed COVID-19 case,Shared accommodation sharing kitchen or bathroom facilities with confirmed case,Spouse or sexual partner of confirmed case,Infant/child of a positive COVID-19 Mother,Commuter/passenger sitting within 5 feet (in any direction) of the COVID-19 case in a tricycle vehicle airplane train or ship, none of the above".Split(','),
                     optionsNextId = "10,10,10,10,10,10,10".Split(','),
                     NextQuestionYes = 10,
                     NextQuestionNo = 10
                 },
                 new question
                 {
                     QuestionId = 6,
                     HasOptions = true,
                     IsMultipleChoice = true,
                     quest = "Medical and Healthcare (please select all that apply and press send)",
                     options = "Cared for a probable or confirmed COVID-19 case including care for dead body,healthcare worker who has had direct contact with a confirmed case,healthcare worker who has either not worn appropriate PPE or had a breach in PPE while in contact with confirmed case,laboratory staff who have handled specimens of suspected cases unprotected or has a breach of laboratory containment while handling specimens?,Been involved in emergency/exposure prone procedures including surgery/CPR/ intubation /suctioning on confirmed cases and without use of appropriate PPE, none of the above".Split(','),
                     optionsNextId = "10,10,10,10,10,10,10".Split(','),
                     NextQuestionYes = 10,
                     NextQuestionNo = 10
                 },
                 new question
                 {
                     QuestionId = 7,
                     HasOptions = true,
                     quest = "Social interaction (please select all that apply and press send)",
                     IsMultipleChoice = true,
                     options = "social/religious gatherings e.g. weddings, burials,Church and Mosque services with a probable or confirmed Covid-19 case,Are you living in the same building (Not same home) with a probable or confirmed Covid-19 case without sharing kitchen or bathroom facilities,None of the above".Split(','),
                      optionsNextId = "10,10,10,10,10,10,10".Split(','),
                     NextQuestionYes = 10,
                     NextQuestionNo = 10

                 },
                  new question
                 {
                    QuestionId = 8,
                    HasOptions =true,
                    quest = "Have you been in contact with someone who just arrived in Nigeria in the last 14 days? ",
                    optionsNextId = "9,10,10".Split(','),
                    NextQuestionYes = 9,
                    NextQuestionNo = 10,
                    options = "Yes,No,Not to my knowledge".Split(','),

                 },
                   new question
                 {
                    QuestionId = 9,
                    HasOptions =true,
                    quest = "Is the individual sick or indicating any COVID-19 symptoms?",
                    optionsNextId = "10,10".Split(','),
                    NextQuestionYes = 10,
                    NextQuestionNo = 10,
                    options = "Yes,No".Split(','),
                    IsMultipleChoice = false

                 },
                   new question
                 {
                    QuestionId = 10,
                    HasOptions =true,
                    quest = "Have you been experiencing any of the following (Select all that apply and press send)",
                    optionsNextId = "11,11,11,11,11,35".Split(','),
                    NextQuestionYes = 11,
                    NextQuestionNo = 35,
                    options = "Difficulty in breathing,fatigue/tiredness,Sore throat,Dry Cough,Fever > 37.8 C,None of the symptoms".Split(','),
                    IsMultipleChoice = true

                 },

                   new question
                   {
                       QuestionId = 11,
                       HasOptions =false,
                       quest = "when did the symptoms start?",
                       optionsNextId = "12".Split(','),
                       NextQuestionNo = 12,
                       NextQuestionYes = 12

                   },
                   
                   new question
                   {
                       QuestionId =12,
                       HasOptions = true,
                       quest = "Have you been self-isolating?",
                       optionsNextId = "35,13".Split(','),
                       NextQuestionYes = 35,
                       NextQuestionNo  = 13,
                       options = "Yes,No".Split(',')


                   },
                   new question
                   {
                       QuestionId =13,
                       HasOptions = true,
                       quest = "Have you visited any public space since you first started to notice symptoms?",
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
