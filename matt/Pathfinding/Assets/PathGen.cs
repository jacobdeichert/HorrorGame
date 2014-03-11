using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathGen : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	//returns path (list of nodes)
	public List<Node> CalculatePath(Node _start, Node _end, List<Node> _nodes){
		Node current;
		List<Node> openList = new List<Node>();
		List<Node> closedList = new List<Node>();
		List<Node> path = new List<Node>();

		//add _start node to openList
		if (_start != null){
			openList.Add(_start);
		}
		//fill openList&closedList
		//sorts openList every loop
		while (!_end.isClosed && openList.Count != 0 && _start != null){
			openList.Sort((x, y) => {
				//if fs are equal check their h values
				if (x.f == y.f){
					if (x.h > y.h) return 1;
					else if (x.h < y.h) return -1;
					else return 0;

				}
				else if (x.f > y.f) return 1;
				else if (x.f < y.f) return -1;
				else return 0;}
			);
			//adds lowest node on openList to closedList
			openList[0].isClosed = true;
			current = openList[0];
			closedList.Add(current);
			//remove lowest openList node
			openList.RemoveAt(0);
			if (current == _end){
				break;
			}
			//go through the array of _nodes
			foreach (Node node in _nodes){
				//if node is above current node
				if (node.transform.position.x == current.transform.position.x && node.transform.position.z == current.transform.position.z + 1){
					//if node isn't a wall or on the closedList
					if (!node.wall && !node.isClosed){
						//if on the openList simply update it's parent
						if (node.isOpen){
							if (node.parent.f > current.f){
								node.parent = current;
							}
						}
						//else add to the openList
						else{
							node.parent = current;
							openList.Add(node);
							node.isOpen = true;
						}
						//calculate nodes traversal values
						node.CalculateValues(node.parent, _end);
					}
				}
				//below
				if (node.transform.position.x == current.transform.position.x && node.transform.position.z == current.transform.position.z - 1){
					if (!node.wall && !node.isClosed){
						if (node.isOpen){
							if (node.parent.f > current.f){
								node.parent = current;
							}
						}
						else{
							node.parent = current;
							openList.Add(node);
							node.isOpen = true;
						}
						node.CalculateValues(node.parent, _end);
					}
				}
				//right
				if (node.transform.position.x == current.transform.position.x + 1 && node.transform.position.z == current.transform.position.z){
					if (!node.wall && !node.isClosed){
						if (node.isOpen){
							if (node.parent.f > current.f){
								node.parent = current;
							}
						}
						else{
							node.parent = current;
							openList.Add(node);
							node.isOpen = true;
						}
						node.CalculateValues(node.parent, _end);
					}
				}
				//left
				if (node.transform.position.x == current.transform.position.x - 1 && node.transform.position.z == current.transform.position.z){
					if (!node.wall && !node.isClosed){
						if (node.isOpen){
							if (node.parent.f > current.f){
								node.parent = current;
							}
						}
						else{
							node.parent = current;
							openList.Add(node);
							node.isOpen = true;
						}
						node.CalculateValues(node.parent, _end);
					}
				}
			}
		}
		//fills path
		if (openList.Count != 0){
			//adds the last node in the closedList aka the target/end
			path.Add(closedList[closedList.Count - 1]);
			current = closedList[closedList.Count - 1];
			//then loops through the parent of current node starting with the end/target
			bool done = false;
			while (!done){
				if (current.parent != null){
					path.Add(current.parent);
					current = current.parent;
				}
				else{
					done = true;
				}
			}
		}
		//clears all nodes isOpen/isClosed variables
		foreach (Node node in _nodes){
			node.isOpen = false;
			node.isClosed = false;
		}
		//reverses the path so the target/end is last and not first, then returns the list
		path.Reverse();
		return path;
	}
	/* //not using anymore
	private bool CheckList(List<Node> _nodes, Node _node){
		bool test = false;
		foreach (Node node in _nodes){
			if (node == _node){
				test = true;
			}
		}
		return test;
	}*/
}
