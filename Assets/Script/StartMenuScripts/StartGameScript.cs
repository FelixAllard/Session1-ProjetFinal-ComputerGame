using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameScript : MonoBehaviour
{
    bool startGame = false;
    float timeSinceStart = 0.0f;
    [SerializeField]
    public Transform earth;

    public Toggle easyToggle;
    public Toggle hardToggle;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (earth == null)
            return; // sécurité

        if (!startGame)
        {
            earth.Rotate(new Vector3(0,0, -0.1f));
            return;
        }

        timeSinceStart += Time.deltaTime;
        earth.Rotate(new Vector3(0,0, -0.3f));
        earth.localScale += new Vector3(0.3f, 0.3f, 0.3f);

        if (timeSinceStart >= 2f)
        {
            SceneManager.LoadScene(1);
        }
    }


    public void StartGame()
    {
        if (easyToggle.isOn)
        {
            GameSettings.Instance.easyMode = true;
        }
        else if(hardToggle.isOn)
        {
            GameSettings.Instance.easyMode = false;
        }
        startGame = true;
    }
    public void QuitGame()
    {
        
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
    #else
           Application.Quit();
    #endif
    }
    
    

}
