using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridNavigation	: MonoBehaviour
{
	public delegate void OnPathFound(List<Vector2> path);

	struct PathRequest
	{
		public GridMotor 	motor;
		public Vector3		source;
		public Vector3 		target;
		public OnPathFound 	callback;
	}

	public float maxSearchTime = 1.0f;
	bool isSearching = false;
	Queue<PathRequest> requests = new Queue<PathRequest>();

	public void RequestPath(GridMotor motor, Transform target, OnPathFound callback)
	{
		if (null != callback)
		{
			if (!isSearching)
			{
				OnSearchStart(new PathRequest() { motor = motor, source = motor.transform.position, target = target.position, callback = callback });
			}
			else
			{
				requests.Enqueue(new PathRequest() { motor = motor, source = motor.transform.position, target = target.position, callback = callback });
			}
		}
	}

	IEnumerator OnSearchStart(PathRequest request)
	{
		yield return new WaitForSeconds(0f);
		List<GridNode> seen = new List<GridNode>();
		List<GridNode> visited = new List<GridNode>();
		List<GridNode> connections = new List<GridNode>();
		GridNode opened = new GridNode() { position = request.motor.transform.position, cost = 0f };
		//float endTime = 
		//do
		//{
			for (float n = request.motor.gridSize, i = -request.motor.gridSize; i <= n; i += n)
			{
				for (float j = -n; j <= n; j += n)
				{
					if (!Mathf.Approximately(i, 0f) && !Mathf.Approximately(j, 0f))
					{
						connections.Add(new GridNode() { position = opened.position + new Vector3(i, request.source.y, j), cost = opened.cost + request.motor.gridSize});
					}
				}
			}
		//} while (0 != connections.Count);
	}

	void OnSearchEnded(List<Vector2> path)
	{

	}

	struct GridNode
	{
		public Vector3 position;
		public float cost;
	}
}