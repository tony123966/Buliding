/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class IconObject
{
	public int edgeIndex;
}
public class RectangleIcon:IconObject
{
	void RectangleIcon() 
	{ 
		 edgeIndex = 4;
	}

}
public class TriangleIcon:IconObject
{
	void TriangleIcon()
	{
		edgeIndex = 3;
	}

}
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
				controlPointList [0].transform.position,
				controlPointList [1].transform.position,
				controlPointList [2].transform.position,
			};
				mesh.triangles = new int[] { 0, 1, 2 };
				verts = mesh.vertices;
				break;
			case 4:
				edgeIndex = 4;
				mesh.vertices = new Vector3[] {
				controlPointList [0].transform.position,
				controlPointList [1].transform.position,
				controlPointList [2].transform.position,
				controlPointList [3].transform.position,
			};

				mesh.triangles = new int[] { 0, 1, 2, 0, 3, 1 };
				verts = mesh.vertices;

				break;
			case 5:
				edgeIndex = 5;
				mesh.vertices = new Vector3[] {
				controlPointList [0].transform.position,
				controlPointList [1].transform.position,
				controlPointList [2].transform.position,
				controlPointList [3].transform.position,
				controlPointList [4].transform.position,
			};
				mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4 };
				verts = mesh.vertices;
				break;
			case 6://specialCase
				mesh.vertices = new Vector3[] {
					controlPointList [0].transform.position,
					controlPointList [1].transform.position,
					controlPointList [2].transform.position,
					controlPointList [3].transform.position,
					controlPointList [4].transform.position,
					controlPointList [5].transform.position
					};

				if (gameObject.tag == "Rectangle")
				{
					edgeIndex = 4;
					mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
					verts = mesh.vertices;

					ini_bodydis.x = controlPointList[1].transform.position.x - controlPointList[0].transform.position.x;
					ini_bodydis.y = controlPointList[1].transform.position.y - controlPointList[2].transform.position.y;
					ini_bodydis = ini_bodydis / 2.0f;

					ini_mainRidgedis = controlPointList[4].transform.position.x - controlPointList[5].transform.position.x;
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
					controlPointList [0].transform.position,
					controlPointList [1].transform.position,
					controlPointList [2].transform.position,
					controlPointList [3].transform.position,
					controlPointList [4].transform.position,
					controlPointList [5].transform.position,
					controlPointList [6].transform.position,
					controlPointList [7].transform.position,
					controlPointList [8].transform.position,
					controlPointList [9].transform.position,
					};
					mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
					verts = mesh.vertices;

					ini_bodydis.x = controlPointList[1].transform.position.x - controlPointList[0].transform.position.x;
					ini_bodydis.y = controlPointList[1].transform.position.y - controlPointList[2].transform.position.y;
					ini_bodydis = ini_bodydis / 2.0f;

					ini_mainRidgedis = controlPointList[4].transform.position.x - controlPointList[5].transform.position.x;
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
			verts[i] = controlPointList[i].transform.position;
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
				Vector3 tmp = dragitemcontroller.chooseObj.transform.position;
				float offset_x = tmp.x - verts[i].x;
				float offset_y = tmp.y - verts[i].y;
				if (this.tag == "Rectangle")
				{
					if (i < 4)
					{
						for (int j = 0; j < controlPointList.Count - 2; j++)
						{
							if (i == j) continue;
							if ((verts[i].x == controlPointList[j].transform.position.x))//x一樣的點
							{
								controlPointList[j].transform.position = new Vector3(tmp.x, verts[j].y - (offset_y), verts[j].z);
							}
							else if ((verts[i].y == controlPointList[j].transform.position.y))//y一樣的點
							{
								controlPointList[j].transform.position = new Vector3(verts[j].x - (offset_x), tmp.y, verts[j].z);
							}
							else//對角的點
							{
								controlPointList[j].transform.position = new Vector3(verts[j].x - (offset_x), verts[j].y - (offset_y), verts[j].z);
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
							controlPointList[5].transform.position = new Vector3(verts[5].x - (offset_x), verts[5].y, verts[5].z);
						}
						else
						{
							controlPointList[4].transform.position = new Vector3(verts[4].x - (offset_x), verts[4].y, verts[4].z);
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
							if ((verts[i].x == controlPointList[j].transform.position.x))//x一樣的點
							{
								controlPointList[j].transform.position = new Vector3(tmp.x, verts[j].y - (offset_y), verts[j].z);
							}
							else if ((verts[i].y == controlPointList[j].transform.position.y))//y一樣的點
							{
								controlPointList[j].transform.position = new Vector3(verts[j].x - (offset_x), tmp.y, verts[j].z);
							}
							else//對角的點
							{
								controlPointList[j].transform.position = new Vector3(verts[j].x - (offset_x), verts[j].y - (offset_y), verts[j].z);
							}
						}
					}
					else if ((i == 4) || (i == 5))//mainRidge
					{
						if (i == 4)
						{
							controlPointList[7].transform.position = new Vector3(controlPointList[4].transform.position.x, controlPointList[7].transform.position.y, controlPointList[7].transform.position.z);
							controlPointList[8].transform.position = new Vector3(controlPointList[4].transform.position.x, controlPointList[8].transform.position.y, controlPointList[8].transform.position.z);
							controlPointList[5].transform.position = new Vector3(verts[5].x - (offset_x), verts[5].y, verts[5].z);
							controlPointList[6].transform.position = new Vector3(controlPointList[5].transform.position.x, controlPointList[6].transform.position.y, controlPointList[6].transform.position.z);
							controlPointList[9].transform.position = new Vector3(controlPointList[5].transform.position.x, controlPointList[9].transform.position.y, controlPointList[9].transform.position.z);
						}
						else
						{
							controlPointList[6].transform.position = new Vector3(controlPointList[5].transform.position.x, controlPointList[6].transform.position.y, controlPointList[6].transform.position.z);
							controlPointList[9].transform.position = new Vector3(controlPointList[5].transform.position.x, controlPointList[9].transform.position.y, controlPointList[9].transform.position.z);
							controlPointList[4].transform.position = new Vector3(verts[4].x - (offset_x), verts[4].y, verts[4].z);
							controlPointList[7].transform.position = new Vector3(controlPointList[4].transform.position.x, controlPointList[7].transform.position.y, controlPointList[7].transform.position.z);
							controlPointList[8].transform.position = new Vector3(controlPointList[4].transform.position.x, controlPointList[8].transform.position.y, controlPointList[8].transform.position.z);
						}
					}
					else 
					{
						for (int j = 6; j < controlPointList.Count; j++)
						{
							if (i == j) continue;
							if ((verts[i].x == controlPointList[j].transform.position.x))//x一樣的點
							{
								controlPointList[j].transform.position = new Vector3(tmp.x, verts[j].y - (offset_y), verts[j].z);
							}
							else if ((verts[i].y == controlPointList[j].transform.position.y))//y一樣的點
							{
								controlPointList[j].transform.position = new Vector3(verts[j].x - (offset_x), tmp.y, verts[j].z);
							}
							else//對角的點
							{
								controlPointList[j].transform.position = new Vector3(verts[j].x - (offset_x), verts[j].y - (offset_y), verts[j].z);
							}
						}
						controlPointList[4].transform.position = new Vector3(controlPointList[7].transform.position.x, controlPointList[4].transform.position.y, controlPointList[4].transform.position.z);
						controlPointList[5].transform.position = new Vector3(controlPointList[6].transform.position.x, controlPointList[5].transform.position.y, controlPointList[5].transform.position.z);
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
						controlPointList[j].transform.position = vectors2Center[j] * cc;
					}
				}
				break;
			}
		}
		//Update
		for (int i = 0; i < controlPointList.Count; i++)
		{
			verts[i] = controlPointList[i].transform.position;
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

*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class lineRendererControl
{
	public List<lineStruct> lineRenderList = new List<lineStruct>();
	float lineWidth = 0.01f;
	public struct lineStruct
	{
		public GameObject startControlPoint;
		public GameObject endControlPoint;
		public LineRenderer lineRenderer;
		public GameObject lineObj;
	}
	public void CreateLineRenderer<T>(T thisGameObject, GameObject strat, GameObject end) where T : Component
	{
		GameObject lineObj = new GameObject("Line", typeof(LineRenderer));
		lineObj.transform.parent = thisGameObject.transform;
		LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();
		lineRenderer.sortingOrder = 1;
		lineRenderer.SetWidth(lineWidth, lineWidth);
		lineRenderer.useWorldSpace = true;
		lineRenderer.material.color = Color.black;
		lineRenderer.SetColors(Color.black, Color.black);
		lineRenderer.SetVertexCount(2);
		lineRenderer.SetPosition(0, strat.transform.position);
		lineRenderer.SetPosition(1, end.transform.position);

		lineStruct tmp = new lineStruct();
		tmp.startControlPoint = strat;
		tmp.endControlPoint = end;
		tmp.lineRenderer = lineRenderer;
		tmp.lineObj = lineObj;
		lineRenderList.Add(tmp);
	}
	public void CreateLineRenderer<T>(T thisGameObject, Vector3 strat, Vector3 end) where T : Component
	{
		GameObject lineObj = new GameObject("Line", typeof(LineRenderer));
		lineObj.transform.parent = thisGameObject.transform;
		LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();
		lineRenderer.sortingOrder = 1;
		lineRenderer.SetWidth(lineWidth, lineWidth);
		lineRenderer.useWorldSpace = true;
		lineRenderer.material.color = Color.black;
		lineRenderer.SetColors(Color.black, Color.black);
		lineRenderer.SetVertexCount(2);
		lineRenderer.SetPosition(0, strat);
		lineRenderer.SetPosition(1, end);

		lineStruct tmp = new lineStruct();
		tmp.lineRenderer = lineRenderer;
		tmp.lineObj = lineObj;
		lineRenderList.Add(tmp);
	}
	public void AdjLineRenderer(int index, GameObject strat, GameObject end)
	{
		lineRenderList[index].lineRenderer.SetPosition(0, strat.transform.position);
		lineRenderList[index].lineRenderer.SetPosition(1, end.transform.position);
	}
	public void AdjLineRenderer(int index, Vector3 strat, Vector3 end)
	{
		lineRenderList[index].lineRenderer.SetPosition(0, strat);
		lineRenderList[index].lineRenderer.SetPosition(1, end);
	}
	public void SetParent2LineRenderList<T>(T thisGameObject)
	where T : Component
	{
		for (int i = 0; i < lineRenderList.Count; i++)
		{
			lineRenderList[i].lineObj.transform.parent = thisGameObject.transform;
		}
	}
	public void SetParent2LineRenderList(GameObject thisGameObject)
	{
		for (int i = 0; i < lineRenderList.Count; i++)
		{
			lineRenderList[i].lineObj.transform.parent = thisGameObject.transform;
		}
	}
	public virtual void UpdateLineRender() { }
	public virtual void InitLineRender<T>(T thisGameObject) where T : Component { }

}
public class IconObject : lineRendererControl
{
	public enum BodyType { GeneralBody = 0, CylinderBody = 1, }
	public List<GameObject> controlPointList = new List<GameObject>();
	public Vector3[] lastControlPointPosition;
	public List<Vector3> controlPointList_Vec3_2_LineRender = new List<Vector3>();//用於lineRenderer的controlPoint
	public GameObject body = null;
	public MeshFilter mFilter;
	public MeshRenderer mRenderer;
	public Collider mCollider;
	public Material silhouetteShader = null;

	public IconObject()
	{
		if (Shader.Find("Outlined/Silhouetted Bumped Diffuse"))
			silhouetteShader = new Material(Shader.Find("Outlined/Silhouetted Bumped Diffuse"));

	}
	public void InitBodySetting(string objName, int bodyType)
	{
		switch (bodyType)
		{
			case (int)BodyType.GeneralBody:
				body = new GameObject(objName);
				mFilter = body.AddComponent<MeshFilter>();
				mRenderer = body.AddComponent<MeshRenderer>() as MeshRenderer;
				mCollider = body.AddComponent<MeshCollider>() as MeshCollider;
				mFilter.mesh = new Mesh();
				mCollider.GetComponent<MeshCollider>().sharedMesh = mFilter.mesh;
				break;
			case (int)BodyType.CylinderBody:
				body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
				body.name = objName;
				mFilter = body.GetComponent<MeshFilter>();
				mRenderer = body.GetComponent<MeshRenderer>() as MeshRenderer;
				mCollider = body.GetComponent<CapsuleCollider>() as CapsuleCollider;
				break;
		}
		mRenderer.sortingOrder = 0;
		body.tag = "MeshBodyCollider";
	}
	public void InitControlPointList2lastControlPointPosition()
	{
		lastControlPointPosition = new Vector3[controlPointList.Count];
		for (int i = 0; i < controlPointList.Count; i++)
		{
			lastControlPointPosition[i] = controlPointList[i].transform.position;
		}
	}
	//縮放
	public Vector3 AdjPos(Vector3 tmp, int index, Vector3 center)
	{
		Vector3[] points2Center = new Vector3[controlPointList.Count];

		for (int i = 0; i < controlPointList.Count; i++)
		{
			points2Center[i] = lastControlPointPosition[i] - center;
		}

		Vector3 a = tmp - center;//now
		Vector3 b = points2Center[index];//before
		float aa = a.magnitude;
		float bb = b.magnitude;
		float cc = aa / bb;     //ratio
		for (int j = 0; j < controlPointList.Count; j++)
		{
			controlPointList[j].transform.localPosition = points2Center[j] * cc;
		}

		Vector3 offset = tmp - lastControlPointPosition[index];

		UpdateLastPos();
		UpdateLineRender();
		return offset;

	}
	public void SetIconObjectColor()
	{
		mRenderer.material.color = Color.red;
		foreach (GameObject controlPoint in controlPointList)
		{
			if (silhouetteShader != null)
				controlPoint.GetComponent<MeshRenderer>().material = silhouetteShader;

			controlPoint.GetComponent<MeshRenderer>().material.color = Color.yellow;

		}
	}
	public void AdjMesh()
	{
		mFilter.mesh.vertices = lastControlPointPosition;
		mFilter.mesh.RecalculateBounds();
		mFilter.mesh.RecalculateNormals();

		UpdateCollider();
	}
	public void UpdateLastPos()
	{
		for (int i = 0; i < controlPointList.Count; i++)
		{
			lastControlPointPosition[i] = controlPointList[i].transform.position;
		}
	}
	public override void InitLineRender<T>(T thisGameObject)
	{
		for (int i = 0; i < controlPointList.Count; i++)
		{
			if (i != controlPointList.Count - 1)
				CreateLineRenderer(thisGameObject, controlPointList[i], controlPointList[i + 1]);
			else
				CreateLineRenderer(thisGameObject, controlPointList[i], controlPointList[0]);
		}
	}
	public override void UpdateLineRender()
	{
		for (int i = 0; i < lineRenderList.Count; i++)
		{
			AdjLineRenderer(i, lineRenderList[i].startControlPoint, lineRenderList[i].endControlPoint);
		}
	}
	public void UpdateCollider() 
	{
		mCollider.GetComponent<MeshCollider>().sharedMesh = mFilter.mesh;
	}
	public GameObject CreateControlPoint(string objName, Vector3 localScale, Vector3 pos)
	{
		GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		obj.tag = "ControlPoint";
		obj.name = objName;
		obj.transform.localScale = localScale;
		obj.transform.position = pos;
		return obj;
	}
	public void SetParent2BodyAndControlPointList<T>(T thisGameObject)
	where T : Component
	{
		if (body != null) body.transform.parent = thisGameObject.transform;
		for (int i = 0; i < controlPointList.Count; i++)
		{
			controlPointList[i].transform.parent = thisGameObject.transform;
		}
	}
	public void SetParent2BodyAndControlPointList(GameObject thisGameObject)
	{
		if (body != null) body.transform.parent = thisGameObject.transform;
		for (int i = 0; i < controlPointList.Count; i++)
		{
			controlPointList[i].transform.parent = thisGameObject.transform;
		}
	}
}
public class VerandaIcon : IconObject//廡殿頂
{
	public int edgeIndex = 4;
	public void VerandaIconCreate<T>(T thisGameObject, string objName, List<GameObject> controlPointList) where T : Component
	{
		InitBodySetting(objName,(int)BodyType.GeneralBody);
		this.controlPointList = controlPointList;
		InitControlPointList2lastControlPointPosition();


		mFilter.mesh.vertices = new Vector3[] {
					controlPointList [0].transform.position,
					controlPointList [1].transform.position,
					controlPointList [2].transform.position,
					controlPointList [3].transform.position,
					controlPointList [4].transform.position,
					controlPointList [5].transform.position
					};
		mFilter.mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
		mFilter.mesh.RecalculateNormals();

		SetParent2BodyAndControlPointList(thisGameObject);
		InitLineRender(thisGameObject);
		SetIconObjectColor();
	}
	public  Vector3 AdjPos(Vector3 tmp, int index)
	{
		float offset_x = tmp.x - lastControlPointPosition[index].x;
		float offset_y = tmp.y - lastControlPointPosition[index].y;
		if (index < 4)
		{
			for (int j = 0; j < controlPointList.Count - 2; j++)
			{
				if (index == j) continue;
				if ((lastControlPointPosition[index].x == lastControlPointPosition[j].x))//x一樣的點
				{
					controlPointList[j].transform.position = new Vector3(tmp.x, lastControlPointPosition[j].y - (offset_y), lastControlPointPosition[j].z);
				}
				else if ((lastControlPointPosition[index].y == lastControlPointPosition[j].y))//y一樣的點
				{
					controlPointList[j].transform.position = new Vector3(lastControlPointPosition[j].x - (offset_x), tmp.y, lastControlPointPosition[j].z);
				}
				else//對角的點
				{
					controlPointList[j].transform.position = new Vector3(lastControlPointPosition[j].x - (offset_x), lastControlPointPosition[j].y - (offset_y), lastControlPointPosition[j].z);
				}
			}
		}
		else//mainRidge
		{
			if (index == 4)
			{
				controlPointList[5].transform.position = new Vector3(lastControlPointPosition[5].x - (offset_x), lastControlPointPosition[5].y, lastControlPointPosition[5].z);
			}
			else
			{
				controlPointList[4].transform.position = new Vector3(lastControlPointPosition[4].x - (offset_x), lastControlPointPosition[4].y, lastControlPointPosition[4].z);
			}
		}
		UpdateLastPos();
		UpdateLineRender();
		return new Vector3(offset_x, offset_y, 0);
	}
	public override void InitLineRender<T>(T thisGameObject)
	{
		CreateLineRenderer(thisGameObject, controlPointList[5], controlPointList[4]);
		CreateLineRenderer(thisGameObject, controlPointList[1], controlPointList[4]);
		CreateLineRenderer(thisGameObject, controlPointList[2], controlPointList[4]);
		CreateLineRenderer(thisGameObject, controlPointList[0], controlPointList[5]);
		CreateLineRenderer(thisGameObject, controlPointList[3], controlPointList[5]);

		CreateLineRenderer(thisGameObject, controlPointList[0], controlPointList[1]);
		CreateLineRenderer(thisGameObject, controlPointList[1], controlPointList[2]);
		CreateLineRenderer(thisGameObject, controlPointList[2], controlPointList[3]);
		CreateLineRenderer(thisGameObject, controlPointList[3], controlPointList[0]);
	}
}
public class ShandingIcon : IconObject//歇山頂
{
	public int edgeIndex = 4;
	public void ShandingIconCreate<T>(T thisGameObject, string objName, List<GameObject> controlPointList) where T : Component
	{
		InitBodySetting(objName, (int)BodyType.GeneralBody);

		this.controlPointList = controlPointList;
		InitControlPointList2lastControlPointPosition();


		mFilter.mesh.vertices = new Vector3[] {
					controlPointList [0].transform.position,
					controlPointList [1].transform.position,
					controlPointList [2].transform.position,
					controlPointList [3].transform.position,
					controlPointList [4].transform.position,
					controlPointList [5].transform.position,
					controlPointList [6].transform.position,
					controlPointList [7].transform.position,
					controlPointList [8].transform.position,
					controlPointList [9].transform.position,
					};
		mFilter.mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
		mFilter.mesh.RecalculateNormals();

		SetParent2BodyAndControlPointList(thisGameObject);
		InitLineRender(thisGameObject);
		SetIconObjectColor();
	}
	public  Vector3 AdjPos(Vector3 tmp, int index)
	{
		float offset_x = tmp.x - lastControlPointPosition[index].x;
		float offset_y = tmp.y - lastControlPointPosition[index].y;
		if (index < 4)
		{
			for (int j = 0; j < 4; j++)
			{
				if (index == j) continue;
				if ((lastControlPointPosition[index].x == lastControlPointPosition[j].x))//x一樣的點
				{
					controlPointList[j].transform.position = new Vector3(tmp.x, lastControlPointPosition[j].y - (offset_y), lastControlPointPosition[j].z);
				}
				else if ((lastControlPointPosition[index].y == lastControlPointPosition[j].y))//y一樣的點
				{
					controlPointList[j].transform.position = new Vector3(lastControlPointPosition[j].x - (offset_x), tmp.y, lastControlPointPosition[j].z);
				}
				else//對角的點
				{
					controlPointList[j].transform.position = new Vector3(lastControlPointPosition[j].x - (offset_x), lastControlPointPosition[j].y - (offset_y), lastControlPointPosition[j].z);
				}
			}
		}
		else if ((index == 4) || (index == 5))//mainRidge
		{
			if (index == 4)
			{
				controlPointList[7].transform.position = new Vector3(controlPointList[4].transform.position.x, controlPointList[7].transform.position.y, controlPointList[7].transform.position.z);
				controlPointList[8].transform.position = new Vector3(controlPointList[4].transform.position.x, controlPointList[8].transform.position.y, controlPointList[8].transform.position.z);
				controlPointList[5].transform.position = new Vector3(lastControlPointPosition[5].x - (offset_x), lastControlPointPosition[5].y, lastControlPointPosition[5].z);
				controlPointList[6].transform.position = new Vector3(controlPointList[5].transform.position.x, controlPointList[6].transform.position.y, controlPointList[6].transform.position.z);
				controlPointList[9].transform.position = new Vector3(controlPointList[5].transform.position.x, controlPointList[9].transform.position.y, controlPointList[9].transform.position.z);
			}
			else
			{
				controlPointList[6].transform.position = new Vector3(controlPointList[5].transform.position.x, controlPointList[6].transform.position.y, controlPointList[6].transform.position.z);
				controlPointList[9].transform.position = new Vector3(controlPointList[5].transform.position.x, controlPointList[9].transform.position.y, controlPointList[9].transform.position.z);
				controlPointList[4].transform.position = new Vector3(lastControlPointPosition[4].x - (offset_x), lastControlPointPosition[4].y, lastControlPointPosition[4].z);
				controlPointList[7].transform.position = new Vector3(controlPointList[4].transform.position.x, controlPointList[7].transform.position.y, controlPointList[7].transform.position.z);
				controlPointList[8].transform.position = new Vector3(controlPointList[4].transform.position.x, controlPointList[8].transform.position.y, controlPointList[8].transform.position.z);
			}
		}
		else
		{
			for (int j = 6; j < controlPointList.Count; j++)
			{
				if (index == j) continue;
				if ((lastControlPointPosition[index].x == lastControlPointPosition[j].x))//x一樣的點
				{
					controlPointList[j].transform.position = new Vector3(tmp.x, lastControlPointPosition[j].y - (offset_y), lastControlPointPosition[j].z);
				}
				else if ((lastControlPointPosition[index].y == lastControlPointPosition[j].y))//y一樣的點
				{
					controlPointList[j].transform.position = new Vector3(lastControlPointPosition[j].x - (offset_x), tmp.y, lastControlPointPosition[j].z);
				}
				else//對角的點
				{
					controlPointList[j].transform.position = new Vector3(lastControlPointPosition[j].x - (offset_x), lastControlPointPosition[j].y - (offset_y), lastControlPointPosition[j].z);
				}
			}
			controlPointList[4].transform.position = new Vector3(controlPointList[7].transform.position.x, controlPointList[4].transform.position.y, controlPointList[4].transform.position.z);
			controlPointList[5].transform.position = new Vector3(controlPointList[6].transform.position.x, controlPointList[5].transform.position.y, controlPointList[5].transform.position.z);
		}
		UpdateLastPos();
		UpdateLineRender();
		return new Vector3(offset_x, offset_y, 0);
	}
	public override void InitLineRender<T>(T thisGameObject)
	{
		CreateLineRenderer(thisGameObject, controlPointList[5], controlPointList[4]);
		CreateLineRenderer(thisGameObject, controlPointList[7], controlPointList[4]);
		CreateLineRenderer(thisGameObject, controlPointList[8], controlPointList[4]);
		CreateLineRenderer(thisGameObject, controlPointList[6], controlPointList[5]);
		CreateLineRenderer(thisGameObject, controlPointList[9], controlPointList[5]);
					
		CreateLineRenderer(thisGameObject, controlPointList[0], controlPointList[6]);
		CreateLineRenderer(thisGameObject, controlPointList[3], controlPointList[9]);
		CreateLineRenderer(thisGameObject, controlPointList[1], controlPointList[7]);
		CreateLineRenderer(thisGameObject, controlPointList[2], controlPointList[8]);

		CreateLineRenderer(thisGameObject, controlPointList[0], controlPointList[1]);
		CreateLineRenderer(thisGameObject, controlPointList[1], controlPointList[2]);
		CreateLineRenderer(thisGameObject, controlPointList[2], controlPointList[3]);
		CreateLineRenderer(thisGameObject, controlPointList[3], controlPointList[0]);
	}
}

public class TriangleIcon : IconObject//三角形
{
	public int edgeIndex = 3;
	public void TriangleIconCreate<T>(T thisGameObject, string objName, List<GameObject> controlPointList) where T : Component
	{
		InitBodySetting(objName, (int)BodyType.GeneralBody);

		this.controlPointList = controlPointList;
		InitControlPointList2lastControlPointPosition();

		mFilter.mesh.vertices = new Vector3[] {
				controlPointList [0].transform.position,
				controlPointList [1].transform.position,
				controlPointList [2].transform.position,
			};
		mFilter.mesh.triangles = new int[] { 0, 1, 2 };
		mFilter.mesh.RecalculateNormals();

		SetParent2BodyAndControlPointList(thisGameObject);
		InitLineRender(thisGameObject);
		SetIconObjectColor();
	}
}
public class RectangleIcon : IconObject//四角形
{
	public int edgeIndex = 4;
	public void RectangleIconCreate<T>(T thisGameObject, string objName, List<GameObject> controlPointList) where T : Component
	{
		InitBodySetting(objName, (int)BodyType.GeneralBody);

		this.controlPointList = controlPointList;
		InitControlPointList2lastControlPointPosition();

		mFilter.mesh.vertices = new Vector3[] {
				controlPointList [0].transform.position,
				controlPointList [1].transform.position,
				controlPointList [2].transform.position,
				controlPointList [3].transform.position,
			};

		mFilter.mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
		mFilter.mesh.RecalculateNormals();

		SetParent2BodyAndControlPointList(thisGameObject);
		InitLineRender(thisGameObject);
		SetIconObjectColor();
	}
}
public class PentagonIcon : IconObject//五邊形
{
	public int edgeIndex = 5;
	public void PentagonIconCreate<T>(T thisGameObject, string objName, List<GameObject> controlPointList) where T : Component
	{
		InitBodySetting(objName, (int)BodyType.GeneralBody);

		this.controlPointList = controlPointList;
		InitControlPointList2lastControlPointPosition();


		mFilter.mesh.vertices = new Vector3[] {
				controlPointList [0].transform.position,
				controlPointList [1].transform.position,
				controlPointList [2].transform.position,
				controlPointList [3].transform.position,
				controlPointList [4].transform.position,
			};
		mFilter.mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4 };
		mFilter.mesh.RecalculateNormals();

		SetParent2BodyAndControlPointList(thisGameObject);
		InitLineRender(thisGameObject);
		SetIconObjectColor();
	}
}
public class HexagonIcon : IconObject//六邊形
{
	public int edgeIndex=6;
	public void HexagonIconCreate<T>(T thisGameObject, string objName, List<GameObject> controlPointList) where T : Component
	{
		InitBodySetting(objName, (int)BodyType.GeneralBody);

		this.controlPointList = controlPointList;
		InitControlPointList2lastControlPointPosition();

		mFilter.mesh.vertices = new Vector3[] {
					controlPointList [0].transform.position,
					controlPointList [1].transform.position,
					controlPointList [2].transform.position,
					controlPointList [3].transform.position,
					controlPointList [4].transform.position,
					controlPointList [5].transform.position
		};
		mFilter.mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5 };
		mFilter.mesh.RecalculateNormals();

		SetParent2BodyAndControlPointList(thisGameObject);
		InitLineRender(thisGameObject);
		SetIconObjectColor();
	}

}
public class MeshObj : MonoBehaviour
{
	public List<GameObject> controlPointList = new List<GameObject>();

	private VerandaIcon verandaIcon;
	private ShandingIcon shandingIcon;
	private TriangleIcon triangleIcon;
	private RectangleIcon rectangleIcon;
	private PentagonIcon pentagonIcon;
	private HexagonIcon hexagonIcon;

	private DragItemController dragitemcontroller;
	private Movement movement;

	public int edgeIndex;

	public Vector2 ini_bodydis;
	public float ini_mainRidgedis;

	public Vector2 chang_bodydis;
	public float chang_mainRidgedis;
	public Vector2 ratio_bodydis;
	public float ratio_mainRidgedis;

	void Start()
	{
		dragitemcontroller = GameObject.Find("DragItemController").GetComponent<DragItemController>();
	}
	VerandaIcon CreateVerandaIcon()
	{
		VerandaIcon verandaIcon = new VerandaIcon();
	
		verandaIcon.VerandaIconCreate(this, "VerandaIcon_mesh", controlPointList);

		edgeIndex = verandaIcon.edgeIndex;

		ini_bodydis.x = controlPointList[1].transform.position.x - controlPointList[0].transform.position.x;
		ini_bodydis.y = controlPointList[1].transform.position.y - controlPointList[2].transform.position.y;
		ini_bodydis = ini_bodydis / 2.0f;

		ini_mainRidgedis = controlPointList[4].transform.position.x - controlPointList[5].transform.position.x;
		ini_mainRidgedis = ini_mainRidgedis / 2.0f;

		return verandaIcon;
	}
	ShandingIcon CreateShandingIcon()
	{
		ShandingIcon shandingIcon = new ShandingIcon();
		shandingIcon.ShandingIconCreate(this, "ShandingIcon_mesh", controlPointList);

		edgeIndex = shandingIcon.edgeIndex;
		ini_bodydis.x = controlPointList[1].transform.position.x - controlPointList[0].transform.position.x;
		ini_bodydis.y = controlPointList[1].transform.position.y - controlPointList[2].transform.position.y;
		ini_bodydis = ini_bodydis / 2.0f;

		ini_mainRidgedis = controlPointList[4].transform.position.x - controlPointList[5].transform.position.x;
		ini_mainRidgedis = ini_mainRidgedis / 2.0f;
		return shandingIcon;
	}
	TriangleIcon CreateTriangleIcon()
	{
		TriangleIcon triIcon = new TriangleIcon();
		
		triIcon.TriangleIconCreate(this, "TiangleIcon_mesh", controlPointList);

		edgeIndex = triIcon.edgeIndex;

		return triIcon;
	}
	RectangleIcon CreateRectangleIcon()
	{
		RectangleIcon rectIcon = new RectangleIcon();
	
		rectIcon.RectangleIconCreate(this, "RectangleIcon_mesh", controlPointList);

		edgeIndex = rectIcon.edgeIndex;
		return rectIcon;
	}
	PentagonIcon CreatePentagonIcon()
	{
		PentagonIcon pentaIcon = new PentagonIcon();
		
		pentaIcon.PentagonIconCreate(this, "PentagonIcon_mesh", controlPointList);

		edgeIndex = pentaIcon.edgeIndex;
		return pentaIcon;
	}
	HexagonIcon CreateHexagonIcon()
	{
		HexagonIcon hexIcon = new HexagonIcon();
	
		hexIcon.HexagonIconCreate(this, "HexagonIcon_mesh", controlPointList);

		edgeIndex = hexIcon.edgeIndex;
		return hexIcon;
	}

	void Awake()
	{
		movement = GameObject.Find("Movement").GetComponent<Movement>();

		switch (gameObject.tag)
		{
			case "VerandaIcon"://specialCase
				verandaIcon = CreateVerandaIcon();
				break;
			case "ShandingIcon"://specialCase
				shandingIcon = CreateShandingIcon();
				break;
			case "TriangleIcon":
				triangleIcon = CreateTriangleIcon();
				break;
			case "RectangleIcon":
				rectangleIcon = CreateRectangleIcon();
				break;
			case "PentagonIcon":
				pentagonIcon = CreatePentagonIcon();
				break;
			case "HexagonIcon":
				hexagonIcon = CreateHexagonIcon();
				break;

		}
		addpoint();
	}

	public void adjPos()
	{
		chang_bodydis = ratio_bodydis = Vector2.zero;
		chang_mainRidgedis = ratio_mainRidgedis = 0;

		for (int i = 0; i < controlPointList.Count; i++)
		{
			if (dragitemcontroller.chooseObj == controlPointList[i])
			{
				Vector3 tmp = dragitemcontroller.chooseObj.transform.position;
				Vector3 center = transform.position;
				switch (gameObject.tag)
				{
					case "VerandaIcon"://specialCase
						Vector3 offsetVector = verandaIcon.AdjPos(tmp, i);
						switch(i)
						{
							case 0:
								chang_bodydis.x = -offsetVector.x;
								chang_bodydis.y = offsetVector.y;
								ratio_bodydis.x = chang_bodydis.x / ini_bodydis.x;
								ratio_bodydis.y = chang_bodydis.y / ini_bodydis.y;
								break;
							case 1:
								chang_bodydis.x = offsetVector.x;
								chang_bodydis.y = offsetVector.y;
								ratio_bodydis.x = chang_bodydis.x / ini_bodydis.x;
								ratio_bodydis.y = chang_bodydis.y / ini_bodydis.y;
								break;
							case 2:
								chang_bodydis.x = offsetVector.x;
								chang_bodydis.y = -offsetVector.y;
								ratio_bodydis.x = chang_bodydis.x / ini_bodydis.x;
								ratio_bodydis.y = chang_bodydis.y / ini_bodydis.y;
								break;
							case 3:
								chang_bodydis.x = -offsetVector.x;
								chang_bodydis.y = -offsetVector.y;
								ratio_bodydis.x = chang_bodydis.x / ini_bodydis.x;
								ratio_bodydis.y = chang_bodydis.y / ini_bodydis.y;
								break;
							break;
							case 4:
								chang_mainRidgedis = -offsetVector.x;
								ratio_mainRidgedis = chang_mainRidgedis / ini_mainRidgedis;
								break;
							case 5:
								chang_mainRidgedis = offsetVector.x;
								ratio_mainRidgedis = chang_mainRidgedis / ini_mainRidgedis;
							break;

						}
						verandaIcon.AdjMesh();
						break;
					case "ShandingIcon"://specialCase
						shandingIcon.AdjPos(tmp, i);
						shandingIcon.AdjMesh();
						break;
					case "TriangleIcon":
						triangleIcon.AdjPos(tmp, i, center);
						triangleIcon.AdjMesh();
						break;
					case "RectangleIcon":
						rectangleIcon.AdjPos(tmp, i, center);
						rectangleIcon.AdjMesh();
						break;
					case "PentagonIcon":
						pentagonIcon.AdjPos(tmp, i, center);
						pentagonIcon.AdjMesh();
						break;
					case "HexagonIcon":
						hexagonIcon.AdjPos(tmp, i, center);
						hexagonIcon.AdjMesh();
						break;

				}
				break;

			}
		}
	}
	public void addpoint()
	{
		controlPointList.RemoveAll(GameObject => GameObject == null); 
		switch (gameObject.tag)
		{
			case "VerandaIcon"://specialCase
				for (int i = 0; i < controlPointList.Count - 2; i++)
				{
					movement.freelist.Add(controlPointList[i]);
				}
				movement.horlist.Add(controlPointList[4]);
				movement.horlist.Add(controlPointList[5]);
				break;
			case "ShandingIcon"://specialCase
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
				break;
			default:
				movement.freelist.AddRange(controlPointList);
				break;

		}

	}
	public Vector3 ClampPos(Vector3 inputPos)
	{
		float minClampX = float.MinValue;
		float maxClampX = float.MaxValue;
		float minClampY = float.MinValue;
		float maxClampY = float.MaxValue;
		if (this.tag == "VerandaIcon")
		{
			float minWidth = ini_mainRidgedis * 0.8f;
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
		else if (this.tag == "ShandingIcon")
		{
			float minWidth = ini_mainRidgedis * 0.8f;
			float minHeight = ini_bodydis.y * 0.5f * 0.5f;
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

