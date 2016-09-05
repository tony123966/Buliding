using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RidgeControl : MonoBehaviour {

    public List<GameObject> ridgemanage = new List<GameObject>();

    public roofcontrol uict;
    
    public EaveControl eavc;
    public roofsurcontrol rfcon;
    public roofsurcon2control rfcon2;
    public roofsurcontrol2 rfconS;


   


    // Use this for initialization



    void Start () 
    {
        /*
        build();
        eavc.ini();
        eavc.build();
        rfcon.ini();
        rfcon.build();
        rfcon2.ini();
        rfcon2.build();
        rfconS.ini();
        rfconS.build();
       */

        /*
        GameObject.Find("bao-ding").transform.position =this.transform.GetChild(0).transform.position;
         */
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    /*
    public void towerbuild()
    {
        int angle = uict.numberslidervalue;





        for (int i =1; i <= angle; i++)
        {
            GameObject go = Instantiate(ridgemanage[0], ridgemanage[0].transform.position, Quaternion.identity) as GameObject;
            go.transform.Translate(0, 10, 0);


            Destroy(go.GetComponent<RidgeControl>());

            Destroy(go.transform.GetChild(0).GetComponent<mouseevent>());
            Destroy(go.transform.GetChild(0).GetComponent<topmove>());
            Destroy(go.transform.GetChild(0).GetComponent<MeshRenderer>());
            Destroy(go.transform.GetChild(0).GetComponent<SphereCollider>());

            Destroy(go.transform.GetChild(1).GetComponent<fourmove>());
            Destroy(go.transform.GetChild(1).GetComponent<mouseevent>());
            Destroy(go.transform.GetChild(1).GetComponent<MeshRenderer>());
            Destroy(go.transform.GetChild(1).GetComponent<SphereCollider>());

            Destroy(go.transform.GetChild(2).GetComponent<MeshRenderer>());
            Destroy(go.transform.GetChild(2).GetComponent<SphereCollider>());


            go.transform.RotateAround(uict.center.transform.position, Vector3.up, (360 / angle) * (i - 1));
            go.name = ("Ridge" + i);

            //go.AddComponent<catline>();


            go.GetComponent<catline>().ResetCatmullRom();


            //this.GetComponent<circlecut1>().reset();
            //go.GetComponent<circlecut1>().reset();


            //go.GetComponent<Ridgetile>().reset();
            //go.GetComponent<catline>().AddControlPoint(go.transform.GetChild(0).gameObject);
            //go.GetComponent<catline>().AddControlPoint(go.transform.GetChild(1).gameObject);
            //go.GetComponent<catline>().AddControlPoint(go.transform.GetChild(2).gameObject);

            go.transform.parent = this.gameObject.transform.parent;
            ridgemanage.Add(go);

        }


        for (int i = 0; i < ridgemanage.Count; i++)
        {


            // ridgemanage[i].GetComponent<circlecut1>().cutpoint();
            //ridgemanage[i].GetComponent<Ridgetile>().creat();

            ridgemanage[i].GetComponent<circlecut1>().reset();
            ridgemanage[i].GetComponent<Ridgetile>().reset();

        }




    }
    */



    public void build()
    {


        ridgemanage.Add(this.gameObject);


        
        ///print(uict.name);
        ///print(uict.transform.parent.name);
        ///print(uict.transform.parent.parent.name);
        int angle = uict.numberslidervalue;








            for (int i = 2; i <= angle; i++)
            {
                GameObject go = Instantiate(ridgemanage[0], ridgemanage[0].transform.position, Quaternion.identity) as GameObject;

                Destroy(go.GetComponent<RidgeControl>());

                Destroy(go.transform.GetChild(0).GetComponent<mouseevent>());
                Destroy(go.transform.GetChild(0).GetComponent<topmove>());
                Destroy(go.transform.GetChild(0).GetComponent<MeshRenderer>());
                Destroy(go.transform.GetChild(0).GetComponent<SphereCollider>());

                Destroy(go.transform.GetChild(1).GetComponent<fourmove>());
                Destroy(go.transform.GetChild(1).GetComponent<mouseevent>());
                Destroy(go.transform.GetChild(1).GetComponent<MeshRenderer>());
                Destroy(go.transform.GetChild(1).GetComponent<SphereCollider>());

                Destroy(go.transform.GetChild(2).GetComponent<MeshRenderer>());
                Destroy(go.transform.GetChild(2).GetComponent<SphereCollider>());


                go.transform.RotateAround(uict.center.transform.position, Vector3.up, (360 / angle) * (i - 1));
                go.name = ("Ridge" + i);

                //go.AddComponent<catline>();


                go.GetComponent<catline>().ResetCatmullRom();


                //this.GetComponent<circlecut1>().reset();
                //go.GetComponent<circlecut1>().reset();


                //go.GetComponent<Ridgetile>().reset();
                //go.GetComponent<catline>().AddControlPoint(go.transform.GetChild(0).gameObject);
                //go.GetComponent<catline>().AddControlPoint(go.transform.GetChild(1).gameObject);
                //go.GetComponent<catline>().AddControlPoint(go.transform.GetChild(2).gameObject);

                go.transform.parent = this.gameObject.transform.parent;
                ridgemanage.Add(go);

            }




            for (int i = 0; i < ridgemanage.Count; i++)
            {


                // ridgemanage[i].GetComponent<circlecut1>().cutpoint();
                //ridgemanage[i].GetComponent<Ridgetile>().creat();

                ridgemanage[i].GetComponent<circlecut1>().reset();
                ridgemanage[i].GetComponent<Ridgetile>().reset();

            }
        
       

        /*
        GameObject.Find("bao-ding").transform.position = this.transform.GetChild(0).transform.position;
        */

    }

     public void reset()
    {

        //this.GetComponent<circlecut1>().kill();
        //this.GetComponent<Ridgetile>().kill();



        for (int i = 1; i < ridgemanage.Count; i++)
        {
        
            Destroy(ridgemanage[i]);
        }
       

        ridgemanage.Clear();


         
         
         build();
    
     
     
     
     
     
     
     }




}
