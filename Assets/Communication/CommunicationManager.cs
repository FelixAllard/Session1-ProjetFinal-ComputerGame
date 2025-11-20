using System;
using UnityEngine;

public class CommunicationManager : MonoBehaviour
{
    static CommunicationManager Instance;
    private void Awake()
    {
        if(Instance!=null)
            Destroy(gameObject);
        Instance = this;
        DontDestroyOnLoad(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceivedMessageFromRemote(string msg)
    {
        if (msg == "BUTTON_A")
        {
            Received_A_ButtonPress();
        }
        else if (msg == "BUTTON_B"){
            Received_B_ButtonPress();
        }
        else if (msg == "BUTTON_C")
        {
            Received_C_ButtonPress();
        }
        else if (msg == "BUTTON_D")
        {
            Received_D_ButtonPress();
        }
        else
        {
            Debug.LogError("Received Unidentified Message: " + msg);
        }
    }

    public void Received_A_ButtonPress()
    {
        
    }

    public void Received_B_ButtonPress()
    {
        
    }

    public void Received_C_ButtonPress()
    {
        
    }

    public void Received_D_ButtonPress()
    {
        
    }
}
