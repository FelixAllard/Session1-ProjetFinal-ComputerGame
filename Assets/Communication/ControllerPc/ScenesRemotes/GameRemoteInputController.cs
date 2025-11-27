using System;
using System.Collections;
using Questions;
using UnityEngine;


namespace Communication.ControllerPc.ScenesRemotes
{
    public class GameRemoteInputController: MonoBehaviour, IRemoteInputController
    {
        [NonSerialized]
     
        
        private bool buttonClicked = true;
        public QuestionManager questionManager;
        
        private void SendQuestionLetter()
        {
            char letter = questionManager.questionChosen.Letter;
            CommunicationManager.Instance.SendMessageToRobot(letter.ToString());
        }
        
        
        public void OnButtonA()
        {
                questionManager.AnswerSelected(0);
                
                if (QuestionManager.suivant == true)
                {
                    questionManager.Next();
                }
                
        }

        public void OnButtonB()
        {
                 questionManager.AnswerSelected(1);
                 if (QuestionManager.suivant == true)
                 {
                     questionManager.Next();
                 }
           
        }

        public void OnButtonC()
        {
                questionManager.AnswerSelected(2);
                if (QuestionManager.suivant == true)
                {
                    questionManager.Next();
                }
            
        }

        public void OnButtonD()
        {
            
                questionManager.AnswerSelected(3);
                if (QuestionManager.suivant == true)
                {
                    questionManager.Next();
                }
        }

        

        public void ReceivedMessageFromRobot(string message)
        {
            CommunicationManager.Instance.SendMessageToRobot("Neck hurt");
        }
    }
}