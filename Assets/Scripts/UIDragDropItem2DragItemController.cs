using UnityEngine;
using System.Collections;

public class UIDragDropItem2DragItemController : UIDragDropItem
{
	private DragItemController dragItemController;
	protected override void Start()
	{
		base.Start();
		dragItemController=GameObject.Find("DragItemController").GetComponent<DragItemController>();
	}
	protected override void OnDragDropStart()
	{
		base.OnDragDropStart();
		if (dragItemController!=null)
		{
			dragItemController.chooseDragObject = gameObject;
		}
	}
	protected override void OnDragDropRelease(GameObject surface)
	{
		base.OnDragDropRelease(surface);

		if (dragItemController!=null)
		{
			dragItemController.SetObjInWindows();
			dragItemController.chooseDragObject = null;
		}
	}
}
