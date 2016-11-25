/*
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
public class BasedPlatformBalustradeIcon : DecorateIconObject
{
	public enum PointIndex { LeftUpPoint = 0, RightUpPoint = 1, RightDownPoint = 2, LeftDownPoint = 3, };
	public List<GameObject> body = new List<GameObject>();
	public Column leftColumn;
	public Column rightColumn;
	public GameObject rightUpPoint;
	public GameObject rightDownPoint;
	public GameObject leftUpPoint;
	public GameObject leftDownPoint;
	public float columnHeight;
	private float offset=0.01f;
	public void BasedPlatformBalustradeIconCreate<T>(T thisGameObject, string objName, BasedPlatformIcon basedPlatformIcon, float columnHeight, GameObject correspondingDragItemObject) where T : Component
	{
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
		
		body.Add(leftColumn.body);
		body.Add(rightColumn.body);


		mFilter.mesh = CreatRecMesh(leftUpPoint.transform.position, rightUpPoint.transform.position, rightDownPoint.transform.position, leftDownPoint.transform.position, mFilter.mesh);
		//初始位置
		controlPointList.Add(leftColumn.upPoint);
		controlPointList.Add(rightColumn.upPoint);
		controlPointList.Add(rightColumn.downPoint);
		controlPointList.Add(leftColumn.downPoint);
		InitControlPointList2lastControlPointPosition();


		InitLineRender(thisGameObject);
		SetIconObjectColor();
		SetParent2BodyAndControlPointList(thisGameObject);

		InitDecorateIconObjectSetting(correspondingDragItemObject);

	}

}
public class BasedPlatformIcon : RecMeshCreate
{
	public enum PointIndex { LeftUpPoint = 0, RightUpPoint = 1, RightDownPoint = 2, LeftDownPoint = 3, };
	public GameObject rightUpPoint;
	public GameObject rightDownPoint;
	public GameObject leftUpPoint;
	public GameObject leftDownPoint;

	public BasedPlatformBalustradeIcon basedPlatformBalustradeIcon=new BasedPlatformBalustradeIcon();
	public void BasedPlatformIconCreate<T>(T thisGameObject, string objName, GameObject leftUpPoint, GameObject rightUpPoint, GameObject rightDownPoint, GameObject leftDownPoint)
	where T : Component
	{
		InitBodySetting("BasedPlatformIcon",(int)BodyType.GeneralBody);

		this.rightUpPoint = rightUpPoint;
		this.rightDownPoint = rightDownPoint;
		this.leftUpPoint = leftUpPoint;
		this.leftDownPoint = leftDownPoint;

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
	public Vector3 AdjPos(Vector3 tmp, int index)
	{
		float offset_x = tmp.x - lastControlPointPosition[index].x;
		float offset_y = tmp.y - lastControlPointPosition[index].y;
		for (int j = 0; j < controlPointList.Count; j++)
		{
			if (index == j) continue;
	/*		if ((lastControlPointPosition[index].x == controlPointList[j].transform.position.x))//x相同
			{
				controlPointList[j].transform.position = new Vector3(tmp.x, lastControlPointPosition[j].y - (offset_y), lastControlPointPosition[j].z);
			}
			else if ((lastControlPointPosition[index].y == controlPointList[j].transform.position.y))//y相同
			{
				controlPointList[j].transform.position = new Vector3(lastControlPointPosition[j].x - (offset_x), tmp.y, lastControlPointPosition[j].z);
			}
			else//對角
			{
				controlPointList[j].transform.position = new Vector3(lastControlPointPosition[j].x - (offset_x), lastControlPointPosition[j].y - (offset_y), lastControlPointPosition[j].z);
			}*/
			if ((lastControlPointPosition[index].y == controlPointList[j].transform.position.y))//y相同
			{
				controlPointList[j].transform.position = new Vector3(lastControlPointPosition[j].x - (offset_x), tmp.y, lastControlPointPosition[j].z);
			}
		}
		  UpdateLastPos();
		  UpdateLineRender();
		return new Vector3(offset_x, offset_y,0);
	}
}
public class platform2icon : MonoBehaviour
{
	public enum PointIndex { LeftUpPoint = 0, RightUpPoint = 1, RightDownPoint = 2, LeftDownPoint = 3, };
	public List<GameObject> controlPointList = new List<GameObject>();


	private DragItemController dragitemcontroller;
	private Movement movement;

	BasedPlatformIcon basedPlatformIcon;

	//for ratio
	public Vector2 chang_platdis;
	public Vector2 ini_platdis;

	public Vector2 ini_platBalustradeDis;

	void Start()
	{
		dragitemcontroller = GameObject.Find("DragItemController").GetComponent<DragItemController>();
		movement = GameObject.Find("Movement").GetComponent<Movement>();

		basedPlatformIcon=CreateBasedPlatformIcon();
	}
	BasedPlatformIcon CreateBasedPlatformIcon()
	{
		BasedPlatformIcon basedPlatformIcon = new BasedPlatformIcon();

		basedPlatformIcon.BasedPlatformIconCreate(this, "BasedPlatformIcon_mesh", controlPointList[(int)PointIndex.LeftUpPoint], controlPointList[(int)PointIndex.RightUpPoint], controlPointList[(int)PointIndex.RightDownPoint], controlPointList[(int)PointIndex.LeftDownPoint]);

		ini_platdis.x = (controlPointList[(int)PointIndex.RightUpPoint].transform.position.x - controlPointList[(int)PointIndex.LeftUpPoint].transform.position.x);
		ini_platdis.y = (controlPointList[(int)PointIndex.RightUpPoint].transform.position.y - controlPointList[(int)PointIndex.RightDownPoint].transform.position.y);

		return basedPlatformIcon;
	}
	public void adjPos()
	{
		for (int i = 0; i < controlPointList.Count; i++)
		{
			if (dragitemcontroller.chooseObj == controlPointList[i])
			{
				Vector3 tmp = dragitemcontroller.chooseObj.transform.position;
				Vector2 offset = basedPlatformIcon.AdjPos(tmp, i);
				basedPlatformIcon.AdjMesh();

				chang_platdis.x = offset.x;
				chang_platdis.y = offset.y;

			}
		}
	}
	public void UpdateFunction(string objName, GameObject correspondingDragItemObject)
	{
		switch (objName)
		{
			case "BasePlatFormBalustrade":
				if (basedPlatformIcon.basedPlatformBalustradeIcon.body == null)
				{
					basedPlatformIcon.basedPlatformBalustradeIcon.BasedPlatformBalustradeIconCreate(this, "basedPlatformBalustradeIcon_mesh", basedPlatformIcon, ini_platBalustradeDis.y, correspondingDragItemObject);
				}
				break;
		}
	}
	public void addpoint()
	{
		movement.freelist.AddRange(controlPointList);
	}
	public Vector3 ClampPos(Vector3 inputPos)
	{
		float minClampX = float.MinValue;
		float maxClampX = float.MaxValue;
		float minClampY = float.MinValue;
		float maxClampY = float.MaxValue;
	
			float minWidth = (ini_platdis.x) * 0.4f;
			float minHeight = (ini_platdis.y) * 0.4f;

			if (dragitemcontroller.chooseObj == controlPointList[(int)PointIndex.LeftUpPoint])
		{
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
			minClampX = controlPointList[(int)PointIndex.LeftUpPoint].transform.position.x + minWidth;
			minClampY = controlPointList[(int)PointIndex.RightDownPoint].transform.position.y + minHeight;
		}
			else if (dragitemcontroller.chooseObj == controlPointList[(int)PointIndex.RightDownPoint])
		{
			minClampX = controlPointList[(int)PointIndex.LeftUpPoint].transform.position.x + minWidth;
			maxClampY = controlPointList[(int)PointIndex.RightUpPoint].transform.position.y - minHeight;
		}
		float posX = Mathf.Clamp(inputPos.x, minClampX, maxClampX);
		float posY = Mathf.Clamp(inputPos.y, minClampY, maxClampY);
		return new Vector3(posX, posY, inputPos.z);
		
		return inputPos;
	}
}