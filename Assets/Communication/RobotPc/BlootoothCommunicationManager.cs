using System;
using System.IO.Ports;
using UnityEngine;

public class BluetoothCommunicationManager : MonoBehaviour
{
    [Header("Serial Settings")]
    [SerializeField] private string comPort = "COM8"; // OUTGOING HC-05 port
    [SerializeField] private int baudRate = 9600;
    [SerializeField] private float sendInterval = 3f;  // seconds between messages

    private SerialPort btPort;
    private float timeSinceLastSend = 0f;

    void Start()
    {
        try
        {
            btPort = new SerialPort(comPort, baudRate)
            {
                ReadTimeout = 50,
                WriteTimeout = 50,
                NewLine = "\n"  // Ensure ReadLine() works correctly
            };
            btPort.Open();
            Debug.Log("Connected to HC-05 on port " + comPort + "!");
        }
        catch (Exception e)
        {
            Debug.LogError("Could not open COM port: " + e.Message);
        }
    }

    void Update()
    {
        if (btPort == null || !btPort.IsOpen)
            return;

        // --- Periodic message sending ---
        timeSinceLastSend += Time.deltaTime;
        if (timeSinceLastSend >= sendInterval)
        {
            SendMessageToArduino("Hello Arduino!");
            timeSinceLastSend = 0f;
        }

        // --- Read incoming data ---
        try
        {
            while (btPort.BytesToRead > 0)
            {
                string message = btPort.ReadLine().Trim();
                if (!string.IsNullOrEmpty(message))
                {
                    Debug.Log("From Arduino: " + message);
                    // Optional: handle commands from Arduino here
                }
            }
        }
        catch (TimeoutException)
        {
            // Normal, no data available, safe to ignore
        }
        catch (Exception e)
        {
            Debug.LogError("Error reading from HC-05: " + e.Message);
        }
    }

    private void SendMessageToArduino(string message)
    {
        if (btPort != null && btPort.IsOpen)
        {
            try
            {
                btPort.WriteLine(message);
                Debug.Log("Sent to Arduino: " + message);
            }
            catch (Exception e)
            {
                Debug.LogError("Error sending to Arduino: " + e.Message);
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (btPort != null)
        {
            try
            {
                if (btPort.IsOpen) btPort.Close();
                btPort.Dispose();
            }
            catch (Exception e)
            {
                Debug.LogError("Error closing COM port: " + e.Message);
            }
        }
    }
}
