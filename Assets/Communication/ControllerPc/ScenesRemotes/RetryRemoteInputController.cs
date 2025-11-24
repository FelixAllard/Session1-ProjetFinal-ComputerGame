using UnityEngine;

namespace Communication.ControllerPc.ScenesRemotes
{
    public class RetryRemoteInputController: MonoBehaviour, IRemoteInputController
    {
        RightAnswerDisplay rightAnswerDisplay;
        public void OnButtonA()
        {
            rightAnswerDisplay.Next();
        }

        public void OnButtonB()
        {
            throw new System.NotImplementedException();
        }

        public void OnButtonC()
        {
            throw new System.NotImplementedException();
        }

        public void OnButtonD()
        {
            throw new System.NotImplementedException();
        }
    }
}