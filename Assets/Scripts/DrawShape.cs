using UnityEngine;
using System.Collections;

public class DrawShape : MonoBehaviour
{
	public GameObject chooseObj = null;
	public Camera chooseCamera = null;

	public Rect choosewindowRect;
	public GameObject TranslateControlPoint;
	void Update()
	{
		if (chooseObj)
		{
			if (Input.GetMouseButtonUp(0))
			{
				chooseObj = null;
				return;
			}
			else
			{
				Vector3 mousePos = new Vector3(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 0);
				if (choosewindowRect.Contains(mousePos)) chooseObj.transform.position = new Vector3(chooseCamera.ScreenToWorldPoint(Input.mousePosition).x, chooseCamera.ScreenToWorldPoint(Input.mousePosition).y, 0);
			}
		}
		else
		{
			Vector3 mousePos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
			if (Input.GetMouseButtonDown(0))
			{
				Ray ray = chooseCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit))
				{
					if (hit.collider.gameObject)
					{
						if (hit.collider.gameObject.tag == "ControlPoint") chooseObj = hit.collider.gameObject;
					}
				}
			}
		}
	}
	public void DrawQuidHouse(Rect position, Color color)
	{
		Vector3[] ControlPointPos = new Vector3[]
		{
			new Vector3(position.x,position.y,0),
			new Vector3(position.x+position.width,position.y,0),
			new Vector3(position.x,position.y+position.height,0),
			new Vector3(position.x+position.width,position.y+position.height,0),
		};
		for (int i = 0; i < ControlPointPos.Length; i++)
		{
			GameObject clone = Instantiate(TranslateControlPoint,(ControlPointPos[i]), TranslateControlPoint.transform.rotation) as GameObject;
		}
	}
}
