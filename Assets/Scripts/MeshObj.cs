using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class lineRendererControl
{
	public List<lineStruct> lineRenderList = new List<lineStruct>();
	float lineWidth = 0.01f;
	public struct lineStruct
	{
		public GameObject startControlPoint;
		public GameObject endControlPoint;
		public LineRenderer lineRenderer;
		public GameObject lineObj;
	}
	public void CreateLineRenderer<T>(T thisGameObject, GameObject strat, GameObject end) where T : Component
	{
		GameObject lineObj = new GameObject("Line", typeof(LineRenderer));
		lineObj.transform.parent = thisGameObject.transform;
		LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();
		lineRenderer.sortingOrder = 1;
		lineRenderer.SetWidth(lineWidth, lineWidth);
		lineRenderer.useWorldSpace = true;
		lineRenderer.material.color = Color.black;
		lineRenderer.SetColors(Color.black, Color.black);
		lineRenderer.SetVertexCount(2);
		lineRenderer.SetPosition(0, strat.transform.position);
		lineRenderer.SetPosition(1, end.transform.position);

		lineStruct tmp = new lineStruct();
		tmp.startControlPoint = strat;
		tmp.endControlPoint = end;
		tmp.lineRenderer = lineRenderer;
		tmp.lineObj = lineObj;
		lineRenderList.Add(tmp);
	}
	public void CreateLineRenderer<T>(T thisGameObject, Vector3 strat, Vector3 end) where T : Component
	{
		GameObject lineObj = new GameObject("Line", typeof(LineRenderer));
		lineObj.transform.parent = thisGameObject.transform;
		LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();
		lineRenderer.sortingOrder = 1;
		lineRenderer.SetWidth(lineWidth, lineWidth);
		lineRenderer.useWorldSpace = true;
		lineRenderer.material.color = Color.black;
		lineRenderer.SetColors(Color.black, Color.black);
		lineRenderer.SetVertexCount(2);
		lineRenderer.SetPosition(0, strat);
		lineRenderer.SetPosition(1, end);

		lineStruct tmp = new lineStruct();
		tmp.lineRenderer = lineRenderer;
		tmp.lineObj = lineObj;
		lineRenderList.Add(tmp);
	}
	public void AdjLineRenderer(int index, GameObject strat, GameObject end)
	{
		lineRenderList[index].lineRenderer.SetPosition(0, strat.transform.position);
		lineRenderList[index].lineRenderer.SetPosition(1, end.transform.position);
	}
	public void AdjLineRenderer(int index, Vector3 strat, Vector3 end)
	{
		lineRenderList[index].lineRenderer.SetPosition(0, strat);
		lineRenderList[index].lineRenderer.SetPosition(1, end);
	}
	public void SetParent2LineRenderList<T>(T thisGameObject)
	where T : Component
	{
		for (int i = 0; i < lineRenderList.Count; i++)
		{
			lineRenderList[i].lineObj.transform.parent = thisGameObject.transform;
		}
	}
	public void SetParent2LineRenderList(GameObject thisGameObject)
	{
		for (int i = 0; i < lineRenderList.Count; i++)
		{
			lineRenderList[i].lineObj.transform.parent = thisGameObject.transform;
		}
	}
	public virtual void UpdateLineRender() { }
	public virtual void InitLineRender<T>(T thisGameObject) where T : Component { }

}

public class IconObject : lineRendererControl
{	public enum PointIndex { LeftUpPoint = 0, RightUpPoint = 1, RightDownPoint = 2, LeftDownPoint = 3, };
	public enum BodyType { GeneralBody = 0, CylinderBody = 1,}
	public List<GameObject> controlPointList = new List<GameObject>();
	public Vector3[] lastControlPointPosition = null;
	public List<Vector3> controlPointList_Vec3_2_LineRender = new List<Vector3>();//用於lineRenderer的controlPoint
	public GameObject body = null;
	public MeshFilter mFilter = null;
	public MeshRenderer mRenderer = null;
	public Collider mCollider = null;
	public Material silhouetteShader = null;

	public IconControl iconMenuControl;
	public IconObject()
	{
		if (Shader.Find("Outlined/Silhouetted Bumped Diffuse"))
			silhouetteShader = new Material(Shader.Find("Outlined/Silhouetted Bumped Diffuse"));

	}
	public virtual void InitIconMenuButtonUpdate(){}
	public void InitIconMenuButtonSetting()
	{
		iconMenuControl.delelteButton.isDeleteIconButton=true;
		iconMenuControl.scrollBarButton.isScrollBarIconButton = false;
	}
	public void InitBodySetting(string objName, int bodyType)
	{
		switch (bodyType)
		{
			case (int)BodyType.GeneralBody:
				body = new GameObject(objName);
				mFilter = body.AddComponent<MeshFilter>();
				mRenderer = body.AddComponent<MeshRenderer>() as MeshRenderer;
				mCollider = body.AddComponent<MeshCollider>() as MeshCollider;
				mFilter.mesh = new Mesh();
				mCollider.GetComponent<MeshCollider>().sharedMesh = mFilter.mesh;
				mRenderer.sortingOrder = 0;
				body.tag = "MeshBodyCollider";
				break;
			case (int)BodyType.CylinderBody:
				body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
				body.name = objName;
				mFilter = body.GetComponent<MeshFilter>();
				mRenderer = body.GetComponent<MeshRenderer>() as MeshRenderer;
				mCollider = body.GetComponent<CapsuleCollider>() as CapsuleCollider;
				mRenderer.sortingOrder = 0;
				body.tag = "MeshBodyCollider";
				break;
		}
		iconMenuControl = body.AddComponent<IconControl>();
	}
	public void InitControlPointList2lastControlPointPosition()
	{
		lastControlPointPosition = new Vector3[controlPointList.Count];
		for (int i = 0; i < controlPointList.Count; i++)
		{
			lastControlPointPosition[i] = controlPointList[i].transform.position;
		}
	}
	//縮放
	public Vector3 AdjPos(Vector3 tmp, int index, Vector3 center)
	{
		Vector3[] points2Center = new Vector3[controlPointList.Count];

		for (int i = 0; i < controlPointList.Count; i++)
		{
			points2Center[i] = lastControlPointPosition[i] - center;
		}

		Vector3 a = tmp - center;//now
		Vector3 b = points2Center[index];//before
		float aa = a.magnitude;
		float bb = b.magnitude;
		float cc = aa / bb;     //ratio
		for (int j = 0; j < controlPointList.Count; j++)
		{
			controlPointList[j].transform.localPosition = points2Center[j] * cc;
		}

		Vector3 offset = tmp - lastControlPointPosition[index];

		UpdateLastPos();
		UpdateLineRender();
		return offset;

	}
	public void SetIconObjectColor()
	{
		if (mRenderer != null) mRenderer.material.color = Color.red;
		foreach (GameObject controlPoint in controlPointList)
		{
			if (silhouetteShader != null)
				controlPoint.GetComponent<MeshRenderer>().material = silhouetteShader;

			controlPoint.GetComponent<MeshRenderer>().material.color = Color.yellow;

		}
	}
	public void AdjMesh()
	{
		mFilter.mesh.vertices = lastControlPointPosition;
		mFilter.mesh.RecalculateBounds();
		mFilter.mesh.RecalculateNormals();

		UpdateCollider();
	}
	public void UpdateLastPos()
	{
	Debug.Log("njfdjm");
		if (lastControlPointPosition==null)return;
		if (lastControlPointPosition.Length < controlPointList.Count)
		{
			lastControlPointPosition = new Vector3[controlPointList.Count];
		}
		for (int i = 0; i < controlPointList.Count; i++)
		{
			lastControlPointPosition[i] = controlPointList[i].transform.position;
		}
		Debug.Log("okoko");
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
			AdjLineRenderer(i, lineRenderList[i].startControlPoint, lineRenderList[i].endControlPoint);
		}
	}
	public void UpdateCollider()
	{
		mCollider.GetComponent<MeshCollider>().sharedMesh = mFilter.mesh;
	}
	public GameObject CreateControlPoint(string objName, Vector3 localScale, Vector3 pos)
	{
		GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		obj.tag = "ControlPoint";
		obj.name = objName;
		obj.transform.localScale = localScale;
		obj.transform.position = pos;
		return obj;
	}
	public void SetParent2BodyAndControlPointList<T>(T thisGameObject)
	where T : Component
	{
		if (body != null) body.transform.parent = thisGameObject.transform;
		for (int i = 0; i < controlPointList.Count; i++)
		{
			controlPointList[i].transform.parent = thisGameObject.transform;
		}
	}
	public void SetParent2BodyAndControlPointList(GameObject thisGameObject)
	{
		if (body != null) body.transform.parent = thisGameObject.transform;
		for (int i = 0; i < controlPointList.Count; i++)
		{
			controlPointList[i].transform.parent = thisGameObject.transform;
		}
	}
}
public class VerandaIcon : IconObject//廡殿頂
{
	public enum PointIndex { LeftUpPoint = 0, RightUpPoint = 1, RightDownPoint = 2, LeftDownPoint = 3, RightMainRidgePoint = 4, LeftMainRidgePoint = 5, };
	public int edgeIndex = 4;
	public float initVerandaIconWidth;
	public float initVerandaIconHeight;
	public float initMainRidgeWidth;
	public float verandaIconWidth;
	public float verandaIconHeight;
	public float mainRidgeWidth;
	public void VerandaIconCreate<T>(T thisGameObject, string objName, List<GameObject> controlPointList) where T : Component
	{
		InitBodySetting(objName, (int)BodyType.GeneralBody);
		InitIconMenuButtonSetting();
		this.controlPointList = controlPointList;
		InitControlPointList2lastControlPointPosition();

		verandaIconWidth = initVerandaIconWidth = controlPointList[(int)PointIndex.RightUpPoint].transform.position.x - controlPointList[(int)PointIndex.LeftUpPoint].transform.position.x;
		verandaIconHeight = initVerandaIconHeight = controlPointList[(int)PointIndex.RightUpPoint].transform.position.y - controlPointList[(int)PointIndex.RightDownPoint].transform.position.y;

		mainRidgeWidth = initMainRidgeWidth = controlPointList[(int)PointIndex.RightMainRidgePoint].transform.position.x - controlPointList[(int)PointIndex.LeftMainRidgePoint].transform.position.x;

		mFilter.mesh.vertices = new Vector3[] {
					controlPointList [0].transform.position,
					controlPointList [1].transform.position,
					controlPointList [2].transform.position,
					controlPointList [3].transform.position,
					controlPointList [4].transform.position,
					controlPointList [5].transform.position
					};
		mFilter.mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
		mFilter.mesh.RecalculateNormals();

		SetParent2BodyAndControlPointList(thisGameObject);
		InitLineRender(thisGameObject);
		SetIconObjectColor();
	}
	public Vector3 ClampPos(Vector3 inputPos, int index)
	{
		float minClampX = float.MinValue;
		float maxClampX = float.MaxValue;
		float minClampY = float.MinValue;
		float maxClampY = float.MaxValue;

		float minWidth = initVerandaIconWidth * 0.8f;
		float minHeight = initVerandaIconHeight * 0.5f;
		switch (index)
		{
			case (int)PointIndex.RightMainRidgePoint://rightMainRidge
				minClampX = controlPointList[(int)PointIndex.LeftMainRidgePoint].transform.position.x;
				maxClampX = controlPointList[(int)PointIndex.RightUpPoint].transform.position.x;
				break;
			case (int)PointIndex.LeftMainRidgePoint://leftMainRidge
				minClampX = controlPointList[(int)PointIndex.LeftUpPoint].transform.position.x;
				maxClampX = controlPointList[(int)PointIndex.RightMainRidgePoint].transform.position.x;
				break;
			case (int)PointIndex.LeftUpPoint://upLeft
				maxClampX = controlPointList[(int)PointIndex.LeftMainRidgePoint].transform.position.x;
				minClampY = controlPointList[(int)PointIndex.LeftMainRidgePoint].transform.position.y + minHeight;
				break;
			case (int)PointIndex.LeftDownPoint://downLeft
				maxClampX = controlPointList[(int)PointIndex.LeftMainRidgePoint].transform.position.x;
				maxClampY = controlPointList[(int)PointIndex.LeftMainRidgePoint].transform.position.y - minHeight;
				break;
			case (int)PointIndex.RightUpPoint://upRight
				minClampX = controlPointList[(int)PointIndex.RightMainRidgePoint].transform.position.x;
				minClampY = controlPointList[(int)PointIndex.RightMainRidgePoint].transform.position.y + minHeight;
				break;
			case (int)PointIndex.RightDownPoint://downRight
				minClampX = controlPointList[(int)PointIndex.RightMainRidgePoint].transform.position.x;
				maxClampY = controlPointList[(int)PointIndex.RightMainRidgePoint].transform.position.y - minHeight;
				break;
		}

		float posX = Mathf.Clamp(inputPos.x, minClampX, maxClampX);
		float posY = Mathf.Clamp(inputPos.y, minClampY, maxClampY);
		return new Vector3(posX, posY, inputPos.z);
	}
	public void AdjPos(Vector3 tmp, int index)
	{
		float offset_x = tmp.x - lastControlPointPosition[index].x;
		float offset_y = tmp.y - lastControlPointPosition[index].y;
		if (index < 4)
		{
			for (int j = 0; j < controlPointList.Count - 2; j++)
			{
				if (index == j) continue;
				if ((lastControlPointPosition[index].x == lastControlPointPosition[j].x))//x一樣的點
				{
					controlPointList[j].transform.position = new Vector3(tmp.x, lastControlPointPosition[j].y - (offset_y), lastControlPointPosition[j].z);
				}
				else if ((lastControlPointPosition[index].y == lastControlPointPosition[j].y))//y一樣的點
				{
					controlPointList[j].transform.position = new Vector3(lastControlPointPosition[j].x - (offset_x), tmp.y, lastControlPointPosition[j].z);
				}
				else//對角的點
				{
					controlPointList[j].transform.position = new Vector3(lastControlPointPosition[j].x - (offset_x), lastControlPointPosition[j].y - (offset_y), lastControlPointPosition[j].z);
				}
			}
			verandaIconWidth = controlPointList[(int)PointIndex.RightUpPoint].transform.position.x - controlPointList[(int)PointIndex.LeftUpPoint].transform.position.x;
			verandaIconHeight =controlPointList[(int)PointIndex.RightUpPoint].transform.position.y - controlPointList[(int)PointIndex.RightDownPoint].transform.position.y;
		}
		else//mainRidge
		{
			if (index == (int)PointIndex.RightMainRidgePoint)
			{
				controlPointList[(int)PointIndex.LeftMainRidgePoint].transform.position = new Vector3(lastControlPointPosition[(int)PointIndex.LeftMainRidgePoint].x - (offset_x), lastControlPointPosition[(int)PointIndex.LeftMainRidgePoint].y, lastControlPointPosition[(int)PointIndex.LeftMainRidgePoint].z);
			}
			else
			{
				controlPointList[(int)PointIndex.RightMainRidgePoint].transform.position = new Vector3(lastControlPointPosition[(int)PointIndex.RightMainRidgePoint].x - (offset_x), lastControlPointPosition[(int)PointIndex.RightMainRidgePoint].y, lastControlPointPosition[(int)PointIndex.RightMainRidgePoint].z);
			}
			mainRidgeWidth  = controlPointList[(int)PointIndex.RightMainRidgePoint].transform.position.x - controlPointList[(int)PointIndex.LeftMainRidgePoint].transform.position.x;
		}
		UpdateLastPos();
		UpdateLineRender();
	}
	public override void InitLineRender<T>(T thisGameObject)
	{
		CreateLineRenderer(thisGameObject, controlPointList[5], controlPointList[4]);
		CreateLineRenderer(thisGameObject, controlPointList[1], controlPointList[4]);
		CreateLineRenderer(thisGameObject, controlPointList[2], controlPointList[4]);
		CreateLineRenderer(thisGameObject, controlPointList[0], controlPointList[5]);
		CreateLineRenderer(thisGameObject, controlPointList[3], controlPointList[5]);

		CreateLineRenderer(thisGameObject, controlPointList[0], controlPointList[1]);
		CreateLineRenderer(thisGameObject, controlPointList[1], controlPointList[2]);
		CreateLineRenderer(thisGameObject, controlPointList[2], controlPointList[3]);
		CreateLineRenderer(thisGameObject, controlPointList[3], controlPointList[0]);
	}
}
public class ShandingIcon : IconObject//歇山頂
{
	public enum PointIndex { LeftUpPoint = 0, RightUpPoint = 1, RightDownPoint = 2, LeftDownPoint = 3, RightMainRidgePoint = 4, LeftMainRidgePoint = 5, LeftUpCenterPoint = 6, RightUpCenterPoint = 7, RightDownCenterPoint = 8, LeftDownCenterPoint = 9, };
	public int edgeIndex = 4;
	float initShandingIconWidth;
	float initShandingIconHeight;
	public void ShandingIconCreate<T>(T thisGameObject, string objName, List<GameObject> controlPointList) where T : Component
	{
		InitBodySetting(objName, (int)BodyType.GeneralBody);
		InitIconMenuButtonSetting();

		this.controlPointList = controlPointList;
		InitControlPointList2lastControlPointPosition();

		initShandingIconWidth = controlPointList[(int)PointIndex.RightUpPoint].transform.position.x - controlPointList[(int)PointIndex.LeftUpPoint].transform.position.x;
		initShandingIconHeight = controlPointList[(int)PointIndex.RightUpPoint].transform.position.y - controlPointList[(int)PointIndex.RightDownPoint].transform.position.y;

		mFilter.mesh.vertices = new Vector3[] {
					controlPointList [0].transform.position,
					controlPointList [1].transform.position,
					controlPointList [2].transform.position,
					controlPointList [3].transform.position,
					controlPointList [4].transform.position,
					controlPointList [5].transform.position,
					controlPointList [6].transform.position,
					controlPointList [7].transform.position,
					controlPointList [8].transform.position,
					controlPointList [9].transform.position,
					};
		mFilter.mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
		mFilter.mesh.RecalculateNormals();

		SetParent2BodyAndControlPointList(thisGameObject);
		InitLineRender(thisGameObject);
		SetIconObjectColor();
	}
	public Vector3 ClampPos(Vector3 inputPos, int index)
	{
		float minClampX = float.MinValue;
		float maxClampX = float.MaxValue;
		float minClampY = float.MinValue;
		float maxClampY = float.MaxValue;

		float minWidth = initShandingIconWidth * 0.8f;
		float minHeight = initShandingIconHeight * 0.5f;

		switch (index)
		{
			case (int)PointIndex.RightMainRidgePoint:
				minClampX = controlPointList[5].transform.position.x + minWidth;
				maxClampX = controlPointList[1].transform.position.x;
			break;
			case (int)PointIndex.LeftMainRidgePoint:
				minClampX = controlPointList[0].transform.position.x;
				maxClampX = controlPointList[4].transform.position.x - minWidth;
			break;
			case (int)PointIndex.LeftUpPoint:
				maxClampX = controlPointList[4].transform.position.x - minWidth;
				minClampY = controlPointList[6].transform.position.y + minHeight;
			break;
			case (int)PointIndex.LeftDownPoint:
				maxClampX = controlPointList[4].transform.position.x - minWidth;
				maxClampY = controlPointList[9].transform.position.y - minHeight;
			break;
			case (int)PointIndex.RightUpPoint:
				minClampX = controlPointList[7].transform.position.x;
				minClampY = controlPointList[7].transform.position.y + minHeight;
			break;
			case (int)PointIndex.RightDownPoint:
				minClampX = controlPointList[8].transform.position.x;
				maxClampY = controlPointList[8].transform.position.y - minHeight;
			break;
			case (int)PointIndex.LeftUpCenterPoint:
				minClampX = controlPointList[0].transform.position.x;
				maxClampX = controlPointList[5].transform.position.x;
				minClampY = controlPointList[5].transform.position.y + minHeight;
				maxClampY = controlPointList[0].transform.position.y - minHeight;
			break;
			case (int)PointIndex.LeftDownCenterPoint:
				minClampX = controlPointList[3].transform.position.x;
				maxClampX = controlPointList[5].transform.position.x;
				minClampY = controlPointList[3].transform.position.y + minHeight;
				maxClampY = controlPointList[5].transform.position.y - minHeight;
			break;
			case (int)PointIndex.RightUpCenterPoint:
				minClampX = controlPointList[5].transform.position.x + minWidth;
				maxClampX = controlPointList[1].transform.position.x;
				minClampY = controlPointList[4].transform.position.y + minHeight;
				maxClampY = controlPointList[1].transform.position.y - minHeight;
			break;
			case (int)PointIndex.RightDownCenterPoint:
				minClampX = controlPointList[5].transform.position.x + minWidth;
				maxClampX = controlPointList[2].transform.position.x;
				minClampY = controlPointList[2].transform.position.y + minHeight;
				maxClampY = controlPointList[4].transform.position.y - minHeight;
			break;

		}
		float posX = Mathf.Clamp(inputPos.x, minClampX, maxClampX);
		float posY = Mathf.Clamp(inputPos.y, minClampY, maxClampY);
		return new Vector3(posX, posY, inputPos.z);
	}
	public Vector3 AdjPos(Vector3 tmp, int index)
	{
		float offset_x = tmp.x - lastControlPointPosition[index].x;
		float offset_y = tmp.y - lastControlPointPosition[index].y;
		if (index < 4)
		{
			for (int j = 0; j < 4; j++)
			{
				if (index == j) continue;
				if ((lastControlPointPosition[index].x == lastControlPointPosition[j].x))//x一樣的點
				{
					controlPointList[j].transform.position = new Vector3(tmp.x, lastControlPointPosition[j].y - (offset_y), lastControlPointPosition[j].z);
				}
				else if ((lastControlPointPosition[index].y == lastControlPointPosition[j].y))//y一樣的點
				{
					controlPointList[j].transform.position = new Vector3(lastControlPointPosition[j].x - (offset_x), tmp.y, lastControlPointPosition[j].z);
				}
				else//對角的點
				{
					controlPointList[j].transform.position = new Vector3(lastControlPointPosition[j].x - (offset_x), lastControlPointPosition[j].y - (offset_y), lastControlPointPosition[j].z);
				}
			}
		}
		else if ((index == 4) || (index == 5))//mainRidge
		{
			if (index == 4)
			{
				controlPointList[7].transform.position = new Vector3(controlPointList[4].transform.position.x, controlPointList[7].transform.position.y, controlPointList[7].transform.position.z);
				controlPointList[8].transform.position = new Vector3(controlPointList[4].transform.position.x, controlPointList[8].transform.position.y, controlPointList[8].transform.position.z);
				controlPointList[5].transform.position = new Vector3(lastControlPointPosition[5].x - (offset_x), lastControlPointPosition[5].y, lastControlPointPosition[5].z);
				controlPointList[6].transform.position = new Vector3(controlPointList[5].transform.position.x, controlPointList[6].transform.position.y, controlPointList[6].transform.position.z);
				controlPointList[9].transform.position = new Vector3(controlPointList[5].transform.position.x, controlPointList[9].transform.position.y, controlPointList[9].transform.position.z);
			}
			else
			{
				controlPointList[6].transform.position = new Vector3(controlPointList[5].transform.position.x, controlPointList[6].transform.position.y, controlPointList[6].transform.position.z);
				controlPointList[9].transform.position = new Vector3(controlPointList[5].transform.position.x, controlPointList[9].transform.position.y, controlPointList[9].transform.position.z);
				controlPointList[4].transform.position = new Vector3(lastControlPointPosition[4].x - (offset_x), lastControlPointPosition[4].y, lastControlPointPosition[4].z);
				controlPointList[7].transform.position = new Vector3(controlPointList[4].transform.position.x, controlPointList[7].transform.position.y, controlPointList[7].transform.position.z);
				controlPointList[8].transform.position = new Vector3(controlPointList[4].transform.position.x, controlPointList[8].transform.position.y, controlPointList[8].transform.position.z);
			}
		}
		else
		{
			for (int j = 6; j < controlPointList.Count; j++)
			{
				if (index == j) continue;
				if ((lastControlPointPosition[index].x == lastControlPointPosition[j].x))//x一樣的點
				{
					controlPointList[j].transform.position = new Vector3(tmp.x, lastControlPointPosition[j].y - (offset_y), lastControlPointPosition[j].z);
				}
				else if ((lastControlPointPosition[index].y == lastControlPointPosition[j].y))//y一樣的點
				{
					controlPointList[j].transform.position = new Vector3(lastControlPointPosition[j].x - (offset_x), tmp.y, lastControlPointPosition[j].z);
				}
				else//對角的點
				{
					controlPointList[j].transform.position = new Vector3(lastControlPointPosition[j].x - (offset_x), lastControlPointPosition[j].y - (offset_y), lastControlPointPosition[j].z);
				}
			}
			controlPointList[4].transform.position = new Vector3(controlPointList[7].transform.position.x, controlPointList[4].transform.position.y, controlPointList[4].transform.position.z);
			controlPointList[5].transform.position = new Vector3(controlPointList[6].transform.position.x, controlPointList[5].transform.position.y, controlPointList[5].transform.position.z);
		}
		UpdateLastPos();
		UpdateLineRender();
		return new Vector3(offset_x, offset_y, 0);
	}
	public override void InitLineRender<T>(T thisGameObject)
	{
		CreateLineRenderer(thisGameObject, controlPointList[5], controlPointList[4]);
		CreateLineRenderer(thisGameObject, controlPointList[7], controlPointList[4]);
		CreateLineRenderer(thisGameObject, controlPointList[8], controlPointList[4]);
		CreateLineRenderer(thisGameObject, controlPointList[6], controlPointList[5]);
		CreateLineRenderer(thisGameObject, controlPointList[9], controlPointList[5]);

		CreateLineRenderer(thisGameObject, controlPointList[0], controlPointList[6]);
		CreateLineRenderer(thisGameObject, controlPointList[3], controlPointList[9]);
		CreateLineRenderer(thisGameObject, controlPointList[1], controlPointList[7]);
		CreateLineRenderer(thisGameObject, controlPointList[2], controlPointList[8]);

		CreateLineRenderer(thisGameObject, controlPointList[0], controlPointList[1]);
		CreateLineRenderer(thisGameObject, controlPointList[1], controlPointList[2]);
		CreateLineRenderer(thisGameObject, controlPointList[2], controlPointList[3]);
		CreateLineRenderer(thisGameObject, controlPointList[3], controlPointList[0]);
	}
}

public class TriangleIcon : IconObject//三角形
{
	public int edgeIndex = 3;
	public void TriangleIconCreate<T>(T thisGameObject, string objName, List<GameObject> controlPointList) where T : Component
	{
		InitBodySetting(objName, (int)BodyType.GeneralBody);
		InitIconMenuButtonSetting();

		this.controlPointList = controlPointList;
		InitControlPointList2lastControlPointPosition();

		mFilter.mesh.vertices = new Vector3[] {
				controlPointList [0].transform.position,
				controlPointList [1].transform.position,
				controlPointList [2].transform.position,
			};
		mFilter.mesh.triangles = new int[] { 0, 1, 2 };
		mFilter.mesh.RecalculateNormals();

		SetParent2BodyAndControlPointList(thisGameObject);
		InitLineRender(thisGameObject);
		SetIconObjectColor();
	}
}
public class RectangleIcon : IconObject//四角形
{
	public int edgeIndex = 4;
	public void RectangleIconCreate<T>(T thisGameObject, string objName, List<GameObject> controlPointList) where T : Component
	{
		InitBodySetting(objName, (int)BodyType.GeneralBody);
		InitIconMenuButtonSetting();

		this.controlPointList = controlPointList;
		InitControlPointList2lastControlPointPosition();

		mFilter.mesh.vertices = new Vector3[] {
				controlPointList [0].transform.position,
				controlPointList [1].transform.position,
				controlPointList [2].transform.position,
				controlPointList [3].transform.position,
			};

		mFilter.mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
		mFilter.mesh.RecalculateNormals();

		SetParent2BodyAndControlPointList(thisGameObject);
		InitLineRender(thisGameObject);
		SetIconObjectColor();
	}
}
public class PentagonIcon : IconObject//五邊形
{
	public int edgeIndex = 5;
	public void PentagonIconCreate<T>(T thisGameObject, string objName, List<GameObject> controlPointList) where T : Component
	{
		InitBodySetting(objName, (int)BodyType.GeneralBody);
		InitIconMenuButtonSetting();

		this.controlPointList = controlPointList;
		InitControlPointList2lastControlPointPosition();


		mFilter.mesh.vertices = new Vector3[] {
				controlPointList [0].transform.position,
				controlPointList [1].transform.position,
				controlPointList [2].transform.position,
				controlPointList [3].transform.position,
				controlPointList [4].transform.position,
			};
		mFilter.mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4 };
		mFilter.mesh.RecalculateNormals();

		SetParent2BodyAndControlPointList(thisGameObject);
		InitLineRender(thisGameObject);
		SetIconObjectColor();
	}
}
public class HexagonIcon : IconObject//六邊形
{
	public int edgeIndex = 6;
	public void HexagonIconCreate<T>(T thisGameObject, string objName, List<GameObject> controlPointList) where T : Component
	{
		InitBodySetting(objName, (int)BodyType.GeneralBody);
		InitIconMenuButtonSetting();

		this.controlPointList = controlPointList;
		InitControlPointList2lastControlPointPosition();

		mFilter.mesh.vertices = new Vector3[] {
					controlPointList [0].transform.position,
					controlPointList [1].transform.position,
					controlPointList [2].transform.position,
					controlPointList [3].transform.position,
					controlPointList [4].transform.position,
					controlPointList [5].transform.position
		};
		mFilter.mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5 };
		mFilter.mesh.RecalculateNormals();

		SetParent2BodyAndControlPointList(thisGameObject);
		InitLineRender(thisGameObject);
		SetIconObjectColor();
	}

}
public class MeshObj : MonoBehaviour
{
	public List<GameObject> controlPointList = new List<GameObject>();

	private VerandaIcon verandaIcon;
	private ShandingIcon shandingIcon;
	private TriangleIcon triangleIcon;
	private RectangleIcon rectangleIcon;
	private PentagonIcon pentagonIcon;
	private HexagonIcon hexagonIcon;

	private DragItemController dragitemcontroller;
	private Movement movement;

	public int edgeIndex;

	public Vector2 ini_bodydis;
	public float ini_mainRidgedis;

	public Vector2 bodydis;
	public float mainRidgedis;


	void Start()
	{
		dragitemcontroller = GameObject.Find("DragItemController").GetComponent<DragItemController>();
	}

	VerandaIcon CreateVerandaIcon()
	{
		VerandaIcon verandaIcon = new VerandaIcon();

		verandaIcon.VerandaIconCreate(this, "VerandaIcon", controlPointList);

		edgeIndex = verandaIcon.edgeIndex;

		ini_bodydis.x =bodydis.x= (controlPointList[1].transform.position.x - controlPointList[0].transform.position.x)/2.0f;
		ini_bodydis.y =bodydis.y= (controlPointList[1].transform.position.y - controlPointList[2].transform.position.y)/2.0f;

		ini_mainRidgedis =mainRidgedis= (controlPointList[4].transform.position.x - controlPointList[5].transform.position.x)/2.0f;

		return verandaIcon;
	}
	ShandingIcon CreateShandingIcon()
	{
		ShandingIcon shandingIcon = new ShandingIcon();
		shandingIcon.ShandingIconCreate(this, "ShandingIcon", controlPointList);

		edgeIndex = shandingIcon.edgeIndex;
		ini_bodydis.x =bodydis.x= (controlPointList[1].transform.position.x - controlPointList[0].transform.position.x)/2.0f;
		ini_bodydis.y =bodydis.y= (controlPointList[1].transform.position.y - controlPointList[2].transform.position.y)/2.0f;

		ini_mainRidgedis =mainRidgedis= (controlPointList[4].transform.position.x - controlPointList[5].transform.position.x)/2.0f;
		return shandingIcon;
	}
	TriangleIcon CreateTriangleIcon()
	{
		TriangleIcon triIcon = new TriangleIcon();

		triIcon.TriangleIconCreate(this, "TiangleIcon", controlPointList);

		edgeIndex = triIcon.edgeIndex;

		return triIcon;
	}
	RectangleIcon CreateRectangleIcon()
	{
		RectangleIcon rectIcon = new RectangleIcon();

		rectIcon.RectangleIconCreate(this, "RectangleIcon", controlPointList);

		edgeIndex = rectIcon.edgeIndex;
		return rectIcon;
	}
	PentagonIcon CreatePentagonIcon()
	{
		PentagonIcon pentaIcon = new PentagonIcon();

		pentaIcon.PentagonIconCreate(this, "PentagonIcon", controlPointList);

		edgeIndex = pentaIcon.edgeIndex;
		return pentaIcon;
	}
	HexagonIcon CreateHexagonIcon()
	{
		HexagonIcon hexIcon = new HexagonIcon();

		hexIcon.HexagonIconCreate(this, "HexagonIcon", controlPointList);

		edgeIndex = hexIcon.edgeIndex;
		return hexIcon;
	}

	void Awake()
	{
		movement = GameObject.Find("Movement").GetComponent<Movement>();

		switch (gameObject.tag)
		{
			case "VerandaIcon"://specialCase
				verandaIcon = CreateVerandaIcon();
				break;
			case "ShandingIcon"://specialCase
				shandingIcon = CreateShandingIcon();
				break;
			case "TriangleIcon":
				triangleIcon = CreateTriangleIcon();
				break;
			case "RectangleIcon":
				rectangleIcon = CreateRectangleIcon();
				break;
			case "PentagonIcon":
				pentagonIcon = CreatePentagonIcon();
				break;
			case "HexagonIcon":
				hexagonIcon = CreateHexagonIcon();
				break;

		}
		addpoint();
	}

	public void adjPos()
	{
		for (int i = 0; i < controlPointList.Count; i++)
		{
			if (dragitemcontroller.chooseObj == controlPointList[i])
			{
				Vector3 tmp = dragitemcontroller.chooseObj.transform.position;
				Vector3 center = transform.position;
				switch (gameObject.tag)
				{
					case "VerandaIcon"://specialCase
						verandaIcon.AdjPos(tmp, i);
						verandaIcon.AdjMesh();
						bodydis = new Vector2(verandaIcon.verandaIconWidth / 2.0f, verandaIcon.verandaIconHeight/2.0f);
						mainRidgedis=(controlPointList[4].transform.position.x - controlPointList[5].transform.position.x)/2.0f;
						break;
					case "ShandingIcon"://specialCase
						shandingIcon.AdjPos(tmp, i);
						shandingIcon.AdjMesh();
						break;
					case "TriangleIcon":
						triangleIcon.AdjPos(tmp, i, center);
						triangleIcon.AdjMesh();
						break;
					case "RectangleIcon":
						rectangleIcon.AdjPos(tmp, i, center);
						rectangleIcon.AdjMesh();
						break;
					case "PentagonIcon":
						pentagonIcon.AdjPos(tmp, i, center);
						pentagonIcon.AdjMesh();
						break;
					case "HexagonIcon":
						hexagonIcon.AdjPos(tmp, i, center);
						hexagonIcon.AdjMesh();
						break;

				}
				break;

			}
		}
	}
	public void addpoint()
	{
		controlPointList.RemoveAll(GameObject => GameObject == null);
		switch (gameObject.tag)
		{
			case "VerandaIcon"://specialCase
				for (int i = 0; i < controlPointList.Count - 2; i++)
				{
					movement.freelist.Add(controlPointList[i]);
				}
				movement.horlist.Add(controlPointList[4]);
				movement.horlist.Add(controlPointList[5]);
				break;
			case "ShandingIcon"://specialCase
				for (int i = 0; i < 4; i++)
				{
					movement.freelist.Add(controlPointList[i]);
				}
				movement.horlist.Add(controlPointList[4]);
				movement.horlist.Add(controlPointList[5]);
				for (int i = 6; i < 10; i++)
				{
					movement.freelist.Add(controlPointList[i]);
				}
				break;
			default:
				movement.freelist.AddRange(controlPointList);
				break;

		}

	}
	public Vector3 ClampPos(Vector3 inputPos)
	{
		if (this.tag == "VerandaIcon")
		{
			for (int i = 0; i < controlPointList.Count; i++)
			{
				if (dragitemcontroller.chooseObj == controlPointList[i])
				{
					return verandaIcon.ClampPos(inputPos, i);
				}
			}
		}
		else if (this.tag == "ShandingIcon")
		{
			for (int i = 0; i < controlPointList.Count; i++)
			{
				if (dragitemcontroller.chooseObj == controlPointList[i])
				{
					return shandingIcon.ClampPos(inputPos, i);
				}
			}
		}
		return inputPos;
	}
	public void IconUpdate()
	{

	}
}

