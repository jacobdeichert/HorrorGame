using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathTest : MonoBehaviour {
	
	private List<Node> nodes = new List<Node>();
	private List<Node> path = new List<Node>();
	private Node start;
	private Node end;
	
	//sets the end target for now
	//sets up all nodes in the scene in a list
	void Start () {
		//can't add nodes in the constructor
	}
	
	//resets nodes
	//calculates path then sets nodes path true to show path
	void Update () {
		if (Input.GetKeyDown(KeyCode.E)){
			if (nodes.Count == 0){
				foreach (Node node in GameObject.FindObjectsOfType<Node>()){
					nodes.Add(node);
					if (node.transform.position == new Vector3(145, -5, 150)){
						end = node;
					}
				}
			}
			FindPath();
		}
	}
	//resets all the nodes parents
	//and sets a new start node based on players position
	private void ResetPath(){
		Vector2 player = new Vector2(GameObject.Find("player(Clone)").transform.position.x, GameObject.Find("player(Clone)").transform.position.z);
		int _nodeSize = gameObject.GetComponent<PathGen>().nodeSize;
		foreach (Node node in nodes){
			node.path = false;
			node.parent = null;
			if (node.transform.position.x - _nodeSize / 2 < player.x && node.transform.position.x + _nodeSize / 2 > player.x &&
			    node.transform.position.z - _nodeSize / 2 < player.y && node.transform.position.z + _nodeSize / 2 > player.y){
				start = node;
			}
		}
	}
	private void FindPath(){
		ResetPath();
		path = gameObject.GetComponent<PathGen>().CalculatePath(start, end, nodes);
		if (path.Count > 0){
			/*foreach (Node node in path){
				Debug.Log("node: " + node.transform.position);
			}*/
		}
		else{
			Debug.Log("No path.");
		}
	}
}