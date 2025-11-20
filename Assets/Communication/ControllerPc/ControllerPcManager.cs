using System;
using UnityEngine;

using System.Threading;
using RJCP.IO.Ports;

public class ControllerPcManager : MonoBehaviour
{
    public static ControllerPcManager Instance;

    private SerialPortStream port;
    private Thread scanThread;
    private bool running = false;

    [Header("Serial Settings")]
    public string targetMessage = "ARDUINO_SERIAL_MEGACONNECTION_CONTROLLER_INITILIAZER_20050412_RADAR";
    public int baudRate = 115200;

    [Header("Connection State")]
    public bool portLocked = false;
    public string lastMessage = "";

    // Event to call when connection is lost
    public Action OnConnectionLost;

    // Event to process every received message
    public Func<string, object> DecodeMessage;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        running = true;
        scanThread = new Thread(ScanCom9);
        scanThread.Start();
    }

    private void ScanCom9()
    {
        string portName = "COM9";
        Debug.Log("Starting COM9 scan...");

        while (running && !portLocked)
        {
            try
            {
                // Only open port if not already open
                if (port == null)
                {
                    port = new SerialPortStream(portName, baudRate);
                    port.DtrEnable = true;
                    port.RtsEnable = true;
                    port.NewLine = "\n"; // optional but recommended
                    port.ReadTimeout = 50;
                    port.Open();
                    Debug.Log("Opened COM9 successfully");
                    Thread.Sleep(2000); // give Arduino time to reboot
                    new Thread(ReadCom9Loop).Start();
                }

            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to open COM9: {ex.Message}");
                port?.Close();
                port = null;
                Thread.Sleep(500); // retry delay
            }

            Thread.Sleep(500); // loop delay to avoid spamming
        }

        Debug.Log("ScanCom9 exiting (portLocked=" + portLocked + ")");
    }

    private void ReadCom9Loop()
    {
        string buffer = "";

        while (running && port != null && port.IsOpen)
        {
            try {
                string data = port.ReadExisting();
                if (!string.IsNullOrEmpty(data)) {
                    buffer += data;

                    int lastNL = Math.Max(buffer.LastIndexOf('\n'), buffer.LastIndexOf('\r'));
                    if (lastNL >= 0) {
                        string complete = buffer.Substring(0, lastNL);
                        buffer = buffer.Substring(lastNL + 1);

                        string[] lines = complete.Split(new[] { "\r\n", "\n", "\r" }, 
                            StringSplitOptions.RemoveEmptyEntries);

                        foreach (string line in lines) {
                            string trimmed = line.Trim();
                            Debug.Log("[COM9 READ] " + trimmed);

                            if (!portLocked && trimmed == targetMessage) {
                                portLocked = true;
                                port.WriteLine("CONNECTED");
                            }

                            DecodeMessage?.Invoke(trimmed);
                        }
                    }
                }
            }
            catch (Exception ex) {
                Debug.LogError("Serial fatal error: " + ex);
                HandleConnectionLost();
                break;
            }

            Thread.Sleep(5);
        }
    }


    private void HandleConnectionLost()
    {
        Debug.LogWarning("Serial connection lost!");
        portLocked = false;
        lastMessage = "";
        port?.Close();
        port = null;

        OnConnectionLost?.Invoke();
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

        if (scanThread != null && scanThread.IsAlive)
            scanThread.Join();

        if (port != null && port.IsOpen)
            port.Close();
    }

    public void SendMessage(string message)
    {
        if (portLocked && port != null && port.IsOpen)
        {
            port.WriteLine(message);
        }
    }
}
