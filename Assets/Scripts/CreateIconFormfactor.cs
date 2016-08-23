using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateIconFormfactor : MonoBehaviour
{

	public GameObject controlPoint;

	public int edgeNum;

	public List<GameObject> controlPointList = new List<GameObject>();
	public Vector3[] verts;
	[HideInInspector]
	Mesh mesh;
	DragItemController dragItemController;

	// Use this for initialization
	void Awake()
	{
		dragItemController = GameObject.Find("DragItemController").GetComponent<DragItemController>();
		CreatePoint();
		SetMesh();
	}

	void CreatePoint()
	{
		controlPointList.Add(controlPoint);
		for (int i = 1; i < edgeNum; i++)
		{
			GameObject clone = Instantiate(controlPoint);
			clone.transform.position = controlPoint.transform.position;

			clone.transform.RotateAround(transform.position, Vector3.forward, 360.0f / edgeNum * i);

			clone.transform.parent = transform;

			controlPointList.Add(clone);

		}

	}

	void SetMesh()
	{
		if(edgeNum<3)return;

		GameObject TTL = new GameObject();

		TTL.transform.parent = transform;
		TTL.name = ("iconMesh");
		TTL.AddComponent<MeshFilter>();
		TTL.AddComponent<MeshRenderer>();
		TTL.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;

		mesh = TTL.GetComponent<MeshFilter>().mesh;

		mesh.Clear();

		Vector3[] vL = new Vector3[edgeNum];
		Vector3[] nL = new Vector3[edgeNum];
		Vector2[] uvL = new Vector2[edgeNum];
		int[] tL = new int[3 * edgeNum];

		uvL[0] = new Vector2(1, 1);
		uvL[1] = new Vector2(0, 1);
		uvL[2] = new Vector2(1, 0);

		nL[0] = -Vector3.up;
		nL[1] = -Vector3.up;
		nL[2] = -Vector3.up;

		Vector3 v1 = controlPointList[0].transform.position;
		Vector3 v2 = controlPointList[1].transform.position;
		Vector3 v3 = controlPointList[2].transform.position;

		vL[0] = v1;
		vL[1] = v2;
		vL[2] = v3;

		tL[0] = 0;
		tL[1] = 1;
		tL[2] = 2;

		for (int i = 4; i <= edgeNum; i++){
			
			Vector3 v4 = controlPointList[i - 1].transform.position;

			vL[i - 1] = v4;

			if (i % 3 == 2){
				uvL[i - 1] = new Vector2(0, 1);
			}
			else if (i % 3 == 1){
				uvL[i - 2] = new Vector2(1, 0);
			}
			else{
				uvL[i - 2] = new Vector2(0, 1);
			}
			
			nL[i - 1] = -Vector3.up;

			tL[i - 1 + (i - 4) * 2] = 0;
			tL[i + (i - 4) * 2] = i - 2;
			tL[(i + 1) + (i - 4) * 2] = i - 1;
		}
		mesh.vertices = vL;
		mesh.triangles = tL;
		mesh.normals = nL;
		mesh.uv = uvL;

		verts=mesh.vertices;
	}
	public void SetControlPointListPos() 
	{
		for (int i = 0; i < controlPointList.Count; i++)
		{
			verts[i] = controlPointList[i].transform.localPosition;

		}
	}
	public void SetControlPointListRatioPos()
	{
		Vector3 middle = Vector3.zero;
		Vector3[] eachToMiddle = new Vector3[controlPointList.Count];

		for (int i = 0; i < controlPointList.Count; i++)
		{
			eachToMiddle[i] = verts[i] - middle;
		}

		for (int i = 0; i < controlPointList.Count; i++)
		{
			if (dragItemController.chooseObj == controlPointList[i])
			{
				float aa, bb, cc;
				Vector3 a = dragItemController.chooseObj.transform.localPosition - middle;//after
				Vector3 b = verts[i] - middle;//before

				aa = a.magnitude;
				bb = b.magnitude;
				cc = aa / bb;     //ratio

				for (int j = 0; j < controlPointList.Count; j++)
				{
					controlPointList[j].transform.localPosition = eachToMiddle[j] * cc;
				}

			}
		}
		for (int i = 0; i < controlPointList.Count; i++)
		{
			verts[i] = controlPointList[i].transform.localPosition;

		}
		AdjustMesh();
	}
	void AdjustMesh()
	{
		for (int i = 0; i < controlPointList.Count; i++)
		{
			verts[i] = controlPointList[i].transform.localPosition;
		}
		mesh.vertices = verts;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

	}
}