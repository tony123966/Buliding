using UnityEngine;
using System.Collections;

public class CameraWindow : MonoBehaviour{

	private Rect windowPosition;//儲存可被拖曳的window的位置
	private Rect windowPositionDrag;//儲存可被拖曳的window的位置
	private Rect buttonPosition;//儲存button在window內的位置
	private Rect buttonPositionScale;//儲存button在window內的位置
	private string title;
	private int windowsId;


	public float windowWidth;
	public float windowHeight;
	public float windowWidthDrag;
	public float windowHeightDrag;
	public float buttonWidthClose;//按鈕的寬度
	public float buttonHeightClose;//按鈕的高度
	public float buttonWidthScale;//按鈕的寬度
	public float buttonHeightScale;//按鈕的高度


	public GUISkin customSkin;
	
	public bool render = true;


	public Camera camera;
	public string cameraName;
	private Rect cameraRectPos;//儲存camera在window內的位置
	private Rect cameraRectRect;

	private DrawShape drawShape;

	void initSetting() 
	{
		windowWidth = 200f;
		windowHeight = 250f;
		windowWidthDrag = windowWidth;
		windowHeightDrag = windowHeight*0.1f;
		buttonWidthClose = 20f;//按鈕的寬度
		buttonHeightClose = 20f;//按鈕的高度
		buttonWidthScale = 5f;//按鈕的寬度
		buttonHeightScale = 5f;//按鈕的高度

		drawShape = GameObject.Find("ShapeController").GetComponent<DrawShape>();
		camera = GameObject.Find(cameraName).GetComponent<Camera>();

		windowPositionDrag = new Rect(0, 0, windowWidthDrag, windowHeightDrag); 
	}
	public void Create(float PosX, float PosY, string Title, int WindowsID)
	{
		title = Title;
		windowsId = WindowsID;
		

		GameObject obj= new GameObject("Camera");
		camera = obj.AddComponent<Camera>();
		camera.gameObject.AddComponent<GUILayer>();
		cameraName = camera.name;
		
		initSetting();
		ShowWindow();
		setMoveWindowPosition(PosX, PosY);
		setButtonPosition();

		GUI.BringWindowToFront(windowsId);
	}

	public void Create(float PosX, float PosY, string Title, int WindowsID, string CameraName)
	{
		title = Title;
		windowsId = WindowsID;
			
		cameraName = CameraName;
		
		initSetting();
		ShowWindow();
		setMoveWindowPosition(PosX, PosY);
		setButtonPosition();

		GUI.BringWindowToFront(windowsId);
	}
	public void setMoveWindowPosition(float PosX, float PosY)//設定window的位置
	{
		float windowLeft = PosX - windowWidth * 0.5f;//window和Game左邊的距離，目前設定的值會讓window顯示在螢幕正中央
		float windowTop = PosY - windowHeight * 0.5f;//window和Game上面的距離，目前設定的值會讓window顯示在螢幕正中央
		windowPosition = new Rect(windowLeft, windowTop, windowWidth, windowHeight);//將可被拖曳的視窗設定在Game中央
	}
	public void setButtonPosition()//設定windows內的button位置
	{
		float buttonLeft = windowPosition.width - buttonWidthClose;//按鈕和window左邊的距離，目前的值會讓button顯示在window的正中央
		float buttonTop = 0;//按鈕和window上面的距離，目前的值會讓button顯示在window的正中央

		buttonPosition = new Rect(buttonLeft, buttonTop, buttonWidthClose, buttonHeightClose);


		buttonLeft = windowPosition.width - buttonWidthScale;//按鈕和window左邊的距離，目前的值會讓button顯示在window的正中央
		buttonTop = windowPosition.height - buttonHeightScale;//按鈕和window上面的距離，目前的值會讓button顯示在window的正中央

		buttonPositionScale = new Rect(buttonLeft, buttonTop, buttonWidthScale, buttonHeightScale);
	}
	private void OnGUI()
	{
		if (render)
		{
			GUI.skin = customSkin;
			//顯示window，可以被拖曳
			windowPosition = GUI.Window(windowsId, windowPosition, windowEvent, title);
			// viewport adjustment code:
			float w = Screen.width;
			float h = Screen.height;
			// make a copy of the window rect...
			cameraRectRect = windowPosition;
			cameraRectRect.y += 20; // add the top margin...
			cameraRectRect.x += 10; // add the left margin...
			cameraRectRect.width -= 20; // adjust width to include left + right margins...
			cameraRectRect.height -= 30; // adjust height to include top + down margins... 
			// set the camera viewport size:
			camera.rect = new Rect(cameraRectRect.x / w, (h - cameraRectRect.y - cameraRectRect.height) / h, cameraRectRect.width / w, cameraRectRect.height / h);

		}
	}
	public void ShowWindow()
	{
		render = true;
		camera.enabled=true;
	}
	public void HideWindow()
	{
		render = false;
		camera.enabled = false;
		Debug.Log("hide camera");
	}
	private void windowEvent(int id)//處理視窗裡面要顯示的文字、按鈕、事件處理。必須要有一個為int的傳入參數
	{

		if (GUI.Button(buttonPosition, "X"))//在window上顯示按鈕
		{
			if (id == windowsId)
			{
				GUI.BringWindowToFront(windowsId);
				this.HideWindow();
			}
		}
		if (id == windowsId)
		{
			GUI.BringWindowToFront(windowsId);
			windowPositionDrag = new Rect(0, 0, windowWidthDrag, windowHeightDrag); 
			GUI.DragWindow(windowPositionDrag);
			Vector3 Pos = new Vector3(Input.mousePosition.x,Screen.height-Input.mousePosition.y,0);
			if (windowPosition.Contains(Pos) && drawShape.chooseObj==null)
			{
				float offsetX = windowPosition.width * 0.05f;
				float offsetY = windowPosition.height * 0.05f;
				drawShape.chooseCamera = camera;
				drawShape.choosewindowRect = new Rect(windowPosition.x + offsetX, windowPosition.y + offsetY + windowHeightDrag, windowPosition.width - 2 * offsetX, windowPosition.height - 2 * offsetY - windowHeightDrag);
			}
		}

	}
/*
	void DrawQuad(Rect position, Color color)
	{
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0, 0, color);
		texture.Apply();
		GUI.skin.box.normal.background = texture;
		GUI.Box(position, GUIContent.none);
	}
	void DrawQuad(Vector2 start, Vector2 end, Color color)
	{
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0, 0, color);
		texture.Apply();
		GUI.skin.box.normal.background = texture;
		Vector2 offsetRec = new Vector2(windowPositionMove.x, windowPositionMove.y);
		Rect boxRec = new Rect(start.x + offsetRec.x, start.y + offsetRec.y, end.x - start.x, end.y - start.y);
		GUI.Box(boxRec, GUIContent.none);
	}
	private void DrawLine(Vector2 start, Vector2 end, int width)
	{
		Vector2 offsetRec = new Vector2(windowPositionMove.x, windowPositionMove.y);
		start += offsetRec;
		end += offsetRec;
		Vector2 d = end - start;
		Texture2D texture = new Texture2D(1, 1);
		float a = Mathf.Rad2Deg * Mathf.Atan(d.y / d.x);
		if (d.x < 0)
			a += 180;

		int width2 = (int)Mathf.Ceil(width / 2);

		GUIUtility.RotateAroundPivot(a, start);
		GUI.DrawTexture(new Rect(start.x, start.y - width2, d.magnitude, width), texture);
		GUIUtility.RotateAroundPivot(-a, start);
	}
	private void DrawCircle(Vector2 centerPos, float radius = 50)
	{
		Vector2 offsetRec = new Vector2(windowPositionMove.x, windowPositionMove.y);
		float DEG2RAD = 3.14159f / 180;
		GL.Begin(GL.LINES);
		GL.Color(Color.red);
		for (int i = 0; i < 360; i++)
		{
			float degInRad = i * DEG2RAD;
			GL.Vertex(centerPos + offsetRec);
			GL.Vertex(new Vector2(centerPos.x + Mathf.Cos(degInRad) * radius, centerPos.y + Mathf.Sin(degInRad) * radius) + offsetRec);
		}
		GL.End();
	}*/
}
