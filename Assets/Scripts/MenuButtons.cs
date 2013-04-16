using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuButtons : MonoBehaviour
{

    public enum GameState { MainMenu, Name, Score, Pause, Play };
    public static GameState gamestate = GameState.MainMenu;

    public GUIText Title;
    public GUIText HighScore;
    public GUIText Quit;

    public GUIText Play;
    public GUIText Resume;
    public GUIText Return;
	
	public GUIText Name;
	public GUIText Command;
	
	public GUITexture background;
	
    List<Scores> highscore;

    void OnMouseDown()
    {
        if (this.gameObject.name == "GUI Play")
            EnterName();
        else if (this.gameObject.name == "GUI High Score")
            ShowScore();
        else if (this.gameObject.name == "GUI Resume")
            ResumeGame();
        else if (this.gameObject.name == "GUI Menu")
            ShowMenu();
        else if (this.gameObject.name == "GUI Return")
            Application.LoadLevel("Main Menu");
        else if (this.gameObject.name == "GUI Quit")
            Application.Quit();
    }

    void Start()
    {
        highscore = new List<Scores>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
            if (gamestate == GameState.Play)
                ShowPause();
		
		//if(gamestate == GameState.MainMenu)
			//Name.text = "Name:" + PlayerProps.name;
		
    }

    void ShowMenu()
    {
        gamestate = GameState.MainMenu;
        Title.text = "Fetch Quest";
        Return.enabled = false;

        Play.enabled = true;
        Title.enabled = true;
        HighScore.enabled = true;
        Quit.enabled = true;
		PlayerStats.playerName = "";
		Command.enabled = false;
    }

    void ShowScore()
    {
        gamestate = GameState.Score;
        Title.enabled = true;
        Title.text = "High Scores";
        Return.enabled = true;

        Play.enabled = false;
        HighScore.enabled = false;
        Quit.enabled = false;
    }

    void ShowPause()
    {
        gamestate = GameState.Pause;
        Resume.enabled = true;
        Title.enabled = true;
        Quit.enabled = true;
		background.enabled = true;
    }

    void ResumeGame()
    {
        gamestate = GameState.Play;
        Resume.enabled = false;
        Title.enabled = false;
        Quit.enabled = false;
		background.enabled = false;
    }
	
	void EnterName()
    {
        gamestate = GameState.Name;
		
		Return.enabled = true;
		Command.enabled = true;

        Play.enabled = false;
        HighScore.enabled = false;
        Quit.enabled = false;
    }
	
    public static void PlayGame()
    {
        gamestate = GameState.Play;
        Application.LoadLevel("level switch");
    }

    void OnGUI()
    {
        if (gamestate == GameState.Score)
        {
            highscore = HighScoreManager._instance.GetHighScore();

            GUILayout.Space(Screen.height / 3);

            GUILayout.BeginHorizontal();
            GUILayout.Space(Screen.width / 4);
            GUILayout.Label("Name", GUILayout.Width(Screen.width / 8));
            GUILayout.Label("Kill Count", GUILayout.Width(Screen.width / 8));
            GUILayout.Label("Lamps Collected", GUILayout.Width(Screen.width / 8));
            GUILayout.EndHorizontal();

            GUILayout.Space(25);

            foreach (Scores _score in highscore)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(Screen.width / 4);
                GUILayout.Label(_score.name, GUILayout.Width(Screen.width / 8));
                GUILayout.Label("" + _score.killCount, GUILayout.Width(Screen.width / 8));
                GUILayout.Label("" + _score.lampsCollected, GUILayout.Width(Screen.width / 8));
                GUILayout.EndHorizontal();
            }
        }
    }
}