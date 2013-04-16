using UnityEngine;
using System.Collections;

public class InputName : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (MenuButtons.gamestate == MenuButtons.GameState.Name)
		{
			foreach (char c in Input.inputString) 
			{
	            if (c == "\b"[0])//\b = backspace
				{
	                if (guiText.text.Length != 0)
	                    guiText.text = guiText.text.Substring(0, guiText.text.Length - 1);
					//print(guiText.text);
				}
	            else
				{
	                if (c == "\n"[0] || c == "\r"[0])//\n = enter
					{
	                    //print("User entered his name: " + guiText.text);
						PlayerStats.playerName = guiText.text;
						MenuButtons.PlayGame();
						//print(PlayerProps.name);
					}
	                else
	                    guiText.text += c;
					//print(guiText.text);
				}
	        }
		}
	}
}
