using System;
using Questions;
using UnityEngine;

namespace Communication.ControllerPc.ScenesRemotes
{
    public class GameRemoteInputController: MonoBehaviour, IRemoteInputController
    {
        private bool buttonClicked = false;
        public QuestionManager questionManager;
        

        public void OnButtonA()
        {
            if (buttonClicked)
            {
                questionManager.Next();
                buttonClicked = false;
            }
            questionManager.AnswerSelected(0);
            buttonClicked = true;
        }

        public void OnButtonB()
        {
            if (buttonClicked)
            {
                questionManager.Next();
                buttonClicked = false;
            }
            questionManager.AnswerSelected(1);
            buttonClicked = true;
        }

        public void OnButtonC()
        {
            if (buttonClicked)
            {
                questionManager.Next();
                buttonClicked = false;
            }
            questionManager.AnswerSelected(2);
            buttonClicked = true;
        }

        public void OnButtonD()
        {
            if (buttonClicked)
            {
                questionManager.Next();
                buttonClicked = false;
            }
            questionManager.AnswerSelected(3);
            buttonClicked = true;
        }
    }
}