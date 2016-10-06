/*


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

	public GameObject  rightColumn.upPoint;
	public GameObject  rightColumn.downPoint;
	public GameObject leftColumn.upPoint;
	public GameObject leftColumn.downPoint;


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

		float dis = Vector3.Distance( rightColumn.upPoint.transform.position,  rightColumn.downPoint.transform.position);
		dis = dis / 2;
		R_cylinder.transform.localScale = new Vector3(radius, dis, radius);
		R_cylinder.transform.position = new Vector3( rightColumn.upPoint.transform.position.x,  rightColumn.upPoint.transform.position.y - dis,  rightColumn.upPoint.transform.position.z);
		L_cylinder.transform.localScale = new Vector3(radius, dis, radius);
		L_cylinder.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, leftColumn.upPoint.transform.position.y - dis, leftColumn.upPoint.transform.position.z);

		cloum2body.Add(R_cylinder);
		cloum2body.Add(L_cylinder);
		movement.horlist.AddRange(cloum2body);

		R_cylinder.tag = "Cylinder";
		L_cylinder.tag = "Cylinder";
		Left2body.Add(L_cylinder);
		Left2body.Add(leftColumn.downPoint);
		Left2body.Add(leftColumn.upPoint);
		Right2body.Add(R_cylinder);
		Right2body.Add( rightColumn.downPoint);
		Right2body.Add( rightColumn.upPoint);
		ini_bodydis =  rightColumn.upPoint.transform.position.x - leftColumn.upPoint.transform.position.x;
		ini_bodydis = Mathf.Abs(ini_bodydis);
		ini_bodydis = ini_bodydis / 2;

		frieze_height = 0.05f;
		balustrade_height = 0.05f;
		ini_cylinderH =  rightColumn.upPoint.transform.position.y -  rightColumn.downPoint.transform.position.y;

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
			float dis2 = (dragitemcontroller.chooseObj.transform.position.y -  rightColumn.downPoint.transform.position.y);
			dis2 = dis2 / 2;
			dis2 = Mathf.Abs(dis2);

			float tmp_y = dragitemcontroller.chooseObj.transform.position.y;
			//update point
			 rightColumn.upPoint.transform.position = new Vector3( rightColumn.upPoint.transform.position.x, tmp_y,  rightColumn.upPoint.transform.position.z);
			leftColumn.upPoint.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, tmp_y, leftColumn.upPoint.transform.position.z);






			R_cylinder.transform.localScale = new Vector3(radius, dis2, radius);
			R_cylinder.transform.position = new Vector3( rightColumn.upPoint.transform.position.x,  rightColumn.upPoint.transform.position.y - dis2,  rightColumn.upPoint.transform.position.z);
			L_cylinder.transform.localScale = new Vector3(radius, dis2, radius);
			L_cylinder.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, leftColumn.upPoint.transform.position.y - dis2, leftColumn.upPoint.transform.position.z);
			//adjmesh (inifrieze_height);
			if (isfrieze)
			{
				frieze_rdpoint.transform.position = new Vector3( rightColumn.upPoint.transform.position.x, tmp_y - frieze_height,  rightColumn.upPoint.transform.position.z);
				frieze_ldpoint.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, tmp_y - frieze_height, leftColumn.upPoint.transform.position.z);
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
			float dis2 = (dragitemcontroller.chooseObj.transform.position.y -  rightColumn.upPoint.transform.position.y);
			dis2 = dis2 / 2;
			dis2 = Mathf.Abs(dis2);
			float tmp_y = dragitemcontroller.chooseObj.transform.position.y;
			//update point
			 rightColumn.downPoint.transform.position = new Vector3( rightColumn.downPoint.transform.position.x, tmp_y,  rightColumn.downPoint.transform.position.z);
			leftColumn.downPoint.transform.position = new Vector3(leftColumn.downPoint.transform.position.x, tmp_y,  rightColumn.downPoint.transform.position.z);

			R_cylinder.transform.localScale = new Vector3(radius, dis2, radius);
			R_cylinder.transform.position = new Vector3( rightColumn.upPoint.transform.position.x,  rightColumn.upPoint.transform.position.y - dis2,  rightColumn.upPoint.transform.position.z);
			L_cylinder.transform.localScale = new Vector3(radius, dis2, radius);
			L_cylinder.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, leftColumn.upPoint.transform.position.y - dis2, leftColumn.upPoint.transform.position.z);


		}
		else if (dragitemcontroller.chooseObj.name == "Cylinder")
		{

			float dis = 0.0f;

			if (dragitemcontroller.chooseObj == R_cylinder)
			{
				dis = (dragitemcontroller.chooseObj.transform.position.x -  rightColumn.upPoint.transform.position.x);
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
				dis = (dragitemcontroller.chooseObj.transform.position.x - leftColumn.upPoint.transform.position.x);

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
			frieze_height =  rightColumn.upPoint.transform.position.y - frieze_rdpoint.transform.position.y;

			adjmesh(frieze_height, frieze_filter.mesh);

			// ***
			ratio_bodydis =0;

		}
		else if (dragitemcontroller.chooseObj.name == "BRU" || dragitemcontroller.chooseObj.name == "BLU")
		{ //balustrade

			float tmp_y = dragitemcontroller.chooseObj.transform.position.y;
			balustrade_lupoint.transform.position = new Vector3(balustrade_lupoint.transform.position.x, tmp_y, balustrade_lupoint.transform.position.z);
			balustrade_rupoint.transform.position = new Vector3(balustrade_rupoint.transform.position.x, tmp_y, balustrade_rupoint.transform.position.z);
			balustrade_height = balustrade_rupoint.transform.position.y -  rightColumn.downPoint.transform.position.y;

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
		MB2 =  rightColumn.upPoint.transform.position;
		MB3 = leftColumn.upPoint.transform.position;
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
		Vector3 frieze_ru =  rightColumn.upPoint.transform.position;
		Vector3 frieze_lu = leftColumn.upPoint.transform.position;
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
		frieze_rdpoint.transform.localScale =  rightColumn.downPoint.transform.localScale;
		frieze_rdpoint.transform.position = frieze_rd;

		frieze_ldpoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		frieze_ldpoint.tag = "ControlPoint";
		frieze_ldpoint.name = "FLD";
		frieze_ldpoint.transform.parent = frieze.gameObject.transform.parent;
		frieze_ldpoint.transform.localScale =  rightColumn.downPoint.transform.localScale;
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
		Vector3 balustrade_rd =  rightColumn.downPoint.transform.position;
		Vector3 balustrade_ld = leftColumn.downPoint.transform.position;
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
		balustrade_rupoint.transform.localScale =  rightColumn.downPoint.transform.localScale;
		balustrade_rupoint.transform.position = balustrade_ru;

		balustrade_lupoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		balustrade_lupoint.tag = "ControlPoint";
		balustrade_lupoint.name = "BLU";
		balustrade_lupoint.transform.parent = balustrade.gameObject.transform.parent;
		balustrade_lupoint.transform.localScale =  rightColumn.downPoint.transform.localScale;
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
		frieze_ru =  rightColumn.upPoint.transform.position;
		frieze_lu = leftColumn.upPoint.transform.position;
		frieze_rd = frieze_ru - h;
		frieze_ld = frieze_lu - h;
		w = CreatRecMesh(frieze_lu, frieze_ru, frieze_rd, frieze_ld, w);
		return w;


	}

	Mesh adjmesh_balustrade(float hh, Mesh w)
	{

		Vector3 h = new Vector3(0.0f, hh, 0.0f);
		w.Clear();
		balustrade_rd =  rightColumn.downPoint.transform.position;
		balustrade_ld = leftColumn.downPoint.transform.position;
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
}*/



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DoubleRoof
{
	public GameObject doubleRoofBody = null;
	public Vector3 rightUpPoint;
	public Vector3 rightDownPoint;
	public Vector3 leftUpPoint;
	public Vector3 leftDownPoint;
	public MeshFilter mFilter;
	public DoubleRoof(string objName, Column rightColumn, Column leftColumn, float doubleRoofHeight, float doubleRoofWidth)
	{
		doubleRoofBody = new GameObject(objName);
		mFilter = doubleRoofBody.AddComponent<MeshFilter>();
		Debug.Log(rightColumn.cylinderBody.name);
		rightDownPoint = rightColumn.upPoint.transform.position;
		leftDownPoint = leftColumn.upPoint.transform.position;

		float tmp = rightDownPoint.y + doubleRoofHeight;
		rightUpPoint = new Vector3(rightDownPoint.x, tmp, rightDownPoint.z);
		leftUpPoint = new Vector3(leftDownPoint.x, tmp, leftDownPoint.z);

		rightDownPoint.x = rightDownPoint.x + doubleRoofWidth;
		leftDownPoint.x = leftDownPoint.x - doubleRoofWidth;

		MeshRenderer renderer = doubleRoofBody.AddComponent<MeshRenderer>() as MeshRenderer;
		mFilter.mesh = CreatRecMesh(leftUpPoint, rightUpPoint, rightDownPoint, leftDownPoint, null);
	}
	public void AdjPos(Column rightColumn, Column leftColumn, float doubleRoofHeight, float doubleRoofWidth)
	{
		rightDownPoint = new Vector3(rightColumn.upPoint.transform.position.x, rightColumn.upPoint.transform.position.y, rightColumn.upPoint.transform.position.z);
		leftDownPoint = new Vector3(leftColumn.upPoint.transform.position.x, leftColumn.upPoint.transform.position.y, leftColumn.upPoint.transform.position.z);

		float _upr = rightDownPoint.y + doubleRoofHeight;
		rightUpPoint = new Vector3(rightUpPoint.x, _upr, rightUpPoint.z);
		float _upl = leftDownPoint.y + doubleRoofHeight;
		leftUpPoint = new Vector3(leftUpPoint.x, _upl, leftUpPoint.z);
		rightDownPoint.x = rightDownPoint.x + doubleRoofWidth;
		leftDownPoint.x = leftDownPoint.x - doubleRoofWidth;

		mFilter.mesh.Clear();
		mFilter.mesh = CreatRecMesh(leftUpPoint, rightUpPoint, rightDownPoint, leftDownPoint, mFilter.mesh);
	}
	public Mesh CreatRecMesh(Vector3 lu, Vector3 ru, Vector3 rd, Vector3 ld, Mesh ismesh)
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
public class Column
{
	public GameObject upPoint = null;
	public GameObject downPoint = null;
	public GameObject cylinderBody = null;
	public GameObject balustradePoint = null;
	public GameObject friezePoint = null;
	public List<GameObject> allObjList = new List<GameObject>();//所有控制點
	public float radius = 0.01f;
	public Column(GameObject upPoint, GameObject downPoint, float columnHeight)
	{
		this.upPoint = upPoint;
		this.downPoint = downPoint;

		this.cylinderBody = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		this.cylinderBody.transform.localScale = new Vector3(radius, columnHeight/2.0f, radius);
		this.cylinderBody.transform.position = new Vector3(upPoint.transform.position.x, upPoint.transform.position.y - columnHeight/2.0f, upPoint.transform.position.z);

		this.cylinderBody.tag = "Cylinder";

		this.allObjList.Add(cylinderBody);
		this.allObjList.Add(downPoint);
		this.allObjList.Add(upPoint);

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


	public float ini_bodydis;
	public float chang_bodydis;
	public float ratio_bodydis;

	public float ini_cylinderHeight;
	public float chang_cylinderHeight;
	public float ratio_cylinderHeight;

	public float ini_friezeHeight;
	public float ini_balustradeHeight;
	public float ini_doubleRoofWidth;
	public float ini_doubleRoofHeight;


	DoubleRoof doubleRoof;

	MeshFilter balustradeFilter;
	MeshFilter friezeFilter;

	public bool isbalustrade;
	public bool isfrieze;
	public bool isDoubleRoof;

	public float cylinderHeight;
	public float friezeHeight;
	public float balustradeHeight;


	// Use this for initialization

	void Start()
	{
		dragitemcontroller = GameObject.Find("DragItemController").GetComponent<DragItemController>();
		movement = GameObject.Find("Movement").GetComponent<Movement>();
		movement.verlist.AddRange(controlPointList);

		cylinderHeight=ini_cylinderHeight = Vector3.Distance(controlPointList[0].transform.position, controlPointList[1].transform.position);

		leftColumn = new Column(controlPointList[0], controlPointList[1], ini_cylinderHeight);
		rightColumn = new Column(controlPointList[2], controlPointList[3], ini_cylinderHeight);

		leftColumn.cylinderBody.transform.parent = this.gameObject.transform;
		rightColumn.cylinderBody.transform.parent = this.gameObject.transform;

		movement.horlist.Add(leftColumn.cylinderBody);
		movement.horlist.Add(rightColumn.cylinderBody);


		friezeHeight = ini_friezeHeight = 0.4f * ini_cylinderHeight;
		balustradeHeight = ini_balustradeHeight = 0.4f * ini_cylinderHeight;
		ini_doubleRoofHeight = 0.4f * ini_cylinderHeight;
		ini_doubleRoofWidth = 0.3f * ini_cylinderHeight;

		ini_bodydis = rightColumn.upPoint.transform.position.x - leftColumn.upPoint.transform.position.x;
		ini_bodydis = ini_bodydis / 2.0f;

	}

	public void adjPos()
	{
		ratio_bodydis = 0;
		Vector2 tmp = dragitemcontroller.chooseObj.transform.position;
		if (dragitemcontroller.chooseObj == rightColumn.upPoint || dragitemcontroller.chooseObj == leftColumn.upPoint)//RU LU
		{
			float dis = (tmp.y - rightColumn.upPoint.transform.position.y);
			cylinderHeight = (dragitemcontroller.chooseObj.transform.position.y - rightColumn.downPoint.transform.position.y) ;
			cylinderHeight = Mathf.Abs(cylinderHeight);
			//update point
			rightColumn.upPoint.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, tmp.y, rightColumn.upPoint.transform.position.z);
			leftColumn.upPoint.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, tmp.y, leftColumn.upPoint.transform.position.z);


			rightColumn.cylinderBody.transform.localScale = new Vector3(rightColumn.radius, cylinderHeight / 2.0f, rightColumn.radius);
			rightColumn.cylinderBody.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, rightColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, rightColumn.upPoint.transform.position.z);
			leftColumn.cylinderBody.transform.localScale = new Vector3(leftColumn.radius, cylinderHeight / 2.0f, leftColumn.radius);
			leftColumn.cylinderBody.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, leftColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, leftColumn.upPoint.transform.position.z);

			if (isfrieze)
			{
				rightColumn.friezePoint.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, tmp.y - friezeHeight, rightColumn.upPoint.transform.position.z);
				leftColumn.friezePoint.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, tmp.y - friezeHeight, leftColumn.upPoint.transform.position.z);
				AdjMeshFrieze();
			}
			if (isDoubleRoof)
			{
				AdjMeshDoubleRoof();
			}
			chang_cylinderHeight=dis;
			ratio_cylinderHeight = chang_cylinderHeight / ini_cylinderHeight;
		}
		else if (dragitemcontroller.chooseObj == rightColumn.downPoint || dragitemcontroller.chooseObj == leftColumn.downPoint)//RD  LD
		{
			float dis = (tmp.y - rightColumn.downPoint.transform.position.y);

			cylinderHeight = (dragitemcontroller.chooseObj.transform.position.y - rightColumn.upPoint.transform.position.y);
			cylinderHeight = Mathf.Abs(cylinderHeight);

			//update point
			rightColumn.downPoint.transform.position = new Vector3(rightColumn.downPoint.transform.position.x, tmp.y, rightColumn.downPoint.transform.position.z);
			leftColumn.downPoint.transform.position = new Vector3(leftColumn.downPoint.transform.position.x, tmp.y, rightColumn.downPoint.transform.position.z);

			rightColumn.cylinderBody.transform.localScale = new Vector3(rightColumn.radius, cylinderHeight/2.0f, rightColumn.radius);
			rightColumn.cylinderBody.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, rightColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, rightColumn.upPoint.transform.position.z);
			leftColumn.cylinderBody.transform.localScale = new Vector3(leftColumn.radius, cylinderHeight / 2.0f, leftColumn.radius);
			leftColumn.cylinderBody.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, leftColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, leftColumn.upPoint.transform.position.z);

			if (isbalustrade)
			{
				rightColumn.balustradePoint.transform.position = new Vector3(rightColumn.downPoint.transform.position.x, tmp.y + balustradeHeight, rightColumn.upPoint.transform.position.z);
				leftColumn.balustradePoint.transform.position = new Vector3(leftColumn.downPoint.transform.position.x, tmp.y + balustradeHeight, leftColumn.upPoint.transform.position.z);
				AdjMeshBalustrade();
			}
			chang_cylinderHeight = dis;
			ratio_cylinderHeight = chang_cylinderHeight / ini_cylinderHeight;
		}
		else if (dragitemcontroller.chooseObj == rightColumn.cylinderBody)
		{
			float dis = (tmp.x - rightColumn.upPoint.transform.position.x);
			for (int i = 0; i < rightColumn.allObjList.Count; i++)
			{
				rightColumn.allObjList[i].transform.position = new Vector3(tmp.x, rightColumn.allObjList[i].transform.position.y, rightColumn.allObjList[i].transform.position.z);
				leftColumn.allObjList[i].transform.position = new Vector3(leftColumn.allObjList[i].transform.position.x - (dis), leftColumn.allObjList[i].transform.position.y, leftColumn.allObjList[i].transform.position.z);
			}
			if (isfrieze)
			{
				AdjMeshFrieze();
			}
			if (isbalustrade)
			{
				AdjMeshBalustrade();
			}
			if (isDoubleRoof)
			{
				AdjMeshDoubleRoof();
			}
			chang_bodydis = dis;
			ratio_bodydis = chang_bodydis / ini_bodydis;
		}
		else if (dragitemcontroller.chooseObj == leftColumn.cylinderBody)
		{
			float dis = (tmp.x - leftColumn.upPoint.transform.position.x);

			for (int i = 0; i < leftColumn.allObjList.Count; i++)
			{
				leftColumn.allObjList[i].transform.position = new Vector3(tmp.x, leftColumn.allObjList[i].transform.position.y, leftColumn.allObjList[i].transform.position.z);
				rightColumn.allObjList[i].transform.position = new Vector3(rightColumn.allObjList[i].transform.position.x - (dis), rightColumn.allObjList[i].transform.position.y, rightColumn.allObjList[i].transform.position.z);

			}
			if (isfrieze)
			{
				AdjMeshFrieze();
			}
			if (isbalustrade)
			{
				AdjMeshBalustrade();
			}
			if (isDoubleRoof)
			{
				AdjMeshDoubleRoof();
			}
			chang_bodydis = dis;
			ratio_bodydis = chang_bodydis / ini_bodydis;
		}
		else if (dragitemcontroller.chooseObj == rightColumn.friezePoint || dragitemcontroller.chooseObj == leftColumn.friezePoint)
		{

			leftColumn.friezePoint.transform.position = new Vector3(leftColumn.friezePoint.transform.position.x, tmp.y, leftColumn.friezePoint.transform.position.z);
			rightColumn.friezePoint.transform.position = new Vector3(rightColumn.friezePoint.transform.position.x, tmp.y, rightColumn.friezePoint.transform.position.z);

			AdjMeshFrieze();


		}
		else if (dragitemcontroller.chooseObj == rightColumn.balustradePoint || dragitemcontroller.chooseObj == leftColumn.balustradePoint)
		{ //balustrade

			leftColumn.balustradePoint.transform.position = new Vector3(leftColumn.balustradePoint.transform.position.x, tmp.y, leftColumn.balustradePoint.transform.position.z);
			rightColumn.balustradePoint.transform.position = new Vector3(rightColumn.balustradePoint.transform.position.x, tmp.y, rightColumn.balustradePoint.transform.position.z);

			AdjMeshBalustrade();

		}
	}
	public void UpdateFunction(string objName)
	{
		switch (objName)
		{
			case "Frieze":
				if (!isfrieze)
					Createfrieze(); // mesh製造機
				break;
			case "Balustrade":
				if (!isbalustrade)
					Createbalustrade(); // mesh製造機
				break;
			case "DoubleRoof":
				if (!isDoubleRoof)
					doubleRoof = CreateDoubleRoof();
				break;


		}
	}
	public void addpoint()
	{
		movement.verlist.AddRange(controlPointList);
		movement.horlist.Add(rightColumn.cylinderBody);
		movement.horlist.Add(leftColumn.cylinderBody);
	}


	DoubleRoof CreateDoubleRoof()
	{
		DoubleRoof doubleRoof = new DoubleRoof("DoubleRoof_mesh", rightColumn, leftColumn, ini_doubleRoofHeight, ini_doubleRoofWidth);

		doubleRoof.doubleRoofBody.transform.parent = this.gameObject.transform;


		isDoubleRoof = true;

		return doubleRoof;
	}

	void Createfrieze()
	{
		Vector3 h = new Vector3(0.0f, ini_friezeHeight, 0.0f);
		Vector3 frieze_ru = rightColumn.upPoint.transform.position;
		Vector3 frieze_lu = leftColumn.upPoint.transform.position;
		Vector3 frieze_rd = frieze_ru - h;
		Vector3 frieze_ld = frieze_lu - h;

		GameObject frieze = new GameObject("frieze_mesh");
		frieze.transform.parent = this.gameObject.transform;
		friezeFilter = frieze.AddComponent<MeshFilter>();
		friezeFilter.mesh = CreatRecMesh(frieze_lu, frieze_ru, frieze_rd, frieze_ld, null);

		//frieze cp
		rightColumn.friezePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		rightColumn.friezePoint.tag = "ControlPoint";
		rightColumn.friezePoint.name = "FRD";
		rightColumn.friezePoint.transform.parent = frieze.gameObject.transform.parent;
		rightColumn.friezePoint.transform.localScale = rightColumn.downPoint.transform.localScale;
		rightColumn.friezePoint.transform.position = frieze_rd;

		leftColumn.friezePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		leftColumn.friezePoint.tag = "ControlPoint";
		leftColumn.friezePoint.name = "FLD";
		leftColumn.friezePoint.transform.parent = frieze.gameObject.transform.parent;
		leftColumn.friezePoint.transform.localScale = rightColumn.downPoint.transform.localScale;
		leftColumn.friezePoint.transform.position = frieze_ld;

		controlPointList.Add(rightColumn.friezePoint);
		controlPointList.Add(leftColumn.friezePoint);

		movement.verlist.Add(rightColumn.friezePoint);
		movement.verlist.Add(leftColumn.friezePoint);

		rightColumn.allObjList.Add(rightColumn.friezePoint);
		leftColumn.allObjList.Add(leftColumn.friezePoint);



		MeshRenderer renderer = frieze.AddComponent<MeshRenderer>() as MeshRenderer;

		isfrieze = true;
	}
	void Createbalustrade()
	{
		Vector3 h = new Vector3(0.0f, ini_balustradeHeight, 0.0f);
		Vector3 balustrade_rd = rightColumn.downPoint.transform.position;
		Vector3 balustrade_ld = leftColumn.downPoint.transform.position;
		Vector3 balustrade_ru = balustrade_rd + h;
		Vector3 balustrade_lu = balustrade_ld + h;

		GameObject balustrade = new GameObject("blustrade_mesh");
		balustrade.transform.parent = this.gameObject.transform;
		balustradeFilter = balustrade.AddComponent<MeshFilter>();
		balustradeFilter.mesh = CreatRecMesh(balustrade_lu, balustrade_ru, balustrade_rd, balustrade_ld, null);

		//frieze cp
		rightColumn.balustradePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		rightColumn.balustradePoint.tag = "ControlPoint";
		rightColumn.balustradePoint.name = "BRU";
		rightColumn.balustradePoint.transform.parent = balustrade.gameObject.transform.parent;
		rightColumn.balustradePoint.transform.localScale = rightColumn.downPoint.transform.localScale;
		rightColumn.balustradePoint.transform.position = balustrade_ru;

		leftColumn.balustradePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		leftColumn.balustradePoint.tag = "ControlPoint";
		leftColumn.balustradePoint.name = "BLU";
		leftColumn.balustradePoint.transform.parent = balustrade.gameObject.transform.parent;
		leftColumn.balustradePoint.transform.localScale = rightColumn.downPoint.transform.localScale;
		leftColumn.balustradePoint.transform.position = balustrade_lu;

		controlPointList.Add(rightColumn.balustradePoint);
		controlPointList.Add(leftColumn.balustradePoint);

		movement.verlist.Add(rightColumn.balustradePoint);
		movement.verlist.Add(leftColumn.balustradePoint);

		rightColumn.allObjList.Add(rightColumn.balustradePoint);
		leftColumn.allObjList.Add(leftColumn.balustradePoint);

		MeshRenderer renderer = balustrade.AddComponent<MeshRenderer>() as MeshRenderer;

		isbalustrade = true;
	}
	Mesh AdjMeshFrieze()
	{
		friezeHeight=rightColumn.upPoint.transform.position.y - rightColumn.friezePoint.transform.position.y;
		Vector3 h = new Vector3(0.0f, friezeHeight, 0.0f);
		friezeFilter.mesh.Clear();
		Vector3 frieze_ru = rightColumn.upPoint.transform.position;
		Vector3 frieze_lu = leftColumn.upPoint.transform.position;
		Vector3 frieze_rd = rightColumn.friezePoint.transform.position = frieze_ru - h;
		Vector3 frieze_ld = leftColumn.friezePoint.transform.position = frieze_lu - h;
		friezeFilter.mesh = CreatRecMesh(frieze_lu, frieze_ru, frieze_rd, frieze_ld, friezeFilter.mesh);
		return friezeFilter.mesh;
	}

	Mesh AdjMeshBalustrade()
	{
		balustradeHeight=rightColumn.balustradePoint.transform.position.y - rightColumn.downPoint.transform.position.y;
		Vector3 h = new Vector3(0.0f, balustradeHeight, 0.0f);
		balustradeFilter.mesh.Clear();
		Vector3 balustrade_rd = rightColumn.downPoint.transform.position;
		Vector3 balustrade_ld = leftColumn.downPoint.transform.position;
		Vector3 balustrade_ru = rightColumn.balustradePoint.transform.position = balustrade_rd + h;
		Vector3 balustrade_lu = leftColumn.balustradePoint.transform.position = balustrade_ld + h;
		balustradeFilter.mesh = CreatRecMesh(balustrade_lu, balustrade_ru, balustrade_rd, balustrade_ld, balustradeFilter.mesh);

		return balustradeFilter.mesh;

	}
	void AdjMeshDoubleRoof()
	{
		doubleRoof.AdjPos(rightColumn, leftColumn, ini_doubleRoofHeight, ini_doubleRoofWidth);
	}
	public Mesh CreatRecMesh(Vector3 lu, Vector3 ru, Vector3 rd, Vector3 ld, Mesh ismesh)
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
	public Vector3 ClampPos(Vector3 inputPos) 
	{
		float minClampX = float.MinValue;
		float maxClampX = float.MaxValue;
		float minClampY = float.MinValue;
		float maxClampY = float.MaxValue;
		float minWidth = 0;
		float minHeight = ini_cylinderHeight * 0.7f;
		float minFriezeHeight = ini_friezeHeight*0.1f;
		float minBalustradeHeight = ini_balustradeHeight * 0.1f;
		if (dragitemcontroller.chooseObj == rightColumn.upPoint)
		{
			minClampY = rightColumn.downPoint.transform.position.y + minHeight;
		}
		else if (dragitemcontroller.chooseObj == rightColumn.downPoint)
		{
			maxClampY = rightColumn.upPoint.transform.position.y - minHeight;
		}
		else if (dragitemcontroller.chooseObj == rightColumn.friezePoint)
		{
			minClampY = rightColumn.downPoint.transform.position.y + minHeight;
			maxClampY = rightColumn.upPoint.transform.position.y - minFriezeHeight;
		}
		else if (dragitemcontroller.chooseObj == rightColumn.balustradePoint)
		{
			minClampY = rightColumn.downPoint.transform.position.y + minBalustradeHeight;
			maxClampY = rightColumn.upPoint.transform.position.y - minHeight;
		}
		else if (dragitemcontroller.chooseObj == leftColumn.upPoint)
		{
			minClampY = leftColumn.downPoint.transform.position.y + minHeight;
		}
		else if (dragitemcontroller.chooseObj == leftColumn.downPoint)
		{
			maxClampY = leftColumn.upPoint.transform.position.y - minHeight;
		}
		else if (dragitemcontroller.chooseObj == leftColumn.friezePoint)
		{
			minClampY = leftColumn.downPoint.transform.position.y + minFriezeHeight + minHeight;
			maxClampY = leftColumn.upPoint.transform.position.y - minFriezeHeight;
		}
		else if (dragitemcontroller.chooseObj == leftColumn.balustradePoint)
		{
			minClampY = leftColumn.downPoint.transform.position.y + minBalustradeHeight;
			maxClampY = leftColumn.upPoint.transform.position.y - minHeight ;
		}
		float posX = Mathf.Clamp(inputPos.x, minClampX, maxClampX);
		float posY = Mathf.Clamp(inputPos.y, minClampY, maxClampY);
		
		return new Vector3(posX, posY, inputPos.z);
	}
}