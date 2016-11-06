/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshCreate : lineRendererControl
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
		SetIconObjectColor(rightColumn, leftColumn);
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
public class WallIcon : MeshCreate
{
	public GameObject rightUpPoint;
	public GameObject rightDownPoint;
	public GameObject leftUpPoint;
	public GameObject leftDownPoint;
	Vector3 offset;
	public void WallIconCreate<T>(T thisGameObject, string objName, ColumnIcon rightColumn, ColumnIcon leftColumn) where T : Component
	{
		offset = new Vector3(Mathf.Abs(rightColumn.upPoint.transform.transform.position.x - leftColumn.upPoint.transform.transform.position.x) * 0.2f, 0, 0);
		//WallIcon cp
		rightUpPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		rightUpPoint.tag = "ControlPoint";
		rightUpPoint.name = "WRU";
		rightUpPoint.transform.localScale = rightColumn.upPoint.transform.localScale;
		rightUpPoint.transform.position = rightColumn.upPoint.transform.transform.position - offset;

		rightDownPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		rightDownPoint.tag = "ControlPoint";
		rightDownPoint.name = "WRD";
		rightDownPoint.transform.localScale = rightColumn.downPoint.transform.localScale;
		rightDownPoint.transform.position = rightColumn.downPoint.transform.transform.position - offset;

		leftUpPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		leftUpPoint.tag = "ControlPoint";
		leftUpPoint.name = "WLU";
		leftUpPoint.transform.localScale = leftColumn.upPoint.transform.localScale;
		leftUpPoint.transform.position = leftColumn.upPoint.transform.transform.position + offset;

		leftDownPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		leftDownPoint.tag = "ControlPoint";
		leftDownPoint.name = "WLD";
		leftDownPoint.transform.localScale = leftColumn.downPoint.transform.localScale;
		leftDownPoint.transform.position = leftColumn.downPoint.transform.transform.position + offset;

		body = new GameObject(objName);

		mFilter = body.AddComponent<MeshFilter>();
		mFilter.mesh = CreatRecMesh(leftUpPoint.transform.position, rightUpPoint.transform.position, rightDownPoint.transform.position, leftDownPoint.transform.position, null);

		mRenderer = body.AddComponent<MeshRenderer>() as MeshRenderer;

		body.transform.parent = thisGameObject.transform;

		leftUpPoint.transform.parent = thisGameObject.transform;
		leftDownPoint.transform.parent = thisGameObject.transform;
		rightUpPoint.transform.parent = thisGameObject.transform;
		rightDownPoint.transform.parent = thisGameObject.transform;

		InitLineRender(thisGameObject);
		SetIconObjectColor(rightColumn, leftColumn);
	}
	public void AdjPos(ColumnIcon rightColumn, ColumnIcon leftColumn)
	{
		mFilter.mesh.Clear();
		mFilter.mesh = CreatRecMesh(leftUpPoint.transform.position, rightUpPoint.transform.position, rightDownPoint.transform.position, leftDownPoint.transform.position, mFilter.mesh);

		UpdateLineRender();
	}
	public override void InitLineRender<T>(T thisGameObject)
	{
		controlPointList.Add(leftUpPoint.transform.position);
		controlPointList.Add(rightUpPoint.transform.position);
		controlPointList.Add(rightDownPoint.transform.position);
		controlPointList.Add(leftDownPoint.transform.position);
		base.InitLineRender(thisGameObject);
	}
	public override void UpdateLineRender()
	{
		controlPointList[0] = (leftUpPoint.transform.position);
		controlPointList[1] = (rightUpPoint.transform.position);
		controlPointList[2] = (rightDownPoint.transform.position);
		controlPointList[3] = (leftDownPoint.transform.position);
		base.UpdateLineRender();
	}
	public void SetIconObjectColor(ColumnIcon rightColumn, ColumnIcon leftColumn)
	{
		mRenderer.material.color = Color.red;
		leftUpPoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
		rightUpPoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
		rightDownPoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
		leftDownPoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
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
		SetIconObjectColor(rightColumn, leftColumn);
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
		mFilter = body.GetComponent<MeshFilter>();
		mRenderer = body.GetComponent<MeshRenderer>();
		mRenderer.sortingOrder = 0;
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

	private WallIcon wallIcon;

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

	public bool isWall;

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
		Vector3 tmp = dragitemcontroller.chooseObj.transform.position;
		if (dragitemcontroller.chooseObj == rightColumn.upPoint || dragitemcontroller.chooseObj == leftColumn.upPoint)//RU LU
		{
			float dis = (tmp.y - rightColumn.upPoint.transform.position.y);
			cylinderHeight = (dragitemcontroller.chooseObj.transform.position.y - rightColumn.downPoint.transform.position.y);
			cylinderHeight = Mathf.Abs(cylinderHeight);
			//update point
			rightColumn.upPoint.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, tmp.y, rightColumn.upPoint.transform.position.z);
			leftColumn.upPoint.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, tmp.y, leftColumn.upPoint.transform.position.z);


			//rightColumn.body.transform.localScale = new Vector3(rightColumn.radius, cylinderHeight / 2.0f, rightColumn.radius);
			rightColumn.body.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, rightColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, rightColumn.upPoint.transform.position.z);
			//leftColumn.body.transform.localScale = new Vector3(leftColumn.radius, cylinderHeight / 2.0f, leftColumn.radius);
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

			//rightColumn.body.transform.localScale = new Vector3(rightColumn.radius, cylinderHeight / 2.0f, rightColumn.radius);
			rightColumn.body.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, rightColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, rightColumn.upPoint.transform.position.z);
			//leftColumn.body.transform.localScale = new Vector3(leftColumn.radius, cylinderHeight / 2.0f, leftColumn.radius);
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
		{//frieze

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
		else if (dragitemcontroller.chooseObj == wallIcon.rightUpPoint)
		{
			float dis = (tmp.x - wallIcon.rightUpPoint.transform.position.x);

			wallIcon.leftUpPoint.transform.position = new Vector3(wallIcon.leftUpPoint.transform.position.x - (dis), wallIcon.leftUpPoint.transform.position.y, wallIcon.leftUpPoint.transform.position.z);
			wallIcon.leftDownPoint.transform.position = new Vector3(wallIcon.leftDownPoint.transform.position.x - (dis), wallIcon.leftDownPoint.transform.position.y, wallIcon.leftDownPoint.transform.position.z);

			wallIcon.rightDownPoint.transform.position = new Vector3(tmp.x, wallIcon.rightDownPoint.transform.position.y, wallIcon.rightDownPoint.transform.position.z);

			wallIcon.AdjPos(rightColumn, leftColumn);
		}
		else if (dragitemcontroller.chooseObj == wallIcon.leftUpPoint)
		{
			float dis = (tmp.x - wallIcon.leftUpPoint.transform.position.x);

			wallIcon.rightUpPoint.transform.position = new Vector3(wallIcon.rightUpPoint.transform.position.x + (dis), wallIcon.rightUpPoint.transform.position.y, wallIcon.rightUpPoint.transform.position.z);
			wallIcon.rightDownPoint.transform.position = new Vector3(wallIcon.rightDownPoint.transform.position.x + (dis), wallIcon.rightDownPoint.transform.position.y, wallIcon.rightDownPoint.transform.position.z);

			wallIcon.leftDownPoint.transform.position = new Vector3(tmp.x, wallIcon.leftDownPoint.transform.position.y, wallIcon.leftDownPoint.transform.position.z);

			wallIcon.AdjPos(rightColumn, leftColumn);
		}
		else if (dragitemcontroller.chooseObj == wallIcon.rightDownPoint)
		{
			float dis = (tmp.x - wallIcon.rightDownPoint.transform.position.x);

			wallIcon.leftUpPoint.transform.position = new Vector3(wallIcon.leftUpPoint.transform.position.x - (dis), wallIcon.leftUpPoint.transform.position.y, wallIcon.leftUpPoint.transform.position.z);
			wallIcon.leftDownPoint.transform.position = new Vector3(wallIcon.leftDownPoint.transform.position.x - (dis), wallIcon.leftDownPoint.transform.position.y, wallIcon.leftDownPoint.transform.position.z);

			wallIcon.rightUpPoint.transform.position = new Vector3(tmp.x, wallIcon.rightUpPoint.transform.position.y, wallIcon.rightUpPoint.transform.position.z);

			wallIcon.AdjPos(rightColumn, leftColumn);
		}
		else if (dragitemcontroller.chooseObj == wallIcon.leftDownPoint)
		{
			float dis = (tmp.x - wallIcon.leftDownPoint.transform.position.x);
			wallIcon.rightUpPoint.transform.position = new Vector3(wallIcon.rightUpPoint.transform.position.x + (dis), wallIcon.rightUpPoint.transform.position.y, wallIcon.rightUpPoint.transform.position.z);
			wallIcon.rightDownPoint.transform.position = new Vector3(wallIcon.rightDownPoint.transform.position.x + (dis), wallIcon.rightDownPoint.transform.position.y, wallIcon.rightDownPoint.transform.position.z);

			wallIcon.leftUpPoint.transform.position = new Vector3(tmp.x, wallIcon.leftUpPoint.transform.position.y, wallIcon.leftUpPoint.transform.position.z);

			wallIcon.AdjPos(rightColumn, leftColumn);
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
			case "Wall":
				if (!isWall)
					wallIcon = CreateWall();
				break;
		}
	}
	public void addpoint()
	{
		movement.verlist.AddRange(controlPointList);
		movement.horlist.Add(rightColumn.body);
		movement.horlist.Add(leftColumn.body);
		if (isWall)
		{
			movement.horlist.Add(wallIcon.rightDownPoint);
			movement.horlist.Add(wallIcon.rightUpPoint);
			movement.horlist.Add(wallIcon.leftDownPoint);
			movement.horlist.Add(wallIcon.leftUpPoint);
		}
	}
	WallIcon CreateWall()
	{
		isWall = true;

		WallIcon wall = new WallIcon();
		wall.WallIconCreate(this, "Wall_mesh", rightColumn, leftColumn);

		movement.horlist.Add(wall.rightDownPoint);
		movement.horlist.Add(wall.rightUpPoint);
		movement.horlist.Add(wall.leftDownPoint);
		movement.horlist.Add(wall.leftUpPoint);

		return wall;
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
		float minWidth = ini_bodydis * 0.2f;
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
		else if (dragitemcontroller.chooseObj == wallIcon.rightUpPoint)
		{
			maxClampX = rightColumn.upPoint.transform.position.x - minWidth;
		}
		else if (dragitemcontroller.chooseObj == wallIcon.rightDownPoint)
		{
			maxClampX = rightColumn.downPoint.transform.position.x - minWidth;
		}
		else if (dragitemcontroller.chooseObj == wallIcon.leftUpPoint)
		{
			minClampX = leftColumn.upPoint.transform.position.x + minWidth;
		}
		else if (dragitemcontroller.chooseObj == wallIcon.leftDownPoint)
		{
			minClampX = leftColumn.downPoint.transform.position.x + minWidth;
		}
		float posX = Mathf.Clamp(inputPos.x, minClampX, maxClampX);
		float posY = Mathf.Clamp(inputPos.y, minClampY, maxClampY);

		return new Vector3(posX, posY, inputPos.z);
	}
}*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecMeshCreate : lineRendererControl
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
public class DoubleRoofIcon : RecMeshCreate
{
	public Vector3 rightUpPoint;
	public Vector3 rightDownPoint;
	public Vector3 leftUpPoint;
	public Vector3 leftDownPoint;
	public void DoubleRoofIconCreate<T>(T thisGameObject, string objName, ColumnIcon columnIcon, float doubleRoofHeight, float doubleRoofWidth)
	where T : Component
	{
		body = new GameObject(objName);
		mFilter = body.AddComponent<MeshFilter>();

		rightDownPoint = columnIcon.rightColumn.upPoint.transform.position;
		leftDownPoint = columnIcon.leftColumn.upPoint.transform.position;

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
	public void AdjMesh(ColumnIcon columnIcon, float doubleRoofHeight, float doubleRoofWidth)
	{
		rightDownPoint = new Vector3(columnIcon.rightColumn.upPoint.transform.position.x, columnIcon.rightColumn.upPoint.transform.position.y, columnIcon.rightColumn.upPoint.transform.position.z);
		leftDownPoint = new Vector3(columnIcon.leftColumn.upPoint.transform.position.x, columnIcon.leftColumn.upPoint.transform.position.y, columnIcon.leftColumn.upPoint.transform.position.z);

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
public class FriezeIcon : RecMeshCreate
{
	public Vector3 rightUpPoint;
	public Vector3 rightDownPoint;
	public Vector3 leftUpPoint;
	public Vector3 leftDownPoint;
	public void FriezeIconCreate<T>(T thisGameObject, string objName, float ini_friezeHeight, ColumnIcon columnIcon) where T : Component
	{
		Vector3 h = new Vector3(0.0f, ini_friezeHeight, 0.0f);
		rightUpPoint = columnIcon.rightColumn.upPoint.transform.position;
		leftUpPoint = columnIcon.leftColumn.upPoint.transform.position;
		rightDownPoint = rightUpPoint - h;
		leftDownPoint = leftUpPoint - h;

		body = new GameObject(objName);

		mFilter = body.AddComponent<MeshFilter>();
		mFilter.mesh = CreatRecMesh(leftUpPoint, rightUpPoint, rightDownPoint, leftDownPoint, null);

		//frieze cp
		columnIcon.rightColumn.friezePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		columnIcon.rightColumn.friezePoint.tag = "ControlPoint";
		columnIcon.rightColumn.friezePoint.name = "FRD";
		columnIcon.rightColumn.friezePoint.transform.localScale = columnIcon.rightColumn.downPoint.transform.localScale;
		columnIcon.rightColumn.friezePoint.transform.position = rightDownPoint;

		columnIcon.leftColumn.friezePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		columnIcon.leftColumn.friezePoint.tag = "ControlPoint";
		columnIcon.leftColumn.friezePoint.name = "FLD";
		columnIcon.leftColumn.friezePoint.transform.localScale = columnIcon.rightColumn.downPoint.transform.localScale;
		columnIcon.leftColumn.friezePoint.transform.position = leftDownPoint;

		columnIcon.rightColumn.allObjList.Add(columnIcon.rightColumn.friezePoint);
		columnIcon.leftColumn.allObjList.Add(columnIcon.leftColumn.friezePoint);

		mRenderer = body.AddComponent<MeshRenderer>() as MeshRenderer;

		body.transform.parent = thisGameObject.transform;
		columnIcon.rightColumn.friezePoint.transform.parent = thisGameObject.transform;
		columnIcon.leftColumn.friezePoint.transform.parent = thisGameObject.transform;

		InitLineRender(thisGameObject);
		SetIconObjectColor(columnIcon);
	}
	public void  AdjMesh(ColumnIcon columnIcon)
	{
		float friezeHeight = columnIcon.rightColumn.upPoint.transform.position.y - columnIcon.rightColumn.friezePoint.transform.position.y;
		Vector3 h = new Vector3(0.0f, friezeHeight, 0.0f);
		mFilter.mesh.Clear();
		rightUpPoint = columnIcon.rightColumn.upPoint.transform.position;
		leftUpPoint = columnIcon.leftColumn.upPoint.transform.position;
		rightDownPoint = columnIcon.rightColumn.friezePoint.transform.position = rightUpPoint - h;
		leftDownPoint = columnIcon.leftColumn.friezePoint.transform.position = leftUpPoint - h;
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
	public void SetIconObjectColor(ColumnIcon columnIcon)
	{
		mRenderer.material.color = Color.red;
		columnIcon.rightColumn.friezePoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
		columnIcon.leftColumn.friezePoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
	}
}
public class WallIcon : RecMeshCreate
{
	public GameObject rightUpPoint;
	public GameObject rightDownPoint;
	public GameObject leftUpPoint;
	public GameObject leftDownPoint;

	public GameObject rightUpWindowPoint;
	public GameObject rightDownWindowPoint;
	public GameObject leftUpWindowPoint;
	public GameObject leftDownWindowPoint;

	
	public void WallIconCreate<T>(T thisGameObject, string objName, ColumnIcon columnIcon, float ini_wallWidth, float ini_windowsHeight) where T : Component
	{
		Vector3 wallOffset = new Vector3(Mathf.Abs(columnIcon.rightColumn.upPoint.transform.transform.position.x - columnIcon.leftColumn.upPoint.transform.transform.position.x) / 2.0f - ini_wallWidth, 0, 0);
		float columnHeight = columnIcon.rightColumn.upPoint.transform.transform.position.y - columnIcon.rightColumn.downPoint.transform.transform.position.y;
		Vector3 windowsOffset = new Vector3(0, columnHeight / 2.0f - ini_windowsHeight / 2.0f, 0);
		//WallIcon cp
		rightUpPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		rightUpPoint.tag = "ControlPoint";
		rightUpPoint.name = "WRU";
		rightUpPoint.transform.localScale = columnIcon.rightColumn.upPoint.transform.localScale;
		rightUpPoint.transform.position = columnIcon.rightColumn.upPoint.transform.transform.position - wallOffset;

		rightDownPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		rightDownPoint.tag = "ControlPoint";
		rightDownPoint.name = "WRD";
		rightDownPoint.transform.localScale = columnIcon.rightColumn.downPoint.transform.localScale;
		rightDownPoint.transform.position = columnIcon.rightColumn.downPoint.transform.transform.position - wallOffset;

		leftUpPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		leftUpPoint.tag = "ControlPoint";
		leftUpPoint.name = "WLU";
		leftUpPoint.transform.localScale = columnIcon.leftColumn.upPoint.transform.localScale;
		leftUpPoint.transform.position = columnIcon.leftColumn.upPoint.transform.transform.position + wallOffset;

		leftDownPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		leftDownPoint.tag = "ControlPoint";
		leftDownPoint.name = "WLD";
		leftDownPoint.transform.localScale = columnIcon.leftColumn.downPoint.transform.localScale;
		leftDownPoint.transform.position = columnIcon.leftColumn.downPoint.transform.transform.position + wallOffset;

		rightUpWindowPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		rightUpWindowPoint.tag = "ControlPoint";
		rightUpWindowPoint.name = "WWRU";
		rightUpWindowPoint.transform.localScale = columnIcon.rightColumn.upPoint.transform.localScale;
		rightUpWindowPoint.transform.position = columnIcon.rightColumn.upPoint.transform.transform.position - windowsOffset - wallOffset;

		rightDownWindowPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		rightDownWindowPoint.tag = "ControlPoint";
		rightDownWindowPoint.name = "WWRD";
		rightDownWindowPoint.transform.localScale = columnIcon.rightColumn.downPoint.transform.localScale;
		rightDownWindowPoint.transform.position = columnIcon.rightColumn.downPoint.transform.transform.position + windowsOffset - wallOffset;

		leftUpWindowPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		leftUpWindowPoint.tag = "ControlPoint";
		leftUpWindowPoint.name = "WWLU";
		leftUpWindowPoint.transform.localScale = columnIcon.leftColumn.upPoint.transform.localScale;
		leftUpWindowPoint.transform.position = columnIcon.leftColumn.upPoint.transform.transform.position - windowsOffset + wallOffset;

		leftDownWindowPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		leftDownWindowPoint.tag = "ControlPoint";
		leftDownWindowPoint.name = "WWLD";
		leftDownWindowPoint.transform.localScale = columnIcon.leftColumn.downPoint.transform.localScale;
		leftDownWindowPoint.transform.position = columnIcon.leftColumn.downPoint.transform.transform.position + windowsOffset + wallOffset;

		body = new GameObject(objName);

		mFilter = body.AddComponent<MeshFilter>();
		mFilter.mesh = CreatRecMesh(leftUpPoint.transform.position, rightUpPoint.transform.position, rightDownPoint.transform.position, leftDownPoint.transform.position, null);

		mRenderer = body.AddComponent<MeshRenderer>() as MeshRenderer;

		body.transform.parent = thisGameObject.transform;

		leftUpPoint.transform.parent = thisGameObject.transform;
		leftDownPoint.transform.parent = thisGameObject.transform;
		rightUpPoint.transform.parent = thisGameObject.transform;
		rightDownPoint.transform.parent = thisGameObject.transform;

		leftUpWindowPoint.transform.parent = thisGameObject.transform;
		leftDownWindowPoint.transform.parent = thisGameObject.transform;
		rightUpWindowPoint.transform.parent = thisGameObject.transform;
		rightDownWindowPoint.transform.parent = thisGameObject.transform;

		InitLineRender(thisGameObject);
		SetIconObjectColor();
	}
	public void AdjMesh()
	{
		mFilter.mesh.Clear();
		mFilter.mesh = CreatRecMesh(leftUpPoint.transform.position, rightUpPoint.transform.position, rightDownPoint.transform.position, leftDownPoint.transform.position, mFilter.mesh);

		UpdateLineRender();
	}
	public override void InitLineRender<T>(T thisGameObject)
	{
		controlPointList.Add(leftUpPoint.transform.position);
		controlPointList.Add(rightUpPoint.transform.position);
		controlPointList.Add(rightDownPoint.transform.position);
		controlPointList.Add(leftDownPoint.transform.position);
		base.InitLineRender(thisGameObject);
	}
	public override void UpdateLineRender()
	{
		controlPointList[0] = (leftUpPoint.transform.position);
		controlPointList[1] = (rightUpPoint.transform.position);
		controlPointList[2] = (rightDownPoint.transform.position);
		controlPointList[3] = (leftDownPoint.transform.position);
		base.UpdateLineRender();
	}
	public void SetIconObjectColor()
	{
		mRenderer.material.color = Color.red;
		leftUpPoint.GetComponent<MeshRenderer>().material.color = Color.white;
		rightUpPoint.GetComponent<MeshRenderer>().material.color = Color.white;
		rightDownPoint.GetComponent<MeshRenderer>().material.color = Color.white;
		leftDownPoint.GetComponent<MeshRenderer>().material.color = Color.white;

		leftUpWindowPoint.GetComponent<MeshRenderer>().material.color = Color.white;
		rightUpWindowPoint.GetComponent<MeshRenderer>().material.color = Color.white;
		rightDownWindowPoint.GetComponent<MeshRenderer>().material.color = Color.white;
		leftDownWindowPoint.GetComponent<MeshRenderer>().material.color = Color.white;
	}
}
public class BalustradeIcon : RecMeshCreate
{
	public Vector3 rightUpPoint;
	public Vector3 rightDownPoint;
	public Vector3 leftUpPoint;
	public Vector3 leftDownPoint;
	public void BalustradeIconCreate<T>(T thisGameObject, string objName, float ini_balustradeHeight, ColumnIcon columnIcon) where T : Component
	{
		Vector3 h = new Vector3(0.0f, ini_balustradeHeight, 0.0f);
		rightDownPoint = columnIcon.rightColumn.downPoint.transform.position;
		leftDownPoint = columnIcon.leftColumn.downPoint.transform.position;
		rightUpPoint = rightDownPoint + h;
		leftUpPoint = leftDownPoint + h;

		body = new GameObject(objName);

		mFilter = body.AddComponent<MeshFilter>();
		mFilter.mesh = CreatRecMesh(leftUpPoint, rightUpPoint, rightDownPoint, leftDownPoint, null);

		//frieze cp
		columnIcon.rightColumn.balustradePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		columnIcon.rightColumn.balustradePoint.tag = "ControlPoint";
		columnIcon.rightColumn.balustradePoint.name = "BRU";
		columnIcon.rightColumn.balustradePoint.transform.localScale = columnIcon.rightColumn.downPoint.transform.localScale;
		columnIcon.rightColumn.balustradePoint.transform.position = rightUpPoint;

		columnIcon.leftColumn.balustradePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		columnIcon.leftColumn.balustradePoint.tag = "ControlPoint";
		columnIcon.leftColumn.balustradePoint.name = "BLU";
		columnIcon.leftColumn.balustradePoint.transform.localScale = columnIcon.rightColumn.downPoint.transform.localScale;
		columnIcon.leftColumn.balustradePoint.transform.position = leftUpPoint;

		columnIcon.rightColumn.allObjList.Add(columnIcon.rightColumn.balustradePoint);
		columnIcon.leftColumn.allObjList.Add(columnIcon.leftColumn.balustradePoint);

		mRenderer = body.AddComponent<MeshRenderer>() as MeshRenderer;

		body.transform.parent = thisGameObject.transform;
		columnIcon.rightColumn.balustradePoint.transform.parent = thisGameObject.transform;
		columnIcon.leftColumn.balustradePoint.transform.parent = thisGameObject.transform;

		InitLineRender(thisGameObject);
		SetIconObjectColor(columnIcon);
	}
	public void AdjMesh(ColumnIcon columnIcon)
	{
		float balustradeHeight = columnIcon.rightColumn.balustradePoint.transform.position.y - columnIcon.rightColumn.downPoint.transform.position.y;
		Vector3 h = new Vector3(0.0f, balustradeHeight, 0.0f);
		mFilter.mesh.Clear();
		rightDownPoint = columnIcon.rightColumn.downPoint.transform.position;
		leftDownPoint = columnIcon.leftColumn.downPoint.transform.position;
		rightUpPoint = columnIcon.rightColumn.balustradePoint.transform.position = rightDownPoint + h;
		leftUpPoint = columnIcon.leftColumn.balustradePoint.transform.position = leftDownPoint + h;
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
	public void SetIconObjectColor(ColumnIcon columnIcon)
	{
		mRenderer.material.color = Color.red;
		columnIcon.rightColumn.balustradePoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
		columnIcon.leftColumn.balustradePoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
	}
}
public class ColumnIcon
{
	public Column leftColumn;
	public Column rightColumn;
	public void ColumnIconCreate<T>(T thisGameObject, GameObject rightUpPoint, GameObject rightDownPoint, GameObject leftUpPoint, GameObject leftDownPoint, float columnHeight) where T : Component
	{
		leftColumn = new Column(leftUpPoint, leftDownPoint, columnHeight);
		rightColumn = new Column(rightUpPoint, rightDownPoint, columnHeight);

		leftColumn.body.transform.parent = thisGameObject.transform;
		rightColumn.body.transform.parent = thisGameObject.transform;
	}
	/*public void AdjPos(Vector3 tmp)
	{
		float disToleftPoint = (tmp.x - leftColumn.upPoint.transform.position.x);
		float disToRightPoint = (tmp.x - rightColumn.upPoint.transform.position.x);
		float disToUpPoint = (tmp.y - rightColumn.upPoint.transform.position.y);
		float disToDownPoint = (tmp.y - rightColumn.downPoint.transform.position.y);
		if (disToleftPoint != 0)
		{
			for (int i = 0; i < rightColumn.allObjList.Count; i++)
			{
				rightColumn.allObjList[i].transform.position = new Vector3(tmp.x, rightColumn.allObjList[i].transform.position.y, rightColumn.allObjList[i].transform.position.z);
				leftColumn.allObjList[i].transform.position = new Vector3(leftColumn.allObjList[i].transform.position.x - (disToleftPoint), leftColumn.allObjList[i].transform.position.y, leftColumn.allObjList[i].transform.position.z);
			}
		}
		else if (disToRightPoint != 0)
		{
			for (int i = 0; i < leftColumn.allObjList.Count; i++)
			{
				leftColumn.allObjList[i].transform.position = new Vector3(tmp.x, leftColumn.allObjList[i].transform.position.y, leftColumn.allObjList[i].transform.position.z);
				rightColumn.allObjList[i].transform.position = new Vector3(rightColumn.allObjList[i].transform.position.x - (disToRightPoint), rightColumn.allObjList[i].transform.position.y, rightColumn.allObjList[i].transform.position.z);

			}
		}
		else if (disToUpPoint!=0)
		{
			float cylinderHeight = (tmp.y - rightColumn.downPoint.transform.position.y);
			cylinderHeight = Mathf.Abs(cylinderHeight);
			//update point
			rightColumn.upPoint.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, tmp.y, rightColumn.upPoint.transform.position.z);
			leftColumn.upPoint.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, tmp.y, leftColumn.upPoint.transform.position.z);

			rightColumn.body.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, rightColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, rightColumn.upPoint.transform.position.z);
			
			leftColumn.body.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, leftColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, leftColumn.upPoint.transform.position.z);

		}
		else if (disToDownPoint!=0)
		{
			float cylinderHeight = (tmp.y - rightColumn.upPoint.transform.position.y);
			cylinderHeight = Mathf.Abs(cylinderHeight);

			//update point
			rightColumn.downPoint.transform.position = new Vector3(rightColumn.downPoint.transform.position.x, tmp.y, rightColumn.downPoint.transform.position.z);
			leftColumn.downPoint.transform.position = new Vector3(leftColumn.downPoint.transform.position.x, tmp.y, rightColumn.downPoint.transform.position.z);

			rightColumn.body.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, rightColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, rightColumn.upPoint.transform.position.z);
		
			leftColumn.body.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, leftColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, leftColumn.upPoint.transform.position.z);
		}
	}*/
}
public class Column : RecMeshCreate
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
		mFilter = body.GetComponent<MeshFilter>();
		mRenderer = body.GetComponent<MeshRenderer>();
		mRenderer.sortingOrder = 0;
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

	//just try two point
	private ColumnIcon columnIcon;

	private DragItemController dragitemcontroller;
	private Movement movement;

	public DoubleRoofIcon doubleRoofIcon;

	public FriezeIcon friezeIcon;
	public BalustradeIcon balustradeIcon;

	public WallIcon wallIcon;

	public Vector2 ini_bodydis;
	public Vector2 chang_bodydis;
	public Vector2 ratio_bodydis;
	public float chang_walldis;
	public float ratio_walldis;
	public float ini_friezeHeight;
	public float ini_balustradeHeight;
	public float ini_doubleRoofWidth;
	public float ini_doubleRoofHeight;
	public float ini_windowHeight;
	public float ini_wallWidth;

	public bool isBalustrade;
	public bool isFrieze;
	public bool isDoubleRoof;
	public bool isWall;

	public float cylinderHeight;
	public float friezeHeight;
	public float balustradeHeight;
	public float windowHeight;
	public float windowUp2TopDis;
	public float windowDown2ButtonDis;

	// Use this for initialization

	void Start()
	{
		dragitemcontroller = GameObject.Find("DragItemController").GetComponent<DragItemController>();
		movement = GameObject.Find("Movement").GetComponent<Movement>();
		movement.verlist.AddRange(controlPointList);

		cylinderHeight = ini_bodydis.y = Vector3.Distance(controlPointList[0].transform.position, controlPointList[1].transform.position);

		columnIcon = new ColumnIcon();
		columnIcon.ColumnIconCreate(this, controlPointList[2], controlPointList[3],controlPointList[0], controlPointList[1], ini_bodydis.y);


		movement.horlist.Add(columnIcon.leftColumn.body);
		movement.horlist.Add(columnIcon.rightColumn.body);


		friezeHeight = ini_friezeHeight = 0.2f * ini_bodydis.y;
		balustradeHeight = ini_balustradeHeight = 0.2f * ini_bodydis.y;
		windowHeight = ini_windowHeight = 0.5f * ini_bodydis.y;
		ini_doubleRoofHeight = 0.4f * ini_bodydis.y;
		ini_doubleRoofWidth = 0.3f * ini_bodydis.y;
		ini_bodydis.x = columnIcon.rightColumn.upPoint.transform.position.x - columnIcon.leftColumn.upPoint.transform.position.x;
		ini_bodydis.x = ini_bodydis.x / 2.0f;
		ini_wallWidth = ini_bodydis.x * 0.6f;
	}

	public void adjPos()
	{
		ratio_bodydis=chang_bodydis= Vector2.zero;
		ratio_walldis =chang_walldis=0;
		Vector3 tmp = dragitemcontroller.chooseObj.transform.position;
		if (dragitemcontroller.chooseObj == columnIcon.rightColumn.upPoint || dragitemcontroller.chooseObj == columnIcon.leftColumn.upPoint)//RU LU
		{
			float dis = (tmp.y - columnIcon.rightColumn.upPoint.transform.position.y);
			cylinderHeight = (dragitemcontroller.chooseObj.transform.position.y - columnIcon.rightColumn.downPoint.transform.position.y);
			cylinderHeight = Mathf.Abs(cylinderHeight);
			//update point
			columnIcon.rightColumn.upPoint.transform.position = new Vector3(columnIcon.rightColumn.upPoint.transform.position.x, tmp.y, columnIcon.rightColumn.upPoint.transform.position.z);
			columnIcon.leftColumn.upPoint.transform.position = new Vector3(columnIcon.leftColumn.upPoint.transform.position.x, tmp.y, columnIcon.leftColumn.upPoint.transform.position.z);


			columnIcon.rightColumn.body.transform.position = new Vector3(columnIcon.rightColumn.upPoint.transform.position.x, columnIcon.rightColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, columnIcon.rightColumn.upPoint.transform.position.z);
			columnIcon.leftColumn.body.transform.position = new Vector3(columnIcon.leftColumn.upPoint.transform.position.x, columnIcon.leftColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, columnIcon.leftColumn.upPoint.transform.position.z);

			columnIcon.rightColumn.body.transform.localScale = new Vector3(columnIcon.rightColumn.radius, cylinderHeight / 2.0f, columnIcon.rightColumn.radius);
			columnIcon.leftColumn.body.transform.localScale = new Vector3(columnIcon.leftColumn.radius, cylinderHeight / 2.0f, columnIcon.leftColumn.radius);
			if (isFrieze)
			{
				columnIcon.rightColumn.friezePoint.transform.position = new Vector3(columnIcon.rightColumn.upPoint.transform.position.x, tmp.y - friezeHeight, columnIcon.rightColumn.upPoint.transform.position.z);
				columnIcon.leftColumn.friezePoint.transform.position = new Vector3(columnIcon.leftColumn.upPoint.transform.position.x, tmp.y - friezeHeight, columnIcon.leftColumn.upPoint.transform.position.z);

				friezeIcon.AdjMesh(columnIcon);
			}
			if (isDoubleRoof)
			{
				doubleRoofIcon.AdjMesh(columnIcon, ini_doubleRoofHeight, ini_doubleRoofWidth);
			}
			if(isWall)
			{
				wallIcon.rightUpPoint.transform.position = new Vector3(wallIcon.rightUpPoint.transform.position.x, tmp.y, wallIcon.rightUpPoint.transform.position.z);
				wallIcon.leftUpPoint.transform.position = new Vector3(wallIcon.leftUpPoint.transform.position.x, tmp.y, wallIcon.leftUpPoint.transform.position.z);

				windowUp2TopDis = wallIcon.rightUpPoint.transform.position.y - wallIcon.rightUpWindowPoint.transform.position.y;

				wallIcon.AdjMesh();
			}
			
			if (dragitemcontroller.chooseObj == columnIcon.rightColumn.upPoint)
			{	chang_bodydis.y = dis;
				ratio_bodydis.y = chang_bodydis.y / ini_bodydis.y;
			}
			if (dragitemcontroller.chooseObj == columnIcon.leftColumn.upPoint)
			{
				chang_bodydis.y = -dis;
				ratio_bodydis.y = chang_bodydis.y / ini_bodydis.y;
			}
		}
		else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.downPoint || dragitemcontroller.chooseObj == columnIcon.leftColumn.downPoint)//RD  LD
		{
			float dis = (tmp.y - columnIcon.rightColumn.downPoint.transform.position.y);

			cylinderHeight = (dragitemcontroller.chooseObj.transform.position.y - columnIcon.rightColumn.upPoint.transform.position.y);
			cylinderHeight = Mathf.Abs(cylinderHeight);

			//update point
			columnIcon.rightColumn.downPoint.transform.position = new Vector3(columnIcon.rightColumn.downPoint.transform.position.x, tmp.y, columnIcon.rightColumn.downPoint.transform.position.z);
			columnIcon.leftColumn.downPoint.transform.position = new Vector3(columnIcon.leftColumn.downPoint.transform.position.x, tmp.y, columnIcon.rightColumn.downPoint.transform.position.z);

			columnIcon.rightColumn.body.transform.position = new Vector3(columnIcon.rightColumn.upPoint.transform.position.x, columnIcon.rightColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, columnIcon.rightColumn.upPoint.transform.position.z);
			columnIcon.leftColumn.body.transform.position = new Vector3(columnIcon.leftColumn.upPoint.transform.position.x, columnIcon.leftColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, columnIcon.leftColumn.upPoint.transform.position.z);

			columnIcon.rightColumn.body.transform.localScale = new Vector3(columnIcon.rightColumn.radius, cylinderHeight / 2.0f, columnIcon.rightColumn.radius);
			columnIcon.leftColumn.body.transform.localScale = new Vector3(columnIcon.leftColumn.radius, cylinderHeight / 2.0f, columnIcon.leftColumn.radius);

			if (isBalustrade)
			{
				columnIcon.rightColumn.balustradePoint.transform.position = new Vector3(columnIcon.rightColumn.downPoint.transform.position.x, tmp.y + balustradeHeight, columnIcon.rightColumn.upPoint.transform.position.z);
				columnIcon.leftColumn.balustradePoint.transform.position = new Vector3(columnIcon.leftColumn.downPoint.transform.position.x, tmp.y + balustradeHeight, columnIcon.leftColumn.upPoint.transform.position.z);

				balustradeIcon.AdjMesh(columnIcon);
			}
			if (isWall)
			{
				wallIcon.rightDownPoint.transform.position = new Vector3(wallIcon.rightDownPoint.transform.position.x, tmp.y, wallIcon.rightDownPoint.transform.position.z);
				wallIcon.leftDownPoint.transform.position = new Vector3(wallIcon.leftDownPoint.transform.position.x, tmp.y, wallIcon.leftDownPoint.transform.position.z);

				windowDown2ButtonDis = wallIcon.rightDownWindowPoint.transform.position.y - wallIcon.rightDownPoint.transform.position.y;

				wallIcon.AdjMesh();
			}

			chang_bodydis.y = dis;
			ratio_bodydis.y = chang_bodydis.y / ini_bodydis.y;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.body)
		{
			float dis = (tmp.x - columnIcon.rightColumn.upPoint.transform.position.x);
			for (int i = 0; i < columnIcon.rightColumn.allObjList.Count; i++)
			{
				columnIcon.rightColumn.allObjList[i].transform.position = new Vector3(tmp.x, columnIcon.rightColumn.allObjList[i].transform.position.y, columnIcon.rightColumn.allObjList[i].transform.position.z);
				columnIcon.leftColumn.allObjList[i].transform.position = new Vector3(columnIcon.leftColumn.allObjList[i].transform.position.x - (dis), columnIcon.leftColumn.allObjList[i].transform.position.y, columnIcon.leftColumn.allObjList[i].transform.position.z);
			}
			if (isFrieze)
			{
				friezeIcon.AdjMesh(columnIcon);
			}
			if (isBalustrade)
			{
				balustradeIcon.AdjMesh(columnIcon);
			}
			if (isDoubleRoof)
			{
				doubleRoofIcon.AdjMesh(columnIcon, ini_doubleRoofHeight, ini_doubleRoofWidth);
			}

			chang_bodydis.x = dis;
			ratio_bodydis.x = chang_bodydis.x / ini_bodydis.x;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.leftColumn.body)
		{
			float dis = (tmp.x - columnIcon.leftColumn.upPoint.transform.position.x);

			for (int i = 0; i < columnIcon.leftColumn.allObjList.Count; i++)
			{
				columnIcon.leftColumn.allObjList[i].transform.position = new Vector3(tmp.x, columnIcon.leftColumn.allObjList[i].transform.position.y, columnIcon.leftColumn.allObjList[i].transform.position.z);
				columnIcon.rightColumn.allObjList[i].transform.position = new Vector3(columnIcon.rightColumn.allObjList[i].transform.position.x - (dis), columnIcon.rightColumn.allObjList[i].transform.position.y, columnIcon.rightColumn.allObjList[i].transform.position.z);

			}
			if (isFrieze)
			{
				friezeIcon.AdjMesh(columnIcon); ;
			}
			if (isBalustrade)
			{
				balustradeIcon.AdjMesh(columnIcon);
			}
			if (isDoubleRoof)
			{
				doubleRoofIcon.AdjMesh(columnIcon, ini_doubleRoofHeight, ini_doubleRoofWidth);
			}
			chang_bodydis.x = -dis;
			ratio_bodydis.x = chang_bodydis.x / ini_bodydis.x;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.friezePoint || dragitemcontroller.chooseObj == columnIcon.leftColumn.friezePoint)
		{//frieze

			columnIcon.leftColumn.friezePoint.transform.position = new Vector3(columnIcon.leftColumn.friezePoint.transform.position.x, tmp.y, columnIcon.leftColumn.friezePoint.transform.position.z);
			columnIcon.rightColumn.friezePoint.transform.position = new Vector3(columnIcon.rightColumn.friezePoint.transform.position.x, tmp.y, columnIcon.rightColumn.friezePoint.transform.position.z);


			friezeHeight = columnIcon.rightColumn.upPoint.transform.position.y - columnIcon.rightColumn.friezePoint.transform.position.y;

			friezeIcon.AdjMesh(columnIcon); ;


		}
		else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.balustradePoint || dragitemcontroller.chooseObj == columnIcon.leftColumn.balustradePoint)
		{ //balustrade

			columnIcon.leftColumn.balustradePoint.transform.position = new Vector3(columnIcon.leftColumn.balustradePoint.transform.position.x, tmp.y, columnIcon.leftColumn.balustradePoint.transform.position.z);
			columnIcon.rightColumn.balustradePoint.transform.position = new Vector3(columnIcon.rightColumn.balustradePoint.transform.position.x, tmp.y, columnIcon.rightColumn.balustradePoint.transform.position.z);

			balustradeHeight = columnIcon.rightColumn.balustradePoint.transform.position.y - columnIcon.rightColumn.downPoint.transform.position.y;
			balustradeIcon.AdjMesh(columnIcon);

		}
		else if (dragitemcontroller.chooseObj == wallIcon.rightUpPoint)
		{
			float dis = (tmp.x - wallIcon.rightDownPoint.transform.position.x);

			wallIcon.leftUpPoint.transform.position = new Vector3(wallIcon.leftUpPoint.transform.position.x - (dis), wallIcon.leftUpPoint.transform.position.y, wallIcon.leftUpPoint.transform.position.z);
			wallIcon.leftDownPoint.transform.position = new Vector3(wallIcon.leftDownPoint.transform.position.x - (dis), wallIcon.leftDownPoint.transform.position.y, wallIcon.leftDownPoint.transform.position.z);
			wallIcon.rightDownPoint.transform.position = new Vector3(tmp.x, wallIcon.rightDownPoint.transform.position.y, wallIcon.rightDownPoint.transform.position.z);


			wallIcon.leftUpWindowPoint.transform.position = new Vector3(wallIcon.leftUpWindowPoint.transform.position.x - (dis), wallIcon.leftUpWindowPoint.transform.position.y, wallIcon.leftUpWindowPoint.transform.position.z);
			wallIcon.leftDownWindowPoint.transform.position = new Vector3(wallIcon.leftDownWindowPoint.transform.position.x - (dis), wallIcon.leftDownWindowPoint.transform.position.y, wallIcon.leftDownWindowPoint.transform.position.z);
			wallIcon.rightDownWindowPoint.transform.position = new Vector3(tmp.x, wallIcon.rightDownWindowPoint.transform.position.y, wallIcon.rightDownWindowPoint.transform.position.z);
			wallIcon.rightUpWindowPoint.transform.position = new Vector3(tmp.x, wallIcon.rightUpWindowPoint.transform.position.y, wallIcon.rightUpWindowPoint.transform.position.z);

			wallIcon.AdjMesh();

			chang_walldis = dis;
			ratio_walldis = chang_walldis / ini_wallWidth;
		}
		else if (dragitemcontroller.chooseObj == wallIcon.leftUpPoint)
		{
			float dis = (tmp.x - wallIcon.leftDownPoint.transform.position.x);

			wallIcon.rightUpPoint.transform.position = new Vector3(wallIcon.rightUpPoint.transform.position.x + (dis), wallIcon.rightUpPoint.transform.position.y, wallIcon.rightUpPoint.transform.position.z);
			wallIcon.rightDownPoint.transform.position = new Vector3(wallIcon.rightDownPoint.transform.position.x + (dis), wallIcon.rightDownPoint.transform.position.y, wallIcon.rightDownPoint.transform.position.z);
			wallIcon.leftDownPoint.transform.position = new Vector3(tmp.x, wallIcon.leftDownPoint.transform.position.y, wallIcon.leftDownPoint.transform.position.z);

			wallIcon.rightUpWindowPoint.transform.position = new Vector3(wallIcon.rightUpWindowPoint.transform.position.x + (dis), wallIcon.rightUpWindowPoint.transform.position.y, wallIcon.rightUpWindowPoint.transform.position.z);
			wallIcon.rightDownWindowPoint.transform.position = new Vector3(wallIcon.rightDownWindowPoint.transform.position.x + (dis), wallIcon.rightDownWindowPoint.transform.position.y, wallIcon.rightDownWindowPoint.transform.position.z);
			wallIcon.leftUpWindowPoint.transform.position = new Vector3(tmp.x, wallIcon.leftUpWindowPoint.transform.position.y, wallIcon.leftUpWindowPoint.transform.position.z);
			wallIcon.leftDownWindowPoint.transform.position = new Vector3(tmp.x, wallIcon.leftDownWindowPoint.transform.position.y, wallIcon.leftDownWindowPoint.transform.position.z);

			wallIcon.AdjMesh();

			chang_walldis = -dis;
			ratio_walldis = chang_walldis / ini_wallWidth;
		}
		else if (dragitemcontroller.chooseObj == wallIcon.rightDownPoint)
		{
			float dis = (tmp.x - wallIcon.rightUpPoint.transform.position.x);

			wallIcon.leftUpPoint.transform.position = new Vector3(wallIcon.leftUpPoint.transform.position.x - (dis), wallIcon.leftUpPoint.transform.position.y, wallIcon.leftUpPoint.transform.position.z);
			wallIcon.leftDownPoint.transform.position = new Vector3(wallIcon.leftDownPoint.transform.position.x - (dis), wallIcon.leftDownPoint.transform.position.y, wallIcon.leftDownPoint.transform.position.z);
			wallIcon.rightUpPoint.transform.position = new Vector3(tmp.x, wallIcon.rightUpPoint.transform.position.y, wallIcon.rightUpPoint.transform.position.z);

			wallIcon.leftUpWindowPoint.transform.position = new Vector3(wallIcon.leftUpWindowPoint.transform.position.x - (dis), wallIcon.leftUpWindowPoint.transform.position.y, wallIcon.leftUpWindowPoint.transform.position.z);
			wallIcon.leftDownWindowPoint.transform.position = new Vector3(wallIcon.leftDownWindowPoint.transform.position.x - (dis), wallIcon.leftDownWindowPoint.transform.position.y, wallIcon.leftDownWindowPoint.transform.position.z);
			wallIcon.rightUpWindowPoint.transform.position = new Vector3(tmp.x, wallIcon.rightUpWindowPoint.transform.position.y, wallIcon.rightUpWindowPoint.transform.position.z);
			wallIcon.rightDownWindowPoint.transform.position = new Vector3(tmp.x, wallIcon.rightDownWindowPoint.transform.position.y, wallIcon.rightDownWindowPoint.transform.position.z);

			wallIcon.AdjMesh();

			chang_walldis = dis;
			ratio_walldis = chang_walldis / ini_wallWidth;
		}
		else if (dragitemcontroller.chooseObj == wallIcon.leftDownPoint)
		{
			float dis = (tmp.x - wallIcon.leftUpPoint.transform.position.x);
			wallIcon.rightUpPoint.transform.position = new Vector3(wallIcon.rightUpPoint.transform.position.x + (dis), wallIcon.rightUpPoint.transform.position.y, wallIcon.rightUpPoint.transform.position.z);
			wallIcon.rightDownPoint.transform.position = new Vector3(wallIcon.rightDownPoint.transform.position.x + (dis), wallIcon.rightDownPoint.transform.position.y, wallIcon.rightDownPoint.transform.position.z);
			wallIcon.leftUpPoint.transform.position = new Vector3(tmp.x, wallIcon.leftUpPoint.transform.position.y, wallIcon.leftUpPoint.transform.position.z);

			wallIcon.rightUpWindowPoint.transform.position = new Vector3(wallIcon.rightUpWindowPoint.transform.position.x + (dis), wallIcon.rightUpWindowPoint.transform.position.y, wallIcon.rightUpWindowPoint.transform.position.z);
			wallIcon.rightDownWindowPoint.transform.position = new Vector3(wallIcon.rightDownWindowPoint.transform.position.x + (dis), wallIcon.rightDownWindowPoint.transform.position.y, wallIcon.rightDownWindowPoint.transform.position.z);
			wallIcon.leftUpWindowPoint.transform.position = new Vector3(tmp.x, wallIcon.leftUpWindowPoint.transform.position.y, wallIcon.leftUpWindowPoint.transform.position.z);
			wallIcon.leftDownWindowPoint.transform.position = new Vector3(tmp.x, wallIcon.leftDownWindowPoint.transform.position.y, wallIcon.leftDownWindowPoint.transform.position.z);

			wallIcon.AdjMesh();

			chang_walldis = -dis;
			ratio_walldis = chang_walldis / ini_wallWidth;
		}
		else if (dragitemcontroller.chooseObj == wallIcon.rightUpWindowPoint)
		{
			float dis = (tmp.y - wallIcon.leftUpWindowPoint.transform.position.y);
			wallIcon.leftUpWindowPoint.transform.position = new Vector3(wallIcon.leftUpWindowPoint.transform.position.x, wallIcon.leftUpWindowPoint.transform.position.y+(dis), wallIcon.rightUpPoint.transform.position.z);

			windowHeight = wallIcon.rightUpWindowPoint.transform.position.y - wallIcon.rightDownWindowPoint.transform.position.y;

			windowUp2TopDis = wallIcon.rightUpPoint.transform.position.y - wallIcon.rightUpWindowPoint.transform.position.y;
		}
		else if (dragitemcontroller.chooseObj == wallIcon.leftUpWindowPoint)
		{
			float dis = (tmp.y - wallIcon.rightUpWindowPoint.transform.position.y);
			wallIcon.rightUpWindowPoint.transform.position = new Vector3(wallIcon.rightUpWindowPoint.transform.position.x, wallIcon.rightUpWindowPoint.transform.position.y + (dis), wallIcon.rightUpWindowPoint.transform.position.z);

			windowHeight = wallIcon.leftUpWindowPoint.transform.position.y - wallIcon.leftDownWindowPoint.transform.position.y;

			windowUp2TopDis = wallIcon.rightUpPoint.transform.position.y - wallIcon.rightUpWindowPoint.transform.position.y;
		}
		else if (dragitemcontroller.chooseObj == wallIcon.rightDownWindowPoint)
		{
			float dis = (tmp.y - wallIcon.leftDownWindowPoint.transform.position.y);
			wallIcon.leftDownWindowPoint.transform.position = new Vector3(wallIcon.leftDownWindowPoint.transform.position.x, wallIcon.leftDownWindowPoint.transform.position.y + (dis), wallIcon.leftDownWindowPoint.transform.position.z);

			windowHeight = wallIcon.rightUpWindowPoint.transform.position.y - wallIcon.rightDownWindowPoint.transform.position.y;

			windowDown2ButtonDis = wallIcon.rightDownWindowPoint.transform.position.y - wallIcon.rightDownPoint.transform.position.y;
		}
		else if (dragitemcontroller.chooseObj == wallIcon.leftDownWindowPoint)
		{
			float dis = (tmp.y - wallIcon.rightDownWindowPoint.transform.position.y);
			wallIcon.rightDownWindowPoint.transform.position = new Vector3(wallIcon.rightDownWindowPoint.transform.position.x, wallIcon.rightDownWindowPoint.transform.position.y + (dis), wallIcon.rightDownWindowPoint.transform.position.z);

			windowHeight = wallIcon.leftUpWindowPoint.transform.position.y - wallIcon.leftDownWindowPoint.transform.position.y;

			windowDown2ButtonDis = wallIcon.rightDownWindowPoint.transform.position.y - wallIcon.rightDownPoint.transform.position.y;
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
			case "Wall":
				if (!isWall)
					wallIcon = CreateWall();
				break;
		}
	}
	public void addpoint()
	{
		movement.verlist.AddRange(controlPointList);
		movement.horlist.Add(columnIcon.rightColumn.body);
		movement.horlist.Add(columnIcon.leftColumn.body);
		if (isWall)
		{
			movement.horlist.Add(wallIcon.rightDownPoint);
			movement.horlist.Add(wallIcon.rightUpPoint);
			movement.horlist.Add(wallIcon.leftDownPoint);
			movement.horlist.Add(wallIcon.leftUpPoint);

			movement.verlist.Add(wallIcon.rightDownWindowPoint);
			movement.verlist.Add(wallIcon.rightUpWindowPoint);
			movement.verlist.Add(wallIcon.leftDownWindowPoint);
			movement.verlist.Add(wallIcon.leftUpWindowPoint);
		}
	}
	WallIcon CreateWall()
	{
		isWall = true;

		WallIcon wall = new WallIcon();
		wall.WallIconCreate(this, "Wall_mesh", columnIcon, ini_wallWidth, ini_windowHeight);

		movement.horlist.Add(wall.rightDownPoint);
		movement.horlist.Add(wall.rightUpPoint);
		movement.horlist.Add(wall.leftDownPoint);
		movement.horlist.Add(wall.leftUpPoint);


		windowUp2TopDis = wall.rightUpPoint.transform.position.y - wall.rightUpWindowPoint.transform.position.y;
		windowDown2ButtonDis = wall.rightDownWindowPoint.transform.position.y - wall.rightDownPoint.transform.position.y;
		return wall;
	}

	DoubleRoofIcon CreateDoubleRoof()
	{
		isDoubleRoof = true;

		DoubleRoofIcon doubleRoof = new DoubleRoofIcon();
		doubleRoof.DoubleRoofIconCreate(this, "DoubleRoof_mesh",columnIcon, ini_doubleRoofHeight, ini_doubleRoofWidth);

		return doubleRoof;
	}

	FriezeIcon Createfrieze()
	{
		isFrieze = true;

		FriezeIcon frieze = new FriezeIcon();
		frieze.FriezeIconCreate(this, "frieze_mesh", ini_friezeHeight, columnIcon);

		controlPointList.Add(columnIcon.rightColumn.friezePoint);
		controlPointList.Add(columnIcon.leftColumn.friezePoint);

		movement.verlist.Add(columnIcon.rightColumn.friezePoint);
		movement.verlist.Add(columnIcon.leftColumn.friezePoint);

		return frieze;
	}
	BalustradeIcon Createbalustrade()
	{
		isBalustrade = true;

		BalustradeIcon balustrade = new BalustradeIcon();
		balustrade.BalustradeIconCreate(this, "blustrade_mesh", ini_balustradeHeight,columnIcon);

		controlPointList.Add(columnIcon.rightColumn.balustradePoint);
		controlPointList.Add(columnIcon.leftColumn.balustradePoint);

		movement.verlist.Add(columnIcon.rightColumn.balustradePoint);
		movement.verlist.Add(columnIcon.leftColumn.balustradePoint);

		return balustrade;

	}
	public Vector3 ClampPos(Vector3 inputPos)
	{
		float minClampX = float.MinValue;
		float maxClampX = float.MaxValue;
		float minClampY = float.MinValue;
		float maxClampY = float.MaxValue;
		float minWidth = ini_bodydis.x * 0.2f;
		float minHeight = ini_bodydis.y * 0.7f;
		float minFriezeHeight = ini_bodydis.y * 0.1f;
		float minBalustradeHeight = ini_bodydis.y * 0.1f;
		float minWallHeight = ini_bodydis.y * 0.1f;
		float minCloseHeight = ini_bodydis.y * 0.1f;
		if (dragitemcontroller.chooseObj == columnIcon.rightColumn.body)
		{
			if (isWall) minClampX =wallIcon.rightUpPoint.transform.position.x+minWidth;
			else minClampX = columnIcon.leftColumn.upPoint.transform.position.x + minWidth;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.leftColumn.body)
		{
			if (isWall) maxClampX = wallIcon.leftUpPoint.transform.position.x - minWidth;
			else maxClampX = columnIcon.leftColumn.upPoint.transform.position.x - minWidth;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.upPoint)
		{
			if (isWall) minClampY = wallIcon.rightUpWindowPoint.transform.position.y + minCloseHeight;
			else minClampY = columnIcon.rightColumn.downPoint.transform.position.y + minCloseHeight+friezeHeight+balustradeHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.downPoint)
		{
			if (isWall) maxClampY = wallIcon.rightDownWindowPoint.transform.position.y - minCloseHeight;
			else maxClampY = columnIcon.rightColumn.upPoint.transform.position.y - minCloseHeight - friezeHeight - balustradeHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.friezePoint)
		{
			minClampY = columnIcon.rightColumn.downPoint.transform.position.y + minCloseHeight+ balustradeHeight;
			maxClampY = columnIcon.rightColumn.upPoint.transform.position.y - minFriezeHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.balustradePoint)
		{
			minClampY = columnIcon.rightColumn.downPoint.transform.position.y + minBalustradeHeight;
			maxClampY = columnIcon.rightColumn.upPoint.transform.position.y - minCloseHeight - friezeHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.leftColumn.upPoint)
		{
			if (isWall) minClampY = wallIcon.leftUpWindowPoint.transform.position.y + minCloseHeight;
			else minClampY = columnIcon.leftColumn.downPoint.transform.position.y + minHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.leftColumn.downPoint)
		{
			if (isWall) maxClampY = wallIcon.leftDownWindowPoint.transform.position.y - minCloseHeight;
			else maxClampY = columnIcon.leftColumn.upPoint.transform.position.y - minHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.leftColumn.friezePoint)
		{
			minClampY = columnIcon.leftColumn.downPoint.transform.position.y + minCloseHeight + balustradeHeight;
			maxClampY = columnIcon.leftColumn.upPoint.transform.position.y - minFriezeHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.leftColumn.balustradePoint)
		{
			minClampY = columnIcon.leftColumn.downPoint.transform.position.y + minBalustradeHeight;
			maxClampY = columnIcon.leftColumn.upPoint.transform.position.y -minCloseHeight - friezeHeight;
		}
		else if (dragitemcontroller.chooseObj == wallIcon.rightUpPoint)
		{
			maxClampX = columnIcon.rightColumn.upPoint.transform.position.x - minWidth;
			minClampX = wallIcon.leftUpPoint.transform.position.x + minWidth;
		}
		else if (dragitemcontroller.chooseObj == wallIcon.rightDownPoint)
		{
			maxClampX = columnIcon.rightColumn.downPoint.transform.position.x - minWidth;
			minClampX = wallIcon.leftUpPoint.transform.position.x + minWidth;
		}
		else if (dragitemcontroller.chooseObj == wallIcon.leftUpPoint)
		{
			minClampX = columnIcon.leftColumn.upPoint.transform.position.x + minWidth;
			maxClampX = wallIcon.rightUpPoint.transform.position.x - minWidth;
		}
		else if (dragitemcontroller.chooseObj == wallIcon.leftDownPoint)
		{
			minClampX = columnIcon.leftColumn.downPoint.transform.position.x + minWidth;
			maxClampX = wallIcon.rightUpPoint.transform.position.x - minWidth;
		}
		else if (dragitemcontroller.chooseObj == wallIcon.rightUpWindowPoint)
		{
			minClampY = wallIcon.rightDownWindowPoint.transform.position.y + minWallHeight;
			maxClampY = wallIcon.rightUpPoint.transform.position.y - minCloseHeight;
		}
		else if (dragitemcontroller.chooseObj == wallIcon.leftUpWindowPoint)
		{
			minClampY = wallIcon.leftDownWindowPoint.transform.position.y + minWallHeight;
			maxClampY = wallIcon.leftUpPoint.transform.position.y - minCloseHeight;
		}
		else if (dragitemcontroller.chooseObj == wallIcon.rightDownWindowPoint)
		{
			minClampY = wallIcon.rightDownPoint.transform.position.y + minCloseHeight;
			maxClampY = wallIcon.rightUpWindowPoint.transform.position.y - minWallHeight;		
		}
		else if (dragitemcontroller.chooseObj == wallIcon.leftDownWindowPoint)
		{
			minClampY = wallIcon.leftDownPoint.transform.position.y + minCloseHeight;
			maxClampY = wallIcon.leftUpWindowPoint.transform.position.y - minWallHeight;
		}
		float posX = Mathf.Clamp(inputPos.x, minClampX, maxClampX);
		float posY = Mathf.Clamp(inputPos.y, minClampY, maxClampY);

		return new Vector3(posX, posY, inputPos.z);
	}
}