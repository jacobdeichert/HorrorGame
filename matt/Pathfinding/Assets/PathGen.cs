using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathGen : MonoBehaviour {

	int nodeSize;

	// Use this for initialization
	void Start () {
		nodeSize = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	//returns path (list of nodes)
	public List<Node> CalculatePath(Node _start, Node _end, List<Node> _nodes){

		_nodes.Sort((x, y) => {
			//if fs are equal check their h values
			if (x.transform.position.z == y.transform.position.z){
				if (x.transform.position.x > y.transform.position.x) return 1;
				else if (x.transform.position.x < y.transform.position.x) return -1;
				else return 0;
			}
			else if (x.transform.position.z > y.transform.position.z) return 1;
			else if (x.transform.position.z < y.transform.position.z) return -1;
			else return 0;}
		);
		foreach (Node node in _nodes){
			node.listIndex = _nodes.IndexOf(node);
		}

		Node current;
		List<Node> openList = new List<Node>();
		List<Node> closedList = new List<Node>();
		List<Node> path = new List<Node>();

		nodeSize = (int)(_start.renderer.bounds.size.x);

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
			if (current.transform.position.x == _end.transform.position.x
			    && current.transform.position.z == _end.transform.position.z){
				Debug.Log("end node reached.");
				break;
			}
			//go through the array of _nodes
//			foreach (Node node in _nodes){
			int _index = current.listIndex - 18;
			if (_index >= 0 && _index < _nodes.Count){
				//if node is above current node
				if (_nodes[_index].transform.position.x == current.transform.position.x && _nodes[_index].transform.position.z == current.transform.position.z - nodeSize){
					//if node isn't a wall or on the closedList
					if (!_nodes[_index].wall && !_nodes[_index].isClosed){
						//if on the openList simply update it's parent
						if (_nodes[_index].isOpen){
							if (_nodes[_index].parent.f > current.f){
								_nodes[_index].parent = current;
							}
						}
						//else add to the openList
						else{
							_nodes[_index].parent = current;
							openList.Add(_nodes[_index]);
							_nodes[_index].isOpen = true;
						}
						//calculate nodes traversal values
						_nodes[_index].CalculateValues(_nodes[_index].parent, _end);
					}
				}
			}
			_index = current.listIndex + 18;
			if (_index >= 0 && _index < _nodes.Count){
				//below
				if (_nodes[_index].transform.position.x == current.transform.position.x && _nodes[_index].transform.position.z == current.transform.position.z + nodeSize){
					if (!_nodes[_index].wall && !_nodes[_index].isClosed){
						if (_nodes[_index].isOpen){
							if (_nodes[_index].parent.f > current.f){
								_nodes[_index].parent = current;
							}
						}
						else{
							_nodes[_index].parent = current;
							openList.Add(_nodes[_index]);
							_nodes[_index].isOpen = true;
						}
						_nodes[_index].CalculateValues(_nodes[_index].parent, _end);
					}
				}
			}
			_index = current.listIndex - 1;
			if (_index >= 0 && _index < _nodes.Count){
				//right
				if (_nodes[_index].transform.position.x == current.transform.position.x - nodeSize && _nodes[_index].transform.position.z == current.transform.position.z){
					if (!_nodes[_index].wall && !_nodes[_index].isClosed){
						if (_nodes[_index].isOpen){
							if (_nodes[_index].parent.f > current.f){
								_nodes[_index].parent = current;
							}
						}
						else{
							_nodes[_index].parent = current;
							openList.Add(_nodes[_index]);
							_nodes[_index].isOpen = true;
						}
						_nodes[_index].CalculateValues(_nodes[_index].parent, _end);
					}
				}
			}
			_index = current.listIndex + 1;
			if (_index >= 0 && _index < _nodes.Count){
				//left
				if (_nodes[_index].transform.position.x == current.transform.position.x + nodeSize && _nodes[_index].transform.position.z == current.transform.position.z){
					if (!_nodes[_index].wall && !_nodes[_index].isClosed){
						if (_nodes[_index].isOpen){
							if (_nodes[_index].parent.f > current.f){
								_nodes[_index].parent = current;
							}
						}
						else{
							_nodes[_index].parent = current;
							openList.Add(_nodes[_index]);
							_nodes[_index].isOpen = true;
						}
						_nodes[_index].CalculateValues(_nodes[_index].parent, _end);
					}
				}
			}
	//		}
		}
		Debug.Log(closedList.Count);
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
		Debug.Log(path.Count);
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
