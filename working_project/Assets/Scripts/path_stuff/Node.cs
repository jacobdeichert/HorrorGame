using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {

	public bool wall, path, isOpen, isClosed;
	public Node parent;
	//path scores
	private float g, h, f;

    public float G {
        get { return g; }
    }
    public float H
    {
        get { return h; }
    }
    public float F
    {
        get { return f; }
    }

	public int listIndex;

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
