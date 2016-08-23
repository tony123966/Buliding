
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RoofIcon : MonoBehaviour {

	public List<GameObject> controlPointList = new List<GameObject>();

	[HideInInspector]
	public Mesh mesh;
	GameObject ca1;
	GameObject ca2;

	void Start () 
    {

		ca1 = new GameObject();
		ca2 = new GameObject();
		ca1.transform.parent=transform;
		ca2.transform.parent=transform;
		
		ca1.AddComponent<Catline>().controlPointList = controlPointList;	
		ca1.GetComponent<Catline>().SetCatmullRom();

		ca2.AddComponent<Catline>().controlPointList.Add(controlPointList[0]);
		for(int i=0;i<controlPointList.Count;i++)
		{
			controlPointList[i].transform.parent = ca1.transform;
		}
		for(int i=1;i<ca1.GetComponent<Catline>().controlPointList.Count;i++)
		{
			GameObject target = ca1.GetComponent<Catline>().controlPointList[i];
			Vector3 v1=ca1.GetComponent<Catline>().controlPointList[0].transform.position;
			Vector3 v2 = target.transform.position;
			GameObject clone = Instantiate(target, new Vector3(v2.x - 2 * (v2.x - v1.x), v2.y, v2.z), target.transform.rotation) as GameObject;
			clone.transform.parent = ca2.transform;
			ca2.GetComponent<Catline>().controlPointList.Add(clone);
		}
		ca2.GetComponent<Catline>().SetCatmullRom();
		SetMesh();
	}

    void SetMesh()
    {

		if(controlPointList.Count<2)return;

        int rson = ca1.GetComponent<Catline>().innerPointList.Count;

        float uvR = (1 / (float)rson);

		GameObject TTL = new GameObject();

		TTL.transform.parent = transform;
		TTL.name = ("iconMesh");
		TTL.AddComponent<MeshFilter>();
		TTL.AddComponent<MeshRenderer>();
		TTL.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;

		mesh = TTL.GetComponent<MeshFilter>().mesh;

		mesh.Clear();

		Vector3[] v = new Vector3[2 * rson];
        Vector3[] n = new Vector3[2 * rson];
        Vector2[] uv = new Vector2[2 * rson];
        int[] t = new int[6 * rson];

        for (int j = 0; j <= rson-1; j++)
        {
            if (j == 0)
            {

				Vector3 v1 = ca1.GetComponent<Catline>().innerPointList[j];
				Vector3 v2 = ca2.GetComponent<Catline>().innerPointList[j+1];
				Vector3 v3 = ca1.GetComponent<Catline>().innerPointList[j+1];

                v[0] = (v1);
                v[1] = (v2);
                v[2] = (v3);

                uv[0] = new Vector2(0, 1);
                uv[1] = new Vector2(0, 1 - uvR);
                uv[2] = new Vector2(uvR, 1 - uvR);

                n[0] = -Vector3.forward;
                n[1] = -Vector3.forward;
                n[2] = -Vector3.forward;


                t[0] = j + 2;
                t[1] = j + 1;
                t[2] = j;
            }
            else if (j == 1)
            { }
            else
            {
				Vector3 v3 = ca2.GetComponent<Catline>().innerPointList[j];
				Vector3 v4 = ca1.GetComponent<Catline>().innerPointList[j];

                v[2 * j - 1] = (v3);
                v[2 * j] = (v4);


                if (j == rson - 1)
                {
                    uv[2 * j - 1] = new Vector2(0, 0);
                    uv[2 * j] = new Vector2(1, 0);
                }
                else
                {
                    uv[2 * j - 1] = new Vector2(0, 1 - j * uvR);

                    uv[2 * j] = new Vector2(j * uvR, 1 - j * uvR);

                }
                t[2 + 6 * (j - 1) - 5] = 2 * j - 1;
                t[2 + 6 * (j - 1) - 4] = 2 * j - 3;
                t[2 + 6 * (j - 1) - 3] = 2 * j - 2;
                t[2 + 6 * (j - 1) - 2] = 2 * j;
                t[2 + 6 * (j - 1) - 1] = 2 * j - 1;
                t[2 + 6 * (j - 1)] = 2 * j - 2;

            }
            mesh.vertices = v;
            mesh.triangles = t;
            mesh.normals = n;
            mesh.uv = uv;
        }

    }
	public void ResetMesh() 
	{
		ca1.GetComponent<Catline>().SetCatmullRom();
		ca2.GetComponent<Catline>().SetCatmullRom();
		SetMesh();
	}
}
