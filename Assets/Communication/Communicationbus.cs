/*using UnityEngine;

namespace Communication
{
    using RJCP.IO.Ports;

    public class TestSerial : MonoBehaviour
    {
        SerialPortStream port;

        void Start() {
            port = new SerialPortStream("COM3", 9600);
            port.Open();
        }

        void Update() {
            if (port.IsOpen) {
                if (port.BytesToRead > 0) {
                    string line = port.ReadLine();
                    Debug.Log("RX: " + line);
                }
            }
        }

        void OnApplicationQuit() {
            if (port != null && port.IsOpen) port.Close();
        }
    }

}*/