using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CreateIconRectangle : MonoBehaviour {

	public List<GameObject> controlPointList;
	[HideInInspector]
	Mesh mesh;
	DragItemController dragItemController;

	void Awake()
	{
		dragItemController = GameObject.Find("DragItemController").GetComponent<DragItemController>();	
	}
	void SetMesh() 
	{
		if (!gameObject.GetComponent<MeshFilter>()) gameObject.AddComponent<MeshFilter>();
		if (!gameObject.GetComponent<MeshRenderer>()) gameObject.AddComponent<MeshRenderer>();
		mesh = GetComponent<MeshFilter>().mesh;
		mesh.Clear();
	
	}
}
