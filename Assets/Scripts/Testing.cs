﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoofStruct
{
	public List<GameObject> bodyControlPointList = new List<GameObject>();
	public List<GameObject> tailControlPointList = new List<GameObject>();
	public catline catLine2Body;
	public catline catLine2Tail;
}
public class BasedRoofIcon : IconObject
{
	public RoofStruct rightRoofLine = new RoofStruct();
	public RoofStruct leftRoofLine = new RoofStruct();
	Vector3 centerPos;
	public int numberOfPoints2Body = 10;
	public int numberOfPoints2Tail = 20;
	public int sliceUnit2Body = 1;
	public int sliceUnit2Tail = 1;
	int linerRenderCount2Body = 0;
	int linerRenderCount2Tail = 0;
	public void CreateBasedRoofIcon<T>(T thisGameObject, string objName, List<GameObject> bodyControlPointList, List<GameObject> tailControlPointList, Vector3 centerPos) where T : Component
	{
		InitBodySetting(objName, (int)BodyType.GeneralBody);
		InitIconMenuButtonSetting();

		this.centerPos = centerPos;

		rightRoofLine.bodyControlPointList = bodyControlPointList;
		rightRoofLine.tailControlPointList = tailControlPointList;

		//RightCatmullromLine
		GameObject body2Body = new GameObject("CatLine_Right_Body");
		body2Body.transform.parent = thisGameObject.transform;
		rightRoofLine.catLine2Body = body2Body.AddComponent<catline>();

		GameObject body2Tail = new GameObject("CatLine_Right_Tail");
		body2Tail.transform.parent = thisGameObject.transform;
		rightRoofLine.catLine2Tail = body2Tail.AddComponent<catline>();

		for (int i = 0; i < bodyControlPointList.Count; i++)
		{
			rightRoofLine.catLine2Body.AddControlPoint(bodyControlPointList[i]);
			controlPointList.Add(bodyControlPointList[i]);
		}
		rightRoofLine.catLine2Tail.AddControlPoint(bodyControlPointList[bodyControlPointList.Count-1]);
		for (int i = 0; i < tailControlPointList.Count; i++)
		{
			rightRoofLine.catLine2Tail.AddControlPoint(tailControlPointList[i]);
			controlPointList.Add(tailControlPointList[i]);
		}

		//LeftCatmullromLine
		body2Body = new GameObject("CatLine_Left_Body");
		body2Body.transform.parent = thisGameObject.transform;
		leftRoofLine.catLine2Body = body2Body.AddComponent<catline>();

		body2Tail = new GameObject("CatLine_Left_Tail");
		body2Tail.transform.parent = thisGameObject.transform;
		leftRoofLine.catLine2Tail = body2Tail.AddComponent<catline>();

		for (int i = 0; i < bodyControlPointList.Count; i++)
		{
			GameObject copy = new GameObject();
			copy.transform.parent = thisGameObject.transform;
			copy.transform.position = new Vector3(bodyControlPointList[i].transform.position.x - 2 * (bodyControlPointList[i].transform.position.x - centerPos.x), bodyControlPointList[i].transform.position.y, bodyControlPointList[i].transform.position.z);
			leftRoofLine.bodyControlPointList.Add(copy);
			leftRoofLine.catLine2Body.AddControlPoint(copy);
		}
		leftRoofLine.catLine2Tail.AddControlPoint(leftRoofLine.bodyControlPointList[leftRoofLine.bodyControlPointList.Count - 1]);
		for (int i = 0; i < tailControlPointList.Count; i++)
		{
			GameObject copy = new GameObject();
			copy.transform.parent = thisGameObject.transform;
			copy.transform.position = new Vector3(tailControlPointList[i].transform.position.x - 2 * (tailControlPointList[i].transform.position.x - centerPos.x), tailControlPointList[i].transform.position.y, tailControlPointList[i].transform.position.z);
			leftRoofLine.tailControlPointList.Add(copy);
			leftRoofLine.catLine2Tail.AddControlPoint(copy);
		}
		InitControlPointList2lastControlPointPosition();

		rightRoofLine.catLine2Body.SetLineNumberOfPoints(numberOfPoints2Body);
		rightRoofLine.catLine2Body.ResetCatmullRom();
		rightRoofLine.catLine2Tail.SetLineNumberOfPoints(numberOfPoints2Tail);
		rightRoofLine.catLine2Tail.ResetCatmullRom();
		leftRoofLine.catLine2Body.SetLineNumberOfPoints(numberOfPoints2Body);
		leftRoofLine.catLine2Body.ResetCatmullRom();
		leftRoofLine.catLine2Tail.SetLineNumberOfPoints(numberOfPoints2Tail);
		leftRoofLine.catLine2Tail.ResetCatmullRom();

		SetParent2BodyAndControlPointList(thisGameObject);
		InitLineRender(thisGameObject);

		SetIconObjectColor();


		AdjMesh();
	}
	public void AdjPos(Vector3 tmp,GameObject chooseObj)
	{
		rightRoofLine.catLine2Body.ResetCatmullRom();
		for (int i = 0; i < leftRoofLine.bodyControlPointList.Count; i++)
		{
			leftRoofLine.bodyControlPointList[i].transform.position = new Vector3(rightRoofLine.bodyControlPointList[i].transform.position.x - 2 * (rightRoofLine.bodyControlPointList[i].transform.position.x - centerPos.x), rightRoofLine.bodyControlPointList[i].transform.position.y, rightRoofLine.bodyControlPointList[i].transform.position.z);
		}
		if (chooseObj == rightRoofLine.bodyControlPointList[rightRoofLine.bodyControlPointList.Count-1])
		{
			Vector3 offset=rightRoofLine.bodyControlPointList[rightRoofLine.bodyControlPointList.Count-1].transform.position-lastControlPointPosition[rightRoofLine.bodyControlPointList.Count-1];
			for(int i=0;i<rightRoofLine.tailControlPointList.Count;i++)
			{
				rightRoofLine.tailControlPointList[i].transform.position += offset;
			}
		}
		for (int i = 0; i < leftRoofLine.tailControlPointList.Count; i++)
		{
			leftRoofLine.tailControlPointList[i].transform.position = new Vector3(rightRoofLine.tailControlPointList[i].transform.position.x - 2 * (rightRoofLine.tailControlPointList[i].transform.position.x - centerPos.x), rightRoofLine.tailControlPointList[i].transform.position.y, rightRoofLine.tailControlPointList[i].transform.position.z);
		}
		leftRoofLine.catLine2Body.ResetCatmullRom();
		rightRoofLine.catLine2Tail.ResetCatmullRom();
		leftRoofLine.catLine2Tail.ResetCatmullRom();
		UpdateLastPos();
	}
	public void AdjMesh()
	{
		int innerPointCount = rightRoofLine.catLine2Body.innerPointList.Count;
		float uvR = (rightRoofLine.catLine2Body.innerPointList[rightRoofLine.catLine2Body.innerPointList.Count - 1].x - leftRoofLine.catLine2Body.innerPointList[rightRoofLine.catLine2Body.innerPointList.Count - 1].x) / ((float)innerPointCount * 2);
		Vector3[] v = new Vector3[4 * innerPointCount];
		Vector3[] n = new Vector3[4 * innerPointCount];
		//Vector2[] uv = new Vector2[2 * innerPointCount];
		int[] t = new int[6 * (innerPointCount-1) * 2+6];
		for (int i = 0; i < innerPointCount; i++)
		{
			v[i] = rightRoofLine.catLine2Body.innerPointList[i];
		}
		for (int i = 0; i < innerPointCount; i++)
		{
			v[i + innerPointCount] = leftRoofLine.catLine2Body.innerPointList[i];
		}
		for (int i = 0; i < innerPointCount*2; i++)
		{
			v[i + innerPointCount * 2] = leftRoofLine.catLine2Body.innerPointList[leftRoofLine.catLine2Body.innerPointList.Count - 1] + new Vector3(uvR * (i + 1), 0, 0);
		}
		int index = 0;

		t[index] = 0;
		t[index + 1] = innerPointCount*3;
		t[index + 2] = innerPointCount;
		t[index + 3] = innerPointCount;
		t[index + 4] = innerPointCount*3;
		t[index + 5] = innerPointCount*3 - 1;
		index += 6;

		for (int i = 0; i < innerPointCount - 1; i++)
		{
			t[index] = i;
			t[index + 1] = (i + 1);
			t[index + 2] = (i + 1) + innerPointCount *3;
			t[index + 3] = i;
			t[index + 4] = (i + 1) + innerPointCount * 3;
			t[index + 5] = i + innerPointCount * 3;
			index += 6;
		}
		for (int i = 0; i < innerPointCount - 1; i++)
		{
			t[index] = innerPointCount + i;
			t[index + 1] = innerPointCount *3-1 - i;
			t[index + 2] = innerPointCount + 1 + i;
			t[index + 3] = innerPointCount + 1 + i;
			t[index + 4] = innerPointCount * 3 - 1 - i;
			t[index + 5] = innerPointCount * 3 - 2 - i;
			index += 6;
		}
		for (int i = 0; i < 4 * innerPointCount; i++)
		{
			n[i] = -Vector3.forward;
		}
		mFilter.mesh.Clear();
		mFilter.mesh.vertices = v;
		mFilter.mesh.triangles = t;
		mFilter.mesh.normals = n;

		mFilter.mesh.RecalculateNormals();
		mFilter.mesh.RecalculateBounds();
		//mFilter.mesh.uv = uv;
		/*	int innerPointCount = rightRoofLine.catLine.innerPointList.Count;
			float uvR = (1 / (float)innerPointCount);
			mFilter.mesh.Clear();
			Vector3[] v = new Vector3[2 * innerPointCount];
			Vector3[] n = new Vector3[2 * innerPointCount];
			//Vector2[] uv = new Vector2[2 * innerPointCount];
			int[] t = new int[6 * innerPointCount];

			for (int i = 0; i < rightRoofLine.catLine.innerPointList.Count; i++)
			{
				v[i] = rightRoofLine.catLine.innerPointList[i];
			}
			for (int i = 0; i <leftRoofLine.catLine.innerPointList.Count; i++)
			{
				v[i + rightRoofLine.catLine.innerPointList.Count] = leftRoofLine.catLine.innerPointList[i];
			}
			int index = 0;
			for (int i = 0; i < rightRoofLine.catLine.innerPointList.Count - 1; i++)
			{
				t[index] = i;
				t[index + 1] = (i + 1);
				t[index + 2] = i + rightRoofLine.catLine.innerPointList.Count;
				t[index + 3] = i + rightRoofLine.catLine.innerPointList.Count;
				t[index + 4] = (i + 1);
				t[index + 5] = (i+1) + rightRoofLine.catLine.innerPointList.Count;
				index += 6;
			}
			for (int i = 0; i < 2 * innerPointCount; i++)
			{
				n[i]=-Vector3.forward;
			}
			mFilter.mesh.vertices = v;
			mFilter.mesh.triangles = t;
			mFilter.mesh.normals = n;
			//mFilter.mesh.uv = uv;*/
		UpdateLineRender();
		UpdateCollider();
	}
	public override void InitLineRender<T>(T thisGameObject)
	{
		for (int i = 0; i < rightRoofLine.catLine2Tail.innerPointList.Count-1; i += sliceUnit2Tail)
		{
			i = Mathf.Min(i, rightRoofLine.catLine2Tail.innerPointList.Count - 1);
			CreateLineRenderer(thisGameObject, rightRoofLine.catLine2Tail.innerPointList[i], rightRoofLine.catLine2Tail.innerPointList[Mathf.Min((i + sliceUnit2Tail), rightRoofLine.catLine2Tail.innerPointList.Count - 1)]);
			linerRenderCount2Tail++;
			if (i == rightRoofLine.catLine2Tail.innerPointList.Count - 1) return;
		}
		for (int i = 0; i < leftRoofLine.catLine2Tail.innerPointList.Count-1; i += sliceUnit2Tail)
		{
			i = Mathf.Min(i, leftRoofLine.catLine2Tail.innerPointList.Count - 1);
			CreateLineRenderer(thisGameObject, leftRoofLine.catLine2Tail.innerPointList[i], leftRoofLine.catLine2Tail.innerPointList[Mathf.Min((i + sliceUnit2Tail), leftRoofLine.catLine2Tail.innerPointList.Count - 1)]);
			linerRenderCount2Tail++;
			if (i == leftRoofLine.catLine2Tail.innerPointList.Count - 1) return;
		}
	}
	public override void UpdateLineRender()
	{
		int size = linerRenderCount2Body;
		for (int i = 0; i < size / 2; i++)
		{
			AdjLineRenderer(i, rightRoofLine.catLine2Body.innerPointList[i * sliceUnit2Body], rightRoofLine.catLine2Body.innerPointList[Mathf.Min((i * sliceUnit2Body + sliceUnit2Body),rightRoofLine.catLine2Body.innerPointList.Count - 1 )]);
		}
		for (int i = 0; i < size / 2; i++)
		{
			AdjLineRenderer(i + size / 2, leftRoofLine.catLine2Body.innerPointList[i * sliceUnit2Body], leftRoofLine.catLine2Body.innerPointList[Mathf.Min((i * sliceUnit2Body + sliceUnit2Body), leftRoofLine.catLine2Body.innerPointList.Count - 1)]);
		}
		AdjLineRenderer(size, leftRoofLine.catLine2Body.innerPointList[0], rightRoofLine.catLine2Body.innerPointList[0]);
		AdjLineRenderer(size + 1, leftRoofLine.catLine2Body.innerPointList[leftRoofLine.catLine2Body.innerPointList.Count - 1], rightRoofLine.catLine2Body.innerPointList[rightRoofLine.catLine2Body.innerPointList.Count - 1]);

		size = linerRenderCount2Tail;
		for (int i = 0; i < size / 2; i++)
		{
			AdjLineRenderer(i + linerRenderCount2Body+2, rightRoofLine.catLine2Tail.innerPointList[i * sliceUnit2Tail], rightRoofLine.catLine2Tail.innerPointList[Mathf.Min((i * sliceUnit2Tail + sliceUnit2Tail), rightRoofLine.catLine2Tail.innerPointList.Count - 1)]);
		}
		for (int i = 0; i < size / 2; i++)
		{
			AdjLineRenderer(i + size / 2 + linerRenderCount2Body+2, leftRoofLine.catLine2Tail.innerPointList[i * sliceUnit2Tail], leftRoofLine.catLine2Tail.innerPointList[Mathf.Min((i * sliceUnit2Tail + sliceUnit2Tail), leftRoofLine.catLine2Tail.innerPointList.Count - 1)]);
		}
	}
	public void SetIconObjectColor()
	{
		mRenderer.material.color = Color.red;
		for (int i = 0; i < rightRoofLine.bodyControlPointList.Count; i++)
		{
			if (silhouetteShader != null) 
				rightRoofLine.bodyControlPointList[i].GetComponent<MeshRenderer>().material = silhouetteShader;

			rightRoofLine.bodyControlPointList[i].GetComponent<MeshRenderer>().material.color = Color.yellow;
		}
		for (int i = 0; i < rightRoofLine.tailControlPointList.Count; i++)
		{
			if (silhouetteShader != null)
				 rightRoofLine.tailControlPointList[i].GetComponent<MeshRenderer>().material = silhouetteShader;

			rightRoofLine.tailControlPointList[i].GetComponent<MeshRenderer>().material.color = Color.yellow;
		}
	}
}
public class Testing : MonoBehaviour
{

	public List<GameObject> bodyControlPointList = new List<GameObject>();
	public List<GameObject> tailControlPointList = new List<GameObject>();

	private DragItemController dragitemcontroller;
	private Movement movement;

	private BasedRoofIcon basedRoofIcon;

	public Vector3 ini_ControlPoint_1_Position;



	public Vector3 ControlPoint_1_position;
	public Vector3 ControlPoint_2_position;
	public Vector3 ControlPoint_3_position;
	public Vector3 ControlPoint_4_position;
	public Vector3 ControlPoint_5_position;


	public Vector2 ControlPoint1Move;
	public Vector2 ControlPoint2Move;
	public Vector2 ControlPoint3Move;
	public Vector2 ControlPoint4Move;
	public Vector2 ControlPoint5Move;


	public float RooficonHeight;
	public float RooficonWide;
	void Start()
	{
		dragitemcontroller = GameObject.Find("DragItemController").GetComponent<DragItemController>();
		movement = GameObject.Find("Movement").GetComponent<Movement>();

		RooficonHeight = Mathf.Abs(bodyControlPointList[0].transform.position.y - bodyControlPointList[bodyControlPointList.Count - 1].transform.position.y);
		RooficonWide = Mathf.Abs(bodyControlPointList[0].transform.position.x - bodyControlPointList[bodyControlPointList.Count - 1].transform.position.x);
		ini_ControlPoint_1_Position = bodyControlPointList[0].transform.position;

		basedRoofIcon = new BasedRoofIcon();
		basedRoofIcon.CreateBasedRoofIcon(this, "BasedRoofIcon_mesh", bodyControlPointList, tailControlPointList, ini_ControlPoint_1_Position);

		ControlPoint_1_position = bodyControlPointList[0].transform.position;
		ControlPoint_2_position = bodyControlPointList[1].transform.position;
		ControlPoint_3_position = bodyControlPointList[2].transform.position;

		//tail
		ControlPoint_4_position = tailControlPointList[0].transform.position;
		ControlPoint_5_position = tailControlPointList[1].transform.position;
	}
	public void adjPos()
	{
		Vector3 tmp = dragitemcontroller.chooseObj.transform.position;
		GameObject chooseObj = dragitemcontroller.chooseObj;

		basedRoofIcon.AdjPos(tmp, chooseObj);
		basedRoofIcon.AdjMesh();


		ControlPoint1Move = new Vector2((bodyControlPointList[0].transform.position.x - ControlPoint_1_position.x) / RooficonWide, (bodyControlPointList[0].transform.position.y - ControlPoint_1_position.y) / RooficonHeight);
		ControlPoint2Move = new Vector2((bodyControlPointList[1].transform.position.x - ControlPoint_2_position.x) / RooficonWide, (bodyControlPointList[1].transform.position.y - ControlPoint_2_position.y) / RooficonHeight);
		ControlPoint3Move = new Vector2((bodyControlPointList[2].transform.position.x - ControlPoint_3_position.x) / RooficonWide, (bodyControlPointList[2].transform.position.y - ControlPoint_3_position.y) / RooficonHeight);

		ControlPoint_1_position = bodyControlPointList[0].transform.position;
		ControlPoint_2_position = bodyControlPointList[1].transform.position;
		ControlPoint_3_position = bodyControlPointList[2].transform.position;

		//tail
		ControlPoint_4_position = tailControlPointList[0].transform.position;
		ControlPoint_5_position = tailControlPointList[1].transform.position;
	}

	public void addpoint()
	{
		movement.freelist.AddRange(bodyControlPointList);
		movement.freelist.AddRange(tailControlPointList);

	}

}
