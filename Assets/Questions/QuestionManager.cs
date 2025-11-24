using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Questions.Questions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;
using UnityEngine.UI;

namespace Questions
{
    public class QuestionManager : MonoBehaviour
    {
        public TextMeshProUGUI questionTMP;
        public TextMeshProUGUI answer1TMP;
        public TextMeshProUGUI answer2TMP;
        public TextMeshProUGUI answer3TMP;
        public TextMeshProUGUI answer4TMP;
        
        public Button[] answerButtons;
        public Button nextButton;
        
        public static int lastCorrect;
        public static string[] lastAnswers = new string[4];
        
        public static List<IQuestion> questions;
        public IQuestion questionChosen;
        
        private Color[] originalColors; 
      
        
        public TextMeshProUGUI timerTMP;
        public TextMeshProUGUI scoreTMP;

        private float timer = 30f;
        public static int score;

        private bool answered = false;
        
        
        private void Start()
        {
           
            originalColors = new Color[answerButtons.Length];
            for (int i = 0; i < answerButtons.Length; i++)
                originalColors[i] = answerButtons[i].GetComponent<Image>().color;

          
            nextButton.gameObject.SetActive(false);

            if (questions == null || questions.Count == 0)
            {
                if (GameSettings.Instance.easyMode)
                    questions = new List<IQuestion>(EasyQuestions);
                else if (!GameSettings.Instance.easyMode)
                    questions = new List<IQuestion>(HardQuestions);
            }

            questionChosen = GetRandomQuestion(questions);
            questions.Remove(questionChosen);

            
                questionTMP.text = questionChosen.Question;

                answer1TMP.text = questionChosen.Answer1;
                answer2TMP.text = questionChosen.Answer2;
                answer3TMP.text = questionChosen.Answer3;
                answer4TMP.text = questionChosen.Answer4;
            
          
        }

        public void AnswerSelected(int buttonClicked)
        {
            if (answered) return;
            answered = true;

            // désactive les boutons
            foreach (var b in answerButtons)
                b.interactable = false;

            // Colorier
            for (int i = 0; i < answerButtons.Length; i++)
            {
                Image img = answerButtons[i].GetComponent<Image>();

                if (i == questionChosen.Answer)
                    img.color = Color.green;
                else
                    img.color = Color.red;
            }

            // Score ✔️
            if (buttonClicked == questionChosen.Answer)
            {
                questionTMP.text = "Bonne Reponse";

                int questionScore = 5000;

                if (timer <= 30)
                {
                    float penalty = (30 - timer) * 100;
                    questionScore -= Mathf.RoundToInt(penalty);

                    if (questionScore < 0) questionScore = 0;
                }

                score += questionScore;
                
                scoreTMP.text = "Score : " + questionScore;
            }
            else
            {
                questionTMP.text = "Mauvaise Reponse";
                scoreTMP.text = "Score : " + score;
            }

            nextButton.gameObject.SetActive(true);
        }

        
        public static List<IQuestion> EasyQuestions = new List<IQuestion>()
        {
            new Question1()
            {
                Id = 0,
                Question = "Quel est le plus grand pays du monde",
                Answer1 = "Canada",
                Answer2 = "France",
                Answer3 = "Russie",
                Answer4 = "Maroc",
                Answer = 2
            },
            new Question2()
            {
                Id = 1,
                Question = "Quel pays n'est pas en Asie",
                Answer1 = "Chine",
                Answer2 = "Japon",
                Answer3 = "Corée",
                Answer4 = "Ukraine",
                Answer = 3
            },
            new Question3()
            {
                Id = 2,
                Question = "Quel pays est le plus peuple",
                Answer1 = "Inde",
                Answer2 = "Chine",
                Answer3 = "États-Unis",
                Answer4 = "Nigéria",
                Answer = 0
            },
            new Question4()
            {
                Id = 3,
                Question = "Quel pays a une feuille d'erable sur son drapeau",
                Answer1 = "Mexique",
                Answer2 = "Canada",
                Answer3 = "Italie",
                Answer4 = "Egypte",
                Answer = 1
            }
        };

        public static List<IQuestion> HardQuestions = new List<IQuestion>()
        {
            new Question1()
            {
                Id = 0,
                Question = "Sur quel continent se situe le departement francais de la Guyane",
                Answer1 = "Amerique du Sud",
                Answer2 = "Afrique",
                Answer3 = "Europe",
                Answer4 = "Asie",
                Answer = 0
            },
            new Question2()
            {
                Id = 1,
                Question = "Quel pays n'est pas en Europe",
                Answer1 = "Estonie",
                Answer2 = "Moldavie",
                Answer3 = "Andorre",
                Answer4 = "Armenie",
                Answer = 3
            },
            
            new Question3()
            {
                Id = 2,
                Question = "Quel pays est accusé de persecuter la minorite ouighoure ?",
                Answer1 = "Russie",
                Answer2 = "Arabie Saoudite",
                Answer3 = "Chine",
                Answer4 = "Colombie",
                Answer = 2
            },
            
            new Question4 ()
            {
                Question = "De quel pays le congo a t'il obtenu son independance en 1960",
                Answer1 = "France",
                Answer2 = "Belgique",
                Answer3 = "Spain",
                Answer4= "Royaume-Uni",
                Answer = 1
            },
            
        };
        
        public IQuestion GetRandomQuestion(List<IQuestion> x)
        {
            return x[Random.Range(0, x.Count)];
        }
        
        public void Next()
        {
            if (questions == null || questions.Count == 0)
            {
                SceneManager.LoadScene(2);
                return;
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        private void Update()
        {
            if (answered) return; // stop timer si réponse donnée

            timer -= Time.deltaTime;

            if (timer < 0)
            {
                timer = 0;
                ForceWrongAnswer();
            }

            timerTMP.text = timer.ToString("0"); // affiche juste un entier
        }
        private void ForceWrongAnswer()
        {
            answered = true;

            // colorie boutons
            for (int i = 0; i < answerButtons.Length; i++)
            {
                Image img = answerButtons[i].GetComponent<Image>();

                if (i == questionChosen.Answer)
                    img.color = Color.green;
                else
                    img.color = Color.red;
            }

            // désactive les boutons
            foreach (var b in answerButtons)
                b.interactable = false;

            questionTMP.text = "Temps ecoule ! Mauvaise reponse";
            
            scoreTMP.text = "Score : " + 0;

            nextButton.gameObject.SetActive(true);
        }

    }
}