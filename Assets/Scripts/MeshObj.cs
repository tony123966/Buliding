﻿/*
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
public class IconObject
{
	public List<GameObject> controlPointList = new List<GameObject>();
	public GameObject body = null;
	public int edgeIndex;
	public MeshFilter mFilter;
	public Vector3[] lastControlPointPosition;
	public Vector3 AdjPos( Vector3 tmp, int index,Vector3 center)
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

		return offset;

	}
	public void AdjMesh()
	{
		mFilter.mesh.vertices = lastControlPointPosition;
		mFilter.mesh.RecalculateBounds();
		mFilter.mesh.RecalculateNormals();

	}
	public void UpdateLastPos()
	{
		for (int i = 0; i < controlPointList.Count; i++)
		{
			lastControlPointPosition[i] = controlPointList[i].transform.position;
		}
	}
	private Vector3 FindCenter(List<GameObject> lists)
	{
		float sumX = 0, sumY = 0, sumZ = 0;
		for(int i=0;i<lists.Count;i++)
		{
			sumX +=lists[i].transform.position.x;
			sumY += lists[i].transform.position.y;
			sumZ += lists[i].transform.position.z;
		}
		return new Vector3(sumX / lists.Count, sumY / lists.Count, sumZ / lists.Count);
	}
}
public class VerandaIcon : IconObject//廡殿頂
{
	public void VerandaIconCreate<T>(T thisGameObject, string objName, List<GameObject> controlPointList) where T : Component
	{
		edgeIndex = 4;
		body = new GameObject(objName);
		mFilter = body.AddComponent<MeshFilter>();
		MeshRenderer renderer = body.AddComponent<MeshRenderer>() as MeshRenderer;
		renderer.sortingOrder = 0;
		this.controlPointList = controlPointList;
		//Debug.Log("ghgfhfghgh:" + this.controlPointList.Count);
		mFilter.mesh=new Mesh();
		mFilter.mesh.vertices = new Vector3[] {
					controlPointList [0].transform.position,
					controlPointList [1].transform.position,
					controlPointList [2].transform.position,
					controlPointList [3].transform.position,
					controlPointList [4].transform.position,
					controlPointList [5].transform.position
					};
		mFilter.mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
		lastControlPointPosition = mFilter.mesh.vertices;
		mFilter.mesh.RecalculateNormals();

		body.transform.parent = thisGameObject.transform;
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
		return new Vector3(offset_x, offset_y, 0);
	}
}
public class ShandingIcon : IconObject//歇山頂
{
	public void ShandingIconCreate<T>(T thisGameObject, string objName, List<GameObject> controlPointList) where T : Component
	{
		edgeIndex = 4;
		body = new GameObject(objName);
		mFilter = body.AddComponent<MeshFilter>();
		MeshRenderer renderer = body.AddComponent<MeshRenderer>() as MeshRenderer;
		renderer.sortingOrder = 0;
		mFilter.mesh = new Mesh();
		this.controlPointList = controlPointList;
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
		lastControlPointPosition = mFilter.mesh.vertices;
		mFilter.mesh.RecalculateNormals();

		body.transform.parent = thisGameObject.transform;

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
		return new Vector3(offset_x, offset_y, 0);
	}
}

public class TriangleIcon : IconObject//三角形
{
	public void TriangleIconCreate<T>(T thisGameObject, string objName, List<GameObject> controlPointList) where T : Component
	{
		edgeIndex = 3;
		body = new GameObject(objName);
		mFilter = body.AddComponent<MeshFilter>();
		MeshRenderer renderer = body.AddComponent<MeshRenderer>() as MeshRenderer;
		renderer.sortingOrder = 0;
		mFilter.mesh = new Mesh();
		this.controlPointList = controlPointList;
	
		mFilter.mesh.vertices = new Vector3[] {
				controlPointList [0].transform.position,
				controlPointList [1].transform.position,
				controlPointList [2].transform.position,
			};
		Debug.Log("mFilter.mesh.vertices[0]" + mFilter.mesh.vertices[0]);
		Debug.Log("mFilter.mesh.vertices[1]" + mFilter.mesh.vertices[1]);
		Debug.Log("mFilter.mesh.vertices[2]" + mFilter.mesh.vertices[2]);
		mFilter.mesh.triangles = new int[] { 0, 1, 2 };
	
		lastControlPointPosition = mFilter.mesh.vertices;
		mFilter.mesh.RecalculateNormals();

		body.transform.parent = thisGameObject.transform;
	}

}
public class RectangleIcon : IconObject//四角形
{
	public void RectangleIconCreate<T>(T thisGameObject, string objName, List<GameObject> controlPointList) where T : Component
	{
		edgeIndex = 4;
		body = new GameObject(objName);
		mFilter = body.AddComponent<MeshFilter>();
		MeshRenderer renderer = body.AddComponent<MeshRenderer>() as MeshRenderer;
		renderer.sortingOrder = 0;
		mFilter.mesh = new Mesh();
		this.controlPointList = controlPointList;
		mFilter.mesh.vertices = new Vector3[] {
				controlPointList [0].transform.position,
				controlPointList [1].transform.position,
				controlPointList [2].transform.position,
				controlPointList [3].transform.position,
			};

		mFilter.mesh.triangles = new int[] { 0, 1, 2, 0, 3, 1 };
		lastControlPointPosition = mFilter.mesh.vertices;
		mFilter.mesh.RecalculateNormals();

		body.transform.parent = thisGameObject.transform;
	}

}
public class PentagonIcon : IconObject//五邊形
{
	public void PentagonIconCreate<T>(T thisGameObject, string objName, List<GameObject> controlPointList) where T : Component
	{
		edgeIndex = 5;
		body = new GameObject(objName);
		mFilter = body.AddComponent<MeshFilter>();
		MeshRenderer renderer = body.AddComponent<MeshRenderer>() as MeshRenderer;
		renderer.sortingOrder = 0;
		mFilter.mesh = new Mesh();
		this.controlPointList = controlPointList;
		mFilter.mesh.vertices = new Vector3[] {
				controlPointList [0].transform.position,
				controlPointList [1].transform.position,
				controlPointList [2].transform.position,
				controlPointList [3].transform.position,
				controlPointList [4].transform.position,
			};
		mFilter.mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4 };
		lastControlPointPosition = mFilter.mesh.vertices;
		mFilter.mesh.RecalculateNormals();

		body.transform.parent = thisGameObject.transform;
	}

}
public class HexagonIcon : IconObject//六邊形
{
	public void HexagonIconCreate<T>(T thisGameObject, string objName, List<GameObject> controlPointList) where T : Component
	{
		edgeIndex = 6;
		body = new GameObject(objName);
		mFilter = body.AddComponent<MeshFilter>();
		MeshRenderer renderer = body.AddComponent<MeshRenderer>() as MeshRenderer;
		renderer.sortingOrder = 0;
		mFilter.mesh = new Mesh();
		this.controlPointList = controlPointList;
		mFilter.mesh.vertices = new Vector3[] {
					controlPointList [0].transform.position,
					controlPointList [1].transform.position,
					controlPointList [2].transform.position,
					controlPointList [3].transform.position,
					controlPointList [4].transform.position,
					controlPointList [5].transform.position
		};
		mFilter.mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5 };
		lastControlPointPosition = mFilter.mesh.vertices;
		mFilter.mesh.RecalculateNormals();

		body.transform.parent = thisGameObject.transform;
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
	VerandaIcon CreateVerandaIcon()
	{
		VerandaIcon verandaIcon = new VerandaIcon();
		edgeIndex = verandaIcon.edgeIndex;
		verandaIcon.VerandaIconCreate(this, "VerandaIcon_mesh", controlPointList);


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
		edgeIndex = shandingIcon.edgeIndex;

		shandingIcon.ShandingIconCreate(this, "ShandingIcon_mesh", controlPointList);
		ini_bodydis.x = controlPointList[1].transform.position.x - controlPointList[0].transform.position.x;
		ini_bodydis.y = controlPointList[1].transform.position.y - controlPointList[2].transform.position.y;
		ini_bodydis = ini_bodydis / 2.0f;

		ini_mainRidgedis = controlPointList[4].transform.position.x - controlPointList[5].transform.position.x;
		ini_mainRidgedis = ini_mainRidgedis / 2.0f;
		return shandingIcon;
	}
	TriangleIcon CreateTriangleIcon()
	{
	Debug.Log("tri");
		TriangleIcon triIcon = new TriangleIcon();
		edgeIndex = triIcon.edgeIndex;
		triIcon.TriangleIconCreate(this, "TiangleIcon_mesh", controlPointList);
		return triIcon;
	}
	RectangleIcon CreateRectangleIcon()
	{
		RectangleIcon rectIcon = new RectangleIcon();
		edgeIndex = rectIcon.edgeIndex;
		rectIcon.RectangleIconCreate(this, "RectangleIcon_mesh", controlPointList);
		return rectIcon;
	}
	PentagonIcon CreatePentagonIcon()
	{
		PentagonIcon pentaIcon = new PentagonIcon();
		edgeIndex = pentaIcon.edgeIndex;
		pentaIcon.PentagonIconCreate(this, "PentagonIcon_mesh", controlPointList);
		return pentaIcon;
	}
	HexagonIcon CreateHexagonIcon()
	{
		HexagonIcon hexIcon = new HexagonIcon();
		edgeIndex = hexIcon.edgeIndex;
		hexIcon.HexagonIconCreate(this, "HexagonIcon_mesh", controlPointList);
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
				Vector3 center=transform.position;
				switch (gameObject.tag)
				{
					case "VerandaIcon"://specialCase
						Vector3 offsetVector = verandaIcon.AdjPos(tmp, i);
						chang_bodydis.x = offsetVector.x;
						chang_bodydis.y = offsetVector.y;
						ratio_bodydis.x = chang_bodydis.x / ini_bodydis.x;
						ratio_bodydis.y = chang_bodydis.y / ini_bodydis.y;

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
	public void CreateLineRenderer(GameObject strat, GameObject end)
	{
		GameObject lineObj = new GameObject("Line", typeof(LineRenderer));
		lineObj.transform.parent = transform;
		LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();
		lineRenderer.sortingOrder = 1;
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
			AdjLineRenderer(0, controlPointList[5], controlPointList[4]);
			AdjLineRenderer(1, controlPointList[1], controlPointList[4]);
			AdjLineRenderer(2, controlPointList[2], controlPointList[4]);
			AdjLineRenderer(3, controlPointList[0], controlPointList[5]);
			AdjLineRenderer(4, controlPointList[3], controlPointList[5]);
		}
		else if (this.tag == "Shanding")
		{
			AdjLineRenderer(0, controlPointList[5], controlPointList[4]);
			AdjLineRenderer(1, controlPointList[7], controlPointList[4]);
			AdjLineRenderer(2, controlPointList[8], controlPointList[4]);
			AdjLineRenderer(3, controlPointList[6], controlPointList[5]);
			AdjLineRenderer(4, controlPointList[9], controlPointList[5]);

			AdjLineRenderer(5, controlPointList[0], controlPointList[6]);
			AdjLineRenderer(6, controlPointList[3], controlPointList[9]);
			AdjLineRenderer(7, controlPointList[1], controlPointList[7]);
			AdjLineRenderer(8, controlPointList[2], controlPointList[8]);
		}
	}
	public void AdjLineRenderer(int index, GameObject strat, GameObject end)
	{
		if (this.tag == "Rectangle")
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

