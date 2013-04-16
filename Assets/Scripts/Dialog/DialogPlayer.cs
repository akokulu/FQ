using UnityEngine;
using System.Collections;

public class DialogPlayer : MonoBehaviour 
{
	public Rect textArea;
	public Rect nameArea;
	public Rect leftAvatarArea;
	public Rect rightAvatarArea;

	bool isPlaying = false;
	Dialog dialog;
	int currentItem = 0;

	void Update()
	{
		if (isPlaying)
		{
			if (Input.GetButtonDown("AdvanceDialog"))
			{
				currentItem++;
				if (currentItem == dialog.dialogItems.Count)
				{
					isPlaying = false;
				}
			}
			else if (Input.GetButtonDown("CloseDialog"))
			{ 
				StopDialog();
			}
		}
	}

	void OnGUI()
	{
		if (isPlaying)
		{
			GUI.skin = dialog.dialogStyle;
			Dialog.DialogElement item = dialog.dialogItems[currentItem];
			Rect textAreaPixel = new Rect(
				(float) Screen.width * textArea.xMin,
				(float) Screen.height * textArea.yMin,
				(float) Screen.width * textArea.width,
				(float) Screen.height * textArea.height
				);
			Rect nameAreaPixel = new Rect(
				(float) Screen.width * nameArea.xMin,
				(float) Screen.height * nameArea.yMin,
				(float) Screen.width * nameArea.width,
				(float) Screen.height * nameArea.height
				);
			Rect avatarArea = (item.avatarPosition == Dialog.DialogElement.AvatarPosition.Left) ? leftAvatarArea : rightAvatarArea;
			Rect avatarAreaPixel = new Rect(
				(float) Screen.width * avatarArea.xMin,
				(float) Screen.height * avatarArea.yMin,
				(float) Screen.width * avatarArea.width,
				(float) Screen.height * avatarArea.height
				);
			if (null != item.dialogText) 		GUI.Label(textAreaPixel, item.dialogText, "box");
			if (null != item.speakerName) 		GUI.Label(nameAreaPixel, item.speakerName);
			if (null != item.speakerPortrait)	GUI.Label(avatarAreaPixel, item.speakerPortrait);
		}
	}

	public void PlayDialog(Dialog dialog)
	{
		PlayDialog(dialog, 0);
	}

	public void PlayDialog(Dialog dialog, int index)
	{
		isPlaying = true;
		this.dialog = dialog;
		currentItem = index;
	}

	public void StopDialog()
	{
		isPlaying = false;
	}
}
