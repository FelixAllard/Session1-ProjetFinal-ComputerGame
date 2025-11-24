using UnityEngine;

namespace Communication.ControllerPc.ScenesRemotes
{
    public class EndingSceneInputController : MonoBehaviour, IRemoteInputController
    {
        public buttonScript buttonScript;
        public void OnButtonA()
        {
            buttonScript.Restart();
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