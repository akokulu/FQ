using UnityEngine;
using System.Collections;
using System.Text;
using System.IO; 

public class DialogScript : MonoBehaviour {

	public static int chooseDialog;
	
	private TextAsset begin;
	private TextAsset end;
	
	static public string dialogStart,dialogEnd;

    public GUIText guiDialog;
	public GUITexture GUITextBackground;

    public AudioSource mumble;

	void Awake ()
	{
		begin = Resources.Load("start") as TextAsset;
        end = Resources.Load("end") as TextAsset;
        //Debug.Log(begin.text);
        //Debug.Log(end.text);
        
        string[] linesStart = begin.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);
		string[] linesEnd = end.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);
		
		chooseDialog = Random.Range(0,linesStart.Length);
		
		dialogStart = linesStart[chooseDialog];
		dialogEnd = linesEnd[chooseDialog];
		
		//print(begin.text);
		//print(end.text);
	}

    void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.name == "Player")
		{
            mumble.Play();
            mumble.loop = true;

            if (!PlayerProps.hasItem)
                guiDialog.text = dialogStart;
            else
                guiDialog.text = dialogEnd;

            GUITextBackground.enabled = true;

            StartCoroutine(ShowDialog(5));
		}
    }

    private IEnumerator ShowDialog(int delay)
    {
        yield return new WaitForSeconds(delay);
        guiDialog.text = "";
        GUITextBackground.enabled = false;
        mumble.Stop();
    }
}
