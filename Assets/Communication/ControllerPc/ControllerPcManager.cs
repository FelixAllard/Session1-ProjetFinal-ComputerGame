using System;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerPcManager : MonoBehaviour
{
    public static ControllerPcManager Instance;

    [Header("Serial Settings")]
    public string comPort = "COM14"; // <-- set your port here
    public int baudRate = 115200;
    public string targetMessage = "ARDUINO_SERIAL_MEGACONNECTION";

    private SerialPort port;
    private Thread readThread;
    private bool running = false;
    private bool connected = false;
    private float lastHeartbeatTime = 0f;

    // Queue for thread-safe message handling
    private readonly Queue<string> messageQueue = new Queue<string>();
    private readonly object queueLock = new object();

    public Action OnDisconnected;
    public Action<string> OnMessage;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        running = true;
        Debug.Log("[DEBUG] Opening known COM port: " + comPort);
        TryOpenPort(comPort);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Optional: refresh any scene-specific references
        Debug.Log("[DEBUG] Scene loaded: " + scene.name);
    }

    private bool TryOpenPort(string com)
    {
        try
        {
            port = new SerialPort(com, baudRate)
            {
                ReadTimeout = -1,
                DtrEnable = false,
                RtsEnable = false
            };
            port.Open();
            Debug.Log("[DEBUG] Port opened: " + com);

            readThread = new Thread(ReadLoop);
            readThread.IsBackground = true;
            readThread.Start();

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("[DEBUG] Failed to open port " + com + ": " + e.Message);
            port = null;
            return false;
        }
    }

    private void ReadLoop()
    {
        string buffer = "";
        Debug.Log("[DEBUG] Starting read loop...");

        while (running && port != null && port.IsOpen)
        {
            try
            {
                string data = port.ReadExisting();
                if (!string.IsNullOrEmpty(data))
                {
                    Debug.Log("[DEBUG] Raw incoming data: " + data.Replace("\r", "\\r").Replace("\n", "\\n"));
                    buffer += data;

                    string[] lines = buffer.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
                    buffer = lines[lines.Length - 1]; // keep last partial line

                    for (int i = 0; i < lines.Length - 1; i++)
                    {
                        string line = lines[i].Trim();
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        Debug.Log("[DEBUG] Complete line received: '" + line + "'");

                        // Handshake detection
                        if (!connected && line.Contains(targetMessage))
                        {
                            port.WriteLine("CONNECTED");
                            connected = true;
                            lastHeartbeatTime = Time.time;
                            Debug.Log(">>> HANDSHAKE COMPLETE <<< (line: '" + line + "')");
                            continue;
                        }

                        // Heartbeat
                        if (line == "ALIVE")
                        {
                            lastHeartbeatTime = Time.time;
                            Debug.Log("[DEBUG] Heartbeat received");
                            continue;
                        }

                        // Queue button messages to main thread
                        if (line.Contains("BUTTON_"))
                        {
                            lock (queueLock)
                            {
                                messageQueue.Enqueue(line);
                            }
                            Debug.Log("[DEBUG] Queued button message: " + line);
                            continue;
                        }

                        Debug.Log("[DEBUG] Ignored unknown line: " + line);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("[DEBUG] Serial read error: " + e.Message);
            }

            Thread.Sleep(5);
        }

        Debug.Log("[DEBUG] Exiting read loop...");
    }

    void Update()
    {
        // Process queued messages on main thread
        lock (queueLock)
        {
            while (messageQueue.Count > 0)
            {
                string msg = messageQueue.Dequeue();
                CommunicationManager cm = CommunicationManager.Instance;
                if (cm != null)
                {
                    cm.ReceivedMessageFromRemote(msg);
                    Debug.Log("[DEBUG] Processed BUTTON on main thread: " + msg);
                }
            }
        }

        // Heartbeat watchdog
        if (connected && Time.time - lastHeartbeatTime > 3f)
        {
            Debug.LogWarning("[DEBUG] Heartbeat lost! Reconnecting…");
            connected = false;
            OnDisconnected?.Invoke();

            if (port != null)
            {
                try
                {
                    port.Close();
                    Debug.Log("[DEBUG] Port closed due to heartbeat loss");
                }
                catch (Exception e)
                {
                    Debug.LogError("[DEBUG] Error closing port: " + e.Message);
                }
            }
        }
    }

    public void SendMessage(string msg)
    {
        if (connected && port != null && port.IsOpen)
        {
            Debug.Log("[DEBUG] Sending message: " + msg);
            port.WriteLine(msg);
        }
        else
        {
            Debug.Log("[DEBUG] Cannot send message, not connected");
        }
    }

    void OnApplicationQuit()
    {
        running = false;
        try
        {
            port?.Close();
            Debug.Log("[DEBUG] Port closed on application quit");
        }
        catch (Exception e)
        {
            Debug.LogError("[DEBUG] Error closing port on quit: " + e.Message);
        }
    }
}
