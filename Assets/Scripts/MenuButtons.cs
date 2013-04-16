using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuButtons : MonoBehaviour
{

    public enum GameState { MainMenu, Name, Score, Pause, Play };
    public static GameState gamestate = GameState.MainMenu;

    public GUITexture Title;

    public GUITexture HighScore;
    public GUITexture Play;
    public GUITexture Return;

    public GUITexture Quit;

    public GUITexture Resume;
    
	public GUIText Name;
	public GUIText Command;
	
	public GUITexture background;

    public Texture TextureTitle;
    public Texture TextureHighScores;

    public Texture TexturePlayGame;
    public Texture TextureReturn;
	
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
        {
            if (gamestate == GameState.MainMenu)
                PlayerStats.playerName = Name.text; // Set player name

            if (gamestate == GameState.Play)
                ShowPause();
        }
		
		//if(gamestate == GameState.MainMenu)
			//Name.text = "Name:" + PlayerProps.name;
		
    }

    void ShowMenu()
    {
        gamestate = GameState.MainMenu;
        Title.texture = TextureTitle;
        Return.enabled = false;

        Play.enabled = true;
        Title.enabled = true;
        HighScore.enabled = true;
        Quit.enabled = true;

		PlayerStats.playerName = "";
        Name.text = "";
		Command.enabled = false;
    }

    void ShowScore()
    {
        gamestate = GameState.Score;
        Title.enabled = true;
        Title.texture = TextureHighScores;
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
            GUILayout.Box("Name", GUILayout.Width(Screen.width / 8));
            GUILayout.Box("Kill Count", GUILayout.Width(Screen.width / 8));
            GUILayout.Box("Lamps Collected", GUILayout.Width(Screen.width / 8));
            GUILayout.EndHorizontal();

            GUILayout.Space(25);

            foreach (Scores _score in highscore)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(Screen.width / 4);
                GUILayout.Box(_score.name, GUILayout.Width(Screen.width / 8));
                GUILayout.Box("" + _score.killCount, GUILayout.Width(Screen.width / 8));
                GUILayout.Box("" + _score.lampsCollected, GUILayout.Width(Screen.width / 8));
                GUILayout.EndHorizontal();
            }
        }
    }
}