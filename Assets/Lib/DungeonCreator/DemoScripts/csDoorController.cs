using UnityEngine;
using System.Collections;

public class csDoorController 
	: MonoBehaviour 
{
	
	private bool opening = false;
	
	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "MainCamera")
			opening = true;		
		
	}
	
	public void OnTriggerExit(Collider other)
	{
		if (other.tag == "MainCamera")
			opening = false;		
	}
	
	public void Update()
	{
		Transform door = transform.FindChild("door");
		
		if (opening)
		{
			door.position = Vector3.Lerp(door.position, transform.TransformPoint(new Vector3(0, 0.95f, 0)), 0.025f);
		}
		else
		{
			door.position = Vector3.Lerp(door.position, transform.TransformPoint(new Vector3(0,0,0)), 0.025f);	
		}
	}
}
