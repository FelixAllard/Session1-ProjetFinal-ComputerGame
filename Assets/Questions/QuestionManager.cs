using System;
using System.Collections;
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

        public static bool suivant = false;
        
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
            
            questions = questions.OrderBy(q => Random.value).Take(3).ToList();

            questionChosen = GetRandomQuestion(questions);
            questions.Remove(questionChosen);
            
            
                questionTMP.text = questionChosen.Question;

                answer1TMP.text = questionChosen.Answer1;
                answer2TMP.text = questionChosen.Answer2;
                answer3TMP.text = questionChosen.Answer3;
                answer4TMP.text = questionChosen.Answer4;

                suivant = false;
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
                scoreTMP.text = "Score : " + 0;
            }

            
           char letter = questionChosen.Letter;
           CommunicationManager.Instance.SendMessageToRobot(letter.ToString());
           
           StartCountdown();
           
        }

        
        public static List<IQuestion> EasyQuestions = new List<IQuestion>()
        {
            new Question1()
            {
                Id = 0,
                Question = "Quel est le plus grand pays du monde ?",
                Answer1 = "Canada",
                Answer2 = "France",
                Answer3 = "Russie",
                Answer4 = "Maroc",
                Answer = 2,
                Letter ='D'
            },
            new Question2()
            {
                Id = 1,
                Question = "Quel pays n'est pas en Asie ?",
                Answer1 = "Chine",
                Answer2 = "Japon",
                Answer3 = "Corée",
                Answer4 = "Ukraine",
                Answer = 3,
                Letter ='C'
            },
            new Question3()
            {
                Id = 2,
                Question = "Quel pays est le plus peuple ?",
                Answer1 = "Inde",
                Answer2 = "Chine",
                Answer3 = "États-Unis",
                Answer4 = "Nigéria",
                Answer = 0,
                Letter ='D'
            },
            new Question4()
            {
                Id = 3,
                Question = "Quel pays a une feuille d'erable sur son drapeau ?",
                Answer1 = "Mexique",
                Answer2 = "Canada",
                Answer3 = "Italie",
                Answer4 = "Egypte",
                Answer = 1,
                Letter ='A'
            },
          
            new Question6()
            {
            Id = 3,
            Question = "Ou se trouve la foret Amazonienne ?",
            Answer1 = "Bresil",
            Answer2 = "Argentine",
            Answer3 = "Mexique",
            Answer4 = "Moldavie",
            Answer = 0,
            Letter ='B'
            },
            new Question7()
            {
                Id = 3,
                Question = "Sur quel continent se situe Madagascar ?",
                Answer1 = "Amerique du Nord",
                Answer2 = "Amerique du Sud",
                Answer3 = "Europe",
                Answer4 = "Afrique",
                Answer = 3,
                Letter ='E'
            },
            new Question8()
            {
            Id = 3,
            Question = "Quel pays est en Afrique ?",
            Answer1 = "France",
            Answer2 = "Senegal",
            Answer3 = "Belgique",
            Answer4 = "Inde",
            Answer = 1,
            Letter ='E'
        },
            
           
            new Question1()
            {
                Id = 1,
                Question = "Pour quelle equipe allez vous voter ?",
                Answer1 = "Les echecs",
                Answer2 = "Battle Pupus! P8",
                Answer3 = "Garfield",
                Answer4 = "Autre",
                Answer = 1,
                Letter ='B'
            },
            new Question1()
            {
                Id = 2,
                Question = "Dans quel pays se situe la grande muraille de chine ?",
                Answer1 = "Chine",
                Answer2 = "Cuba",
                Answer3 = "Egypte",
                Answer4 = "Afrique du Sud",
                Answer = 0,
                Letter ='D'
            },
            new Question4()
            {
                Id = 3,
                Question = "Dans quel pays se trouvent les grandes pyramides ?",
                Answer1 = "Soudan",
                Answer2 = "Egypte",
                Answer3 = "Mali",
                Answer4 = "Las Vegas",
                Answer = 1,
                Letter ='E'
            },
          
            new Question6()
            {
            Id = 3,
            Question = "Quel est le plus grand continent ?",
            Answer1 = "Asie",
            Answer2 = "Europe",
            Answer3 = "Amerique du Nord",
            Answer4 = "Afrique",
            Answer = 0,
            Letter ='D'
            },
            new Question7()
            {
                Id = 3,
                Question = "Dans quel continent se situe le desert du Sahara ?",
                Answer1 = "Asie",
                Answer2 = "Amerique du Sud",
                Answer3 = "Europe",
                Answer4 = "Afrique",
                Answer = 3,
                Letter ='E'
            },
            new Question8()
            {
            Id = 3,
            Question = "Ou se trouve le mont Everest ?",
            Answer1 = "France",
            Answer2 = "Nepal",
            Answer3 = "Pakistan",
            Answer4 = "Inde",
            Answer = 1,
            Letter ='D'
        },
            
            new Question8()
                        {
                        Id = 3,
                        Question = "Quel pays a pour capitale Rome ?",
                        Answer1 = "Autriche",
                        Answer2 = "Algerie",
                        Answer3 = "Grece",
                        Answer4 = "Italie",
                        Answer = 3,
                        Letter ='C'
                    },
            
            new Question8()
            {
                Id = 3,
                Question = "Quel pays est au Sud de l'équateur ?",
                Answer1 = "Espagne",
                Answer2 = "Mexique",
                Answer3 = "Argentine",
                Answer4 = "Kosovo",
                Answer = 2,
                Letter ='B'
            },
            
            new Question8()
            {
                Id = 3,
                Question = "Quel pays a comme animal national l'aigle a tête blanche ?",
                Answer1 = "Canada",
                Answer2 = "Etats-unis",
                Answer3 = "Croatie",
                Answer4 = "Albanie",
                Answer = 1,
                Letter ='A'
            },
        };

        public static List<IQuestion> HardQuestions = new List<IQuestion>()
        {
            new Question1()
            {
                Id = 0,
                Question = "Sur quel continent se situe la Guyane ?",
                Answer1 = "Amerique du Sud",
                Answer2 = "Afrique",
                Answer3 = "Europe",
                Answer4 = "Asie",
                Answer = 0,
                Letter ='B'
            },
            new Question2()
            {
                Id = 1,
                Question = "Quel pays n'est pas en Europe ?",
                Answer1 = "Estonie",
                Answer2 = "Moldavie",
                Answer3 = "Andorre",
                Answer4 = "Armenie",
                Answer = 3,
                Letter ='D'
            },
            
            new Question3()
            {
                Id = 2,
                Question = "Quel pays a comme capitale Pekin ?",
                Answer1 = "Russie",
                Answer2 = "Arabie Saoudite",
                Answer3 = "Chine",
                Answer4 = "Colombie",
                Answer = 2,
                Letter ='D'
            },
            
            new Question4 ()
            {
                Question = "De quel pays le Congo a t'il obtenu son independance en 1960 ?",
                Answer1 = "France",
                Answer2 = "Belgique",
                Answer3 = "Spain",
                Answer4= "Royaume-Uni",
                Answer = 1,
                Letter ='C'
            },
            
                        
            new Question5 ()
            {
                Question = "Quel pays est surnomme le pays du soleil-levant ?",
                Answer1 = "Chine",
                Answer2 = "Vietnam",
                Answer3 = "Australie",
                Answer4= "Japon",
                Answer = 3,
                Letter ='D'
            },
            
            new Question6 ()
            {
                Question = "Lequel de ces pays se situe en scandinavie ?",
                Answer1 = "Norvege",
                Answer2 = "Irlande",
                Answer3 = "Chili",
                Answer4= "Zimbabwe",
                Answer = 0,
                Letter ='C'
            },

            new Question7 ()
            {
                Question = "Dans quel pays se situe le plus grand lac d'eau douce au monde, le lac Superieur ?",
                Answer1 = "Chine",
                Answer2 = "Canada",
                Answer3 = "Etats-unis",
                Answer4 = "Cameroun",
                Answer = 1,
                Letter ='A'
            },

            new Question8 ()
            {
                Question = "Sur quel continent se trouve la Sierra Leone ?",
                Answer1 = "Asie",
                Answer2 = "Europe",
                Answer3 = "Afrique",
                Answer4= "Amerique du Sud",
                Answer = 2,
                Letter ='E'
            },
            
            new Question9 ()
            {
                Question = "Dans quel pays se trouve le mont Kilimanjaro ?",
                Answer1 = "Suisse",
                Answer2 = "Cambodge",
                Answer3 = "Nepal",
                Answer4= "Tanzanie",
                Answer = 3,
                Letter ='E'
            },
            
            new Question1 ()
            {
                Question = "Sur qul continent se trouve la Sierra Leone ?",
                Answer1 = "Asie",
                Answer2 = "Europe",
                Answer3 = "Afrique",
                Answer4= "Amerique du Sud",
                Answer = 2,
                Letter ='E'
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
        
        private void StartCountdown()
        {
            StartCoroutine(Wait10SecondsAndContinue());
            
        }
        IEnumerator Wait10SecondsAndContinue()
        {
            yield return new WaitForSeconds(20);
            nextButton.gameObject.SetActive(true);
            suivant = true;
            
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
            
            char letter = questionChosen.Letter;
            CommunicationManager.Instance.SendMessageToRobot(letter.ToString());

            suivant = true;
            nextButton.gameObject.SetActive(true);
        }

    }
}