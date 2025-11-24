using Questions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonScript : MonoBehaviour
{
    public TextMeshProUGUI scoreTMP;

    private void Start()
    {
        scoreTMP.text = "Votre score est : " + QuestionManager.score.ToString();
    }
    public void QuitGame()
    {
        
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
    #else
           Application.Quit();
    #endif
    }

    public void Restart()
    {
        QuestionManager.score = 0; 
        SceneManager.LoadScene(0);
    }
}
