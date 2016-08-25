using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MeshObj : MonoBehaviour
{
	public List<GameObject> controlPointList = new List<GameObject> ();
	public List<GameObject> horizontal_List = new List<GameObject> ();
	public List<GameObject> testlist = new List<GameObject> ();
	// Use this for initialization
	public GameObject chooseOBJ = null;
	Mesh mesh;
	Vector3[] verts;
	Vector3[] each_tomiddle;
	Vector3 vertPos;
	Vector3 FirstPos;

	public DragItemController dragitemcontroller;
	public Movement movement;
	public Material lineMat;
//	public GameObject cp1;
//	public GameObject cp2;
	public Material[] mats;

	public int lengthOfLineRenderer = 6;
	LineRenderer lineRenderer;




	void Start(){

		dragitemcontroller = GameObject.Find ("DragItemController").GetComponent<DragItemController> ();
		//lineRenderer = GetComponent<LineRenderer> ();

	}

	void Awake () 
	{
		
		mesh = GetComponent<MeshFilter>().mesh;
		movement = GameObject.Find ("Movement").GetComponent<Movement> ();


		if (!gameObject.GetComponent<MeshFilter>()) gameObject.AddComponent<MeshFilter>();
		if (!gameObject.GetComponent<MeshRenderer>()) gameObject.AddComponent<MeshRenderer>();
        //mesh.Clear();


		//rectangle 
		if (controlPointList.Count ==  6 && this.tag == "Rectangle")
		{
			mesh.vertices = new Vector3[] {
				controlPointList [0].transform.localPosition,
				controlPointList [1].transform.localPosition,
				controlPointList [2].transform.localPosition,
				controlPointList [3].transform.localPosition,
				controlPointList [4].transform.localPosition,
				controlPointList [5].transform.localPosition
			};
//			mesh.uv = new Vector2[] {
//				new Vector2 (0, 1),
//				new Vector2 (1, 1),
//				new Vector2 (0, 0),
//				new Vector2 (1, 0) };




			mesh.triangles =  new int[]{0, 1, 2,0,2,3}; 
			verts = mesh.vertices;
			for (int i = 0; i < controlPointList.Count-2; i++) {
				movement.freelist.Add (controlPointList [i]);
			}
			movement.horlist.Add(controlPointList[4]);
			movement.horlist.Add(controlPointList[5]);

			//store first 
		}

		else if (controlPointList.Count ==  4)
		{
				mesh.vertices = new Vector3[] {
				controlPointList [0].transform.localPosition,
				controlPointList [1].transform.localPosition,
				controlPointList [2].transform.localPosition,
				controlPointList [3].transform.localPosition,
			};

			//mesh.uv = new Vector2[] {new Vector2 (0, 0),  new Vector2 (0, 1), new Vector2 (1, 1)};
			mesh.triangles =  new int[]{0, 1, 2,0,3,1}; 
			verts = mesh.vertices;
			//store first 
			movement.freelist = controlPointList;

		}
		else if (controlPointList.Count ==  6)
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
			mesh.triangles =  new int[]{0,1,2,0,2,3,0,3,4,0,4,5}; 
			verts = mesh.vertices;
			//store first 
			movement.freelist = controlPointList;

		}

		//還有五


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
		//Drawline ();

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
			each_tomiddle [i] = verts [i] - middle;
		}	
		for (int i = 0; i < controlPointList.Count; i++) {
			if (dragitemcontroller.chooseObj == controlPointList [i]) { 
				
				if (controlPointList.Count == 6  && this.tag == "Rectangle") {
					//regtangle and its not middle point

					//1. find offset_x, offset_y of mouse position and original position. 
					float offset_x, offset_y;

					Vector3 mouse_pos = dragitemcontroller.chooseObj.transform.localPosition;	
					Vector3 ori_mouse_pos = verts [i]; //找x和他一樣的

					offset_x = mouse_pos.x - ori_mouse_pos.x;
					offset_y = mouse_pos.y - ori_mouse_pos.y;
//					print ("offset_x:" + offset_x);
//					print ("offset_y:" + offset_y);

//					offset_x = Mathf.Abs (offset_x);
//					offset_y = Mathf.Abs (offset_y);
//					print ("a__offset_x:" + offset_x);
//					print ("a__offset_y:" + offset_y);

//					2.adjust other point.
					for (int j = 0; j < controlPointList.Count-2; j++) {
						if ((ori_mouse_pos.x == controlPointList [j].transform.localPosition.x) && (i!=j)) { //* i->choose obj *//
							
							//print (j +" x thesame");
							controlPointList [j].transform.localPosition =
								new Vector3 (mouse_pos.x, 
									verts [j].y - (offset_y), 
									verts [j].z);
						} else if ((ori_mouse_pos.y == controlPointList [j].transform.localPosition.y) && (i!=j)) {
							//print (j +" y thesame");
							controlPointList [j].transform.localPosition =
							new Vector3 (verts [j].x - (offset_x), 
								mouse_pos.y, 
								verts [j].z);
						} else if((i!=j) && (i < 4)){
							//print (j +" no thesame");
							controlPointList [j].transform.localPosition = 
								new Vector3 (verts [j].x - (offset_x),
									verts [j].y - (offset_y),
									verts [j].z);

						}
					}


					//move horizontal point and right point first
					//float width = Vector3.Distance(controlPointList[0].transform.localPosition,controlPointList[1].transform.localPosition);
					//float height = Vector3.Distance(controlPointList[0].transform.localPosition,controlPointList[2].transform.localPosition);
					//print ("width:" + width);
					//print ("height: " + height);


//					horizontal_List [0].transform.localPosition = new Vector3 (controlPointList [0].transform.localPosition.x, controlPointList [0].transform.localPosition.y - height / 2, controlPointList [0].transform.localPosition.z);
//					horizontal_List [1].transform.localPosition = new Vector3 (controlPointList [1].transform.localPosition.x, controlPointList [1].transform.localPosition.y - height / 2, controlPointList [1].transform.localPosition.z);
//						}
//					} else {
//						Vector3 a = dragitemcontroller.chooseObj.transform.localPosition;
//						Vector3 b = verts [i]; //找x和他一樣的
//						for (int j = 0; j < controlPointList.Count; j++) {
//							if (b.y == controlPointList [j].transform.localPosition.y) {
//								controlPointList [j].transform.localPosition = new Vector3 (controlPointList [j].transform.localPosition.x, a.y, controlPointList [j].transform.localPosition.z);
//							}
//				}

				} else {
					Vector3 a = dragitemcontroller.chooseObj.transform.localPosition;//after
					a = a - middle;
					Vector3 b = verts [i] - middle;//before
					aa = a.magnitude;
					bb = b.magnitude;
					cc = aa / bb;     //ratio
					for (int j = 0; j < controlPointList.Count; j++) {
						controlPointList[j].transform.localPosition = each_tomiddle [j] * cc;
					}

				}
			}
		}
		for (int x = 0; x < controlPointList.Count; x++) {
			verts [x] = controlPointList [x].transform.localPosition;
		}


//		horizontal_List[0] = controlPointList [4];
//		horizontal_List[1] = controlPointList [5];

		adjMesh ();
	}

	public void Drawline(){

		lineRenderer.SetVertexCount (controlPointList.Count);
		lineRenderer.SetWidth (0.01f, 0.01f);
		for (int i = 0; i < 5; i++) {
			lineRenderer.SetPosition (i, controlPointList [i].transform.localPosition);			
		}
	}
}



		//Debug.Log ("FirstPos:" + FirstPos.ToString("f2"));

		//update controller maybe not
		//for (int i = 0; i < controlPointList.Count; i++) {
		//	verts [i] = controlPointList [i].transform.localPosition;
		//	//Debug.Log ("verts:" + i + verts [i]);
		//}
		//for (int i = 0; i < verts.Length; i++) {
		//	each_tomiddle [i] = verts [i] - middle;

		//}
	



		//Drawline ();



		//Debug.Log ("FirstPos:" + FirstPos.ToString("f2"));

		//update controller maybe not
		//for (int i = 0; i < controlPointList.Count; i++) {
		//	verts [i] = controlPointList [i].transform.localPosition;
		//	//Debug.Log ("verts:" + i + verts [i]);
		//}
		//for (int i = 0; i < verts.Length; i++) {
		//	each_tomiddle [i] = verts [i] - middle;

		//}
	
	


		
