using UnityEngine;
using System.Collections.Generic;

public class DragItemController : MonoBehaviour
{

	public GameObject chooseDragObject = null;
	public GameObject chooseWindow = null;
	public GameObject chooseObj = null;
	public Camera UICamera;
	private bool setCloneObj = false;
	public List<GameObject> windowsList = new List<GameObject>();
	public List<Camera> cameraList = new List<Camera>();

	public GameObject quadItem;


	private Camera sectionCamera;
	void Start()
	{

		UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
	}
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

			}
		}
		else
		{
			if (Input.GetMouseButtonDown(0))
			{
				ChooseWindow();
				/*Vector3 mousePos = new Vector3(UICamera.ScreenToViewportPoint(Input.mousePosition).x, UICamera.ScreenToViewportPoint(Input.mousePosition).y, 0);
				mousePos = sectionCamera.ViewportToScreenPoint(mousePos);*/
				if (sectionCamera != null)
				{
		/*			Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
					Vector3 pos = sectionCamera.ViewportToScreenPoint(new Vector3(UICamera.ScreenToViewportPoint(mousePos).x, UICamera.ScreenToViewportPoint(mousePos).y));*/
					Vector2 mousePos = new Vector2(Input.mousePosition.x - Screen.width / 2.0f, Input.mousePosition.y - Screen.height / 2.0f);
					Ray ray = sectionCamera.ScreenPointToRay(mousePos);
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
	}
	void ChooseWindow()
	{
		RaycastHit[] hits;
		Ray ray = UICamera.ScreenPointToRay(Input.mousePosition);
		hits = Physics.RaycastAll(ray);

		foreach (RaycastHit item in hits)
		{
			foreach (GameObject windowRec in windowsList)
			{
				if (windowRec == item.collider.gameObject)
				{
					chooseWindow = windowRec;
					switch (chooseWindow.name)
					{
						case "FormFractorWindowRect":
							sectionCamera = GameObject.Find("FormFactorViewCamera").GetComponent<Camera>();
							break;
						case "RoofWindowRect":
							sectionCamera = GameObject.Find("RoofViewCamera").GetComponent<Camera>();
							break;
						case "BodyWindowRect":
							sectionCamera = GameObject.Find("BodyViewCamera").GetComponent<Camera>();
							break;
						case "PlatformWindowRect":
							sectionCamera = GameObject.Find("PlatformViewCamera").GetComponent<Camera>();
							break;
					}
				}
			}
		}
	}
	public void SetMouseInWiindow()
	{

		ChooseWindow();
		if (sectionCamera != null)
		{
			Debug.Log(sectionCamera.name);

/*
							Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
							Vector3 pos = sectionCamera.transform.position;
							pos.z = sectionCamera.farClipPlane / 2.0f;
							Instantiate(quadItem, pos, quadItem.transform.rotation);*/


			Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			Vector3 pos = sectionCamera.ViewportToWorldPoint(new Vector3(UICamera.ScreenToViewportPoint(mousePos).x, UICamera.ScreenToViewportPoint(mousePos).y));
			pos.z = sectionCamera.farClipPlane / 2.0f;
			Debug.Log("pos:"+pos);
			Instantiate(quadItem, pos, quadItem.transform.rotation);


			/*

								ray = sectionCamera.ScreenPointToRay(Input.mousePosition);
								RaycastHit hit;
								if (Physics.Raycast(ray, out hit)) 
									if (hit.collider.tag=="CollisionPlane")
									{
										Instantiate(quadItem, hit.point, quadItem.transform.rotation);
									}
							}*/
		}
	}
}
