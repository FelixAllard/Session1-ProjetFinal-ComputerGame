using Script.Utilities;

namespace Communication.RobotPc
{
    using System;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using UnityEngine;

    public class ESP_RobotConnection : MonoBehaviour
    {
        
        public static ESP_RobotConnection Instance;
        
        [Header("ESP8266 Settings")]
        public string ip = "192.168.4.1";   // Replace with your ESP AP IP
        public int port = 23;               // Must match the ESP server port
        
        private TcpClient client;
        private NetworkStream stream;
        private Thread receiveThread;
        private bool running = false;

        public Action<string> OnMessageReceived;
        
        public CommunicationManager CommunicationManager;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        void Start()
        {
            Connect();
            Send("Connected");
            
        }

        void OnApplicationQuit()
        {
            Disconnect();
        }

        // -----------------------------
        // CONNECT
        // -----------------------------
        public void Connect()
        {
            try
            {
                client = new TcpClient();
                client.Connect(ip, port);

                stream = client.GetStream();
                running = true;

                receiveThread = new Thread(ReceiveLoop);
                receiveThread.Start();
                OnMessageReceived += CommunicationManager.Instance.ReceivedMessageFromRobot;

                Debug.Log("Connected to ESP8266!");
            }
            catch (Exception ex)
            {
                Debug.LogError("TCP Connect Failed: " + ex.Message);
            }
        }

        // -----------------------------
        // DISCONNECT
        // -----------------------------
        public void Disconnect()
        {
            OnMessageReceived -= CommunicationManager.Instance.ReceivedMessageFromRobot;
            running = false;

            if (receiveThread != null && receiveThread.IsAlive)
                receiveThread.Abort();

            if (stream != null) stream.Close();
            if (client != null) client.Close();
        }

        // -----------------------------
        // SEND
        // -----------------------------
        public void Send(string msg)
        {
            if (client == null || !client.Connected) return;

            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(msg);
                stream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                Debug.LogError("TCP Send Error: " + ex.Message);
            }
        }

        // -----------------------------
        // RECEIVE LOOP (Background)
        // -----------------------------
        private void ReceiveLoop()
        {
            byte[] buffer = new byte[1024];

            while (running)
            {
                try
                {
                    if (stream == null || !stream.DataAvailable)
                    {
                        Thread.Sleep(5);
                        continue;
                    }

                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        string msg = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        UnityMainThread(msg);
                    }
                }
                catch
                {
                    // Ignore temp disconnect errors
                }
            }
        }

        // -----------------------------
        // Invoke callback on Unity thread
        // -----------------------------
        private void UnityMainThread(string msg)
        {
            // Unity thread dispatching
            UnityMainThreadDispatcher.Enqueue(() =>
            {
                OnMessageReceived?.Invoke(msg);
            });
        }
    }

}