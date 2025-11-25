using System;
using System.Net.Configuration;
using Communication.ControllerPc;
using Communication.RobotPc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CommunicationManager : MonoBehaviour
{
    public static CommunicationManager Instance;

    private IRemoteInputController currentHandler;
    

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentHandler = FindFirstHandlerInScene();

        if (currentHandler != null)
            Debug.Log("Remote input handler found in scene: " + currentHandler.GetType().Name);
        else
            Debug.LogWarning("No remote input handler present in scene.");
    }
    private IRemoteInputController FindFirstHandlerInScene()
    {
        foreach (var mb in FindObjectsOfType<MonoBehaviour>())
        {
            if (mb is IRemoteInputController handler)
                return handler;
        }
        return null;
    }


    public void ReceivedMessageFromRemote(string msg)
    {
        if (currentHandler == null)
        {
            Debug.LogWarning("Message received but no handler found: " + msg);
            return;
        }
        Debug.Log("Button Pressed : "+ msg);

        if(msg.Contains("BUTTON_A"))
            currentHandler.OnButtonA();
        else if(msg.Contains("BUTTON_B"))
            currentHandler.OnButtonB();
        else if(msg.Contains("BUTTON_C"))
            currentHandler.OnButtonC();
        else if(msg.Contains("BUTTON_D"))
            currentHandler.OnButtonD();
        else
        {
            Debug.LogError("Received Unidentified Message: " + msg);
        }
    }

    public void SendMessageToRobot(string message)
    {
        if (message == "" || message == null)
            throw new ArgumentNullException("Why is your string empty? You think I can send air through wifi???");
        
        ESP_RobotConnection.Instance.Send(message);
    }
    public void ReceivedMessageFromRobot(string message)
    {
        
    }
}