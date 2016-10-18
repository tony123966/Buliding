/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MeshCreate
{
	public GameObject body = null;
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
	public void CreateLineRenderer(Vector3 stratPos, Vector3 endPos)
	{
		//create a new empty gameobject and line renderer component
		GameObject lineObj = new GameObject("Line", typeof(LineRenderer));
		LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();
		lineRenderer.SetWidth(0.01f, 0.01f);
		lineRenderer.useWorldSpace = true;
		lineRenderer.material.color = Color.black;
		lineRenderer.SetColors(Color.black, Color.black);
		lineRenderer.SetVertexCount(2);
		lineRenderer.SetPosition(0, stratPos);
		lineRenderer.SetPosition(1, endPos);
	}
}
public class DoubleRoof : MeshCreate
{
	public Vector3 rightUpPoint;
	public Vector3 rightDownPoint;
	public Vector3 leftUpPoint;
	public Vector3 leftDownPoint;
	public MeshFilter mFilter;
	public DoubleRoof(string objName, Column rightColumn, Column leftColumn, float doubleRoofHeight, float doubleRoofWidth)
	{
		body = new GameObject(objName);
		mFilter = body.AddComponent<MeshFilter>();
		Debug.Log(rightColumn.body.name);
		rightDownPoint = rightColumn.upPoint.transform.position;
		leftDownPoint = leftColumn.upPoint.transform.position;

		float tmp = rightDownPoint.y + doubleRoofHeight;
		rightUpPoint = new Vector3(rightDownPoint.x, tmp, rightDownPoint.z);
		leftUpPoint = new Vector3(leftDownPoint.x, tmp, leftDownPoint.z);

		rightDownPoint.x = rightDownPoint.x + doubleRoofWidth;
		leftDownPoint.x = leftDownPoint.x - doubleRoofWidth;

		MeshRenderer renderer = body.AddComponent<MeshRenderer>() as MeshRenderer;
		mFilter.mesh = CreatRecMesh(leftUpPoint, rightUpPoint, rightDownPoint, leftDownPoint, null);
		SetLine();
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
	void SetLine()
	{
		CreateLineRenderer(rightUpPoint, leftUpPoint);
		CreateLineRenderer(rightUpPoint, rightDownPoint);
		CreateLineRenderer(leftUpPoint, leftDownPoint);
		CreateLineRenderer(rightDownPoint, leftDownPoint);
	}
}
public class Column : MeshCreate
{
	public GameObject upPoint = null;
	public GameObject downPoint = null;
	public GameObject balustradePoint = null;
	public GameObject friezePoint = null;
	public List<GameObject> allObjList = new List<GameObject>();//所有控制點
	public float radius = 0.01f;
	public Column(GameObject upPoint, GameObject downPoint, float columnHeight)
	{
		this.upPoint = upPoint;
		this.downPoint = downPoint;

		this.body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		this.body.transform.localScale = new Vector3(radius, columnHeight / 2.0f, radius);
		this.body.transform.position = new Vector3(upPoint.transform.position.x, upPoint.transform.position.y - columnHeight / 2.0f, upPoint.transform.position.z);

		this.body.tag = "Cylinder";

		this.allObjList.Add(body);
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

	private DoubleRoof doubleRoof;

	private MeshFilter balustradeFilter;
	private MeshFilter friezeFilter;


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

		cylinderHeight = ini_cylinderHeight = Vector3.Distance(controlPointList[0].transform.position, controlPointList[1].transform.position);

		leftColumn = new Column(controlPointList[0], controlPointList[1], ini_cylinderHeight);
		rightColumn = new Column(controlPointList[2], controlPointList[3], ini_cylinderHeight);

		leftColumn.body.transform.parent = this.gameObject.transform;
		rightColumn.body.transform.parent = this.gameObject.transform;

		movement.horlist.Add(leftColumn.body);
		movement.horlist.Add(rightColumn.body);


		friezeHeight = ini_friezeHeight = 0.2f * ini_cylinderHeight;
		balustradeHeight = ini_balustradeHeight = 0.2f * ini_cylinderHeight;
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
			cylinderHeight = (dragitemcontroller.chooseObj.transform.position.y - rightColumn.downPoint.transform.position.y);
			cylinderHeight = Mathf.Abs(cylinderHeight);
			//update point
			rightColumn.upPoint.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, tmp.y, rightColumn.upPoint.transform.position.z);
			leftColumn.upPoint.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, tmp.y, leftColumn.upPoint.transform.position.z);


			rightColumn.body.transform.localScale = new Vector3(rightColumn.radius, cylinderHeight / 2.0f, rightColumn.radius);
			rightColumn.body.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, rightColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, rightColumn.upPoint.transform.position.z);
			leftColumn.body.transform.localScale = new Vector3(leftColumn.radius, cylinderHeight / 2.0f, leftColumn.radius);
			leftColumn.body.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, leftColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, leftColumn.upPoint.transform.position.z);

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
			chang_cylinderHeight = dis;
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

			rightColumn.body.transform.localScale = new Vector3(rightColumn.radius, cylinderHeight / 2.0f, rightColumn.radius);
			rightColumn.body.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, rightColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, rightColumn.upPoint.transform.position.z);
			leftColumn.body.transform.localScale = new Vector3(leftColumn.radius, cylinderHeight / 2.0f, leftColumn.radius);
			leftColumn.body.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, leftColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, leftColumn.upPoint.transform.position.z);

			if (isbalustrade)
			{
				rightColumn.balustradePoint.transform.position = new Vector3(rightColumn.downPoint.transform.position.x, tmp.y + balustradeHeight, rightColumn.upPoint.transform.position.z);
				leftColumn.balustradePoint.transform.position = new Vector3(leftColumn.downPoint.transform.position.x, tmp.y + balustradeHeight, leftColumn.upPoint.transform.position.z);
				AdjMeshBalustrade();
			}
			chang_cylinderHeight = dis;
			ratio_cylinderHeight = chang_cylinderHeight / ini_cylinderHeight;
		}
		else if (dragitemcontroller.chooseObj == rightColumn.body)
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
		else if (dragitemcontroller.chooseObj == leftColumn.body)
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
		movement.horlist.Add(rightColumn.body);
		movement.horlist.Add(leftColumn.body);
	}


	DoubleRoof CreateDoubleRoof()
	{
		DoubleRoof doubleRoof = new DoubleRoof("DoubleRoof_mesh", rightColumn, leftColumn, ini_doubleRoofHeight, ini_doubleRoofWidth);

		doubleRoof.body.transform.parent = this.gameObject.transform;


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
		friezeHeight = rightColumn.upPoint.transform.position.y - rightColumn.friezePoint.transform.position.y;
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
		balustradeHeight = rightColumn.balustradePoint.transform.position.y - rightColumn.downPoint.transform.position.y;
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

	public Vector3 ClampPos(Vector3 inputPos)
	{
		float minClampX = float.MinValue;
		float maxClampX = float.MaxValue;
		float minClampY = float.MinValue;
		float maxClampY = float.MaxValue;
		float minWidth = 0;
		float minHeight = ini_cylinderHeight * 0.7f;
		float minFriezeHeight = ini_cylinderHeight * 0.1f;
		float minBalustradeHeight = ini_cylinderHeight * 0.1f;
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
			maxClampY = leftColumn.upPoint.transform.position.y - minHeight;
		}
		float posX = Mathf.Clamp(inputPos.x, minClampX, maxClampX);
		float posY = Mathf.Clamp(inputPos.y, minClampY, maxClampY);

		return new Vector3(posX, posY, inputPos.z);
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
}*/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshCreate:lineRendererControl
{
	public List<Vector3> controlPointList = new List<Vector3>();
	public GameObject body = null;
	public MeshFilter mFilter;
	public MeshRenderer mRenderer;
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
			if (i != controlPointList.Count - 1)
				AdjLineRenderer(i, controlPointList[i], controlPointList[i + 1]);
			else
				AdjLineRenderer(i, controlPointList[i], controlPointList[0]);
		}
	}
}
public class DoubleRoofIcon : MeshCreate
{
	public Vector3 rightUpPoint;
	public Vector3 rightDownPoint;
	public Vector3 leftUpPoint;
	public Vector3 leftDownPoint;
	public void DoubleRoofIconCreate<T>(T thisGameObject, string objName, ColumnIcon rightColumn, ColumnIcon leftColumn, float doubleRoofHeight, float doubleRoofWidth)
	where T : Component
	{
		body = new GameObject(objName);
		mFilter = body.AddComponent<MeshFilter>();

		rightDownPoint = rightColumn.upPoint.transform.position;
		leftDownPoint = leftColumn.upPoint.transform.position;

		float tmp = rightDownPoint.y + doubleRoofHeight;
		rightUpPoint = new Vector3(rightDownPoint.x, tmp, rightDownPoint.z);
		leftUpPoint = new Vector3(leftDownPoint.x, tmp, leftDownPoint.z);

		rightDownPoint.x = rightDownPoint.x + doubleRoofWidth;
		leftDownPoint.x = leftDownPoint.x - doubleRoofWidth;

		mRenderer = body.AddComponent<MeshRenderer>() as MeshRenderer;
		mFilter.mesh = CreatRecMesh(leftUpPoint, rightUpPoint, rightDownPoint, leftDownPoint, null);

		body.transform.parent = thisGameObject.transform;

		InitLineRender(thisGameObject);
		SetIconObjectColor();
	}
	public void AdjPos(ColumnIcon rightColumn, ColumnIcon leftColumn, float doubleRoofHeight, float doubleRoofWidth)
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

		UpdateLineRender();
	}
	public override void InitLineRender<T>(T thisGameObject)
	{
		controlPointList.Add(leftUpPoint);
		controlPointList.Add(rightUpPoint);
		controlPointList.Add(rightDownPoint);
		controlPointList.Add(leftDownPoint);
		base.InitLineRender(thisGameObject);
	}
	public override void UpdateLineRender()
	{
		controlPointList[0] = (leftUpPoint);
		controlPointList[1] = (rightUpPoint);
		controlPointList[2] = (rightDownPoint);
		controlPointList[3] = (leftDownPoint);
		base.UpdateLineRender();
	}
	public void SetIconObjectColor()
	{
		mRenderer.material.color = Color.red;
	}
}
public class FriezeIcon : MeshCreate
{
	public Vector3 rightUpPoint;
	public Vector3 rightDownPoint;
	public Vector3 leftUpPoint;
	public Vector3 leftDownPoint;
	public void FriezeIconCreate<T>(T thisGameObject, string objName, float ini_friezeHeight, ColumnIcon rightColumn, ColumnIcon leftColumn) where T : Component
	{
		Vector3 h = new Vector3(0.0f, ini_friezeHeight, 0.0f);
		rightUpPoint = rightColumn.upPoint.transform.position;
		leftUpPoint = leftColumn.upPoint.transform.position;
		rightDownPoint = rightUpPoint - h;
		leftDownPoint = leftUpPoint - h;

		body = new GameObject(objName);

		mFilter = body.AddComponent<MeshFilter>();
		mFilter.mesh = CreatRecMesh(leftUpPoint, rightUpPoint, rightDownPoint, leftDownPoint, null);

		//frieze cp
		rightColumn.friezePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		rightColumn.friezePoint.tag = "ControlPoint";
		rightColumn.friezePoint.name = "FRD";
		rightColumn.friezePoint.transform.localScale = rightColumn.downPoint.transform.localScale;
		rightColumn.friezePoint.transform.position = rightDownPoint;

		leftColumn.friezePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		leftColumn.friezePoint.tag = "ControlPoint";
		leftColumn.friezePoint.name = "FLD";
		leftColumn.friezePoint.transform.localScale = rightColumn.downPoint.transform.localScale;
		leftColumn.friezePoint.transform.position = leftDownPoint;

		rightColumn.allObjList.Add(rightColumn.friezePoint);
		leftColumn.allObjList.Add(leftColumn.friezePoint);

		mRenderer = body.AddComponent<MeshRenderer>() as MeshRenderer;

		body.transform.parent = thisGameObject.transform;
		rightColumn.friezePoint.transform.parent = thisGameObject.transform;
		leftColumn.friezePoint.transform.parent = thisGameObject.transform;
		
		InitLineRender(thisGameObject);
		SetIconObjectColor( rightColumn,  leftColumn);
	}
	public float AdjPos(ColumnIcon rightColumn, ColumnIcon leftColumn)
	{
		float friezeHeight = rightColumn.upPoint.transform.position.y - rightColumn.friezePoint.transform.position.y;
		Vector3 h = new Vector3(0.0f, friezeHeight, 0.0f);
		mFilter.mesh.Clear();
		rightUpPoint = rightColumn.upPoint.transform.position;
		leftUpPoint = leftColumn.upPoint.transform.position;
		rightDownPoint = rightColumn.friezePoint.transform.position = rightUpPoint - h;
		leftDownPoint = leftColumn.friezePoint.transform.position = leftUpPoint - h;
		mFilter.mesh = CreatRecMesh(leftUpPoint, rightUpPoint, rightDownPoint, leftDownPoint, mFilter.mesh);

		UpdateLineRender();

		return friezeHeight;
	}
	public override void InitLineRender<T>(T thisGameObject)
	{
		controlPointList.Add(leftUpPoint);
		controlPointList.Add(rightUpPoint);
		controlPointList.Add(rightDownPoint);
		controlPointList.Add(leftDownPoint);
		base.InitLineRender(thisGameObject);
	}
	public override void UpdateLineRender()
	{
		controlPointList[0] = (leftUpPoint);
		controlPointList[1] = (rightUpPoint);
		controlPointList[2] = (rightDownPoint);
		controlPointList[3] = (leftDownPoint);
		base.UpdateLineRender();
	}
	public void SetIconObjectColor(ColumnIcon rightColumn, ColumnIcon leftColumn)
	{
		mRenderer.material.color = Color.red;
		rightColumn.friezePoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
		leftColumn.friezePoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
	}
}
public class BalustradeIcon : MeshCreate
{
	public Vector3 rightUpPoint;
	public Vector3 rightDownPoint;
	public Vector3 leftUpPoint;
	public Vector3 leftDownPoint;
	public void BalustradeIconCreate<T>(T thisGameObject, string objName, float ini_balustradeHeight, ColumnIcon rightColumn, ColumnIcon leftColumn) where T : Component
	{
		Vector3 h = new Vector3(0.0f, ini_balustradeHeight, 0.0f);
		rightDownPoint = rightColumn.downPoint.transform.position;
		leftDownPoint = leftColumn.downPoint.transform.position;
		rightUpPoint = rightDownPoint + h;
		leftUpPoint = leftDownPoint + h;

		body = new GameObject(objName);

		mFilter = body.AddComponent<MeshFilter>();
		mFilter.mesh = CreatRecMesh(leftUpPoint, rightUpPoint, rightDownPoint, leftDownPoint, null);

		//frieze cp
		rightColumn.balustradePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		rightColumn.balustradePoint.tag = "ControlPoint";
		rightColumn.balustradePoint.name = "BRU";
		rightColumn.balustradePoint.transform.localScale = rightColumn.downPoint.transform.localScale;
		rightColumn.balustradePoint.transform.position = rightUpPoint;

		leftColumn.balustradePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		leftColumn.balustradePoint.tag = "ControlPoint";
		leftColumn.balustradePoint.name = "BLU";
		leftColumn.balustradePoint.transform.localScale = rightColumn.downPoint.transform.localScale;
		leftColumn.balustradePoint.transform.position = leftUpPoint;

		rightColumn.allObjList.Add(rightColumn.balustradePoint);
		leftColumn.allObjList.Add(leftColumn.balustradePoint);

		mRenderer = body.AddComponent<MeshRenderer>() as MeshRenderer;

		body.transform.parent = thisGameObject.transform;
		rightColumn.balustradePoint.transform.parent = thisGameObject.transform;
		leftColumn.balustradePoint.transform.parent = thisGameObject.transform;

		InitLineRender(thisGameObject);
		SetIconObjectColor(rightColumn,leftColumn);
	}
	public float AdjPos(ColumnIcon rightColumn, ColumnIcon leftColumn)
	{
		float balustradeHeight = rightColumn.balustradePoint.transform.position.y - rightColumn.downPoint.transform.position.y;
		Vector3 h = new Vector3(0.0f, balustradeHeight, 0.0f);
		mFilter.mesh.Clear();
		rightDownPoint = rightColumn.downPoint.transform.position;
		leftDownPoint = leftColumn.downPoint.transform.position;
		rightUpPoint = rightColumn.balustradePoint.transform.position = rightDownPoint + h;
		leftUpPoint = leftColumn.balustradePoint.transform.position = leftDownPoint + h;
		mFilter.mesh = CreatRecMesh(leftUpPoint, rightUpPoint, rightDownPoint, leftDownPoint, mFilter.mesh);

		UpdateLineRender();
		return balustradeHeight;
	}
	public override void InitLineRender<T>(T thisGameObject)
	{
		controlPointList.Add(leftUpPoint);
		controlPointList.Add(rightUpPoint);
		controlPointList.Add(rightDownPoint);
		controlPointList.Add(leftDownPoint);
		base.InitLineRender(thisGameObject);
	}
	public override void UpdateLineRender()
	{
		controlPointList[0] = (leftUpPoint);
		controlPointList[1] = (rightUpPoint);
		controlPointList[2] = (rightDownPoint);
		controlPointList[3] = (leftDownPoint);
		base.UpdateLineRender();
	}
	public void SetIconObjectColor(ColumnIcon rightColumn, ColumnIcon leftColumn)
	{
		mRenderer.material.color = Color.red;
		rightColumn.balustradePoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
		leftColumn.balustradePoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
	}
}
public class ColumnIcon : MeshCreate
{
	public GameObject upPoint = null;
	public GameObject downPoint = null;
	public GameObject balustradePoint = null;
	public GameObject friezePoint = null;
	public List<GameObject> allObjList = new List<GameObject>();//所有控制點
	public float radius = 0.01f;
	public ColumnIcon(GameObject upPoint, GameObject downPoint, float columnHeight)
	{
		this.upPoint = upPoint;
		this.downPoint = downPoint;

		this.body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		mFilter=body.GetComponent<MeshFilter>();
		mRenderer = body.GetComponent<MeshRenderer>();
		this.body.transform.localScale = new Vector3(radius, columnHeight / 2.0f, radius);
		this.body.transform.position = new Vector3(upPoint.transform.position.x, upPoint.transform.position.y - columnHeight / 2.0f, upPoint.transform.position.z);

		this.body.tag = "Cylinder";

		this.allObjList.Add(body);
		this.allObjList.Add(downPoint);
		this.allObjList.Add(upPoint);

		SetIconObjectColor();
	}
	public void SetIconObjectColor()
	{
		mRenderer.material.color = Color.red;
		upPoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
		downPoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
	}
}

public class body2icon : MonoBehaviour
{
	public List<GameObject> controlPointList = new List<GameObject>();//所有控制點
	public ColumnIcon leftColumn;
	public ColumnIcon rightColumn;
	//just try two point

	private DragItemController dragitemcontroller;
	private Movement movement;

	private DoubleRoofIcon doubleRoofIcon;

	private FriezeIcon friezeIcon;
	private BalustradeIcon balustradeIcon;


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

	public bool isBalustrade;
	public bool isFrieze;
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

		cylinderHeight = ini_cylinderHeight = Vector3.Distance(controlPointList[0].transform.position, controlPointList[1].transform.position);

		leftColumn = new ColumnIcon(controlPointList[0], controlPointList[1], ini_cylinderHeight);
		rightColumn = new ColumnIcon(controlPointList[2], controlPointList[3], ini_cylinderHeight);

		leftColumn.body.transform.parent = this.gameObject.transform;
		rightColumn.body.transform.parent = this.gameObject.transform;

		movement.horlist.Add(leftColumn.body);
		movement.horlist.Add(rightColumn.body);


		friezeHeight = ini_friezeHeight = 0.2f * ini_cylinderHeight;
		balustradeHeight = ini_balustradeHeight = 0.2f * ini_cylinderHeight;
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
			cylinderHeight = (dragitemcontroller.chooseObj.transform.position.y - rightColumn.downPoint.transform.position.y);
			cylinderHeight = Mathf.Abs(cylinderHeight);
			//update point
			rightColumn.upPoint.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, tmp.y, rightColumn.upPoint.transform.position.z);
			leftColumn.upPoint.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, tmp.y, leftColumn.upPoint.transform.position.z);


			rightColumn.body.transform.localScale = new Vector3(rightColumn.radius, cylinderHeight / 2.0f, rightColumn.radius);
			rightColumn.body.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, rightColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, rightColumn.upPoint.transform.position.z);
			leftColumn.body.transform.localScale = new Vector3(leftColumn.radius, cylinderHeight / 2.0f, leftColumn.radius);
			leftColumn.body.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, leftColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, leftColumn.upPoint.transform.position.z);

			if (isFrieze)
			{
				rightColumn.friezePoint.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, tmp.y - friezeHeight, rightColumn.upPoint.transform.position.z);
				leftColumn.friezePoint.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, tmp.y - friezeHeight, leftColumn.upPoint.transform.position.z);
				friezeHeight = friezeIcon.AdjPos(rightColumn, leftColumn);
			}
			if (isDoubleRoof)
			{
				doubleRoofIcon.AdjPos(rightColumn, leftColumn, ini_doubleRoofHeight, ini_doubleRoofWidth);
			}
			chang_cylinderHeight = dis;
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

			rightColumn.body.transform.localScale = new Vector3(rightColumn.radius, cylinderHeight / 2.0f, rightColumn.radius);
			rightColumn.body.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, rightColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, rightColumn.upPoint.transform.position.z);
			leftColumn.body.transform.localScale = new Vector3(leftColumn.radius, cylinderHeight / 2.0f, leftColumn.radius);
			leftColumn.body.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, leftColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, leftColumn.upPoint.transform.position.z);

			if (isBalustrade)
			{
				rightColumn.balustradePoint.transform.position = new Vector3(rightColumn.downPoint.transform.position.x, tmp.y + balustradeHeight, rightColumn.upPoint.transform.position.z);
				leftColumn.balustradePoint.transform.position = new Vector3(leftColumn.downPoint.transform.position.x, tmp.y + balustradeHeight, leftColumn.upPoint.transform.position.z);

				balustradeHeight = balustradeIcon.AdjPos(rightColumn, leftColumn);
			}
			chang_cylinderHeight = dis;
			ratio_cylinderHeight = chang_cylinderHeight / ini_cylinderHeight;
		}
		else if (dragitemcontroller.chooseObj == rightColumn.body)
		{
			float dis = (tmp.x - rightColumn.upPoint.transform.position.x);
			for (int i = 0; i < rightColumn.allObjList.Count; i++)
			{
				rightColumn.allObjList[i].transform.position = new Vector3(tmp.x, rightColumn.allObjList[i].transform.position.y, rightColumn.allObjList[i].transform.position.z);
				leftColumn.allObjList[i].transform.position = new Vector3(leftColumn.allObjList[i].transform.position.x - (dis), leftColumn.allObjList[i].transform.position.y, leftColumn.allObjList[i].transform.position.z);
			}
			if (isFrieze)
			{
				friezeHeight = friezeIcon.AdjPos(rightColumn, leftColumn);
			}
			if (isBalustrade)
			{
				balustradeHeight = balustradeIcon.AdjPos(rightColumn, leftColumn);
			}
			if (isDoubleRoof)
			{
				doubleRoofIcon.AdjPos(rightColumn, leftColumn, ini_doubleRoofHeight, ini_doubleRoofWidth);
			}
			chang_bodydis = dis;
			ratio_bodydis = chang_bodydis / ini_bodydis;
		}
		else if (dragitemcontroller.chooseObj == leftColumn.body)
		{
			float dis = (tmp.x - leftColumn.upPoint.transform.position.x);

			for (int i = 0; i < leftColumn.allObjList.Count; i++)
			{
				leftColumn.allObjList[i].transform.position = new Vector3(tmp.x, leftColumn.allObjList[i].transform.position.y, leftColumn.allObjList[i].transform.position.z);
				rightColumn.allObjList[i].transform.position = new Vector3(rightColumn.allObjList[i].transform.position.x - (dis), rightColumn.allObjList[i].transform.position.y, rightColumn.allObjList[i].transform.position.z);

			}
			if (isFrieze)
			{
				friezeHeight = friezeIcon.AdjPos(rightColumn, leftColumn); ;
			}
			if (isBalustrade)
			{
				balustradeHeight = balustradeIcon.AdjPos(rightColumn, leftColumn);
			}
			if (isDoubleRoof)
			{
				doubleRoofIcon.AdjPos(rightColumn, leftColumn, ini_doubleRoofHeight, ini_doubleRoofWidth);
			}
			chang_bodydis = dis;
			ratio_bodydis = chang_bodydis / ini_bodydis;
		}
		else if (dragitemcontroller.chooseObj == rightColumn.friezePoint || dragitemcontroller.chooseObj == leftColumn.friezePoint)
		{

			leftColumn.friezePoint.transform.position = new Vector3(leftColumn.friezePoint.transform.position.x, tmp.y, leftColumn.friezePoint.transform.position.z);
			rightColumn.friezePoint.transform.position = new Vector3(rightColumn.friezePoint.transform.position.x, tmp.y, rightColumn.friezePoint.transform.position.z);

			friezeHeight = friezeIcon.AdjPos(rightColumn, leftColumn); ;


		}
		else if (dragitemcontroller.chooseObj == rightColumn.balustradePoint || dragitemcontroller.chooseObj == leftColumn.balustradePoint)
		{ //balustrade

			leftColumn.balustradePoint.transform.position = new Vector3(leftColumn.balustradePoint.transform.position.x, tmp.y, leftColumn.balustradePoint.transform.position.z);
			rightColumn.balustradePoint.transform.position = new Vector3(rightColumn.balustradePoint.transform.position.x, tmp.y, rightColumn.balustradePoint.transform.position.z);

			balustradeHeight = balustradeIcon.AdjPos(rightColumn, leftColumn);

		}
	}
	public void UpdateFunction(string objName)
	{
		switch (objName)
		{
			case "Frieze":
				if (!isFrieze)
					friezeIcon = Createfrieze(); // mesh製造機
				break;
			case "Balustrade":
				if (!isBalustrade)
					balustradeIcon = Createbalustrade(); // mesh製造機
				break;
			case "DoubleRoof":
				if (!isDoubleRoof)
					doubleRoofIcon = CreateDoubleRoof();
				break;


		}
	}
	public void addpoint()
	{
		movement.verlist.AddRange(controlPointList);
		movement.horlist.Add(rightColumn.body);
		movement.horlist.Add(leftColumn.body);
	}


	DoubleRoofIcon CreateDoubleRoof()
	{
		isDoubleRoof = true;

		DoubleRoofIcon doubleRoof = new DoubleRoofIcon();
		doubleRoof.DoubleRoofIconCreate(this, "DoubleRoof_mesh", rightColumn, leftColumn, ini_doubleRoofHeight, ini_doubleRoofWidth);


		return doubleRoof;
	}

	FriezeIcon Createfrieze()
	{
		isFrieze = true;

		FriezeIcon frieze = new FriezeIcon();
		frieze.FriezeIconCreate(this, "frieze_mesh", ini_friezeHeight, rightColumn, leftColumn);

		controlPointList.Add(rightColumn.friezePoint);
		controlPointList.Add(leftColumn.friezePoint);

		movement.verlist.Add(rightColumn.friezePoint);
		movement.verlist.Add(leftColumn.friezePoint);

		return frieze;
	}
	BalustradeIcon Createbalustrade()
	{
		isBalustrade = true;

		BalustradeIcon balustrade = new BalustradeIcon();
		balustrade.BalustradeIconCreate(this, "blustrade_mesh", ini_balustradeHeight, rightColumn, leftColumn);

		controlPointList.Add(rightColumn.balustradePoint);
		controlPointList.Add(leftColumn.balustradePoint);

		movement.verlist.Add(rightColumn.balustradePoint);
		movement.verlist.Add(leftColumn.balustradePoint);

		return balustrade;

	}
	public Vector3 ClampPos(Vector3 inputPos)
	{
		float minClampX = float.MinValue;
		float maxClampX = float.MaxValue;
		float minClampY = float.MinValue;
		float maxClampY = float.MaxValue;
		float minWidth = 0;
		float minHeight = ini_cylinderHeight * 0.7f;
		float minFriezeHeight = ini_cylinderHeight * 0.1f;
		float minBalustradeHeight = ini_cylinderHeight * 0.1f;
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
			maxClampY = leftColumn.upPoint.transform.position.y - minHeight;
		}
		float posX = Mathf.Clamp(inputPos.x, minClampX, maxClampX);
		float posY = Mathf.Clamp(inputPos.y, minClampY, maxClampY);

		return new Vector3(posX, posY, inputPos.z);
	}
}