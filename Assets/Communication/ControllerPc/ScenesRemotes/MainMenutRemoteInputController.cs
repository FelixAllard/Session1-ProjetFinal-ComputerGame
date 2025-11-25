using UnityEngine;

namespace Communication.ControllerPc.ScenesRemotes
{
    public class MainMenutRemoteInputController : MonoBehaviour, IRemoteInputController{
        public StartGameScript startGame;
        public void OnButtonA()
        
        {
            startGame.StartGame();
        }

        public void OnButtonB()
        {
            startGame.easyToggle.isOn = !startGame.easyToggle.isOn;
            startGame.hardToggle.isOn = !startGame.hardToggle.isOn;
        }

        public void OnButtonC()
        {

        }

        public void OnButtonD()
        {

        }

        public void ReceivedMessageFromRobot(string message)
        {
            
        }
    }
}