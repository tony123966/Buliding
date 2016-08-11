using UnityEngine;
using System.Collections.Generic;

public class DragItemController : MonoBehaviour
{

	public GameObject chooseDragObject = null;
	public GameObject chooseWindow ;
	public GameObject chooseGrid;
	public GameObject chooseObj = null;
	private Camera chooseCamera;

	public Camera uICamera;
	public List<GameObject> windowsList = new List<GameObject>();
	public List<GameObject> gridList = new List<GameObject>();
	public List<Camera> cameraList = new List<Camera>();

	public GameObject quadItem;


	private bool isDropInChooseWindow=false;
	void Start()
	{
		uICamera = GameObject.Find("UICamera").GetComponent<Camera>();
	}
	void Update()
	{
		RayCastToChooseObj();
	}
	void RayCastToChooseObj() 
	{
		if (chooseObj)
		{
			if (Input.GetMouseButtonUp(0))
			{
				chooseObj.GetComponent<Collider>().enabled = true;
				chooseObj = null;
				return;
			}
			else
			{

				Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				Vector2 mousePosToWorld = uICamera.ScreenToWorldPoint(mousePos);
				Ray ray = uICamera.ScreenPointToRay(mousePos);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit))
				{
					if (hit.collider.gameObject == chooseWindow)
					{
						chooseObj.transform.position = new Vector3(mousePosToWorld.x, mousePosToWorld.y, chooseObj.transform.position.z);
					}
				}
			}
		}
		else
		{
			if (Input.GetMouseButtonDown(0))
			{
				ChooseWindow();
				if (chooseWindow != null)
				{
					Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
					Ray ray = uICamera.ScreenPointToRay(mousePos);
					RaycastHit hit;
					/*
										Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
										Ray ray = sectionCamera.ScreenPointToRay(mousePos);
										Debug.Log("PP:"+sectionCamera.ScreenToWorldPoint(mousePos));
										RaycastHit hit;

										Debug.DrawLine(mousePos, Vector3.forward, Color.green);*/


					if (Physics.Raycast(ray, out hit))
					{
						if (hit.collider.gameObject)
						{
							if (hit.collider.gameObject.tag == "ControlPoint")
							{
								chooseObj = hit.collider.gameObject;
								chooseObj.GetComponent<Collider>().enabled = false;
							}
						}
					}
				}
			}
		}
	}
	bool ChooseWindow()
	{
		RaycastHit[] hits;
		Ray ray = uICamera.ScreenPointToRay(Input.mousePosition);
		hits = Physics.RaycastAll(ray);

		foreach (RaycastHit item in hits)
		{
			for (int i = 0; i < windowsList.Count; i++)
			{
				if (windowsList[i] == item.collider.gameObject)
				{
					if (windowsList[i] == chooseWindow) isDropInChooseWindow = true;
					
					chooseWindow = windowsList[i];
					chooseCamera = cameraList[i].GetComponent<Camera>();

					
					chooseGrid.SetActive(false);
					chooseGrid=gridList[i];
					chooseGrid.SetActive(true);
					

					return true;
				}

			}
		}
		return false;
	}
	public bool SetObjInWiindow()
	{

		ChooseWindow();
		if (chooseWindow != null && isDropInChooseWindow)
		{
			Debug.Log(chooseWindow.name);

			Vector3 pos = chooseWindow.transform.position;
			pos.z -= 1.0f;
			Instantiate(quadItem, pos, quadItem.transform.rotation);

			isDropInChooseWindow=false;
/*
			Vector2 mousePos = new Vector2(Input.mousePosition.x ,Input.mousePosition.y);
			Vector3 pos = sectionCamera.ViewportToWorldPoint(new Vector3(uICamera.ScreenToViewportPoint(mousePos).x, uICamera.ScreenToViewportPoint(mousePos).y));
			pos.z = sectionCamera.farClipPlane / 2.0f;
			Debug.Log("pos:"+pos);
			Instantiate(quadItem, pos, quadItem.transform.rotation);
*/			return true;
		}
		return false;
	}
}
