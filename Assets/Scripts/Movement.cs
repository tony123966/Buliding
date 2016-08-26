using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movement : MonoBehaviour {

	public List<GameObject> horlist; 
	public List<GameObject> verlist; 
	public List<GameObject> freelist;
	// Use this for initialization
	public DragItemController dragitemcontroller;
	public MeshObj meshobj;
	void Start () {
		dragitemcontroller = GameObject.Find ("DragItemController").GetComponent<DragItemController> ();

	}
	
	// Update is called once per frame
	void Update () {
	}

	void Awake(){
		
		List<GameObject> horlist =  new List<GameObject>();
		List<GameObject> verlist =  new List<GameObject>(); 
		List<GameObject> freelist = new List<GameObject>();


	
	
	
	}
	public void move(Vector2 mospos_){
		
		//1.找自己在哪個list ver or hor
		GameObject obj = dragitemcontroller.chooseObj;


		for (int h = 0; h < horlist.Count; h++) {
			if (obj == horlist [h]) {
				horizontal (mospos_,obj);
			}
		}


		// 找水平

		//找垂直
		for (int v = 0; v < verlist.Count; v++){
			if (obj == verlist [v]) {
				obj.transform.position = new Vector3 (obj.transform.position.x, mospos_.y, obj.transform.position.z);
			}
		}
		//free
		for (int f = 0; f < freelist.Count; f++) {
			if (obj == freelist [f]) {
				obj.transform.position = new Vector3 (mospos_.x, mospos_.y,obj.transform.position.z);
			}
		}
		if (obj.transform.parent.GetComponent<MeshObj> ()) {
			obj.transform.parent.GetComponent<MeshObj> ().adjPos ();
		}
		if (obj.transform.parent.GetComponent<platform2icon> ()) {
			obj.transform.parent.GetComponent<platform2icon> ().adjPos ();
		}
		if (obj.transform.parent.GetComponent<body2icon> ()) {
			obj.transform.parent.GetComponent<body2icon> ().adjPos ();
		}


		// free
		//2.limit movement

	
	
	
	}
	void horizontal(Vector2 mospos_, GameObject obj){
		
		meshobj = GameObject.FindWithTag ("Rectangle").GetComponent<MeshObj> ();
		//meshobj = GameObject.Find("RectangleObj()").GetComponent<MeshObj> ();
		float min = meshobj.controlPointList [0].transform.position.x;
		float max = meshobj.controlPointList [1].transform.position.x;
		float b = Mathf.Clamp (mospos_.x,min,max);
		obj.transform.position = new Vector3 (b, obj.transform.position.y, obj.transform.position.z);

	}
}
