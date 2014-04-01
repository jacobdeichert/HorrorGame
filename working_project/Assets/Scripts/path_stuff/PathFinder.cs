using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour
{
	public List<Node> nodes = new List<Node>();
	public List<Node> path = new List<Node>();
	Node startNode;
	Node endNode;

	void Start () 
	{
		foreach (Node node in GameObject.FindObjectsOfType<Node>())
			nodes.Add(node);
	}

	public void ResetPath(Vector3 sourcePos)
	{
		Vector2 startPos = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.z));
		Vector2 endPos = new Vector2(Mathf.Round(sourcePos.x), Mathf.Round(sourcePos.z));
		foreach (Node node in nodes)
		{
			node.path = false;
			node.parent = null;
			if (new Vector2(node.transform.position.x, node.transform.position.z) == startPos)
				startNode = node;
			else if(new Vector2(node.transform.position.x, node.transform.position.z) == endPos)
				endNode = node;
		}

		path = gameObject.GetComponent<PathGen>().CalculatePath(startNode, endNode, nodes);
		if (path.Count > 0)
			foreach (Node node in path)
				if (node != null)
					node.path = true; //sets nodes path true to change it's color
        else
            Debug.Log("No path.");
	}
}
