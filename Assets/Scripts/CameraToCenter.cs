using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraToCenter : MonoBehaviour
{
	private Camera uICamera;
	public GameObject constraintArea;

	public GameObject target;//the target object
	public float dragSpeedMod = 10.0f;//a speed modifier
	public float roomInSpeedMod = 20.0f;//a speed modifier

	public bool clampXY = false;
	public float minDistance = 5.0f;
	public float maxDistance = 300.0f;


	private Vector3 targetPoint;//the coord to the point where the camera looks at
	private float x = 0;
	private float y = 0;
	private int isClamp = 0;
	private bool isRotating = false;
	private Bounds bounds;
	private float originMaxDistance;
	public float distanceToTarget;
	void Start()
	{//Set up things on the start method
		uICamera = GameObject.Find("UICamera").GetComponent<Camera>();
		bounds = NGUIMath.CalculateAbsoluteWidgetBounds(constraintArea.transform);
		targetPoint = target.transform.position;//get target's coords
		transform.LookAt(targetPoint);//makes the camera look to it
		originMaxDistance=maxDistance;
		distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
	}
	public void ChangeCenter2TargetObject(Vector3 focusNewPos,float disOffset) 
	{
		targetPoint = focusNewPos;
		 transform.LookAt(targetPoint);
		 transform.position -= (transform.forward * disOffset);
		 distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
		 maxDistance = originMaxDistance + disOffset;
	}
	void Update()
	{//makes the camera rotate around "point" coords, rotating around its Y axis, 20 degrees per second times the speed modifier
		Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		Vector2 mousePos2World = uICamera.ScreenToWorldPoint(mousePos);
		if (bounds.Contains(mousePos2World))
		{
			if (Input.GetMouseButtonDown(0) && !isRotating) isRotating = true;

			if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
			{
				Vector3 dotVector=target.transform.position- transform.position;
				Vector3 offset=(transform.forward * Mathf.Sign(Input.GetAxisRaw("Mouse ScrollWheel")) * roomInSpeedMod * Time.smoothDeltaTime);
				distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
				float afterDistanceToTarget = Vector3.Distance(target.transform.position, transform.position + offset);
				Vector3 afterDotVector = target.transform.position - (transform.position + offset);
				if (maxDistance > afterDistanceToTarget && minDistance < afterDistanceToTarget && Vector3.Dot(afterDotVector, dotVector)>0)
				{
					transform.position += offset;
				}
			}
		}
		if (isRotating)
		{
			if (clampXY)
			{
				if (Mathf.Abs(Input.GetAxis("Mouse X")) > Mathf.Abs(Input.GetAxis("Mouse Y")) && isClamp != -1)
				{
					isClamp = 1;
					x += Input.GetAxis("Mouse X") * dragSpeedMod * Time.smoothDeltaTime;

					transform.RotateAround(targetPoint, target.transform.up, x);

				}
				else if (Mathf.Abs(Input.GetAxis("Mouse X")) < Mathf.Abs(Input.GetAxis("Mouse Y")) && isClamp != 1)
				{
					isClamp = -1;

					y += Input.GetAxis("Mouse Y") * dragSpeedMod * Time.smoothDeltaTime;
					transform.RotateAround(targetPoint, -transform.right, y);
				}
			}
			else
			{
				x += Input.GetAxis("Mouse X") * dragSpeedMod * Time.smoothDeltaTime;
				y += Input.GetAxis("Mouse Y") * dragSpeedMod * Time.smoothDeltaTime;
				transform.RotateAround(targetPoint, target.transform.up, x);
				transform.RotateAround(targetPoint, -transform.right, y);
			}

		}
		if (Input.GetMouseButtonUp(0))
		{
			x = 0; y = 0;
			isClamp = 0;
			isRotating = false;
		}
	}
}