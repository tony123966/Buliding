using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BC : MonoBehaviour {

    public List<GameObject> BCL = new List<GameObject>();

    public static int ohoh = 7;

    public UIcontrol uict;
    public ColumnControl columncon;

	// Use this for initialization
	void Start () {


     
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ini()
    {
        ohoh = (int)uict.ohoh.value;

        int i = uict.numberslidervalue;

        GameObject bcc = new GameObject();
        bcc.name = ("balustrade1");
        bcc.transform.parent = this.transform;

        for (int j = 0; j < ohoh; j++)
        {
            Vector3 v0 = new Vector3(0, 0, 0);
            GameObject haha = GameObject.Find("balustrade");
            GameObject balustrade = Instantiate(haha, v0, Quaternion.identity) as GameObject;

            
            balustrade.transform.parent = bcc.transform;


            Vector3 v1 = columncon.columnmanage[0].transform.GetChild(2).position;
            Vector3 v2 = columncon.columnmanage[1].transform.GetChild(2).position;
            Vector3 v3 = columncon.columnmanage[0].transform.GetChild(3).position;
            Vector3 v4 = columncon.columnmanage[1].transform.GetChild(3).position;

            Vector3 bigv = v2 - v1;
            Vector3 smallv = (v2 - v1) / ohoh;
            Vector3 mid = (v1 + v3) / 2;



            float yy = Vector3.Distance(v1, v3);
            float zz = smallv.magnitude;

            float xxb = balustrade.transform.GetChild(0).GetComponent<MeshRenderer>().bounds.size.x;
            float yyb = balustrade.transform.GetChild(0).GetComponent<MeshRenderer>().bounds.size.y;
            float zzb = balustrade.transform.GetChild(0).GetComponent<MeshRenderer>().bounds.size.z;


            Vector3 scale = balustrade.transform.GetChild(0).transform.localScale;

            scale.x = 0.15f * scale.x / xxb;
            scale.y = yy * scale.y / yyb;
            scale.z = zz * scale.z / zzb;

            balustrade.transform.GetChild(0).transform.localScale = scale;

            balustrade.transform.position = v1 + (smallv * j);
            balustrade.transform.Rotate(0, (360/i/2)-180 , 0);

            //dodo.transform.GetChild(0).transform.localScale = new Vector3(0.1f, yy, zz);

        }


        BCL.Add(bcc);


    }

    public void build()
    {
        int angle = uict.numberslidervalue;

        for (int i = 2; i <= angle; i++)
        {

            GameObject go = Instantiate(BCL[0], BCL[0].transform.position, Quaternion.identity) as GameObject;

            go.transform.RotateAround(uict.center.transform.position, Vector3.up, (360 / angle) * (i - 1));
            go.name = ("balustrade" + i);
            go.transform.parent = this.transform;


            BCL.Add(go);
        }


    }

    public void reset()
    {
        for (int i = 0; i < BCL.Count; i++)
        {
            Destroy(BCL[i]);
        }
        BCL.Clear();

        ini();
        build();
    }



}
