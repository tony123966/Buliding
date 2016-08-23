/*

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class WindowsList : DragItemController
{
	public Dictionary<string, List<GameObject>> allComponent;
	public Dictionary<string,Dictionary<string, List<GameObject>>> temporateComponent;
	public GameObject lastChooseMainDragObject=null;

	public void PrintAllComponentCount()
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allComponent)
		{
			Debug.Log(kvp.Key + kvp.Value.Count);
		}
	}

	public void ClearAllComponent()
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allComponent)
		{
			//childComponent.Remove(kvp.Key);......??????
			for (int i = 0; i < kvp.Value.Count; i++)
			{
				Destroy(kvp.Value[i]);
			}
		}
		allComponent.Clear();
	}
	public void HideAllComponent()
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allComponent)
		{
			//childComponent.Remove(kvp.Key);......??????
			for (int i = 0; i < kvp.Value.Count; i++)
			{
	
				int children = kvp.Value[i].transform.childCount;
				for (int n = 0; n < children; n++)
				{
					kvp.Value[i].transform.GetChild(n).GetComponent<MeshRenderer>().enabled = false;
				}
				(kvp.Value[i]).GetComponent<MeshRenderer>().enabled = false;
			}
		}
		allComponent.Clear();
	}
	public void ShowAllComponent()
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allComponent)
		{
			//childComponent.Remove(kvp.Key);......??????
			for (int i = 0; i < kvp.Value.Count; i++)
			{
				int children = kvp.Value[i].transform.childCount;
				for (int n = 0; n < children ;n++)
				{
					kvp.Value[i].transform.GetChild(n).GetComponent<MeshRenderer>().enabled = true;
				}
				(kvp.Value[i]).GetComponent<MeshRenderer>().enabled = true;
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
	private bool isCorrectOperate = false;
	//四個視窗中的物件集合
	public WindowsList[] AllwindowsComponent;

	
	private int windowsIndex;
	private bool ggg=false;
	void Start()
	{
		uICamera = GameObject.Find("UICamera").GetComponent<Camera>();
		InitWindowListSetting();
	}
	void Update()
	{
		RayCastToChooseObj();
		if (Input.GetKeyDown(KeyCode.R))
		{
			for (int i = 0; i < windowsList.Count; i++)
			{
				if (chooseWindow == windowsList[i])
				{
					AllwindowsComponent[i].PrintAllComponentCount();
				}
			}
		}

	}
	void InitWindowListSetting()
	{
		AllwindowsComponent = new WindowsList[windowsList.Count];
		for (int i = 0; i < windowsList.Count; i++)
		{
			AllwindowsComponent[i] = new WindowsList();
			AllwindowsComponent[i].allComponent = new Dictionary<string, List<GameObject>>();
			AllwindowsComponent[i].temporateComponent = new Dictionary<string, Dictionary<string, List<GameObject>>>();
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
				windowsIndex=ChooseWindow();
				if (chooseWindow != null)
				{
					Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
					Ray ray = uICamera.ScreenPointToRay(mousePos);
					RaycastHit hit;

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
							if (AllwindowsComponent[index].lastChooseMainDragObject)
							{
								if (!AllwindowsComponent[index].temporateComponent.ContainsKey(AllwindowsComponent[index].lastChooseMainDragObject.name))
								{
									Debug.Log("First");
									Dictionary<string, List<GameObject>> copy = new Dictionary<string, List<GameObject>>(AllwindowsComponent[index].allComponent);
									AllwindowsComponent[index].temporateComponent.Add(AllwindowsComponent[index].lastChooseMainDragObject.name, copy);
								}
								else
								{
									Debug.Log("Second");
									Dictionary<string, List<GameObject>> copy = new Dictionary<string, List<GameObject>>(AllwindowsComponent[index].allComponent);
									AllwindowsComponent[index].temporateComponent[AllwindowsComponent[index].lastChooseMainDragObject.name] = copy;
								}
							}
							if (chooseDragObject.tag == "MainComponent")
							{
								if (AllwindowsComponent[index].allComponent.ContainsKey("MainComponent") == false)//在選擇的視窗內 且視窗內物件為空
								{
									isCorrectOperate = true;
									Debug.Log("000");
								}
								else//視窗內物件為不為空
								{
									if (AllwindowsComponent[index].lastChooseMainDragObject != chooseDragObject)//如果不是拖曳同一個主物件取代原本的主物物件
									{
										//紀錄操作的物件
										if (AllwindowsComponent[index].temporateComponent.ContainsKey(chooseDragObject.name)) //有編輯過此視窗
										{
											AllwindowsComponent[index].HideAllComponent();
											AllwindowsComponent[index].allComponent = AllwindowsComponent[index].temporateComponent[chooseDragObject.name];
											AllwindowsComponent[index].ShowAllComponent();
											Debug.Log("1111");
										}
										else //沒有編輯過此視窗
										{
											AllwindowsComponent[index].HideAllComponent();
											isCorrectOperate = true;
											Debug.Log("222");
										}
									}
									else //如果拖曳同一個主物件取代原本的主物物件
									{
										//清除此視窗物件
										AllwindowsComponent[index].ClearAllComponent();
										AllwindowsComponent[index].temporateComponent[chooseDragObject.name].Clear();
										isCorrectOperate = true;
										Debug.Log("333");
									}
								}
								AllwindowsComponent[index].lastChooseMainDragObject = chooseDragObject;
							}
							else if (chooseDragObject.tag == "DecorateComponent")
							{
								if (AllwindowsComponent[index].allComponent["MainComponent"] != null)//如果有拖曳物件 且在選擇的視窗內 且視窗內物件為空
								{
									isCorrectOperate = true;
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
	public bool SetObjInWiindow()//暫時先在攝影機前產生
	{

		windowsIndex=ChooseWindow();
		if (chooseWindow != null && isCorrectOperate)
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

					List<GameObject> allComponentList = new List<GameObject>();
					allComponentList.Add(clone);
					AllwindowsComponent[windowsIndex].allComponent.Add("MainComponent", allComponentList);
					Debug.Log("TTT" + chooseDragObject.name + " " + AllwindowsComponent[windowsIndex].allComponent["MainComponent"].Count);
				}
				else if (chooseDragObject.tag == "DecorateComponent")
				{
					GameObject clone = Instantiate(cloneCorrespondingObj, pos, cloneCorrespondingObj.transform.rotation) as GameObject;
					if (AllwindowsComponent[windowsIndex].allComponent.ContainsKey(chooseDragObject.name))
					{
						if (AllwindowsComponent[windowsIndex].allComponent[chooseDragObject.name].Count < correspondingDragItemMaxCount)
						{
							AllwindowsComponent[windowsIndex].allComponent[chooseDragObject.name].Add(clone);
						}
						else
							Debug.Log(chooseDragObject.name + "    Count over MaxCount");
					}
					else
					{
						List<GameObject> newList = new List<GameObject>();
						newList.Clear();
						newList.Add(clone);
						AllwindowsComponent[windowsIndex].allComponent.Add(chooseDragObject.name, newList);
					}

				}
			}
			isCorrectOperate = false;

			return true;
		}
		return false;
	}
}
*/
/*******************************************************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class WindowsList : DragItemController
{
	public Dictionary<string, List<GameObject>> allComponent;
	public Dictionary<string, Dictionary<string, List<GameObject>>> temporateComponent;
	public GameObject lastChooseMainDragObject = null;

	public void PrintAllComponentCount()
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allComponent)
		{
			Debug.Log(kvp.Key + kvp.Value.Count);
		}
	}

	public void ClearAllComponent()
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allComponent)
		{
			//childComponent.Remove(kvp.Key);......??????
			for (int i = 0; i < kvp.Value.Count; i++)
			{
				Destroy(kvp.Value[i]);
			}
		}
		allComponent.Clear();
	}
	public void HideAllComponent()
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allComponent)
		{
			for (int i = 0; i < kvp.Value.Count; i++)
				(kvp.Value[i]).SetActive(false);
		}
		allComponent.Clear();
	}
	public void ShowAllComponent()
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allComponent)
		{
			for (int i = 0; i < kvp.Value.Count; i++)
				(kvp.Value[i]).SetActive(true);
		}
	}
}
public class DragItemController : MonoBehaviour
{
	const string MAINCOMPONENT = "MainComponent";
	const string CONTROLPOINT = "ControlPoint";
	const string DECORATECOMPONENT = "DecorateComponent";
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

	//四個視窗中的物件集合
	public WindowsList[] AllwindowsComponent;

	void Start()
	{
		uICamera = GameObject.Find("UICamera").GetComponent<Camera>();
		InitWindowListSetting();
	}
	void Update()
	{
		RayCastToChooseObj();
		if (Input.GetKeyDown(KeyCode.R))
		{
			for (int i = 0; i < windowsList.Count; i++)
			{
				if (chooseWindow == windowsList[i])
				{
					AllwindowsComponent[i].PrintAllComponentCount();
				}
			}
		}

	}
	void InitWindowListSetting()
	{
		AllwindowsComponent = new WindowsList[windowsList.Count];
		for (int i = 0; i < windowsList.Count; i++)
		{
			AllwindowsComponent[i] = new WindowsList();
			AllwindowsComponent[i].allComponent = new Dictionary<string, List<GameObject>>();
			AllwindowsComponent[i].temporateComponent = new Dictionary<string, Dictionary<string, List<GameObject>>>();
		}

	}
	void RayCastToChooseObj()
	{
		if (chooseObj){
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
				if (Physics.Raycast(ray, out hit)){
					if (hit.collider.gameObject == chooseWindow){
						if (chooseObj.transform.parent.GetComponent<MeshObj> ()) {
							if (chooseObj.transform.parent.GetComponent<MeshObj>().name == "rectangle_Obj"){
								if (chooseObj.name == "_Horizontalcp" || chooseObj.name == "_Horizontalcp2"){
									Vector3 _mousepos = new Vector3(mousePosToWorld.x, chooseObj.transform.position.y, chooseObj.transform.position.z);
								}
								else{
								         chooseObj.transform.position = new Vector3 (mousePosToWorld.x, mousePosToWorld.y, chooseObj.transform.position.z);
										 chooseObj.transform.parent.GetComponent<MeshObj> ().adjPos();	
								}
							}
							else{
								chooseObj.transform.position = new Vector3(mousePosToWorld.x, mousePosToWorld.y, chooseObj.transform.position.z);
								chooseObj.transform.parent.GetComponent<MeshObj>().adjPos();	
							}
						}
						else { 
						chooseObj.transform.position = new Vector3(mousePosToWorld.x, mousePosToWorld.y, chooseObj.transform.position.z);
						if (chooseObj.GetComponent<ooficonmidcontrolpointr>())
						{
							chooseObj.GetComponent<ooficonmidcontrolpointr>().ControlPoint22.GetComponent<fourmove>().reset();
							chooseObj.GetComponent<ooficonmidcontrolpointr>().meshreset.GetComponent<rooficon>().reset();
						}
						}
					}
	
					}
				}
		}
		else
		{
			if (Input.GetMouseButtonDown(0))
			{
				int index = ChooseWindow();

				if (index != -1) {
					SetCameraAndGrid(index);
					//選擇視窗
					chooseWindow = windowsList[index];
				}

				if (chooseWindow != null)
				{
					Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
					Ray ray = uICamera.ScreenPointToRay(mousePos);
					RaycastHit hit;

					if (Physics.Raycast(ray, out hit))
					{
						if (hit.collider.gameObject)
						{
							if (hit.collider.gameObject.tag == CONTROLPOINT)
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
					return index;
				}
			}
		}
		return -1;
	}
	void SetCameraAndGrid(int index)
	{
		//選擇鏡頭
		chooseCamera = cameraList[index].GetComponent<Camera>();

		//選擇Grid
		chooseGrid.SetActive(false);
		chooseGrid = gridList[index];
		chooseGrid.SetActive(true);
	}
	public void SetObjInWiindows()//暫時先在攝影機前產生
	{
		int index = ChooseWindow();
		if (index != -1 && chooseDragObject)
		{
			SetCameraAndGrid(index);
			if (windowsList[index] == chooseWindow)
			{
				if (AllwindowsComponent[index].lastChooseMainDragObject)
				{
					if (!AllwindowsComponent[index].temporateComponent.ContainsKey(AllwindowsComponent[index].lastChooseMainDragObject.name))
					{
						Debug.Log("First");
						Dictionary<string, List<GameObject>> copy = new Dictionary<string, List<GameObject>>(AllwindowsComponent[index].allComponent);
						AllwindowsComponent[index].temporateComponent.Add(AllwindowsComponent[index].lastChooseMainDragObject.name, copy);
					}
					else
					{
						Debug.Log("Second");
						Dictionary<string, List<GameObject>> copy = new Dictionary<string, List<GameObject>>(AllwindowsComponent[index].allComponent);
						AllwindowsComponent[index].temporateComponent[AllwindowsComponent[index].lastChooseMainDragObject.name] = copy;
					}
				}
				if (chooseDragObject.tag == MAINCOMPONENT)
				{
					if (AllwindowsComponent[index].allComponent.ContainsKey(MAINCOMPONENT) == false)//在選擇的視窗內 且視窗內物件為空
					{
						CreateMainComponent(index);
						Debug.Log("000");
					}
					else//視窗內物件為不為空
					{
						if (AllwindowsComponent[index].lastChooseMainDragObject != chooseDragObject)//如果不是拖曳同一個主物件取代原本的主物物件
						{
							//紀錄操作的物件
							if (AllwindowsComponent[index].temporateComponent.ContainsKey(chooseDragObject.name)) //有編輯過此視窗
							{
								AllwindowsComponent[index].HideAllComponent();
								AllwindowsComponent[index].allComponent = AllwindowsComponent[index].temporateComponent[chooseDragObject.name];
								AllwindowsComponent[index].ShowAllComponent();
								Debug.Log("1111");
							}
							else //沒有編輯過此視窗
							{
								AllwindowsComponent[index].HideAllComponent();
								CreateMainComponent(index);
								Debug.Log("222");
							}
						}
						else //如果拖曳同一個主物件取代原本的主物物件
						{
							//清除此視窗物件
							AllwindowsComponent[index].ClearAllComponent();
							AllwindowsComponent[index].temporateComponent[chooseDragObject.name].Clear();
							CreateMainComponent(index);
							Debug.Log("333");
						}
					}
					AllwindowsComponent[index].lastChooseMainDragObject = chooseDragObject;
				}
				else if (chooseDragObject.tag == DECORATECOMPONENT)
				{
					if (AllwindowsComponent[index].allComponent.ContainsKey(MAINCOMPONENT))//如果有拖曳物件 且在選擇的視窗內 且視窗內物件為空
					{
						CreateDecorateComponent(index);
					}
				}
			}
			//選擇視窗
			chooseWindow = windowsList[index];
		}
	}
	void CreateMainComponent(int index)
	{
		Vector3 pos = chooseWindow.transform.position; pos.z -= 0.01f;

		GameObject cloneCorrespondingObj = chooseDragObject.GetComponent<CorespondingDragItem>().corespondingDragItem;

		GameObject clone = Instantiate(cloneCorrespondingObj, pos, cloneCorrespondingObj.transform.rotation) as GameObject;
		clone.transform.parent = chooseWindow.transform;

		List<GameObject> allComponentList = new List<GameObject>();
		allComponentList.Add(clone);
		AllwindowsComponent[index].allComponent.Add(MAINCOMPONENT, allComponentList);
	}
	void CreateDecorateComponent(int index)
	{
		Vector3 pos = chooseWindow.transform.position; pos.z -= 0.01f;

		GameObject cloneCorrespondingObj = chooseDragObject.GetComponent<CorespondingDragItem>().corespondingDragItem;
		int correspondingDragItemMaxCount = chooseDragObject.GetComponent<CorespondingDragItem>().correspondingDragItemMaxCount;

		if (AllwindowsComponent[index].allComponent.ContainsKey(chooseDragObject.name))
		{
			if (AllwindowsComponent[index].allComponent[chooseDragObject.name].Count < correspondingDragItemMaxCount)
			{
				GameObject clone = Instantiate(cloneCorrespondingObj, pos, cloneCorrespondingObj.transform.rotation) as GameObject;
				clone.transform.parent = chooseWindow.transform;
				AllwindowsComponent[index].allComponent[chooseDragObject.name].Add(clone);
			}
			else
				Debug.Log(chooseDragObject.name + "    Count over MaxCount");
		}
		else
		{
			GameObject clone = Instantiate(cloneCorrespondingObj, pos, cloneCorrespondingObj.transform.rotation) as GameObject;
			clone.transform.parent = chooseWindow.transform;
			List<GameObject> newList = new List<GameObject>();
			newList.Clear();
			newList.Add(clone);
			AllwindowsComponent[index].allComponent.Add(chooseDragObject.name, newList);
		}
	}
}
