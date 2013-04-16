using UnityEngine;
using System.Collections;

public class DungeonManager : MonoBehaviour 
{
	ceDungeonGenerator generator;

	void Start () 
	{
		generator = GameObject.FindObjectOfType(typeof(ceDungeonGenerator)) as ceDungeonGenerator;
		Invoke("GenerateDungeon", Random.Range(0.0f, 0.2f));
	}

	void GenerateDungeon()
	{
		generator.DestroyDungeon();
		generator.CreateDungeon();
	}
}
