using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject[] Levels;

    int currentLevelIndex;

    public void correctAnswer()
    {
        if(currentLevelIndex +1 != Levels.Length)
        {
            Levels[currentLevelIndex].SetActive(false);
            currentLevelIndex++;
            Levels[currentLevelIndex].SetActive(true);
        }
    }
}
