using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MeshObj : MonoBehaviour
{
	public List<GameObject> controlPointList;
	// Use this for initialization
	void Awake () 
	{
		if (!gameObject.GetComponent<MeshFilter>()) gameObject.AddComponent<MeshFilter>();
		if (!gameObject.GetComponent<MeshRenderer>()) gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
		if (controlPointList.Count==3)
		{
			mesh.vertices = new Vector3[] { controlPointList[0].transform.localPosition, controlPointList[1].transform.localPosition, controlPointList[2].transform.localPosition };
			mesh.uv = new Vector2[] {new Vector2 (0, 0), new Vector2 (0, 1), new Vector2 (1, 1)};
			mesh.triangles =  new int[]{0, 1, 2};
		}
	}
}
