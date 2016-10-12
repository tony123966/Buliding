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

	List<LineRenderer> lineRenderList = new List<LineRenderer>();
	List<LineRenderer> lineRenderListA = new List<LineRenderer>();
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

		gameObject.GetComponent<MeshRenderer>().sortingOrder=0;
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

					ini_bodydis.x = controlPointList[1].transform.localPosition.x - controlPointList[0].transform.localPosition.x;
					ini_bodydis.y = controlPointList[1].transform.localPosition.y - controlPointList[2].transform.localPosition.y;
					ini_bodydis = ini_bodydis / 2.0f;

					ini_mainRidgedis = controlPointList[4].transform.localPosition.x - controlPointList[5].transform.localPosition.x;
					ini_mainRidgedis = ini_mainRidgedis / 2.0f;

					CreateLineRenderer(controlPointList[5], controlPointList[4]);
					CreateLineRenderer(controlPointList[1], controlPointList[4]);
					CreateLineRenderer(controlPointList[2], controlPointList[4]);
					CreateLineRenderer(controlPointList[0], controlPointList[5]);
					CreateLineRenderer(controlPointList[3], controlPointList[5]);
				}
				else
				{
					edgeIndex = 6;
					mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5 };
					verts = mesh.vertices;
				}
				break;
			case 10://specialCase
				if (gameObject.tag == "Shanding")
				{
					edgeIndex = 4;
					mesh.vertices = new Vector3[] {
					controlPointList [0].transform.localPosition,
					controlPointList [1].transform.localPosition,
					controlPointList [2].transform.localPosition,
					controlPointList [3].transform.localPosition,
					controlPointList [4].transform.localPosition,
					controlPointList [5].transform.localPosition,
					controlPointList [6].transform.localPosition,
					controlPointList [7].transform.localPosition,
					controlPointList [8].transform.localPosition,
					controlPointList [9].transform.localPosition,
					};
					mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
					verts = mesh.vertices;

					ini_bodydis.x = controlPointList[1].transform.localPosition.x - controlPointList[0].transform.localPosition.x;
					ini_bodydis.y = controlPointList[1].transform.localPosition.y - controlPointList[2].transform.localPosition.y;
					ini_bodydis = ini_bodydis / 2.0f;

					ini_mainRidgedis = controlPointList[4].transform.localPosition.x - controlPointList[5].transform.localPosition.x;
					ini_mainRidgedis = ini_mainRidgedis / 2.0f;

					CreateLineRenderer(controlPointList[5], controlPointList[4]);
					CreateLineRenderer(controlPointList[7], controlPointList[4]);
					CreateLineRenderer(controlPointList[8], controlPointList[4]);
					CreateLineRenderer(controlPointList[6], controlPointList[5]);
					CreateLineRenderer(controlPointList[9], controlPointList[5]);

					CreateLineRenderer(controlPointList[0], controlPointList[6]);
					CreateLineRenderer(controlPointList[3], controlPointList[9]);
					CreateLineRenderer(controlPointList[1], controlPointList[7]);
					CreateLineRenderer(controlPointList[2], controlPointList[8]);
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
	public void adjPos()
	{
		Vector3 middle = Vector3.zero;

		chang_bodydis = ratio_bodydis = Vector2.zero;
		chang_mainRidgedis = ratio_mainRidgedis = 0;

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
				float offset_x = tmp.x - verts[i].x;
				float offset_y = tmp.y - verts[i].y;
				if (this.tag == "Rectangle")
				{
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
						if (i == 4)
						{
							controlPointList[5].transform.localPosition = new Vector3(verts[5].x - (offset_x), verts[5].y, verts[5].z);
						}
						else
						{
							controlPointList[4].transform.localPosition = new Vector3(verts[4].x - (offset_x), verts[4].y, verts[4].z);
						}

						chang_mainRidgedis = offset_x;
						ratio_mainRidgedis = chang_mainRidgedis / ini_mainRidgedis;
					}
					UpdateLineRender();
				}
				else if (this.tag == "Shanding")
				{
					if (i < 4)
					{
						for (int j = 0; j < 4; j++)
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
					}
					else if ((i == 4) || (i == 5))//mainRidge
					{
						if (i == 4)
						{
							controlPointList[7].transform.localPosition = new Vector3(controlPointList[4].transform.localPosition.x, controlPointList[7].transform.localPosition.y, controlPointList[7].transform.localPosition.z);
							controlPointList[8].transform.localPosition = new Vector3(controlPointList[4].transform.localPosition.x, controlPointList[8].transform.localPosition.y, controlPointList[8].transform.localPosition.z);
							controlPointList[5].transform.localPosition = new Vector3(verts[5].x - (offset_x), verts[5].y, verts[5].z);
							controlPointList[6].transform.localPosition = new Vector3(controlPointList[5].transform.localPosition.x, controlPointList[6].transform.localPosition.y, controlPointList[6].transform.localPosition.z);
							controlPointList[9].transform.localPosition = new Vector3(controlPointList[5].transform.localPosition.x, controlPointList[9].transform.localPosition.y, controlPointList[9].transform.localPosition.z);
						}
						else
						{
							controlPointList[6].transform.localPosition = new Vector3(controlPointList[5].transform.localPosition.x, controlPointList[6].transform.localPosition.y, controlPointList[6].transform.localPosition.z);
							controlPointList[9].transform.localPosition = new Vector3(controlPointList[5].transform.localPosition.x, controlPointList[9].transform.localPosition.y, controlPointList[9].transform.localPosition.z);
							controlPointList[4].transform.localPosition = new Vector3(verts[4].x - (offset_x), verts[4].y, verts[4].z);
							controlPointList[7].transform.localPosition = new Vector3(controlPointList[4].transform.localPosition.x, controlPointList[7].transform.localPosition.y, controlPointList[7].transform.localPosition.z);
							controlPointList[8].transform.localPosition = new Vector3(controlPointList[4].transform.localPosition.x, controlPointList[8].transform.localPosition.y, controlPointList[8].transform.localPosition.z);
						}
					}
					else 
					{
						for (int j = 6; j < controlPointList.Count; j++)
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
						controlPointList[4].transform.localPosition = new Vector3(controlPointList[7].transform.localPosition.x, controlPointList[4].transform.localPosition.y, controlPointList[4].transform.localPosition.z);
						controlPointList[5].transform.localPosition = new Vector3(controlPointList[6].transform.localPosition.x, controlPointList[5].transform.localPosition.y, controlPointList[5].transform.localPosition.z);
					}
					UpdateLineRender();
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
		else if (this.tag == "Shanding")
		{
			for (int i = 0; i < 4; i++)
			{
				movement.freelist.Add(controlPointList[i]);
			}
			movement.horlist.Add(controlPointList[4]);
			movement.horlist.Add(controlPointList[5]);
			for (int i = 6; i < 10; i++)
			{
				movement.freelist.Add(controlPointList[i]);
			}
		}
		else
		{
			movement.freelist.AddRange(controlPointList);

		}
	}
	public void CreateLineRenderer(GameObject strat, GameObject end)
	{
		GameObject lineObj = new GameObject("Line", typeof(LineRenderer));
		lineObj.transform.parent = transform;
		LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();
		lineRenderer.sortingOrder=1;
		lineRenderer.SetWidth(0.01f, 0.01f);
		lineRenderer.useWorldSpace = true;
		lineRenderer.material.color = Color.black;
		lineRenderer.SetColors(Color.black, Color.black);
		lineRenderer.SetVertexCount(2);
		lineRenderer.SetPosition(0, strat.transform.position);
		lineRenderer.SetPosition(1, end.transform.position);
			if (this.tag == "Rectangle")
		{
		lineRenderList.Add(lineRenderer);
		}
		else if (this.tag == "Shanding")
		{
			lineRenderListA.Add(lineRenderer);
		 }
	}
	public void UpdateLineRender() 
	{
		if (this.tag == "Rectangle")
		{
		AdjLineRenderer(0,controlPointList[5], controlPointList[4]);
		AdjLineRenderer(1,controlPointList[1], controlPointList[4]);
		AdjLineRenderer(2,controlPointList[2], controlPointList[4]);
		AdjLineRenderer(3,controlPointList[0], controlPointList[5]);
		AdjLineRenderer(4,controlPointList[3], controlPointList[5]);
		}
		else if (this.tag == "Shanding")
		{
		AdjLineRenderer(0,controlPointList[5], controlPointList[4]);
		AdjLineRenderer(1,controlPointList[7], controlPointList[4]);
		AdjLineRenderer(2,controlPointList[8], controlPointList[4]);
		AdjLineRenderer(3, controlPointList[6], controlPointList[5]);
		AdjLineRenderer(4, controlPointList[9], controlPointList[5]);

		AdjLineRenderer(5, controlPointList[0], controlPointList[6]);
		AdjLineRenderer(6, controlPointList[3], controlPointList[9]);
		AdjLineRenderer(7, controlPointList[1], controlPointList[7]);
		AdjLineRenderer(8, controlPointList[2], controlPointList[8]);
		}
	}
	public void AdjLineRenderer(int index, GameObject strat, GameObject end) 
	{	if (this.tag == "Rectangle")
		{
		lineRenderList[index].SetPosition(0, strat.transform.position);
		lineRenderList[index].SetPosition(1, end.transform.position);
		}
		else if (this.tag == "Shanding")
		{
			lineRenderListA[index].SetPosition(0, strat.transform.position);
			lineRenderListA[index].SetPosition(1, end.transform.position);
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
			float minWidth = ini_mainRidgedis* 0.8f;
			float minHeight = ini_bodydis.y * 0.5f;
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
			else if (dragitemcontroller.chooseObj == controlPointList[0])//upLeft
			{
				maxClampX = controlPointList[5].transform.position.x;
				minClampY = controlPointList[5].transform.position.y + minHeight;
			}
			else if (dragitemcontroller.chooseObj == controlPointList[3])//downLeft
			{
				maxClampX = controlPointList[5].transform.position.x;
				maxClampY = controlPointList[5].transform.position.y - minHeight;
			}
			else if (dragitemcontroller.chooseObj == controlPointList[1])//upRight
			{
				minClampX = controlPointList[4].transform.position.x;
				minClampY = controlPointList[4].transform.position.y + minHeight;
			}
			else if (dragitemcontroller.chooseObj == controlPointList[2])//downRight
			{
				minClampX = controlPointList[4].transform.position.x;
				maxClampY = controlPointList[4].transform.position.y - minHeight;
			}

		}
		else if (this.tag == "Shanding")
		{
			float minWidth = ini_mainRidgedis * 0.8f;
			float minHeight = ini_bodydis.y*0.5f*0.5f;
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
			else if (dragitemcontroller.chooseObj == controlPointList[0])//upLeft
			{
				maxClampX = controlPointList[4].transform.position.x - minWidth;
				minClampY = controlPointList[6].transform.position.y + minHeight;
			}
			else if (dragitemcontroller.chooseObj == controlPointList[3])//downLeft
			{
				maxClampX = controlPointList[4].transform.position.x - minWidth;
				maxClampY = controlPointList[9].transform.position.y - minHeight;
			}
			else if (dragitemcontroller.chooseObj == controlPointList[1])//upRight
			{
				minClampX = controlPointList[7].transform.position.x;
				minClampY = controlPointList[7].transform.position.y + minHeight;
			}
			else if (dragitemcontroller.chooseObj == controlPointList[2])//downRight
			{
				minClampX = controlPointList[8].transform.position.x;
				maxClampY = controlPointList[8].transform.position.y - minHeight;
			}
			else if (dragitemcontroller.chooseObj == controlPointList[6])//upLeftCenter
			{
				minClampX = controlPointList[0].transform.position.x;
				maxClampX = controlPointList[5].transform.position.x;
				minClampY = controlPointList[5].transform.position.y + minHeight;
				maxClampY = controlPointList[0].transform.position.y - minHeight;
			}
			else if (dragitemcontroller.chooseObj == controlPointList[9])//downLeftCenter
			{
				minClampX = controlPointList[3].transform.position.x;
				maxClampX = controlPointList[5].transform.position.x;
				minClampY = controlPointList[3].transform.position.y + minHeight;
				maxClampY = controlPointList[5].transform.position.y - minHeight;

			}
			else if (dragitemcontroller.chooseObj == controlPointList[7])//upRightCenter
			{
				minClampX = controlPointList[5].transform.position.x + minWidth;
				maxClampX = controlPointList[1].transform.position.x;
				minClampY = controlPointList[4].transform.position.y + minHeight;
				maxClampY = controlPointList[1].transform.position.y - minHeight;
			}
			else if (dragitemcontroller.chooseObj == controlPointList[8])//downRightCenter
			{
				minClampX = controlPointList[5].transform.position.x + minWidth;
				maxClampX = controlPointList[2].transform.position.x;
				minClampY = controlPointList[2].transform.position.y + minHeight;
				maxClampY = controlPointList[4].transform.position.y - minHeight;
			}
		}
		float posX = Mathf.Clamp(inputPos.x, minClampX, maxClampX);
		float posY = Mathf.Clamp(inputPos.y, minClampY, maxClampY);
		return new Vector3(posX, posY, inputPos.z);
	}
}

