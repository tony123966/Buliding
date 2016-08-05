using UnityEngine;
using System.Collections.Generic;

public class DragItemController : MonoBehaviour {

	public GameObject chooseGameObject=null;
	private bool setCloneObj=false;
	public List<GameObject>windowsList=new List<GameObject>();
	public GameObject zzz;
	public void DrawCircle(Vector2 centerPos, float radius = 100)
	{
		float DEG2RAD = 3.14159f / 180;
		GL.Begin(GL.LINES);
		GL.Color(Color.red);
		for (int i = 0; i < 360; i++)
		{
			float degInRad = i * DEG2RAD;
			GL.Vertex(centerPos);
			GL.Vertex(new Vector2(centerPos.x + Mathf.Cos(degInRad) * radius, centerPos.y + Mathf.Sin(degInRad) * radius));
		}
		GL.End();
		Debug.Log("zzz");
	}
	public void PrintPos(Vector2 pos)
	{
		Debug.Log("pos:" + pos);
		Camera ca= GameObject.Find("UICamera").GetComponent<Camera>();
		pos=ca.ScreenToWorldPoint(pos);
		//Instantiate(zzz, pos, zzz.transform.rotation);
		DrawCircle(pos);
		Debug.Log("pos:" + pos);
	}
}
