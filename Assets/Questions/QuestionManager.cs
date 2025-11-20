using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Questions.Questions;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Questions
{
    public class QuestionManager : MonoBehaviour
    {
        public TextMeshProUGUI questionTMP;
        public TextMeshProUGUI answer1TMP;
        public TextMeshProUGUI answer2TMP;
        public TextMeshProUGUI answer3TMP;
        public TextMeshProUGUI answer4TMP;

        public IQuestion questionChosen;
        private void Start()
        {
            List<IQuestion> questions = GetAllQuestions();
            
            IQuestion questionFound = GetRandomQuestion(questions);
            
            questionChosen = questionFound;
            
            questionTMP.text = questionFound.Question;
            
            answer1TMP.text = questionFound.Answer1;
            answer2TMP.text = questionFound.Answer2;
            answer3TMP.text = questionFound.Answer3;
            answer4TMP.text = questionFound.Answer4;
            
        }

        public void AnswerSelected(int buttonClicked)
        {
            if (buttonClicked == questionChosen.Answer)
            {
                questionTMP.text = "YES MY SIGMA";
            }
            else
            {
                questionTMP.text = "No MY SIGMA";
            }
        }
        
        public static List<IQuestion> GetAllQuestions()
        { 
            /*List<IQuestion> questions = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass &&
                            !t.IsAbstract &&
                            typeof(IQuestion).IsAssignableFrom(t))
                .ToList();

            int id = 0;
            foreach (var question in questions)
            {
                question. = id++;
            }*/
            return new List<IQuestion>()
            {
                new Question1()
                {
                    Id = 0,
                    Question = "Sigma are not?",
                    Answer1 = "YES",
                    Answer2 = "No",
                    Answer3 = "Maybe",
                    Answer4 = "No you",
                    Answer = 0,
                    Tip = "Sigma boy dans le bendo",
                    AnswerCoordinate = new Coordinate(3, 3)
                }
            };
        }

        public IQuestion GetRandomQuestion(List<IQuestion> x)
        {
            return x[Random.Range(0, x.Count)];
        }
    }
}