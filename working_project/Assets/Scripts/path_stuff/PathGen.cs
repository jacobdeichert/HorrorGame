using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathGen : MonoBehaviour {

	public int nodeSize;
	//width of map
	public int mapSize;

	// Use this for initialization
	void Start () {

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
		//temp lists
		Node current;
		List<Node> openList = new List<Node>();
		List<Node> closedList = new List<Node>();
		List<Node> path = new List<Node>();
		//sets nodeSize to renderer bounds
		//nodeSize = (int)(_start.renderer.bounds.size.x);

		//add _start node to openList
		if (_start != null){
			openList.Add(_start);
		}
		//fill openList&closedList
		//sorts openList every loop
		while (!_end.isClosed && openList.Count != 0 && _start != null){
			openList.Sort((x, y) => {
				//if fs are equal check their h values
				if (x.F == y.F){
					if (x.H > y.H) return 1;
					else if (x.H < y.H) return -1;
					else return 0;
				}
				else if (x.F > y.F) return 1;
				else if (x.F < y.F) return -1;
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
			//access adjacent nodes by their index
			int _index = current.listIndex - mapSize;
			//set index to currents index - mapWidth (above)
			if (_index >= 0 && _index < _nodes.Count){
				//if node isn't a wall or on the closedList
				if (!_nodes[_index].wall && !_nodes[_index].isClosed){
					//if on the openList simply update it's parent
					if (_nodes[_index].isOpen){
						if (_nodes[_index].parent.F > current.F){
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
			_index = current.listIndex + mapSize;
			//set index to currents index + mapWidth (below)
			if (_index >= 0 && _index < _nodes.Count){
				if (!_nodes[_index].wall && !_nodes[_index].isClosed){
					if (_nodes[_index].isOpen){
						if (_nodes[_index].parent.F > current.F){
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
			_index = current.listIndex - 1;
			//set index to currents index - 1 (right)
			if (_index >= 0 && _index < _nodes.Count){
				//need to check actual position of nodes because current node may be an edge
				if (_nodes[_index].transform.position.x == current.transform.position.x - nodeSize && _nodes[_index].transform.position.z == current.transform.position.z){
					if (!_nodes[_index].wall && !_nodes[_index].isClosed){
						if (_nodes[_index].isOpen){
							if (_nodes[_index].parent.F > current.F){
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
			//set index to currents index + 1 (left)
			if (_index >= 0 && _index < _nodes.Count){
				//need to check actual position of nodes because current node may be an edge
				if (_nodes[_index].transform.position.x == current.transform.position.x + nodeSize && _nodes[_index].transform.position.z == current.transform.position.z){
					if (!_nodes[_index].wall && !_nodes[_index].isClosed){
						if (_nodes[_index].isOpen){
							if (_nodes[_index].parent.F > current.F){
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
}
