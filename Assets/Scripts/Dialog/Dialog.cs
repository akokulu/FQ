using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Dialog : ScriptableObject 
{
	public GUISkin dialogStyle;

	[System.Serializable]
	public class DialogElement
	{
		public enum AvatarPosition { Left, Right };

		public string 			speakerName;
		public Texture2D 		speakerPortrait;
		public string 			dialogText;
		public AvatarPosition 	avatarPosition;
	}
	public List<DialogElement> dialogItems;


	[MenuItem("Assets/Create/Dialogue")]
	public static void CreateAsset()
	{
		CustomAssetUtility.CreateAsset<Dialog>();
	}
}