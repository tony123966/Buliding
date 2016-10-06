using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class platform2icon : MonoBehaviour
{


	public List<GameObject> controlPointList = new List<GameObject>();


	private DragItemController dragitemcontroller;
	private Movement movement;


	Mesh mesh;
	Vector3[] verts;
	Vector3[] ini_verts;


	//for ratio
	public Vector2 chang_platdis;

	void Awake()
	{

		mesh = GetComponent<MeshFilter>().mesh;
		movement = GameObject.Find("Movement").GetComponent<Movement>();

		if (!gameObject.GetComponent<MeshFilter>()) gameObject.AddComponent<MeshFilter>();
		if (!gameObject.GetComponent<MeshRenderer>()) gameObject.AddComponent<MeshRenderer>();


		if (controlPointList.Count == 4)
		{
			mesh.vertices = new Vector3[] {
				 controlPointList [0].transform.localPosition,
				 controlPointList [1].transform.localPosition,
				 controlPointList [2].transform.localPosition,
				 controlPointList [3].transform.localPosition
			};
			mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
			verts = mesh.vertices;
			movement.freelist.AddRange(controlPointList);

			ini_verts = new Vector3[controlPointList.Count];
			for (int i = 0; i < controlPointList.Count; i++)
			{
				ini_verts[i] = controlPointList[i].transform.localPosition;
			}
		}

	}

	// Use this for initialization
	void Start()
	{
		dragitemcontroller = GameObject.Find("DragItemController").GetComponent<DragItemController>();
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
		for (int i = 0; i < controlPointList.Count; i++)
		{
			if (dragitemcontroller.chooseObj == controlPointList[i])
			{

				Vector3 tmp = dragitemcontroller.chooseObj.transform.localPosition;
				if (controlPointList.Count == 4)
				{
					float offset_x, offset_y;
					offset_x = tmp.x - verts[i].x;
					offset_y = tmp.y - verts[i].y;
					for (int j = 0; j < controlPointList.Count; j++)
					{
						if (i == j) continue;
						if ((verts[i].x == controlPointList[j].transform.localPosition.x))
						{
							controlPointList[j].transform.localPosition = new Vector3(tmp.x, verts[j].y - (offset_y), verts[j].z);
						}
						else if ((verts[i].y == controlPointList[j].transform.localPosition.y))
						{
							controlPointList[j].transform.localPosition = new Vector3(verts[j].x - (offset_x), tmp.y, verts[j].z);
						}
						else
						{
							controlPointList[j].transform.localPosition = new Vector3(verts[j].x - (offset_x), verts[j].y - (offset_y), verts[j].z);
						}
					}
					//
					chang_platdis.x = tmp.x - ini_verts[i].x;
					chang_platdis.y = tmp.y - ini_verts[i].y;
				}
				break;
			}
		}

		for (int x = 0; x < controlPointList.Count; x++)
		{
			verts[x] = controlPointList[x].transform.localPosition;
		}
		adjMesh();
	}
	public void addpoint()
	{
		if (controlPointList.Count == 4)
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
		if (controlPointList.Count == 4)
		{
			float minWidth = (ini_verts[1].x - ini_verts[0].x) * 0.2f;
			float minHeight = (ini_verts[1].y - ini_verts[2].y) * 0.2f;

			if (dragitemcontroller.chooseObj == controlPointList[0] )
			{
				maxClampX = controlPointList[1].transform.position.x - minWidth;
				minClampY = controlPointList[3].transform.position.y + minHeight;
			}
			else if (dragitemcontroller.chooseObj == controlPointList[3])
			{
				maxClampX = controlPointList[1].transform.position.x - minWidth;
				maxClampY = controlPointList[0].transform.position.y - minHeight;
			}
			else if (dragitemcontroller.chooseObj == controlPointList[1] )
			{
				minClampX = controlPointList[0].transform.position.x + minWidth;
				minClampY = controlPointList[2].transform.position.y + minHeight;
			}
			else if ( dragitemcontroller.chooseObj == controlPointList[2])
			{
				minClampX = controlPointList[0].transform.position.x + minWidth;
				maxClampY = controlPointList[1].transform.position.y - minHeight;
			}
			float posX = Mathf.Clamp(inputPos.x, minClampX, maxClampX);
			float posY = Mathf.Clamp(inputPos.y, minClampY, maxClampY);
			return new Vector3(posX, posY, inputPos.z);
		}
		return inputPos;
	}
}