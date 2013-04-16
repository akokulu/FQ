using UnityEngine;
using System.Collections;

public class SmoothOffsetFollowCamera : MonoBehaviour 
{
	public Transform target;

	public Vector3 offset;
	public float lerpFactor = 0.5f;

	// // Use this for initialization
	// void Start () 
	// {
	// 	offset = Vector3.zero;
	// 	if (null != target)
	// 	{
	// 		offset = target.position - transform.position;
	// 	}
	// }
	
	// Update is called once per frame
	void LateUpdate () 
	{
		transform.position = Vector3.Lerp(transform.position, target.position + offset, lerpFactor);
	}
}
