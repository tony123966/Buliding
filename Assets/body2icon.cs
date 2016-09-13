using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class body2icon : MonoBehaviour
{


	//public List<GameObject> list2column = new List<GameObject> ();
	public List<GameObject> cp2body = new List<GameObject>();
	public List<GameObject> cloum2body = new List<GameObject>();
	public List<GameObject> Left2body = new List<GameObject>();
	public List<GameObject> Right2body = new List<GameObject>();
	//just try two point



	public DragItemController dragitemcontroller;
	public Movement movement;
	public Mesh cylinderMesh;
	public Material lineMat;

	public GameObject R_up_point;
	public GameObject R_down_point;
	public GameObject L_up_point;
	public GameObject L_down_point;


	float ini_bodydis;
	float chang_bodydis;
	public float ratio_bodydis;

	GameObject frieze;
	GameObject frieze_ldpoint;
	GameObject frieze_rdpoint;
	MeshFilter frieze_filter;
	public bool isfrieze;
	int frieze_count = 0;
	public float frieze_height;




	GameObject balustrade;
	GameObject balustrade_lupoint;
	GameObject balustrade_rupoint;
	MeshFilter balustrade_filter;
	public bool isbalustrade;
	int balustrade_count = 0;
	public float balustrade_height;

	public float ini_cylinderH;


	Vector3 frieze_ru;
	Vector3 frieze_rd;
	Vector3 frieze_lu;
	Vector3 frieze_ld;
	Vector3 balustrade_rd;
	Vector3 balustrade_ru;
	Vector3 balustrade_ld;
	Vector3 balustrade_lu;



	public float radius = 0.01f;

	GameObject R_cylinder;
	GameObject L_cylinder;

	Vector3 tmpscale;
	// Use this for initialization
	void Start()
	{
		dragitemcontroller = GameObject.Find("DragItemController").GetComponent<DragItemController>();
	}
	void Awake()
	{


		movement = GameObject.Find("Movement").GetComponent<Movement>();
		movement.verlist.AddRange(cp2body);

		R_cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		L_cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

		R_cylinder.transform.parent = this.gameObject.transform;
		L_cylinder.transform.parent = this.gameObject.transform;

		float dis = Vector3.Distance(R_up_point.transform.position, R_down_point.transform.position);
		dis = dis / 2;
		R_cylinder.transform.localScale = new Vector3(radius, dis, radius);
		R_cylinder.transform.position = new Vector3(R_up_point.transform.position.x, R_up_point.transform.position.y - dis, R_up_point.transform.position.z);
		L_cylinder.transform.localScale = new Vector3(radius, dis, radius);
		L_cylinder.transform.position = new Vector3(L_up_point.transform.position.x, L_up_point.transform.position.y - dis, L_up_point.transform.position.z);

		cloum2body.Add(R_cylinder);
		cloum2body.Add(L_cylinder);
		movement.horlist.AddRange(cloum2body);

		R_cylinder.tag = "Cylinder";
		L_cylinder.tag = "Cylinder";
		Left2body.Add(L_cylinder);
		Left2body.Add(L_down_point);
		Left2body.Add(L_up_point);
		Right2body.Add(R_cylinder);
		Right2body.Add(R_down_point);
		Right2body.Add(R_up_point);
		ini_bodydis = R_up_point.transform.position.x - L_up_point.transform.position.x;
		ini_bodydis = Mathf.Abs(ini_bodydis);
		ini_bodydis = ini_bodydis / 2;

		frieze_height = 0.05f;
		balustrade_height = 0.05f;
		ini_cylinderH = R_up_point.transform.position.y - R_down_point.transform.position.y;

		//print ("Frieze");





		//
	}
	// Update is called once per frame
	void Update()
	{
	}
	public void adjPos(Vector2 mospos_)
	{
		if (dragitemcontroller.chooseObj.name == "RU" || dragitemcontroller.chooseObj.name == "LU")
		{
			float dis2 = (dragitemcontroller.chooseObj.transform.position.y - R_down_point.transform.position.y);
			dis2 = dis2 / 2;
			dis2 = Mathf.Abs(dis2);

			float tmp_y = dragitemcontroller.chooseObj.transform.position.y;
			//update point
			R_up_point.transform.position = new Vector3(R_up_point.transform.position.x, tmp_y, R_up_point.transform.position.z);
			L_up_point.transform.position = new Vector3(L_up_point.transform.position.x, tmp_y, L_up_point.transform.position.z);






			R_cylinder.transform.localScale = new Vector3(radius, dis2, radius);
			R_cylinder.transform.position = new Vector3(R_up_point.transform.position.x, R_up_point.transform.position.y - dis2, R_up_point.transform.position.z);
			L_cylinder.transform.localScale = new Vector3(radius, dis2, radius);
			L_cylinder.transform.position = new Vector3(L_up_point.transform.position.x, L_up_point.transform.position.y - dis2, L_up_point.transform.position.z);
			//adjmesh (inifrieze_height);
			if (isfrieze)
			{
				frieze_rdpoint.transform.position = new Vector3(R_up_point.transform.position.x, tmp_y - frieze_height, R_up_point.transform.position.z);
				frieze_ldpoint.transform.position = new Vector3(L_up_point.transform.position.x, tmp_y - frieze_height, L_up_point.transform.position.z);
				adjmesh(frieze_height, frieze_filter.mesh);

			}







		}
		else if (dragitemcontroller.chooseObj.name == "RD" || dragitemcontroller.chooseObj.name == "LD")
		{
			float dis2 = (dragitemcontroller.chooseObj.transform.position.y - R_up_point.transform.position.y);
			dis2 = dis2 / 2;
			dis2 = Mathf.Abs(dis2);
			float tmp_y = dragitemcontroller.chooseObj.transform.position.y;
			//update point
			R_down_point.transform.position = new Vector3(R_down_point.transform.position.x, tmp_y, R_down_point.transform.position.z);
			L_down_point.transform.position = new Vector3(L_down_point.transform.position.x, tmp_y, R_down_point.transform.position.z);

			R_cylinder.transform.localScale = new Vector3(radius, dis2, radius);
			R_cylinder.transform.position = new Vector3(R_up_point.transform.position.x, R_up_point.transform.position.y - dis2, R_up_point.transform.position.z);
			L_cylinder.transform.localScale = new Vector3(radius, dis2, radius);
			L_cylinder.transform.position = new Vector3(L_up_point.transform.position.x, L_up_point.transform.position.y - dis2, L_up_point.transform.position.z);


		}
		else if (dragitemcontroller.chooseObj.name == "Cylinder")
		{

			float dis = 0.0f;

			if (dragitemcontroller.chooseObj == R_cylinder)
			{
				dis = (dragitemcontroller.chooseObj.transform.position.x - R_up_point.transform.position.x);
				for (int i = 0; i < Right2body.Count; i++)
				{
					Right2body[i].transform.position = new Vector3(mospos_.x, Right2body[i].transform.position.y, Right2body[i].transform.position.z);
					Left2body[i].transform.position = new Vector3(Left2body[i].transform.position.x - (dis), Left2body[i].transform.position.y, Left2body[i].transform.position.z);

				}
				if (isfrieze)
				{
					adjmesh(frieze_height, frieze_filter.mesh);

				}
				if (isbalustrade)
				{
					adjmesh_balustrade(balustrade_height, balustrade_filter.mesh);

				}

			}
			else
			{
				dis = (dragitemcontroller.chooseObj.transform.position.x - L_up_point.transform.position.x);

				for (int i = 0; i < Left2body.Count; i++)
				{
					Left2body[i].transform.position = new Vector3(mospos_.x, Left2body[i].transform.position.y, Left2body[i].transform.position.z);
					Right2body[i].transform.position = new Vector3(Right2body[i].transform.position.x - (dis), Right2body[i].transform.position.y, Right2body[i].transform.position.z);

				}
				if (isfrieze)
				{
					adjmesh(frieze_height, frieze_filter.mesh);

				}
				if (isbalustrade)
				{
					adjmesh_balustrade(balustrade_height, balustrade_filter.mesh);

				}

			}
			chang_bodydis = dis;
			ratio_bodydis = chang_bodydis / ini_bodydis;

		}
		else if (dragitemcontroller.chooseObj.name == "FRD" || dragitemcontroller.chooseObj.name == "FLD")
		{

			float tmp_y = dragitemcontroller.chooseObj.transform.position.y;
			frieze_ldpoint.transform.position = new Vector3(frieze_ldpoint.transform.position.x, tmp_y, frieze_ldpoint.transform.position.z);
			frieze_rdpoint.transform.position = new Vector3(frieze_rdpoint.transform.position.x, tmp_y, frieze_rdpoint.transform.position.z);
			frieze_height = R_up_point.transform.position.y - frieze_rdpoint.transform.position.y;

			adjmesh(frieze_height, frieze_filter.mesh);

            //***
            ratio_bodydis =0;

		}
		else if (dragitemcontroller.chooseObj.name == "BRU" || dragitemcontroller.chooseObj.name == "BLU")
		{ //balustrade

			float tmp_y = dragitemcontroller.chooseObj.transform.position.y;
			balustrade_lupoint.transform.position = new Vector3(balustrade_lupoint.transform.position.x, tmp_y, balustrade_lupoint.transform.position.z);
			balustrade_rupoint.transform.position = new Vector3(balustrade_rupoint.transform.position.x, tmp_y, balustrade_rupoint.transform.position.z);
			balustrade_height = balustrade_rupoint.transform.position.y - R_down_point.transform.position.y;

			adjmesh_balustrade(balustrade_height, balustrade_filter.mesh);

            //***
            ratio_bodydis = 0;

		}
	}
	public void UpdateFunction(string objName, int count)
	{
		switch (objName)
		{
			case "Frieze":
				if (frieze_count < count)
					Createfrieze(frieze_height); // mesh製造機
				frieze_count++;
				break;
			case "Balustrade":
				if (balustrade_count < count)
					Createbalustrade(balustrade_height); // mesh製造機
				balustrade_count++;
				break;


		}
	}
	public void addpoint()
	{


		movement.verlist.AddRange(cp2body);
		movement.horlist.AddRange(cloum2body);

	}
	void Createfrieze(float height)
	{

		isfrieze = true;
		frieze = new GameObject("frieze_mesh");
		Vector3 h = new Vector3(0.0f, height, 0.0f);
		Vector3 frieze_ru = R_up_point.transform.position;
		Vector3 frieze_lu = L_up_point.transform.position;
		Vector3 frieze_rd = frieze_ru - h;
		Vector3 frieze_ld = frieze_lu - h;
		frieze.transform.parent = this.gameObject.transform;
		frieze_filter = frieze.AddComponent<MeshFilter>();
		frieze_filter.mesh = CreatRecMesh(frieze_lu, frieze_ru, frieze_rd, frieze_ld, null);

		//frieze cp
		frieze_rdpoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		frieze_rdpoint.tag = "ControlPoint";
		frieze_rdpoint.name = "FRD";
		frieze_rdpoint.transform.parent = frieze.gameObject.transform.parent;
		frieze_rdpoint.transform.localScale = R_down_point.transform.localScale;
		frieze_rdpoint.transform.position = frieze_rd;

		frieze_ldpoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		frieze_ldpoint.tag = "ControlPoint";
		frieze_ldpoint.name = "FLD";
		frieze_ldpoint.transform.parent = frieze.gameObject.transform.parent;
		frieze_ldpoint.transform.localScale = R_down_point.transform.localScale;
		frieze_ldpoint.transform.position = frieze_ld;

		cp2body.Add(frieze_ldpoint);
		cp2body.Add(frieze_rdpoint);
		movement.verlist.Add(frieze_ldpoint);
		movement.verlist.Add(frieze_rdpoint);
		Left2body.Add(frieze_ldpoint);
		Right2body.Add(frieze_rdpoint);


		MeshRenderer renderer = frieze.AddComponent<MeshRenderer>() as MeshRenderer;

	}
	void Createbalustrade(float height)
	{


		isbalustrade = true;
		balustrade = new GameObject("blustrade_mesh");
		Vector3 h = new Vector3(0.0f, height, 0.0f);
		Vector3 balustrade_rd = R_down_point.transform.position;
		Vector3 balustrade_ld = L_down_point.transform.position;
		Vector3 balustrade_ru = balustrade_rd + h;
		Vector3 balustrade_lu = balustrade_ld + h;
		balustrade.transform.parent = this.gameObject.transform;
		balustrade_filter = balustrade.AddComponent<MeshFilter>();
		balustrade_filter.mesh = CreatRecMesh(balustrade_lu, balustrade_ru, balustrade_rd, balustrade_ld, null);

		//frieze cp
		balustrade_rupoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		balustrade_rupoint.tag = "ControlPoint";
		balustrade_rupoint.name = "BRU";
		balustrade_rupoint.transform.parent = balustrade.gameObject.transform.parent;
		balustrade_rupoint.transform.localScale = R_down_point.transform.localScale;
		balustrade_rupoint.transform.position = balustrade_ru;

		balustrade_lupoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		balustrade_lupoint.tag = "ControlPoint";
		balustrade_lupoint.name = "BLU";
		balustrade_lupoint.transform.parent = balustrade.gameObject.transform.parent;
		balustrade_lupoint.transform.localScale = R_down_point.transform.localScale;
		balustrade_lupoint.transform.position = balustrade_lu;

		cp2body.Add(balustrade_lupoint);
		cp2body.Add(balustrade_rupoint);
		movement.verlist.Add(balustrade_lupoint);
		movement.verlist.Add(balustrade_rupoint);
		Left2body.Add(balustrade_lupoint);
		Right2body.Add(balustrade_rupoint);

		MeshRenderer renderer = balustrade.AddComponent<MeshRenderer>() as MeshRenderer;

	}

	Mesh adjmesh(float hh, Mesh w)
	{
		Vector3 h = new Vector3(0.0f, hh, 0.0f);
		w.Clear();
		frieze_ru = R_up_point.transform.position;
		frieze_lu = L_up_point.transform.position;
		frieze_rd = frieze_ru - h;
		frieze_ld = frieze_lu - h;
		w = CreatRecMesh(frieze_lu, frieze_ru, frieze_rd, frieze_ld, w);
		return w;


	}

	Mesh adjmesh_balustrade(float hh, Mesh w)
	{

		Vector3 h = new Vector3(0.0f, hh, 0.0f);
		w.Clear();
		balustrade_rd = R_down_point.transform.position;
		balustrade_ld = L_down_point.transform.position;
		balustrade_ru = balustrade_rd + h;
		balustrade_lu = balustrade_ld + h;
		w = CreatRecMesh(balustrade_lu, balustrade_ru, balustrade_rd, balustrade_ld, w);


		return w;

	}

	Mesh CreatRecMesh(Vector3 lu, Vector3 ru, Vector3 rd, Vector3 ld, Mesh ismesh)
	{
		Mesh m;

		if (!ismesh)
		{
			m = new Mesh();
		}
		else
		{
			m = ismesh;
		}

		m.vertices = new Vector3[] {

			lu,
			ru,
			rd,
			ld

		};
		m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
		m.RecalculateNormals();
		return m;
	}
}