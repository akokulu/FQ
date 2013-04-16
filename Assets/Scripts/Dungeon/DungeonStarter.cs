using UnityEngine;
using System.Collections;

public class DungeonStarter : MonoBehaviour {
	
	public Transform player;
	
	// Use this for initialization
	void Start () 
	{
		StartCoroutine(StartDungeon(1));
		StartCoroutine(Retrigger(10));
	}
	private IEnumerator StartDungeon(int delay)
	{
		yield return new WaitForSeconds(delay);
		Vector3 temp = transform.position;
		temp.x = player.transform.position.x;
		temp.z = player.transform.position.z;
		transform.position = temp;
	}
	
	private IEnumerator Retrigger(int delay)
	{
		yield return new WaitForSeconds(delay);
		collider.enabled = true;
		Debug.Log("may leave");
	}
}
