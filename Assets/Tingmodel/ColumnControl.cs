using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColumnControl : MonoBehaviour
{

	public List<GameObject> columnmanage = new List<GameObject>();
	public UIcontrol uict;

	// Use this for initialization
	public FC fc;
	public BC bc;
	public PlatForm pf;

	public Vector3 top;
	public Vector3 down;


	public float EyeToColumn;

    public float ColumnLong;



	void Start()
	{
		EyeToColumn = Mathf.Abs(transform.GetChild(3).transform.position.x - uict.center.transform.position.x);

        ColumnLong = Mathf.Abs(transform.GetChild(0).transform.position.y - transform.GetChild(3).transform.position.y);
		/*
		build();
		fc.ini();
		fc.build();
		bc.ini();
		bc.build();
		pf.build();
		*/


	}

	// Update is called once per frame
	void Update()
	{

	}


	public void build()
	{
		columnmanage.Add(this.gameObject);

		int angle = uict.numberslidervalue;


		for (int i = 2; i <= angle; i++)
		{
			GameObject go = Instantiate(this.gameObject, this.transform.position, Quaternion.identity) as GameObject;

			Destroy(go.GetComponent<ColumnControl>());

			Destroy(go.transform.GetChild(0).GetComponent<MeshRenderer>());
			Destroy(go.transform.GetChild(0).GetComponent<SphereCollider>());
			Destroy(go.transform.GetChild(1).GetComponent<MeshRenderer>());
			Destroy(go.transform.GetChild(1).GetComponent<SphereCollider>());
			Destroy(go.transform.GetChild(2).GetComponent<MeshRenderer>());
			Destroy(go.transform.GetChild(2).GetComponent<SphereCollider>());
			Destroy(go.transform.GetChild(3).GetComponent<MeshRenderer>());
			Destroy(go.transform.GetChild(3).GetComponent<SphereCollider>());



			go.transform.RotateAround(uict.center.transform.position, Vector3.up, (360 / angle) * (i - 1));
			go.name = ("Column" + i);

			go.AddComponent<catline>();
			go.GetComponent<catline>().ResetCatmullRom();
			go.transform.parent = this.transform.parent;

			columnmanage.Add(go);

		}


		for (int i = 0; i < columnmanage.Count; i++)
		{


			// ridgemanage[i].GetComponent<circlecut1>().cutpoint();
			//ridgemanage[i].GetComponent<Ridgetile>().creat();


			if (columnmanage[i].GetComponent<circlecut1>())
			{
				columnmanage[i].GetComponent<circlecut1>().reset();
				columnmanage[i].GetComponent<Columntile>().reset();
			}

		}



	}

	public void reset()
	{
		for (int i = 1; i < columnmanage.Count; i++)
		{
			Destroy(columnmanage[i]);
		}
		columnmanage.Clear();

		build();
	}


}
