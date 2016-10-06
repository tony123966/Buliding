/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MeshObj : MonoBehaviour
{
	public List<GameObject> controlPointList = new List<GameObject> ();
	public List<GameObject> horizontal_List = new List<GameObject> ();
	// Use this for initialization
	private GameObject chooseOBJ = null;
	Mesh mesh;
	Vector3[] verts;
	Vector3[] each_tomiddle;
	Vector3 vertPos;
	Vector3 FirstPos;

	public DragItemController dragitemcontroller;
	public Movement movement;

	public int edgei;

	Vector2 ini_bodydis;
	float ini_mainRidgedis;

	Vector2 chang_bodydis;
	float chang_mainRidgedis;
	Vector2 ratio_bodydis;
	float ratio_mainRidgedis;
	float half_mainRidgedis; 
	void Start(){

		dragitemcontroller = GameObject.Find ("DragItemController").GetComponent<DragItemController> ();
	}

	void Awake () 
	{
		
		mesh = GetComponent<MeshFilter>().mesh;
		movement = GameObject.Find ("Movement").GetComponent<Movement> ();

		if (!gameObject.GetComponent<MeshFilter>()) gameObject.AddComponent<MeshFilter>();
		if (!gameObject.GetComponent<MeshRenderer>()) gameObject.AddComponent<MeshRenderer>();
        //mesh.Clear();


		//rectangle 
		if (controlPointList.Count == 6 && this.tag == "Rectangle") {
			//ting_model = 1;
			mesh.vertices = new Vector3[] {
				controlPointList [0].transform.localPosition,
				controlPointList [1].transform.localPosition,
				controlPointList [2].transform.localPosition,
				controlPointList [3].transform.localPosition,
				controlPointList [4].transform.localPosition,
				controlPointList [5].transform.localPosition
			};

			mesh.triangles = new int[]{ 0, 1, 2, 0, 2, 3 }; 
			verts = mesh.vertices;
			for (int i = 0; i < controlPointList.Count - 2; i++) {
				movement.freelist.Add (controlPointList [i]);
			}
			movement.horlist.Add (controlPointList [4]);
			movement.horlist.Add (controlPointList [5]);
			edgei = 4;


			ini_bodydis.x = controlPointList[0].transform.localPosition.x - controlPointList[1].transform.localPosition.x;
			ini_bodydis.y = controlPointList[0].transform.localPosition.y - controlPointList[3].transform.localPosition.y;
			ini_mainRidgedis = controlPointList[5].transform.localPosition.x - controlPointList[4].transform.localPosition.x;
			//store first 
		} else if (controlPointList.Count == 4) {
			//ting_model = 2;
			mesh.vertices = new Vector3[] {
				controlPointList [0].transform.localPosition,
				controlPointList [1].transform.localPosition,
				controlPointList [2].transform.localPosition,
				controlPointList [3].transform.localPosition,
			};

			//mesh.uv = new Vector2[] {new Vector2 (0, 0),  new Vector2 (0, 1), new Vector2 (1, 1)};
			mesh.triangles = new int[]{ 0, 1, 2, 0, 3, 1 }; 
			verts = mesh.vertices;
			//store first 
			movement.freelist.AddRange(controlPointList);
			edgei=4;

		} else if (controlPointList.Count == 6) {
			//ting_model = 4;
			mesh.vertices = new Vector3[] {
				controlPointList [0].transform.localPosition,
				controlPointList [1].transform.localPosition,
				controlPointList [2].transform.localPosition,
				controlPointList [3].transform.localPosition,
				controlPointList [4].transform.localPosition,
				controlPointList [5].transform.localPosition
			};
			//mesh.uv = new Vector2[] {new Vector2 (0, 0),  new Vector2 (0, 1), new Vector2 (1, 1)};
			mesh.triangles = new int[]{ 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5 }; 
			verts = mesh.vertices;
			//store first 
			movement.freelist.AddRange(controlPointList);
			edgei=6;

		

		} else if (controlPointList.Count == 5) {
			//ting_model = 3;
			mesh.vertices = new Vector3[] {
				controlPointList [0].transform.localPosition,
				controlPointList [1].transform.localPosition,
				controlPointList [2].transform.localPosition,
				controlPointList [3].transform.localPosition,
				controlPointList [4].transform.localPosition,
			};
			mesh.triangles = new int[]{ 0, 1, 2, 0, 2, 3, 0, 3, 4 }; 
			verts = mesh.vertices;
			movement.freelist.AddRange(controlPointList);
			edgei=5;
		}else if (controlPointList.Count == 3) {
			//ting_model = 5;
			mesh.vertices = new Vector3[] {
				controlPointList [0].transform.localPosition,
				controlPointList [1].transform.localPosition,
				controlPointList [2].transform.localPosition,

			};
			mesh.triangles = new int[]{ 0, 1, 2}; 
			verts = mesh.vertices;
			movement.freelist.AddRange(controlPointList);
			edgei=3;
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
	public void adjPos()
	{

		Vector3 middle = Vector3.zero;
		float aa, bb, cc;
		each_tomiddle = new Vector3[6];

		for (int i = 0; i < verts.Length; i++) {
			each_tomiddle [i] = verts [i] - middle;
		}	
		for (int i = 0; i < controlPointList.Count; i++) {
			if (dragitemcontroller.chooseObj == controlPointList [i]) { 
				if (controlPointList.Count == 6 && this.tag == "Rectangle") {//廡殿	
					Vector2 dis =Vector2.zero;		
					if (i < 4) {//1. find offset_x, offset_y of mouse position and original position. 
						float offset_x, offset_y;
						Vector3 mouse_pos = dragitemcontroller.chooseObj.transform.localPosition;	
						Vector3 ori_mouse_pos = verts [i]; //找x和他一樣的
						offset_x = mouse_pos.x - ori_mouse_pos.x;
						offset_y = mouse_pos.y - ori_mouse_pos.y;
						for (int j = 0; j < controlPointList.Count - 2; j++) {
							if ((ori_mouse_pos.x == controlPointList [j].transform.localPosition.x) && (i != j)) { // * i->choose obj * ////2.adjust other point.
								controlPointList [j].transform.localPosition =
								new Vector3 (mouse_pos.x, 
									verts [j].y - (offset_y), 
									verts [j].z);
							} else if ((ori_mouse_pos.y == controlPointList [j].transform.localPosition.y) && (i != j)) {
								controlPointList [j].transform.localPosition =
							new Vector3 (verts [j].x - (offset_x), 
									mouse_pos.y, 
									verts [j].z);
							} else if ((i != j) && (i < 4)) {
								controlPointList [j].transform.localPosition = 
								new Vector3 (verts [j].x - (offset_x),
									verts [j].y - (offset_y),
									verts [j].z);
							}
						}

					} else {
						float offset_x;
						Vector3 mouse_pos = dragitemcontroller.chooseObj.transform.localPosition;	
						Vector3 ori_mouse_pos = verts [i]; //找x和他一樣的
						offset_x = mouse_pos.x - ori_mouse_pos.x;
						if (i == 4) {
							float a = Mathf.Clamp (verts [5].x - (offset_x), controlPointList [0].transform.localPosition.x, controlPointList [1].transform.localPosition.x);
							controlPointList [5].transform.localPosition =	new Vector3 (a, verts [5].y, verts [5].z);
						} else {
							float b = Mathf.Clamp (verts [4].x - (offset_x), controlPointList [0].transform.localPosition.x, controlPointList [1].transform.localPosition.x);
							controlPointList [4].transform.localPosition = new Vector3 (b, verts [4].y, verts [4].z);

						}
					}//rectangle end
					if (dragitemcontroller.chooseObj == controlPointList[0])
					{
					
					}
					 chang_bodydis.x = dis.x;
					 chang_bodydis.y = dis.y;
					 chang_mainRidgedis = controlPointList[5].transform.localPosition.x - controlPointList[4].transform.localPosition.x;

					 ratio_bodydis.x= chang_bodydis.x/ini_bodydis.x;
					 ratio_bodydis.y = chang_bodydis.y / ini_bodydis.y;
					 ratio_mainRidgedis = chang_mainRidgedis/ini_mainRidgedis;
				} else {//六角亭
					Vector3 a = dragitemcontroller.chooseObj.transform.localPosition;//after
					a = a - middle;
					Vector3 b = verts [i] - middle;//before
					aa = a.magnitude;
					bb = b.magnitude;
					cc = aa / bb;     //ratio
					for (int j = 0; j < controlPointList.Count; j++) {
						controlPointList [j].transform.localPosition = each_tomiddle [j] * cc;
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



	public void addpoint(){
		float count = controlPointList.Count;
		if (this.tag == "Rectangle") {
			for (int i = 0; i < controlPointList.Count - 2; i++) {
				movement.freelist.Add (controlPointList [i]);
			}
			movement.horlist.Add (controlPointList [4]);
			movement.horlist.Add (controlPointList [5]);
		} else {
			movement.freelist.AddRange(controlPointList);

		}
	}
}

*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MeshObj : MonoBehaviour
{
	public List<GameObject> controlPointList = new List<GameObject>();

	private Mesh mesh;
	private Vector3[] verts;
	private Vector3[] vectors2Center;

	private DragItemController dragitemcontroller;
	private Movement movement;

	public int edgeIndex;

	Vector2 ini_bodydis;
	float ini_mainRidgedis;

	Vector2 chang_bodydis;
	float chang_mainRidgedis;
	Vector2 ratio_bodydis;
	float ratio_mainRidgedis;

	void Start()
	{
		dragitemcontroller = GameObject.Find("DragItemController").GetComponent<DragItemController>();
	}

	void Awake()
	{

		mesh = GetComponent<MeshFilter>().mesh;
		movement = GameObject.Find("Movement").GetComponent<Movement>();

		if (!gameObject.GetComponent<MeshFilter>()) gameObject.AddComponent<MeshFilter>();
		if (!gameObject.GetComponent<MeshRenderer>()) gameObject.AddComponent<MeshRenderer>();
		switch (controlPointList.Count)
		{
			case 3:
				edgeIndex = 3;
				mesh.vertices = new Vector3[] {
				controlPointList [0].transform.localPosition,
				controlPointList [1].transform.localPosition,
				controlPointList [2].transform.localPosition,
			};
				mesh.triangles = new int[] { 0, 1, 2 };
				verts = mesh.vertices;
				break;
			case 4:
				edgeIndex = 4;
				mesh.vertices = new Vector3[] {
				controlPointList [0].transform.localPosition,
				controlPointList [1].transform.localPosition,
				controlPointList [2].transform.localPosition,
				controlPointList [3].transform.localPosition,
			};

				mesh.triangles = new int[] { 0, 1, 2, 0, 3, 1 };
				verts = mesh.vertices;

				break;
			case 5:
				edgeIndex = 5;
				mesh.vertices = new Vector3[] {
				controlPointList [0].transform.localPosition,
				controlPointList [1].transform.localPosition,
				controlPointList [2].transform.localPosition,
				controlPointList [3].transform.localPosition,
				controlPointList [4].transform.localPosition,
			};
				mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4 };
				verts = mesh.vertices;
				break;
			case 6://specialCase
				mesh.vertices = new Vector3[] {
				controlPointList [0].transform.localPosition,
				controlPointList [1].transform.localPosition,
				controlPointList [2].transform.localPosition,
				controlPointList [3].transform.localPosition,
				controlPointList [4].transform.localPosition,
				controlPointList [5].transform.localPosition
			};
				if (gameObject.tag == "Rectangle")
				{
					edgeIndex = 4;
					mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
					verts = mesh.vertices;
					for (int i = 0; i < controlPointList.Count - 2; i++)
					{
						movement.freelist.Add(controlPointList[i]);
					}

					ini_bodydis.x = controlPointList[1].transform.localPosition.x - controlPointList[0].transform.localPosition.x;
					ini_bodydis.y = controlPointList[1].transform.localPosition.y - controlPointList[2].transform.localPosition.y;
					ini_bodydis = ini_bodydis / 2.0f;

					ini_mainRidgedis = controlPointList[4].transform.localPosition.x - controlPointList[5].transform.localPosition.x;
					ini_mainRidgedis = ini_mainRidgedis / 2.0f;
				}
				else
				{
					edgeIndex = 6;
					mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5 };
					verts = mesh.vertices;
				}
				break;

		}
		addpoint();
	}
	void adjMesh()
	{
		for (int i = 0; i < controlPointList.Count; i++)
		{
			verts[i] = controlPointList[i].transform.localPosition;
		}
		mesh.vertices = verts;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

	}
	bool CheckSameDir()
	{

		return false;
	}
	public void adjPos()
	{
		Vector3 middle = Vector3.zero;

		chang_bodydis = ratio_bodydis = Vector2.zero;
		chang_mainRidgedis =ratio_mainRidgedis= 0;
		
		vectors2Center = new Vector3[verts.Length];

		for (int i = 0; i < verts.Length; i++)
		{
			vectors2Center[i] = verts[i] - middle;
		}

		for (int i = 0; i < controlPointList.Count; i++)
		{
			if (dragitemcontroller.chooseObj == controlPointList[i])
			{
				Vector3 tmp = dragitemcontroller.chooseObj.transform.localPosition;
				if (this.tag == "Rectangle")
				{
					float offset_x, offset_y;
					offset_x = tmp.x - verts[i].x;
					offset_y = tmp.y - verts[i].y;
					if (i < 4)
					{
						for (int j = 0; j < controlPointList.Count - 2; j++)
						{
							if (i == j) continue;
							if ((verts[i].x == controlPointList[j].transform.localPosition.x))//x一樣的點
							{
								controlPointList[j].transform.localPosition = new Vector3(tmp.x, verts[j].y - (offset_y), verts[j].z);
							}
							else if ((verts[i].y == controlPointList[j].transform.localPosition.y))//y一樣的點
							{
								controlPointList[j].transform.localPosition = new Vector3(verts[j].x - (offset_x), tmp.y, verts[j].z);
							}
							else//對角的點
							{
								controlPointList[j].transform.localPosition = new Vector3(verts[j].x - (offset_x), verts[j].y - (offset_y), verts[j].z);
							}
						}
						chang_bodydis.x = offset_x;
						chang_bodydis.y = offset_y;
						ratio_bodydis.x = chang_bodydis.x / ini_bodydis.x;
						ratio_bodydis.y = chang_bodydis.y / ini_bodydis.y;
					}
					else//mainRidge
					{
						float minClampX = controlPointList[0].transform.localPosition.x;
						float maxClampX = controlPointList[1].transform.localPosition.x;
						if (i == 4)
						{
							float a = Mathf.Clamp(verts[5].x - (offset_x), minClampX, maxClampX);
							controlPointList[5].transform.localPosition = new Vector3(a, verts[5].y, verts[5].z);
						}
						else
						{
							float b = Mathf.Clamp(verts[4].x - (offset_x), minClampX, maxClampX);
							controlPointList[4].transform.localPosition = new Vector3(b, verts[4].y, verts[4].z);

						}

						chang_mainRidgedis = offset_x;
						ratio_mainRidgedis = chang_mainRidgedis / ini_mainRidgedis;
					}
		
				}
				else//縮放
				{
					Vector3 a = tmp - middle;//now
					Vector3 b = vectors2Center[i];//before
					float aa = a.magnitude;
					float bb = b.magnitude;
					float cc = aa / bb;     //ratio
					for (int j = 0; j < controlPointList.Count; j++)
					{
						controlPointList[j].transform.localPosition = vectors2Center[j] * cc;
					}
				}
				break;
			}
		}
		//Update
		for (int i = 0; i < controlPointList.Count; i++)
		{
			verts[i] = controlPointList[i].transform.localPosition;
		}

		adjMesh();
	}
	public void addpoint()
	{
		if (this.tag == "Rectangle")
		{
			for (int i = 0; i < controlPointList.Count - 2; i++)
			{
				movement.freelist.Add(controlPointList[i]);
			}
			movement.horlist.Add(controlPointList[4]);
			movement.horlist.Add(controlPointList[5]);
		}
		else
		{
			movement.freelist.AddRange(controlPointList);

		}
	}
	public Vector3 ClampPos(Vector3 inputPos)
	{
		float minClampX = float.MinValue;
		float maxClampX = float.MaxValue;
		float minClampY = float.MinValue;
		float maxClampY = float.MaxValue;
		if (this.tag == "Rectangle")
		{
			float minWidth = ini_bodydis.x * 0.2f;
			float minHeight = ini_bodydis.y * 0.2f;

			if (dragitemcontroller.chooseObj == controlPointList[4])//rightMainRidge
			{
				minClampX = controlPointList[5].transform.position.x + minWidth;
				maxClampX = controlPointList[1].transform.position.x;
			}
			else if (dragitemcontroller.chooseObj == controlPointList[5])//leftMainRidge
			{
				minClampX = controlPointList[0].transform.position.x;
				maxClampX = controlPointList[4].transform.position.x - minWidth;
			}
			else if (dragitemcontroller.chooseObj == controlPointList[0] )//upLeft
			{
				maxClampX = controlPointList[5].transform.position.x;
				minClampY = controlPointList[3].transform.position.y + minHeight;
			}
			else if ( dragitemcontroller.chooseObj == controlPointList[3])//downLeft
			{
				maxClampX = controlPointList[5].transform.position.x;
				maxClampY = controlPointList[0].transform.position.y - minHeight;
			}
			else if (dragitemcontroller.chooseObj == controlPointList[1] )//upRight
			{
				minClampX = controlPointList[4].transform.position.x;
				minClampY = controlPointList[2].transform.position.y + minHeight;
			}
			else if (dragitemcontroller.chooseObj == controlPointList[2])//downRight
			{
				minClampX = controlPointList[4].transform.position.x;
				maxClampY = controlPointList[1].transform.position.y - minHeight;
			}

			float posX = Mathf.Clamp(inputPos.x, minClampX, maxClampX);
			float posY = Mathf.Clamp(inputPos.y, minClampY, maxClampY);
			return new Vector3(posX, posY, inputPos.z);
		}
		return inputPos;
	}
}

