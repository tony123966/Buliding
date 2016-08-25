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


		// 找水平
		for (int h = 0; h < horlist.Count; h++) {
			if (obj == horlist [h]) {
				obj.transform.position = new Vector3 (mospos_.x, obj.transform.position.y, obj.transform.position.z);
			}
		}
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

		obj.transform.parent.GetComponent<MeshObj>().adjPos();


		// free
		//2.limit movement

	
	
	
	}
	void vertical (){
	
	
	
	}
	void horizontal(){
	
	
	
	}
}
