﻿using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {

	public bool wall, path, isOpen, isClosed;
	public Node parent;
	//path scores
	public float g, h, f;
	public int listIndex;

	// Use this for initialization
	void Start () {

	}
	
	//used only to change the blocks colors
	void Update () {

	}
	//calculates traversal values for a node
	public void CalculateValues(Node _parent, Node _end){
		float x, z;
		//set g *distance from start*
		if (_parent == null){
			g = 0;
		}
		else{
			g = _parent.g + 10;
		}
		//set h *distance from end*
		x = Mathf.Abs(gameObject.transform.position.x - _end.transform.position.x);
		z = Mathf.Abs(gameObject.transform.position.z - _end.transform.position.z);
		h = x + z;
		//set f *distances added*
		f = g + h;
	}
}
