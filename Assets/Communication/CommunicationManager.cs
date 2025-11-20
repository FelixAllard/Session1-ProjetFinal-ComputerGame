using System;
using Communication.ControllerPc;
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

        switch (msg)
        {

            case "BUTTON_A": currentHandler.OnButtonA(); break;
            case "BUTTON_B": currentHandler.OnButtonB(); break;
            case "BUTTON_C": currentHandler.OnButtonC(); break;
            case "BUTTON_D": currentHandler.OnButtonD(); break;
            default:
                Debug.LogError("Received Unidentified Message: " + msg);
                break;
        }
    }
}

internal interface IRemoteInputHandler
{
}