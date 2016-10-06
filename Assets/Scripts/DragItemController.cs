/*

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class WindowsList : MonoBehaviour
{
	public List<Dictionary<string, List<GameObject>>> allComponent;
	public List<Dictionary<string, Dictionary<string, List<GameObject>>>> temporateComponent;
	public string lastChooseMainDragObjectName = null;
	public int inUseComponentIndex = 0;

	public void PrintAllComponentCount()
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allComponent[inUseComponentIndex])
		{
			Debug.Log(kvp.Key + kvp.Value.Count);
		}
	}

	public void ClearAllComponent()
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allComponent[inUseComponentIndex])
		{
			//childComponent.Remove(kvp.Key);......??????
			for (int i = 0; i < kvp.Value.Count; i++)
			{
				Destroy(kvp.Value[i]);
			}
		}
		allComponent[inUseComponentIndex].Clear();
	}
	public void HideAllComponent()
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allComponent[inUseComponentIndex])
		{
			for (int i = 0; i < kvp.Value.Count; i++)
				(kvp.Value[i]).SetActive(false);
		}
		allComponent[inUseComponentIndex].Clear();
	}
	public void ShowAllComponent()
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allComponent[inUseComponentIndex])
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
	public GameObject chooseObj = null;
	public GameObject chooseWindow;
	public GameObject chooseGrid;
	private Camera chooseCamera;
	//UICamera
	private Camera uICamera;
	//四大視窗
	public List<GameObject> windowsList = new List<GameObject>();
	public List<GameObject> gridList = new List<GameObject>();
	public List<Camera> cameraList = new List<Camera>();
	//
	public List<GameObject> windowSetList = new List<GameObject>();
	//
	public List<GameObject> buttonList = new List<GameObject>();


	//四個視窗中的物件集合
	private WindowsList[] AllwindowsComponent;
	//當前使用視窗中的物件集合
	[HideInInspector]
	public WindowsList ThisWindowsComponent;
	//
	private Vector3 scrollViewOriginPos;
	//
	public int changeLayoutIndexInWindowsSet = 0;
	private int mainWindowsinUseIndex = 0;
	private GameObject mainWindows;
	//
	private Movement movement;
	private AllInOne building;
	//
	public GameObject missionTab;
	private List<GameObject> missionTabsList = new List<GameObject>();
	private int missionTabsCount = 0;
	void Start()
	{
		uICamera = GameObject.Find("UICamera").GetComponent<Camera>();
		movement = GameObject.Find("Movement").GetComponent<Movement>();
		building = GameObject.Find("build").GetComponent<AllInOne>();
		InitWindowListMemorySetting();
		InitStateSetting();
		SwitchWindow();
	}
	void Update()
	{
		RayCastToChooseObj();
		if (Input.GetKeyDown(KeyCode.Delete))
		{
			DeleteMuliBody() ;
		}

	}
	void DeleteMuliBody() 
	{
		if (missionTabsList.Count == 0)return;
		int index = ChooseWindow();
	
	}
	void InitWindowListMemorySetting()
	{
		AllwindowsComponent = new WindowsList[windowsList.Count];
		for (int i = 0; i < windowsList.Count; i++)
		{
			AllwindowsComponent[i] = new WindowsList();
			AllwindowsComponent[i].allComponent = new List<Dictionary<string, List<GameObject>>>();
			AllwindowsComponent[i].temporateComponent = new List<Dictionary<string, Dictionary<string, List<GameObject>>>>();
			Dictionary<string, List<GameObject>> newAllComponent = new Dictionary<string, List<GameObject>>();
			AllwindowsComponent[i].allComponent.Add(newAllComponent);
			Dictionary<string, Dictionary<string, List<GameObject>>> newTemporateComponent = new Dictionary<string, Dictionary<string, List<GameObject>>>();
			AllwindowsComponent[i].temporateComponent.Add(newTemporateComponent);
			/ *

						AllwindowsComponent[i].allComponent[AllwindowsComponent[i].inUseComponentIndex] = new Dictionary<string, List<GameObject>>();
						AllwindowsComponent[i].temporateComponent[AllwindowsComponent[i].inUseComponentIndex] = new Dictionary<string, Dictionary<string, List<GameObject>>>();* /
		}
	}
	void InitStateSetting()
	{
		scrollViewOriginPos = gridList[0].transform.position;
		chooseGrid = gridList[0];
		SetCameraAndGrid(0);

		mainWindows = windowsList[4];

		if (changeLayoutIndexInWindowsSet == 0)
			chooseWindow = windowsList[0];
		else if (changeLayoutIndexInWindowsSet == 1)
			chooseWindow = mainWindows;
	}
	void RayCastToChooseObj()
	{
		Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		Vector2 mousePos2World = uICamera.ScreenToWorldPoint(mousePos);

		if (chooseObj)
		{
			if (Input.GetMouseButtonUp(0))
			{
				chooseObj.GetComponent<Collider>().enabled = true;
				chooseObj = null;
				//
				building.UpdateRoof();
				return;
			}
			else
			{
				Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(chooseWindow.transform);
				if (bounds.Contains(mousePos2World))
				{
					Vector2 hitLocalUV;
					Vector3 localHit = mousePos2World;

					// normalize
					hitLocalUV.x = (localHit.x - bounds.min.x) / (bounds.size.x);
					hitLocalUV.y = (localHit.y - bounds.min.y) / (bounds.size.y);
					float border = 0.2f;

					if (hitLocalUV.x >= border && hitLocalUV.x <= (1 - border) && hitLocalUV.y >= border && hitLocalUV.y <= (1 - border))
					{
						Vector2 loc = chooseCamera.ScreenToWorldPoint(new Vector2(hitLocalUV.x * chooseCamera.pixelWidth, hitLocalUV.y * chooseCamera.pixelHeight));
						movement.move(loc);

						if (chooseObj.GetComponent<ooficonmidcontrolpointr>())
						{
							chooseObj.GetComponent<ooficonmidcontrolpointr>().meshreset.GetComponent<rooficon>().reset();
						}
						//判斷是否為body
						if (chooseObj.transform.parent.GetComponent<body2icon>())
						{
							building.MoveBody(chooseObj.transform.parent.GetComponent<body2icon>().ratio_bodydis);


							building.UpdateBody_B(chooseObj.transform.parent.GetComponent<body2icon>().isbalustrade);
							building.UpdateBody_F(chooseObj.transform.parent.GetComponent<body2icon>().isfrieze);

							//20160916

							building.Move_F(chooseObj.transform.parent.GetComponent<body2icon>().frieze_height, chooseObj.transform.parent.GetComponent<body2icon>().ini_cylinderH);
							building.Move_B(chooseObj.transform.parent.GetComponent<body2icon>().balustrade_height, chooseObj.transform.parent.GetComponent<body2icon>().ini_cylinderH);
						}

						//判斷是否為plat
						if (chooseObj.transform.parent.GetComponent<platform2icon>())
							building.paraplat(chooseObj.transform.parent.GetComponent<platform2icon>().ratio_platdis_h, chooseObj.transform.parent.GetComponent<platform2icon>().ratio_platdis_w);

						//判斷是否為roof
						if (chooseObj.transform.parent.GetComponent<rooficon>())
						{
							building.MoveRoof_Cp1(chooseObj.transform.parent.GetComponent<rooficon>().ControlPoint1Move);
							building.MoveRoof_Cp2(chooseObj.transform.parent.GetComponent<rooficon>().ControlPoint2Move);
							building.MoveRoof_Cp3(chooseObj.transform.parent.GetComponent<rooficon>().ControlPoint3Move);

						}
					}
				}
			}
		}
		else
		{

			if (Input.GetMouseButtonDown(0))
			{
				switch (changeLayoutIndexInWindowsSet)
				{
					case 0:
						int index = ChooseWindow();
						if (index != -1)
						{

							ThisWindowsComponent = AllwindowsComponent[index];
							SetCameraAndGrid(index);
							//選擇視窗
							chooseWindow = windowsList[index];

							for (int i = 0; i < missionTabsList.Count; i++)
							{
								Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(missionTabsList[i].transform);
								if (bounds.Contains(mousePos2World))
								{
									SetInUseTabIndex(index, i);
									break;
								}
							}
						}
						break;
					case 1:
						for (int i = 0; i < buttonList.Count; i++)
						{
							Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(buttonList[i].transform);
							if (bounds.Contains(mousePos2World))
							{
								mainWindows.GetComponent<UITexture>().mainTexture = windowsList[i].GetComponent<UITexture>().mainTexture;
								ThisWindowsComponent = AllwindowsComponent[i];
								SetCameraAndGrid(i);
								SortButtonListToMainWindows(i);
								break;
							}

						}
						//	Debug.Log("missionTabsList.Count:" + missionTabsList.Count);
						for (int i = 0; i < missionTabsList.Count; i++)
						{
							Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(missionTabsList[i].transform);
							if (bounds.Contains(mousePos2World))
							{
								Debug.Log("index:" + i);
								SetInUseTabIndex(mainWindowsinUseIndex, i);
								break;
							}
						}
						break;

				}

				if (chooseWindow != null)
				{

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
	void SwapGameObject(GameObject a, GameObject b)
	{
		Vector3 temp = a.transform.position;

		a.transform.position = b.transform.position;
		b.transform.position = temp;
	}
	void SwapButtonToMainWindows(int index)
	{
		if (mainWindowsinUseIndex == index) return;

		SwapGameObject(buttonList[index], buttonList[mainWindowsinUseIndex]);

		mainWindowsinUseIndex = index;

	}
	void SortButtonListToMainWindows(int index)
	{
		if (mainWindowsinUseIndex == index) return;

		int diff = Mathf.Abs(mainWindowsinUseIndex - index);

		if (diff > 1)
		{
			if (mainWindowsinUseIndex < index)
			{
				for (int i = 0; i < diff - 1; i++)
				{
					SwapGameObject(buttonList[index], buttonList[index - 1 - i]);
				}
			}
			else if (mainWindowsinUseIndex > index)
			{
				for (int i = 0; i < diff - 1; i++)
				{
					SwapGameObject(buttonList[index], buttonList[index + 1 + i]);
				}
			}
		}
		SwapGameObject(buttonList[index], buttonList[mainWindowsinUseIndex]);

		mainWindowsinUseIndex = index;
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

		Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(chooseWindow.transform);
		float targetAspect = (float)bounds.extents.x / bounds.extents.y;
		chooseCamera.aspect = targetAspect;

		//選擇Grid
		chooseGrid.SetActive(false);
		chooseGrid = gridList[index];
		chooseGrid.SetActive(true);

		if (index == 2)
		{
			for (int i = 0; i < missionTabsList.Count; i++)
			{
				missionTabsList[i].SetActive(true);
			}
		}
		else
		{
			for (int i = 0; i < missionTabsList.Count; i++)
			{
				missionTabsList[i].SetActive(false);
			}

		}

		StartCoroutine(RecoverScrollView());
	}
	private IEnumerator RecoverScrollView()
	{
		Vector3 scrollViewRefSpeed = Vector3.zero;
		float scrollViewSmoothTime = 0.3f;
		while (Vector3.Distance(chooseGrid.transform.position, scrollViewOriginPos) >= 0.01f)
		{
			chooseGrid.transform.position = Vector3.SmoothDamp(chooseGrid.transform.position, scrollViewOriginPos, ref scrollViewRefSpeed, scrollViewSmoothTime);
			yield return null;
		}
	}
	void SetInUseTabIndex(int index, int number)
	{

		if (AllwindowsComponent[index].inUseComponentIndex == number) return;

		SaveState2MainComponent(index);
		AllwindowsComponent[index].HideAllComponent();
		AllwindowsComponent[index].inUseComponentIndex = number;


		AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex] = AllwindowsComponent[index].temporateComponent[AllwindowsComponent[index].inUseComponentIndex][AllwindowsComponent[index].lastChooseMainDragObjectName];
		AllwindowsComponent[index].ShowAllComponent();

	}
	void SwitchWindow()
	{
		for (int i = 0; i < windowSetList.Count; i++)
		{
			windowSetList[i].SetActive(false);
		}
		if (changeLayoutIndexInWindowsSet < windowSetList.Count)
			windowSetList[changeLayoutIndexInWindowsSet].SetActive(true);
	}
	void CreateMissionTabs()
	{
		Bounds windowsBounds = NGUIMath.CalculateAbsoluteWidgetBounds(chooseWindow.transform);
		Bounds tabsBounds = NGUIMath.CalculateAbsoluteWidgetBounds(missionTab.transform);
		float offset = tabsBounds.size.x * 0.01f;
		GameObject clone;
		Vector3 pos;

		if (missionTabsList.Count == 0)
		{
			pos = windowsBounds.min + new Vector3(tabsBounds.extents.x, tabsBounds.extents.y, 0.0f);
			clone = Instantiate(missionTab, pos, missionTab.transform.rotation) as GameObject;
			clone.transform.parent = chooseWindow.transform.parent.transform;
			missionTabsList.Add(clone);
		}
		pos = windowsBounds.min + new Vector3((missionTabsList.Count * (tabsBounds.size.x + offset)) + tabsBounds.extents.x, tabsBounds.extents.y, 0.0f);
		clone = Instantiate(missionTab, pos, missionTab.transform.rotation) as GameObject;
		clone.transform.parent = chooseWindow.transform.parent.transform;
		missionTabsList.Add(clone);

	}
	public void SetObjInWindows()
	{
		switch (changeLayoutIndexInWindowsSet)
		{
			case 0:
				int index = ChooseWindow();
				if (index != -1 && chooseDragObject)
				{
					SetCameraAndGrid(index);

					if (windowsList[index] == chooseWindow)
					{
						ThisWindowsComponent = AllwindowsComponent[index];

						SetWindowsComponent(index);
					}
					//選擇視窗
					chooseWindow = windowsList[index];
				}
				break;
			case 1:
				index = ChooseWindow();
				if (index == 4 && chooseDragObject)
				{
					SetCameraAndGrid(mainWindowsinUseIndex);

					ThisWindowsComponent = AllwindowsComponent[mainWindowsinUseIndex];

					SetWindowsComponent(mainWindowsinUseIndex);
				}
				break;
		}
	}
	void SetWindowsComponent(int index)
	{

		SaveState2MainComponent(index);
		switch (chooseDragObject.tag)
		{
			case MAINCOMPONENT:
				movement.freelist.Clear();
				movement.horlist.Clear();
				movement.verlist.Clear();
				if (!AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex].ContainsKey(MAINCOMPONENT))//在選擇的視窗內 且視窗內物件為空
				{
					CreateMainComponent(index);
					Debug.Log("000");
				}
				else//視窗內物件為不為空
				{
					if (AllwindowsComponent[index].lastChooseMainDragObjectName != chooseDragObject.name)//如果不是拖曳同一個主物件取代原本的主物物件
					{
						//紀錄操作的物件
						if (AllwindowsComponent[index].temporateComponent[AllwindowsComponent[index].inUseComponentIndex].ContainsKey(chooseDragObject.name)) //有編輯過此視窗
						{
							AllwindowsComponent[index].HideAllComponent();
							AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex] = AllwindowsComponent[index].temporateComponent[AllwindowsComponent[index].inUseComponentIndex][chooseDragObject.name];
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
					else //如果拖曳同一個主物件
					{
						//取代原本的主物物件 清除此視窗物件
						AllwindowsComponent[index].ClearAllComponent();
						AllwindowsComponent[index].temporateComponent[AllwindowsComponent[index].inUseComponentIndex][chooseDragObject.name].Clear();
						CreateMainComponent(index);
						Debug.Log("333");

					}
				}
				AllwindowsComponent[index].lastChooseMainDragObjectName = chooseDragObject.name;
				if (AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][MAINCOMPONENT][0].GetComponent<MeshObj>()) building.UpdateAll(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][MAINCOMPONENT][0].GetComponent<MeshObj>().edgeIndex);

				break;
			case DECORATECOMPONENT:

				if (AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex].ContainsKey(MAINCOMPONENT))//如果有拖曳物件 且在選擇的視窗內 且視窗內物件為空
				{
					CreateDecorateComponent(index);

					if (AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][MAINCOMPONENT][0].GetComponent<body2icon>())
					{

						if (AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex].ContainsKey(chooseDragObject.name)) AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][MAINCOMPONENT][0].GetComponent<body2icon>().UpdateFunction(chooseDragObject.name, AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][chooseDragObject.name].Count);

						building.UpdateBody_F(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][MAINCOMPONENT][0].GetComponent<body2icon>().isbalustrade);
						building.UpdateBody_B(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][MAINCOMPONENT][0].GetComponent<body2icon>().isfrieze);



						building.Move_F(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][MAINCOMPONENT][0].GetComponent<body2icon>().frieze_height, AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][MAINCOMPONENT][0].GetComponent<body2icon>().ini_cylinderH);
						building.Move_B(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][MAINCOMPONENT][0].GetComponent<body2icon>().balustrade_height, AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][MAINCOMPONENT][0].GetComponent<body2icon>().ini_cylinderH);

					}

					//frieze ＆ Balustrade
				}

				break;
		}
	}
	void SaveState2MainComponent(int index)
	{
		if (AllwindowsComponent[index].lastChooseMainDragObjectName != null)
		{
			if (!AllwindowsComponent[index].temporateComponent[AllwindowsComponent[index].inUseComponentIndex].ContainsKey(AllwindowsComponent[index].lastChooseMainDragObjectName))
			{
				Debug.Log("First");
				Dictionary<string, List<GameObject>> copy = new Dictionary<string, List<GameObject>>(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex]);
				AllwindowsComponent[index].temporateComponent[AllwindowsComponent[index].inUseComponentIndex].Add(AllwindowsComponent[index].lastChooseMainDragObjectName, copy);
			}
			else
			{
				Debug.Log("Second");
				Dictionary<string, List<GameObject>> copy = new Dictionary<string, List<GameObject>>(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex]);
				AllwindowsComponent[index].temporateComponent[AllwindowsComponent[index].inUseComponentIndex][AllwindowsComponent[index].lastChooseMainDragObjectName] = copy;
			}
		}
	}
	void CreateMainComponent(int index)
	{
		Vector3 pos = chooseCamera.transform.position; pos.z = chooseCamera.farClipPlane / 2.0f;

		GameObject cloneCorrespondingObj = chooseDragObject.GetComponent<CorespondingDragItem>().corespondingDragItem;

		GameObject clone = Instantiate(cloneCorrespondingObj, pos, cloneCorrespondingObj.transform.rotation) as GameObject;
		clone.transform.parent = this.transform;

		List<GameObject> allComponentList = new List<GameObject>();
		allComponentList.Add(clone);
		AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex].Add(MAINCOMPONENT, allComponentList);
	}
	void CreateDecorateComponent(int index)
	{
		Vector3 pos = chooseCamera.transform.position; pos.z = chooseCamera.farClipPlane / 2.0f;
		GameObject cloneCorrespondingObj = chooseDragObject.GetComponent<CorespondingDragItem>().corespondingDragItem;
		int correspondingDragItemMaxCount = chooseDragObject.GetComponent<CorespondingDragItem>().correspondingDragItemMaxCount;

		if (chooseDragObject.name == "MutiBody")
		{
			if (missionTabsList.Count < correspondingDragItemMaxCount)
			{
				SaveState2MainComponent(index);
				AllwindowsComponent[index].HideAllComponent();
				AllwindowsComponent[index].inUseComponentIndex = AllwindowsComponent[index].allComponent.Count;

				Dictionary<string, List<GameObject>> newAllComponent = new Dictionary<string, List<GameObject>>();
				AllwindowsComponent[index].allComponent.Add(newAllComponent);
				Dictionary<string, Dictionary<string, List<GameObject>>> newTemporateComponent = new Dictionary<string, Dictionary<string, List<GameObject>>>();
				AllwindowsComponent[index].temporateComponent.Add(newTemporateComponent);
				

				CreateMainComponent(index);

				CreateMissionTabs();
			}

		}
		else
		{

			if (AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex].ContainsKey(chooseDragObject.name))
			{
				if (AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][chooseDragObject.name].Count < correspondingDragItemMaxCount)
				{
					GameObject clone = Instantiate(cloneCorrespondingObj, pos, cloneCorrespondingObj.transform.rotation) as GameObject;
					clone.transform.parent = this.transform;
					AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][chooseDragObject.name].Add(clone);
				}
				else
					Debug.Log(chooseDragObject.name + "    Count over MaxCount");
			}
			else
			{
				GameObject clone = Instantiate(cloneCorrespondingObj, pos, cloneCorrespondingObj.transform.rotation) as GameObject;
				clone.transform.parent = this.transform;
				List<GameObject> newList = new List<GameObject>();
				newList.Clear();
				newList.Add(clone);
				AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex].Add(chooseDragObject.name, newList);
			}
		}
	}
}
*/

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class WindowsList : MonoBehaviour
{
	public List<Dictionary<string, List<GameObject>>> allComponent;
	public List<Dictionary<string, Dictionary<string, List<GameObject>>>> temporateComponent;
	public string lastChooseMainDragObjectName = null;
	public int inUseComponentIndex = 0;

	public void PrintAllComponentCount()
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allComponent[inUseComponentIndex])
		{
			Debug.Log(kvp.Key + kvp.Value.Count);
		}
	}

	public void ClearAllComponent()
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allComponent[inUseComponentIndex])
		{
			//childComponent.Remove(kvp.Key);......??????
			for (int i = 0; i < kvp.Value.Count; i++)
			{
				Destroy(kvp.Value[i]);
			}
		}
		allComponent[inUseComponentIndex].Clear();
	}
	public void HideAllComponent()
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allComponent[inUseComponentIndex])
		{
			for (int i = 0; i < kvp.Value.Count; i++)
				(kvp.Value[i]).SetActive(false);
		}
		allComponent[inUseComponentIndex].Clear();
	}
	public void ShowAllComponent()
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allComponent[inUseComponentIndex])
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
	public GameObject chooseObj = null;
	public GameObject chooseWindow;
	public GameObject chooseGrid;
	private Camera chooseCamera;
	//UICamera
	private Camera uICamera;
	//四大視窗
	public List<GameObject> windowsList = new List<GameObject>();
	public List<GameObject> gridList = new List<GameObject>();
	public List<Camera> cameraList = new List<Camera>();
	//
	public List<GameObject> windowSetList = new List<GameObject>();
	//
	public List<GameObject> buttonList = new List<GameObject>();


	//四個視窗中的物件集合
	private WindowsList[] AllwindowsComponent;
	//當前使用視窗中的物件集合
	[HideInInspector]
	public WindowsList ThisWindowsComponent;
	//
	private Vector3 scrollViewOriginPos;
	//
	public int changeLayoutIndexInWindowsSet = 0;
	private int mainWindowsinUseIndex = 0;
	private GameObject mainWindows;
	//
	private Movement movement;
	private AllInOne building;
	//
	public GameObject missionTab;
	private List<GameObject> missionTabsList = new List<GameObject>();
	private int missionTabsCount = 0;
	void Start()
	{
		uICamera = GameObject.Find("UICamera").GetComponent<Camera>();
		movement = GameObject.Find("Movement").GetComponent<Movement>();
		building = GameObject.Find("build").GetComponent<AllInOne>();
		InitWindowListMemorySetting();
		InitStateSetting();
		SwitchWindow();
	}
	void Update()
	{
		RayCastToChooseObj();
		if (Input.GetKeyDown(KeyCode.Delete))
		{
			DeleteMuliBody();
		}

	}
	void DeleteMuliBody()
	{
		if (missionTabsList.Count == 0) return;
		int index = ChooseWindow();

	}
	void InitWindowListMemorySetting()
	{
		AllwindowsComponent = new WindowsList[windowsList.Count];
		for (int i = 0; i < windowsList.Count; i++)
		{
			AllwindowsComponent[i] = new WindowsList();
			AllwindowsComponent[i].allComponent = new List<Dictionary<string, List<GameObject>>>();
			AllwindowsComponent[i].temporateComponent = new List<Dictionary<string, Dictionary<string, List<GameObject>>>>();
			Dictionary<string, List<GameObject>> newAllComponent = new Dictionary<string, List<GameObject>>();
			AllwindowsComponent[i].allComponent.Add(newAllComponent);
			Dictionary<string, Dictionary<string, List<GameObject>>> newTemporateComponent = new Dictionary<string, Dictionary<string, List<GameObject>>>();
			AllwindowsComponent[i].temporateComponent.Add(newTemporateComponent);
		}
	}
	void InitStateSetting()
	{
		scrollViewOriginPos = gridList[0].transform.position;
		chooseGrid = gridList[0];
		SetCameraAndGrid(0);

		mainWindows = windowsList[4];

		if (changeLayoutIndexInWindowsSet == 0)
			chooseWindow = windowsList[0];
		else if (changeLayoutIndexInWindowsSet == 1)
			chooseWindow = mainWindows;
	}
	void RayCastToChooseObj()
	{
		Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		Vector2 mousePos2World = uICamera.ScreenToWorldPoint(mousePos);

		if (chooseObj)
		{
			if (Input.GetMouseButtonUp(0))
			{
				chooseObj.GetComponent<Collider>().enabled = true;
				chooseObj = null;
				//
				building.UpdateRoof();
				return;
			}
			else
			{
				Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(chooseWindow.transform);
				if (bounds.Contains(mousePos2World))
				{
					Vector2 hitLocalUV;
					Vector3 localHit = mousePos2World;

					// normalize
					hitLocalUV.x = (localHit.x - bounds.min.x) / (bounds.size.x);
					hitLocalUV.y = (localHit.y - bounds.min.y) / (bounds.size.y);
					float border = 0.2f;

					if (hitLocalUV.x >= border && hitLocalUV.x <= (1 - border) && hitLocalUV.y >= border && hitLocalUV.y <= (1 - border))
					{
						Vector2 loc = chooseCamera.ScreenToWorldPoint(new Vector2(hitLocalUV.x * chooseCamera.pixelWidth, hitLocalUV.y * chooseCamera.pixelHeight));
						movement.Move(loc);

						if (chooseObj.GetComponent<ooficonmidcontrolpointr>())
						{
							chooseObj.GetComponent<ooficonmidcontrolpointr>().meshreset.GetComponent<rooficon>().reset();
						}
						//判斷是否為body
						if (chooseObj.transform.parent.GetComponent<body2icon>())
						{
							building.MoveBody(chooseObj.transform.parent.GetComponent<body2icon>().ratio_bodydis);


							building.UpdateBody_B(chooseObj.transform.parent.GetComponent<body2icon>().isbalustrade);
							building.UpdateBody_F(chooseObj.transform.parent.GetComponent<body2icon>().isfrieze);

							//20160916

							building.Move_F(chooseObj.transform.parent.GetComponent<body2icon>().friezeHeight, chooseObj.transform.parent.GetComponent<body2icon>().ini_cylinderHeight);
							building.Move_B(chooseObj.transform.parent.GetComponent<body2icon>().balustradeHeight, chooseObj.transform.parent.GetComponent<body2icon>().ini_cylinderHeight);
						}

						//判斷是否為plat
						if (chooseObj.transform.parent.GetComponent<platform2icon>())
							building.paraplat(chooseObj.transform.parent.GetComponent<platform2icon>().chang_platdis.y, chooseObj.transform.parent.GetComponent<platform2icon>().chang_platdis.x);

						//判斷是否為roof
						if (chooseObj.transform.parent.GetComponent<rooficon>())
						{
							building.MoveRoof_Cp1(chooseObj.transform.parent.GetComponent<rooficon>().ControlPoint1Move);
							building.MoveRoof_Cp2(chooseObj.transform.parent.GetComponent<rooficon>().ControlPoint2Move);
							building.MoveRoof_Cp3(chooseObj.transform.parent.GetComponent<rooficon>().ControlPoint3Move);

						}
					}
				}
			}
		}
		else
		{

			if (Input.GetMouseButtonDown(0))
			{
				switch (changeLayoutIndexInWindowsSet)
				{
					case 0:
						int index = ChooseWindow();
						if (index != -1)
						{

							ThisWindowsComponent = AllwindowsComponent[index];
							SetCameraAndGrid(index);
							//選擇視窗
							chooseWindow = windowsList[index];

							for (int i = 0; i < missionTabsList.Count; i++)
							{
								Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(missionTabsList[i].transform);
								if (bounds.Contains(mousePos2World))
								{
									SetInUseTabIndex(index, i);
									break;
								}
							}
						}
						break;
					case 1:
						for (int i = 0; i < buttonList.Count; i++)
						{
							Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(buttonList[i].transform);
							if (bounds.Contains(mousePos2World))
							{
								mainWindows.GetComponent<UITexture>().mainTexture = windowsList[i].GetComponent<UITexture>().mainTexture;
								ThisWindowsComponent = AllwindowsComponent[i];
								SetCameraAndGrid(i);
								SortButtonListToMainWindows(i);
								break;
							}

						}
						//	Debug.Log("missionTabsList.Count:" + missionTabsList.Count);
						for (int i = 0; i < missionTabsList.Count; i++)
						{
							Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(missionTabsList[i].transform);
							if (bounds.Contains(mousePos2World))
							{
								Debug.Log("index:" + i);
								SetInUseTabIndex(mainWindowsinUseIndex, i);
								break;
							}
						}
						break;

				}

				if (chooseWindow != null)
				{

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
								movement.intiAllList();
								if (chooseObj.transform.parent.GetComponent<MeshObj>())
								{
									chooseObj.transform.parent.GetComponent<MeshObj>().addpoint();
								}
								if (chooseObj.transform.parent.GetComponent<platform2icon>())
								{
									chooseObj.transform.parent.GetComponent<platform2icon>().addpoint();
								}
								if (chooseObj.transform.parent.GetComponent<body2icon>())
								{
									chooseObj.transform.parent.GetComponent<body2icon>().addpoint();
								}
								if (chooseObj.transform.parent.GetComponent<rooficon>())
								{
									chooseObj.transform.parent.GetComponent<rooficon>().addpoint();
								}
								chooseObj.GetComponent<Collider>().enabled = false;
							}
						}
					}

				}

			}
		}
	}
	void SwapGameObject(GameObject a, GameObject b)
	{
		Vector3 temp = a.transform.position;

		a.transform.position = b.transform.position;
		b.transform.position = temp;
	}
	void SwapButtonToMainWindows(int index)
	{
		if (mainWindowsinUseIndex == index) return;

		SwapGameObject(buttonList[index], buttonList[mainWindowsinUseIndex]);

		mainWindowsinUseIndex = index;

	}
	void SortButtonListToMainWindows(int index)
	{
		if (mainWindowsinUseIndex == index) return;

		int diff = Mathf.Abs(mainWindowsinUseIndex - index);

		if (diff > 1)
		{
			if (mainWindowsinUseIndex < index)
			{
				for (int i = 0; i < diff - 1; i++)
				{
					SwapGameObject(buttonList[index], buttonList[index - 1 - i]);
				}
			}
			else if (mainWindowsinUseIndex > index)
			{
				for (int i = 0; i < diff - 1; i++)
				{
					SwapGameObject(buttonList[index], buttonList[index + 1 + i]);
				}
			}
		}
		SwapGameObject(buttonList[index], buttonList[mainWindowsinUseIndex]);

		mainWindowsinUseIndex = index;
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

		Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(chooseWindow.transform);
		float targetAspect = (float)bounds.extents.x / bounds.extents.y;
		chooseCamera.aspect = targetAspect;

		//選擇Grid
		chooseGrid.SetActive(false);
		chooseGrid = gridList[index];
		chooseGrid.SetActive(true);

		if (index == 2)
		{
			for (int i = 0; i < missionTabsList.Count; i++)
			{
				missionTabsList[i].SetActive(true);
			}
		}
		else
		{
			for (int i = 0; i < missionTabsList.Count; i++)
			{
				missionTabsList[i].SetActive(false);
			}

		}

		StartCoroutine(RecoverScrollView());
	}
	private IEnumerator RecoverScrollView()
	{
		Vector3 scrollViewRefSpeed = Vector3.zero;
		float scrollViewSmoothTime = 0.3f;
		while (Vector3.Distance(chooseGrid.transform.position, scrollViewOriginPos) >= 0.01f)
		{
			chooseGrid.transform.position = Vector3.SmoothDamp(chooseGrid.transform.position, scrollViewOriginPos, ref scrollViewRefSpeed, scrollViewSmoothTime);
			yield return null;
		}
	}
	void SetInUseTabIndex(int index, int number)
	{

		if (AllwindowsComponent[index].inUseComponentIndex == number) return;

		SaveState2MainComponent(index);
		AllwindowsComponent[index].HideAllComponent();
		AllwindowsComponent[index].inUseComponentIndex = number;


		AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex] = AllwindowsComponent[index].temporateComponent[AllwindowsComponent[index].inUseComponentIndex][AllwindowsComponent[index].lastChooseMainDragObjectName];
		AllwindowsComponent[index].ShowAllComponent();

	}
	void SwitchWindow()
	{
		for (int i = 0; i < windowSetList.Count; i++)
		{
			windowSetList[i].SetActive(false);
		}
		if (changeLayoutIndexInWindowsSet < windowSetList.Count)
			windowSetList[changeLayoutIndexInWindowsSet].SetActive(true);
	}
	void CreateMissionTabs()
	{
		Bounds windowsBounds = NGUIMath.CalculateAbsoluteWidgetBounds(chooseWindow.transform);
		Bounds tabsBounds = NGUIMath.CalculateAbsoluteWidgetBounds(missionTab.transform);
		float offset = tabsBounds.size.x * 0.01f;
		GameObject clone;
		Vector3 pos;

		if (missionTabsList.Count == 0)
		{
			pos = windowsBounds.min + new Vector3(tabsBounds.extents.x, tabsBounds.extents.y, 0.0f);
			clone = Instantiate(missionTab, pos, missionTab.transform.rotation) as GameObject;
			clone.transform.parent = chooseWindow.transform.parent.transform;
			missionTabsList.Add(clone);
		}
		pos = windowsBounds.min + new Vector3((missionTabsList.Count * (tabsBounds.size.x + offset)) + tabsBounds.extents.x, tabsBounds.extents.y, 0.0f);
		clone = Instantiate(missionTab, pos, missionTab.transform.rotation) as GameObject;
		clone.transform.parent = chooseWindow.transform.parent.transform;
		missionTabsList.Add(clone);

	}
	public void SetObjInWindows()
	{
		switch (changeLayoutIndexInWindowsSet)
		{
			case 0:
				int index = ChooseWindow();
				if (index != -1 && chooseDragObject)
				{
					SetCameraAndGrid(index);

					if (windowsList[index] == chooseWindow)
					{
						ThisWindowsComponent = AllwindowsComponent[index];

						SetWindowsComponent(index);
					}
					//選擇視窗
					chooseWindow = windowsList[index];
				}
				break;
			case 1:
				index = ChooseWindow();
				if (index == 4 && chooseDragObject)
				{
					SetCameraAndGrid(mainWindowsinUseIndex);

					ThisWindowsComponent = AllwindowsComponent[mainWindowsinUseIndex];

					SetWindowsComponent(mainWindowsinUseIndex);
				}
				break;
		}
	}
	void SetWindowsComponent(int index)
	{

		SaveState2MainComponent(index);
		switch (chooseDragObject.tag)
		{
			case MAINCOMPONENT:
				movement.intiAllList();
				Debug.Log(AllwindowsComponent[index].lastChooseMainDragObjectName);
				if (!AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex].ContainsKey(MAINCOMPONENT))//在選擇的視窗內 且視窗內物件為空
				{
					CreateMainComponent(index);
					Debug.Log("000");
				}
				else//視窗內物件為不為空
				{
					if (AllwindowsComponent[index].lastChooseMainDragObjectName != chooseDragObject.name)//如果不是拖曳同一個主物件取代原本的主物物件
					{
						//紀錄操作的物件
						if (AllwindowsComponent[index].temporateComponent[AllwindowsComponent[index].inUseComponentIndex].ContainsKey(chooseDragObject.name)) //有編輯過此視窗
						{
							AllwindowsComponent[index].HideAllComponent();
							AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex] = AllwindowsComponent[index].temporateComponent[AllwindowsComponent[index].inUseComponentIndex][chooseDragObject.name];
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
					else //如果拖曳同一個主物件
					{
						//取代原本的主物物件 清除此視窗物件
						AllwindowsComponent[index].ClearAllComponent();
						AllwindowsComponent[index].temporateComponent[AllwindowsComponent[index].inUseComponentIndex][chooseDragObject.name].Clear();
						CreateMainComponent(index);
						Debug.Log("333");

					}
				}
				AllwindowsComponent[index].lastChooseMainDragObjectName = chooseDragObject.name;
				if (AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][MAINCOMPONENT][0].GetComponent<MeshObj>()) building.UpdateAll(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][MAINCOMPONENT][0].GetComponent<MeshObj>().edgeIndex);
				if (AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][MAINCOMPONENT][0].GetComponent<body2icon>())
				{
					building.UpdateBody_B(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][MAINCOMPONENT][0].GetComponent<body2icon>().isbalustrade);
					building.UpdateBody_F(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][MAINCOMPONENT][0].GetComponent<body2icon>().isfrieze);
				}
				break;
			case DECORATECOMPONENT:

				if (AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex].ContainsKey(MAINCOMPONENT))//如果有拖曳物件 且在選擇的視窗內 且視窗內物件為空
				{
					CreateDecorateComponent(index);

					if (AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][MAINCOMPONENT][0].GetComponent<body2icon>())
					{

						if (AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex].ContainsKey(chooseDragObject.name)) AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][MAINCOMPONENT][0].GetComponent<body2icon>().UpdateFunction(chooseDragObject.name);

						building.UpdateBody_F(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][MAINCOMPONENT][0].GetComponent<body2icon>().isbalustrade);
						building.UpdateBody_B(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][MAINCOMPONENT][0].GetComponent<body2icon>().isfrieze);



						building.Move_F(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][MAINCOMPONENT][0].GetComponent<body2icon>().friezeHeight, AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][MAINCOMPONENT][0].GetComponent<body2icon>().ini_cylinderHeight);
						building.Move_B(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][MAINCOMPONENT][0].GetComponent<body2icon>().balustradeHeight, AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][MAINCOMPONENT][0].GetComponent<body2icon>().ini_cylinderHeight);

					}

					//frieze ＆ Balustrade
				}

				break;
		}
	}
	void SaveState2MainComponent(int index)
	{
		if (AllwindowsComponent[index].lastChooseMainDragObjectName != null)
		{
			if (!AllwindowsComponent[index].temporateComponent[AllwindowsComponent[index].inUseComponentIndex].ContainsKey(AllwindowsComponent[index].lastChooseMainDragObjectName))
			{
				Debug.Log("First");
				Dictionary<string, List<GameObject>> copy = new Dictionary<string, List<GameObject>>(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex]);
				AllwindowsComponent[index].temporateComponent[AllwindowsComponent[index].inUseComponentIndex].Add(AllwindowsComponent[index].lastChooseMainDragObjectName, copy);
			}
			else
			{
				Debug.Log("Second");
				Dictionary<string, List<GameObject>> copy = new Dictionary<string, List<GameObject>>(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex]);
				AllwindowsComponent[index].temporateComponent[AllwindowsComponent[index].inUseComponentIndex][AllwindowsComponent[index].lastChooseMainDragObjectName] = copy;
			}
		}
	}
	void CreateMainComponent(int index)
	{
		Vector3 pos = chooseCamera.transform.position; pos.z = chooseCamera.farClipPlane / 2.0f;

		GameObject cloneCorrespondingObj = chooseDragObject.GetComponent<CorespondingDragItem>().corespondingDragItem;

		GameObject clone = Instantiate(cloneCorrespondingObj, pos, cloneCorrespondingObj.transform.rotation) as GameObject;
		clone.transform.parent = this.transform;

		List<GameObject> allComponentList = new List<GameObject>();
		allComponentList.Add(clone);
		AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex].Add(MAINCOMPONENT, allComponentList);
	}
	void CreateDecorateComponent(int index)
	{
		Vector3 pos = chooseCamera.transform.position; pos.z = chooseCamera.farClipPlane / 2.0f;
		GameObject cloneCorrespondingObj = chooseDragObject.GetComponent<CorespondingDragItem>().corespondingDragItem;
		int correspondingDragItemMaxCount = chooseDragObject.GetComponent<CorespondingDragItem>().correspondingDragItemMaxCount;

		if (chooseDragObject.name == "MutiBody")
		{
			if (missionTabsList.Count < correspondingDragItemMaxCount)
			{
				SaveState2MainComponent(index);
				AllwindowsComponent[index].HideAllComponent();
				AllwindowsComponent[index].inUseComponentIndex = AllwindowsComponent[index].allComponent.Count;

				Dictionary<string, List<GameObject>> newAllComponent = new Dictionary<string, List<GameObject>>();
				AllwindowsComponent[index].allComponent.Add(newAllComponent);
				Dictionary<string, Dictionary<string, List<GameObject>>> newTemporateComponent = new Dictionary<string, Dictionary<string, List<GameObject>>>();
				AllwindowsComponent[index].temporateComponent.Add(newTemporateComponent);


				CreateMainComponent(index);

				CreateMissionTabs();
			}

		}
		else
		{

			if (AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex].ContainsKey(chooseDragObject.name))
			{
				if (AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][chooseDragObject.name].Count < correspondingDragItemMaxCount)
				{
					GameObject clone = Instantiate(cloneCorrespondingObj, pos, cloneCorrespondingObj.transform.rotation) as GameObject;
					clone.transform.parent = this.transform;
					AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex][chooseDragObject.name].Add(clone);
				}
				else
					Debug.Log(chooseDragObject.name + "    Count over MaxCount");
			}
			else
			{
				GameObject clone = Instantiate(cloneCorrespondingObj, pos, cloneCorrespondingObj.transform.rotation) as GameObject;
				clone.transform.parent = this.transform;
				List<GameObject> newList = new List<GameObject>();
				newList.Clear();
				newList.Add(clone);
				AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseComponentIndex].Add(chooseDragObject.name, newList);
			}
		}
	}
}
