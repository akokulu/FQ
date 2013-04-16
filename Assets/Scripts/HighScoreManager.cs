using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
 
/// <summary>
/// High score manager.
/// Local highScore manager for LeaderboardLength number of entries
/// 
/// this is a singleton class.  to access these functions, use HighScoreManager._instance object.
/// eg: HighScoreManager._instance.SaveHighScore("meh",1232);
/// No need to attach this to any game object, thought it would create errors attaching.
/// </summary>
 
public class HighScoreManager : MonoBehaviour
{
    private static HighScoreManager m_instance;
    private const int LeaderboardLength = 10;
 
    public static HighScoreManager _instance 
    {
        get 
        {
            if (m_instance == null)
                m_instance = new GameObject ("HighScoreManager").AddComponent<HighScoreManager> ();          

            return m_instance;
        }
    }
 
    void Awake ()
    {
        if (m_instance == null)
            m_instance = this;      
        else if (m_instance != this)       
            Destroy (gameObject);    
 
        DontDestroyOnLoad (gameObject);
    }

    public void SaveHighScore(string name, int killCount, int lampsCollected)
    {
        List<Scores> HighScores = new List<Scores> ();

        int i = 1;
        while (i <= LeaderboardLength && PlayerPrefs.HasKey("HighScore" + i + "killCount"))
        {
            Scores temp = new Scores ();
            temp.lampsCollected = PlayerPrefs.GetInt("HighScore" + i + "lampsCollected");
            temp.killCount = PlayerPrefs.GetInt("HighScore" + i + "killCount");
            temp.name = PlayerPrefs.GetString ("HighScore" + i + "name");
            HighScores.Add (temp);
            i++;
        }

        if (HighScores.Count == 0) 
        {
            Scores _temp = new Scores();
            _temp.name = name;
            _temp.killCount = killCount;
            _temp.lampsCollected = lampsCollected;
            HighScores.Add (_temp);
        } 
        else 
        {
            for (i = 1; i <= HighScores.Count && i <= LeaderboardLength; i++) 
            {
                if (lampsCollected > HighScores[i - 1].lampsCollected)
                {
                    Scores _temp = new Scores();
                    _temp.name = name;
                    _temp.killCount = killCount;
                    _temp.lampsCollected = lampsCollected;
                    HighScores.Insert(i - 1, _temp);
                    break;
                }
                else if (lampsCollected == HighScores[i - 1].lampsCollected)
                {
                    if (killCount > HighScores[i - 1].killCount) 
                    {
                        Scores _temp = new Scores ();
                        _temp.name = name;
                        _temp.killCount = killCount;
                        _temp.lampsCollected = lampsCollected;
                        HighScores.Insert (i - 1, _temp);
                        break;
                    }
                    else if (killCount == HighScores[i - 1].killCount) 
                    {
                        if (string.Compare(name, HighScores[i - 1].name) <= 0)  // If name is alphabetically before
                        {
                               Scores _temp = new Scores ();
                            _temp.name = name;
                            _temp.killCount = killCount;
                            _temp.lampsCollected = lampsCollected;
                            HighScores.Insert (i - 1, _temp);
                            break;
                        }
                    }
                }
                else if (i == HighScores.Count && i < LeaderboardLength)
                {
                    Scores _temp = new Scores();
                    _temp.name = name;
                    _temp.killCount = killCount;
                    _temp.lampsCollected = lampsCollected;
                    HighScores.Add(_temp);
                    break;
                }
            }
        }
 
        i = 1;
        while (i <= LeaderboardLength && i <= HighScores.Count) 
        {
            PlayerPrefs.SetString ("HighScore" + i + "name", HighScores [i - 1].name);
            PlayerPrefs.SetInt ("HighScore" + i + "killCount", HighScores [i - 1].killCount);
            PlayerPrefs.SetInt("HighScore" + i + "lampsCollected", HighScores[i - 1].lampsCollected);
            i++;
        }
    }
 
    public List<Scores>  GetHighScore ()
    {
        List<Scores> HighScores = new List<Scores> ();
 
        int i = 1;
        while (i <= LeaderboardLength && PlayerPrefs.HasKey("HighScore" + i + "killCount"))
        {
            Scores temp = new Scores ();
            temp.lampsCollected = PlayerPrefs.GetInt("HighScore" + i + "lampsCollected");
            temp.killCount = PlayerPrefs.GetInt ("HighScore" + i + "killCount");
            temp.name = PlayerPrefs.GetString ("HighScore" + i + "name");
            HighScores.Add (temp);
            i++;
        }
 
        return HighScores;
    }
 
    public void ClearLeaderBoard ()
    {
        //for(int i=0;i<HighScores.
        List<Scores> HighScores = GetHighScore();
 
        for(int i=1; i <= HighScores.Count; i++)
        {
            PlayerPrefs.DeleteKey("HighScore" + i + "name");
            PlayerPrefs.DeleteKey("HighScore" + i + "killCount");
            PlayerPrefs.DeleteKey("HighScore" + i + "lampsCollected");
        }
    }
 
    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
 
public class Scores
{
    public int killCount;
    public int lampsCollected;
    public string name;
}