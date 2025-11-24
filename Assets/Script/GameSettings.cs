using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;

    public bool easyMode = true;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}