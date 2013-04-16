using UnityEngine;
using System.Collections;

[RequireComponent(typeof(DialogPlayer))]
public class TestDialogPlayer : MonoBehaviour 
{
	public Dialog dialog;

	
	void Start () 
	{
		DialogPlayer player = GameObject.FindObjectOfType(typeof(DialogPlayer)) as DialogPlayer;
		if (player)
		{
			player.PlayDialog(dialog);
		}
	}
}
