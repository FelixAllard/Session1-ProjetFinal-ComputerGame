using System;
using UnityEngine;

namespace Communication.ControllerPc
{
    public class SendMessageToRobot
    {
        public CommunicationManager communicationManager;

        public SendMessageToRobot(string msg)
        {
            if(communicationManager ==null)
                communicationManager = CommunicationManager.Instance;
            try
            {
                communicationManager.SendMessageToRobot(msg);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}