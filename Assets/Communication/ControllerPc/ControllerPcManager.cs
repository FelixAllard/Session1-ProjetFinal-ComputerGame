/*
using UnityEngine;
using RJCP.IO.Ports;
using System.Threading;

using UnityEngine;
using RJCP.IO.Ports;
using System.Threading;
using System.Collections.Generic;

public class ControllerPcManager : MonoBehaviour
{
    private SerialPortStream port;
    private Thread readThread;
    private bool running = false;

    public string lastMessage = "";
    public string targetMessage = "ARDUINO_SERIAL_MEGACONNECTION_CONTROLLER_INITILIAZER_20050412_RADAR";  // Message to look for
    private bool portLocked = false;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        running = true;

        // Start scanning ports in background thread
        readThread = new Thread(ScanPorts);
        readThread.Start();
    }

    private void ScanPorts()
    {
        while (running && !portLocked)
        {
            string[] portNames = SerialPortStream.GetPortNames();

            foreach (string portName in portNames)
            {
                if (!running || portLocked) break;

                SerialPortStream testPort = new SerialPortStream(portName, 9600);
                testPort.ReadTimeout = 500;

                try
                {
                    testPort.Open();
                    Debug.Log("Trying port: " + portName);

                    while (!portLocked && running)
                    {
                        if (testPort.BytesToRead > 0)
                        {
                            string line = testPort.ReadLine();

                            if (line == targetMessage)
                            {
                                Debug.Log("Target message received on port: " + portName);
                                port = testPort;  // Lock this port
                                portLocked = true;
                                lastMessage = line;
                                break;
                            }
                        }
                        Thread.Sleep(10);
                    }
                }
                catch
                {
                    // Ignore ports that can't be opened
                    testPort.Close();
                }

                if (!portLocked)
                {
                    testPort.Close();
                }
            }

            Thread.Sleep(500); // wait before scanning again
        }

        // Start reading from locked port
        if (portLocked && port != null && port.IsOpen)
        {
            Debug.Log("Locked to port: " + port.PortName);
            ReadLockedPort();
        }
    }

    private void ReadLockedPort()
    {
        while (running && portLocked)
        {
            try
            {
                if (port.BytesToRead > 0)
                {
                    string line = port.ReadLine();
                    lastMessage = line;
                }
            }
            catch
            {
                // Ignore read timeouts
            }
            Thread.Sleep(10);
        }
    }

    void Update()
    {
        if (!string.IsNullOrEmpty(lastMessage))
        {
            Debug.Log("Serial: " + lastMessage);
            lastMessage = "";
        }
    }

    void OnApplicationQuit()
    {
        running = false;

        if (readThread != null && readThread.IsAlive)
            readThread.Join();

        if (port != null && port.IsOpen)
            port.Close();
    }
}
*/
