﻿/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BasedPlatformIcon
{ 

}
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
}*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BasedPlatformStairIcon:DecorateIconObject
{
	public static Vector3 rightUpPoint;
	public static Vector3 rightDownPoint;
	public static Vector3 leftUpPoint;
	public static Vector3 leftDownPoint;

	public float stairWidth;
	public float stairHeight;
	public void BasedPlatformStairCreate<T>(T thisGameObject, string objName, BasedPlatformIcon basedPlatformIcon, float stairWidth, GameObject correspondingDragItemObject)
where T : Component
	{
		InitBodySetting(objName, (int)BodyType.GeneralBody);
		mRenderer.sortingOrder = 3;


		this.stairHeight = basedPlatformIcon.platformHeight;
		this.stairWidth = stairWidth;

		float centerPosX=(basedPlatformIcon.rightDownPoint.transform.position.x+basedPlatformIcon.leftDownPoint.transform.position.x)/2.0f;
		rightDownPoint = new Vector3(centerPosX+stairWidth/2.0f, basedPlatformIcon.rightDownPoint.transform.position.y, basedPlatformIcon.rightDownPoint.transform.position.z);
		leftDownPoint = new Vector3(centerPosX - stairWidth / 2.0f, basedPlatformIcon.leftDownPoint.transform.position.y, basedPlatformIcon.leftDownPoint.transform.position.z);


		rightUpPoint = new Vector3(centerPosX + stairWidth / 2.0f, basedPlatformIcon.rightUpPoint.transform.position.y, basedPlatformIcon.rightUpPoint.transform.position.z);
		leftUpPoint = new Vector3(centerPosX - stairWidth / 2.0f, basedPlatformIcon.leftUpPoint.transform.position.y, basedPlatformIcon.leftUpPoint.transform.position.z);


		mFilter.mesh = CreatRecMesh(leftUpPoint, rightUpPoint, rightDownPoint, leftDownPoint, mFilter.mesh);

		InitLineRender(thisGameObject);
		SetIconObjectColor();
		SetParent2BodyAndControlPointList(thisGameObject);

		InitDecorateIconObjectSetting(correspondingDragItemObject);
	}
	public void AdjPos(BasedPlatformIcon basedPlatformIcon)
	{

		float centerPosX = (basedPlatformIcon.rightDownPoint.transform.position.x + basedPlatformIcon.leftDownPoint.transform.position.x) / 2.0f;
		rightDownPoint = new Vector3(centerPosX + stairWidth / 2.0f, basedPlatformIcon.rightDownPoint.transform.position.y, basedPlatformIcon.rightDownPoint.transform.position.z);
		leftDownPoint = new Vector3(centerPosX - stairWidth / 2.0f, basedPlatformIcon.leftDownPoint.transform.position.y, basedPlatformIcon.leftDownPoint.transform.position.z);


		rightUpPoint = new Vector3(centerPosX + stairWidth / 2.0f, basedPlatformIcon.rightUpPoint.transform.position.y, basedPlatformIcon.rightUpPoint.transform.position.z);
		leftUpPoint = new Vector3(centerPosX - stairWidth / 2.0f, basedPlatformIcon.leftUpPoint.transform.position.y, basedPlatformIcon.leftUpPoint.transform.position.z);
	}
	public void AdjMesh()
	{

		mFilter.mesh.Clear();
		mFilter.mesh = CreatRecMesh(leftUpPoint, rightUpPoint, rightDownPoint, leftDownPoint, mFilter.mesh);

		UpdateLineRender();
		UpdateCollider();
	}
	public void SetIconObjectColor()
	{
		mRenderer.material.color = Color.white;
	}
	public override void InitLineRender<T>(T thisGameObject)
	{
		controlPointList_Vec3_2_LineRender.Add(leftUpPoint);
		controlPointList_Vec3_2_LineRender.Add(rightUpPoint);
		controlPointList_Vec3_2_LineRender.Add(rightDownPoint);
		controlPointList_Vec3_2_LineRender.Add(leftDownPoint);

		for (int i = 0; i < controlPointList_Vec3_2_LineRender.Count; i++)
		{
			if (i != controlPointList_Vec3_2_LineRender.Count - 1)
				CreateLineRenderer(thisGameObject, controlPointList_Vec3_2_LineRender[i], controlPointList_Vec3_2_LineRender[i + 1]);
			else
				CreateLineRenderer(thisGameObject, controlPointList_Vec3_2_LineRender[i], controlPointList_Vec3_2_LineRender[0]);
		}
	}
	public override void UpdateLineRender()
	{
		controlPointList_Vec3_2_LineRender[(int)PointIndex.LeftUpPoint] = (leftUpPoint);
		controlPointList_Vec3_2_LineRender[(int)PointIndex.RightUpPoint] = (rightUpPoint);
		controlPointList_Vec3_2_LineRender[(int)PointIndex.RightDownPoint] = (rightDownPoint);
		controlPointList_Vec3_2_LineRender[(int)PointIndex.LeftDownPoint] = (leftDownPoint);
		for (int i = 0; i < lineRenderList.Count; i++)
		{
			if (i != controlPointList_Vec3_2_LineRender.Count - 1)
				AdjLineRenderer(i, controlPointList_Vec3_2_LineRender[i], controlPointList_Vec3_2_LineRender[i + 1]);
			else
				AdjLineRenderer(i, controlPointList_Vec3_2_LineRender[i], controlPointList_Vec3_2_LineRender[0]);
		}
	}
}
public class BasedPlatformBalustradeIcon : DecorateIconObject
{
	public enum PointIndex { LeftUpPoint = 0, RightUpPoint = 1, RightDownPoint = 2, LeftDownPoint = 3, };
	public List<GameObject> body = null;
	public Column leftColumn;
	public Column rightColumn;
	public GameObject rightUpPoint;
	public GameObject rightDownPoint;
	public GameObject leftUpPoint;
	public GameObject leftDownPoint;
	public float columnHeight;
	private float offset = 0.01f;
	public void BasedPlatformBalustradeCreate<T>(T thisGameObject, string objName, BasedPlatformIcon basedPlatformIcon, float columnHeight, GameObject correspondingDragItemObject) where T : Component
	{
		//InitBodySetting(objName, (int)BodyType.GeneralBody);

		Vector3 h = new Vector3(0.0f, columnHeight, 0.0f);
		Vector3 offsetVector = new Vector3(0.0f, offset, 0.0f);
		Vector3 rightDownPointPos = basedPlatformIcon.rightUpPoint.transform.position + offsetVector;
		Vector3 leftDownPointPos = basedPlatformIcon.leftUpPoint.transform.position + offsetVector;
		Vector3 rightUpPointPos = rightDownPointPos + h;
		Vector3 leftUpPointPos = leftDownPointPos + h;

		rightDownPoint = CreateControlPoint("PBRD", basedPlatformIcon.rightUpPoint.transform.localScale, rightDownPointPos);
		leftDownPoint = CreateControlPoint("PBLD", basedPlatformIcon.leftUpPoint.transform.localScale, leftDownPointPos);
		rightUpPoint = CreateControlPoint("PBRU", basedPlatformIcon.rightUpPoint.transform.localScale, rightUpPointPos);
		leftUpPoint = CreateControlPoint("PBLU", basedPlatformIcon.leftUpPoint.transform.localScale, leftUpPointPos);


		leftColumn = new Column(leftUpPoint, leftDownPoint, columnHeight);
		rightColumn = new Column(rightUpPoint, rightDownPoint, columnHeight);
		this.columnHeight = columnHeight;

		body = new List<GameObject>();
		body.Add(leftColumn.body);
		body.Add(rightColumn.body);


		//mFilter.mesh = CreatRecMesh(leftUpPoint.transform.position, rightUpPoint.transform.position, rightDownPoint.transform.position, leftDownPoint.transform.position, mFilter.mesh);
		//初始位置
		controlPointList.Add(leftColumn.upPoint);
		controlPointList.Add(rightColumn.upPoint);
		controlPointList.Add(rightColumn.downPoint);
		controlPointList.Add(leftColumn.downPoint);
		InitControlPointList2lastControlPointPosition();


		SetIconObjectColor();
		SetParent2BodyAndControlPointList(thisGameObject);

		InitDecorateIconObjectSetting(correspondingDragItemObject);
	}

	public void InitDecorateIconObjectSetting(GameObject correspondingDragItemObject)
	{

		if (!correspondingDragItemObject.GetComponent<DecorateEmptyObjectList>()) return;

		DecorateEmptyObjectList decorateEmptyObjectList = correspondingDragItemObject.GetComponent<DecorateEmptyObjectList>();
		decorateEmptyObjectList.objectList.Clear();
		for (int i = 0; i < body.Count; i++)
		{
			decorateEmptyObjectList.objectList.Add(body[i]);
		}
		for (int i = 0; i < controlPointList.Count; i++)
		{
			decorateEmptyObjectList.objectList.Add(controlPointList[i]);
		}
		for (int i = 0; i < lineRenderList.Count; i++)
		{
			decorateEmptyObjectList.objectList.Add(lineRenderList[i].lineObj);
		}

	}
	public void SetParent2BodyAndControlPointList<T>(T thisGameObject)
where T : Component
	{
		for (int i = 0; i < body.Count; i++)
		{
			body[i].transform.parent = thisGameObject.transform;
		}
		for (int i = 0; i < controlPointList.Count; i++)
		{
			controlPointList[i].transform.parent = thisGameObject.transform;
		}
	}
	public void AdjPos(Vector3 tmp, GameObject chooseObject)
	{
		float OffsetX = 0;
		if (chooseObject == leftUpPoint || chooseObject == rightUpPoint)
		{
			columnHeight = (tmp.y - rightColumn.downPoint.transform.position.y);
			columnHeight = Mathf.Abs(columnHeight);
			//update point
			rightColumn.upPoint.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, tmp.y, rightColumn.upPoint.transform.position.z);
			leftColumn.upPoint.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, tmp.y, leftColumn.upPoint.transform.position.z);

			rightColumn.body.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, rightColumn.upPoint.transform.position.y - columnHeight / 2.0f, rightColumn.upPoint.transform.position.z);

			leftColumn.body.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, leftColumn.upPoint.transform.position.y - columnHeight / 2.0f, leftColumn.upPoint.transform.position.z);

			rightColumn.body.transform.localScale = new Vector3(rightColumn.radius, columnHeight / 2.0f, rightColumn.radius);
			leftColumn.body.transform.localScale = new Vector3(leftColumn.radius, columnHeight / 2.0f, leftColumn.radius);
		}
		else if (chooseObject == rightColumn.body || chooseObject == rightDownPoint)
		{
			OffsetX = (tmp.x - lastControlPointPosition[(int)PointIndex.RightUpPoint].x);

			for (int i = 0; i < rightColumn.controlPointList.Count; i++)
			{
				rightColumn.controlPointList[i].transform.position = new Vector3(tmp.x, rightColumn.controlPointList[i].transform.position.y, rightColumn.controlPointList[i].transform.position.z);
				leftColumn.controlPointList[i].transform.position = new Vector3(leftColumn.controlPointList[i].transform.position.x - (OffsetX), leftColumn.controlPointList[i].transform.position.y, leftColumn.controlPointList[i].transform.position.z);

			}
		}
		else if (chooseObject == leftColumn.body || chooseObject == leftDownPoint)
		{
			OffsetX = (tmp.x - lastControlPointPosition[(int)PointIndex.LeftUpPoint].x);

			for (int i = 0; i < leftColumn.controlPointList.Count; i++)
			{
				leftColumn.controlPointList[i].transform.position = new Vector3(tmp.x, leftColumn.controlPointList[i].transform.position.y, leftColumn.controlPointList[i].transform.position.z);
				rightColumn.controlPointList[i].transform.position = new Vector3(rightColumn.controlPointList[i].transform.position.x - (OffsetX), rightColumn.controlPointList[i].transform.position.y, rightColumn.controlPointList[i].transform.position.z);
			}
		}

		rightColumn.UpdateLastPos();
		leftColumn.UpdateLastPos();
		UpdateLastPos();
	}

}
public class BasedPlatformIcon : RecMeshCreate
{
	public enum PointIndex { LeftUpPoint = 0, RightUpPoint = 1, RightDownPoint = 2, LeftDownPoint = 3, };
	public GameObject rightUpPoint;
	public GameObject rightDownPoint;
	public GameObject leftUpPoint;
	public GameObject leftDownPoint;

	public float platformHeight;
	public float platformWidth;
	public BasedPlatformBalustradeIcon basedPlatformBalustradeIcon = new BasedPlatformBalustradeIcon();
	public BasedPlatformStairIcon basedPlatformStairIcon = new BasedPlatformStairIcon();
	public void BasedPlatformIconCreate<T>(T thisGameObject, string objName, GameObject leftUpPoint, GameObject rightUpPoint, GameObject rightDownPoint, GameObject leftDownPoint)
	where T : Component
	{
		InitBodySetting("BasedPlatformIcon", (int)BodyType.GeneralBody);

		this.rightUpPoint = rightUpPoint;
		this.rightDownPoint = rightDownPoint;
		this.leftUpPoint = leftUpPoint;
		this.leftDownPoint = leftDownPoint;


		platformHeight=rightUpPoint.transform.position.y-rightDownPoint.transform.position.y;
		platformWidth = (rightUpPoint.transform.position.x - leftUpPoint.transform.position.x)/2.0f;


		mFilter.mesh = CreatRecMesh(leftUpPoint.transform.position, rightUpPoint.transform.position, rightDownPoint.transform.position, leftDownPoint.transform.position, mFilter.mesh);

		//初始位置
		controlPointList.Add(leftUpPoint);
		controlPointList.Add(rightUpPoint);
		controlPointList.Add(rightDownPoint);
		controlPointList.Add(leftDownPoint);
		InitControlPointList2lastControlPointPosition();

		SetParent2BodyAndControlPointList(thisGameObject);
		InitLineRender(thisGameObject);
		SetIconObjectColor();
	}
	public Vector3 AdjPos(Vector3 tmp, GameObject chooseObj)
	{
		int index = -1;
		for (int i = 0; i < controlPointList.Count; i++)
		{
			if (chooseObj == controlPointList[i])
			{
				index = i;
				break;
			}
		}
		if (index == -1) return Vector3.zero;

		float offset_x = tmp.x - lastControlPointPosition[index].x;
		float offset_y = tmp.y - lastControlPointPosition[index].y;
		for (int j = 0; j < controlPointList.Count; j++)
		{
			if (index == j) continue;
			if ((lastControlPointPosition[index].y == controlPointList[j].transform.position.y))//y相同
			{
				controlPointList[j].transform.position = new Vector3(lastControlPointPosition[j].x - (offset_x), tmp.y, lastControlPointPosition[j].z);
			}
		}
		if (chooseObj == rightUpPoint || chooseObj == leftUpPoint)
		{
			if (basedPlatformBalustradeIcon.body!=null)
			{
				for (int i = 0; i < basedPlatformBalustradeIcon.rightColumn.controlPointList.Count; i++)
				{
					basedPlatformBalustradeIcon.rightColumn.controlPointList[i].transform.position = new Vector3(basedPlatformBalustradeIcon.rightColumn.controlPointList[i].transform.position.x, basedPlatformBalustradeIcon.rightColumn.controlPointList[i].transform.position.y + (offset_y), basedPlatformBalustradeIcon.rightColumn.controlPointList[i].transform.position.z);
					basedPlatformBalustradeIcon.leftColumn.controlPointList[i].transform.position = new Vector3(basedPlatformBalustradeIcon.leftColumn.controlPointList[i].transform.position.x, basedPlatformBalustradeIcon.leftColumn.controlPointList[i].transform.position.y + (offset_y), basedPlatformBalustradeIcon.leftColumn.controlPointList[i].transform.position.z);

				}
			}	
		}
		if (basedPlatformStairIcon.body != null)
		{
			basedPlatformStairIcon.AdjPos(this);
			basedPlatformStairIcon.AdjMesh();
			basedPlatformStairIcon.UpdateLastPos();
		}
		platformHeight = rightUpPoint.transform.position.y - rightDownPoint.transform.position.y;
		platformWidth = (rightUpPoint.transform.position.x - leftUpPoint.transform.position.x) / 2.0f;

		UpdateLastPos();
		UpdateLineRender();
		return new Vector3(offset_x, offset_y, 0);
	}

	public void CreateBasedPlatformBalustrade<T>(T thisGameObject, string objName, float ini_platBalustradeHeight, GameObject correspondingDragItemObject) where T : Component
	{
		basedPlatformBalustradeIcon = new BasedPlatformBalustradeIcon();
		basedPlatformBalustradeIcon.BasedPlatformBalustradeCreate(thisGameObject, objName, this, ini_platBalustradeHeight, correspondingDragItemObject);
	}
	public void CreateBasedPlatformStair<T>(T thisGameObject, string objName, float stairWidth, GameObject correspondingDragItemObject) where T : Component
	{
		basedPlatformStairIcon = new BasedPlatformStairIcon();
		basedPlatformStairIcon.BasedPlatformStairCreate(thisGameObject, objName, this, stairWidth, correspondingDragItemObject);
	}
}
public class platform2icon : MonoBehaviour
{
	public enum PointIndex { LeftUpPoint = 0, RightUpPoint = 1, RightDownPoint = 2, LeftDownPoint = 3, };
	public List<GameObject> controlPointList = new List<GameObject>();


	private DragItemController dragitemcontroller;
	private Movement movement;

	BasedPlatformIcon basedPlatformIcon;

	public float stairWidth;
	//for ratio
	public Vector2 chang_platdis;
	public Vector2 ini_platdis;
	public Vector2 chang_platBalustradeDis;
	public Vector2 ini_platBalustradeDis;

	public bool isBasedPlatformBalustrade;
	public bool isBasedPlatformStair;

	void Start()
	{
		dragitemcontroller = GameObject.Find("DragItemController").GetComponent<DragItemController>();
		movement = GameObject.Find("Movement").GetComponent<Movement>();

		basedPlatformIcon = CreateBasedPlatformIcon();
	}
	BasedPlatformIcon CreateBasedPlatformIcon()
	{
		BasedPlatformIcon basedPlatformIcon = new BasedPlatformIcon();

		basedPlatformIcon.BasedPlatformIconCreate(this, "BasedPlatformIcon_mesh", controlPointList[(int)PointIndex.LeftUpPoint], controlPointList[(int)PointIndex.RightUpPoint], controlPointList[(int)PointIndex.RightDownPoint], controlPointList[(int)PointIndex.LeftDownPoint]);

		ini_platdis.x = (controlPointList[(int)PointIndex.RightUpPoint].transform.position.x - controlPointList[(int)PointIndex.LeftUpPoint].transform.position.x);
		ini_platdis.y = (controlPointList[(int)PointIndex.RightUpPoint].transform.position.y - controlPointList[(int)PointIndex.RightDownPoint].transform.position.y);

		ini_platBalustradeDis.x = ini_platdis.x;
		ini_platBalustradeDis.y = ini_platdis.y * 0.8f;

		stairWidth = ini_platdis.x*0.3f;

		return basedPlatformIcon;
	}
	public void adjPos()
	{
		chang_platdis = Vector2.zero;

		Vector3 tmp = dragitemcontroller.chooseObj.transform.position;
		GameObject chooseObj = dragitemcontroller.chooseObj;
		float dis=0;
		if (chooseObj == basedPlatformIcon.rightUpPoint || chooseObj == basedPlatformIcon.leftUpPoint ||
			chooseObj == basedPlatformIcon.rightDownPoint || chooseObj == basedPlatformIcon.leftDownPoint)
		{
			Vector2 offset = basedPlatformIcon.AdjPos(tmp, chooseObj);
			basedPlatformIcon.AdjMesh();
			chang_platdis.x = offset.x;
			chang_platdis.y = offset.y;

		}
		else if (chooseObj == basedPlatformIcon.basedPlatformBalustradeIcon.rightUpPoint || chooseObj == basedPlatformIcon.basedPlatformBalustradeIcon.leftUpPoint)
		{
			dis = (tmp.y - basedPlatformIcon.basedPlatformBalustradeIcon.rightUpPoint.transform.position.y);
			basedPlatformIcon.basedPlatformBalustradeIcon.AdjPos(tmp, chooseObj);

			chang_platBalustradeDis.y = dis;
		}
		else if (chooseObj == basedPlatformIcon.basedPlatformBalustradeIcon.rightDownPoint)
		{
			dis = (tmp.x - basedPlatformIcon.basedPlatformBalustradeIcon.rightUpPoint.transform.position.x);
			basedPlatformIcon.basedPlatformBalustradeIcon.AdjPos(tmp, chooseObj);

			chang_platBalustradeDis.x = dis;
		}
		else if(chooseObj == basedPlatformIcon.basedPlatformBalustradeIcon.leftDownPoint)
		{
			dis = (tmp.x - basedPlatformIcon.basedPlatformBalustradeIcon.rightUpPoint.transform.position.x);
			basedPlatformIcon.basedPlatformBalustradeIcon.AdjPos(tmp, chooseObj);

			chang_platBalustradeDis.x = -dis;
		}
		else if (chooseObj == basedPlatformIcon.basedPlatformBalustradeIcon.rightColumn.body)
		{
			dis = (tmp.x - basedPlatformIcon.basedPlatformBalustradeIcon.rightUpPoint.transform.position.x);
			basedPlatformIcon.basedPlatformBalustradeIcon.AdjPos(tmp, chooseObj);

			chang_platBalustradeDis.x = dis;
		}
		else if(chooseObj == basedPlatformIcon.basedPlatformBalustradeIcon.leftColumn.body)
		{
			dis = (tmp.x - basedPlatformIcon.basedPlatformBalustradeIcon.rightUpPoint.transform.position.x);
			basedPlatformIcon.basedPlatformBalustradeIcon.AdjPos(tmp, chooseObj);

			chang_platBalustradeDis.x = -dis;
		}
	}
	public void DestroyFunction(string objName)
	{
		switch (objName)
		{
			case "BasePlatFormBalustrade":
				isBasedPlatformBalustrade = false;
				basedPlatformIcon.basedPlatformBalustradeIcon.body=null;
				break;
			case "BasedPlatformStair":
				isBasedPlatformStair = false;
				break;
		}
	}
	public void UpdateFunction(string objName, GameObject correspondingDragItemObject)
	{
		switch (objName)
		{
			case "BasePlatFormBalustrade":
				if (basedPlatformIcon.basedPlatformBalustradeIcon.body == null)
				{
					isBasedPlatformBalustrade = true;
					basedPlatformIcon.CreateBasedPlatformBalustrade(this, "BasedPlatformBalustradeIcon_mesh", ini_platBalustradeDis.y, correspondingDragItemObject);
				}
				break;
			case "BasedPlatformStair":
				if (basedPlatformIcon.basedPlatformStairIcon.body == null)
				{
					isBasedPlatformStair = true;
					basedPlatformIcon.CreateBasedPlatformStair(this, "BasedPlatformStairIcon_mesh", stairWidth, correspondingDragItemObject);
				}
				break;
		}
	}
	public void addpoint()
	{
		controlPointList.RemoveAll(GameObject => GameObject == null);
		movement.freelist.AddRange(controlPointList);
		if (isBasedPlatformBalustrade)
		{
			movement.verlist.Add(basedPlatformIcon.basedPlatformBalustradeIcon.leftUpPoint);
			movement.verlist.Add(basedPlatformIcon.basedPlatformBalustradeIcon.rightUpPoint);
			movement.horlist.Add(basedPlatformIcon.basedPlatformBalustradeIcon.rightDownPoint);
			movement.horlist.Add(basedPlatformIcon.basedPlatformBalustradeIcon.leftDownPoint);

			movement.horlist.Add(basedPlatformIcon.basedPlatformBalustradeIcon.rightColumn.body);
			movement.horlist.Add(basedPlatformIcon.basedPlatformBalustradeIcon.leftColumn.body);
		}
	}
	public Vector3 ClampPos(Vector3 inputPos)
	{
		float minClampX = float.MinValue;
		float maxClampX = float.MaxValue;
		float minClampY = float.MinValue;
		float maxClampY = float.MaxValue;

		float minWidth = (ini_platdis.x) * 0.4f;
		float minHeight = (ini_platdis.y) * 0.4f;
		float minCloseHeight = ini_platdis.y * 0.1f;
		if (dragitemcontroller.chooseObj == controlPointList[(int)PointIndex.LeftUpPoint])
		{
			if (basedPlatformIcon.basedPlatformBalustradeIcon.body != null)
				maxClampX = basedPlatformIcon.basedPlatformBalustradeIcon.leftUpPoint.transform.position.x;
			else 
				maxClampX = controlPointList[(int)PointIndex.RightUpPoint].transform.position.x - minWidth;
			minClampY = controlPointList[(int)PointIndex.LeftDownPoint].transform.position.y + minHeight;
		}
		else if (dragitemcontroller.chooseObj == controlPointList[(int)PointIndex.LeftDownPoint])
		{
			maxClampX = controlPointList[(int)PointIndex.RightUpPoint].transform.position.x - minWidth;
			maxClampY = controlPointList[(int)PointIndex.LeftUpPoint].transform.position.y - minHeight;
		}
		else if (dragitemcontroller.chooseObj == controlPointList[(int)PointIndex.RightUpPoint])
		{
			if (basedPlatformIcon.basedPlatformBalustradeIcon.body != null)
				minClampX = basedPlatformIcon.basedPlatformBalustradeIcon.rightUpPoint.transform.position.x;
			else 
				minClampX = controlPointList[(int)PointIndex.LeftUpPoint].transform.position.x + minWidth;

			minClampY = controlPointList[(int)PointIndex.RightDownPoint].transform.position.y + minHeight;
			
		}
		else if (dragitemcontroller.chooseObj == controlPointList[(int)PointIndex.RightDownPoint])
		{
			minClampX = controlPointList[(int)PointIndex.LeftUpPoint].transform.position.x + minWidth;
			maxClampY = controlPointList[(int)PointIndex.RightUpPoint].transform.position.y - minHeight;
		}
		else if (dragitemcontroller.chooseObj == basedPlatformIcon.basedPlatformBalustradeIcon.rightUpPoint)
		{
			minClampY = basedPlatformIcon.basedPlatformBalustradeIcon.rightDownPoint.transform.position.y + minHeight;
		}
		else if (dragitemcontroller.chooseObj == basedPlatformIcon.basedPlatformBalustradeIcon.leftUpPoint)
		{
			minClampY = basedPlatformIcon.basedPlatformBalustradeIcon.leftDownPoint.transform.position.y + minHeight;
		}
		else if (dragitemcontroller.chooseObj == basedPlatformIcon.basedPlatformBalustradeIcon.rightDownPoint)
		{
			maxClampX = basedPlatformIcon.rightUpPoint.transform.position.x;
			minClampX = basedPlatformIcon.basedPlatformBalustradeIcon.leftUpPoint.transform.position.x + minWidth;
		}
		else if (dragitemcontroller.chooseObj == basedPlatformIcon.basedPlatformBalustradeIcon.leftDownPoint)
		{
			maxClampX = basedPlatformIcon.basedPlatformBalustradeIcon.rightUpPoint.transform.position.x - minWidth;
			minClampX = basedPlatformIcon.leftUpPoint.transform.position.x;
		}
		else if (dragitemcontroller.chooseObj == basedPlatformIcon.basedPlatformBalustradeIcon.rightColumn.body)
		{
			maxClampX = basedPlatformIcon.rightUpPoint.transform.position.x;
			minClampX = basedPlatformIcon.basedPlatformBalustradeIcon.leftUpPoint.transform.position.x + minWidth;
		}
		else if (dragitemcontroller.chooseObj == basedPlatformIcon.basedPlatformBalustradeIcon.leftColumn.body)
		{
			maxClampX = basedPlatformIcon.basedPlatformBalustradeIcon.rightUpPoint.transform.position.x - minWidth;
			minClampX = basedPlatformIcon.leftUpPoint.transform.position.x;
		}
		float posX = Mathf.Clamp(inputPos.x, minClampX, maxClampX);
		float posY = Mathf.Clamp(inputPos.y, minClampY, maxClampY);
		return new Vector3(posX, posY, inputPos.z);

		return inputPos;
	}
}