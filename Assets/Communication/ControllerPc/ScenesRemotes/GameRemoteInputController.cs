using Questions;
using UnityEngine;

namespace Communication.ControllerPc.ScenesRemotes
{
    public class GameRemoteInputController: MonoBehaviour, IRemoteInputController
    {
        public QuestionManager questionManager;
        
        public void OnButtonA()
        {
            questionManager.AnswerSelected(0);
        }

        public void OnButtonB()
        {
            
            questionManager.AnswerSelected(1);
        }

        public void OnButtonC()
        {
            
            questionManager.AnswerSelected(2);
        }

        public void OnButtonD()
        {
           
            questionManager.AnswerSelected(3);
        }
    }
}