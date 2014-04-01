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
		Vector2 startPos = new Vector2(transform.position.x, transform.position.z);
		Vector2 endPos = new Vector2(sourcePos.x, sourcePos.z);
		float _nodeSize = gameObject.GetComponent<PathGen>().nodeSize;
		foreach (Node node in nodes)
		{
			node.path = false;
			node.parent = null;

			/*if (node.transform.position.x - _nodeSize / 2 < startPos.x && node.transform.position.x + _nodeSize / 2 > startPos.x &&
			    node.transform.position.z - _nodeSize / 2 < startPos.y && node.transform.position.z + _nodeSize / 2 > startPos.y){
				Debug.Log("start node found");
				startNode = node;
			}*/
			if (node.transform.position == new Vector3(75, -5, 75)){
				Debug.Log("temp start found");
				startNode = node;
			}
			/*if (node.transform.position.x - _nodeSize / 2 < endPos.x && node.transform.position.x + _nodeSize / 2 > endPos.x &&
			    node.transform.position.z - _nodeSize / 2 < endPos.y && node.transform.position.z + _nodeSize / 2 > endPos.y){
				Debug.Log("end node found");
				endNode = node;
			}*/
			if (node.transform.position == new Vector3(145, -5, 150)){
				Debug.Log("temp end found");
				endNode = node;
			}
		}
		if (startNode && endNode){
			path = gameObject.GetComponent<PathGen>().CalculatePath(startNode, endNode, nodes);
		}
		if (path.Count > 0)
			foreach (Node node in path){
				Debug.Log(node.transform.position);
				node.path = true; //sets nodes path true to change it's color
			}
        else
            Debug.Log("No path.");
	}
}
