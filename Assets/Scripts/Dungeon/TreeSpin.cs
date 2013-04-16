using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TreeSpin : MonoBehaviour 
{

	void Start () 
	{
		transform.rotation *= Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward);
	}
	
}
