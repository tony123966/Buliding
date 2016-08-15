using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Windows : DragItemController
{
	public GameObject mainComponent = null;
	public Dictionary<string, List<GameObject>> childComponent;

	public void GetAllComponentCount()
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in childComponent)
		{
			Debug.Log(kvp.Key + kvp.Value.Count);
		}
	}
	public void DeleteAllComponent()
	{
		Destroy(mainComponent);
		foreach (KeyValuePair<string, List<GameObject>> kvp in childComponent)
		{
			for(int i=0;i<kvp.Value.Count;i++)
			{
				Destroy(kvp.Value[i]);
			}
		}
	}
}
public class DragItemController : MonoBehaviour
{
	enum WINDOWS { FormFactor, Roof, Body, Platform };
	//選定的物件
	public GameObject chooseDragObject = null;
	public GameObject chooseWindow;
	public GameObject chooseGrid;
	public GameObject chooseObj = null;
	private Camera chooseCamera;
	//UICamera
	public Camera uICamera;
	//四大視窗
	public List<GameObject> windowsList = new List<GameObject>();
	public List<GameObject> gridList = new List<GameObject>();
	public List<Camera> cameraList = new List<Camera>();

	//是否滑鼠在選定的視窗中放開
	private bool isDropInChooseWindow = false;
	//四個視窗中的物件集合
	public Windows[] AllwindowsComponent;
	
	void Start()
	{
		uICamera = GameObject.Find("UICamera").GetComponent<Camera>();
		InitWindowListSetting();
	}
	void Update()
	{
		RayCastToChooseObj();
		if(Input.GetKeyDown(KeyCode.R))
		{
			for(int i=0;i<windowsList.Count;i++){
				if(chooseWindow==windowsList[i]){
					AllwindowsComponent[i].GetAllComponentCount();
				}
			}
		}
	}
	void InitWindowListSetting()
	{
		AllwindowsComponent = new Windows[windowsList.Count];
		for (int i = 0; i < windowsList.Count; i++)
		{
			AllwindowsComponent[i] = new Windows();
			AllwindowsComponent[i].childComponent = new Dictionary<string, List<GameObject>>();
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
						if (windowsList[index] == chooseWindow)
						{
							if (chooseDragObject.tag == "MainComponent")
							{
								if (AllwindowsComponent[index].mainComponent == null)//如果有拖曳物件 且在選擇的視窗內 且視窗內物件為空
								{
									isDropInChooseWindow = true;
								}
								else//如果有拖曳物件 且在選擇的視窗內 且視窗內物件為不為空 刪除原物件
								{
									AllwindowsComponent[index].DeleteAllComponent();
									isDropInChooseWindow = true;
								}
							}
							else if (chooseDragObject.tag == "DecorateComponent")
							{
								if (AllwindowsComponent[index].mainComponent != null)//如果有拖曳物件 且在選擇的視窗內 且視窗內物件為空
								{
									isDropInChooseWindow = true;
								}
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

		int windowIndex = ChooseWindow();
		if (chooseWindow != null && isDropInChooseWindow)
		{
			Vector3 pos = chooseWindow.transform.position;
			pos.z -= 0.01f;

			GameObject cloneCorrespondingObj = chooseDragObject.GetComponent<CorespondingDragItem>().corespondingDragItem;
			int correspondingDragItemMaxCount = chooseDragObject.GetComponent<CorespondingDragItem>().correspondingDragItemMaxCount;
			if (cloneCorrespondingObj)
			{
				if (chooseDragObject.tag == "MainComponent")
				{
					GameObject clone = Instantiate(cloneCorrespondingObj, pos, cloneCorrespondingObj.transform.rotation) as GameObject;
					AllwindowsComponent[windowIndex].mainComponent = clone;
				}
				else if (chooseDragObject.tag == "DecorateComponent")
				{
					GameObject clone = Instantiate(cloneCorrespondingObj, pos, cloneCorrespondingObj.transform.rotation) as GameObject;
					if (AllwindowsComponent[windowIndex].childComponent.ContainsKey(chooseDragObject.name))
					{
						if (AllwindowsComponent[windowIndex].childComponent[chooseDragObject.name].Count < correspondingDragItemMaxCount)
							AllwindowsComponent[windowIndex].childComponent[chooseDragObject.name].Add(clone);
						else
							Debug.Log(chooseDragObject.name+"    Count over MaxCount");
					}
					else
					{
						List<GameObject> newList = new List<GameObject>();
						newList.Clear();
						newList.Add(clone);
						AllwindowsComponent[windowIndex].childComponent.Add(chooseDragObject.name, newList);
					}

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
			*/
			return true;
		}
		return false;
	}
}
