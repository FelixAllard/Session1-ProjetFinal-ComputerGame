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
                questionTMP.text = "Bonne Réponse";
                Start();
            }
            else
            {
                questionTMP.text = "Mauvaise Réponse";
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
                    Question = "Quel est le plus grand pays du monde",
                    Answer1 = "Canada",
                    Answer2 = "France",
                    Answer3 = "Russie",
                    Answer4 = "Maroc",
                    Answer = 2,
                    Tip = "En Asie",
                    AnswerCoordinate = new Coordinate(3, 3)
                },
                
                new Question2()
                  {
                      Id = 1,
                      Question = "Quel pays n'est pas en Asie",
                      Answer1 = "Chine",
                      Answer2 = "Japon",
                      Answer3 = "Corée",
                      Answer4 = "Ukraine",
                      Answer = 3,
                      Tip = "Herp",
                      AnswerCoordinate = new Coordinate(3, 3)
                  },
                 new Question3()
                  {
                      Id = 2,
                      Question = "Quel pays est le plus peuplé",
                      Answer1 = "Inde",
                      Answer2 = "Chine",
                      Answer3 = "États-Unis",
                      Answer4 = "Nigéria",
                      Answer = 0,
                      Tip = "Herp",
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