/*

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecMeshCreate : IconObject
{
	public List<Vector3> controlPointList_Vec3 = new List<Vector3>();//用於lineRenderer

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
		for (int i = 0; i < controlPointList_Vec3.Count; i++)
		{
			if (i != controlPointList_Vec3.Count - 1)
				CreateLineRenderer(thisGameObject, controlPointList_Vec3[i], controlPointList_Vec3[i + 1]);
			else
				CreateLineRenderer(thisGameObject, controlPointList_Vec3[i], controlPointList_Vec3[0]);
		}
	}
	public override void UpdateLineRender()
	{
		for (int i = 0; i < lineRenderList.Count; i++)
		{
			if (i != controlPointList_Vec3.Count - 1)
				AdjLineRenderer(i, controlPointList_Vec3[i], controlPointList_Vec3[i + 1]);
			else
				AdjLineRenderer(i, controlPointList_Vec3[i], controlPointList_Vec3[0]);
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
		controlPointList_Vec3.Add(leftUpPoint);
		controlPointList_Vec3.Add(rightUpPoint);
		controlPointList_Vec3.Add(rightDownPoint);
		controlPointList_Vec3.Add(leftDownPoint);
		base.InitLineRender(thisGameObject);
	}
	public override void UpdateLineRender()
	{
		controlPointList_Vec3[0] = (leftUpPoint);
		controlPointList_Vec3[1] = (rightUpPoint);
		controlPointList_Vec3[2] = (rightDownPoint);
		controlPointList_Vec3[3] = (leftDownPoint);
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
		controlPointList_Vec3.Add(leftUpPoint);
		controlPointList_Vec3.Add(rightUpPoint);
		controlPointList_Vec3.Add(rightDownPoint);
		controlPointList_Vec3.Add(leftDownPoint);
		base.InitLineRender(thisGameObject);
	}
	public override void UpdateLineRender()
	{
		controlPointList_Vec3[0] = (leftUpPoint);
		controlPointList_Vec3[1] = (rightUpPoint);
		controlPointList_Vec3[2] = (rightDownPoint);
		controlPointList_Vec3[3] = (leftDownPoint);
		base.UpdateLineRender();
	}
	public void SetIconObjectColor(ColumnIcon columnIcon)
	{
		mRenderer.material.color = Color.red;
		columnIcon.rightColumn.friezePoint.GetComponent<MeshRenderer>().material=outLineShader;
		columnIcon.rightColumn.friezePoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
		columnIcon.leftColumn.friezePoint.GetComponent<MeshRenderer>().material=outLineShader;
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
		controlPointList_Vec3.Add(leftUpPoint.transform.position);
		controlPointList_Vec3.Add(rightUpPoint.transform.position);
		controlPointList_Vec3.Add(rightDownPoint.transform.position);
		controlPointList_Vec3.Add(leftDownPoint.transform.position);
		base.InitLineRender(thisGameObject);
	}
	public override void UpdateLineRender()
	{
		controlPointList_Vec3[0] = (leftUpPoint.transform.position);
		controlPointList_Vec3[1] = (rightUpPoint.transform.position);
		controlPointList_Vec3[2] = (rightDownPoint.transform.position);
		controlPointList_Vec3[3] = (leftDownPoint.transform.position);
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
		controlPointList_Vec3.Add(leftUpPoint);
		controlPointList_Vec3.Add(rightUpPoint);
		controlPointList_Vec3.Add(rightDownPoint);
		controlPointList_Vec3.Add(leftDownPoint);
		base.InitLineRender(thisGameObject);
	}
	public override void UpdateLineRender()
	{
		controlPointList_Vec3[0] = (leftUpPoint);
		controlPointList_Vec3[1] = (rightUpPoint);
		controlPointList_Vec3[2] = (rightDownPoint);
		controlPointList_Vec3[3] = (leftDownPoint);
		base.UpdateLineRender();
	}
	public void SetIconObjectColor(ColumnIcon columnIcon)
	{
		mRenderer.material.color = Color.red;
		columnIcon.rightColumn.balustradePoint.GetComponent<MeshRenderer>().material = outLineShader;
		columnIcon.rightColumn.balustradePoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
		columnIcon.leftColumn.balustradePoint.GetComponent<MeshRenderer>().material=outLineShader;
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
	/ *public void AdjPos(Vector3 tmp)
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
	}* /
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

		mRenderer.material = outLineShader;
		mRenderer.material.color = Color.red;
		upPoint.GetComponent<MeshRenderer>().material = outLineShader;
		upPoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
		downPoint.GetComponent<MeshRenderer>().material=outLineShader;
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
		float dis=0;
		if (dragitemcontroller.chooseObj == columnIcon.rightColumn.upPoint || dragitemcontroller.chooseObj == columnIcon.leftColumn.upPoint)//RU LU
		{
		
			if(dragitemcontroller.chooseObj == columnIcon.leftColumn.upPoint)
			{
				dis = (tmp.y - columnIcon.rightColumn.upPoint.transform.position.y);
				cylinderHeight = (dragitemcontroller.chooseObj.transform.position.y - columnIcon.leftColumn.downPoint.transform.position.y);
				cylinderHeight = Mathf.Abs(cylinderHeight);
			}
			else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.upPoint)
			{
				dis = (tmp.y - columnIcon.leftColumn.upPoint.transform.position.y);
				cylinderHeight = (dragitemcontroller.chooseObj.transform.position.y - columnIcon.rightColumn.downPoint.transform.position.y);
				cylinderHeight = Mathf.Abs(cylinderHeight);
			}

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
			chang_bodydis.y = dis;
			ratio_bodydis.y = chang_bodydis.y / ini_bodydis.y;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.downPoint || dragitemcontroller.chooseObj == columnIcon.leftColumn.downPoint)//RD  LD
		{
			if (dragitemcontroller.chooseObj == columnIcon.leftColumn.downPoint)
			{
				dis = (tmp.y - columnIcon.rightColumn.downPoint.transform.position.y);

				cylinderHeight = (dragitemcontroller.chooseObj.transform.position.y - columnIcon.leftColumn.upPoint.transform.position.y);
				cylinderHeight = Mathf.Abs(cylinderHeight);

			}
			else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.downPoint)
			{
				dis = (tmp.y - columnIcon.leftColumn.downPoint.transform.position.y);

				cylinderHeight = (dragitemcontroller.chooseObj.transform.position.y - columnIcon.rightColumn.upPoint.transform.position.y);
				cylinderHeight = Mathf.Abs(cylinderHeight);

			}

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
			chang_bodydis.y = -dis;
			ratio_bodydis.y = chang_bodydis.y / ini_bodydis.y;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.body)
		{
			dis = (tmp.x - columnIcon.rightColumn.upPoint.transform.position.x);
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
			dis = (tmp.x - columnIcon.leftColumn.upPoint.transform.position.x);

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
			 dis = (tmp.x - wallIcon.rightDownPoint.transform.position.x);

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
			dis = (tmp.x - wallIcon.leftDownPoint.transform.position.x);

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
			dis = (tmp.x - wallIcon.rightUpPoint.transform.position.x);

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
			dis = (tmp.x - wallIcon.leftUpPoint.transform.position.x);
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
			dis = (tmp.y - wallIcon.leftUpWindowPoint.transform.position.y);
			wallIcon.leftUpWindowPoint.transform.position = new Vector3(wallIcon.leftUpWindowPoint.transform.position.x, wallIcon.leftUpWindowPoint.transform.position.y+(dis), wallIcon.rightUpPoint.transform.position.z);

			windowHeight = wallIcon.rightUpWindowPoint.transform.position.y - wallIcon.rightDownWindowPoint.transform.position.y;

			windowUp2TopDis = wallIcon.rightUpPoint.transform.position.y - wallIcon.rightUpWindowPoint.transform.position.y;
		}
		else if (dragitemcontroller.chooseObj == wallIcon.leftUpWindowPoint)
		{
			dis = (tmp.y - wallIcon.rightUpWindowPoint.transform.position.y);
			wallIcon.rightUpWindowPoint.transform.position = new Vector3(wallIcon.rightUpWindowPoint.transform.position.x, wallIcon.rightUpWindowPoint.transform.position.y + (dis), wallIcon.rightUpWindowPoint.transform.position.z);

			windowHeight = wallIcon.leftUpWindowPoint.transform.position.y - wallIcon.leftDownWindowPoint.transform.position.y;

			windowUp2TopDis = wallIcon.rightUpPoint.transform.position.y - wallIcon.rightUpWindowPoint.transform.position.y;
		}
		else if (dragitemcontroller.chooseObj == wallIcon.rightDownWindowPoint)
		{
			 dis = (tmp.y - wallIcon.leftDownWindowPoint.transform.position.y);
			wallIcon.leftDownWindowPoint.transform.position = new Vector3(wallIcon.leftDownWindowPoint.transform.position.x, wallIcon.leftDownWindowPoint.transform.position.y + (dis), wallIcon.leftDownWindowPoint.transform.position.z);

			windowHeight = wallIcon.rightUpWindowPoint.transform.position.y - wallIcon.rightDownWindowPoint.transform.position.y;

			windowDown2ButtonDis = wallIcon.rightDownWindowPoint.transform.position.y - wallIcon.rightDownPoint.transform.position.y;
		}
		else if (dragitemcontroller.chooseObj == wallIcon.leftDownWindowPoint)
		{
			 dis = (tmp.y - wallIcon.rightDownWindowPoint.transform.position.y);
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
}*/

/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecMeshCreate : IconObject
{
	public List<Vector3> controlPointList_Vec3 = new List<Vector3>();//用於lineRenderer

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
		for (int i = 0; i < controlPointList_Vec3.Count; i++)
		{
			if (i != controlPointList_Vec3.Count - 1)
				CreateLineRenderer(thisGameObject, controlPointList_Vec3[i], controlPointList_Vec3[i + 1]);
			else
				CreateLineRenderer(thisGameObject, controlPointList_Vec3[i], controlPointList_Vec3[0]);
		}
	}
	public override void UpdateLineRender()
	{
		for (int i = 0; i < lineRenderList.Count; i++)
		{
			if (i != controlPointList_Vec3.Count - 1)
				AdjLineRenderer(i, controlPointList_Vec3[i], controlPointList_Vec3[i + 1]);
			else
				AdjLineRenderer(i, controlPointList_Vec3[i], controlPointList_Vec3[0]);
		}
	}
}
public class DoubleRoofIcon : RecMeshCreate
{
	public enum PointIndex { LeftUpPoint = 0, RightUpPoint = 1, RightDownPoint = 2, LeftDownPoint = 3, };
	public Vector3 rightUpPoint;
	public Vector3 rightDownPoint;
	public Vector3 leftUpPoint;
	public Vector3 leftDownPoint;
	public float doubleRoofWidth;
	public float doubleRoofHeight;
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
		controlPointList_Vec3.Add(leftUpPoint);
		controlPointList_Vec3.Add(rightUpPoint);
		controlPointList_Vec3.Add(rightDownPoint);
		controlPointList_Vec3.Add(leftDownPoint);
		base.InitLineRender(thisGameObject);
	}
	public override void UpdateLineRender()
	{
		controlPointList_Vec3[(int)PointIndex.LeftUpPoint] = (leftUpPoint);
		controlPointList_Vec3[(int)PointIndex.RightUpPoint] = (rightUpPoint);
		controlPointList_Vec3[(int)PointIndex.RightDownPoint] = (rightDownPoint);
		controlPointList_Vec3[(int)PointIndex.LeftDownPoint] = (leftDownPoint);
		base.UpdateLineRender();
	}
	public void SetIconObjectColor()
	{
		mRenderer.material.color = Color.red;
	}
}
public class FriezeIcon : RecMeshCreate
{
	public enum PointIndex { LeftUpPoint = 0, RightUpPoint = 1, RightDownPoint = 2, LeftDownPoint = 3, };
	public GameObject rightUpPoint;
	public GameObject rightDownPoint;
	public GameObject leftUpPoint;
	public GameObject leftDownPoint;
	public float friezeHeight;
	public void FriezeIconCreate<T>(T thisGameObject, string objName, ColumnIcon columnIcon, float ini_friezeHeight) where T : Component
	{
		Vector3 h = new Vector3(0.0f, ini_friezeHeight, 0.0f);
		rightUpPoint = columnIcon.rightColumn.upPoint;
		leftUpPoint = columnIcon.leftColumn.upPoint;
		Vector3 rightDownPointPos = rightUpPoint.transform.position - h;
		Vector3 leftDownPointPos = leftUpPoint.transform.position - h;

		//frieze cp
		columnIcon.rightColumn.friezePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		columnIcon.rightColumn.friezePoint.tag = "ControlPoint";
		columnIcon.rightColumn.friezePoint.name = "FRD";
		columnIcon.rightColumn.friezePoint.transform.localScale = columnIcon.rightColumn.downPoint.transform.localScale;
		columnIcon.rightColumn.friezePoint.transform.position = rightDownPointPos;
		rightDownPoint = columnIcon.rightColumn.friezePoint;

		columnIcon.leftColumn.friezePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		columnIcon.leftColumn.friezePoint.tag = "ControlPoint";
		columnIcon.leftColumn.friezePoint.name = "FLD";
		columnIcon.leftColumn.friezePoint.transform.localScale = columnIcon.rightColumn.downPoint.transform.localScale;
		columnIcon.leftColumn.friezePoint.transform.position = leftDownPointPos;
		leftDownPoint = columnIcon.leftColumn.friezePoint;

		body = new GameObject(objName);

		mFilter = body.AddComponent<MeshFilter>();
		mFilter.mesh = CreatRecMesh(leftUpPoint.transform.position, rightUpPoint.transform.position, rightDownPoint.transform.position, leftDownPoint.transform.position, null);
		//初始位置
		controlPointList.Add(leftUpPoint);
		controlPointList.Add(rightUpPoint);
		controlPointList.Add(rightDownPoint);
		controlPointList.Add(leftDownPoint);
		lastControlPointPosition = mFilter.mesh.vertices;

		columnIcon.rightColumn.controlPointList.Add(columnIcon.rightColumn.friezePoint);
		columnIcon.leftColumn.controlPointList.Add(columnIcon.leftColumn.friezePoint);

		mRenderer = body.AddComponent<MeshRenderer>() as MeshRenderer;

		body.transform.parent = thisGameObject.transform;
		columnIcon.rightColumn.friezePoint.transform.parent = thisGameObject.transform;
		columnIcon.leftColumn.friezePoint.transform.parent = thisGameObject.transform;

		InitLineRender(thisGameObject);
		SetIconObjectColor(columnIcon);
	}
	public void AdjMesh(ColumnIcon columnIcon)
	{
		float friezeHeight = columnIcon.rightColumn.upPoint.transform.position.y - columnIcon.rightColumn.friezePoint.transform.position.y;
		Vector3 h = new Vector3(0.0f, friezeHeight, 0.0f);
		mFilter.mesh.Clear();

		rightDownPoint.transform.position = rightUpPoint.transform.position - h;
		leftDownPoint.transform.position = leftUpPoint.transform.position - h;
		mFilter.mesh = CreatRecMesh(leftUpPoint.transform.position, rightUpPoint.transform.position, rightDownPoint.transform.position, leftDownPoint.transform.position, mFilter.mesh);

		UpdateLineRender();

	}
	public override void InitLineRender<T>(T thisGameObject)
	{
		controlPointList_Vec3.Add(leftUpPoint.transform.position);
		controlPointList_Vec3.Add(rightUpPoint.transform.position);
		controlPointList_Vec3.Add(rightDownPoint.transform.position);
		controlPointList_Vec3.Add(leftDownPoint.transform.position);
		base.InitLineRender(thisGameObject);
	}
	public override void UpdateLineRender()
	{
		controlPointList_Vec3[(int)PointIndex.LeftUpPoint] = (leftUpPoint.transform.position);
		controlPointList_Vec3[(int)PointIndex.RightUpPoint] = (rightUpPoint.transform.position);
		controlPointList_Vec3[(int)PointIndex.RightDownPoint] = (rightDownPoint.transform.position);
		controlPointList_Vec3[(int)PointIndex.LeftDownPoint] = (leftDownPoint.transform.position);
		base.UpdateLineRender();
	}
	public void SetIconObjectColor(ColumnIcon columnIcon)
	{
		mRenderer.material.color = Color.red;
		columnIcon.rightColumn.friezePoint.GetComponent<MeshRenderer>().material = outLineShader;
		columnIcon.rightColumn.friezePoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
		columnIcon.leftColumn.friezePoint.GetComponent<MeshRenderer>().material = outLineShader;
		columnIcon.leftColumn.friezePoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
	}
}
public class WallIcon : RecMeshCreate
{
	public enum PointIndex { LeftUpPoint = 0, RightUpPoint = 1, RightDownPoint = 2, LeftDownPoint = 3, };
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
		lastControlPointPosition = mFilter.mesh.vertices;


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
		controlPointList_Vec3.Add(leftUpPoint.transform.position);
		controlPointList_Vec3.Add(rightUpPoint.transform.position);
		controlPointList_Vec3.Add(rightDownPoint.transform.position);
		controlPointList_Vec3.Add(leftDownPoint.transform.position);
		base.InitLineRender(thisGameObject);
	}
	public override void UpdateLineRender()
	{
		controlPointList_Vec3[0] = (leftUpPoint.transform.position);
		controlPointList_Vec3[1] = (rightUpPoint.transform.position);
		controlPointList_Vec3[2] = (rightDownPoint.transform.position);
		controlPointList_Vec3[3] = (leftDownPoint.transform.position);
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
	public enum PointIndex { LeftUpPoint = 0, RightUpPoint = 1, RightDownPoint = 2, LeftDownPoint = 3, };
	public GameObject rightUpPoint;
	public GameObject rightDownPoint;
	public GameObject leftUpPoint;
	public GameObject leftDownPoint;
	public float balustradeHeight;
	public void BalustradeIconCreate<T>(T thisGameObject, string objName, ColumnIcon columnIcon, float ini_balustradeHeight) where T : Component
	{
		Vector3 h = new Vector3(0.0f, ini_balustradeHeight, 0.0f);
		rightDownPoint = columnIcon.rightColumn.downPoint;
		leftDownPoint = columnIcon.leftColumn.downPoint;
		Vector3 rightUpPointPos = rightDownPoint.transform.position + h;
		Vector3 leftUpPointPos = leftDownPoint.transform.position + h;

		//right
		columnIcon.rightColumn.balustradePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		columnIcon.rightColumn.balustradePoint.tag = "ControlPoint";
		columnIcon.rightColumn.balustradePoint.name = "BRU";
		columnIcon.rightColumn.balustradePoint.transform.localScale = columnIcon.rightColumn.downPoint.transform.localScale;
		columnIcon.rightColumn.balustradePoint.transform.position = rightUpPointPos;
		rightUpPoint = columnIcon.rightColumn.balustradePoint;
		//left
		columnIcon.leftColumn.balustradePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		columnIcon.leftColumn.balustradePoint.tag = "ControlPoint";
		columnIcon.leftColumn.balustradePoint.name = "BLU";
		columnIcon.leftColumn.balustradePoint.transform.localScale = columnIcon.rightColumn.downPoint.transform.localScale;
		columnIcon.leftColumn.balustradePoint.transform.position = leftUpPointPos;
		leftUpPoint = columnIcon.leftColumn.balustradePoint;
		//body
		body = new GameObject(objName);

		mFilter = body.AddComponent<MeshFilter>();
		mFilter.mesh = CreatRecMesh(leftUpPoint.transform.position, rightUpPoint.transform.position, rightDownPoint.transform.position, leftDownPoint.transform.position, null);
		//初始位置
		controlPointList.Add(leftUpPoint);
		controlPointList.Add(rightUpPoint);
		controlPointList.Add(rightDownPoint);
		controlPointList.Add(leftDownPoint);
		lastControlPointPosition = mFilter.mesh.vertices;

		columnIcon.rightColumn.controlPointList.Add(columnIcon.rightColumn.balustradePoint);
		columnIcon.leftColumn.controlPointList.Add(columnIcon.leftColumn.balustradePoint);

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
		rightUpPoint.transform.position = rightDownPoint.transform.position + h;
		leftUpPoint.transform.position = leftDownPoint.transform.position + h;
		mFilter.mesh = CreatRecMesh(leftUpPoint.transform.position, rightUpPoint.transform.position, rightDownPoint.transform.position, leftDownPoint.transform.position, mFilter.mesh);

		UpdateLineRender();
	}
	public override void InitLineRender<T>(T thisGameObject)
	{
		controlPointList_Vec3.Add(leftUpPoint.transform.position);
		controlPointList_Vec3.Add(rightUpPoint.transform.position);
		controlPointList_Vec3.Add(rightDownPoint.transform.position);
		controlPointList_Vec3.Add(leftDownPoint.transform.position);
		base.InitLineRender(thisGameObject);
	}
	public override void UpdateLineRender()
	{
		controlPointList_Vec3[(int)PointIndex.LeftUpPoint] = (leftUpPoint.transform.position);
		controlPointList_Vec3[(int)PointIndex.RightUpPoint] = (rightUpPoint.transform.position);
		controlPointList_Vec3[(int)PointIndex.RightDownPoint] = (rightDownPoint.transform.position);
		controlPointList_Vec3[(int)PointIndex.LeftDownPoint] = (leftDownPoint.transform.position);
		base.UpdateLineRender();
	}
	public void SetIconObjectColor(ColumnIcon columnIcon)
	{
		mRenderer.material.color = Color.red;
		columnIcon.rightColumn.balustradePoint.GetComponent<MeshRenderer>().material = outLineShader;
		columnIcon.rightColumn.balustradePoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
		columnIcon.leftColumn.balustradePoint.GetComponent<MeshRenderer>().material = outLineShader;
		columnIcon.leftColumn.balustradePoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
	}
}
public class ColumnIcon : IconObject
{
	public enum PointIndex { LeftUpPoint = 0, RightUpPoint = 1, RightDownPoint = 2, LeftDownPoint = 3, LeftColumnBody = 4, RightColumnBody = 5 };

	public Column leftColumn;
	public Column rightColumn;

	public DoubleRoofIcon doubleRoofIcon;

	public FriezeIcon friezeIcon;
	public BalustradeIcon balustradeIcon;

	public WallIcon wallIcon;

	public void ColumnIconCreate<T>(T thisGameObject, GameObject rightUpPoint, GameObject rightDownPoint, GameObject leftUpPoint, GameObject leftDownPoint, float columnHeight) where T : Component
	{
		leftColumn = new Column(leftUpPoint, leftDownPoint, columnHeight);
		rightColumn = new Column(rightUpPoint, rightDownPoint, columnHeight);

		leftColumn.body.transform.parent = thisGameObject.transform;
		rightColumn.body.transform.parent = thisGameObject.transform;

		controlPointList.Add(leftColumn.upPoint);
		controlPointList.Add(rightColumn.upPoint);
		controlPointList.Add(rightColumn.downPoint);
		controlPointList.Add(leftColumn.downPoint);
		lastControlPointPosition = new Vector3[controlPointList.Count];
		lastControlPointPosition[(int)PointIndex.LeftUpPoint] = leftColumn.upPoint.transform.position;
		lastControlPointPosition[(int)PointIndex.RightUpPoint] = rightColumn.upPoint.transform.position;
		lastControlPointPosition[(int)PointIndex.LeftUpPoint] = leftColumn.upPoint.transform.position;
		lastControlPointPosition[(int)PointIndex.RightUpPoint] = rightColumn.upPoint.transform.position;
	}
	public void CreateWall<T>(T thisGameObject, string objName, float ini_wallWidth, float ini_windowHeight) where T : Component
	{
		wallIcon = new WallIcon();
		wallIcon.WallIconCreate(thisGameObject, objName, this, ini_wallWidth, ini_windowHeight);
	}
	public void CreateDoubleRoof<T>(T thisGameObject, string objName, float ini_doubleRoofHeight, float ini_doubleRoofWidth) where T : Component
	{
		doubleRoofIcon = new DoubleRoofIcon();
		doubleRoofIcon.DoubleRoofIconCreate(thisGameObject, objName, this, ini_doubleRoofHeight, ini_doubleRoofWidth);
	}
	public void CreateBlustrade<T>(T thisGameObject, string objName, float ini_balustradeHeight) where T : Component
	{
		balustradeIcon = new BalustradeIcon();
		balustradeIcon.BalustradeIconCreate(thisGameObject, objName, this, ini_balustradeHeight);
	}
	public void CreateFrieze<T>(T thisGameObject, string objName, float ini_friezeHeight) where T : Component
	{
		friezeIcon = new FriezeIcon();
		friezeIcon.FriezeIconCreate(thisGameObject, objName, this, ini_friezeHeight);
	}
	public void AdjPos(Vector3 tmp, int index)
	{

		float cylinderHeight = 0;
		switch (index)
		{
			case (int)PointIndex.LeftUpPoint:
			case (int)PointIndex.RightUpPoint:
				cylinderHeight = (tmp.y - rightColumn.downPoint.transform.position.y);
				cylinderHeight = Mathf.Abs(cylinderHeight);
				//update point
				rightColumn.upPoint.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, tmp.y, rightColumn.upPoint.transform.position.z);
				leftColumn.upPoint.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, tmp.y, leftColumn.upPoint.transform.position.z);

				rightColumn.body.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, rightColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, rightColumn.upPoint.transform.position.z);

				leftColumn.body.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, leftColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, leftColumn.upPoint.transform.position.z);

				rightColumn.body.transform.localScale = new Vector3(rightColumn.radius, cylinderHeight / 2.0f, rightColumn.radius);
				leftColumn.body.transform.localScale = new Vector3(leftColumn.radius, cylinderHeight / 2.0f, leftColumn.radius);

				if (friezeIcon.body)
				{
					rightColumn.friezePoint.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, tmp.y - friezeIcon.friezeHeight, rightColumn.upPoint.transform.position.z);
					leftColumn.friezePoint.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, tmp.y - friezeIcon.friezeHeight, leftColumn.upPoint.transform.position.z);

					friezeIcon.AdjMesh(this);
				}
				if (doubleRoofIcon.body)
				{
					doubleRoofIcon.AdjMesh(this, doubleRoofIcon.doubleRoofHeight, doubleRoofIcon.doubleRoofWidth);
				}
				if (wallIcon.body)
				{
					wallIcon.rightUpPoint.transform.position = new Vector3(wallIcon.rightUpPoint.transform.position.x, tmp.y, wallIcon.rightUpPoint.transform.position.z);
					wallIcon.leftUpPoint.transform.position = new Vector3(wallIcon.leftUpPoint.transform.position.x, tmp.y, wallIcon.leftUpPoint.transform.position.z);


					wallIcon.AdjMesh();
				}
				break;
			case (int)PointIndex.RightDownPoint:
			case (int)PointIndex.LeftDownPoint:
				cylinderHeight = (tmp.y - rightColumn.upPoint.transform.position.y);
				cylinderHeight = Mathf.Abs(cylinderHeight);

				//update point
				rightColumn.downPoint.transform.position = new Vector3(rightColumn.downPoint.transform.position.x, tmp.y, rightColumn.downPoint.transform.position.z);
				leftColumn.downPoint.transform.position = new Vector3(leftColumn.downPoint.transform.position.x, tmp.y, rightColumn.downPoint.transform.position.z);

				rightColumn.body.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, rightColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, rightColumn.upPoint.transform.position.z);

				leftColumn.body.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, leftColumn.upPoint.transform.position.y - cylinderHeight / 2.0f, leftColumn.upPoint.transform.position.z);

				rightColumn.body.transform.localScale = new Vector3(rightColumn.radius, cylinderHeight / 2.0f, rightColumn.radius);
				leftColumn.body.transform.localScale = new Vector3(leftColumn.radius, cylinderHeight / 2.0f, leftColumn.radius);
			
			if (balustradeIcon.body)
			{
				rightColumn.balustradePoint.transform.position = new Vector3(rightColumn.downPoint.transform.position.x, tmp.y + balustradeIcon.balustradeHeight, rightColumn.upPoint.transform.position.z);
				leftColumn.balustradePoint.transform.position = new Vector3(leftColumn.downPoint.transform.position.x, tmp.y + balustradeIcon.balustradeHeight, leftColumn.upPoint.transform.position.z);

				balustradeIcon.AdjMesh(this);
			}
			if (wallIcon.body)
			{
				wallIcon.rightDownPoint.transform.position = new Vector3(wallIcon.rightDownPoint.transform.position.x, tmp.y, wallIcon.rightDownPoint.transform.position.z);
				wallIcon.leftDownPoint.transform.position = new Vector3(wallIcon.leftDownPoint.transform.position.x, tmp.y, wallIcon.leftDownPoint.transform.position.z);

				wallIcon.AdjMesh();
			}
				break;
			case (int)PointIndex.LeftColumnBody:
				float leftUpPointOffsetX = (tmp.x - lastControlPointPosition[(int)PointIndex.LeftUpPoint].x);
				for (int i = 0; i < rightColumn.controlPointList.Count; i++)
				{
					rightColumn.controlPointList[i].transform.position = new Vector3(tmp.x, rightColumn.controlPointList[i].transform.position.y, rightColumn.controlPointList[i].transform.position.z);
					leftColumn.controlPointList[i].transform.position = new Vector3(leftColumn.controlPointList[i].transform.position.x - (leftUpPointOffsetX), leftColumn.controlPointList[i].transform.position.y, leftColumn.controlPointList[i].transform.position.z);
				}
			if (friezeIcon.body)
			{
				friezeIcon.AdjMesh(this);
			}
			if (balustradeIcon.body)
			{
				balustradeIcon.AdjMesh(this);
			}
			if (doubleRoofIcon.body)
			{
				doubleRoofIcon.AdjMesh(this, doubleRoofIcon.doubleRoofHeight, doubleRoofIcon.doubleRoofWidth);
			}
				break;
			case (int)PointIndex.RightColumnBody:
				float rightUpPointOffsetX = (tmp.x - lastControlPointPosition[(int)PointIndex.RightUpPoint].x);
				for (int i = 0; i < leftColumn.controlPointList.Count; i++)
				{
					leftColumn.controlPointList[i].transform.position = new Vector3(tmp.x, leftColumn.controlPointList[i].transform.position.y, leftColumn.controlPointList[i].transform.position.z);
					rightColumn.controlPointList[i].transform.position = new Vector3(rightColumn.controlPointList[i].transform.position.x - (rightUpPointOffsetX), rightColumn.controlPointList[i].transform.position.y, rightColumn.controlPointList[i].transform.position.z);

				}
			if (friezeIcon.body)
			{
				friezeIcon.AdjMesh(this);
			}
			if (balustradeIcon.body)
			{
				balustradeIcon.AdjMesh(this);
			}
			if (doubleRoofIcon.body)
			{
				doubleRoofIcon.AdjMesh(this, doubleRoofIcon.doubleRoofHeight, doubleRoofIcon.doubleRoofWidth);
			}
				break;

		}

	}
}
public class Column : IconObject
{
	public enum PointIndex { UpPoint = 0, DownPoint = 1, Body = 2 };
	public GameObject upPoint = null;
	public GameObject downPoint = null;
	public GameObject balustradePoint = null;
	public GameObject friezePoint = null;
	public float radius = 0.01f;
	public Column(GameObject upPoint, GameObject downPoint, float columnHeight)
	{
		this.upPoint = upPoint;
		this.downPoint = downPoint;

		this.body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		mFilter = body.GetComponent<MeshFilter>();
		mRenderer = body.GetComponent<MeshRenderer>();
		mRenderer.sortingOrder = 0;

		controlPointList.Add(upPoint);
		controlPointList.Add(downPoint);
		controlPointList.Add(body);
		lastControlPointPosition = new Vector3[controlPointList.Count];
		lastControlPointPosition[(int)PointIndex.UpPoint] = upPoint.transform.position;
		lastControlPointPosition[(int)PointIndex.DownPoint] = downPoint.transform.position;

		this.body.transform.localScale = new Vector3(radius, columnHeight / 2.0f, radius);
		this.body.transform.position = new Vector3(upPoint.transform.position.x, upPoint.transform.position.y - columnHeight / 2.0f, upPoint.transform.position.z);

		this.body.tag = "Cylinder";

		lastControlPointPosition[(int)PointIndex.Body] = this.body.transform.position;

		SetIconObjectColor();
	}
	public void SetIconObjectColor()
	{

		mRenderer.material = outLineShader;
		mRenderer.material.color = Color.red;
		upPoint.GetComponent<MeshRenderer>().material = outLineShader;
		upPoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
		downPoint.GetComponent<MeshRenderer>().material = outLineShader;
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
		columnIcon.ColumnIconCreate(this, controlPointList[2], controlPointList[3], controlPointList[0], controlPointList[1], ini_bodydis.y);


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
		ratio_bodydis = chang_bodydis = Vector2.zero;
		ratio_walldis = chang_walldis = 0;
		Vector3 tmp = dragitemcontroller.chooseObj.transform.position;
		float dis = 0;
		if (dragitemcontroller.chooseObj == columnIcon.rightColumn.upPoint || dragitemcontroller.chooseObj == columnIcon.leftColumn.upPoint)//RU LU
		{

			if (dragitemcontroller.chooseObj == columnIcon.leftColumn.upPoint)
			{
				dis = (tmp.y - columnIcon.rightColumn.upPoint.transform.position.y);
				cylinderHeight = (dragitemcontroller.chooseObj.transform.position.y - columnIcon.leftColumn.downPoint.transform.position.y);
				cylinderHeight = Mathf.Abs(cylinderHeight);
			}
			else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.upPoint)
			{
				dis = (tmp.y - columnIcon.leftColumn.upPoint.transform.position.y);
				cylinderHeight = (dragitemcontroller.chooseObj.transform.position.y - columnIcon.rightColumn.downPoint.transform.position.y);
				cylinderHeight = Mathf.Abs(cylinderHeight);
			}

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

				columnIcon.friezeIcon.AdjMesh(columnIcon);
			}
			if (isDoubleRoof)
			{
				columnIcon.doubleRoofIcon.AdjMesh(columnIcon, ini_doubleRoofHeight, ini_doubleRoofWidth);
			}
			if (isWall)
			{
				columnIcon.wallIcon.rightUpPoint.transform.position = new Vector3(columnIcon.wallIcon.rightUpPoint.transform.position.x, tmp.y, columnIcon.wallIcon.rightUpPoint.transform.position.z);
				columnIcon.wallIcon.leftUpPoint.transform.position = new Vector3(columnIcon.wallIcon.leftUpPoint.transform.position.x, tmp.y, columnIcon.wallIcon.leftUpPoint.transform.position.z);

				windowUp2TopDis = columnIcon.wallIcon.rightUpPoint.transform.position.y - columnIcon.wallIcon.rightUpWindowPoint.transform.position.y;

				columnIcon.wallIcon.AdjMesh();
			}
			chang_bodydis.y = dis;
			ratio_bodydis.y = chang_bodydis.y / ini_bodydis.y;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.downPoint || dragitemcontroller.chooseObj == columnIcon.leftColumn.downPoint)//RD  LD
		{
			if (dragitemcontroller.chooseObj == columnIcon.leftColumn.downPoint)
			{
				dis = (tmp.y - columnIcon.rightColumn.downPoint.transform.position.y);

				cylinderHeight = (dragitemcontroller.chooseObj.transform.position.y - columnIcon.leftColumn.upPoint.transform.position.y);
				cylinderHeight = Mathf.Abs(cylinderHeight);

			}
			else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.downPoint)
			{
				dis = (tmp.y - columnIcon.leftColumn.downPoint.transform.position.y);

				cylinderHeight = (dragitemcontroller.chooseObj.transform.position.y - columnIcon.rightColumn.upPoint.transform.position.y);
				cylinderHeight = Mathf.Abs(cylinderHeight);

			}

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

				columnIcon.balustradeIcon.AdjMesh(columnIcon);
			}
			if (isWall)
			{
				columnIcon.wallIcon.rightDownPoint.transform.position = new Vector3(columnIcon.wallIcon.rightDownPoint.transform.position.x, tmp.y, columnIcon.wallIcon.rightDownPoint.transform.position.z);
				columnIcon.wallIcon.leftDownPoint.transform.position = new Vector3(columnIcon.wallIcon.leftDownPoint.transform.position.x, tmp.y, columnIcon.wallIcon.leftDownPoint.transform.position.z);

				windowDown2ButtonDis = columnIcon.wallIcon.rightDownWindowPoint.transform.position.y - columnIcon.wallIcon.rightDownPoint.transform.position.y;

				columnIcon.wallIcon.AdjMesh();
			}
			chang_bodydis.y = -dis;
			ratio_bodydis.y = chang_bodydis.y / ini_bodydis.y;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.body)
		{
			dis = (tmp.x - columnIcon.rightColumn.upPoint.transform.position.x);
			for (int i = 0; i < columnIcon.rightColumn.controlPointList.Count; i++)
			{
				columnIcon.rightColumn.controlPointList[i].transform.position = new Vector3(tmp.x, columnIcon.rightColumn.controlPointList[i].transform.position.y, columnIcon.rightColumn.controlPointList[i].transform.position.z);
				columnIcon.leftColumn.controlPointList[i].transform.position = new Vector3(columnIcon.leftColumn.controlPointList[i].transform.position.x - (dis), columnIcon.leftColumn.controlPointList[i].transform.position.y, columnIcon.leftColumn.controlPointList[i].transform.position.z);
			}
			if (isFrieze)
			{
				columnIcon.friezeIcon.AdjMesh(columnIcon);
			}
			if (isBalustrade)
			{
				columnIcon.balustradeIcon.AdjMesh(columnIcon);
			}
			if (isDoubleRoof)
			{
				columnIcon.doubleRoofIcon.AdjMesh(columnIcon, ini_doubleRoofHeight, ini_doubleRoofWidth);
			}

			chang_bodydis.x = dis;
			ratio_bodydis.x = chang_bodydis.x / ini_bodydis.x;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.leftColumn.body)
		{
			dis = (tmp.x - columnIcon.leftColumn.upPoint.transform.position.x);

			for (int i = 0; i < columnIcon.leftColumn.controlPointList.Count; i++)
			{
				columnIcon.leftColumn.controlPointList[i].transform.position = new Vector3(tmp.x, columnIcon.leftColumn.controlPointList[i].transform.position.y, columnIcon.leftColumn.controlPointList[i].transform.position.z);
				columnIcon.rightColumn.controlPointList[i].transform.position = new Vector3(columnIcon.rightColumn.controlPointList[i].transform.position.x - (dis), columnIcon.rightColumn.controlPointList[i].transform.position.y, columnIcon.rightColumn.controlPointList[i].transform.position.z);

			}
			if (isFrieze)
			{
				columnIcon.friezeIcon.AdjMesh(columnIcon); ;
			}
			if (isBalustrade)
			{
				columnIcon.balustradeIcon.AdjMesh(columnIcon);
			}
			if (isDoubleRoof)
			{
				columnIcon.doubleRoofIcon.AdjMesh(columnIcon, ini_doubleRoofHeight, ini_doubleRoofWidth);
			}
			chang_bodydis.x = -dis;
			ratio_bodydis.x = chang_bodydis.x / ini_bodydis.x;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.friezePoint || dragitemcontroller.chooseObj == columnIcon.leftColumn.friezePoint)
		{//frieze

			columnIcon.leftColumn.friezePoint.transform.position = new Vector3(columnIcon.leftColumn.friezePoint.transform.position.x, tmp.y, columnIcon.leftColumn.friezePoint.transform.position.z);
			columnIcon.rightColumn.friezePoint.transform.position = new Vector3(columnIcon.rightColumn.friezePoint.transform.position.x, tmp.y, columnIcon.rightColumn.friezePoint.transform.position.z);


			friezeHeight = columnIcon.rightColumn.upPoint.transform.position.y - columnIcon.rightColumn.friezePoint.transform.position.y;

			columnIcon.friezeIcon.AdjMesh(columnIcon); ;


		}
		else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.balustradePoint || dragitemcontroller.chooseObj == columnIcon.leftColumn.balustradePoint)
		{ //balustrade

			columnIcon.leftColumn.balustradePoint.transform.position = new Vector3(columnIcon.leftColumn.balustradePoint.transform.position.x, tmp.y, columnIcon.leftColumn.balustradePoint.transform.position.z);
			columnIcon.rightColumn.balustradePoint.transform.position = new Vector3(columnIcon.rightColumn.balustradePoint.transform.position.x, tmp.y, columnIcon.rightColumn.balustradePoint.transform.position.z);

			balustradeHeight = columnIcon.rightColumn.balustradePoint.transform.position.y - columnIcon.rightColumn.downPoint.transform.position.y;
			columnIcon.balustradeIcon.AdjMesh(columnIcon);

		}
		else if (dragitemcontroller.chooseObj == columnIcon.wallIcon.rightUpPoint)
		{
			dis = (tmp.x - columnIcon.wallIcon.rightDownPoint.transform.position.x);

			columnIcon.wallIcon.leftUpPoint.transform.position = new Vector3(columnIcon.wallIcon.leftUpPoint.transform.position.x - (dis), columnIcon.wallIcon.leftUpPoint.transform.position.y, columnIcon.wallIcon.leftUpPoint.transform.position.z);
			columnIcon.wallIcon.leftDownPoint.transform.position = new Vector3(columnIcon.wallIcon.leftDownPoint.transform.position.x - (dis), columnIcon.wallIcon.leftDownPoint.transform.position.y, columnIcon.wallIcon.leftDownPoint.transform.position.z);
			columnIcon.wallIcon.rightDownPoint.transform.position = new Vector3(tmp.x, columnIcon.wallIcon.rightDownPoint.transform.position.y, columnIcon.wallIcon.rightDownPoint.transform.position.z);


			columnIcon.wallIcon.leftUpWindowPoint.transform.position = new Vector3(columnIcon.wallIcon.leftUpWindowPoint.transform.position.x - (dis), columnIcon.wallIcon.leftUpWindowPoint.transform.position.y, columnIcon.wallIcon.leftUpWindowPoint.transform.position.z);
			columnIcon.wallIcon.leftDownWindowPoint.transform.position = new Vector3(columnIcon.wallIcon.leftDownWindowPoint.transform.position.x - (dis), columnIcon.wallIcon.leftDownWindowPoint.transform.position.y, columnIcon.wallIcon.leftDownWindowPoint.transform.position.z);
			columnIcon.wallIcon.rightDownWindowPoint.transform.position = new Vector3(tmp.x, columnIcon.wallIcon.rightDownWindowPoint.transform.position.y, columnIcon.wallIcon.rightDownWindowPoint.transform.position.z);
			columnIcon.wallIcon.rightUpWindowPoint.transform.position = new Vector3(tmp.x, columnIcon.wallIcon.rightUpWindowPoint.transform.position.y, columnIcon.wallIcon.rightUpWindowPoint.transform.position.z);

			columnIcon.wallIcon.AdjMesh();

			chang_walldis = dis;
			ratio_walldis = chang_walldis / ini_wallWidth;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.wallIcon.leftUpPoint)
		{
			dis = (tmp.x - columnIcon.wallIcon.leftDownPoint.transform.position.x);

			columnIcon.wallIcon.rightUpPoint.transform.position = new Vector3(columnIcon.wallIcon.rightUpPoint.transform.position.x + (dis), columnIcon.wallIcon.rightUpPoint.transform.position.y, columnIcon.wallIcon.rightUpPoint.transform.position.z);
			columnIcon.wallIcon.rightDownPoint.transform.position = new Vector3(columnIcon.wallIcon.rightDownPoint.transform.position.x + (dis), columnIcon.wallIcon.rightDownPoint.transform.position.y, columnIcon.wallIcon.rightDownPoint.transform.position.z);
			columnIcon.wallIcon.leftDownPoint.transform.position = new Vector3(tmp.x, columnIcon.wallIcon.leftDownPoint.transform.position.y, columnIcon.wallIcon.leftDownPoint.transform.position.z);

			columnIcon.wallIcon.rightUpWindowPoint.transform.position = new Vector3(columnIcon.wallIcon.rightUpWindowPoint.transform.position.x + (dis), columnIcon.wallIcon.rightUpWindowPoint.transform.position.y, columnIcon.wallIcon.rightUpWindowPoint.transform.position.z);
			columnIcon.wallIcon.rightDownWindowPoint.transform.position = new Vector3(columnIcon.wallIcon.rightDownWindowPoint.transform.position.x + (dis), columnIcon.wallIcon.rightDownWindowPoint.transform.position.y, columnIcon.wallIcon.rightDownWindowPoint.transform.position.z);
			columnIcon.wallIcon.leftUpWindowPoint.transform.position = new Vector3(tmp.x, columnIcon.wallIcon.leftUpWindowPoint.transform.position.y, columnIcon.wallIcon.leftUpWindowPoint.transform.position.z);
			columnIcon.wallIcon.leftDownWindowPoint.transform.position = new Vector3(tmp.x, columnIcon.wallIcon.leftDownWindowPoint.transform.position.y, columnIcon.wallIcon.leftDownWindowPoint.transform.position.z);

			columnIcon.wallIcon.AdjMesh();

			chang_walldis = -dis;
			ratio_walldis = chang_walldis / ini_wallWidth;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.wallIcon.rightDownPoint)
		{
			dis = (tmp.x - columnIcon.wallIcon.rightUpPoint.transform.position.x);

			columnIcon.wallIcon.leftUpPoint.transform.position = new Vector3(columnIcon.wallIcon.leftUpPoint.transform.position.x - (dis), columnIcon.wallIcon.leftUpPoint.transform.position.y, columnIcon.wallIcon.leftUpPoint.transform.position.z);
			columnIcon.wallIcon.leftDownPoint.transform.position = new Vector3(columnIcon.wallIcon.leftDownPoint.transform.position.x - (dis), columnIcon.wallIcon.leftDownPoint.transform.position.y, columnIcon.wallIcon.leftDownPoint.transform.position.z);
			columnIcon.wallIcon.rightUpPoint.transform.position = new Vector3(tmp.x, columnIcon.wallIcon.rightUpPoint.transform.position.y, columnIcon.wallIcon.rightUpPoint.transform.position.z);

			columnIcon.wallIcon.leftUpWindowPoint.transform.position = new Vector3(columnIcon.wallIcon.leftUpWindowPoint.transform.position.x - (dis), columnIcon.wallIcon.leftUpWindowPoint.transform.position.y, columnIcon.wallIcon.leftUpWindowPoint.transform.position.z);
			columnIcon.wallIcon.leftDownWindowPoint.transform.position = new Vector3(columnIcon.wallIcon.leftDownWindowPoint.transform.position.x - (dis), columnIcon.wallIcon.leftDownWindowPoint.transform.position.y, columnIcon.wallIcon.leftDownWindowPoint.transform.position.z);
			columnIcon.wallIcon.rightUpWindowPoint.transform.position = new Vector3(tmp.x, columnIcon.wallIcon.rightUpWindowPoint.transform.position.y, columnIcon.wallIcon.rightUpWindowPoint.transform.position.z);
			columnIcon.wallIcon.rightDownWindowPoint.transform.position = new Vector3(tmp.x, columnIcon.wallIcon.rightDownWindowPoint.transform.position.y, columnIcon.wallIcon.rightDownWindowPoint.transform.position.z);

			columnIcon.wallIcon.AdjMesh();

			chang_walldis = dis;
			ratio_walldis = chang_walldis / ini_wallWidth;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.wallIcon.leftDownPoint)
		{
			dis = (tmp.x - columnIcon.wallIcon.leftUpPoint.transform.position.x);
			columnIcon.wallIcon.rightUpPoint.transform.position = new Vector3(columnIcon.wallIcon.rightUpPoint.transform.position.x + (dis), columnIcon.wallIcon.rightUpPoint.transform.position.y, columnIcon.wallIcon.rightUpPoint.transform.position.z);
			columnIcon.wallIcon.rightDownPoint.transform.position = new Vector3(columnIcon.wallIcon.rightDownPoint.transform.position.x + (dis), columnIcon.wallIcon.rightDownPoint.transform.position.y, columnIcon.wallIcon.rightDownPoint.transform.position.z);
			columnIcon.wallIcon.leftUpPoint.transform.position = new Vector3(tmp.x, columnIcon.wallIcon.leftUpPoint.transform.position.y, columnIcon.wallIcon.leftUpPoint.transform.position.z);

			columnIcon.wallIcon.rightUpWindowPoint.transform.position = new Vector3(columnIcon.wallIcon.rightUpWindowPoint.transform.position.x + (dis), columnIcon.wallIcon.rightUpWindowPoint.transform.position.y, columnIcon.wallIcon.rightUpWindowPoint.transform.position.z);
			columnIcon.wallIcon.rightDownWindowPoint.transform.position = new Vector3(columnIcon.wallIcon.rightDownWindowPoint.transform.position.x + (dis), columnIcon.wallIcon.rightDownWindowPoint.transform.position.y, columnIcon.wallIcon.rightDownWindowPoint.transform.position.z);
			columnIcon.wallIcon.leftUpWindowPoint.transform.position = new Vector3(tmp.x, columnIcon.wallIcon.leftUpWindowPoint.transform.position.y, columnIcon.wallIcon.leftUpWindowPoint.transform.position.z);
			columnIcon.wallIcon.leftDownWindowPoint.transform.position = new Vector3(tmp.x, columnIcon.wallIcon.leftDownWindowPoint.transform.position.y, columnIcon.wallIcon.leftDownWindowPoint.transform.position.z);

			columnIcon.wallIcon.AdjMesh();

			chang_walldis = -dis;
			ratio_walldis = chang_walldis / ini_wallWidth;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.wallIcon.rightUpWindowPoint)
		{
			dis = (tmp.y - columnIcon.wallIcon.leftUpWindowPoint.transform.position.y);
			columnIcon.wallIcon.leftUpWindowPoint.transform.position = new Vector3(columnIcon.wallIcon.leftUpWindowPoint.transform.position.x, columnIcon.wallIcon.leftUpWindowPoint.transform.position.y + (dis), columnIcon.wallIcon.rightUpPoint.transform.position.z);

			windowHeight = columnIcon.wallIcon.rightUpWindowPoint.transform.position.y - columnIcon.wallIcon.rightDownWindowPoint.transform.position.y;

			windowUp2TopDis = columnIcon.wallIcon.rightUpPoint.transform.position.y - columnIcon.wallIcon.rightUpWindowPoint.transform.position.y;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.wallIcon.leftUpWindowPoint)
		{
			dis = (tmp.y - columnIcon.wallIcon.rightUpWindowPoint.transform.position.y);
			columnIcon.wallIcon.rightUpWindowPoint.transform.position = new Vector3(columnIcon.wallIcon.rightUpWindowPoint.transform.position.x, columnIcon.wallIcon.rightUpWindowPoint.transform.position.y + (dis), columnIcon.wallIcon.rightUpWindowPoint.transform.position.z);

			windowHeight = columnIcon.wallIcon.leftUpWindowPoint.transform.position.y - columnIcon.wallIcon.leftDownWindowPoint.transform.position.y;

			windowUp2TopDis = columnIcon.wallIcon.rightUpPoint.transform.position.y - columnIcon.wallIcon.rightUpWindowPoint.transform.position.y;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.wallIcon.rightDownWindowPoint)
		{
			dis = (tmp.y - columnIcon.wallIcon.leftDownWindowPoint.transform.position.y);
			columnIcon.wallIcon.leftDownWindowPoint.transform.position = new Vector3(columnIcon.wallIcon.leftDownWindowPoint.transform.position.x, columnIcon.wallIcon.leftDownWindowPoint.transform.position.y + (dis), columnIcon.wallIcon.leftDownWindowPoint.transform.position.z);

			windowHeight = columnIcon.wallIcon.rightUpWindowPoint.transform.position.y - columnIcon.wallIcon.rightDownWindowPoint.transform.position.y;

			windowDown2ButtonDis = columnIcon.wallIcon.rightDownWindowPoint.transform.position.y - columnIcon.wallIcon.rightDownPoint.transform.position.y;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.wallIcon.leftDownWindowPoint)
		{
			dis = (tmp.y - columnIcon.wallIcon.rightDownWindowPoint.transform.position.y);
			columnIcon.wallIcon.rightDownWindowPoint.transform.position = new Vector3(columnIcon.wallIcon.rightDownWindowPoint.transform.position.x, columnIcon.wallIcon.rightDownWindowPoint.transform.position.y + (dis), columnIcon.wallIcon.rightDownWindowPoint.transform.position.z);

			windowHeight = columnIcon.wallIcon.leftUpWindowPoint.transform.position.y - columnIcon.wallIcon.leftDownWindowPoint.transform.position.y;

			windowDown2ButtonDis = columnIcon.wallIcon.rightDownWindowPoint.transform.position.y - columnIcon.wallIcon.rightDownPoint.transform.position.y;
		}

	}
	public void UpdateFunction(string objName)
	{
		switch (objName)
		{
			case "Frieze":
				if (!isFrieze)
				{
					isFrieze = true;

					columnIcon.CreateFrieze(this, "Frieze_mesh", ini_friezeHeight);


					controlPointList.Add(columnIcon.rightColumn.friezePoint);
					controlPointList.Add(columnIcon.leftColumn.friezePoint);

					movement.verlist.Add(columnIcon.rightColumn.friezePoint);
					movement.verlist.Add(columnIcon.leftColumn.friezePoint);

				}
				break;
			case "Balustrade":
				if (!isBalustrade)
				{
					isBalustrade = true;
					columnIcon.CreateBlustrade(this, "Blustrade_mesh", ini_balustradeHeight);

					controlPointList.Add(columnIcon.rightColumn.balustradePoint);
					controlPointList.Add(columnIcon.leftColumn.balustradePoint);

					movement.verlist.Add(columnIcon.rightColumn.balustradePoint);
					movement.verlist.Add(columnIcon.leftColumn.balustradePoint);

				}
				break;
			case "DoubleRoof":
				if (!isDoubleRoof)
				{
					isDoubleRoof = true;

					columnIcon.CreateDoubleRoof(this, "DoubleRoof_mesh", ini_doubleRoofHeight, ini_doubleRoofWidth);

				}
				break;
			case "Wall":
				if (!isWall)
				{
					isWall = true;

					columnIcon.CreateWall(this, "Wall_mesh", ini_wallWidth, ini_windowHeight);

					movement.horlist.Add(columnIcon.wallIcon.rightDownPoint);
					movement.horlist.Add(columnIcon.wallIcon.rightUpPoint);
					movement.horlist.Add(columnIcon.wallIcon.leftDownPoint);
					movement.horlist.Add(columnIcon.wallIcon.leftUpPoint);

					windowUp2TopDis = columnIcon.wallIcon.rightUpPoint.transform.position.y - columnIcon.wallIcon.rightUpWindowPoint.transform.position.y;
					windowDown2ButtonDis = columnIcon.wallIcon.rightDownWindowPoint.transform.position.y - columnIcon.wallIcon.rightDownPoint.transform.position.y;

				}
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
			movement.horlist.Add(columnIcon.wallIcon.rightDownPoint);
			movement.horlist.Add(columnIcon.wallIcon.rightUpPoint);
			movement.horlist.Add(columnIcon.wallIcon.leftDownPoint);
			movement.horlist.Add(columnIcon.wallIcon.leftUpPoint);

			movement.verlist.Add(columnIcon.wallIcon.rightDownWindowPoint);
			movement.verlist.Add(columnIcon.wallIcon.rightUpWindowPoint);
			movement.verlist.Add(columnIcon.wallIcon.leftDownWindowPoint);
			movement.verlist.Add(columnIcon.wallIcon.leftUpWindowPoint);
		}
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
			if (isWall) minClampX = columnIcon.wallIcon.rightUpPoint.transform.position.x + minWidth;
			else minClampX = columnIcon.leftColumn.upPoint.transform.position.x + minWidth;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.leftColumn.body)
		{
			if (isWall) maxClampX = columnIcon.wallIcon.leftUpPoint.transform.position.x - minWidth;
			else maxClampX = columnIcon.leftColumn.upPoint.transform.position.x - minWidth;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.upPoint)
		{
			if (isWall) minClampY = columnIcon.wallIcon.rightUpWindowPoint.transform.position.y + minCloseHeight;
			else minClampY = columnIcon.rightColumn.downPoint.transform.position.y + minCloseHeight + friezeHeight + balustradeHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.downPoint)
		{
			if (isWall) maxClampY = columnIcon.wallIcon.rightDownWindowPoint.transform.position.y - minCloseHeight;
			else maxClampY = columnIcon.rightColumn.upPoint.transform.position.y - minCloseHeight - friezeHeight - balustradeHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.friezePoint)
		{
			minClampY = columnIcon.rightColumn.downPoint.transform.position.y + minCloseHeight + balustradeHeight;
			maxClampY = columnIcon.rightColumn.upPoint.transform.position.y - minFriezeHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.balustradePoint)
		{
			minClampY = columnIcon.rightColumn.downPoint.transform.position.y + minBalustradeHeight;
			maxClampY = columnIcon.rightColumn.upPoint.transform.position.y - minCloseHeight - friezeHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.leftColumn.upPoint)
		{
			if (isWall) minClampY = columnIcon.wallIcon.leftUpWindowPoint.transform.position.y + minCloseHeight;
			else minClampY = columnIcon.leftColumn.downPoint.transform.position.y + minHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.leftColumn.downPoint)
		{
			if (isWall) maxClampY = columnIcon.wallIcon.leftDownWindowPoint.transform.position.y - minCloseHeight;
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
			maxClampY = columnIcon.leftColumn.upPoint.transform.position.y - minCloseHeight - friezeHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.wallIcon.rightUpPoint)
		{
			maxClampX = columnIcon.rightColumn.upPoint.transform.position.x - minWidth;
			minClampX = columnIcon.wallIcon.leftUpPoint.transform.position.x + minWidth;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.wallIcon.rightDownPoint)
		{
			maxClampX = columnIcon.rightColumn.downPoint.transform.position.x - minWidth;
			minClampX = columnIcon.wallIcon.leftUpPoint.transform.position.x + minWidth;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.wallIcon.leftUpPoint)
		{
			minClampX = columnIcon.leftColumn.upPoint.transform.position.x + minWidth;
			maxClampX = columnIcon.wallIcon.rightUpPoint.transform.position.x - minWidth;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.wallIcon.leftDownPoint)
		{
			minClampX = columnIcon.leftColumn.downPoint.transform.position.x + minWidth;
			maxClampX = columnIcon.wallIcon.rightUpPoint.transform.position.x - minWidth;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.wallIcon.rightUpWindowPoint)
		{
			minClampY = columnIcon.wallIcon.rightDownWindowPoint.transform.position.y + minWallHeight;
			maxClampY = columnIcon.wallIcon.rightUpPoint.transform.position.y - minCloseHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.wallIcon.leftUpWindowPoint)
		{
			minClampY = columnIcon.wallIcon.leftDownWindowPoint.transform.position.y + minWallHeight;
			maxClampY = columnIcon.wallIcon.leftUpPoint.transform.position.y - minCloseHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.wallIcon.rightDownWindowPoint)
		{
			minClampY = columnIcon.wallIcon.rightDownPoint.transform.position.y + minCloseHeight;
			maxClampY = columnIcon.wallIcon.rightUpWindowPoint.transform.position.y - minWallHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.wallIcon.leftDownWindowPoint)
		{
			minClampY = columnIcon.wallIcon.leftDownPoint.transform.position.y + minCloseHeight;
			maxClampY = columnIcon.wallIcon.leftUpWindowPoint.transform.position.y - minWallHeight;
		}
		float posX = Mathf.Clamp(inputPos.x, minClampX, maxClampX);
		float posY = Mathf.Clamp(inputPos.y, minClampY, maxClampY);

		return new Vector3(posX, posY, inputPos.z);
	}
}*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecMeshCreate : IconObject
{
	public List<Vector3> controlPointList_Vec3 = new List<Vector3>();//用於lineRenderer

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
		for (int i = 0; i < controlPointList_Vec3.Count; i++)
		{
			if (i != controlPointList_Vec3.Count - 1)
				CreateLineRenderer(thisGameObject, controlPointList_Vec3[i], controlPointList_Vec3[i + 1]);
			else
				CreateLineRenderer(thisGameObject, controlPointList_Vec3[i], controlPointList_Vec3[0]);
		}
	}
	public override void UpdateLineRender()
	{
		for (int i = 0; i < lineRenderList.Count; i++)
		{
			if (i != controlPointList_Vec3.Count - 1)
				AdjLineRenderer(i, controlPointList_Vec3[i], controlPointList_Vec3[i + 1]);
			else
				AdjLineRenderer(i, controlPointList_Vec3[i], controlPointList_Vec3[0]);
		}
	}
}
public class DoubleRoofIcon : RecMeshCreate
{
	public enum PointIndex { LeftUpPoint = 0, RightUpPoint = 1, RightDownPoint = 2, LeftDownPoint = 3, };
	public Vector3 rightUpPoint;
	public Vector3 rightDownPoint;
	public Vector3 leftUpPoint;
	public Vector3 leftDownPoint;
	public float doubleRoofWidth;
	public float doubleRoofHeight;
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
		controlPointList_Vec3.Add(leftUpPoint);
		controlPointList_Vec3.Add(rightUpPoint);
		controlPointList_Vec3.Add(rightDownPoint);
		controlPointList_Vec3.Add(leftDownPoint);
		base.InitLineRender(thisGameObject);
	}
	public override void UpdateLineRender()
	{
		controlPointList_Vec3[(int)PointIndex.LeftUpPoint] = (leftUpPoint);
		controlPointList_Vec3[(int)PointIndex.RightUpPoint] = (rightUpPoint);
		controlPointList_Vec3[(int)PointIndex.RightDownPoint] = (rightDownPoint);
		controlPointList_Vec3[(int)PointIndex.LeftDownPoint] = (leftDownPoint);
		base.UpdateLineRender();
	}
	public void SetIconObjectColor()
	{
		mRenderer.material.color = Color.red;
	}
}
public class FriezeIcon : RecMeshCreate
{
	public enum PointIndex { LeftUpPoint = 0, RightUpPoint = 1, RightDownPoint = 2, LeftDownPoint = 3, };
	public GameObject rightUpPoint;
	public GameObject rightDownPoint;
	public GameObject leftUpPoint;
	public GameObject leftDownPoint;
	public float friezeHeight;
	public void FriezeIconCreate<T>(T thisGameObject, string objName, ColumnIcon columnIcon, float ini_friezeHeight) where T : Component
	{
		Vector3 h = new Vector3(0.0f, ini_friezeHeight, 0.0f);
		friezeHeight = ini_friezeHeight;
		rightUpPoint = columnIcon.rightColumn.upPoint;
		leftUpPoint = columnIcon.leftColumn.upPoint;
		Vector3 rightDownPointPos = rightUpPoint.transform.position - h;
		Vector3 leftDownPointPos = leftUpPoint.transform.position - h;

		//frieze cp
		columnIcon.rightColumn.friezePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		columnIcon.rightColumn.friezePoint.tag = "ControlPoint";
		columnIcon.rightColumn.friezePoint.name = "FRD";
		columnIcon.rightColumn.friezePoint.transform.localScale = columnIcon.rightColumn.downPoint.transform.localScale;
		columnIcon.rightColumn.friezePoint.transform.position = rightDownPointPos;
		rightDownPoint = columnIcon.rightColumn.friezePoint;

		columnIcon.leftColumn.friezePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		columnIcon.leftColumn.friezePoint.tag = "ControlPoint";
		columnIcon.leftColumn.friezePoint.name = "FLD";
		columnIcon.leftColumn.friezePoint.transform.localScale = columnIcon.rightColumn.downPoint.transform.localScale;
		columnIcon.leftColumn.friezePoint.transform.position = leftDownPointPos;
		leftDownPoint = columnIcon.leftColumn.friezePoint;

		body = new GameObject(objName);

		mFilter = body.AddComponent<MeshFilter>();
		mFilter.mesh = CreatRecMesh(leftUpPoint.transform.position, rightUpPoint.transform.position, rightDownPoint.transform.position, leftDownPoint.transform.position, null);
		//初始位置
		controlPointList.Add(leftUpPoint);
		controlPointList.Add(rightUpPoint);
		controlPointList.Add(rightDownPoint);
		controlPointList.Add(leftDownPoint);
		lastControlPointPosition = mFilter.mesh.vertices;

		columnIcon.leftColumn.controlPointList.Add(columnIcon.leftColumn.friezePoint);
		columnIcon.rightColumn.controlPointList.Add(columnIcon.rightColumn.friezePoint);

		mRenderer = body.AddComponent<MeshRenderer>() as MeshRenderer;

		body.transform.parent = thisGameObject.transform;
		columnIcon.rightColumn.friezePoint.transform.parent = thisGameObject.transform;
		columnIcon.leftColumn.friezePoint.transform.parent = thisGameObject.transform;

		InitLineRender(thisGameObject);
		SetIconObjectColor();
	}
	public void AdjMesh()
	{
		Vector3 h = new Vector3(0.0f, friezeHeight, 0.0f);
		mFilter.mesh.Clear();

		rightDownPoint.transform.position = rightUpPoint.transform.position - h;
		leftDownPoint.transform.position = leftUpPoint.transform.position - h;
		mFilter.mesh = CreatRecMesh(leftUpPoint.transform.position, rightUpPoint.transform.position, rightDownPoint.transform.position, leftDownPoint.transform.position, mFilter.mesh);

		UpdateLineRender();

	}
	public override void InitLineRender<T>(T thisGameObject)
	{
		controlPointList_Vec3.Add(leftUpPoint.transform.position);
		controlPointList_Vec3.Add(rightUpPoint.transform.position);
		controlPointList_Vec3.Add(rightDownPoint.transform.position);
		controlPointList_Vec3.Add(leftDownPoint.transform.position);
		base.InitLineRender(thisGameObject);
	}
	public override void UpdateLineRender()
	{
		controlPointList_Vec3[(int)PointIndex.LeftUpPoint] = (leftUpPoint.transform.position);
		controlPointList_Vec3[(int)PointIndex.RightUpPoint] = (rightUpPoint.transform.position);
		controlPointList_Vec3[(int)PointIndex.RightDownPoint] = (rightDownPoint.transform.position);
		controlPointList_Vec3[(int)PointIndex.LeftDownPoint] = (leftDownPoint.transform.position);
		base.UpdateLineRender();
	}
	public void SetIconObjectColor()
	{
		mRenderer.material.color = Color.red;
		rightDownPoint.GetComponent<MeshRenderer>().material = outLineShader;
		rightDownPoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
		leftDownPoint.GetComponent<MeshRenderer>().material = outLineShader;
		leftDownPoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
	}
	public void AdjPos(Vector3 tmp)
	{
		leftDownPoint.transform.position = new Vector3(leftDownPoint.transform.position.x, tmp.y, leftDownPoint.transform.position.z);
		rightDownPoint.transform.position = new Vector3(rightDownPoint.transform.position.x, tmp.y, rightDownPoint.transform.position.z);

		friezeHeight = rightUpPoint.transform.position.y - rightDownPoint.transform.position.y;
		UpdateLastPos();
	}
}
public class WallIcon : RecMeshCreate
{
	public enum PointIndex { LeftUpPoint = 0, RightUpPoint = 1, RightDownPoint = 2, LeftDownPoint = 3, LeftUpWindowPoint = 4, RightUpWindowPoint = 5, RightDownWindowPoint = 6, LeftDownWindowPoint = 7, };
	public GameObject rightUpPoint;
	public GameObject rightDownPoint;
	public GameObject leftUpPoint;
	public GameObject leftDownPoint;

	public GameObject rightUpWindowPoint;
	public GameObject rightDownWindowPoint;
	public GameObject leftUpWindowPoint;
	public GameObject leftDownWindowPoint;

	public float wallWidth;
	public float windowHeight;

	public void WallIconCreate<T>(T thisGameObject, string objName, ColumnIcon columnIcon, float ini_wallWidth, float ini_windowsHeight) where T : Component
	{
		Vector3 wallOffset = new Vector3(Mathf.Abs(columnIcon.rightColumn.upPoint.transform.transform.position.x - columnIcon.leftColumn.upPoint.transform.transform.position.x) / 2.0f - ini_wallWidth, 0, 0);
		float columnHeight = columnIcon.rightColumn.upPoint.transform.transform.position.y - columnIcon.rightColumn.downPoint.transform.transform.position.y;
		Vector3 windowsOffset = new Vector3(0, columnHeight / 2.0f - ini_windowsHeight / 2.0f, 0);

		wallWidth=ini_wallWidth;
		windowHeight=ini_windowsHeight;
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
		controlPointList.Add(leftUpPoint);
		controlPointList.Add(rightUpPoint);
		controlPointList.Add(rightDownPoint);
		controlPointList.Add(leftDownPoint);
		controlPointList.Add(leftUpWindowPoint);
		controlPointList.Add(rightUpWindowPoint);
		controlPointList.Add(rightDownWindowPoint);
		controlPointList.Add(leftDownWindowPoint);
		lastControlPointPosition = new Vector3[controlPointList.Count];
		lastControlPointPosition[(int)PointIndex.LeftUpPoint] = leftUpPoint.transform.position;
		lastControlPointPosition[(int)PointIndex.RightUpPoint] = rightUpPoint.transform.position;
		lastControlPointPosition[(int)PointIndex.RightDownPoint] = rightDownPoint.transform.position;
		lastControlPointPosition[(int)PointIndex.LeftDownPoint] = leftDownPoint.transform.position;
		lastControlPointPosition[(int)PointIndex.LeftUpWindowPoint] = leftUpWindowPoint.transform.position;
		lastControlPointPosition[(int)PointIndex.RightUpWindowPoint] = rightUpPoint.transform.position;
		lastControlPointPosition[(int)PointIndex.RightDownWindowPoint] =rightDownWindowPoint.transform.position;
		lastControlPointPosition[(int)PointIndex.LeftDownWindowPoint] = leftDownWindowPoint.transform.position;


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
		controlPointList_Vec3.Add(leftUpPoint.transform.position);
		controlPointList_Vec3.Add(rightUpPoint.transform.position);
		controlPointList_Vec3.Add(rightDownPoint.transform.position);
		controlPointList_Vec3.Add(leftDownPoint.transform.position);
		base.InitLineRender(thisGameObject);
	}
	public override void UpdateLineRender()
	{
		controlPointList_Vec3[(int)PointIndex.LeftUpPoint] = (leftUpPoint.transform.position);
		controlPointList_Vec3[(int)PointIndex.RightUpPoint] = (rightUpPoint.transform.position);
		controlPointList_Vec3[(int)PointIndex.RightDownPoint] = (rightDownPoint.transform.position);
		controlPointList_Vec3[(int)PointIndex.LeftDownPoint] = (leftDownPoint.transform.position);
		base.UpdateLineRender();
	}
	public void SetIconObjectColor()
	{
		mRenderer.material.color = Color.red;
		leftUpPoint.GetComponent<MeshRenderer>().material.color = Color.white;
		rightUpPoint.GetComponent<MeshRenderer>().material.color = Color.white;
		rightDownPoint.GetComponent<MeshRenderer>().material.color = Color.white;
		leftDownPoint.GetComponent<MeshRenderer>().material.color = Color.white;

		leftUpWindowPoint.GetComponent<MeshRenderer>().material.color = Color.blue;
		rightUpWindowPoint.GetComponent<MeshRenderer>().material.color = Color.blue;
		rightDownWindowPoint.GetComponent<MeshRenderer>().material.color = Color.blue;
		leftDownWindowPoint.GetComponent<MeshRenderer>().material.color = Color.blue;
	}
	public void AdjPos(Vector3 tmp, GameObject chooseGameObject)
	{
		float OffsetX = 0;
		float OffsetY = 0;
		int index=0;
		for(int i=0;i<controlPointList.Count;i++)
		{
			if (chooseGameObject == controlPointList[i])
			{
				index=i;
				break;
			}
		}
		switch (index)
		{
			case (int)PointIndex.LeftUpPoint:
				OffsetX = (tmp.x - lastControlPointPosition[(int)PointIndex.LeftUpPoint].x);
				rightUpPoint.transform.position = new Vector3(rightUpPoint.transform.position.x + (OffsetX), rightUpPoint.transform.position.y, rightUpPoint.transform.position.z);
				rightDownPoint.transform.position = new Vector3(rightDownPoint.transform.position.x + (OffsetX), rightDownPoint.transform.position.y, rightDownPoint.transform.position.z);
				leftDownPoint.transform.position = new Vector3(tmp.x, leftDownPoint.transform.position.y, leftDownPoint.transform.position.z);

				rightUpWindowPoint.transform.position = new Vector3(rightUpWindowPoint.transform.position.x + (OffsetX), rightUpWindowPoint.transform.position.y, rightUpWindowPoint.transform.position.z);
				rightDownWindowPoint.transform.position = new Vector3(rightDownWindowPoint.transform.position.x + (OffsetX), rightDownWindowPoint.transform.position.y, rightDownWindowPoint.transform.position.z);
				leftUpWindowPoint.transform.position = new Vector3(tmp.x, leftUpWindowPoint.transform.position.y, leftUpWindowPoint.transform.position.z);
				leftDownWindowPoint.transform.position = new Vector3(tmp.x, leftDownWindowPoint.transform.position.y, leftDownWindowPoint.transform.position.z);
				wallWidth = (rightUpPoint.transform.position.x - leftUpPoint.transform.position.x)/2.0f;
				break;
			case (int)PointIndex.RightUpPoint:
				OffsetX = (tmp.x - lastControlPointPosition[(int)PointIndex.RightUpPoint].x);

				leftUpPoint.transform.position = new Vector3(leftUpPoint.transform.position.x - (OffsetX), leftUpPoint.transform.position.y, leftUpPoint.transform.position.z);
				leftDownPoint.transform.position = new Vector3(leftDownPoint.transform.position.x - (OffsetX), leftDownPoint.transform.position.y, leftDownPoint.transform.position.z);
				rightDownPoint.transform.position = new Vector3(tmp.x, rightDownPoint.transform.position.y, rightDownPoint.transform.position.z);


				leftUpWindowPoint.transform.position = new Vector3(leftUpWindowPoint.transform.position.x - (OffsetX), leftUpWindowPoint.transform.position.y, leftUpWindowPoint.transform.position.z);
				leftDownWindowPoint.transform.position = new Vector3(leftDownWindowPoint.transform.position.x - (OffsetX), leftDownWindowPoint.transform.position.y, leftDownWindowPoint.transform.position.z);
				rightDownWindowPoint.transform.position = new Vector3(tmp.x, rightDownWindowPoint.transform.position.y, rightDownWindowPoint.transform.position.z);
				rightUpWindowPoint.transform.position = new Vector3(tmp.x, rightUpWindowPoint.transform.position.y, rightUpWindowPoint.transform.position.z);
				wallWidth = (rightUpPoint.transform.position.x - leftUpPoint.transform.position.x) / 2.0f;
				break;
			case (int)PointIndex.RightDownPoint:
				OffsetX = (tmp.x - lastControlPointPosition[(int)PointIndex.RightDownPoint].x);

				leftUpPoint.transform.position = new Vector3(leftUpPoint.transform.position.x - (OffsetX), leftUpPoint.transform.position.y, leftUpPoint.transform.position.z);
				leftDownPoint.transform.position = new Vector3(leftDownPoint.transform.position.x - (OffsetX), leftDownPoint.transform.position.y, leftDownPoint.transform.position.z);
				rightUpPoint.transform.position = new Vector3(tmp.x, rightUpPoint.transform.position.y, rightUpPoint.transform.position.z);

				leftUpWindowPoint.transform.position = new Vector3(leftUpWindowPoint.transform.position.x - (OffsetX), leftUpWindowPoint.transform.position.y, leftUpWindowPoint.transform.position.z);
				leftDownWindowPoint.transform.position = new Vector3(leftDownWindowPoint.transform.position.x - (OffsetX), leftDownWindowPoint.transform.position.y, leftDownWindowPoint.transform.position.z);
				rightUpWindowPoint.transform.position = new Vector3(tmp.x, rightUpWindowPoint.transform.position.y, rightUpWindowPoint.transform.position.z);
				rightDownWindowPoint.transform.position = new Vector3(tmp.x, rightDownWindowPoint.transform.position.y, rightDownWindowPoint.transform.position.z);
				wallWidth = (rightUpPoint.transform.position.x - leftUpPoint.transform.position.x) / 2.0f;
				break;
			case (int)PointIndex.LeftDownPoint:
				OffsetX = (tmp.x - lastControlPointPosition[(int)PointIndex.LeftDownPoint].x);
				rightUpPoint.transform.position = new Vector3(rightUpPoint.transform.position.x + (OffsetX), rightUpPoint.transform.position.y, rightUpPoint.transform.position.z);
				rightDownPoint.transform.position = new Vector3(rightDownPoint.transform.position.x + (OffsetX), rightDownPoint.transform.position.y, rightDownPoint.transform.position.z);
				leftUpPoint.transform.position = new Vector3(tmp.x, leftUpPoint.transform.position.y, leftUpPoint.transform.position.z);

				rightUpWindowPoint.transform.position = new Vector3(rightUpWindowPoint.transform.position.x + (OffsetX), rightUpWindowPoint.transform.position.y, rightUpWindowPoint.transform.position.z);
				rightDownWindowPoint.transform.position = new Vector3(rightDownWindowPoint.transform.position.x + (OffsetX), rightDownWindowPoint.transform.position.y, rightDownWindowPoint.transform.position.z);
				leftUpWindowPoint.transform.position = new Vector3(tmp.x, leftUpWindowPoint.transform.position.y, leftUpWindowPoint.transform.position.z);
				leftDownWindowPoint.transform.position = new Vector3(tmp.x, leftDownWindowPoint.transform.position.y, leftDownWindowPoint.transform.position.z);
				wallWidth = (rightUpPoint.transform.position.x - leftUpPoint.transform.position.x) / 2.0f;
				break;
			case (int)PointIndex.LeftUpWindowPoint:
				OffsetY = (tmp.y - lastControlPointPosition[(int)PointIndex.LeftUpWindowPoint].y);
				rightUpWindowPoint.transform.position = new Vector3(rightUpWindowPoint.transform.position.x, rightUpWindowPoint.transform.position.y + (OffsetY), rightUpWindowPoint.transform.position.z);
				windowHeight = (rightUpPoint.transform.position.x - leftUpPoint.transform.position.x) / 2.0f;
				break;
			case (int)PointIndex.RightUpWindowPoint:
				OffsetY = (tmp.y - lastControlPointPosition[(int)PointIndex.RightUpWindowPoint].y);
				leftUpWindowPoint.transform.position = new Vector3(leftUpWindowPoint.transform.position.x, leftUpWindowPoint.transform.position.y + (OffsetY), rightUpPoint.transform.position.z);
				windowHeight = (rightUpPoint.transform.position.x - leftUpPoint.transform.position.x) / 2.0f;
				break;
			case (int)PointIndex.RightDownWindowPoint:
				OffsetY = (tmp.y - lastControlPointPosition[(int)PointIndex.RightDownWindowPoint].y);
				leftDownWindowPoint.transform.position = new Vector3(leftDownWindowPoint.transform.position.x, leftDownWindowPoint.transform.position.y + (OffsetY), leftDownWindowPoint.transform.position.z);
				windowHeight = (rightUpPoint.transform.position.x - leftUpPoint.transform.position.x) / 2.0f;
				break;
			case (int)PointIndex.LeftDownWindowPoint:
				OffsetY = (tmp.y - lastControlPointPosition[(int)PointIndex.LeftDownWindowPoint].y);
				rightDownWindowPoint.transform.position = new Vector3(rightDownWindowPoint.transform.position.x, rightDownWindowPoint.transform.position.y + (OffsetY), rightDownWindowPoint.transform.position.z);
				windowHeight = (rightUpPoint.transform.position.x - leftUpPoint.transform.position.x) / 2.0f;
				break;
		}
		UpdateLastPos();
	}
}
public class BalustradeIcon : RecMeshCreate
{
	public enum PointIndex { LeftUpPoint = 0, RightUpPoint = 1, RightDownPoint = 2, LeftDownPoint = 3, };
	public GameObject rightUpPoint;
	public GameObject rightDownPoint;
	public GameObject leftUpPoint;
	public GameObject leftDownPoint;
	public float balustradeHeight;
	public void BalustradeIconCreate<T>(T thisGameObject, string objName, ColumnIcon columnIcon, float ini_balustradeHeight) where T : Component
	{
		Vector3 h = new Vector3(0.0f, ini_balustradeHeight, 0.0f);
		balustradeHeight = ini_balustradeHeight;
		rightDownPoint = columnIcon.rightColumn.downPoint;
		leftDownPoint = columnIcon.leftColumn.downPoint;
		Vector3 rightUpPointPos = rightDownPoint.transform.position + h;
		Vector3 leftUpPointPos = leftDownPoint.transform.position + h;

		//right
		columnIcon.rightColumn.balustradePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		columnIcon.rightColumn.balustradePoint.tag = "ControlPoint";
		columnIcon.rightColumn.balustradePoint.name = "BRU";
		columnIcon.rightColumn.balustradePoint.transform.localScale = columnIcon.rightColumn.downPoint.transform.localScale;
		columnIcon.rightColumn.balustradePoint.transform.position = rightUpPointPos;
		rightUpPoint = columnIcon.rightColumn.balustradePoint;
		//left
		columnIcon.leftColumn.balustradePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		columnIcon.leftColumn.balustradePoint.tag = "ControlPoint";
		columnIcon.leftColumn.balustradePoint.name = "BLU";
		columnIcon.leftColumn.balustradePoint.transform.localScale = columnIcon.rightColumn.downPoint.transform.localScale;
		columnIcon.leftColumn.balustradePoint.transform.position = leftUpPointPos;
		leftUpPoint = columnIcon.leftColumn.balustradePoint;
		//body
		body = new GameObject(objName);

		mFilter = body.AddComponent<MeshFilter>();
		mFilter.mesh = CreatRecMesh(leftUpPoint.transform.position, rightUpPoint.transform.position, rightDownPoint.transform.position, leftDownPoint.transform.position, null);
		//初始位置
		controlPointList.Add(leftUpPoint);
		controlPointList.Add(rightUpPoint);
		controlPointList.Add(rightDownPoint);
		controlPointList.Add(leftDownPoint);
		lastControlPointPosition = mFilter.mesh.vertices;

		columnIcon.leftColumn.controlPointList.Add(columnIcon.leftColumn.balustradePoint);
		columnIcon.rightColumn.controlPointList.Add(columnIcon.rightColumn.balustradePoint);

		mRenderer = body.AddComponent<MeshRenderer>() as MeshRenderer;

		body.transform.parent = thisGameObject.transform;
		columnIcon.rightColumn.balustradePoint.transform.parent = thisGameObject.transform;
		columnIcon.leftColumn.balustradePoint.transform.parent = thisGameObject.transform;

		InitLineRender(thisGameObject);
		SetIconObjectColor(columnIcon);
	}
	public void AdjMesh()
	{
		Vector3 h = new Vector3(0.0f, balustradeHeight, 0.0f);
		mFilter.mesh.Clear();
		rightUpPoint.transform.position = rightDownPoint.transform.position + h;
		leftUpPoint.transform.position = leftDownPoint.transform.position + h;
		mFilter.mesh = CreatRecMesh(leftUpPoint.transform.position, rightUpPoint.transform.position, rightDownPoint.transform.position, leftDownPoint.transform.position, mFilter.mesh);

		UpdateLineRender();
	}
	public override void InitLineRender<T>(T thisGameObject)
	{
		controlPointList_Vec3.Add(leftUpPoint.transform.position);
		controlPointList_Vec3.Add(rightUpPoint.transform.position);
		controlPointList_Vec3.Add(rightDownPoint.transform.position);
		controlPointList_Vec3.Add(leftDownPoint.transform.position);
		base.InitLineRender(thisGameObject);
	}
	public override void UpdateLineRender()
	{
		controlPointList_Vec3[(int)PointIndex.LeftUpPoint] = (leftUpPoint.transform.position);
		controlPointList_Vec3[(int)PointIndex.RightUpPoint] = (rightUpPoint.transform.position);
		controlPointList_Vec3[(int)PointIndex.RightDownPoint] = (rightDownPoint.transform.position);
		controlPointList_Vec3[(int)PointIndex.LeftDownPoint] = (leftDownPoint.transform.position);
		base.UpdateLineRender();
	}
	public void SetIconObjectColor(ColumnIcon columnIcon)
	{
		mRenderer.material.color = Color.red;
		columnIcon.rightColumn.balustradePoint.GetComponent<MeshRenderer>().material = outLineShader;
		columnIcon.rightColumn.balustradePoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
		columnIcon.leftColumn.balustradePoint.GetComponent<MeshRenderer>().material = outLineShader;
		columnIcon.leftColumn.balustradePoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
	}
	public void AdjPos(Vector3 tmp)
	{
		leftUpPoint.transform.position = new Vector3(leftUpPoint.transform.position.x, tmp.y, leftUpPoint.transform.position.z);
		rightUpPoint.transform.position = new Vector3(rightUpPoint.transform.position.x, tmp.y, rightUpPoint.transform.position.z);

		balustradeHeight = rightUpPoint.transform.position.y - rightDownPoint.transform.position.y;

		UpdateLastPos();
		UpdateLineRender();
	}
}
public class ColumnIcon : IconObject
{
	public enum PointIndex { LeftUpPoint = 0, RightUpPoint = 1, RightDownPoint = 2, LeftDownPoint = 3, LeftColumnBody = 4, RightColumnBody = 5, };

	public Column leftColumn;
	public Column rightColumn;

	public DoubleRoofIcon doubleRoofIcon = new DoubleRoofIcon();

	public FriezeIcon friezeIcon = new FriezeIcon();
	public BalustradeIcon balustradeIcon = new BalustradeIcon();

	public WallIcon wallIcon = new WallIcon();

	public float columnHeight;

	public void ColumnIconCreate<T>(T thisGameObject, GameObject rightUpPoint, GameObject rightDownPoint, GameObject leftUpPoint, GameObject leftDownPoint, float columnHeight) where T : Component
	{
		leftColumn = new Column(leftUpPoint, leftDownPoint, columnHeight);
		rightColumn = new Column(rightUpPoint, rightDownPoint, columnHeight);

		this.columnHeight=columnHeight;

		leftColumn.body.transform.parent = thisGameObject.transform;
		rightColumn.body.transform.parent = thisGameObject.transform;

		controlPointList.Add(leftColumn.upPoint);
		controlPointList.Add(rightColumn.upPoint);
		controlPointList.Add(rightColumn.downPoint);
		controlPointList.Add(leftColumn.downPoint);
		lastControlPointPosition = new Vector3[controlPointList.Count];
		lastControlPointPosition[(int)PointIndex.LeftUpPoint] = leftColumn.upPoint.transform.position;
		lastControlPointPosition[(int)PointIndex.RightUpPoint] = rightColumn.upPoint.transform.position;
		lastControlPointPosition[(int)PointIndex.RightDownPoint] = rightColumn.downPoint.transform.position;
		lastControlPointPosition[(int)PointIndex.LeftDownPoint] = leftColumn.downPoint.transform.position;
	}
	public void CreateWall<T>(T thisGameObject, string objName, float ini_wallWidth, float ini_windowHeight) where T : Component
	{
		wallIcon.WallIconCreate(thisGameObject, objName, this, ini_wallWidth, ini_windowHeight);
	}
	public void CreateDoubleRoof<T>(T thisGameObject, string objName, float ini_doubleRoofHeight, float ini_doubleRoofWidth) where T : Component
	{
		doubleRoofIcon.DoubleRoofIconCreate(thisGameObject, objName, this, ini_doubleRoofHeight, ini_doubleRoofWidth);
	}
	public void CreateBlustrade<T>(T thisGameObject, string objName, float ini_balustradeHeight) where T : Component
	{
		balustradeIcon.BalustradeIconCreate(thisGameObject, objName, this, ini_balustradeHeight);
	}
	public void CreateFrieze<T>(T thisGameObject, string objName, float ini_friezeHeight) where T : Component
	{
		friezeIcon.FriezeIconCreate(thisGameObject, objName, this, ini_friezeHeight);
	}
	public void AdjPos(Vector3 tmp, GameObject chooseObject)
	{
		float OffsetX = 0;
		int index=0;
		for(int i=0;i<controlPointList.Count;i++)
		{
			if (chooseObject == controlPointList[i])
			{
				index=i;
				break;
			}
		}
		switch (index)
		{
			case (int)PointIndex.LeftUpPoint:
			case (int)PointIndex.RightUpPoint:
				columnHeight = (tmp.y - rightColumn.downPoint.transform.position.y);
				columnHeight = Mathf.Abs(columnHeight);
				//update point
				rightColumn.upPoint.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, tmp.y, rightColumn.upPoint.transform.position.z);
				leftColumn.upPoint.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, tmp.y, leftColumn.upPoint.transform.position.z);

				rightColumn.body.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, rightColumn.upPoint.transform.position.y - columnHeight / 2.0f, rightColumn.upPoint.transform.position.z);

				leftColumn.body.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, leftColumn.upPoint.transform.position.y - columnHeight / 2.0f, leftColumn.upPoint.transform.position.z);

				rightColumn.body.transform.localScale = new Vector3(rightColumn.radius, columnHeight / 2.0f, rightColumn.radius);
				leftColumn.body.transform.localScale = new Vector3(leftColumn.radius, columnHeight / 2.0f, leftColumn.radius);


				if (friezeIcon.body != null)
				{
					rightColumn.friezePoint.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, tmp.y - friezeIcon.friezeHeight, rightColumn.upPoint.transform.position.z);
					leftColumn.friezePoint.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, tmp.y - friezeIcon.friezeHeight, leftColumn.upPoint.transform.position.z);

					friezeIcon.AdjMesh();
				}
				if (doubleRoofIcon.body != null)
				{
					doubleRoofIcon.AdjMesh(this, doubleRoofIcon.doubleRoofHeight, doubleRoofIcon.doubleRoofWidth);
				}
				if (wallIcon.body != null)
				{
					wallIcon.rightUpPoint.transform.position = new Vector3(wallIcon.rightUpPoint.transform.position.x, tmp.y, wallIcon.rightUpPoint.transform.position.z);
					wallIcon.leftUpPoint.transform.position = new Vector3(wallIcon.leftUpPoint.transform.position.x, tmp.y, wallIcon.leftUpPoint.transform.position.z);


					wallIcon.AdjMesh();
				}
				break;
			case (int)PointIndex.RightDownPoint:
			case (int)PointIndex.LeftDownPoint:
				columnHeight = (tmp.y - rightColumn.upPoint.transform.position.y);
				columnHeight=Mathf.Abs(columnHeight);
				//update point
				rightColumn.downPoint.transform.position = new Vector3(rightColumn.downPoint.transform.position.x, tmp.y, rightColumn.downPoint.transform.position.z);
				leftColumn.downPoint.transform.position = new Vector3(leftColumn.downPoint.transform.position.x, tmp.y, rightColumn.downPoint.transform.position.z);
				rightColumn.body.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, rightColumn.upPoint.transform.position.y - columnHeight / 2.0f, rightColumn.upPoint.transform.position.z);
				leftColumn.body.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, leftColumn.upPoint.transform.position.y - columnHeight / 2.0f, leftColumn.upPoint.transform.position.z);


				rightColumn.body.transform.localScale = new Vector3(rightColumn.radius, columnHeight / 2.0f, rightColumn.radius);
				leftColumn.body.transform.localScale = new Vector3(leftColumn.radius, columnHeight / 2.0f, leftColumn.radius);


				if (balustradeIcon.body != null)
				{
					rightColumn.balustradePoint.transform.position = new Vector3(rightColumn.downPoint.transform.position.x, tmp.y + balustradeIcon.balustradeHeight, rightColumn.upPoint.transform.position.z);
					leftColumn.balustradePoint.transform.position = new Vector3(leftColumn.downPoint.transform.position.x, tmp.y + balustradeIcon.balustradeHeight, leftColumn.upPoint.transform.position.z);

					balustradeIcon.AdjMesh();
				}
				if (wallIcon.body != null)
				{
					wallIcon.rightDownPoint.transform.position = new Vector3(wallIcon.rightDownPoint.transform.position.x, tmp.y, wallIcon.rightDownPoint.transform.position.z);
					wallIcon.leftDownPoint.transform.position = new Vector3(wallIcon.leftDownPoint.transform.position.x, tmp.y, wallIcon.leftDownPoint.transform.position.z);

					wallIcon.AdjMesh();
				}
				break;
			case (int)PointIndex.LeftColumnBody:
				OffsetX = (tmp.x - lastControlPointPosition[(int)PointIndex.LeftUpPoint].x);
				for (int i = 0; i < rightColumn.controlPointList.Count; i++)
				{
					leftColumn.controlPointList[i].transform.position = new Vector3(tmp.x, leftColumn.controlPointList[i].transform.position.y, leftColumn.controlPointList[i].transform.position.z);
					rightColumn.controlPointList[i].transform.position = new Vector3(rightColumn.controlPointList[i].transform.position.x - (OffsetX), rightColumn.controlPointList[i].transform.position.y, rightColumn.controlPointList[i].transform.position.z);
				}
				if (friezeIcon.body != null)
				{
					friezeIcon.AdjMesh();
					friezeIcon.UpdateLastPos();
				}
				if (balustradeIcon.body != null)
				{
					balustradeIcon.AdjMesh();
					balustradeIcon.UpdateLastPos();
				}
				if (doubleRoofIcon.body != null)
				{
					doubleRoofIcon.AdjMesh(this, doubleRoofIcon.doubleRoofHeight, doubleRoofIcon.doubleRoofWidth);
				}
				break;
			case (int)PointIndex.RightColumnBody:
				OffsetX = (tmp.x - lastControlPointPosition[(int)PointIndex.RightUpPoint].x);
				for (int i = 0; i < leftColumn.controlPointList.Count; i++)
				{
					rightColumn.controlPointList[i].transform.position = new Vector3(tmp.x, rightColumn.controlPointList[i].transform.position.y, rightColumn.controlPointList[i].transform.position.z);
					leftColumn.controlPointList[i].transform.position = new Vector3(leftColumn.controlPointList[i].transform.position.x - (OffsetX), leftColumn.controlPointList[i].transform.position.y, leftColumn.controlPointList[i].transform.position.z);

				}
				if (friezeIcon.body != null)
				{
					friezeIcon.AdjMesh();
					friezeIcon.UpdateLastPos();
				}
				if (balustradeIcon.body != null)
				{
					balustradeIcon.AdjMesh();
					balustradeIcon.UpdateLastPos();
				}
				if (doubleRoofIcon.body != null)
				{
					doubleRoofIcon.AdjMesh(this, doubleRoofIcon.doubleRoofHeight, doubleRoofIcon.doubleRoofWidth);
				}
				break;

		}
		UpdateLastPos();
	}
}
public class Column : IconObject
{
	public enum PointIndex { UpPoint = 0, DownPoint = 1, Body = 2 };
	public GameObject upPoint = null;
	public GameObject downPoint = null;
	public GameObject balustradePoint = null;
	public GameObject friezePoint = null;
	public float radius = 0.01f;
	public Column(GameObject upPoint, GameObject downPoint, float columnHeight)
	{
		this.upPoint = upPoint;
		this.downPoint = downPoint;

		this.body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		mFilter = body.GetComponent<MeshFilter>();
		mRenderer = body.GetComponent<MeshRenderer>();
		mRenderer.sortingOrder = 0;

		controlPointList.Add(upPoint);
		controlPointList.Add(downPoint);
		controlPointList.Add(body);
		lastControlPointPosition = new Vector3[controlPointList.Count];
		lastControlPointPosition[(int)PointIndex.UpPoint] = upPoint.transform.position;
		lastControlPointPosition[(int)PointIndex.DownPoint] = downPoint.transform.position;

		this.body.transform.localScale = new Vector3(radius, columnHeight / 2.0f, radius);
		this.body.transform.position = new Vector3(upPoint.transform.position.x, upPoint.transform.position.y - columnHeight / 2.0f, upPoint.transform.position.z);

		this.body.tag = "Cylinder";

		lastControlPointPosition[(int)PointIndex.Body] = this.body.transform.position;

		SetIconObjectColor();
	}
	public void SetIconObjectColor()
	{

		mRenderer.material = outLineShader;
		mRenderer.material.color = Color.red;
		upPoint.GetComponent<MeshRenderer>().material = outLineShader;
		upPoint.GetComponent<MeshRenderer>().material.color = Color.yellow;
		downPoint.GetComponent<MeshRenderer>().material = outLineShader;
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
		columnIcon.ColumnIconCreate(this, controlPointList[2], controlPointList[3], controlPointList[0], controlPointList[1], ini_bodydis.y);


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
		ratio_bodydis = chang_bodydis = Vector2.zero;
		ratio_walldis = chang_walldis = 0;
		Vector3 tmp = dragitemcontroller.chooseObj.transform.position;
		GameObject chooseObj=dragitemcontroller.chooseObj;
		float dis = 0;
		if (chooseObj == columnIcon.rightColumn.upPoint || chooseObj == columnIcon.leftColumn.upPoint)//RU LU
		{

			if (chooseObj == columnIcon.leftColumn.upPoint)
			{
				dis = (tmp.y - columnIcon.rightColumn.upPoint.transform.position.y);
				columnIcon.AdjPos(tmp, chooseObj);
				cylinderHeight = columnIcon.columnHeight;
			}
			else if (chooseObj == columnIcon.rightColumn.upPoint)
			{
				dis = (tmp.y - columnIcon.leftColumn.upPoint.transform.position.y);
				columnIcon.AdjPos(tmp, chooseObj);

				cylinderHeight = columnIcon.columnHeight;
			}

			chang_bodydis.y = dis;
			ratio_bodydis.y = chang_bodydis.y / ini_bodydis.y;
		}
		else if (chooseObj == columnIcon.rightColumn.downPoint || chooseObj == columnIcon.leftColumn.downPoint)//RD  LD
		{
			if (chooseObj == columnIcon.leftColumn.downPoint)
			{
				dis = (tmp.y - columnIcon.rightColumn.downPoint.transform.position.y);

				columnIcon.AdjPos(tmp, chooseObj);
				cylinderHeight = columnIcon.columnHeight;

			}
			else if (chooseObj == columnIcon.rightColumn.downPoint)
			{
				dis = (tmp.y - columnIcon.leftColumn.downPoint.transform.position.y);

				columnIcon.AdjPos(tmp, chooseObj);
				cylinderHeight = columnIcon.columnHeight;

			}
			chang_bodydis.y = -dis;
			ratio_bodydis.y = chang_bodydis.y / ini_bodydis.y;
		}
		else if (chooseObj == columnIcon.rightColumn.body)
		{
			dis = (tmp.x - columnIcon.rightColumn.upPoint.transform.position.x);

			columnIcon.AdjPos(tmp, chooseObj);

			chang_bodydis.x = dis;
			ratio_bodydis.x = chang_bodydis.x / ini_bodydis.x;
		}
		else if (chooseObj == columnIcon.leftColumn.body)
		{
			dis = (tmp.x - columnIcon.leftColumn.upPoint.transform.position.x);

			columnIcon.AdjPos(tmp, chooseObj);
			chang_bodydis.x = -dis;
			ratio_bodydis.x = chang_bodydis.x / ini_bodydis.x;
		}
		else if (chooseObj == columnIcon.rightColumn.friezePoint || chooseObj == columnIcon.leftColumn.friezePoint)
		{//frieze

			columnIcon.friezeIcon.AdjPos(tmp);


			friezeHeight = columnIcon.friezeIcon.friezeHeight;

			columnIcon.friezeIcon.AdjMesh(); ;


		}
		else if (chooseObj == columnIcon.rightColumn.balustradePoint || chooseObj == columnIcon.leftColumn.balustradePoint)
		{ //balustrade

			columnIcon.balustradeIcon.AdjPos(tmp);

			balustradeHeight = columnIcon.balustradeIcon.balustradeHeight;
			columnIcon.balustradeIcon.AdjMesh();

		}
		else if (chooseObj == columnIcon.wallIcon.rightUpPoint)
		{
			dis = (tmp.x - columnIcon.wallIcon.rightDownPoint.transform.position.x);
			columnIcon.wallIcon.AdjPos(tmp, chooseObj);
			columnIcon.wallIcon.AdjMesh();

			chang_walldis = dis;
			ratio_walldis = chang_walldis / ini_wallWidth;
		}
		else if (chooseObj == columnIcon.wallIcon.leftUpPoint)
		{
			dis = (tmp.x - columnIcon.wallIcon.leftDownPoint.transform.position.x);

			columnIcon.wallIcon.AdjPos(tmp, chooseObj);

			columnIcon.wallIcon.AdjMesh();

			chang_walldis = -dis;
			ratio_walldis = chang_walldis / ini_wallWidth;
		}
		else if (chooseObj == columnIcon.wallIcon.rightDownPoint)
		{
			dis = (tmp.x - columnIcon.wallIcon.rightUpPoint.transform.position.x);

			columnIcon.wallIcon.AdjPos(tmp, chooseObj);

			columnIcon.wallIcon.AdjMesh();

			chang_walldis = dis;
			ratio_walldis = chang_walldis / ini_wallWidth;
		}
		else if (chooseObj == columnIcon.wallIcon.leftDownPoint)
		{
			dis = (tmp.x - columnIcon.wallIcon.leftUpPoint.transform.position.x);

			columnIcon.wallIcon.AdjPos(tmp, chooseObj);

			columnIcon.wallIcon.AdjMesh();

			chang_walldis = -dis;
			ratio_walldis = chang_walldis / ini_wallWidth;
		}
		else if (chooseObj == columnIcon.wallIcon.rightUpWindowPoint)
		{
			dis = (tmp.y - columnIcon.wallIcon.leftUpWindowPoint.transform.position.y);

			columnIcon.wallIcon.AdjPos(tmp, chooseObj);

			windowHeight = columnIcon.wallIcon.windowHeight;

			windowUp2TopDis = columnIcon.wallIcon.rightUpPoint.transform.position.y - columnIcon.wallIcon.rightUpWindowPoint.transform.position.y;
		}
		else if (chooseObj == columnIcon.wallIcon.leftUpWindowPoint)
		{
			dis = (tmp.y - columnIcon.wallIcon.rightUpWindowPoint.transform.position.y);

			columnIcon.wallIcon.AdjPos(tmp, chooseObj);

			windowHeight = columnIcon.wallIcon.windowHeight;

			windowUp2TopDis = columnIcon.wallIcon.rightUpPoint.transform.position.y - columnIcon.wallIcon.rightUpWindowPoint.transform.position.y;
		}
		else if (chooseObj == columnIcon.wallIcon.rightDownWindowPoint)
		{
			dis = (tmp.y - columnIcon.wallIcon.leftDownWindowPoint.transform.position.y);

			columnIcon.wallIcon.AdjPos(tmp, chooseObj);

			windowHeight = columnIcon.wallIcon.windowHeight;

			windowDown2ButtonDis = columnIcon.wallIcon.rightDownWindowPoint.transform.position.y - columnIcon.wallIcon.rightDownPoint.transform.position.y;
		}
		else if (chooseObj == columnIcon.wallIcon.leftDownWindowPoint)
		{
			dis = (tmp.y - columnIcon.wallIcon.rightDownWindowPoint.transform.position.y);

			columnIcon.wallIcon.AdjPos(tmp, chooseObj);

			windowHeight = columnIcon.wallIcon.windowHeight;

			windowDown2ButtonDis = columnIcon.wallIcon.rightDownWindowPoint.transform.position.y - columnIcon.wallIcon.rightDownPoint.transform.position.y;
		}

	}
	public void UpdateFunction(string objName)
	{
		switch (objName)
		{
			case "Frieze":
				if (!isFrieze)
				{
					isFrieze = true;

					columnIcon.CreateFrieze(this, "Frieze_mesh", ini_friezeHeight);


					controlPointList.Add(columnIcon.rightColumn.friezePoint);
					controlPointList.Add(columnIcon.leftColumn.friezePoint);

					movement.verlist.Add(columnIcon.rightColumn.friezePoint);
					movement.verlist.Add(columnIcon.leftColumn.friezePoint);

				}
				break;
			case "Balustrade":
				if (!isBalustrade)
				{
					isBalustrade = true;
					columnIcon.CreateBlustrade(this, "Blustrade_mesh", ini_balustradeHeight);

					controlPointList.Add(columnIcon.rightColumn.balustradePoint);
					controlPointList.Add(columnIcon.leftColumn.balustradePoint);

					movement.verlist.Add(columnIcon.rightColumn.balustradePoint);
					movement.verlist.Add(columnIcon.leftColumn.balustradePoint);

				}
				break;
			case "DoubleRoof":
				if (!isDoubleRoof)
				{
					isDoubleRoof = true;

					columnIcon.CreateDoubleRoof(this, "DoubleRoof_mesh", ini_doubleRoofHeight, ini_doubleRoofWidth);

				}
				break;
			case "Wall":
				if (!isWall)
				{
					isWall = true;

					columnIcon.CreateWall(this, "Wall_mesh", ini_wallWidth, ini_windowHeight);

					movement.horlist.Add(columnIcon.wallIcon.rightDownPoint);
					movement.horlist.Add(columnIcon.wallIcon.rightUpPoint);
					movement.horlist.Add(columnIcon.wallIcon.leftDownPoint);
					movement.horlist.Add(columnIcon.wallIcon.leftUpPoint);

					windowUp2TopDis = columnIcon.wallIcon.rightUpPoint.transform.position.y - columnIcon.wallIcon.rightUpWindowPoint.transform.position.y;
					windowDown2ButtonDis = columnIcon.wallIcon.rightDownWindowPoint.transform.position.y - columnIcon.wallIcon.rightDownPoint.transform.position.y;

				}
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
			movement.horlist.Add(columnIcon.wallIcon.rightDownPoint);
			movement.horlist.Add(columnIcon.wallIcon.rightUpPoint);
			movement.horlist.Add(columnIcon.wallIcon.leftDownPoint);
			movement.horlist.Add(columnIcon.wallIcon.leftUpPoint);

			movement.verlist.Add(columnIcon.wallIcon.rightDownWindowPoint);
			movement.verlist.Add(columnIcon.wallIcon.rightUpWindowPoint);
			movement.verlist.Add(columnIcon.wallIcon.leftDownWindowPoint);
			movement.verlist.Add(columnIcon.wallIcon.leftUpWindowPoint);
		}
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
			if (isWall) minClampX = columnIcon.wallIcon.rightUpPoint.transform.position.x + minWidth;
			else minClampX = columnIcon.leftColumn.upPoint.transform.position.x + minWidth;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.leftColumn.body)
		{
			if (isWall) maxClampX = columnIcon.wallIcon.leftUpPoint.transform.position.x - minWidth;
			else maxClampX = columnIcon.leftColumn.upPoint.transform.position.x - minWidth;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.upPoint)
		{
			if (isWall) minClampY = columnIcon.wallIcon.rightUpWindowPoint.transform.position.y + minCloseHeight;
			else minClampY = columnIcon.rightColumn.downPoint.transform.position.y + minCloseHeight + friezeHeight + balustradeHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.downPoint)
		{
			if (isWall) maxClampY = columnIcon.wallIcon.rightDownWindowPoint.transform.position.y - minCloseHeight;
			else maxClampY = columnIcon.rightColumn.upPoint.transform.position.y - minCloseHeight - friezeHeight - balustradeHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.friezePoint)
		{
			minClampY = columnIcon.rightColumn.downPoint.transform.position.y + minCloseHeight + balustradeHeight;
			maxClampY = columnIcon.rightColumn.upPoint.transform.position.y - minFriezeHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.rightColumn.balustradePoint)
		{
			minClampY = columnIcon.rightColumn.downPoint.transform.position.y + minBalustradeHeight;
			maxClampY = columnIcon.rightColumn.upPoint.transform.position.y - minCloseHeight - friezeHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.leftColumn.upPoint)
		{
			if (isWall) minClampY = columnIcon.wallIcon.leftUpWindowPoint.transform.position.y + minCloseHeight;
			else minClampY = columnIcon.leftColumn.downPoint.transform.position.y + minHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.leftColumn.downPoint)
		{
			if (isWall) maxClampY = columnIcon.wallIcon.leftDownWindowPoint.transform.position.y - minCloseHeight;
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
			maxClampY = columnIcon.leftColumn.upPoint.transform.position.y - minCloseHeight - friezeHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.wallIcon.rightUpPoint)
		{
			maxClampX = columnIcon.rightColumn.upPoint.transform.position.x - minWidth;
			minClampX = columnIcon.wallIcon.leftUpPoint.transform.position.x + minWidth;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.wallIcon.rightDownPoint)
		{
			maxClampX = columnIcon.rightColumn.downPoint.transform.position.x - minWidth;
			minClampX = columnIcon.wallIcon.leftUpPoint.transform.position.x + minWidth;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.wallIcon.leftUpPoint)
		{
			minClampX = columnIcon.leftColumn.upPoint.transform.position.x + minWidth;
			maxClampX = columnIcon.wallIcon.rightUpPoint.transform.position.x - minWidth;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.wallIcon.leftDownPoint)
		{
			minClampX = columnIcon.leftColumn.downPoint.transform.position.x + minWidth;
			maxClampX = columnIcon.wallIcon.rightUpPoint.transform.position.x - minWidth;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.wallIcon.rightUpWindowPoint)
		{
			minClampY = columnIcon.wallIcon.rightDownWindowPoint.transform.position.y + minWallHeight;
			maxClampY = columnIcon.wallIcon.rightUpPoint.transform.position.y - minCloseHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.wallIcon.leftUpWindowPoint)
		{
			minClampY = columnIcon.wallIcon.leftDownWindowPoint.transform.position.y + minWallHeight;
			maxClampY = columnIcon.wallIcon.leftUpPoint.transform.position.y - minCloseHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.wallIcon.rightDownWindowPoint)
		{
			minClampY = columnIcon.wallIcon.rightDownPoint.transform.position.y + minCloseHeight;
			maxClampY = columnIcon.wallIcon.rightUpWindowPoint.transform.position.y - minWallHeight;
		}
		else if (dragitemcontroller.chooseObj == columnIcon.wallIcon.leftDownWindowPoint)
		{
			minClampY = columnIcon.wallIcon.leftDownPoint.transform.position.y + minCloseHeight;
			maxClampY = columnIcon.wallIcon.leftUpWindowPoint.transform.position.y - minWallHeight;
		}
		float posX = Mathf.Clamp(inputPos.x, minClampX, maxClampX);
		float posY = Mathf.Clamp(inputPos.y, minClampY, maxClampY);

		return new Vector3(posX, posY, inputPos.z);
	}
}