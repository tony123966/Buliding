using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class body2icon : MonoBehaviour {


	public List<GameObject> list2column = new List<GameObject> ();
	public List<GameObject> cp2body = new List<GameObject> ();



	public DragItemController dragitemcontroller;
	public Movement movement;
	void Awake(){
	
		movement = GameObject.Find ("Movement").GetComponent<Movement> ();
		movement.verlist = cp2body;
		movement.freelist = list2column;
	
	}
	// Use this for initialization
	void Start () {
		dragitemcontroller = GameObject.Find ("DragItemController").GetComponent<DragItemController> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void adjPos(){
		for (int i = 0; i < cp2body.Count; i++) {
			if (dragitemcontroller.chooseObj == cp2body [i]) { 
	
	
	
	
	
	
	
			}
		}
	}
}
