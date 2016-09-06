/*

/ ******************************************************************************************************************************************************************************* /

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class WindowsList : DragItemController
{
/ *
	public int[][] zzz=new int[5][]
	{new int[5]{1,1,1,1,1},
	new int[5]{1,1,1,1,1},
	new int[5]{1,1,1,1,1},
	new int[5]{1,1,1,1,1},
	new int[5]{1,1,1,1,1}};
	public int[,] ttt=new int[5,5]
	{{1,1,1,1,1},
	{1,1,1,1,1},
	{1,1,1,1,1},
	{1,1,1,1,1},
	{1,1,1,1,1}};* /
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
	const string CYLINDER = "Cylinder";


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

	//
	public Movement movement;
	public AllInOne building;
	public MeshObj meshObj;//??????
	void Start()
	{
		uICamera = GameObject.Find("UICamera").GetComponent<Camera>();
		movement = GameObject.Find ("Movement").GetComponent<Movement> ();
		building=GameObject.Find("build").GetComponent<AllInOne>();
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
			if (Input.GetMouseButtonUp (0)) {
				chooseObj.GetComponent<Collider> ().enabled = true;
				chooseObj = null;
				return;
			} else {

				Vector2 mousePos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
				Vector2 mousePosToWorld = uICamera.ScreenToWorldPoint (mousePos);
				Ray ray = uICamera.ScreenPointToRay (mousePos);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {
					if (hit.collider.gameObject == chooseWindow) {


						//if (chooseObj.transform.parent.GetComponent<MeshObj> ()) { ??????????
						movement.move (mousePosToWorld);
						//chooseObj.transform.parent.GetComponent<MeshObj>().adjPos();
//							if (chooseObj.transform.parent.GetComponent<MeshObj>().name == "rectangle_Obj"){
//								if (chooseObj.name == "_Horizontalcp" || chooseObj.name == "_Horizontalcp2"){
//									Vector3 _mousepos = new Vector3(mousePosToWorld.x, chooseObj.transform.position.y, chooseObj.transform.position.z);
//								}
//								else{
//								         chooseObj.transform.position = new Vector3 (mousePosToWorld.x, mousePosToWorld.y, chooseObj.transform.position.z);
//										 chooseObj.transform.parent.GetComponent<MeshObj> ().adjPos();	
//								}
//							}
//							else{
//								chooseObj.transform.position = new Vector3(mousePosToWorld.x, mousePosToWorld.y, chooseObj.transform.position.z);
//								chooseObj.transform.parent.GetComponent<MeshObj>().adjPos();	
//							}
//						}
//						else { 
//						chooseObj.transform.position = new Vector3(mousePosToWorld.x, mousePosToWorld.y, chooseObj.transform.position.z);
						if (chooseObj.GetComponent<ooficonmidcontrolpointr> ()) {
							chooseObj.GetComponent<ooficonmidcontrolpointr> ().ControlPoint22.GetComponent<fourmove> ().reset ();
							chooseObj.GetComponent<ooficonmidcontrolpointr> ().meshreset.GetComponent<rooficon> ().reset ();
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
							if (hit.collider.gameObject.tag == CONTROLPOINT || hit.collider.gameObject.tag == CYLINDER)
							{	
								
								chooseObj = hit.collider.gameObject;
								movement.checkrepeat ();
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
					movement.freelist.Clear ();
					movement.horlist.Clear ();
					movement.verlist.Clear ();
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
					if (AllwindowsComponent[index].allComponent[MAINCOMPONENT][0].GetComponent<MeshObj>()) building.UpdateAll(AllwindowsComponent[index].allComponent[MAINCOMPONENT][0].GetComponent<MeshObj>().edgeIndex);
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
	const string CYLINDER = "Cylinder";


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
	//當前使用視窗中的物件集合
	public WindowsList ThisWindowsComponent;

	//
	public Movement movement;
	public AllInOne building;
	public MeshObj meshObj;//??????
	void Start()
	{
		uICamera = GameObject.Find("UICamera").GetComponent<Camera>();
		movement = GameObject.Find("Movement").GetComponent<Movement>();
		building = GameObject.Find("build").GetComponent<AllInOne>();
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
				Vector2 mousePos2World = uICamera.ScreenToWorldPoint(mousePos);
				Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(chooseWindow.transform);
				if (bounds.Contains(mousePos2World))
				{
					Vector2 hitLocalUV;
					Vector3 localHit = mousePos2World;

					// normalize
					hitLocalUV.x = (localHit.x - bounds.min.x) / (bounds.size.x);
					hitLocalUV.y = (localHit.y - bounds.min.y) / (bounds.size.y);
					float border=0.2f;

					if (hitLocalUV.x >= border && hitLocalUV.x <= (1 - border) && hitLocalUV.y >= border && hitLocalUV.y <= (1 - border))
					{
						Vector2 loc = chooseCamera.ScreenToWorldPoint(new Vector2(hitLocalUV.x * chooseCamera.pixelWidth, hitLocalUV.y * chooseCamera.pixelHeight));
						movement.move(loc);

						if (chooseObj.GetComponent<ooficonmidcontrolpointr>())
						{
							chooseObj.GetComponent<ooficonmidcontrolpointr>().ControlPoint22.GetComponent<fourmove>().reset();
							chooseObj.GetComponent<ooficonmidcontrolpointr>().meshreset.GetComponent<rooficon>().reset();
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
				if (index != -1)
				{
					SetCameraAndGrid(index);
					//選擇視窗
					chooseWindow = windowsList[index];
				}

				if (chooseWindow != null)
				{
					Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
					Vector2 mousePos2World = uICamera.ScreenToWorldPoint(mousePos);
					Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(chooseWindow.transform);

					if (bounds.Contains(mousePos2World))
					{

						Vector2 hitLocalUV;
						Vector3 localHit = mousePos2World;
						// normalize
						hitLocalUV.x = (localHit.x - bounds.min.x) / (bounds.size.x);
						hitLocalUV.y = (localHit.y - bounds.min.y) / (bounds.size.y);

						Ray srsRay = chooseCamera.ScreenPointToRay(new Vector2(hitLocalUV.x * chooseCamera.pixelWidth, hitLocalUV.y * chooseCamera.pixelHeight));
						RaycastHit srsHit;
						if (Physics.Raycast(srsRay, out srsHit))
						{
							if (srsHit.collider.gameObject.tag == CONTROLPOINT || srsHit.collider.gameObject.tag == CYLINDER)
							{
								chooseObj = srsHit.collider.gameObject;
								movement.checkrepeat();
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
					ThisWindowsComponent = AllwindowsComponent[index];
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
				switch (chooseDragObject.tag)
				{
					case MAINCOMPONENT:
						movement.freelist.Clear();
						movement.horlist.Clear();
						movement.verlist.Clear();
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
						if (AllwindowsComponent[index].allComponent[MAINCOMPONENT][0].GetComponent<MeshObj>()) building.UpdateAll(AllwindowsComponent[index].allComponent[MAINCOMPONENT][0].GetComponent<MeshObj>().edgeIndex);

						break;
					case DECORATECOMPONENT:

						if (AllwindowsComponent[index].allComponent.ContainsKey(MAINCOMPONENT))//如果有拖曳物件 且在選擇的視窗內 且視窗內物件為空
						{
							CreateDecorateComponent(index);
							if (AllwindowsComponent[index].allComponent[MAINCOMPONENT][0].GetComponent<body2icon>())
								AllwindowsComponent[index].allComponent[MAINCOMPONENT][0].GetComponent<body2icon>().UpdateFunction(AllwindowsComponent[index].allComponent[chooseDragObject.name].Count);
						}

						break;
				}
			}
			//選擇視窗
			chooseWindow = windowsList[index];
		}
	}
	void CreateMainComponent(int index)
	{
		Vector3 pos = chooseCamera.transform.position; pos.z = chooseCamera.farClipPlane/2.0f;

		GameObject cloneCorrespondingObj = chooseDragObject.GetComponent<CorespondingDragItem>().corespondingDragItem;

		GameObject clone = Instantiate(cloneCorrespondingObj, pos, cloneCorrespondingObj.transform.rotation) as GameObject;
		clone.transform.parent = chooseWindow.transform;

		List<GameObject> allComponentList = new List<GameObject>();
		allComponentList.Add(clone);
		AllwindowsComponent[index].allComponent.Add(MAINCOMPONENT, allComponentList);
	}
	void CreateDecorateComponent(int index)
	{
		Vector3 pos = chooseCamera.transform.position; pos.z = chooseCamera.farClipPlane / 2.0f;

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

