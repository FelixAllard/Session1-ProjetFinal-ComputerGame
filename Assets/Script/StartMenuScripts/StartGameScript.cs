using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameScript : MonoBehaviour
{
    bool startGame = false;
    float timeSinceStart = 0.0f;
    [SerializeField]
    public Transform earth;
    public string QuestionScene = "QuestionScene";
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
        }
            
        
    }

    public void StartGame()
    {
        startGame = true;
    }
}
