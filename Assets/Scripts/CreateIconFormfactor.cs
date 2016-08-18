using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateIconFormfactor : MonoBehaviour
{

	public GameObject controlPoint;

	public int edgeNum;

	public List<GameObject> controlPointList = new List<GameObject>();


	// Use this for initialization
	void Awake()
	{
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
/*
		if (!gameObject.GetComponent<MeshFilter>()) gameObject.AddComponent<MeshFilter>();
		if (!gameObject.GetComponent<MeshRenderer>()) gameObject.AddComponent<MeshRenderer>();*/
		GameObject TTL = new GameObject();

		TTL.transform.parent = transform;

		TTL.name = ("iconmesh");
		TTL.AddComponent<MeshFilter>();
		TTL.AddComponent<MeshRenderer>();


		TTL.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;

		Mesh mesh = TTL.GetComponent<MeshFilter>().mesh;

		mesh.Clear();

		Vector3[] vL = new Vector3[2 * edgeNum];
		Vector3[] nL = new Vector3[2 * edgeNum];
		Vector2[] uvL = new Vector2[2 * edgeNum];
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

		for (int i = 4; i <= edgeNum; i++)
		{

			Vector3 v4 = controlPointList[i - 1].transform.position;

			vL[i - 1] = v4;

			if (i % 3 == 2)
			{
				uvL[i - 1] = new Vector2(0, 1);
			}
			else if (i % 3 == 1)
			{
				uvL[i - 2] = new Vector2(1, 0);
			}
			else
			{
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
	}

	/*
		void mesh()
		{






			int angle = int.Parse(stringToEdit);







			GameObject TTL = new GameObject();

			TTL.transform.parent = transform;

			TTL.name = ("iconmesh");
			TTL.AddComponent<MeshFilter>();
			TTL.AddComponent<MeshRenderer>();


			MeshRenderer floor = TTL.GetComponent<MeshRenderer>();

			MeshRenderer floor1 = ori.GetComponent<MeshRenderer>();

			floor.material = floor1.material;


			Mesh meshL = TTL.GetComponent<MeshFilter>().mesh;

			meshL.Clear();

			Vector3[] vL = new Vector3[2 * angle];
			Vector3[] nL = new Vector3[2 * angle];
			Vector2[] uvL = new Vector2[2 * angle];
			int[] tL = new int[3 * angle];




			uvL[0] = new Vector2(1, 1);
			uvL[1] = new Vector2(0, 1);
			uvL[2] = new Vector2(1, 0);

			nL[0] = -Vector3.up;
			nL[1] = -Vector3.up;
			nL[2] = -Vector3.up;


			Vector3 v1 = meshpoint[0].transform.position;
			Vector3 v2 = meshpoint[1].transform.position;
			Vector3 v3 = meshpoint[2].transform.position;



			vL[0] = v1;
			vL[1] = v2;
			vL[2] = v3;

			tL[0] = 0;
			tL[1] = 1;
			tL[2] = 2;

			for (int i = 4; i <= angle; i++)
			{

				Vector3 v4 = meshpoint[i - 1].transform.position;

				vL[i - 1] = v4;


				if (i % 3 == 2)
				{
					uvL[i - 1] = new Vector2(0, 1);
				}
				else if (i % 3 == 1)
				{
					uvL[i - 2] = new Vector2(1, 0);
				}
				else
				{
					uvL[i - 2] = new Vector2(0, 1);
				}



				nL[i - 1] = -Vector3.up;

				tL[i - 1 + (i - 4) * 2] = 0;
				tL[i + (i - 4) * 2] = i - 2;
				tL[(i + 1) + (i - 4) * 2] = i - 1;

			}


			meshL.vertices = vL;
			meshL.triangles = tL;
			meshL.normals = nL;
			meshL.uv = uvL;

			meshh = TTL;

		}*/
}
