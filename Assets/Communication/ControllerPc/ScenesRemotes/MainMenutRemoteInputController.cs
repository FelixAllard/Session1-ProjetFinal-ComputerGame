using UnityEngine;

namespace Communication.ControllerPc.ScenesRemotes
{
    public class MainMenutRemoteInputController : MonoBehaviour, IRemoteInputController
    {
        public StartGameScript startGame;
        public void OnButtonA()
        {
            startGame.StartGame();
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