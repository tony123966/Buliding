using UnityEngine;
using System.Collections;

public class QuadItem : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (!gameObject.GetComponent<MeshFilter>()) gameObject.AddComponent<MeshFilter>();
		if (!gameObject.GetComponent<MeshRenderer>()) gameObject.AddComponent<MeshRenderer>();
		Mesh mesh = GetComponent<MeshFilter>().mesh;

		mesh.Clear();


		Camera ca = GameObject.Find("UICamera").GetComponent<Camera>();

		Vector3[] vertices = new Vector3[]
         {
            (new Vector3( 1, 1, 0)),
             ( new Vector3( 1, -1, 0)),
             (new Vector3(-1, 1,  0)),
            ( new Vector3(-1, -1, 0)),
         };

		Vector2[] uv = new Vector2[]
         {
             new Vector2(1, 1),
             new Vector2(1, 0),
             new Vector2(0, 1),
             new Vector2(0, 0),
         };

		int[] triangles = new int[]
         {
             0, 1, 2,
             2, 1, 3,
         };

		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
	}
}
