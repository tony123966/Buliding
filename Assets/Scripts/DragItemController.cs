/*
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
		if (missionTabsList.Count >=2)//剩二層時要刪除 要一次刪兩層
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
	private MisstionTab misstionTab2Body;

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
		misstionTab2Body = new MisstionTab();
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
		InitCameraSetting();

		if (changeLayoutIndexInWindowsSet == (int)WindowsSetIndex.FourBaseWindows)
			chooseWindow = windowsList[(int)WindowsIndex.Formfactor];
		else if (changeLayoutIndexInWindowsSet == (int)WindowsSetIndex.SingleWindow)
			chooseWindow = windowsList[(int)WindowsIndex.SingleWindow];

		SetCameraAndGrid((int)WindowsIndex.Formfactor);
		SetMissionTab((int)WindowsIndex.Formfactor);

		mainSingleWindowinUseIndex = (int)WindowsIndex.Formfactor;

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
	//設定鏡頭、Grid
	void InitCameraSetting()
	{
		Camera camera = cameraList[(int)WindowsIndex.Formfactor].GetComponent<Camera>();
		Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(windowsList[(int)WindowsIndex.Formfactor].transform);
		float targetAspect = (float)bounds.extents.x / bounds.extents.y;
		camera.aspect = targetAspect;

		camera = cameraList[(int)WindowsIndex.Roof].GetComponent<Camera>();
		bounds = NGUIMath.CalculateAbsoluteWidgetBounds(windowsList[(int)WindowsIndex.Roof].transform);
		targetAspect = (float)bounds.extents.x / bounds.extents.y;
		camera.aspect = targetAspect;

		camera = cameraList[(int)WindowsIndex.Body].GetComponent<Camera>();
		bounds = NGUIMath.CalculateAbsoluteWidgetBounds(windowsList[(int)WindowsIndex.Body].transform);
		targetAspect = (float)bounds.extents.x / bounds.extents.y;
		camera.aspect = targetAspect;

		camera = cameraList[(int)WindowsIndex.Platform].GetComponent<Camera>();
		bounds = NGUIMath.CalculateAbsoluteWidgetBounds(windowsList[(int)WindowsIndex.Platform].transform);
		targetAspect = (float)bounds.extents.x / bounds.extents.y;
		camera.aspect = targetAspect;
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

							inUseTab = misstionTab2Body.ChooseInUseMissionTabsIndex(mousePos2World);
							if (inUseTab != -1) SetInUseTabIndex2Window(index, inUseTab);
							if (misstionTab2Body.deleteButton && misstionTab2Body.ChooseMissionTabsDeleteButton(mousePos2World))
							{
								misstionTab2Body.DeleteMisstionTab();
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
						inUseTab = misstionTab2Body.ChooseInUseMissionTabsIndex(mousePos2World);
						if (inUseTab != -1) SetInUseTabIndex2Window(mainSingleWindowinUseIndex, inUseTab);
						if (misstionTab2Body.deleteButton && misstionTab2Body.ChooseMissionTabsDeleteButton(mousePos2World)) 
						{
							misstionTab2Body.DeleteMisstionTab();
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
			misstionTab2Body.SetAllMisstionTabsActive(true);
		}
		else
		{
			misstionTab2Body.SetAllMisstionTabsActive(false);
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
			if (misstionTab2Body.IsNotOverMaxCount(correspondingDragItemMaxCount))
			{
				SaveState2MainComponent(index);
				AllWindowsStruct[index].HideAllComponent();
				AllWindowsStruct[index].inUseTab2ComponentLayerIndex = AllWindowsStruct[index].allFloorItem.Count;
				Debug.Log("CCCC" + AllWindowsStruct[index].inUseTab2ComponentLayerIndex);

				Dictionary<string, List<GameObject>> newAllComponent = new Dictionary<string, List<GameObject>>();


				AllWindowsStruct[index].allFloorItem.Add(newAllComponent);

				AllWindowsStruct[index].temporateAllFloorItem[AllWindowsStruct[index].lastChooseMainDragObjectName].Add(newAllComponent);

				misstionTab2Body.CreateMissionTabs(this, misstionTabObj, chooseWindow);

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
*/
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class MisstionTab : MonoBehaviour
{
	public List<GameObject> missionTabsList = new List<GameObject>();
	public GameObject deleteButton = null;
	public int inUseIndex;
	Bounds windowsBounds;
	Bounds tabsBounds;
	float offset;
	public void CreateMissionTabs<T>(T thisGameObject, GameObject missionTabObj, GameObject chooseWindow) where T : Component
	{
		windowsBounds = NGUIMath.CalculateAbsoluteWidgetBounds(chooseWindow.transform);
		tabsBounds = NGUIMath.CalculateAbsoluteWidgetBounds(missionTabObj.transform);
		offset = tabsBounds.size.x * 0.01f;
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
	public void DeleteLastMisstionTab()
	{
		if (missionTabsList.Count >= 2)//剩二層時要刪除 要一次刪兩層
		{
			GameObject tmp = missionTabsList[missionTabsList.Count - 1];
			missionTabsList.Remove(tmp);
			Destroy(tmp);
			if (missionTabsList.Count == 1)
			{
				tmp = deleteButton;
				deleteButton = null;
				Destroy(tmp);

				tmp = missionTabsList[missionTabsList.Count - 1];
				missionTabsList.Remove(tmp);
				Destroy(tmp);
			}
		}
	}
	public void DeleteCurrentMisstionTab()
	{
		Debug.Log("MisstionTabinUseIndex" + inUseIndex);
		if (missionTabsList.Count >= 2)//剩二層時要刪除 要一次刪兩層
		{
			GameObject tmp = missionTabsList[inUseIndex];
			Destroy(tmp);
			missionTabsList.RemoveAt(inUseIndex);
			if (inUseIndex > 0) inUseIndex--;
			if (missionTabsList.Count == 1)
			{
				tmp = deleteButton;
				deleteButton = null;
				Destroy(tmp);

				tmp = missionTabsList[missionTabsList.Count - 1];
				Destroy(tmp);
				missionTabsList.RemoveAt(missionTabsList.Count - 1);
				if (inUseIndex > 0) inUseIndex--;
			}
		}
		SortMisstionTabPosition();
	}
	public void SortMisstionTabPosition()
	{
		for (int i = 0; i < missionTabsList.Count; i++)
		{
			missionTabsList[i].transform.position = windowsBounds.min + new Vector3((i * (tabsBounds.size.x + offset)) + tabsBounds.extents.x, tabsBounds.extents.y, 0.0f);
		}
	}
}
public class WindowsList : MonoBehaviour//視窗
{
	//視窗中正在使用的components
	[SerializeField]
	public List<Dictionary<string, List<GameObject>>> allFloorItem;
	//allFloorItem[第幾層樓][(物件名稱)MAINCOMPONENT].Count:第幾層樓的主物件有幾個
	[SerializeField]
	//暫存視窗中所有曾經編輯的components
	public Dictionary<string, List<Dictionary<string, List<GameObject>>>> temporateAllFloorItem;
	//temporateAllFloorItem[曾編輯過的主物件][第幾層樓][(物件名稱)MAINCOMPONENT].Count:第幾層樓的主物件有幾個
	//上次選用的MainIcon
	public string lastChooseMainDragObjectName = null;
	//第幾號allFloorItem 用於樓層
	public int inUseTab2ComponentLayerIndex = 0;

	public MisstionTab misstionTab;

	public GameObject deleteIconButton = null;

	public GameObject inUseComponentObject = null;
	public WindowsList()
	{
		misstionTab = new MisstionTab();
	}
	public void PrintAllComponentCount()//印出視窗中所有正在使用的components
	{
		foreach (KeyValuePair<string, List<GameObject>> kvp in allFloorItem[inUseTab2ComponentLayerIndex])
		{
			Debug.Log(kvp.Key + kvp.Value.Count);
		}
	}

	public void ClearAllComponent()//清除當前樓層Component
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
	public void DeleteAllComponent(int index)//清除當前樓層Component 並減少一層樓
	{
		Debug.Log("AllComponent : " + allFloorItem.Count);

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
	//重要的tag
	const string MAINCOMPONENT = "MainComponent";
	const string DECORATECOMPONENT = "DecorateComponent";
	const string CONTROLPOINT = "ControlPoint";
	const string MESHBODYCOLLIDER = "MeshBodyCollider";
	const string ICONMENUBUTTON = "IconMenuButton";
	//四大視窗編號
	public enum WindowsIndex { Formfactor = 0, Roof = 1, Body = 2, Platform = 3, SingleWindow = 4, };
	//兩種windowSet模式編號
	public enum WindowsSetIndex { FourBaseWindows = 0, SingleWindow = 1, };
	//選定的物件
	public GameObject lastChooseIconObject = null;
	public GameObject chooseDragObject = null;
	public GameObject chooseObj = null;
	public GameObject chooseWindow;
	public GameObject chooseGrid;
	private Camera chooseCamera;
	//GeneralViewCamera
	public Camera generalViewCamera;
	private Vector3 generalViewCameraCenterOffset=new Vector3(0,10,0);
	private float generalViewCameraDisOffset =10.0f;
	//UICamera
	private Camera uICamera;
	//四大視窗
	public List<GameObject> windowsList = new List<GameObject>();
	public List<GameObject> gridList = new List<GameObject>();
	public List<Camera> cameraList = new List<Camera>();
	//windowSetList
	public List<GameObject> windowSetList = new List<GameObject>();
	//windowSwitchButtonList
	public List<GameObject> windowSwitchButtonList = new List<GameObject>();

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
	//script
	private Movement movement;
	private AllInOne building;
	//MissionTabButton
	public GameObject misstionTabObj;
	//IconControlMenuPanel
	public GameObject IconControlMenuPanel;
	private Bounds IconControlMenuPanelBounds;
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
		InitWindowSetSetting();

		InitIconSetting();
	}
	void Update()
	{
		RayCastToChooseObj();
	}
	void InitWindowListMemorySetting()
	{
		AllWindowsStruct = new WindowsList[windowsList.Count];
		for (int i = 0; i < windowsList.Count; i++)
		{
			AllWindowsStruct[i] = new WindowsList();
			AllWindowsStruct[i].allFloorItem = new List<Dictionary<string, List<GameObject>>>();
			AllWindowsStruct[i].temporateAllFloorItem = new Dictionary<string, List<Dictionary<string, List<GameObject>>>>();
			Dictionary<string, List<GameObject>> newAllComponent = new Dictionary<string, List<GameObject>>();
			AllWindowsStruct[i].allFloorItem.Add(newAllComponent);

		}
	}
	//預設相機、Grid、視窗
	void InitStateSetting()
	{
		scrollViewOriginPos = gridList[(int)WindowsIndex.Formfactor].transform.position;
		chooseGrid = gridList[(int)WindowsIndex.Formfactor];
		InitCameraSetting();

		if (changeLayoutIndexInWindowsSet == (int)WindowsSetIndex.FourBaseWindows)
			chooseWindow = windowsList[(int)WindowsIndex.Formfactor];
		else if (changeLayoutIndexInWindowsSet == (int)WindowsSetIndex.SingleWindow)
			chooseWindow = windowsList[(int)WindowsIndex.SingleWindow];

		SetCameraAndGrid((int)WindowsIndex.Formfactor);
		SetMissionTab((int)WindowsIndex.Formfactor);

		mainSingleWindowinUseIndex = (int)WindowsIndex.Formfactor;

	}
	void InitIconSetting()
	{
		if (formFractorInitDragIconObj&&!AllWindowsStruct[(int)WindowsIndex.Formfactor].allFloorItem[AllWindowsStruct[(int)WindowsIndex.Formfactor].inUseTab2ComponentLayerIndex].ContainsKey(MAINCOMPONENT))
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
	GameObject ReturnInitWindowsIcon(int index)
	{
		SaveState2MainComponent(index);
		switch (index) 
		{
			case (int)WindowsIndex.Formfactor:
				if (formFractorInitDragIconObj)
				{
					return formFractorInitDragIconObj;
				}
			break;
			case (int)WindowsIndex.Roof:
			if (roofInitDragIconObj)
				{
					return roofInitDragIconObj;
				}
			break;
			case (int)WindowsIndex.Body:
			if (bodyInitDragIconObj)
				{
					return  bodyInitDragIconObj;
				}
			break;
			case (int)WindowsIndex.Platform:
			if (platformInitDragIconObj)
				{
					return platformInitDragIconObj;
				}
			break;
		}
		return null;
	}
	//設定鏡頭、Grid
	void InitCameraSetting()
	{
		Camera camera = cameraList[(int)WindowsIndex.Formfactor].GetComponent<Camera>();
		Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(windowsList[(int)WindowsIndex.Formfactor].transform);
		float targetAspect = (float)bounds.extents.x / bounds.extents.y;
		camera.aspect = targetAspect;

		camera = cameraList[(int)WindowsIndex.Roof].GetComponent<Camera>();
		bounds = NGUIMath.CalculateAbsoluteWidgetBounds(windowsList[(int)WindowsIndex.Roof].transform);
		targetAspect = (float)bounds.extents.x / bounds.extents.y;
		camera.aspect = targetAspect;

		camera = cameraList[(int)WindowsIndex.Body].GetComponent<Camera>();
		bounds = NGUIMath.CalculateAbsoluteWidgetBounds(windowsList[(int)WindowsIndex.Body].transform);
		targetAspect = (float)bounds.extents.x / bounds.extents.y;
		camera.aspect = targetAspect;

		camera = cameraList[(int)WindowsIndex.Platform].GetComponent<Camera>();
		bounds = NGUIMath.CalculateAbsoluteWidgetBounds(windowsList[(int)WindowsIndex.Platform].transform);
		targetAspect = (float)bounds.extents.x / bounds.extents.y;
		camera.aspect = targetAspect;
	}
	//選控制點
	void RayCastToChooseObj()
	{
		Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		Vector2 mousePos2World = uICamera.ScreenToWorldPoint(mousePos);


		if (chooseObj)//已選到控制點
		{
			IconControlMenuPanel.SetActive(false);
			if (Input.GetMouseButtonUp(0))//放開選到的控制點
			{
				chooseObj.GetComponent<Collider>().enabled = true;

				if (chooseObj.transform.parent.GetComponent<MeshObj>())
				{

					building.rectangle_column();

					building.rectangle_Platform(building.roof_tall, building.roof_long);

				}

				if (chooseObj.transform.parent.GetComponent<Testing>())
				{

					building.UpdateRoof();

				}

				chooseObj = null;

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


						//判斷是否上面MeshObj

						if (chooseObj.transform.parent.GetComponent<MeshObj>())
						{


							building.ChangeRoof(0, -chooseObj.transform.parent.GetComponent<MeshObj>().chang_mainRidgedis * (building.ini_roof_tall / chooseObj.transform.parent.GetComponent<MeshObj>().ini_mainRidgedis), -chooseObj.transform.parent.GetComponent<MeshObj>().chang_mainRidgedis * (building.ini_roof_tall / chooseObj.transform.parent.GetComponent<MeshObj>().ini_mainRidgedis));




							//print(chooseObj.transform.parent.GetComponent<MeshObj>().chang_mainRidgedis);
							building.ChangeRoof(2, chooseObj.transform.parent.GetComponent<MeshObj>().chang_bodydis.x * (building.ini_roof_tall / chooseObj.transform.parent.GetComponent<MeshObj>().ini_bodydis.x), chooseObj.transform.parent.GetComponent<MeshObj>().chang_bodydis.x * (building.ini_roof_tall / chooseObj.transform.parent.GetComponent<MeshObj>().ini_bodydis.x));



							building.ChangeRoof(2, chooseObj.transform.parent.GetComponent<MeshObj>().chang_bodydis.y * (building.ini_roof_long / chooseObj.transform.parent.GetComponent<MeshObj>().ini_bodydis.y), -chooseObj.transform.parent.GetComponent<MeshObj>().chang_bodydis.y * (building.ini_roof_long / chooseObj.transform.parent.GetComponent<MeshObj>().ini_bodydis.y));

						}

						//判斷是否為body
						if (chooseObj.transform.parent.GetComponent<body2icon>())
						{
							building.MoveBody(chooseObj.transform.parent.GetComponent<body2icon>().ratio_bodydis.x);

							building.moveBOBODY(chooseObj.transform.parent.GetComponent<body2icon>().ratio_walldis);

							building.Move_F(chooseObj.transform.parent.GetComponent<body2icon>().friezeHeight, chooseObj.transform.parent.GetComponent<body2icon>().ini_bodydis.y);

							building.Move_B(chooseObj.transform.parent.GetComponent<body2icon>().balustradeHeight, chooseObj.transform.parent.GetComponent<body2icon>().ini_bodydis.y);

							building.bodytall(chooseObj.transform.parent.GetComponent<body2icon>().ratio_bodydis.y);


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
		else//沒選到控制點
		{
			if (Input.GetMouseButtonDown(0))//選擇視窗
			{
				if (IconControlMenuPanel.activeSelf) 
				{
					if (!IconControlMenuPanelBounds.Contains(mousePos2World))
					{

						IconControlMenuPanel.SetActive(false);
					}
					else
					{
						Ray ray = uICamera.ScreenPointToRay(mousePos);
						RaycastHit srsHit;
						if (Physics.Raycast(ray, out srsHit))
						{
							if (srsHit.collider.gameObject.tag == ICONMENUBUTTON)
							{
								IconControlMenuPanel.SetActive(true);
							}
						}
					}
				}
				else
				{
					int inUseTab = 0;
					int index = 0;
					switch (changeLayoutIndexInWindowsSet)
					{
						case (int)WindowsSetIndex.FourBaseWindows://四個視窗
							index = ChooseWindow();
							if (index != -1)
							{
								ThisWindowsComponent = AllWindowsStruct[index];
								SetCameraAndGrid(index);
								SetMissionTab(index);
								//選擇視窗
								chooseWindow = windowsList[index];
								if (AllWindowsStruct[index].allFloorItem.Count > 0)
								{
									inUseTab = AllWindowsStruct[index].misstionTab.ChooseInUseMissionTabsIndex(mousePos2World);
									if (inUseTab != -1) SetInUseTabIndex2Window(index, inUseTab);
									if (AllWindowsStruct[index].misstionTab.deleteButton && AllWindowsStruct[index].misstionTab.ChooseMissionTabsDeleteButton(mousePos2World))
									{
										AllWindowsStruct[index].misstionTab.DeleteCurrentMisstionTab();
										ClearCurrentTab2Window(index);
									}
								}

							}
							break;
						case (int)WindowsSetIndex.SingleWindow://單個視窗
							for (index = 0; index < windowSwitchButtonList.Count; index++)
							{
								Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(windowSwitchButtonList[index].transform);
								if (bounds.Contains(mousePos2World))
								{
									chooseWindow.GetComponent<UITexture>().mainTexture = windowsList[index].GetComponent<UITexture>().mainTexture;
									ThisWindowsComponent = AllWindowsStruct[index];
									SetCameraAndGrid(index);
									SetMissionTab(index);
									SortWindowSwitchButtonList2MainSingleWindow(index);
									break;
								}

							}
							if (AllWindowsStruct[mainSingleWindowinUseIndex].allFloorItem.Count > 0)
							{
								//視窗中是否有missionTab 有的話切換missionTab 並設定AllwindowsComponent內容
								inUseTab = AllWindowsStruct[mainSingleWindowinUseIndex].misstionTab.ChooseInUseMissionTabsIndex(mousePos2World);
								if (inUseTab != -1)
								{

									SetInUseTabIndex2Window(mainSingleWindowinUseIndex, inUseTab);

									building.changeLayer(inUseTab);

								}
								if (AllWindowsStruct[mainSingleWindowinUseIndex].misstionTab.deleteButton && AllWindowsStruct[mainSingleWindowinUseIndex].misstionTab.ChooseMissionTabsDeleteButton(mousePos2World))
								{

									building.DeleteLayer(AllWindowsStruct[mainSingleWindowinUseIndex].inUseTab2ComponentLayerIndex);


									AllWindowsStruct[mainSingleWindowinUseIndex].misstionTab.DeleteCurrentMisstionTab();
									ClearCurrentTab2Window(mainSingleWindowinUseIndex);

									//building.DeleteLayer(AllWindowsStruct[mainSingleWindowinUseIndex].allFloorItem.Count);

									generalViewCamera.GetComponent<CameraToCenter>().ChangeCenter2TargetObject(generalViewCameraCenterOffset, generalViewCameraDisOffset, AllWindowsStruct[mainSingleWindowinUseIndex].allFloorItem.Count-1);
								}
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
								if (srsHit.collider.gameObject.tag == CONTROLPOINT)
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
									//寫成lastChooseIconObject = chooseObj.transform.parent.gameObject會出錯??????????????
									//lastChooseIconObject = srsHit.collider.transform.parent.gameObject;
									lastChooseIconObject = srsHit.collider.transform.gameObject;
								}
								else if (srsHit.collider.gameObject.tag == MESHBODYCOLLIDER)
								{
									lastChooseIconObject = srsHit.collider.transform.gameObject;
								}
							}
						}
					}
				}
			
			}
			else if (Input.GetMouseButtonDown(1))//右鍵找控制點開啟IconMenu
				{
					IconControlMenuPanelBounds = NGUIMath.CalculateAbsoluteWidgetBounds(IconControlMenuPanel.transform);
					IconControlMenuPanel.SetActive(false);
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
						Debug.Log("zzzz");
						if (Physics.Raycast(srsRay, out srsHit))
						{
							if (srsHit.collider.gameObject.tag == CONTROLPOINT || srsHit.collider.gameObject.tag == MESHBODYCOLLIDER)
							{
								Debug.Log("push");
								lastChooseIconObject = srsHit.collider.transform.gameObject;
								IconControlMenuPanel.SetActive(true);
								IconControlMenuPanel.transform.position = new Vector3(mousePos2World.x + IconControlMenuPanelBounds.size.x / 2.0f, mousePos2World.y - IconControlMenuPanelBounds.size.y / 2.0f, 0);
							}
						}
					}
				}

		}
	}
	public void IConMenu2DeleteChooseIcon()
	{
		SaveState2MainComponent(mainSingleWindowinUseIndex);
		//如果只有一個MainComponent
		bool isMainComponentObject=true;
		if (AllWindowsStruct[mainSingleWindowinUseIndex].allFloorItem[AllWindowsStruct[mainSingleWindowinUseIndex].inUseTab2ComponentLayerIndex].Count > 1)//有兩個以上的component
		{
			foreach (KeyValuePair<string, List<GameObject>> kvp in AllWindowsStruct[mainSingleWindowinUseIndex].allFloorItem[AllWindowsStruct[mainSingleWindowinUseIndex].inUseTab2ComponentLayerIndex])
			{
				if (kvp.Key == MAINCOMPONENT) continue;
		
				for (int i = 0; i < kvp.Value.Count; i++)
				{
					for (int j = 0; j < kvp.Value[i].GetComponent<DecorateEmptyObjectList>().objectList.Count; j++)
					{
						if (lastChooseIconObject == kvp.Value[i].GetComponent<DecorateEmptyObjectList>().objectList[j])
						{
							Destroy(kvp.Value[i]);
							AllWindowsStruct[mainSingleWindowinUseIndex].allFloorItem[AllWindowsStruct[mainSingleWindowinUseIndex].inUseTab2ComponentLayerIndex][kvp.Key].Clear();
							isMainComponentObject=false;

							if (lastChooseIconObject.transform.parent.GetComponent<body2icon>())
							{
								lastChooseIconObject.transform.parent.GetComponent<body2icon>().DestroyFunction(kvp.Key);
							}

							break;
						}
					}
				}
/*
				for (int i = 0; i < kvp.Value.Count; i++)
				{
					Debug.Log("kvp.key" + kvp.Key);
					Debug.Log("kvp.Value[i].name" + kvp.Value[i].name);
					Transform[] obj = kvp.Value[i].GetComponentsInChildren<Transform>(true) as Transform[];
					for (int j = 0; j < obj.Length; j++)
					{
						if (obj[j].gameObject == kvp.Value[i])continue;
						Debug.Log("obj[j].gameObject.name" + obj[j].gameObject.name);
						if (lastChooseIconObject == obj[j].gameObject)
						{
							Destroy(kvp.Value[i]);
							kvp.Value.RemoveAt(i);
							AllWindowsStruct[mainSingleWindowinUseIndex].allFloorItem[AllWindowsStruct[mainSingleWindowinUseIndex].inUseTab2ComponentLayerIndex][kvp.Key].Clear();
							break;
						}
					}

				}*/
			}
			if (isMainComponentObject)//如果點到的東西是MainComponentObject
			{
				//AllWindowsStruct[mainSingleWindowinUseIndex].ClearAllComponent();
				GameObject initWindowsIconDragObject = ReturnInitWindowsIcon(mainSingleWindowinUseIndex);
				SetWindowsComponent(mainSingleWindowinUseIndex, initWindowsIconDragObject);
				Debug.Log("addMain");
			}

		}
		else
		{
			//AllWindowsStruct[mainSingleWindowinUseIndex].ClearAllComponent();
			GameObject initWindowsIconDragObject = ReturnInitWindowsIcon(mainSingleWindowinUseIndex);
			SetWindowsComponent(mainSingleWindowinUseIndex, initWindowsIconDragObject);
			Debug.Log("addMain");
		}
		IconControlMenuPanel.SetActive(false);
	}
	//隱藏開啟MissionTab
	void SetMissionTab(int index)
	{
		if (changeLayoutIndexInWindowsSet == (int)WindowsSetIndex.SingleWindow)
		{
			for (int i = 0; i < AllWindowsStruct.Length; i++)
			{
				AllWindowsStruct[i].misstionTab.SetAllMisstionTabsActive(false);
			}
			if (AllWindowsStruct[index].misstionTab.missionTabsList.Count>0)
			{
				AllWindowsStruct[index].misstionTab.SetAllMisstionTabsActive(true);
			}
		}
	}
	//BUTTON排序
	void SwapGameObject(GameObject a, GameObject b)
	{
		Vector3 temp = a.transform.position;

		a.transform.position = b.transform.position;
		b.transform.position = temp;
	}
	//單純視窗按鈕交換
	void SwapButton2MainSingleWindow(int index)
	{
		if (mainSingleWindowinUseIndex == index) return;

		SwapGameObject(windowSwitchButtonList[index], windowSwitchButtonList[mainSingleWindowinUseIndex]);

		mainSingleWindowinUseIndex = index;

	}
	//依照四大視窗按鈕順序
	void SortWindowSwitchButtonList2MainSingleWindow(int index)
	{
		if (mainSingleWindowinUseIndex == index) return;

		int diff = Mathf.Abs(mainSingleWindowinUseIndex - index);

		if (diff > 1)
		{
			if (mainSingleWindowinUseIndex < index)
			{
				for (int i = 0; i < diff - 1; i++)
				{
					SwapGameObject(windowSwitchButtonList[index], windowSwitchButtonList[index - 1 - i]);
				}
			}
			else if (mainSingleWindowinUseIndex > index)
			{
				for (int i = 0; i < diff - 1; i++)
				{
					SwapGameObject(windowSwitchButtonList[index], windowSwitchButtonList[index + 1 + i]);
				}
			}
		}
		SwapGameObject(windowSwitchButtonList[index], windowSwitchButtonList[mainSingleWindowinUseIndex]);

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
		//減少一層樓 清除記憶體
		AllWindowsStruct[index].temporateAllFloorItem[AllWindowsStruct[index].lastChooseMainDragObjectName][AllWindowsStruct[index].allFloorItem.Count - 1].Clear();
		AllWindowsStruct[index].temporateAllFloorItem[AllWindowsStruct[index].lastChooseMainDragObjectName].RemoveAt(AllWindowsStruct[index].allFloorItem.Count - 1);
		if (AllWindowsStruct[index].inUseTab2ComponentLayerIndex == AllWindowsStruct[index].allFloorItem.Count - 1)//最後一個missionTab對應的commponent內容正在編輯
		{
			//刪除最後一個

			AllWindowsStruct[index].DeleteAllComponent(AllWindowsStruct[index].allFloorItem.Count - 1);
			//顯示前一個
			AllWindowsStruct[index].inUseTab2ComponentLayerIndex--;
			AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex] = AllWindowsStruct[index].temporateAllFloorItem[AllWindowsStruct[index].lastChooseMainDragObjectName][AllWindowsStruct[index].inUseTab2ComponentLayerIndex];
			AllWindowsStruct[index].ShowAllComponent();
		}
		else//最後一個missionTab對應的commponent內容沒有正在編輯
		{
			//刪除最後一個

			AllWindowsStruct[index].DeleteAllComponent(AllWindowsStruct[index].allFloorItem.Count - 1);
		}
	}
	//按下刪除鈕後，清除選用視窗當前的missionTab對應的commponent內容
	void ClearCurrentTab2Window(int index)
	{
		SaveState2MainComponent(index);
		//減少一層樓 清除記憶體
		AllWindowsStruct[index].temporateAllFloorItem[AllWindowsStruct[index].lastChooseMainDragObjectName][AllWindowsStruct[index].inUseTab2ComponentLayerIndex].Clear();
		AllWindowsStruct[index].temporateAllFloorItem[AllWindowsStruct[index].lastChooseMainDragObjectName].RemoveAt(AllWindowsStruct[index].inUseTab2ComponentLayerIndex);
		AllWindowsStruct[index].DeleteAllComponent(AllWindowsStruct[index].inUseTab2ComponentLayerIndex);
		//刪除當前的那一個
		if (AllWindowsStruct[index].inUseTab2ComponentLayerIndex > 0)
		{
			AllWindowsStruct[index].inUseTab2ComponentLayerIndex--;

		}
		AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex] = AllWindowsStruct[index].temporateAllFloorItem[AllWindowsStruct[index].lastChooseMainDragObjectName][AllWindowsStruct[index].inUseTab2ComponentLayerIndex];
		AllWindowsStruct[index].ShowAllComponent();

	}
	//
	void InitWindowSetSetting()
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

						SetWindowsComponent(index, chooseDragObject);
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

					SetWindowsComponent(mainSingleWindowinUseIndex, chooseDragObject);
				}
				break;
		}
	}
	void SetWindowsComponent(int index, GameObject setDragObject)
	{
		if (setDragObject==null) return;
		SaveState2MainComponent(index);
		switch (setDragObject.tag)
		{
			case MAINCOMPONENT:
				movement.intiAllList();
				Debug.Log(AllWindowsStruct[index].lastChooseMainDragObjectName);
				if (!AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex].ContainsKey(MAINCOMPONENT))//在選擇的視窗內 且視窗內物件為空
				{
					CreateMainComponent(index, setDragObject);
					Debug.Log("000");
				}
				else//視窗內物件為不為空
				{
					if (AllWindowsStruct[index].lastChooseMainDragObjectName != setDragObject.name)//如果不是拖曳同一個主物件取代原本的主物物件
					{
						//紀錄操作的物件
						if (AllWindowsStruct[index].temporateAllFloorItem.ContainsKey(setDragObject.name)) //有編輯過此視窗
						{
							AllWindowsStruct[index].HideAllComponent();
							AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex] = AllWindowsStruct[index].temporateAllFloorItem[setDragObject.name][AllWindowsStruct[index].inUseTab2ComponentLayerIndex];
							AllWindowsStruct[index].ShowAllComponent();
							Debug.Log("1111");

						}
						else //沒有編輯過此視窗
						{
							AllWindowsStruct[index].HideAllComponent();
							CreateMainComponent(index, setDragObject);
							Debug.Log("222");
						}
					}
					else //如果拖曳同一個主物件
					{
						//取代原本的主物物件 清除此視窗物件
						AllWindowsStruct[index].ClearAllComponent();
						CreateMainComponent(index, setDragObject);
						Debug.Log("333");

					}
				}
				AllWindowsStruct[index].lastChooseMainDragObjectName = setDragObject.name;
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
					GameObject correspondingDragItemObject = CreateDecorateComponent(index);

					if (AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>())
					{

						if (AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex].ContainsKey(setDragObject.name)) AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().UpdateFunction(setDragObject.name, correspondingDragItemObject);



						if (setDragObject.name == "Balustrade")
						{

							//building.UpdateBody_F(AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().isFrieze);
							building.UpdateBody_F(true);
						}

						if (setDragObject.name == "Frieze")
						{
							//building.UpdateBody_B(AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().isBalustrade);
							building.UpdateBody_B(true);
						}

						/*
						building.Move_F(AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().friezeHeight, AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().ini_bodydis.y);
						building.Move_B(AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().balustradeHeight, AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].GetComponent<body2icon>().ini_bodydis.y);
						*/
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
				Dictionary<string, List<GameObject>> copy = new Dictionary<string, List<GameObject>>(AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex]);
				List<Dictionary<string, List<GameObject>>> tmpList = new List<Dictionary<string, List<GameObject>>>();
				tmpList.Add(copy);
				AllWindowsStruct[index].temporateAllFloorItem.Add(AllWindowsStruct[index].lastChooseMainDragObjectName, tmpList);
			}
			else
			{
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

		//clone.transform.parent = this.transform;

		List<GameObject> allComponentList = new List<GameObject>();
		allComponentList.Add(clone);
		AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex].Add(MAINCOMPONENT, allComponentList);
	}
	void CreateMainComponent(int index)
	{
		Vector3 pos = chooseCamera.transform.position; pos.z = chooseCamera.farClipPlane / 2.0f;

		GameObject cloneCorrespondingObj = chooseDragObject.GetComponent<CorespondingDragItem>().corespondingDragItem;

		GameObject clone = Instantiate(cloneCorrespondingObj, pos, cloneCorrespondingObj.transform.rotation) as GameObject;

		//clone.transform.parent = this.transform;

		List<GameObject> allComponentList = new List<GameObject>();
		allComponentList.Add(clone);
		AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex].Add(MAINCOMPONENT, allComponentList);
	}
	GameObject CreateDecorateComponent(int index)
	{
		GameObject clone = null;
		Vector3 pos = chooseCamera.transform.position; pos.z = chooseCamera.farClipPlane / 2.0f;
		GameObject cloneCorrespondingObj=null;
		if (chooseDragObject.GetComponent<CorespondingDragItem>().corespondingDragItem!=null)
			cloneCorrespondingObj = chooseDragObject.GetComponent<CorespondingDragItem>().corespondingDragItem;

		int correspondingDragItemMaxCount = chooseDragObject.GetComponent<CorespondingDragItem>().correspondingDragItemMaxCount;
		switch (chooseDragObject.name)
		{
			case "MutiBody":
				if (index == (int)WindowsIndex.Body)
				{
					if (AllWindowsStruct[index].misstionTab.IsNotOverMaxCount(correspondingDragItemMaxCount))
					{
						SaveState2MainComponent(index);
						AllWindowsStruct[index].HideAllComponent();
						AllWindowsStruct[index].inUseTab2ComponentLayerIndex = AllWindowsStruct[index].allFloorItem.Count;
						//增加
						Dictionary<string, List<GameObject>> newAllComponent = new Dictionary<string, List<GameObject>>();

						AllWindowsStruct[index].allFloorItem.Add(newAllComponent);

						AllWindowsStruct[index].temporateAllFloorItem[AllWindowsStruct[index].lastChooseMainDragObjectName].Add(newAllComponent);

						AllWindowsStruct[index].misstionTab.CreateMissionTabs(this, misstionTabObj, chooseWindow);

						CreateMainComponent(index);

						generalViewCamera.GetComponent<CameraToCenter>().ChangeCenter2TargetObject(generalViewCameraCenterOffset, generalViewCameraDisOffset, AllWindowsStruct[index].allFloorItem.Count-1);

						building.upup(17.5f);

			

					}
				}
				break;
			case "MutiPlatform":
				if (index == (int)WindowsIndex.Platform)
				{
					if (AllWindowsStruct[index].misstionTab.IsNotOverMaxCount(correspondingDragItemMaxCount))
					{
						SaveState2MainComponent(index);
						AllWindowsStruct[index].HideAllComponent();
						AllWindowsStruct[index].inUseTab2ComponentLayerIndex = AllWindowsStruct[index].allFloorItem.Count;

						Dictionary<string, List<GameObject>> newAllComponent = new Dictionary<string, List<GameObject>>();

						AllWindowsStruct[index].allFloorItem.Add(newAllComponent);

						AllWindowsStruct[index].temporateAllFloorItem[AllWindowsStruct[index].lastChooseMainDragObjectName].Add(newAllComponent);

						AllWindowsStruct[index].misstionTab.CreateMissionTabs(this, misstionTabObj, chooseWindow);

						CreateMainComponent(index);
					}
				}
				break;
			default:
				if (AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex].ContainsKey(chooseDragObject.name))
				{
					if (AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][chooseDragObject.name].Count < correspondingDragItemMaxCount)
					{
						if (cloneCorrespondingObj!=null)
							clone = Instantiate(cloneCorrespondingObj, pos, cloneCorrespondingObj.transform.rotation) as GameObject;
						else
						{
							clone=new GameObject();	
						}
						//clone.transform.parent = this.transform;
						clone.transform.parent = AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].transform;
						if (!clone.GetComponent<DecorateEmptyObjectList>()) clone.AddComponent<DecorateEmptyObjectList>();
						AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][chooseDragObject.name].Add(clone);
					}
					else
						Debug.Log(chooseDragObject.name + "    Count over MaxCount");
				}
				else
				{
					if (cloneCorrespondingObj != null)
						clone = Instantiate(cloneCorrespondingObj, pos, cloneCorrespondingObj.transform.rotation) as GameObject;
					else
					{
						clone = new GameObject();
					}
					//clone.transform.parent = this.transform;
					clone.transform.parent = AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].transform;

					//clone.transform.SetParent(AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex][MAINCOMPONENT][0].transform, false);

					if (!clone.GetComponent<DecorateEmptyObjectList>()) clone.AddComponent<DecorateEmptyObjectList>();
					List<GameObject> newList = new List<GameObject>();
					newList.Clear();
					newList.Add(clone);
					AllWindowsStruct[index].allFloorItem[AllWindowsStruct[index].inUseTab2ComponentLayerIndex].Add(chooseDragObject.name, newList);
				}

				if (chooseDragObject.name == "Wall")
				{
					building.Bodytruefalse(true);
				}

				if (chooseDragObject.name == "DoubleRoof")
				{
					building.Double_Eave();
				}

				break;
		}
		return clone;
	}
}
