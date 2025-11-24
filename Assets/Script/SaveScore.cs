using Questions;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveScore : MonoBehaviour
{
    public TMP_InputField nameInput;
    public Button saveButton;
    
    public void SavePlayerScore()
    {
        string playerName = nameInput.text;

        if (playerName == "")
            playerName = "Joueur";

        int score = QuestionManager.score;

      
        Leaderboard.AddScore(playerName, score);
        
 
        saveButton.interactable = false;

      
        nameInput.interactable = false;
      
        FindObjectOfType<Leaderboard>().DisplayLeaderboard();
    }

}