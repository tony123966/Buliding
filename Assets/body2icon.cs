

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

	//
	int DoubleRoof_count = 0;
	GameObject DoubleRoof;
	MeshFilter DoubleRoof_filter;
	public bool isDoubleRoof;

	float DoubleRoof_height = 0.07f;
	float DoubleRoof_length = 0.03f;

	Vector3 MB0;
	Vector3 MB5;
	Vector3 MB2;
	Vector3 MB3;




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
			if(isDoubleRoof){
				
				MB2 = new Vector3 (MB2.x, tmp_y, MB2.z);
				MB3 = new Vector3 (MB3.x, tmp_y, MB3.z);
				float _upr = MB2.y + DoubleRoof_height;
				MB0 = new Vector3 (MB0.x,_upr, MB0.z);
				float _upl = MB3.y + DoubleRoof_height;
				MB5 = new Vector3 (MB5.x,_upl, MB5.z);


				adjmesh_DoubleRoof (DoubleRoof_filter.mesh);
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

			// ***
			ratio_bodydis =0;

		}
		else if (dragitemcontroller.chooseObj.name == "BRU" || dragitemcontroller.chooseObj.name == "BLU")
		{ //balustrade

			float tmp_y = dragitemcontroller.chooseObj.transform.position.y;
			balustrade_lupoint.transform.position = new Vector3(balustrade_lupoint.transform.position.x, tmp_y, balustrade_lupoint.transform.position.z);
			balustrade_rupoint.transform.position = new Vector3(balustrade_rupoint.transform.position.x, tmp_y, balustrade_rupoint.transform.position.z);
			balustrade_height = balustrade_rupoint.transform.position.y - R_down_point.transform.position.y;

			adjmesh_balustrade(balustrade_height, balustrade_filter.mesh);

			// ***
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
		case "DoubleRoof":
			if (DoubleRoof_count < count)
			CreateDoubleRoof ();
			DoubleRoof_count++;
			break;


		}
	}
	public void addpoint()
	{


		movement.verlist.AddRange(cp2body);
		movement.horlist.AddRange(cloum2body);

	}


	void CreateDoubleRoof(){


		isDoubleRoof = true;
		DoubleRoof = new GameObject ("DoubleRoof_mesh");
		MB2 = R_up_point.transform.position;
		MB3 = L_up_point.transform.position;
		DoubleRoof.transform.parent = this.gameObject.transform;
		DoubleRoof_filter = DoubleRoof.AddComponent<MeshFilter> ();

		//MutiBody CP

		float tmp =  MB2.y + DoubleRoof_height;
		MB0 = new Vector3 (MB2.x, tmp, MB2.z);
		MB5 = new Vector3 (MB3.x, tmp, MB3.z);
		MB2.x = MB2.x + DoubleRoof_height; 
		MB3.x = MB3.x - DoubleRoof_height;

		DoubleRoof_filter.mesh = CreatRecMesh (MB5,MB0,MB2,MB3,null);
			

		MeshRenderer renderer = DoubleRoof.AddComponent<MeshRenderer>() as MeshRenderer;

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
	Mesh adjmesh_DoubleRoof(Mesh w){
	
		w.Clear ();
		DoubleRoof_filter.mesh = CreatRecMesh (MB5,MB0,MB2,MB3,DoubleRoof_filter.mesh);
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


/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Column 
{
	public GameObject upPoint;
	public GameObject downPoint;
	public GameObject cylinderBody;
	public List<GameObject> allObjList = new List<GameObject>();//所有控制點
	public float height = 0;
	public float radius = 0.01f;
	public Column(GameObject upPoint, GameObject downPoint)
	{
		this.upPoint=upPoint;
		this.upPoint = downPoint;
		height = Vector3.Distance(upPoint.transform.position, downPoint.transform.position)/2.0f;

		cylinderBody = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		cylinderBody.transform.localScale = new Vector3(radius, height, radius);
		cylinderBody.transform.position = new Vector3(upPoint.transform.position.x, upPoint.transform.position.y - height, upPoint.transform.position.z);
		
		cylinderBody.tag = "Cylinder";

		allObjList.Add(cylinderBody);
		allObjList.Add(downPoint);
		allObjList.Add(upPoint);
	}
}
public class body2icon : MonoBehaviour
{


	public List<GameObject> controlPointList = new List<GameObject>();//所有控制點
	public Column leftColumn;
	public Column rightColumn;
	//just try two point

	private DragItemController dragitemcontroller;
	private Movement movement;

	public GameObject R_up_point;
	public GameObject R_down_point;
	public GameObject L_up_point;
	public GameObject L_down_point;


	float ini_bodydis;
	float chang_bodydis;
	public float ratio_bodydis;



	GameObject balustrade;
	GameObject balustrade_lupoint;
	GameObject balustrade_rupoint;
	MeshFilter balustrade_filter;
	public bool isbalustrade;
	int balustrade_count = 0;
	public float balustrade_height;

	public float ini_cylinderH;

	GameObject frieze;
	GameObject frieze_ldpoint;
	GameObject frieze_rdpoint;
	MeshFilter frieze_filter;
	public float frieze_height;

	Vector3 frieze_ru;
	Vector3 frieze_rd;
	Vector3 frieze_lu;
	Vector3 frieze_ld;
	int frieze_count = 0;
	public bool isfrieze;

	Vector3 balustrade_rd;
	Vector3 balustrade_ru;
	Vector3 balustrade_ld;
	Vector3 balustrade_lu;

	//
	int DoubleRoof_count = 0;
	GameObject DoubleRoof;
	MeshFilter DoubleRoof_filter;
	public bool isDoubleRoof;

	float DoubleRoof_height = 0.07f;
	float DoubleRoof_length = 0.03f;

	Vector3 MB0;
	Vector3 MB5;
	Vector3 MB2;
	Vector3 MB3;



	Vector3 tmpscale;
	// Use this for initialization
	void Start()
	{
		dragitemcontroller = GameObject.Find("DragItemController").GetComponent<DragItemController>();
	}
	void Awake()
	{

		movement = GameObject.Find("Movement").GetComponent<Movement>();
		movement.verlist.AddRange(controlPointList);
	
		leftColumn=new Column(controlPointList[0],controlPointList[1]);
		rightColumn=new Column(controlPointList[2],controlPointList[3]);

		leftColumn.cylinderBody.transform.parent = this.gameObject.transform;
		rightColumn.cylinderBody.transform.parent = this.gameObject.transform;
		
		movement.horlist.Add(leftColumn.cylinderBody);
		movement.horlist.Add(rightColumn.cylinderBody);
		
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

	}
	// Update is called once per frame
	void Update()
	{
	}
	public void adjPos(Vector2 mospos_)
	{
		ratio_bodydis = 0;
		if (dragitemcontroller.chooseObj.name == "RU" || dragitemcontroller.chooseObj.name == "LU")
		{

			float dis2 = (dragitemcontroller.chooseObj.transform.position.y - R_down_point.transform.position.y);
			dis2 = dis2 / 2;
			dis2 = Mathf.Abs(dis2);

			float tmp_y = dragitemcontroller.chooseObj.transform.position.y;
			//update point
			R_up_point.transform.position = new Vector3(R_up_point.transform.position.x, tmp_y, R_up_point.transform.position.z);
			L_up_point.transform.position = new Vector3(L_up_point.transform.position.x, tmp_y, L_up_point.transform.position.z);



			rightColumn.cylinderBody.transform.localScale = new Vector3(rightColumn.radius, dis2, rightColumn.radius);
			rightColumn.cylinderBody.transform.position = new Vector3(R_up_point.transform.position.x, R_up_point.transform.position.y - dis2, R_up_point.transform.position.z);
			leftColumn.cylinderBody.transform.localScale = new Vector3(leftColumn.radius, dis2, leftColumn.radius);
			leftColumn.cylinderBody.transform.position = new Vector3(L_up_point.transform.position.x, L_up_point.transform.position.y - dis2, L_up_point.transform.position.z);
			//adjmesh (inifrieze_height);
			if (isfrieze)
			{
				frieze_rdpoint.transform.position = new Vector3(R_up_point.transform.position.x, tmp_y - frieze_height, R_up_point.transform.position.z);
				frieze_ldpoint.transform.position = new Vector3(L_up_point.transform.position.x, tmp_y - frieze_height, L_up_point.transform.position.z);
				adjmesh(frieze_height, frieze_filter.mesh);

			}
			if (isDoubleRoof)
			{
				adjmesh_DoubleRoof(DoubleRoof_filter.mesh);
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
				if (isDoubleRoof) {

					adjmesh_DoubleRoof(DoubleRoof_filter.mesh);
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



		}
		else if (dragitemcontroller.chooseObj.name == "BRU" || dragitemcontroller.chooseObj.name == "BLU")
		{ //balustrade

			float tmp_y = dragitemcontroller.chooseObj.transform.position.y;
			balustrade_lupoint.transform.position = new Vector3(balustrade_lupoint.transform.position.x, tmp_y, balustrade_lupoint.transform.position.z);
			balustrade_rupoint.transform.position = new Vector3(balustrade_rupoint.transform.position.x, tmp_y, balustrade_rupoint.transform.position.z);
			balustrade_height = balustrade_rupoint.transform.position.y - R_down_point.transform.position.y;

			adjmesh_balustrade(balustrade_height, balustrade_filter.mesh);


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
			case "DoubleRoof":
				if (DoubleRoof_count < count)
					CreateDoubleRoof();
				DoubleRoof_count++;
				break;


		}
	}
	public void addpoint()
	{

		movement.verlist.AddRange(controlPointList);
		movement.horlist.AddRange(columnList);

	}


	void CreateDoubleRoof()
	{


		isDoubleRoof = true;
		DoubleRoof = new GameObject("DoubleRoof_mesh");
		MB2 = R_up_point.transform.position;
		MB3 = L_up_point.transform.position;
		DoubleRoof.transform.parent = this.gameObject.transform;
		DoubleRoof_filter = DoubleRoof.AddComponent<MeshFilter>();

		//MutiBody CP

		float tmp = MB2.y + DoubleRoof_height;
		MB0 = new Vector3(MB2.x, tmp, MB2.z);
		MB5 = new Vector3(MB3.x, tmp, MB3.z);
		MB2.x = MB2.x + DoubleRoof_height;
		MB3.x = MB3.x - DoubleRoof_height;

		DoubleRoof_filter.mesh = CreatRecMesh(MB5, MB0, MB2, MB3, null);


		MeshRenderer renderer = DoubleRoof.AddComponent<MeshRenderer>() as MeshRenderer;

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

		controlPointList.Add(frieze_ldpoint);
		controlPointList.Add(frieze_rdpoint);
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

		controlPointList.Add(balustrade_lupoint);
		controlPointList.Add(balustrade_rupoint);
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
	Mesh adjmesh_DoubleRoof(Mesh w)
	{

		MB2 = new Vector3(R_up_point.transform.position.x, R_up_point.transform.position.y, R_up_point.transform.position.z);
		MB3 = new Vector3(L_up_point.transform.position.x, L_up_point.transform.position.y, L_up_point.transform.position.z);
		float _upr = MB2.y + DoubleRoof_height;
		MB0 = new Vector3(MB0.x, _upr, MB0.z);
		float _upl = MB3.y + DoubleRoof_height;
		MB5 = new Vector3(MB5.x, _upl, MB5.z);
		MB2.x = MB2.x + DoubleRoof_height;
		MB3.x = MB3.x - DoubleRoof_height;
		w.Clear();
		DoubleRoof_filter.mesh = CreatRecMesh(MB5, MB0, MB2, MB3, DoubleRoof_filter.mesh);
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
}*/