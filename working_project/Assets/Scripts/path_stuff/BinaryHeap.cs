using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BinaryHeap {

	private List<Node> nodes = new List<Node>();

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Add(Node _node){
		if (nodes.Count == 0){
			nodes.Add(_node);
		}
		else{
			nodes.Add(_node);
			int cursor = nodes.Count - 1;
			while (cursor > 1){
				//parent < child
				if (TestNodes(nodes[Mathf.RoundToInt(cursor / 2)], nodes[cursor]) == 1){
					break;
				}
				//parent > child
				else{
					Node tempNode = nodes[Mathf.RoundToInt(cursor / 2)];
					nodes[Mathf.RoundToInt(cursor / 2)] = nodes[cursor];
					nodes[cursor] = tempNode;
					
					cursor = Mathf.RoundToInt(cursor / 2);
				}
			}
		}
	}
	public Node Remove(){
		if (nodes.Count > 0){
			Node temp = nodes[0];
			nodes[0] = nodes[nodes.Count - 1];
			nodes.RemoveAt(nodes.Count - 1);
			//setup cursor
			if (nodes.Count > 1){
				int cursor = 1;
				while (cursor <= nodes.Count - 1){
					//check if node has 2 children
					if (cursor * 2 <= nodes.Count - 1 && cursor * 2 + 1 <= nodes.Count - 1){
						//check if either child is smaller then parent
						if (nodes[cursor].f >= nodes[cursor * 2].f || nodes[cursor].f >= nodes[cursor * 2 + 1].f){
							//TEST H VALUES WHEN TIE
							if (TestNodes(nodes[cursor * 2], nodes[cursor * 2 + 1]) == 1){
								//TEST FOR H VALUES WHEN THERE IS A TIE
								if (TestNodes (nodes[cursor], nodes[cursor * 2]) == 1){
									break;
								}
								else{
									Node _tempNode = nodes[cursor];
									nodes[cursor] = nodes[cursor * 2];
									nodes[cursor * 2] = _tempNode;
									cursor = cursor * 2;
								}
							}
							else{
								//TEST FOR H VALUES WHEN THERE IS A TIE
								if (TestNodes (nodes[cursor], nodes[cursor * 2 + 1]) == 1){
									break;
								}
								else{
									Node _tempNode = nodes[cursor];
									nodes[cursor] = nodes[cursor * 2 + 1];
									nodes[cursor * 2 + 1] = _tempNode;
									cursor = cursor * 2 + 1;
								}
							}
						}
						else{
							break;
						}
					}
					//else if check if has only left child
					else if (cursor * 2 <= nodes.Count - 1 && cursor * 2 + 1 > nodes.Count - 1){
						if (TestNodes(nodes[cursor], nodes[cursor * 2]) == 1){
							break;
						}
						//else break
						else{
							Node _tempNode = nodes[cursor];
							nodes[cursor] = nodes[cursor * 2];
							nodes[cursor * 2] = _tempNode;
							cursor = cursor * 2;
						}
					}
					//no children
					else{
						break;
					}
				}
			}
			return temp;
		}
		else{
			//empty list
			return null;
		}
	}
	private int TestNodes(Node _parent, Node _child){
		if (_parent.f == _child.f){
			if (_parent.h <= _child.h){
				return 1;
			}
			else{
				return -1;
			}
		}
		else if (_parent.f < _child.f){
			return 1;
		}
		else{
			return -1;
		}
	}
	public List<Node> GetList(){
		return nodes;
	}
}
