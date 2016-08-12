using UnityEngine;
using System.Collections.Generic;
using System.Collections;
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


	private bool isDropInChooseWindow=false;

	public List<GameObject> MainComponentInWindowsList = new List<GameObject>();
	public List<GameObject>[] ChildComponentInWindowsList;
	class ComponentInWindowsList<T>
	{
		public FormFactorComponent formFactorComponent = new FormFactorComponent();
		public RoofComponent roofComponent = new RoofComponent();
		public BodyComponent bodyComponent = new BodyComponent();
		public PlatformComponent platformComponent = new PlatformComponent();


	}

	struct FormFactorComponent
	{
		public GameObject MainComponent;
	}
	struct RoofComponent
	{
		public GameObject MainComponent ;
	}
	struct BodyComponent
	{
		public GameObject MainComponent ;
		public List<GameObject> door ;
	}
	struct PlatformComponent
	{
		public GameObject MainComponent ;
	}
	
	void Start()
	{
		uICamera = GameObject.Find("UICamera").GetComponent<Camera>();
		InitWindowListSetting();
	}
	void Update()
	{
		RayCastToChooseObj();
	}
	void InitWindowListSetting() 
	{ 
		for(int i=0;i<windowsList.Count;i++)
		{
			MainComponentInWindowsList.Add(null);
		}
		ChildComponentInWindowsList = new List<GameObject>[MainComponentInWindowsList.Count];
		for(int i=0;i<windowsList.Count;i++)
		{
			ChildComponentInWindowsList[i] = new List<GameObject>();
		}
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
	int ChooseWindow()
	{
		RaycastHit[] hits;
		Ray ray = uICamera.ScreenPointToRay(Input.mousePosition);
		hits = Physics.RaycastAll(ray);

		foreach (RaycastHit item in hits)
		{
			for (int index = 0; index < windowsList.Count; index++)
			{
				if (windowsList[index] == item.collider.gameObject)//滑鼠所在的視窗
				{
					if (chooseDragObject)
					{ 
						if(windowsList[index] == chooseWindow)
						{
							if(chooseDragObject.tag=="MainComponent")
							{
								if (MainComponentInWindowsList[index] == null)//如果有拖曳物件 且在選擇的視窗內 且視窗內物件為空
									isDropInChooseWindow = true;
							}
							else if (chooseDragObject.tag == "DecorateComponent")
							{
								if (MainComponentInWindowsList[index] != null)//如果有拖曳物件 且在選擇的視窗內 且視窗內物件為空
									isDropInChooseWindow = true;
							}
						}
					}
					
					//選擇視窗
					chooseWindow = windowsList[index];
					//選擇鏡頭
					chooseCamera = cameraList[index].GetComponent<Camera>();

					//選擇Grid
					chooseGrid.SetActive(false);
					chooseGrid = gridList[index];
					chooseGrid.SetActive(true);


					return index;
				}

			}
		}
		return -1;
	}
	public bool SetObjInWiindow()
	{

		int windowIndex=ChooseWindow();
		if (chooseWindow != null && isDropInChooseWindow)
		{		
			Vector3 pos = chooseWindow.transform.position;
			pos.z -= 0.01f;

			GameObject cloneCorrespondingObj=chooseDragObject.GetComponent<CorespondingDragItem>().corespondingDragItem;
			if(cloneCorrespondingObj)
			{
				if (chooseDragObject.tag == "MainComponent")
				{
					GameObject clone = Instantiate(cloneCorrespondingObj, pos, cloneCorrespondingObj.transform.rotation) as GameObject;
					MainComponentInWindowsList[windowIndex] = clone;
				}
				else if (chooseDragObject.tag == "DecorateComponent")
				{
					GameObject clone = Instantiate(cloneCorrespondingObj, pos, cloneCorrespondingObj.transform.rotation) as GameObject;
					ChildComponentInWindowsList[windowIndex].Add(clone);
				}
			}
			isDropInChooseWindow = false;
/*
			Vector2 mousePos = new Vector2(Input.mousePosition.x ,Input.mousePosition.y);
			Vector3 pos = sectionCamera.ViewportToWorldPoint(new Vector3(uICamera.ScreenToViewportPoint(mousePos).x, uICamera.ScreenToViewportPoint(mousePos).y));
			pos.z = sectionCamera.farClipPlane / 2.0f;
			Debug.Log("pos:"+pos);
			Instantiate(quadItem, pos, quadItem.transform.rotation);
 * 			isDropInChooseWindow = false;
*/			return true;
		}
		return false;
	}
}
