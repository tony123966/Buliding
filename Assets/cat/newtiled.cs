using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class newtiled : MonoBehaviour
{


    public float tilelong = 3;
    public  float tilelong2 = 1.40f;

    public List<GameObject> tileds = new List<GameObject>();

    public GameObject haha;
    public GameObject haha1;
    public GameObject haha2;




    // Use this for initialization
    void Start()
    {

      haha = GameObject.Find("roundtile-eaveR");
    haha1 = GameObject.Find("roundtileREE");
     haha2 = GameObject.Find("roundtileR");


        creat();
    }

    // Update is called once per frame
    void Update()
    {

    }





    void creat()
    {

        float x = transform.parent.parent.parent.GetChild(1).GetComponent<roofcontrol>().twvalue;
        Vector3 xx = transform.parent.parent.parent.GetChild(0).GetChild(0).GetChild(0).GetComponent<RidgeControl>().ridgemanage[0].transform.GetChild(2).transform.position - transform.parent.parent.parent.GetChild(0).GetChild(0).GetChild(0).GetComponent<RidgeControl>().ridgemanage[1].transform.GetChild(2).transform.position;


        tilelong = Vector3.Magnitude(xx / (x  ));




        //print(transform.parent.parent.parent.GetChild(1).name);
        int ui = transform.parent.parent.parent.GetChild(1).GetComponent<roofcontrol>().numberslidervalue;
        roofsurcontrol2 r2 = transform.parent.parent.GetComponent<roofsurcontrol2>();

        circlecut1 cir = transform.GetComponent<circlecut1>();
        newplanecut pla = transform.GetComponent<newplanecut>();

       // cir.anchorpointlist

        GameObject neew = new GameObject();
        neew.name = (this.name + "-tile");
        neew.transform.parent = this.transform;

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
            if (i == 0)
            {
                //GameObject haha = GameObject.Find("roundtile-eaveR");
                GameObject tile = Instantiate(haha, (ori + letter) / 2, Quaternion.identity) as GameObject;
               
                tile.transform.parent = neew.transform;
                int angle = r2.roofsurface2manage.IndexOf(this.transform.parent.gameObject);

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
                 */
                tile.transform.Rotate(oo, (90 + (360 / ui) / 2) + (360 / ui) * (j ), -zz);

                tile.transform.localScale = new Vector3(tilelong, 2, tilelong2);
                tileds.Add(tile);

}

            else if (i == pla.anchorpointlist.Count-3)
            {
                //GameObject haha1 = GameObject.Find("roundtileREE");
                GameObject tile = Instantiate(haha1, (ori + letter) / 2, Quaternion.identity) as GameObject;
                
                tile.transform.parent = neew.transform;
                int angle = r2.roofsurface2manage.IndexOf(this.transform.parent.gameObject);


                //int x = int.Parse(neew.transform.name.Substring(16, 1));
                int y = i;

                /*
                Vector3 zr = plane.sloslopR[x + 1, y] - plane.sloslopR[x, y];
                Vector3 zrp = new Vector3(plane.sloslopR[x + 1, y].x - plane.sloslopR[x, y].x, 0, plane.sloslopR[x + 1, y].z - plane.sloslopR[x, y].z);
                zz = Vector3.Angle(zr, zrp);

    */



                int j = angle;
                tile.transform.GetChild(2).transform.Rotate(-6, 0, 0);
                tile.transform.Rotate(oo, (90 + (360 / ui) / 2) + (360 / ui) * (j ), -zz);

                tile.transform.localScale = new Vector3(tilelong, 2, tilelong2);

                tileds.Add(tile);
            }

            else
            {
                //GameObject haha2 = GameObject.Find("roundtileR");
                GameObject tile = Instantiate(haha2, (ori + letter) / 2, Quaternion.identity) as GameObject;
               
                tile.transform.parent = neew.transform;
                int angle = r2.roofsurface2manage.IndexOf(this.transform.parent.gameObject);


                //int x = int.Parse(neew.transform.name.Substring(16, 1));
                int y = i;

                /*
                Vector3 zr = plane.sloslopR[x + 1, y] - plane.sloslopR[x, y];
                Vector3 zrp = new Vector3(plane.sloslopR[x + 1, y].x - plane.sloslopR[x, y].x, 0, plane.sloslopR[x + 1, y].z - plane.sloslopR[x, y].z);
                zz = Vector3.Angle(zr, zrp);

    */



                int j = angle;
                tile.transform.GetChild(2).transform.Rotate(-6, 0, 0);
                tile.transform.Rotate(oo, (90 + (360 / ui) / 2) + (360 / ui) * (j), -zz);

                tile.transform.localScale = new Vector3(tilelong, 2, tilelong2);
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

        /*
        if (GameObject.Find(this.name + "-tile"))
        {
            GameObject trash = (GameObject.Find(this.name + "-tile"));
            Destroy(trash);
        }

         */ 
        tileds.Clear();
        creat();

    }


}
