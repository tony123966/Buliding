using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MeshObj : MonoBehaviour
{
	public List<GameObject> controlPointList;
	// Use this for initialization
	public GameObject chooseOBJ = null;
	Mesh mesh;
	Vector3[] verts;
	Vector3[] each_tomiddle;
	Vector3 vertPos;
	Vector3 FirstPos;

	public DragItemController dragitemcontroller;
	public Material lineMat;
	public GameObject cp1;
	public GameObject cp2;



	void Start(){

		dragitemcontroller = GameObject.Find ("DragItemController").GetComponent<DragItemController> ();

	}

	void Awake () 
	{
		mesh = GetComponent<MeshFilter>().mesh;
		if (!gameObject.GetComponent<MeshFilter>()) gameObject.AddComponent<MeshFilter>();
		if (!gameObject.GetComponent<MeshRenderer>()) gameObject.AddComponent<MeshRenderer>();
        mesh.Clear();
		if (controlPointList.Count ==  6)
		{
			mesh.vertices = new Vector3[] {
				controlPointList [0].transform.localPosition,
				controlPointList [1].transform.localPosition,
				controlPointList [2].transform.localPosition,
				controlPointList [3].transform.localPosition,
				controlPointList [4].transform.localPosition,
				controlPointList [5].transform.localPosition
			};
			//mesh.uv = new Vector2[] {new Vector2 (0, 0),  new Vector2 (0, 1), new Vector2 (1, 1)};
			mesh.triangles =  new int[]{0, 1, 2,0,3,1}; 
			verts = mesh.vertices;
			//store first 
		}
	}

	void Update () {
		 
		//f (dragitemcontroller.chooseObj) {
		//	FirstPos = dragitemcontroller.chooseObj.transform.localPosition;
		//	Debug.Log ("FirstPos:" + FirstPos);
		//	adjPos();
		//	adjMesh();
		//
		if(Input.GetMouseButtonUp(0)){
			print ("Update verts array");
			for (int i = 0; i < controlPointList.Count; i++) {
				verts [i] = controlPointList [i].transform.localPosition;
			
			}
		}

	}
	void adjMesh() {
		for (int i = 0; i < controlPointList.Count; i++) {
			//Debug.Log (controlPointList[i].transform.localPosition);
			verts [i] = controlPointList [i].transform.localPosition;
		}
		mesh.vertices = verts;
		mesh.RecalculateBounds (); 
		mesh.RecalculateNormals (); 
	
	}
	public void adjPos(){

		Vector3 middle = Vector3.zero;
		float aa, bb, cc;
		each_tomiddle = new Vector3[6];

		for (int i = 0; i < verts.Length; i++) {
			each_tomiddle[i] = verts [i] - middle;
		}



		 
		for (int i = 0; i < controlPointList.Count; i++) {
			//print ("For loop: " + transform.GetChild (i).gameObject);
			if (dragitemcontroller.chooseObj == controlPointList[i]) {
				Vector3 a = dragitemcontroller.chooseObj.transform.localPosition;//after
				a = a - middle;
				Vector3 b = verts[i] - middle;//before

				aa = a.magnitude;
				bb = b.magnitude;
				cc = aa / bb;     //ratio

				for(int j =0;j<controlPointList.Count;j++){
					controlPointList [j].transform.localPosition = each_tomiddle[j] * cc;
				}



				//print ("controlpos: " + controlPointList [i].transform.localPosition.ToString("f2"));
				//Debug.Log("a:"+a.ToString("f4"));
				//Debug.Log("b:"+b.ToString("f4"));
				//Debug.Log ("a:"+a.ToString("f2"));
				//Debug.Log ("a:"+a);


				//Debug.Log("aa:"+aa.ToString("f4"));
				//Debug.Log("bb:"+bb.ToString("f4"));
				//Debug.Log("cc:"+cc.ToString("f4"));

			}
		}
		for (int i = 0; i < controlPointList.Count; i++) {
			verts [i] = controlPointList [i].transform.localPosition;

		}
		adjMesh();



		//Debug.Log ("FirstPos:" + FirstPos.ToString("f2"));

		//update controller maybe not
		//for (int i = 0; i < controlPointList.Count; i++) {
		//	verts [i] = controlPointList [i].transform.localPosition;
		//	//Debug.Log ("verts:" + i + verts [i]);
		//}
		//for (int i = 0; i < verts.Length; i++) {
		//	each_tomiddle [i] = verts [i] - middle;

		//}
	
	}
}
		
