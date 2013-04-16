using UnityEngine;
using System.Collections;

public class BillboardSprite : MonoBehaviour 
{
	// Update is called once per frame
	void LateUpdate () 
	{
		transform.LookAt(Camera.main.transform.position, Vector3.up);
		transform.rotation *= Quaternion.Euler(0f, 180f, 0f);
	}
}
