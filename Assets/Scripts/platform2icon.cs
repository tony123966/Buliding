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
public class BasedPlatformStairIcon : DecorateIconObject<M>
{
	public static Vector3 rightUpPoint;
	public static Vector3 rightDownPoint;
	public static Vector3 leftUpPoint;
	public static Vector3 leftDownPoint;

	public int stairIconCount = 1;
	public int stairIconMaxCount = 10;

	public float stairWidth;
	public float stairHeight;
	public void BasedPlatformStairCreate<T,M>(T thisGameObject, string objName, M basedPlatformIcon, float stairWidth, GameObject correspondingDragItemObject)
where T : Component
where M : class
	{

		MainComponent = basedPlatformIcon;

		InitBodySetting(objName, (int)BodyType.GeneralBody);
		InitIconMenuButtonSetting();

		mRenderer.sortingOrder = 3;


		this.stairHeight = basedPlatformIcon.platformHeight;
		this.stairWidth = stairWidth;

		float centerPosX = (basedPlatformIcon.rightDownPoint.transform.position.x + basedPlatformIcon.leftDownPoint.transform.position.x) / 2.0f;
		rightDownPoint = new Vector3(centerPosX + stairWidth / 2.0f, basedPlatformIcon.rightDownPoint.transform.position.y, basedPlatformIcon.rightDownPoint.transform.position.z);
		leftDownPoint = new Vector3(centerPosX - stairWidth / 2.0f, basedPlatformIcon.leftDownPoint.transform.position.y, basedPlatformIcon.leftDownPoint.transform.position.z);


		rightUpPoint = new Vector3(centerPosX + stairWidth / 2.0f, basedPlatformIcon.rightUpPoint.transform.position.y, basedPlatformIcon.rightUpPoint.transform.position.z);
		leftUpPoint = new Vector3(centerPosX - stairWidth / 2.0f, basedPlatformIcon.leftUpPoint.transform.position.y, basedPlatformIcon.leftUpPoint.transform.position.z);


		mFilter.mesh = CreatRecMesh(leftUpPoint, rightUpPoint, rightDownPoint, leftDownPoint, mFilter.mesh);

		InitLineRender(thisGameObject);
		SetIconObjectColor();
		SetParent2BodyAndControlPointList(thisGameObject);

		InitDecorateIconObjectSetting(correspondingDragItemObject);
	}
	public void AdjPos(GameObject baseLeftUpPoint, GameObject baseRightUpPoint, GameObject baseRightDownPoint, GameObject baseLeftDownPoint)
	{

		float centerPosX = (baseRightDownPoint.transform.position.x + baseLeftDownPoint.transform.position.x) / 2.0f;
		rightDownPoint = new Vector3(centerPosX + stairWidth / 2.0f, baseRightDownPoint.transform.position.y, baseRightDownPoint.transform.position.z);
		leftDownPoint = new Vector3(centerPosX - stairWidth / 2.0f, baseLeftDownPoint.transform.position.y, baseLeftDownPoint.transform.position.z);


		rightUpPoint = new Vector3(centerPosX + stairWidth / 2.0f, baseRightUpPoint.transform.position.y, baseRightUpPoint.transform.position.z);
		leftUpPoint = new Vector3(centerPosX - stairWidth / 2.0f, baseLeftUpPoint.transform.position.y, baseLeftUpPoint.transform.position.z);
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
	public void InitIconMenuButtonSetting()
	{
		iconMenuControl.delelteButton.isDeleteIconButton = true;
		iconMenuControl.scrollBarButton.isScrollBarIconButton = true;
		iconMenuControl.scrollBarButton.scrollBarIconValue = stairIconCount;
		iconMenuControl.scrollBarButton.scrollBarIconMaxValue = stairIconMaxCount;
		iconMenuControl.scrollBarButton.scrollBarIconType = (int)ScrollBarButton.ScrollType.INT;
	}
	public override void InitIconMenuButtonUpdate()
	{
		stairIconCount = iconMenuControl.scrollBarButton.scrollBarIconValue;
	}
}
public class BasedPlatformBalustradeIcon : DecorateIconObject<BasedPlatformIcon>
{
	public enum PointIndex { LeftUpPoint = 0, RightUpPoint = 1, RightDownPoint = 2, LeftDownPoint = 3, };
	public List<GameObject> body = null;
	public Column leftColumn;
	public Column rightColumn;
	public GameObject rightUpPoint;
	public GameObject rightDownPoint;
	public GameObject leftUpPoint;
	public GameObject leftDownPoint;

	public int mutiColumnIconCount = 3;
	public int mutiColumnIconMaxCount = 13;
	public float balustradeColumnHeight;
	public float balustradeColumnWidth;

	public float initBalustradeColumnHeight;
	public float initBalustradeColumnWidth;

	float offset = 0.01f;
	public void BasedPlatformBalustradeCreate<T>(T thisGameObject, string objName, BasedPlatformIcon basedPlatformIcon, float columnHeight, GameObject correspondingDragItemObject) where T : Component
	{

		MainComponent = basedPlatformIcon;
		InitBodySetting(thisGameObject);
		InitIconMenuButtonSetting();

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


		leftColumn = new Column("BasedPlatformBalustradeIcon", leftUpPoint, leftDownPoint, columnHeight);
		rightColumn = new Column("BasedPlatformBalustradeIcon", rightUpPoint, rightDownPoint, columnHeight);
		initBalustradeColumnHeight = balustradeColumnHeight = columnHeight;
		initBalustradeColumnWidth = balustradeColumnWidth = rightDownPointPos.x - leftDownPointPos.x;



		body = new List<GameObject>();
		body.Add(leftColumn.body);
		body.Add(rightColumn.body);


		//mFilter.mesh = CreatRecMesh(leftUpPoint.transform.position, rightUpPoint.transform.position, rightDownPoint.transform.position, leftDownPoint.transform.position, mFilter.mesh);
		//初始位置
		controlPointList.Add(leftColumn.upPoint);
		controlPointList.Add(rightColumn.upPoint);
		controlPointList.Add(rightColumn.downPoint);
		controlPointList.Add(leftColumn.downPoint);
		controlPointList.Add(rightColumn.body);
		controlPointList.Add(leftColumn.body);
		InitControlPointList2lastControlPointPosition();


		SetIconObjectColor();
		SetParent2BodyAndControlPointList(thisGameObject);

		InitDecorateIconObjectSetting(correspondingDragItemObject);

		for (int i = 0; i < body.Count; i++)
		{
			if (body[i].GetComponent<IconControl>())
				UnityEngine.Object.Destroy(body[i].GetComponent<IconControl>());
		}
	}
	public void SetIconObjectColor()
	{
		base.SetIconObjectColor();
		rightColumn.body.GetComponent<MeshRenderer>().material.color = Color.red;
		leftColumn.body.GetComponent<MeshRenderer>().material.color = Color.red;
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
	public Vector3 ClampPos(Vector3 inputPos, GameObject chooseObj)
	{
		float minClampX = float.MinValue;
		float maxClampX = float.MaxValue;
		float minClampY = float.MinValue;
		float maxClampY = float.MaxValue;

		float minWidth = (initBalustradeColumnWidth) * 0.4f;
		float minHeight = (initBalustradeColumnHeight) * 0.4f;

		if (chooseObj == rightUpPoint)
		{
			minClampY = rightDownPoint.transform.position.y + minHeight;
		}
		else if (chooseObj == leftUpPoint)
		{
			minClampY = leftDownPoint.transform.position.y + minHeight;
		}
		else if (chooseObj == rightDownPoint)
		{
			maxClampX = MainComponent.rightUpPoint.transform.position.x;
			minClampX = leftUpPoint.transform.position.x + minWidth;
		}
		else if (chooseObj == leftDownPoint)
		{
			maxClampX = rightUpPoint.transform.position.x - minWidth;
			minClampX = MainComponent.leftUpPoint.transform.position.x;
		}
		else if (chooseObj == rightColumn.body)
		{
			maxClampX = MainComponent.rightUpPoint.transform.position.x;
			minClampX = leftUpPoint.transform.position.x + minWidth;
		}
		else if (chooseObj == leftColumn.body)
		{
			maxClampX = rightUpPoint.transform.position.x - minWidth;
			minClampX = MainComponent.leftUpPoint.transform.position.x;
		}

		float posX = Mathf.Clamp(inputPos.x, minClampX, maxClampX);
		float posY = Mathf.Clamp(inputPos.y, minClampY, maxClampY);
		return new Vector3(posX, posY, inputPos.z);
	}
	public void AdjPos(Vector3 tmp, GameObject chooseObject)
	{
		float OffsetX = 0;
		if (chooseObject == leftUpPoint || chooseObject == rightUpPoint)
		{
			balustradeColumnHeight = (tmp.y - rightColumn.downPoint.transform.position.y);
			balustradeColumnHeight = Mathf.Abs(balustradeColumnHeight);
			//update point
			rightColumn.upPoint.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, tmp.y, rightColumn.upPoint.transform.position.z);
			leftColumn.upPoint.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, tmp.y, leftColumn.upPoint.transform.position.z);

			rightColumn.body.transform.position = new Vector3(rightColumn.upPoint.transform.position.x, rightColumn.upPoint.transform.position.y - balustradeColumnHeight / 2.0f, rightColumn.upPoint.transform.position.z);

			leftColumn.body.transform.position = new Vector3(leftColumn.upPoint.transform.position.x, leftColumn.upPoint.transform.position.y - balustradeColumnHeight / 2.0f, leftColumn.upPoint.transform.position.z);

			rightColumn.body.transform.localScale = new Vector3(rightColumn.radius, balustradeColumnHeight / 2.0f, rightColumn.radius);
			leftColumn.body.transform.localScale = new Vector3(leftColumn.radius, balustradeColumnHeight / 2.0f, leftColumn.radius);
		}
		else if (chooseObject == rightColumn.body || chooseObject == rightDownPoint)
		{
			OffsetX = (tmp.x - lastControlPointPosition[(int)PointIndex.RightUpPoint].x);

			for (int i = 0; i < rightColumn.controlPointList.Count; i++)
			{
				rightColumn.controlPointList[i].transform.position = new Vector3(tmp.x, rightColumn.controlPointList[i].transform.position.y, rightColumn.controlPointList[i].transform.position.z);
				leftColumn.controlPointList[i].transform.position = new Vector3(leftColumn.controlPointList[i].transform.position.x - (OffsetX), leftColumn.controlPointList[i].transform.position.y, leftColumn.controlPointList[i].transform.position.z);

			}
			balustradeColumnWidth = rightDownPoint.transform.position.x - leftDownPoint.transform.position.x;
		}
		else if (chooseObject == leftColumn.body || chooseObject == leftDownPoint)
		{
			OffsetX = (tmp.x - lastControlPointPosition[(int)PointIndex.LeftUpPoint].x);

			for (int i = 0; i < leftColumn.controlPointList.Count; i++)
			{
				leftColumn.controlPointList[i].transform.position = new Vector3(tmp.x, leftColumn.controlPointList[i].transform.position.y, leftColumn.controlPointList[i].transform.position.z);
				rightColumn.controlPointList[i].transform.position = new Vector3(rightColumn.controlPointList[i].transform.position.x - (OffsetX), rightColumn.controlPointList[i].transform.position.y, rightColumn.controlPointList[i].transform.position.z);
			}
			balustradeColumnWidth = rightDownPoint.transform.position.x - leftDownPoint.transform.position.x;
		}

		rightColumn.UpdateLastPos();
		leftColumn.UpdateLastPos();
		UpdateLastPos();
	}
	public void InitBodySetting<T>(T thisGameObject) where T : Component
	{
		iconMenuControl = thisGameObject.gameObject.AddComponent<IconControl>();
	}
	public void InitIconMenuButtonSetting()
	{
		iconMenuControl.delelteButton.isDeleteIconButton = true;
		iconMenuControl.scrollBarButton.isScrollBarIconButton = true;
		iconMenuControl.scrollBarButton.scrollBarIconValue = mutiColumnIconCount;
		iconMenuControl.scrollBarButton.scrollBarIconMaxValue = mutiColumnIconMaxCount;
		iconMenuControl.scrollBarButton.scrollBarIconType = (int)ScrollBarButton.ScrollType.OddINT;
	}
	public override void InitIconMenuButtonUpdate()
	{
		mutiColumnIconCount = iconMenuControl.scrollBarButton.scrollBarIconValue;
	}
}
public class CurvePlatformStruct
{
	public List<GameObject> controlPointList = new List<GameObject>();
	public catline catLine;
}
public class CurvePlatformIcon : IconObject
{
	public enum PointIndex { LeftUpPoint = 0, RightUpPoint = 1, RightDownPoint = 2, LeftDownPoint = 3, LeftMidPoint = 4, RightMidPoint = 5, };

	public GameObject rightUpPoint;
	public GameObject rightMidPoint;
	public GameObject rightDownPoint;
	public GameObject leftUpPoint;
	public GameObject leftMidPoint;
	public GameObject leftDownPoint;

	public CurvePlatformStruct rightPlatformLine = new CurvePlatformStruct();
	public CurvePlatformStruct leftPlatformLine = new CurvePlatformStruct();
	public int numberOfPoints = 10;
	public int sliceUnit = 1;

	public float platformHeight;
	public float platformTopWidth;
	public float platformButtonWidth;

	public float initPlatformHeight;
	public float initPlatformTopWidth;
	public float initPlatformButtonWidth;


	public BasedPlatformBalustradeIcon basedPlatformBalustradeIcon = null;
	public BasedPlatformStairIcon basedPlatformStairIcon = null;
	public void CurvePlatformIconCreate<T>(T thisGameObject, string objName, GameObject leftUpPoint, GameObject rightUpPoint, GameObject rightDownPoint, GameObject leftDownPoint, GameObject leftMidPoint, GameObject rightMidPoint)
	where T : Component
	{
		InitBodySetting(objName, (int)BodyType.GeneralBody);
		InitIconMenuButtonSetting();

		controlPointList.Add(leftUpPoint);
		controlPointList.Add(rightUpPoint);
		controlPointList.Add(rightDownPoint);
		controlPointList.Add(leftDownPoint);
		controlPointList.Add(leftMidPoint);
		controlPointList.Add(rightMidPoint);

		//RightCatmullromLine
		GameObject cR = new GameObject("CatLine_Right");
		cR.transform.parent = thisGameObject.transform;
		rightPlatformLine.catLine = cR.AddComponent<catline>();
		rightPlatformLine.controlPointList.Add(rightUpPoint);
		rightPlatformLine.controlPointList.Add(rightMidPoint);
		rightPlatformLine.controlPointList.Add(rightDownPoint);
		rightPlatformLine.catLine.AddControlPoint(rightUpPoint);
		rightPlatformLine.catLine.AddControlPoint(rightMidPoint);
		rightPlatformLine.catLine.AddControlPoint(rightDownPoint);
		//RightCatmullromLine
		GameObject cL = new GameObject("CatLine_Left");
		cL.transform.parent = thisGameObject.transform;
		leftPlatformLine.catLine = cR.AddComponent<catline>();
		leftPlatformLine.controlPointList.Add(leftUpPoint);
		leftPlatformLine.controlPointList.Add(leftMidPoint);
		leftPlatformLine.controlPointList.Add(leftDownPoint);
		leftPlatformLine.catLine.AddControlPoint(leftUpPoint);
		leftPlatformLine.catLine.AddControlPoint(leftMidPoint);
		leftPlatformLine.catLine.AddControlPoint(leftDownPoint);
		InitControlPointList2lastControlPointPosition();

		rightPlatformLine.catLine.SetLineNumberOfPoints(numberOfPoints);
		rightPlatformLine.catLine.ResetCatmullRom();

		leftPlatformLine.catLine.SetLineNumberOfPoints(numberOfPoints);
		leftPlatformLine.catLine.ResetCatmullRom();


		SetParent2BodyAndControlPointList(thisGameObject);
		InitLineRender(thisGameObject);

		SetIconObjectColor();


		AdjMesh();
	}
	public void AdjMesh()
	{

		int innerPointCount = rightPlatformLine.catLine.innerPointList.Count;
		float uvR = (1 / (float)innerPointCount);
		mFilter.mesh.Clear();
		Vector3[] v = new Vector3[2 * innerPointCount];
		Vector3[] n = new Vector3[2 * innerPointCount];
		//Vector2[] uv = new Vector2[2 * innerPointCount];
		int[] t = new int[6 * innerPointCount];

		for (int i = 0; i < innerPointCount; i++)
		{
			v[i] = rightPlatformLine.catLine.innerPointList[i];
		}
		for (int i = 0; i < innerPointCount; i++)
		{
			v[i + innerPointCount] = leftPlatformLine.catLine.innerPointList[i];
		}
		int index = 0;
		for (int i = 0; i < rightPlatformLine.catLine.innerPointList.Count - 1; i++)
		{
			t[index] = i;
			t[index + 1] = (i + 1);
			t[index + 2] = i + rightPlatformLine.catLine.innerPointList.Count;
			t[index + 3] = i + rightPlatformLine.catLine.innerPointList.Count;
			t[index + 4] = (i + 1);
			t[index + 5] = (i + 1) + rightPlatformLine.catLine.innerPointList.Count;
			index += 6;
		}
		for (int i = 0; i < 2 * innerPointCount; i++)
		{
			n[i] = -Vector3.forward;
		}
		mFilter.mesh.vertices = v;
		mFilter.mesh.triangles = t;
		mFilter.mesh.normals = n;

		mFilter.mesh.RecalculateNormals();
		mFilter.mesh.RecalculateBounds();
		//mFilter.mesh.uv = uv;

		//UpdateLineRender();
		UpdateCollider();
	}
	public override void InitLineRender<T>(T thisGameObject)
	{
		for (int i = 0; i < rightPlatformLine.catLine.innerPointList.Count - 1; i += sliceUnit)
		{
			i = Mathf.Min(i, rightPlatformLine.catLine.innerPointList.Count - 1);
			CreateLineRenderer(thisGameObject, rightPlatformLine.catLine.innerPointList[i], rightPlatformLine.catLine.innerPointList[Mathf.Min((i + sliceUnit), rightPlatformLine.catLine.innerPointList.Count - 1)]);
			if (i == rightPlatformLine.catLine.innerPointList.Count - 1) return;
		}
		for (int i = 0; i < leftPlatformLine.catLine.innerPointList.Count - 1; i += sliceUnit)
		{
			i = Mathf.Min(i, leftPlatformLine.catLine.innerPointList.Count - 1);
			CreateLineRenderer(thisGameObject, leftPlatformLine.catLine.innerPointList[i], leftPlatformLine.catLine.innerPointList[Mathf.Min((i + sliceUnit), leftPlatformLine.catLine.innerPointList.Count - 1)]);
			if (i == leftPlatformLine.catLine.innerPointList.Count - 1) return;
		}
		CreateLineRenderer(thisGameObject, leftPlatformLine.catLine.innerPointList[0], rightPlatformLine.catLine.innerPointList[0]);
		CreateLineRenderer(thisGameObject, leftPlatformLine.catLine.innerPointList[leftPlatformLine.catLine.innerPointList.Count - 1], rightPlatformLine.catLine.innerPointList[rightPlatformLine.catLine.innerPointList.Count - 1]);
	}
	public Vector3 ClampPos(Vector3 inputPos, GameObject chooseObj)
	{
		float minClampX = float.MinValue;
		float maxClampX = float.MaxValue;
		float minClampY = float.MinValue;
		float maxClampY = float.MaxValue;

		float minWidth = (initPlatformTopWidth) * 0.4f;
		float minHeight = (initPlatformHeight) * 0.4f;

		if (chooseObj == controlPointList[(int)PointIndex.LeftUpPoint])
		{
			if (basedPlatformBalustradeIcon != null)
				maxClampX = basedPlatformBalustradeIcon.leftUpPoint.transform.position.x;
			else
				maxClampX = controlPointList[(int)PointIndex.RightUpPoint].transform.position.x - minWidth;
			minClampY = controlPointList[(int)PointIndex.LeftDownPoint].transform.position.y + minHeight;
		}
		else if (chooseObj == controlPointList[(int)PointIndex.LeftDownPoint])
		{
			maxClampX = controlPointList[(int)PointIndex.RightDownPoint].transform.position.x - minWidth;
			maxClampY = controlPointList[(int)PointIndex.LeftUpPoint].transform.position.y - minHeight;
		}
		else if (chooseObj == controlPointList[(int)PointIndex.RightUpPoint])
		{
			if (basedPlatformBalustradeIcon != null)
				minClampX = basedPlatformBalustradeIcon.rightUpPoint.transform.position.x;
			else
				minClampX = controlPointList[(int)PointIndex.LeftUpPoint].transform.position.x + minWidth;

			minClampY = controlPointList[(int)PointIndex.RightDownPoint].transform.position.y + minHeight;

		}
		else if (chooseObj == controlPointList[(int)PointIndex.RightDownPoint])
		{
			minClampX = controlPointList[(int)PointIndex.LeftDownPoint].transform.position.x + minWidth;
			maxClampY = controlPointList[(int)PointIndex.RightUpPoint].transform.position.y - minHeight;
		}

		float posX = Mathf.Clamp(inputPos.x, minClampX, maxClampX);
		float posY = Mathf.Clamp(inputPos.y, minClampY, maxClampY);
		return new Vector3(posX, posY, inputPos.z);
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
			if (basedPlatformBalustradeIcon != null)
			{
				for (int i = 0; i < basedPlatformBalustradeIcon.rightColumn.controlPointList.Count; i++)
				{
					basedPlatformBalustradeIcon.rightColumn.controlPointList[i].transform.position = new Vector3(basedPlatformBalustradeIcon.rightColumn.controlPointList[i].transform.position.x, basedPlatformBalustradeIcon.rightColumn.controlPointList[i].transform.position.y + (offset_y), basedPlatformBalustradeIcon.rightColumn.controlPointList[i].transform.position.z);
					basedPlatformBalustradeIcon.leftColumn.controlPointList[i].transform.position = new Vector3(basedPlatformBalustradeIcon.leftColumn.controlPointList[i].transform.position.x, basedPlatformBalustradeIcon.leftColumn.controlPointList[i].transform.position.y + (offset_y), basedPlatformBalustradeIcon.leftColumn.controlPointList[i].transform.position.z);

				}
			}
		}
		if (basedPlatformStairIcon != null)
		{
			basedPlatformStairIcon.AdjPos(leftUpPoint, rightUpPoint, rightDownPoint, leftDownPoint);
			basedPlatformStairIcon.AdjMesh();
			basedPlatformStairIcon.UpdateLastPos();
		}
		platformHeight = rightUpPoint.transform.position.y - rightDownPoint.transform.position.y;
		platformTopWidth = (rightUpPoint.transform.position.x - leftUpPoint.transform.position.x);
		platformButtonWidth = (rightDownPoint.transform.position.x - leftDownPoint.transform.position.x);
		UpdateLastPos();
		UpdateLineRender();
		return new Vector3(offset_x, offset_y, 0);
	}

	public void CreateBasedPlatformBalustrade<T>(T thisGameObject, string objName, float ini_platBalustradeHeight, GameObject correspondingDragItemObject) where T : Component
	{
		basedPlatformBalustradeIcon = new BasedPlatformBalustradeIcon();
		//basedPlatformBalustradeIcon.BasedPlatformBalustradeCreate(thisGameObject, objName, this, ini_platBalustradeHeight, correspondingDragItemObject);
	}
	public void CreateBasedPlatformStair<T>(T thisGameObject, string objName, float stairWidth, GameObject correspondingDragItemObject) where T : Component
	{
		basedPlatformStairIcon = new BasedPlatformStairIcon();
		//basedPlatformStairIcon.BasedPlatformStairCreate(thisGameObject, objName, this, stairWidth, correspondingDragItemObject);
	}
	public override void InitIconMenuButtonUpdate()
	{
		if (basedPlatformBalustradeIcon != null) basedPlatformBalustradeIcon.InitIconMenuButtonUpdate();
		if (basedPlatformStairIcon != null) basedPlatformStairIcon.InitIconMenuButtonUpdate();
	}
}
public class BasedPlatformIcon : RecMeshCreate
{
	public enum PointIndex { LeftUpPoint = 0, RightUpPoint = 1, RightDownPoint = 2, LeftDownPoint = 3, };

	public float platformHeight;
	public float platformTopWidth;
	public float platformButtonWidth;

	public float initPlatformHeight;
	public float initPlatformTopWidth;
	public float initPlatformButtonWidth;

	public BasedPlatformBalustradeIcon basedPlatformBalustradeIcon = null;
	public BasedPlatformStairIcon basedPlatformStairIcon = null;
	public void BasedPlatformIconCreate<T>(T thisGameObject, string objName, GameObject leftUpPoint, GameObject rightUpPoint, GameObject rightDownPoint, GameObject leftDownPoint)
	where T : Component
	{
		InitBodySetting("BasedPlatformIcon", (int)BodyType.GeneralBody);
		InitIconMenuButtonSetting();

		this.rightUpPoint = rightUpPoint;
		this.rightDownPoint = rightDownPoint;
		this.leftUpPoint = leftUpPoint;
		this.leftDownPoint = leftDownPoint;


		initPlatformHeight = platformHeight = rightUpPoint.transform.position.y - rightDownPoint.transform.position.y;
		initPlatformTopWidth = platformTopWidth = (rightUpPoint.transform.position.x - leftUpPoint.transform.position.x);
		initPlatformButtonWidth = platformButtonWidth = (rightDownPoint.transform.position.x - leftDownPoint.transform.position.x);

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
	public Vector3 ClampPos(Vector3 inputPos, GameObject chooseObj)
	{
		float minClampX = float.MinValue;
		float maxClampX = float.MaxValue;
		float minClampY = float.MinValue;
		float maxClampY = float.MaxValue;

		float minWidth = (initPlatformTopWidth) * 0.4f;
		float minHeight = (initPlatformHeight) * 0.4f;

		if (chooseObj == controlPointList[(int)PointIndex.LeftUpPoint])
		{
			if (basedPlatformBalustradeIcon != null)
				maxClampX = basedPlatformBalustradeIcon.leftUpPoint.transform.position.x;
			else
				maxClampX = controlPointList[(int)PointIndex.RightUpPoint].transform.position.x - minWidth;
			minClampY = controlPointList[(int)PointIndex.LeftDownPoint].transform.position.y + minHeight;
		}
		else if (chooseObj == controlPointList[(int)PointIndex.LeftDownPoint])
		{
			maxClampX = controlPointList[(int)PointIndex.RightDownPoint].transform.position.x - minWidth;
			maxClampY = controlPointList[(int)PointIndex.LeftUpPoint].transform.position.y - minHeight;
		}
		else if (chooseObj == controlPointList[(int)PointIndex.RightUpPoint])
		{
			if (basedPlatformBalustradeIcon != null)
				minClampX = basedPlatformBalustradeIcon.rightUpPoint.transform.position.x;
			else
				minClampX = controlPointList[(int)PointIndex.LeftUpPoint].transform.position.x + minWidth;

			minClampY = controlPointList[(int)PointIndex.RightDownPoint].transform.position.y + minHeight;

		}
		else if (chooseObj == controlPointList[(int)PointIndex.RightDownPoint])
		{
			minClampX = controlPointList[(int)PointIndex.LeftDownPoint].transform.position.x + minWidth;
			maxClampY = controlPointList[(int)PointIndex.RightUpPoint].transform.position.y - minHeight;
		}

		float posX = Mathf.Clamp(inputPos.x, minClampX, maxClampX);
		float posY = Mathf.Clamp(inputPos.y, minClampY, maxClampY);
		return new Vector3(posX, posY, inputPos.z);
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
			if (basedPlatformBalustradeIcon != null)
			{
				for (int i = 0; i < basedPlatformBalustradeIcon.rightColumn.controlPointList.Count; i++)
				{
					basedPlatformBalustradeIcon.rightColumn.controlPointList[i].transform.position = new Vector3(basedPlatformBalustradeIcon.rightColumn.controlPointList[i].transform.position.x, basedPlatformBalustradeIcon.rightColumn.controlPointList[i].transform.position.y + (offset_y), basedPlatformBalustradeIcon.rightColumn.controlPointList[i].transform.position.z);
					basedPlatformBalustradeIcon.leftColumn.controlPointList[i].transform.position = new Vector3(basedPlatformBalustradeIcon.leftColumn.controlPointList[i].transform.position.x, basedPlatformBalustradeIcon.leftColumn.controlPointList[i].transform.position.y + (offset_y), basedPlatformBalustradeIcon.leftColumn.controlPointList[i].transform.position.z);

				}
			}
		}
		if (basedPlatformStairIcon != null)
		{
			basedPlatformStairIcon.AdjPos(leftUpPoint, rightUpPoint, rightDownPoint, leftDownPoint);
			basedPlatformStairIcon.AdjMesh();
			basedPlatformStairIcon.UpdateLastPos();
		}
		platformHeight = rightUpPoint.transform.position.y - rightDownPoint.transform.position.y;
		platformTopWidth = (rightUpPoint.transform.position.x - leftUpPoint.transform.position.x);
		platformButtonWidth = (rightDownPoint.transform.position.x - leftDownPoint.transform.position.x);
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
	public override void InitIconMenuButtonUpdate()
	{
		if (basedPlatformBalustradeIcon != null) basedPlatformBalustradeIcon.InitIconMenuButtonUpdate();
		if (basedPlatformStairIcon != null) basedPlatformStairIcon.InitIconMenuButtonUpdate();
	}
}
public class platform2icon : MonoBehaviour
{
	public enum PointIndex { LeftUpPoint = 0, RightUpPoint = 1, RightDownPoint = 2, LeftDownPoint = 3, LeftMidPoint = 4, RightMidPoint = 5, };
	public List<GameObject> controlPointList = new List<GameObject>();


	private DragItemController dragitemcontroller;
	private Movement movement;

	[HideInInspector]
	[SerializeField]
	public BasedPlatformIcon basedPlatformIcon;
	[HideInInspector]
	[SerializeField]
	public CurvePlatformIcon curvePlatformIcon;
	public float stairWidth;
	//for ratio
	public float platTopWidthDis;
	public float ini_platTopWidthDis;
	public float platButtomWidthDis;
	public float ini_platButtomWidthDis;
	public float platHeightDis;
	public float ini_platHeightDis;
	public Vector2 platBalustradeDis;
	public Vector2 ini_platBalustradeDis;

	public bool isBasedPlatformBalustrade;
	public bool isBasedPlatformStair;

	void Awake()
	{
		dragitemcontroller = GameObject.Find("DragItemController").GetComponent<DragItemController>();
		movement = GameObject.Find("Movement").GetComponent<Movement>();


		switch (gameObject.tag)
		{
			case "CurvePlatformIcon"://specialCase
				curvePlatformIcon = CreateCurvePlatformIcon();
				break;
			case "BasedPlatformIcon"://specialCase
				basedPlatformIcon = CreateBasedPlatformIcon();
				break;
		}
	}
	private CurvePlatformIcon CreateCurvePlatformIcon()
	{
		CurvePlatformIcon curvePlatformIcon = new CurvePlatformIcon();

		curvePlatformIcon.CurvePlatformIconCreate(this, "CurvePlatformIcon", controlPointList[(int)PointIndex.LeftUpPoint], controlPointList[(int)PointIndex.RightUpPoint], controlPointList[(int)PointIndex.RightDownPoint], controlPointList[(int)PointIndex.LeftDownPoint], controlPointList[(int)PointIndex.LeftMidPoint], controlPointList[(int)PointIndex.RightMidPoint]);

		ini_platTopWidthDis = (controlPointList[(int)PointIndex.RightUpPoint].transform.position.x - controlPointList[(int)PointIndex.LeftUpPoint].transform.position.x);
		ini_platButtomWidthDis = (controlPointList[(int)PointIndex.RightDownPoint].transform.position.x - controlPointList[(int)PointIndex.LeftDownPoint].transform.position.x);
		ini_platHeightDis = (controlPointList[(int)PointIndex.RightUpPoint].transform.position.y - controlPointList[(int)PointIndex.RightDownPoint].transform.position.y);

		ini_platBalustradeDis.x = ini_platTopWidthDis;
		ini_platBalustradeDis.y = ini_platHeightDis * 0.8f;

		stairWidth = ini_platTopWidthDis * 0.3f;

		return curvePlatformIcon;
	}
	private BasedPlatformIcon CreateBasedPlatformIcon()
	{
		BasedPlatformIcon basedPlatformIcon = new BasedPlatformIcon();

		basedPlatformIcon.BasedPlatformIconCreate(this, "BasedPlatformIcon", controlPointList[(int)PointIndex.LeftUpPoint], controlPointList[(int)PointIndex.RightUpPoint], controlPointList[(int)PointIndex.RightDownPoint], controlPointList[(int)PointIndex.LeftDownPoint]);

		ini_platTopWidthDis = (controlPointList[(int)PointIndex.RightUpPoint].transform.position.x - controlPointList[(int)PointIndex.LeftUpPoint].transform.position.x);
		ini_platButtomWidthDis = (controlPointList[(int)PointIndex.RightDownPoint].transform.position.x - controlPointList[(int)PointIndex.LeftDownPoint].transform.position.x);
		ini_platHeightDis = (controlPointList[(int)PointIndex.RightUpPoint].transform.position.y - controlPointList[(int)PointIndex.RightDownPoint].transform.position.y);

		ini_platBalustradeDis.x = ini_platTopWidthDis;
		ini_platBalustradeDis.y = ini_platHeightDis * 0.8f;

		stairWidth = ini_platTopWidthDis * 0.3f;

		return basedPlatformIcon;
	}
	public void adjPos()
	{
		Vector3 tmp = dragitemcontroller.chooseObj.transform.position;
		GameObject chooseObj = dragitemcontroller.chooseObj;
		switch (gameObject.tag)
		{
			case "CurvePlatformIcon"://specialCase
				break;
			case "BasedPlatformIcon"://specialCase
				if (chooseObj == basedPlatformIcon.rightUpPoint || chooseObj == basedPlatformIcon.leftUpPoint)
				{
					Vector2 offset = basedPlatformIcon.AdjPos(tmp, chooseObj);
					basedPlatformIcon.AdjMesh();

					platTopWidthDis = basedPlatformIcon.platformTopWidth / 2.0f;
					platHeightDis = basedPlatformIcon.platformHeight;
				}
				else if (chooseObj == basedPlatformIcon.rightDownPoint || chooseObj == basedPlatformIcon.leftDownPoint)
				{
					Vector2 offset = basedPlatformIcon.AdjPos(tmp, chooseObj);
					basedPlatformIcon.AdjMesh();

					platButtomWidthDis = basedPlatformIcon.platformButtonWidth / 2.0f;
					platHeightDis = basedPlatformIcon.platformHeight;
				}
				if (isBasedPlatformBalustrade)
				{
					if (chooseObj == basedPlatformIcon.basedPlatformBalustradeIcon.rightColumn.body || chooseObj == basedPlatformIcon.basedPlatformBalustradeIcon.leftColumn.body || chooseObj == basedPlatformIcon.basedPlatformBalustradeIcon.rightUpPoint || chooseObj == basedPlatformIcon.basedPlatformBalustradeIcon.leftUpPoint || chooseObj == basedPlatformIcon.basedPlatformBalustradeIcon.rightDownPoint || chooseObj == basedPlatformIcon.basedPlatformBalustradeIcon.leftDownPoint)
					{
						basedPlatformIcon.basedPlatformBalustradeIcon.AdjPos(tmp, chooseObj);
					}

					platBalustradeDis.x = basedPlatformIcon.basedPlatformBalustradeIcon.balustradeColumnWidth / 2.0f;
					platBalustradeDis.y = basedPlatformIcon.basedPlatformBalustradeIcon.balustradeColumnHeight;
				}
				break;
		}

	}
	public void DestroyFunction(string objName)
	{
		switch (objName)
		{
			case "BasePlatFormBalustrade":
				isBasedPlatformBalustrade = false;
				basedPlatformIcon.basedPlatformBalustradeIcon = null;
				break;
			case "BasedPlatformStair":
				isBasedPlatformStair = false;
				basedPlatformIcon.basedPlatformStairIcon = null;
				break;
		}
	}
	public void UpdateFunction(string objName, GameObject correspondingDragItemObject)
	{
		switch (objName)
		{
			case "BasePlatFormBalustrade":
				if (basedPlatformIcon.basedPlatformBalustradeIcon == null)
				{
					isBasedPlatformBalustrade = true;
					basedPlatformIcon.CreateBasedPlatformBalustrade(this, "BasedPlatformBalustradeIcon", ini_platBalustradeDis.y, correspondingDragItemObject);
				}
				break;
			case "BasedPlatformStair":
				if (basedPlatformIcon.basedPlatformStairIcon == null)
				{
					isBasedPlatformStair = true;
					basedPlatformIcon.CreateBasedPlatformStair(this, "BasedPlatformStairIcon", stairWidth, correspondingDragItemObject);
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
		GameObject chooseObj = dragitemcontroller.chooseObj;
		switch (gameObject.tag)
		{
			case "CurvePlatformIcon"://specialCase
				curvePlatformIcon = CreateCurvePlatformIcon();
				break;
			case "BasedPlatformIcon"://specialCase
				if (basedPlatformIcon.basedPlatformBalustradeIcon != null)
				{
					foreach (GameObject controlPoint in basedPlatformIcon.basedPlatformBalustradeIcon.controlPointList)
					{
						if (chooseObj == controlPoint)
							return basedPlatformIcon.basedPlatformBalustradeIcon.ClampPos(inputPos, chooseObj);
					}
				}

				foreach (GameObject controlPoint in controlPointList)
				{
					if (chooseObj == controlPoint)
						return basedPlatformIcon.ClampPos(inputPos, chooseObj);
				}
				break;
		}

		return inputPos;
	}
	public void IconUpdate()
	{

		switch (gameObject.tag)
		{
			case "CurvePlatformIcon"://specialCase
				break;
			case "BasedPlatformIcon"://specialCase
				basedPlatformIcon.InitIconMenuButtonUpdate();
				break;
		}
	}
}