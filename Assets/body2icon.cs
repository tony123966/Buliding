using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class body2icon : MonoBehaviour {


	//public List<GameObject> list2column = new List<GameObject> ();
	public List<GameObject> cp2body = new List<GameObject> ();
	public List<GameObject> cloum2body = new List<GameObject> ();
	public List<GameObject> Left2body = new List<GameObject> ();
	public List<GameObject> Right2body = new List<GameObject> ();
	//just try two point



	public DragItemController dragitemcontroller;
	public Movement movement;
	public Mesh cylinderMesh;
	public Material lineMat;

	public GameObject R_up_point;
	public GameObject R_down_point;
	public GameObject L_up_point;
	public GameObject L_down_point;


	public float radius = 0.01f;

	GameObject R_cylinder;
	GameObject L_cylinder;
	Vector3 tmpscale;
	// Use this for initialization
	void Start () {
		dragitemcontroller = GameObject.Find ("DragItemController").GetComponent<DragItemController> ();
	}
	void Awake(){

		movement = GameObject.Find ("Movement").GetComponent<Movement> ();
		movement.verlist.AddRange (cp2body);
//
//		GameObject ringOffsetCylinderMeshObject = new GameObject ();
//
//		ringOffsetCylinderMeshObject.transform.localPosition = new Vector3(0f, 1f, 0f);
//		ringOffsetCylinderMeshObject.transform.localScale = new Vector3(0.05f, 1f, 0.05f);



		R_cylinder = GameObject.CreatePrimitive (PrimitiveType.Cylinder);
		L_cylinder = GameObject.CreatePrimitive (PrimitiveType.Cylinder);


		R_cylinder.transform.parent = this.gameObject.transform;
		L_cylinder.transform.parent = this.gameObject.transform;


		float dis = Vector3.Distance (R_up_point.transform.position, R_down_point.transform.position);
		dis = dis / 2;
		R_cylinder.transform.localScale = new Vector3 (radius, dis, radius);
		R_cylinder.transform.position = new Vector3 (R_up_point.transform.position.x, R_up_point.transform.position.y - dis, R_up_point.transform.position.z); 
		L_cylinder.transform.localScale = new Vector3 (radius, dis, radius);
		L_cylinder.transform.position = new Vector3 (L_up_point.transform.position.x, L_up_point.transform.position.y - dis, L_up_point.transform.position.z); 


		cloum2body.Add (R_cylinder);
		cloum2body.Add (L_cylinder);
		movement.horlist.AddRange (cloum2body);

		R_cylinder.tag = "Cylinder";
		L_cylinder.tag = "Cylinder";


		Left2body.Add (L_cylinder);
		Left2body.Add (L_down_point);
		Left2body.Add (L_up_point);

		Right2body.Add (R_cylinder);
		Right2body.Add (R_down_point);
		Right2body.Add (R_up_point);



		//print ("awake:R_up_point:" + R_up_point.transform.position);
		//print ("dis1:" + dis);


		//MeshFilter ringMesh = cylinder.AddComponent<MeshFilter>();
		//ringMesh.mesh = this.cylinderMesh;
		//cylinder.transform.LookAt(this.up_point.transform.position, Vector3.up);

//		MeshRenderer RRenderer = R_cylinder.AddComponent<MeshRenderer>();
//		MeshRenderer LRenderer = L_cylinder.AddComponent<MeshRenderer>();
//		RRenderer.material = lineMat;
//		LRenderer.material = lineMat;


//		float cylinderDistance = 0.5f * Vector3.Distance (this.up_point.transform.position, this.down_point.transform.position);
//		this.cylinder.transform.localScale = new Vector3 (this.cylinder.transform.localScale.x, cylinderDistance, this.cylinder.transform.localScale.z);
//
//		this.cylinder.transform.LookAt (this.up_point.transform, Vector3.up);
//
	}
	// Update is called once per frame
	void Update () {

 	}

	public void adjPos(Vector2 mospos_){



		if (dragitemcontroller.chooseObj.name == "RU" || dragitemcontroller.chooseObj.name == "LU") {
			float dis2 = (dragitemcontroller.chooseObj.transform.position.y - R_down_point.transform.position.y);
			dis2 = dis2 / 2;
			dis2 = Mathf.Abs (dis2);

			float tmp_y = dragitemcontroller.chooseObj.transform.position.y;
			//update point
			R_up_point.transform.position = new Vector3 (R_up_point.transform.position.x, tmp_y, R_up_point.transform.position.z);
			L_up_point.transform.position = new Vector3 (L_up_point.transform.position.x, tmp_y, L_up_point.transform.position.z);

			R_cylinder.transform.localScale = new Vector3 (radius, dis2, radius);
			R_cylinder.transform.position = new Vector3 (R_up_point.transform.position.x, R_up_point.transform.position.y - dis2, R_up_point.transform.position.z);
			L_cylinder.transform.localScale = new Vector3 (radius, dis2, radius);
			L_cylinder.transform.position = new Vector3 (L_up_point.transform.position.x, L_up_point.transform.position.y - dis2, L_up_point.transform.position.z);



		} else if (dragitemcontroller.chooseObj.name == "RD" || dragitemcontroller.chooseObj.name == "LD") {
			float dis2 = (dragitemcontroller.chooseObj.transform.position.y - R_up_point.transform.position.y);
			dis2 = dis2 / 2;
			dis2 = Mathf.Abs (dis2);
			float tmp_y = dragitemcontroller.chooseObj.transform.position.y;
			//update point
			R_down_point.transform.position = new Vector3 (R_down_point.transform.position.x, tmp_y, R_down_point.transform.position.z);
			L_down_point.transform.position = new Vector3 (L_down_point.transform.position.x, tmp_y, R_down_point.transform.position.z);

			R_cylinder.transform.localScale = new Vector3 (radius, dis2, radius);
			R_cylinder.transform.position = new Vector3 (R_up_point.transform.position.x, R_up_point.transform.position.y - dis2, R_up_point.transform.position.z);
			L_cylinder.transform.localScale = new Vector3 (radius, dis2, radius);
			L_cylinder.transform.position = new Vector3 (L_up_point.transform.position.x, L_up_point.transform.position.y - dis2, L_up_point.transform.position.z);
		
		
		} else if (dragitemcontroller.chooseObj.name == "Cylinder") {


			if (dragitemcontroller.chooseObj == R_cylinder) {
				float dis = (dragitemcontroller.chooseObj.transform.position.x - R_up_point.transform.position.x);
				for (int i = 0; i < Right2body.Count; i++) {
					Right2body [i].transform.position = new Vector3 (mospos_.x, Right2body [i].transform.position.y, 0.0f);	
					Left2body [i].transform.position = new Vector3 (Left2body [i].transform.position.x - (dis), Left2body [i].transform.position.y, 0.0f);			

				}

			} else {
				float dis = (dragitemcontroller.chooseObj.transform.position.x - L_up_point.transform.position.x);

				for (int i = 0; i < Left2body.Count; i++) {
					Left2body [i].transform.position = new Vector3 (mospos_.x, Left2body [i].transform.position.y, 0.0f);	
					Right2body [i].transform.position = new Vector3 (Right2body [i].transform.position.x - (dis), Right2body [i].transform.position.y, 0.0f);	

				}

			}
		}



		//L_cylinder.transform.localScale = new Vector3 (radius, dis, radius);
		//L_cylinder.transform.position = new Vector3 (L_up_point.transform.position.x, L_up_point.transform.position.y - dis, L_up_point.transform.position.z);
		//print ("2134");
	}
	public void addpoint(){
		movement.verlist.AddRange (cp2body);
		movement.horlist.AddRange (cloum2body);
	}

}
