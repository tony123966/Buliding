using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoofStruct 
{
	public GameObject body=null;
	public List<GameObject> bodyControlPointList=new List<GameObject>();
	public List<GameObject> tailControlPointList=new List<GameObject>();
	public catline catLine;
}
public class BasedRoofIcon : IconObject 
{
	public RoofStruct rightRoofLine = new RoofStruct();
	public RoofStruct leftRoofLine = new RoofStruct();
	Vector3 centerPos;
	public void CreateBasedRoofIcon<T>(T thisGameObject, string objName, List<GameObject> bodyControlPointList, List<GameObject> tailControlPointList, Vector3 centerPos) where T : Component
	{
		this.centerPos=centerPos;
		body = new GameObject(objName);
		body.transform.parent = thisGameObject.transform;
		mFilter = body.AddComponent<MeshFilter>();
		mFilter.mesh = new Mesh();
		mRenderer = body.AddComponent<MeshRenderer>() as MeshRenderer;

		rightRoofLine.bodyControlPointList=bodyControlPointList;
		rightRoofLine.tailControlPointList=tailControlPointList;
		
		//RightCatmullromLine
		rightRoofLine.body = new GameObject("CatLine_Right");
		rightRoofLine.body.transform.parent = body.transform;
		rightRoofLine.catLine = rightRoofLine.body.AddComponent<catline>();
		for(int i=0;i<bodyControlPointList.Count;i++)
		{
			rightRoofLine.catLine.AddControlPoint(bodyControlPointList[i]);
		}
		rightRoofLine.catLine.ResetCatmullRom();

		//LeftCatmullromLine
		leftRoofLine.body = new GameObject("CatLine_Left");
		leftRoofLine.body.transform.parent = body.transform;
		leftRoofLine.catLine = leftRoofLine.body.AddComponent<catline>();

		for(int i=0;i<bodyControlPointList.Count;i++)
		{
			GameObject copy = new GameObject();
			copy.transform.parent = leftRoofLine.body.transform;
			copy.transform.position = new Vector3(bodyControlPointList[i].transform.position.x - 2 * (bodyControlPointList[i].transform.position.x - centerPos.x), bodyControlPointList[i].transform.position.y, bodyControlPointList[i].transform.position.z);
			leftRoofLine.bodyControlPointList.Add(copy);
			leftRoofLine.catLine.AddControlPoint(copy);
		}
		for (int i = 0; i < tailControlPointList.Count; i++)
		{
			GameObject copy = new GameObject();
			copy.transform.parent = leftRoofLine.body.transform;
			copy.transform.position = new Vector3(tailControlPointList[i].transform.position.x - 2 * (tailControlPointList[i].transform.position.x - centerPos.x), tailControlPointList[i].transform.position.y, tailControlPointList[i].transform.position.z);
			leftRoofLine.tailControlPointList.Add(copy);
		}

		leftRoofLine.catLine.ResetCatmullRom();
		AdjMesh();
		SetIconObjectColor();
	}
	public void AdjPos()
	{
		rightRoofLine.catLine.ResetCatmullRom();
		for (int i = 0; i < rightRoofLine.bodyControlPointList.Count; i++)
		{
			leftRoofLine.bodyControlPointList[i].transform.position = new Vector3(rightRoofLine.bodyControlPointList[i].transform.position.x - 2 * (rightRoofLine.bodyControlPointList[i].transform.position.x - centerPos.x), rightRoofLine.bodyControlPointList[i].transform.position.y, rightRoofLine.bodyControlPointList[i].transform.position.z);
		}
		leftRoofLine.catLine.ResetCatmullRom();
		AdjMesh();
	}
	public void AdjMesh()
	{
		int innerPointCount = rightRoofLine.catLine.innerPointList.Count;
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
		//mFilter.mesh.uv = uv;
	}
	public override void InitLineRender<T>(T thisGameObject)
	{

	}
	public void SetIconObjectColor()
	{
		mRenderer.material.color = Color.red;
		for(int i=0;i<rightRoofLine.bodyControlPointList.Count;i++)
		{
			rightRoofLine.bodyControlPointList[i].GetComponent<MeshRenderer>().material.color = Color.yellow;
		}
		for (int i = 0; i < rightRoofLine.tailControlPointList.Count; i++)
		{
			rightRoofLine.tailControlPointList[i].GetComponent<MeshRenderer>().material.color = Color.yellow;
		}
	}
}
public class Testing : MonoBehaviour {

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

		basedRoofIcon=new BasedRoofIcon();
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
		basedRoofIcon.AdjPos();
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
