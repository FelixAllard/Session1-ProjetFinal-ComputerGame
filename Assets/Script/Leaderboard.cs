using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    // Les champs dans ta scène Leaderboard
    [SerializeField] private List<TextMeshProUGUI> names;
    [SerializeField] private List<TextMeshProUGUI> scores;

    private const int MaxEntries = 5; // TOP 5
    
    void Start()
    {
        DisplayLeaderboard();
    }

   
    public static void AddScore(string playerName, int score)
    {
      
        List<Entry> entries = LoadEntries();

        
        entries.Add(new Entry(playerName, score));

      
        entries.Sort((a, b) => b.score.CompareTo(a.score));

     
        if (entries.Count > MaxEntries)
            entries.RemoveRange(MaxEntries, entries.Count - MaxEntries);

      
        SaveEntries(entries);
    }

    public void DisplayLeaderboard()
    {
        List<Entry> entries = LoadEntries();

        for (int i = 0; i < names.Count; i++)
        {
            if (i < entries.Count)
            {
                names[i].text = entries[i].name;
                scores[i].text = entries[i].score.ToString();
            }
            else
            {
                names[i].text = "-";
                scores[i].text = "0";
            }
        }
    }


   
    private static void SaveEntries(List<Entry> entries)
    {
        // Clear old entries
        int oldCount = PlayerPrefs.GetInt("LB_Count", 0);
        for (int i = 0; i < oldCount; i++)
        {
            PlayerPrefs.DeleteKey("LB_Name_" + i);
            PlayerPrefs.DeleteKey("LB_Score_" + i);
        }

        // Save new entries
        for (int i = 0; i < entries.Count; i++)
        {
            PlayerPrefs.SetString("LB_Name_" + i, entries[i].name);
            PlayerPrefs.SetInt("LB_Score_" + i, entries[i].score);
        }

        PlayerPrefs.SetInt("LB_Count", entries.Count);
        PlayerPrefs.Save();
    }


    
    private static List<Entry> LoadEntries()
    {
        List<Entry> list = new List<Entry>();

        int count = PlayerPrefs.GetInt("LB_Count", 0);

        for (int i = 0; i < count; i++)
        {
            string name = PlayerPrefs.GetString("LB_Name_" + i, "-");
            int score = PlayerPrefs.GetInt("LB_Score_" + i, 0);

            list.Add(new Entry(name, score));
        }

        return list;
    }

   
    private class Entry
    {
        public string name;
        public int score;

        public Entry(string n, int s)
        {
            name = n;
            score = s;
        }
    }
    
    public void ResetLeaderboard()
    {
        PlayerPrefs.DeleteKey("LB_Count");

        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.DeleteKey("LB_Name_" + i);
            PlayerPrefs.DeleteKey("LB_Score_" + i);
        }

        PlayerPrefs.Save();

        
        DisplayLeaderboard();
    }

    
}
