using System;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class ControllerPcManager : MonoBehaviour
{
    public static ControllerPcManager Instance;

    private SerialPort port;
    private Thread readThread;
    private bool running = false;

    public string targetMessage = "ARDUINO_SERIAL_MEGACONNECTION_CONTROLLER_INITILIAZER_20050412_RADAR";
    public int baudRate = 115200;

    private float lastHeartbeatTime = 0f;
    private bool connected = false;

    public Action OnDisconnected;
    public Action<string> OnMessage;

    public CommunicationManager communicationManager;
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        running = true;
        ControllerPcManager.Instance.OnMessage += msg => {
            communicationManager.ReceivedMessageFromRemote(msg);
            Debug.Log("Received from Arduino (non-heartbeat): " + msg);
        };
        
        new Thread(PortScanLoop).Start();
    }

    // ============================================================
    //  AUTO SCAN FOR ARDUINO
    // ============================================================
    void PortScanLoop()
    {
        while (running)
        {
            if (!connected)
            {
                foreach (string com in SerialPort.GetPortNames())
                {
                    TryOpenPort(com);
                    if (connected) break;
                }
            }

            Thread.Sleep(500);
        }
    }

    bool TryOpenPort(string com)
    {
        try
        {
            port = new SerialPort(com, baudRate)
            {
                NewLine = "\n",
                ReadTimeout = 50,
                DtrEnable = false,
                RtsEnable = false
            };

            port.Open();
            Debug.Log("Opened " + com);

            readThread = new Thread(ReadLoop);
            readThread.Start();

            return true;
        }
        catch
        {
            port = null;
            return false;
        }
    }

    // ============================================================
    //  READ LOOP
    // ============================================================
    void ReadLoop()
    {
        string buffer = "";

        while (running && port != null && port.IsOpen)
        {
            try
            {
                string incoming = port.ReadExisting();
                if (incoming.Length > 0)
                {
                    buffer += incoming;

                    while (buffer.Contains("\n"))
                    {
                        int idx = buffer.IndexOf("\n");
                        string line = buffer.Substring(0, idx).Trim();
                        buffer = buffer.Substring(idx + 1);

                        HandleLine(line);
                    }
                }
            }
            catch { }

            Thread.Sleep(5);
        }
    }

    // ============================================================
    //  PROCESS EACH LINE
    // ============================================================
    void HandleLine(string line)
    {
        Debug.Log("[SERIAL] " + line);

        // handshake message
        if (!connected && line == targetMessage)
        {
            port.WriteLine("CONNECTED");
            connected = true;
            lastHeartbeatTime = Time.time;
            Debug.Log(">>> HANDSHAKE COMPLETE <<<");
            return;
        }

        // heartbeat
        if (line == "ALIVE")
        {
            lastHeartbeatTime = Time.time;
            return;
        }

        // non-heartbeat → user logic
        OnMessage?.Invoke(line);
    }

    // ============================================================
    //  HEARTBEAT WATCHDOG
    // ============================================================
    void Update()
    {
        if (connected)
        {
            if (Time.time - lastHeartbeatTime > 3f)
            {
                Debug.LogWarning("Heartbeat lost! Reconnecting…");
                connected = false;
                OnDisconnected?.Invoke();

                if (port != null)
                {
                    try { port.Close(); } catch { }
                }
            }
        }
    }

    // ============================================================
    //  SEND
    // ============================================================
    public void SendMessage(string msg)
    {
        if (connected && port != null && port.IsOpen)
            port.WriteLine(msg);
    }

    void OnApplicationQuit()
    {
        running = false;
        try { port?.Close(); } catch { }
    }
}
