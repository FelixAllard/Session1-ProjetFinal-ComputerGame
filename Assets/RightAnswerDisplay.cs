using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RightAnswerDisplay : MonoBehaviour
{
    public static int correctAnswerIndex;

    public Button[] answerButtons; // Assignés dans l’Inspector

    private void Start()
    {
        // 1. Assigner les textes aux boutons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text =
                Questions.QuestionManager.lastAnswers[i];
        }

        // 2. Colorer les boutons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            var img = answerButtons[i].GetComponent<Image>();

            if (i == correctAnswerIndex)
                img.color = Color.green;
            else
                img.color = Color.red;
        }
    }

    public void Next()
    {
        SceneManager.LoadScene(1);
    }
}