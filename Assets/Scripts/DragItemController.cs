/*
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class WindowsList : MonoBehaviour
{
	public List<Dictionary<string, List<GameObject>>> allComponent;
	public List<Dictionary<string, Dictionary<string, List<GameObject>>>> temporateAllFloorItem;
	public string lastChooseMainDragObjectName = null;
	public int inUseTab2ComponentLayerIndex = 0;

	public void PrintAllComponentCount()
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allComponent[inUseTab2ComponentLayerIndex])
		{
			Debug.Log(kvp.Key + kvp.Value.Count);
		}
	}

	public void ClearAllComponent()
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allComponent[inUseTab2ComponentLayerIndex])
		{
			//childComponent.Remove(kvp.Key);......??????
			for (int i = 0; i < kvp.Value.Count; i++)
			{
				Destroy(kvp.Value[i]);
			}
		}
		allComponent[inUseTab2ComponentLayerIndex].Clear();
	}
	public void HideAllComponent()
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allComponent[inUseTab2ComponentLayerIndex])
		{
			for (int i = 0; i < kvp.Value.Count; i++)
				(kvp.Value[i]).SetActive(false);
		}
		allComponent[inUseTab2ComponentLayerIndex].Clear();
	}
	public void ShowAllComponent()
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allComponent[inUseTab2ComponentLayerIndex])
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
	private int mainSingleWindowinUseIndex = 0;
	private GameObject mainSingleWindow;
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
			AllwindowsComponent[i].temporateAllFloorItem = new List<Dictionary<string, Dictionary<string, List<GameObject>>>>();
			Dictionary<string, List<GameObject>> newAllComponent = new Dictionary<string, List<GameObject>>();
			AllwindowsComponent[i].allComponent.Add(newAllComponent);
			Dictionary<string, Dictionary<string, List<GameObject>>> newtemporateAllFloorItem = new Dictionary<string, Dictionary<string, List<GameObject>>>();
			AllwindowsComponent[i].temporateAllFloorItem.Add(newtemporateAllFloorItem);
		}
	}
	void InitStateSetting()
	{
		scrollViewOriginPos = gridList[0].transform.position;
		chooseGrid = gridList[0];
		SetCameraAndGrid(0);

		mainSingleWindow = windowsList[4];

		if (changeLayoutIndexInWindowsSet == 0)
			chooseWindow = windowsList[0];
		else if (changeLayoutIndexInWindowsSet == 1)
			chooseWindow = mainSingleWindow;
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


							building.UpdateBody_B(chooseObj.transform.parent.GetComponent<body2icon>().isBalustrade);
							building.UpdateBody_F(chooseObj.transform.parent.GetComponent<body2icon>().isFrieze);

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
								mainSingleWindow.GetComponent<UITexture>().mainTexture = windowsList[i].GetComponent<UITexture>().mainTexture;
								ThisWindowsComponent = AllwindowsComponent[i];
								SetCameraAndGrid(i);
								SortButtonListTomainSingleWindow(i);
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
								SetInUseTabIndex(mainSingleWindowinUseIndex, i);
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
	void SwapButtonTomainSingleWindow(int index)
	{
		if (mainSingleWindowinUseIndex == index) return;

		SwapGameObject(buttonList[index], buttonList[mainSingleWindowinUseIndex]);

		mainSingleWindowinUseIndex = index;

	}
	void SortButtonListTomainSingleWindow(int index)
	{
		if (mainSingleWindowinUseIndex == index) return;

		int diff = Mathf.Abs(mainSingleWindowinUseIndex - index);

		if (diff > 1)
		{
			if (mainSingleWindowinUseIndex < index)
			{
				for (int i = 0; i < diff - 1; i++)
				{
					SwapGameObject(buttonList[index], buttonList[index - 1 - i]);
				}
			}
			else if (mainSingleWindowinUseIndex > index)
			{
				for (int i = 0; i < diff - 1; i++)
				{
					SwapGameObject(buttonList[index], buttonList[index + 1 + i]);
				}
			}
		}
		SwapGameObject(buttonList[index], buttonList[mainSingleWindowinUseIndex]);

		mainSingleWindowinUseIndex = index;
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

		if (AllwindowsComponent[index].inUseTab2ComponentLayerIndex == number) return;

		SaveState2MainComponent(index);
		AllwindowsComponent[index].HideAllComponent();
		AllwindowsComponent[index].inUseTab2ComponentLayerIndex = number;


		AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex] = AllwindowsComponent[index].temporateAllFloorItem[AllwindowsComponent[index].inUseTab2ComponentLayerIndex][AllwindowsComponent[index].lastChooseMainDragObjectName];
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
					SetCameraAndGrid(mainSingleWindowinUseIndex);

					ThisWindowsComponent = AllwindowsComponent[mainSingleWindowinUseIndex];

					SetWindowsComponent(mainSingleWindowinUseIndex);
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
				if (!AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex].ContainsKey(MAINCOMPONENT))//在選擇的視窗內 且視窗內物件為空
				{
					CreateMainComponent(index);
					Debug.Log("000");
				}
				else//視窗內物件為不為空
				{
					if (AllwindowsComponent[index].lastChooseMainDragObjectName != chooseDragObject.name)//如果不是拖曳同一個主物件取代原本的主物物件
					{
						//紀錄操作的物件
						if (AllwindowsComponent[index].temporateAllFloorItem[AllwindowsComponent[index].inUseTab2ComponentLayerIndex].ContainsKey(chooseDragObject.name)) //有編輯過此視窗
						{
							AllwindowsComponent[index].HideAllComponent();
							AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex] = AllwindowsComponent[index].temporateAllFloorItem[AllwindowsComponent[index].inUseTab2ComponentLayerIndex][chooseDragObject.name];
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
						AllwindowsComponent[index].temporateAllFloorItem[AllwindowsComponent[index].inUseTab2ComponentLayerIndex][chooseDragObject.name].Clear();
						CreateMainComponent(index);
						Debug.Log("333");

					}
				}
				AllwindowsComponent[index].lastChooseMainDragObjectName = chooseDragObject.name;
				if (AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<MeshObj>()) building.UpdateAll(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<MeshObj>().edgeIndex);
				if (AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>())
				{
					building.UpdateBody_B(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().isBalustrade);
					building.UpdateBody_F(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().isFrieze);
				}
				break;
			case DECORATECOMPONENT:

				if (AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex].ContainsKey(MAINCOMPONENT))//如果有拖曳物件 且在選擇的視窗內 且視窗內物件為空
				{
					CreateDecorateComponent(index);

					if (AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>())
					{

						if (AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex].ContainsKey(chooseDragObject.name)) AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().UpdateFunction(chooseDragObject.name);

						building.UpdateBody_F(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().isBalustrade);
						building.UpdateBody_B(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().isFrieze);



						building.Move_F(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().friezeHeight, AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().ini_cylinderHeight);
						building.Move_B(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().balustradeHeight, AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().ini_cylinderHeight);

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
			if (!AllwindowsComponent[index].temporateAllFloorItem[AllwindowsComponent[index].inUseTab2ComponentLayerIndex].ContainsKey(AllwindowsComponent[index].lastChooseMainDragObjectName))
			{
				Debug.Log("First");
				Dictionary<string, List<GameObject>> copy = new Dictionary<string, List<GameObject>>(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex]);
				AllwindowsComponent[index].temporateAllFloorItem[AllwindowsComponent[index].inUseTab2ComponentLayerIndex].Add(AllwindowsComponent[index].lastChooseMainDragObjectName, copy);
			}
			else
			{
				Debug.Log("Second");
				Dictionary<string, List<GameObject>> copy = new Dictionary<string, List<GameObject>>(AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex]);
				AllwindowsComponent[index].temporateAllFloorItem[AllwindowsComponent[index].inUseTab2ComponentLayerIndex][AllwindowsComponent[index].lastChooseMainDragObjectName] = copy;
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
		AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex].Add(MAINCOMPONENT, allComponentList);
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
				AllwindowsComponent[index].inUseTab2ComponentLayerIndex = AllwindowsComponent[index].allComponent.Count;

				Dictionary<string, List<GameObject>> newAllComponent = new Dictionary<string, List<GameObject>>();
				AllwindowsComponent[index].allComponent.Add(newAllComponent);
				Dictionary<string, Dictionary<string, List<GameObject>>> newtemporateAllFloorItem = new Dictionary<string, Dictionary<string, List<GameObject>>>();
				AllwindowsComponent[index].temporateAllFloorItem.Add(newtemporateAllFloorItem);


				CreateMainComponent(index);

				CreateMissionTabs();
			}

		}
		else
		{

			if (AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex].ContainsKey(chooseDragObject.name))
			{
				if (AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex][chooseDragObject.name].Count < correspondingDragItemMaxCount)
				{
					GameObject clone = Instantiate(cloneCorrespondingObj, pos, cloneCorrespondingObj.transform.rotation) as GameObject;
					clone.transform.parent = this.transform;
					AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex][chooseDragObject.name].Add(clone);
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
				AllwindowsComponent[index].allComponent[AllwindowsComponent[index].inUseTab2ComponentLayerIndex].Add(chooseDragObject.name, newList);
			}
		}
	}
}
*/
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class MisstionTab : MonoBehaviour
{
	public List<GameObject> missionTabsList = new List<GameObject>();
	public GameObject deleteButton=null;
	public int inUseIndex;
	public void CreateMissionTabs<T>(T thisGameObject, GameObject missionTabObj, GameObject chooseWindow) where T : Component
	{
		Bounds windowsBounds = NGUIMath.CalculateAbsoluteWidgetBounds(chooseWindow.transform);
		Bounds tabsBounds = NGUIMath.CalculateAbsoluteWidgetBounds(missionTabObj.transform);
		float offset = tabsBounds.size.x * 0.01f;
		GameObject clone;
		Vector3 pos;
		//加入樓icon要加入兩個missionTab按鈕 控制一、二層
		if (missionTabsList.Count == 0)
		{
			//missionTab按鈕
			pos = windowsBounds.min + new Vector3(tabsBounds.extents.x, tabsBounds.extents.y, 0.0f);
			clone = Instantiate(missionTabObj, pos, missionTabObj.transform.rotation) as GameObject;
			clone.transform.parent = chooseWindow.transform.parent.transform;
			missionTabsList.Add(clone);
			//deleteButton
			pos = windowsBounds.max - new Vector3(tabsBounds.extents.x, tabsBounds.extents.y, 0.0f);
			clone = Instantiate(missionTabObj, pos, missionTabObj.transform.rotation) as GameObject;
			clone.transform.parent = chooseWindow.transform.parent.transform;
			deleteButton = clone;
		}
		pos = windowsBounds.min + new Vector3((missionTabsList.Count * (tabsBounds.size.x + offset)) + tabsBounds.extents.x, tabsBounds.extents.y, 0.0f);
		clone = Instantiate(missionTabObj, pos, missionTabObj.transform.rotation) as GameObject;
		clone.transform.parent = chooseWindow.transform.parent.transform;
		missionTabsList.Add(clone);
	}
	public int ChooseInUseMissionTabsIndex(Vector2 mousePos)//選擇正在使用的missionTab
	{
		for (int i = 0; i < missionTabsList.Count; i++)
		{
			Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(missionTabsList[i].transform);
			if (bounds.Contains(mousePos))
			{
				inUseIndex = i;
				return inUseIndex;
			}
		}
		return -1;
	}
	public bool ChooseMissionTabsDeleteButton(Vector2 mousePos)//選擇正在使用的missionTab
	{
		Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(deleteButton.transform);
		if (bounds.Contains(mousePos))
		{
			return true;
		}
		return false;
	}
	public bool IsNotOverMaxCount(int maxCount)//判斷missionTab按鈕是否超過上限
	{
		return missionTabsList.Count < maxCount;
	}
	public void SetAllMisstionTabsActive(bool isActive)//設定隱藏或開啟missionTab按鈕
	{
		for (int i = 0; i < missionTabsList.Count; i++)
		{
			missionTabsList[i].SetActive(isActive);
		}
		if (deleteButton) deleteButton.SetActive(isActive);
	}
	public void DeleteMisstionTab()
	{
		if (missionTabsList.Count >=2)
		{
			GameObject tmp = missionTabsList[missionTabsList.Count - 1];
			missionTabsList.Remove(tmp);
			Destroy(tmp);
			if (missionTabsList.Count==1)
			{
				tmp =deleteButton;
				deleteButton=null;
				Destroy(tmp);

				tmp = missionTabsList[missionTabsList.Count - 1];
				missionTabsList.Remove(tmp);
				Destroy(tmp);
			}
		}
	}

}
public class WindowsList : MonoBehaviour//視窗
{
	//視窗中正在使用的components
	[SerializeField]
	public List<Dictionary<string, List<GameObject>>> allFloorItem;
	//allComponent[第幾層樓][(物件名稱)MAINCOMPONENT].Count:第幾層樓的主物件有幾個
	[SerializeField]
	//暫存視窗中所有曾經編輯的components
	public Dictionary<string,  List<Dictionary<string, List<GameObject>>>> temporateAllFloorItem;
	//temporateAllFloorItem[曾編輯過的主物件][第幾層樓][(物件名稱)MAINCOMPONENT].Count:第幾層樓的主物件有幾個
	//上次選用的MainIcon
	public string lastChooseMainDragObjectName = null;
	//第幾號allComponent 用於樓層
	public int inUseTab2ComponentLayerIndex = 0;
	public void PrintAllComponentCount()//印出視窗中所有正在使用的components
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allFloorItem[inUseTab2ComponentLayerIndex])
		{
			Debug.Log(kvp.Key + kvp.Value.Count);
		}
	}

	public void ClearAllComponent()
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allFloorItem[inUseTab2ComponentLayerIndex])
		{
			for (int i = 0; i < kvp.Value.Count; i++)
			{
				Destroy(kvp.Value[i]);
			}
		}
		allFloorItem[inUseTab2ComponentLayerIndex].Clear();
	}
	public void DeleteAllComponent(int index)
	{
		Debug.Log("AllComponent : " + allFloorItem.Count);

		//Dictionary<string, List<GameObject>
		foreach (KeyValuePair<string, List<GameObject>> kvp in allFloorItem[index])
		{
			Debug.Log("enterDeleteAll");
			
			for (int i = 0; i < kvp.Value.Count; i++)
			{	
				Destroy(kvp.Value[i]);
				Debug.Log("kvp.Value" + kvp.Value.Count);
			}
	
		}
		allFloorItem[index].Clear();
		allFloorItem.RemoveAt(index);
	}
	public void HideAllComponent()
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allFloorItem[inUseTab2ComponentLayerIndex])
		{
			for (int i = 0; i < kvp.Value.Count; i++)
				(kvp.Value[i]).SetActive(false);
		}
		allFloorItem[inUseTab2ComponentLayerIndex].Clear();
	}
	public void ShowAllComponent()
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allFloorItem[inUseTab2ComponentLayerIndex])
		{
			for (int i = 0; i < kvp.Value.Count; i++)
				(kvp.Value[i]).SetActive(true);
		}
	}
	public void ShowAllComponent(int index)
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allFloorItem[index])
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

	public enum WindowsIndex { Formfactor = 0, Roof = 1, Body = 2, Platform = 3, SingleWindow = 4, };
	public enum WindowsSetIndex { FourBaseWindows = 0, SingleWindow = 1, };
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
	private WindowsList[] AllWindowsStruct;
	//當前使用視窗中的物件集合
	[HideInInspector]
	public WindowsList ThisWindowsComponent;
	//scrollView位置
	private Vector3 scrollViewOriginPos;
	//切換視窗配置
	public int changeLayoutIndexInWindowsSet = 0;
	//單個視窗
	private int mainSingleWindowinUseIndex;
	//
	private Movement movement;
	private AllInOne building;
	//MissionTabs
	public GameObject misstionTabObj;
	private MisstionTab misstionTab;

	//InitIconSetting

	public GameObject formFractorInitDragIconObj;
	public GameObject roofInitDragIconObj;
	public GameObject bodyInitDragIconObj;
	public GameObject platformInitDragIconObj;
	void Start()
	{
		uICamera = GameObject.Find("UICamera").GetComponent<Camera>();
		movement = GameObject.Find("Movement").GetComponent<Movement>();
		building = GameObject.Find("build").GetComponent<AllInOne>();

		InitWindowListMemorySetting();
		InitStateSetting();
		SwitchWindow();

		InitIconSetting();
	}
	void Update()
	{
		RayCastToChooseObj();
		if(Input.GetKeyDown(KeyCode.C))
		{
			Debug.Log( ThisWindowsComponent.allFloorItem.Count);
			Debug.Log(ThisWindowsComponent.temporateAllFloorItem.Count);
		}
	}
	void InitWindowListMemorySetting()
	{
		misstionTab = new MisstionTab();
		AllWindowsStruct = new WindowsList[windowsList.Count];
		for (int i = 0; i < windowsList.Count; i++)
		{
			AllWindowsStruct[i] = new WindowsList();
			AllWindowsStruct[i].allFloorItem = new List<Dictionary<string, List<GameObject>>>();
			AllWindowsStruct[i].temporateAllFloorItem = new Dictionary<string,  List<Dictionary<string, List<GameObject>>>>();
			Dictionary<string, List<GameObject>> newAllComponent = new Dictionary<string, List<GameObject>>();
			AllWindowsStruct[i].allFloorItem.Add(newAllComponent);
	
		}
	}
	//預設相機、Grid、視窗
	void InitStateSetting()
	{
		scrollViewOriginPos = gridList[(int)WindowsIndex.Formfactor].transform.position;
		chooseGrid = gridList[(int)WindowsIndex.Formfactor];
		SetCameraAndGrid((int)WindowsIndex.Formfactor);
		SetMissionTab((int)WindowsIndex.Formfactor);

		mainSingleWindowinUseIndex = (int)WindowsIndex.Formfactor;

		if (changeLayoutIndexInWindowsSet == (int)WindowsSetIndex.FourBaseWindows)
			chooseWindow = windowsList[(int)WindowsIndex.Formfactor];
		else if (changeLayoutIndexInWindowsSet == (int)WindowsSetIndex.SingleWindow)
			chooseWindow = windowsList[(int)WindowsIndex.SingleWindow];
	}
	void InitIconSetting() 
	{
		if (formFractorInitDragIconObj && !AllWindowsStruct[(int)WindowsIndex.Formfactor].allFloorItem[AllWindowsStruct[(int)WindowsIndex.Formfactor].inUseTab2ComponentLayerIndex].ContainsKey(MAINCOMPONENT))
		{
			CreateMainComponent((int)WindowsIndex.Formfactor, formFractorInitDragIconObj);
			AllWindowsStruct[(int)WindowsIndex.Formfactor].lastChooseMainDragObjectName = formFractorInitDragIconObj.name;
			ThisWindowsComponent = AllWindowsStruct[(int)WindowsIndex.Formfactor];

		}
		if (roofInitDragIconObj && !AllWindowsStruct[(int)WindowsIndex.Roof].allFloorItem[AllWindowsStruct[(int)WindowsIndex.Roof].inUseTab2ComponentLayerIndex].ContainsKey(MAINCOMPONENT))
		{
			CreateMainComponent((int)WindowsIndex.Roof, roofInitDragIconObj);
			AllWindowsStruct[(int)WindowsIndex.Roof].lastChooseMainDragObjectName = roofInitDragIconObj.name;
			
		}
		if (bodyInitDragIconObj && !AllWindowsStruct[(int)WindowsIndex.Body].allFloorItem[AllWindowsStruct[(int)WindowsIndex.Body].inUseTab2ComponentLayerIndex].ContainsKey(MAINCOMPONENT))
		{
			CreateMainComponent((int)WindowsIndex.Body, bodyInitDragIconObj);
			AllWindowsStruct[(int)WindowsIndex.Body].lastChooseMainDragObjectName = bodyInitDragIconObj.name;
	
		}
		if (platformInitDragIconObj && !AllWindowsStruct[(int)WindowsIndex.Platform].allFloorItem[AllWindowsStruct[(int)WindowsIndex.Platform].inUseTab2ComponentLayerIndex].ContainsKey(MAINCOMPONENT))
		{
			CreateMainComponent((int)WindowsIndex.Platform, platformInitDragIconObj);
			AllWindowsStruct[(int)WindowsIndex.Platform].lastChooseMainDragObjectName = platformInitDragIconObj.name;
		}
	}
	//選控制點
	void RayCastToChooseObj()
	{
		Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		Vector2 mousePos2World = uICamera.ScreenToWorldPoint(mousePos);

		if (chooseObj)//已選到控制點
		{
			if (Input.GetMouseButtonUp(0))//放開選到的控制點
			{
				chooseObj.GetComponent<Collider>().enabled = true;
				chooseObj = null;
				//
				building.UpdateRoof();
				return;
			}
			else//若在選定視窗內移動控制點
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
						//移動控制點
						movement.Move(loc);

						//判斷是否為body
						if (chooseObj.transform.parent.GetComponent<body2icon>())
						{
							building.MoveBody(chooseObj.transform.parent.GetComponent<body2icon>().ratio_bodydis.x);


							building.UpdateBody_B(chooseObj.transform.parent.GetComponent<body2icon>().isBalustrade);
							building.UpdateBody_F(chooseObj.transform.parent.GetComponent<body2icon>().isFrieze);

							//20160916

							building.Move_F(chooseObj.transform.parent.GetComponent<body2icon>().friezeHeight, chooseObj.transform.parent.GetComponent<body2icon>().ini_bodydis.y);
							building.Move_B(chooseObj.transform.parent.GetComponent<body2icon>().balustradeHeight, chooseObj.transform.parent.GetComponent<body2icon>().ini_bodydis.y);
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

						if (chooseObj.transform.parent.GetComponent<Testing>())
						{
							building.MoveRoof_Cp1(chooseObj.transform.parent.GetComponent<Testing>().ControlPoint1Move);
							building.MoveRoof_Cp2(chooseObj.transform.parent.GetComponent<Testing>().ControlPoint2Move);
							building.MoveRoof_Cp3(chooseObj.transform.parent.GetComponent<Testing>().ControlPoint3Move);

						}
					}
				}
			}
		}
		else
		{
			if (Input.GetMouseButtonDown(0))//選擇視窗
			{
				int inUseTab;
				switch (changeLayoutIndexInWindowsSet)
				{
					case (int)WindowsSetIndex.FourBaseWindows://四個視窗
						int index = ChooseWindow();
						if (index != -1)
						{
							ThisWindowsComponent = AllWindowsStruct[index];
							SetCameraAndGrid(index);
							SetMissionTab(index);
							//選擇視窗
							chooseWindow = windowsList[index];

							inUseTab = misstionTab.ChooseInUseMissionTabsIndex(mousePos2World);
							if (inUseTab != -1) SetInUseTabIndex2Window(index, inUseTab);
							if (misstionTab.deleteButton && misstionTab.ChooseMissionTabsDeleteButton(mousePos2World))
							{
								misstionTab.DeleteMisstionTab();
								ClearLastTab2Window(index);
							}
						}
						break;
					case (int)WindowsSetIndex.SingleWindow://單個視窗
						for (int i = 0; i < buttonList.Count; i++)
						{
							Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(buttonList[i].transform);
							if (bounds.Contains(mousePos2World))
							{
								chooseWindow.GetComponent<UITexture>().mainTexture = windowsList[i].GetComponent<UITexture>().mainTexture;
								ThisWindowsComponent = AllWindowsStruct[i];
								SetCameraAndGrid(i);
								SetMissionTab(i);
								SortButtonListTomainSingleWindow(i);
								break;
							}

						}
						//視窗中是否有missionTab 有的話切換missionTab 並設定AllwindowsComponent內容
						inUseTab = misstionTab.ChooseInUseMissionTabsIndex(mousePos2World);
						if (inUseTab != -1) SetInUseTabIndex2Window(mainSingleWindowinUseIndex, inUseTab);
						if (misstionTab.deleteButton && misstionTab.ChooseMissionTabsDeleteButton(mousePos2World)) 
						{
							misstionTab.DeleteMisstionTab();
							ClearLastTab2Window(mainSingleWindowinUseIndex);
				
						}
						break;

				}

				if (chooseWindow != null)//選擇控制點
				{

					Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(chooseWindow.transform);

					if (bounds.Contains(mousePos2World))
					{

						Vector2 hitLocalUV;
						Vector3 localHit = mousePos2World;
						// normalize
						hitLocalUV.x = (localHit.x - bounds.min.x) / (bounds.size.x);
						hitLocalUV.y = (localHit.y - bounds.min.y) / (bounds.size.y);
						//從圖片位置到世界相機座標選取點
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
								if (chooseObj.transform.parent.GetComponent<Testing>())
								{
									chooseObj.transform.parent.GetComponent<Testing>().addpoint();
								}
								chooseObj.GetComponent<Collider>().enabled = false;
							}
						}
					}

				}

			}
		}
	}
	//隱藏開啟MissionTab
	void SetMissionTab(int index)
	{
		if (index == (int)WindowsIndex.Body)
		{
			misstionTab.SetAllMisstionTabsActive(true);
		}
		else
		{
			misstionTab.SetAllMisstionTabsActive(false);
		}
	}
	//BUTTON排序
	void SwapGameObject(GameObject a, GameObject b)
	{
		Vector3 temp = a.transform.position;

		a.transform.position = b.transform.position;
		b.transform.position = temp;
	}
	void SwapButtonTomainSingleWindow(int index)
	{
		if (mainSingleWindowinUseIndex == index) return;

		SwapGameObject(buttonList[index], buttonList[mainSingleWindowinUseIndex]);

		mainSingleWindowinUseIndex = index;

	}
	void SortButtonListTomainSingleWindow(int index)
	{
		if (mainSingleWindowinUseIndex == index) return;

		int diff = Mathf.Abs(mainSingleWindowinUseIndex - index);

		if (diff > 1)
		{
			if (mainSingleWindowinUseIndex < index)
			{
				for (int i = 0; i < diff - 1; i++)
				{
					SwapGameObject(buttonList[index], buttonList[index - 1 - i]);
				}
			}
			else if (mainSingleWindowinUseIndex > index)
			{
				for (int i = 0; i < diff - 1; i++)
				{
					SwapGameObject(buttonList[index], buttonList[index + 1 + i]);
				}
			}
		}
		SwapGameObject(buttonList[index], buttonList[mainSingleWindowinUseIndex]);

		mainSingleWindowinUseIndex = index;
	}
	//選擇正在使用的視窗
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
	//設定鏡頭、Grid
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
	//設定選用視窗layerIndex
	void SetInUseTabIndex2Window(int index, int number)
	{

		if (AllWindowsStruct[index].inUseTab2ComponentLayerIndex == number) return;

		SaveState2MainComponent(index);
		AllWindowsStruct[index].HideAllComponent();
		AllWindowsStruct[index].inUseTab2ComponentLayerIndex = number;


		AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex] = AllWindowsStruct[index].temporateAllFloorItem[AllWindowsStruct[index].lastChooseMainDragObjectName][AllWindowsStruct[index].inUseTab2ComponentLayerIndex];
		AllWindowsStruct[index].ShowAllComponent();

	}
	//按下刪除鈕後，清除選用視窗最後的missionTab對應的commponent內容
	void ClearLastTab2Window(int index)
	{
	
		  SaveState2MainComponent(index);
		  AllWindowsStruct[index].temporateAllFloorItem[AllWindowsStruct[index].lastChooseMainDragObjectName][AllWindowsStruct[index].allFloorItem.Count - 1].Clear();
		  AllWindowsStruct[index].temporateAllFloorItem[AllWindowsStruct[index].lastChooseMainDragObjectName].RemoveAt(AllWindowsStruct[index].allFloorItem.Count - 1);
		  if (AllWindowsStruct[index].inUseTab2ComponentLayerIndex == AllWindowsStruct[index].allFloorItem.Count - 1)//最後一個missionTab對應的commponent內容正在編輯
		  {
			  //刪除最後一個

			  Debug.Log("last:" + (AllWindowsStruct[index].allFloorItem.Count - 1));

			  AllWindowsStruct[index].DeleteAllComponent(AllWindowsStruct[index].allFloorItem.Count - 1);
			  //顯示前一個
			  AllWindowsStruct[index].inUseTab2ComponentLayerIndex--;
			  AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex] = AllWindowsStruct[index].temporateAllFloorItem[AllWindowsStruct[index].lastChooseMainDragObjectName][AllWindowsStruct[index].inUseTab2ComponentLayerIndex];
			  AllWindowsStruct[index].ShowAllComponent();
		  }
		  else//最後一個missionTab對應的commponent內容沒有正在編輯
		  {
			  //刪除最後一個
			  Debug.Log("last:" + (AllWindowsStruct[index].allFloorItem.Count - 1));

			  AllWindowsStruct[index].DeleteAllComponent(AllWindowsStruct[index].allFloorItem.Count - 1);
		  }
	}
	//
	void SwitchWindow()
	{
		for (int i = 0; i < windowSetList.Count; i++)
		{
			windowSetList[i].SetActive(false);
		}
		if (changeLayoutIndexInWindowsSet < windowSetList.Count)
			windowSetList[changeLayoutIndexInWindowsSet].SetActive(true);
	}
	public void SetObjInWindows()
	{
		switch (changeLayoutIndexInWindowsSet)
		{
			case (int)WindowsSetIndex.FourBaseWindows:
				int index = ChooseWindow();
				if (index != -1 && chooseDragObject)
				{
					SetCameraAndGrid(index);
					SetMissionTab(index);
					if (windowsList[index] == chooseWindow)
					{
						ThisWindowsComponent = AllWindowsStruct[index];

						SetWindowsComponent(index);
					}
					//選擇視窗
					chooseWindow = windowsList[index];
				}
				break;
			case (int)WindowsSetIndex.SingleWindow:
				index = ChooseWindow();
				if (index == (int)WindowsIndex.SingleWindow && chooseDragObject)
				{
					SetCameraAndGrid(mainSingleWindowinUseIndex);
					SetMissionTab(mainSingleWindowinUseIndex);

					ThisWindowsComponent = AllWindowsStruct[mainSingleWindowinUseIndex];

					SetWindowsComponent(mainSingleWindowinUseIndex);
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
				Debug.Log(AllWindowsStruct[index].lastChooseMainDragObjectName);
				if (!AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex].ContainsKey(MAINCOMPONENT))//在選擇的視窗內 且視窗內物件為空
				{
					CreateMainComponent(index);
					Debug.Log("000");
				}
				else//視窗內物件為不為空
				{
					if (AllWindowsStruct[index].lastChooseMainDragObjectName != chooseDragObject.name)//如果不是拖曳同一個主物件取代原本的主物物件
					{
						//紀錄操作的物件
						if (AllWindowsStruct[index].temporateAllFloorItem.ContainsKey(chooseDragObject.name)) //有編輯過此視窗
						{
							AllWindowsStruct[index].HideAllComponent();
							AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex] = AllWindowsStruct[index].temporateAllFloorItem[chooseDragObject.name][AllWindowsStruct[index].inUseTab2ComponentLayerIndex];
							AllWindowsStruct[index].ShowAllComponent();
							Debug.Log("1111");

						}
						else //沒有編輯過此視窗
						{
							AllWindowsStruct[index].HideAllComponent();
							CreateMainComponent(index);
							Debug.Log("222");
						}
					}
					else //如果拖曳同一個主物件
					{
						//取代原本的主物物件 清除此視窗物件
						AllWindowsStruct[index].ClearAllComponent();
						AllWindowsStruct[index].temporateAllFloorItem[chooseDragObject.name][AllWindowsStruct[index].inUseTab2ComponentLayerIndex].Clear();
						CreateMainComponent(index);
						Debug.Log("333");

					}
				}
				AllWindowsStruct[index].lastChooseMainDragObjectName = chooseDragObject.name;
				if (AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<MeshObj>()) building.UpdateAll(AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<MeshObj>().edgeIndex);
				if (AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>())
				{
					building.UpdateBody_B(AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().isBalustrade);
					building.UpdateBody_F(AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().isFrieze);
				}
				break;
			case DECORATECOMPONENT:

				if (AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex].ContainsKey(MAINCOMPONENT))//如果有拖曳物件 且在選擇的視窗內 且視窗內物件為空
				{
					CreateDecorateComponent(index);

					if (AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>())
					{

						if (AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex].ContainsKey(chooseDragObject.name)) AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().UpdateFunction(chooseDragObject.name);

						building.UpdateBody_F(AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().isBalustrade);
						building.UpdateBody_B(AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().isFrieze);



						building.Move_F(AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().friezeHeight, AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().ini_bodydis.y);
						building.Move_B(AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().balustradeHeight, AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().ini_bodydis.y);

					}

					//frieze ＆ Balustrade
				}

				break;
		}
	}
	void SaveState2MainComponent(int index)
	{
		if (AllWindowsStruct[index].lastChooseMainDragObjectName != null)
		{
			if (!AllWindowsStruct[index].temporateAllFloorItem.ContainsKey(AllWindowsStruct[index].lastChooseMainDragObjectName))
			{
				Debug.Log("First");
				Dictionary<string, List<GameObject>> copy = new Dictionary<string, List<GameObject>>(AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex]);
				  List<Dictionary<string, List<GameObject>>> tmpList=new  List<Dictionary<string, List<GameObject>>>();
				  tmpList.Add(copy);
				  AllWindowsStruct[index].temporateAllFloorItem.Add(AllWindowsStruct[index].lastChooseMainDragObjectName, tmpList);
			}
			else
			{
				Debug.Log("Second");
				Dictionary<string, List<GameObject>> copy = new Dictionary<string, List<GameObject>>(AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex]);
				AllWindowsStruct[index].temporateAllFloorItem[AllWindowsStruct[index].lastChooseMainDragObjectName][AllWindowsStruct[index].inUseTab2ComponentLayerIndex] = copy;
			}
		}
	}
	void CreateMainComponent(int index, GameObject IconDragObj)
	{
		Vector3 pos = cameraList[index].transform.position; pos.z = cameraList[index].farClipPlane / 2.0f;

		GameObject cloneCorrespondingObj = IconDragObj.GetComponent<CorespondingDragItem>().corespondingDragItem;

		GameObject clone = Instantiate(cloneCorrespondingObj, pos, cloneCorrespondingObj.transform.rotation) as GameObject;

		clone.transform.parent = this.transform;

		List<GameObject> allComponentList = new List<GameObject>();
		allComponentList.Add(clone);
		AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex].Add(MAINCOMPONENT, allComponentList);
	}
	void CreateMainComponent(int index)
	{
		Vector3 pos = chooseCamera.transform.position; pos.z = chooseCamera.farClipPlane / 2.0f;

		GameObject cloneCorrespondingObj = chooseDragObject.GetComponent<CorespondingDragItem>().corespondingDragItem;

		GameObject clone = Instantiate(cloneCorrespondingObj, pos, cloneCorrespondingObj.transform.rotation) as GameObject;

		clone.transform.parent = this.transform;

		List<GameObject> allComponentList = new List<GameObject>();
		allComponentList.Add(clone);
		AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex].Add(MAINCOMPONENT, allComponentList);
	}
	void CreateDecorateComponent(int index)
	{
		Vector3 pos = chooseCamera.transform.position; pos.z = chooseCamera.farClipPlane / 2.0f;
		GameObject cloneCorrespondingObj = chooseDragObject.GetComponent<CorespondingDragItem>().corespondingDragItem;
		int correspondingDragItemMaxCount = chooseDragObject.GetComponent<CorespondingDragItem>().correspondingDragItemMaxCount;

		if (chooseDragObject.name == "MutiBody")
		{
			if (misstionTab.IsNotOverMaxCount(correspondingDragItemMaxCount))
			{
				SaveState2MainComponent(index);
				AllWindowsStruct[index].HideAllComponent();
				AllWindowsStruct[index].inUseTab2ComponentLayerIndex = AllWindowsStruct[index].allFloorItem.Count;
				Debug.Log("CCCC" + AllWindowsStruct[index].inUseTab2ComponentLayerIndex);

				Dictionary<string, List<GameObject>> newAllComponent = new Dictionary<string, List<GameObject>>();


				AllWindowsStruct[index].allFloorItem.Add(newAllComponent);

				AllWindowsStruct[index].temporateAllFloorItem[AllWindowsStruct[index].lastChooseMainDragObjectName].Add(newAllComponent);

				misstionTab.CreateMissionTabs(this, misstionTabObj, chooseWindow);

				CreateMainComponent(index);
			}

		}
		else
		{

			if (AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex].ContainsKey(chooseDragObject.name))
			{
				if (AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][chooseDragObject.name].Count < correspondingDragItemMaxCount)
				{
					GameObject clone = Instantiate(cloneCorrespondingObj, pos, cloneCorrespondingObj.transform.rotation) as GameObject;
					clone.transform.parent = this.transform;
					AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][chooseDragObject.name].Add(clone);
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
				AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex].Add(chooseDragObject.name, newList);
			}
		}
	}
}
