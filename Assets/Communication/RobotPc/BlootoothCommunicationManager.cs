using System;
using System.IO.Ports;
using UnityEngine;

public class BluetoothManager : MonoBehaviour
{
    [SerializeField] private string comPort = "COM8"; // OUTGOING HC-05 port
    [SerializeField] private int baudRate = 9600;

    private SerialPort btPort;
    private string buffer = "";
    [NonSerialized]
    private float timeSend = 0;
    void Start() {
        btPort = new SerialPort(comPort, baudRate);
        btPort.ReadTimeout = 50;
        btPort.WriteTimeout = 50;

        try { btPort.Open(); Debug.Log("Connected to HC-05!"); }
        catch { Debug.LogError("Could not open COM port!"); }
    }

    void Update() {
        timeSend += Time.deltaTime;
        while (timeSend > 3)
        {
            SendMessageToArduino("Hello Arduino!");
            timeSend = 0;
        }
        
        

        // --- Read incoming data from Arduino ---
        if (btPort != null && btPort.IsOpen && btPort.BytesToRead > 0) {
            try {
                while (btPort.BytesToRead > 0) {
                    char c = (char)btPort.ReadChar();
                    buffer += c;

                    if (c == '\n') {
                        string msg = buffer.Trim();
                        buffer = "";
                        Debug.Log("From Arduino: " + msg);
                    }
                }
            } catch { }
        }
    }

    private void SendMessageToArduino(string message) {
        if (btPort != null && btPort.IsOpen) {
            btPort.WriteLine(message); // newline ensures Arduino can read full line
            Debug.Log("Sent to Arduino: " + message);
        }
    }

    private void OnApplicationQuit() {
        if (btPort != null && btPort.IsOpen) btPort.Close();
    }
}