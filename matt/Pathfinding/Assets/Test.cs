using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {
	
	private List<Node> nodes = new List<Node>();
	private List<Node> path = new List<Node>();
	private Node start;
	private Node end;

	//sets the end target for now
	//sets up all nodes in the scene in a list
	void Start () {
		gameObject.AddComponent<PathGen>();
		end = GameObject.Find ("End").GetComponent<Node>();
		foreach (Node node in GameObject.FindObjectsOfType<Node>()){
			nodes.Add(node);
		}
	}
	
	//resets nodes
	//calculates path then sets nodes path true to show path
	void Update () {
		if (Input.GetKeyDown(KeyCode.E)){
			FindPath();
		}
		//FindPath();
	}
	//resets all the nodes parents
	//and sets a new start node based on players position
	private void ResetPath(){
		Vector2 player = new Vector2(Mathf.Round(GameObject.Find("3rd Person Controller").transform.position.x), Mathf.Round(GameObject.Find("3rd Person Controller").transform.position.z));
		foreach (Node node in nodes){
			node.path = false;
			node.parent = null;
			if (new Vector2(node.transform.position.x, node.transform.position.z) == player){
				start = node;
			}
		}
	}
	private void FindPath(){
		ResetPath();
		path = gameObject.GetComponent<PathGen>().CalculatePath(start, end, nodes);
		if (path.Count > 0){
			foreach (Node node in path){
				if (node != null){
					//sets nodes path true to change it's color
					node.path = true;
				}
			}
		}
		else{
			Debug.Log("No path.");
		}
	}
}
