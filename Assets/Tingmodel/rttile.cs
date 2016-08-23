﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class rttile : MonoBehaviour {


    public float tilelong = 0.03f;
    public float tilelong2 = 0.0140f;

    public List<GameObject> tileds = new List<GameObject>();




    // Use this for initialization
    void Start()
    {
        //creat();
    }

    // Update is called once per frame
    void Update()
    {

    }





    public void creat()
    {

        int ui = transform.parent.parent.parent.GetChild(1).GetComponent<roofcontrol>().numberslidervalue;
        RidgetailControl r2 = this.transform.parent.GetChild(0).GetComponent<RidgetailControl>();

        circlecut1 pla = transform.GetComponent<circlecut1>();
        //midplanecut pla = transform.GetComponent<midplanecut>();

        // cir.anchorpointlist



        int c1 = pla.anchorpointlist.Count / 9;
       



        for (int i = 0; i < pla.anchorpointlist.Count - 2; i++)
        {
            Vector3 ori = pla.anchorpointlist[i];
            Vector3 letter = pla.anchorpointlist[i + 1];
            Vector3 bottomline = new Vector3(ori.x - letter.x, 0, ori.z - letter.z);
            Vector3 slopeline = ori - letter;

            //Vector3.AngleBetween(bottomline, slopeline);
            float oo = Vector3.Angle(bottomline, slopeline);
            if (letter.y - ori.y < 0)
            {
                oo = -oo;
            }


            float zz = 0f;
            if (i < c1*2)
            {
                GameObject haha = GameObject.Find("main_ridge");
                GameObject tile = Instantiate(haha, (ori + letter) / 2, Quaternion.identity) as GameObject;

                tile.transform.parent = this.transform;
                int angle = r2.ridgetailmanage.IndexOf(this.transform.gameObject);

                //int x = int.Parse(neew.transform.name.Substring(16, 1));
                int y = i;

                /*
                Vector3 zr = plane.sloslopR[x + 1, y] - plane.sloslopR[x, y];
                Vector3 zrp = new Vector3(plane.sloslopR[x + 1, y].x - plane.sloslopR[x, y].x, 0, plane.sloslopR[x + 1, y].z - plane.sloslopR[x, y].z);
                zz = Vector3.Angle(zr, zrp);
                */


                int j = angle;
                /*
                tile.transform.GetChild(2).transform.Rotate(-6, 0, 0);
                tile.transform.GetChild(4).transform.Rotate(-6, 0, 0);
                 * */
                tile.transform.Rotate(oo, -90 + (360 / ui) * (j), 0);

                tile.transform.localScale = new Vector3(tilelong, 0.02f, tilelong2);
                tileds.Add(tile);

            }

            else if (i >=c1*5)
            {
                GameObject haha = GameObject.Find("main_ridge3");
                GameObject tile = Instantiate(haha, (ori + letter) / 2, Quaternion.identity) as GameObject;
                tile.transform.parent = this.transform;
                int angle = r2.ridgetailmanage.IndexOf(this.transform.gameObject);


                //int x = int.Parse(neew.transform.name.Substring(16, 1));
                int y = i;

                /*
                Vector3 zr = plane.sloslopR[x + 1, y] - plane.sloslopR[x, y];
                Vector3 zrp = new Vector3(plane.sloslopR[x + 1, y].x - plane.sloslopR[x, y].x, 0, plane.sloslopR[x + 1, y].z - plane.sloslopR[x, y].z);
                zz = Vector3.Angle(zr, zrp);

    */



                int j = angle;

                tile.transform.Rotate(oo, -90 + (360 / ui) * (j), 0);

                tile.transform.localScale = new Vector3(tilelong, 0.02f, tilelong2);

                tileds.Add(tile);
            }

            else if (i>=c1*2)
            {
                GameObject haha = GameObject.Find("main_ridge2");
                GameObject tile = Instantiate(haha, (ori + letter) / 2, Quaternion.identity) as GameObject;
                tile.transform.parent = this.transform;
                int angle = r2.ridgetailmanage.IndexOf(this.transform.gameObject);


                //int x = int.Parse(neew.transform.name.Substring(16, 1));
                int y = i;

                /*
                Vector3 zr = plane.sloslopR[x + 1, y] - plane.sloslopR[x, y];
                Vector3 zrp = new Vector3(plane.sloslopR[x + 1, y].x - plane.sloslopR[x, y].x, 0, plane.sloslopR[x + 1, y].z - plane.sloslopR[x, y].z);
                zz = Vector3.Angle(zr, zrp);

    */



                int j = angle;

                tile.transform.Rotate(oo, -90 + (360 / ui) * (j), 0);

                tile.transform.localScale = new Vector3(tilelong, 0.02f, tilelong2);
                tileds.Add(tile);
            }

        }
    }

    public void reset()
    {


        for (int i = 0; i < tileds.Count; i++)
        {
            Destroy(tileds[i]);

        }


        tileds.Clear();
        creat();

    }


    public void kill()
    {


        for (int i = 0; i < tileds.Count; i++)
        {
            Destroy(tileds[i]);

        }


        tileds.Clear();


    }
}
