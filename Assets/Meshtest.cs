using UnityEngine;
using System.Collections;

public class Meshtest : MonoBehaviour {

	// Use this for initialization
	public void Start () {


		MeshFilter filter = gameObject.AddComponent<MeshFilter> ();
		gameObject.AddComponent<MeshRenderer> ();

		Vector3[] vertices = new Vector3[] {
			new Vector3 (-1, -1, 0),
			new Vector3 (1, -1, 0),
			new Vector3 (1, 1, 0),
			new Vector3 (-1, 1, 0)
		};
		int[] triangle = new int[] {
			0, 1, 2,
			0, 2, 3
		};

		filter.mesh = new Mesh();
		filter.mesh.vertices = vertices;
		filter.mesh.triangles = triangle;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
