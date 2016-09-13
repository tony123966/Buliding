using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraToCenter : MonoBehaviour
{
	private Camera uICamera;
	public GameObject constraintArea;

	public GameObject target;//the target object
	private float speedMod = 10.0f;//a speed modifier
	private Vector3 point;//the coord to the point where the camera looks at
	public bool clampXY = false;
	private float x = 0;
	private float y = 0;
	private int isClamp = 0;
	private bool isRotating = false;
	private Bounds bounds;
	void Start()
	{//Set up things on the start method
		uICamera = GameObject.Find("UICamera").GetComponent<Camera>();
		bounds = NGUIMath.CalculateAbsoluteWidgetBounds(constraintArea.transform);
		point = target.transform.position;//get target's coords
		transform.LookAt(point);//makes the camera look to it
	}

	void Update()
	{//makes the camera rotate around "point" coords, rotating around its Y axis, 20 degrees per second times the speed modifier
		Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		Vector2 mousePos2World = uICamera.ScreenToWorldPoint(mousePos);
		if (bounds.Contains(mousePos2World) && Input.GetMouseButtonDown(0) && !isRotating)
		{
			isRotating=true;
		}
		if (isRotating)
		{
				if (clampXY) {
				if (Mathf.Abs(Input.GetAxis("Mouse X")) > Mathf.Abs(Input.GetAxis("Mouse Y")) && isClamp != -1)
				{
					isClamp = 1;
					x += Input.GetAxis("Mouse X") * speedMod * Time.deltaTime;

					transform.RotateAround(point, target.transform.up, x * Time.deltaTime * speedMod);

				}
				else if (Mathf.Abs(Input.GetAxis("Mouse X")) < Mathf.Abs(Input.GetAxis("Mouse Y")) && isClamp != 1)
				{
					isClamp = -1;

					y += Input.GetAxis("Mouse Y") * speedMod * Time.deltaTime;
					transform.RotateAround(point, -transform.right, y * Time.deltaTime * speedMod);
				}
			}
			else 
			{
				x += Input.GetAxis("Mouse X") * speedMod * Time.deltaTime;
				y += Input.GetAxis("Mouse Y") * speedMod * Time.deltaTime;
				transform.RotateAround(point, target.transform.up, x * Time.deltaTime * speedMod);
				transform.RotateAround(point, -transform.right, y * Time.deltaTime * speedMod);
			}
		
		}
		if (Input.GetMouseButtonUp(0))
		{
			x = 0; y = 0;
			isClamp = 0;
			isRotating=false;
		}
		
	}
}