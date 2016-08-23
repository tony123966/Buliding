using UnityEngine;
using System.Collections;

public class rooficon : MonoBehaviour {

    public GameObject ori;

    public GameObject meshh;

    public GameObject right;
    public GameObject left;


    public GameObject center;



    public GameObject ting;
    public GameObject ControlPoint1;
    public GameObject ControlPoint2;

	// Use this for initialization
	void Start () 
    {

		center=GameObject.Find("eye");
        right = transform.GetChild(0).GetChild(0).gameObject;

        catline ca= transform.GetChild(0).GetChild(0).gameObject.AddComponent<catline>();

        ca.AddControlPoint(transform.GetChild(0).GetChild(0).GetChild(0).gameObject);
        ca.AddControlPoint(transform.GetChild(0).GetChild(0).GetChild(1).gameObject);
        ca.AddControlPoint(transform.GetChild(0).GetChild(0).GetChild(2).gameObject);

        ca.ResetCatmullRom();
        transform.GetChild(0).GetChild(0).gameObject.AddComponent<circlecut1>().reset();



        Vector3 v1 = transform.GetChild(0).GetChild(0).GetChild(0).transform.position;
        Vector3 v2 = transform.GetChild(0).GetChild(0).GetChild(1).transform.position;
        Vector3 v3 = transform.GetChild(0).GetChild(0).GetChild(2).transform.position;

        GameObject v1c = new GameObject();
        GameObject v2c = new GameObject();
        GameObject v3c = new GameObject();

        v1c.transform.parent = transform.GetChild(0).GetChild(1);
        v2c.transform.parent = transform.GetChild(0).GetChild(1);
        v3c.transform.parent = transform.GetChild(0).GetChild(1);

        v1c.transform.position = v1;
        v2c.transform.position = new Vector3(v2.x - 2 * (v2.x-v1.x), v2.y, v2.z);
        v3c.transform.position = new Vector3(v3.x - 2 * (v3.x - v1.x), v3.y, v3.z);

        left = transform.GetChild(0).GetChild(1).gameObject;

        catline ca2 = transform.GetChild(0).GetChild(1).gameObject.AddComponent<catline>();

        ca2.AddControlPoint(v1c);
        ca2.AddControlPoint(v2c);
        ca2.AddControlPoint(v3c);



        ca2.ResetCatmullRom();
        transform.GetChild(0).GetChild(1).gameObject.AddComponent<circlecut1>().reset();

        mesh();





        ThreePointToBuild();
        
    
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    void rebuild()
    {

        right = transform.GetChild(0).GetChild(0).gameObject;

        catline ca = transform.GetChild(0).GetChild(0).gameObject.AddComponent<catline>();

        ca.AddControlPoint(transform.GetChild(0).GetChild(0).GetChild(0).gameObject);
        ca.AddControlPoint(transform.GetChild(0).GetChild(0).GetChild(1).gameObject);
        ca.AddControlPoint(transform.GetChild(0).GetChild(0).GetChild(2).gameObject);

        ca.ResetCatmullRom();
        transform.GetChild(0).GetChild(0).gameObject.AddComponent<circlecut1>().reset();



        Vector3 v1 = transform.GetChild(0).GetChild(0).GetChild(0).transform.position;
        Vector3 v2 = transform.GetChild(0).GetChild(0).GetChild(1).transform.position;
        Vector3 v3 = transform.GetChild(0).GetChild(0).GetChild(2).transform.position;

        GameObject v1c = new GameObject();
        GameObject v2c = new GameObject();
        GameObject v3c = new GameObject();




        GameObject another = new GameObject();

        another.transform.parent = transform.GetChild(0);

        /*
        v1c.transform.parent = transform.GetChild(0).GetChild(1);
        v2c.transform.parent = transform.GetChild(0).GetChild(1);
        v3c.transform.parent = transform.GetChild(0).GetChild(1);
        */

        v1c.transform.parent = another.transform;
        v2c.transform.parent = another.transform;
        v3c.transform.parent = another.transform;


        v1c.transform.position = v1;
        v2c.transform.position = new Vector3(v2.x - 2 * (v2.x - v1.x), v2.y, v2.z);
        v3c.transform.position = new Vector3(v3.x - 2 * (v3.x - v1.x), v3.y, v3.z);

        left = another.gameObject;

        catline ca2 = transform.GetChild(0).GetChild(1).gameObject.AddComponent<catline>();

        ca2.AddControlPoint(v1c);
        ca2.AddControlPoint(v2c);
        ca2.AddControlPoint(v3c);



        ca2.ResetCatmullRom();
        transform.GetChild(0).GetChild(1).gameObject.AddComponent<circlecut1>().reset();

        mesh();





        ThreePointToBuild();





    }

    


    void mesh()
    {


        int rson = transform.GetChild(0).GetChild(0).GetComponent<circlecut1>().anchorpointlist.Count;



        print("00c:" + transform.GetChild(0).GetChild(0).transform.childCount);
        print("rson:"+rson);


        float uvR = (1 / (float)rson);


        GameObject TTL = new GameObject();


        meshh = TTL;

        TTL.transform.parent = transform;

        TTL.name = ("rooficonmesh");
        TTL.AddComponent<MeshFilter>();
        TTL.AddComponent<MeshRenderer>();



        MeshRenderer floor = TTL.GetComponent<MeshRenderer>();
        MeshRenderer floor1 = ori.GetComponent<MeshRenderer>();
        floor.material = floor1.material;


        Mesh mesh = TTL.GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        Vector3[] v = new Vector3[2 * rson];
        Vector3[] n = new Vector3[2 * rson];
        Vector2[] uv = new Vector2[2 * rson];
        int[] t = new int[6 * rson];

        for (int j = 0; j <= rson-1; j++)
        {
            if (j == 0)
            {


                /*


                Vector3 v1 = transform.GetChild(0).GetChild(0).GetChild(3).transform.position;
                Vector3 v2 = transform.GetChild(0).GetChild(1).GetChild(3).transform.position;
                Vector3 v3 = transform.GetChild(0).GetChild(0).GetChild(4).transform.position;


                */


                /*
                Vector3 v1 = GameObject.Find("roofsurface" + i).transform.GetChild(j).transform.position;
                Vector3 v2 = GameObject.Find("roofsurfaceM" + i).transform.GetChild(j + 1).transform.position;
                Vector3 v3 = GameObject.Find("roofsurface" + i).transform.GetChild(j + 1).transform.position;
                */
                /*
                Vector3 v1 = transform.GetChild(0).GetChild(0).GetChild(j).transform.position;
                Vector3 v2 = transform.GetChild(0).GetChild(1).GetChild(j + 1).transform.position;
                Vector3 v3 = transform.GetChild(0).GetChild(0).GetChild(j + 1).transform.position;
                
                */
                Vector3 v1 = transform.GetChild(0).GetChild(0).GetComponent<circlecut1>().anchorpointlist[j];
                Vector3 v2 = transform.GetChild(0).GetChild(1).GetComponent<circlecut1>().anchorpointlist[j+1];
                Vector3 v3 = transform.GetChild(0).GetChild(0).GetComponent<circlecut1>().anchorpointlist[j+1];
                
                v[0] = (v1);
                v[1] = (v2);
                v[2] = (v3);


                uv[0] = new Vector2(0, 1);
                uv[1] = new Vector2(0, 1 - uvR);
                uv[2] = new Vector2(uvR, 1 - uvR);


                n[0] = -Vector3.forward;
                n[1] = -Vector3.forward;
                n[2] = -Vector3.forward;



                t[0] = j + 2;
                t[1] = j + 1;
                t[2] = j;
            }
            else if (j == 1)
            { }
            else
            {
                Vector3 v3 = transform.GetChild(0).GetChild(1).GetComponent<circlecut1>().anchorpointlist[j];
                Vector3 v4 = transform.GetChild(0).GetChild(0).GetComponent<circlecut1>().anchorpointlist[j];

                v[2 * j - 1] = (v3);
                v[2 * j] = (v4);


                if (j == rson - 1)
                {
                    uv[2 * j - 1] = new Vector2(0, 0);
                    uv[2 * j] = new Vector2(1, 0);
                }
                else
                {
                    uv[2 * j - 1] = new Vector2(0, 1 - j * uvR);

                    uv[2 * j] = new Vector2(j * uvR, 1 - j * uvR);

                }

                //東西南北
                /*
                if ((360 / angle) * i <= 90)
                {
                    n[2 * j - 1] = -Vector3.forward;
                    n[2 * j] = -Vector3.forward;
                }
                if ((360 / angle) * i <= 180 && (360 / angle) * i > 90)
                {
                    n[2 * j - 1] = -Vector3.right;
                    n[2 * j] = -Vector3.right;
                }
                if ((360 / angle) * i <= 270 && (360 / angle) * i > 180)
                {
                    n[2 * j - 1] = -Vector3.back;
                    n[2 * j] = -Vector3.back;
                }
                if ((360 / angle) * i <= 360 && (360 / angle) * i > 270)
                {
                    n[2 * j - 1] = -Vector3.left;
                    n[2 * j] = -Vector3.left;
                }
                */
                //東西南   

                t[2 + 6 * (j - 1) - 5] = 2 * j - 1;
                t[2 + 6 * (j - 1) - 4] = 2 * j - 3;
                t[2 + 6 * (j - 1) - 3] = 2 * j - 2;
                t[2 + 6 * (j - 1) - 2] = 2 * j;
                t[2 + 6 * (j - 1) - 1] = 2 * j - 1;
                t[2 + 6 * (j - 1)] = 2 * j - 2;

            }
            mesh.vertices = v;
            mesh.triangles = t;
            mesh.normals = n;
            mesh.uv = uv;
        }

    }



    public void reset()
    {


        print("yayaya");




        //Destroy(right);
        Destroy(left);
        Destroy(meshh);
        Destroy(ting);
        



        
        transform.GetChild(0).GetChild(0).GetComponent<catline>().ResetCatmullRom();
        //transform.GetChild(0).GetChild(1).gameObject.GetComponent<catline>().ResetCatmullRom();
        
        
        
        transform.GetChild(0).GetChild(0).gameObject.GetComponent<circlecut1>().reset();
       // transform.GetChild(0).GetChild(1).gameObject.GetComponent<circlecut1>().reset();



        rebuild();


        /*
        mesh();
        ThreePointToBuild();
         */

    }





    void ThreePointToBuild()
    {

        Vector3 move = center.transform.position - right.transform.GetChild(0).transform.position;





        Vector3 v1 = right.transform.GetChild(0).transform.position+move;
        Vector3 v2 = right.transform.GetChild(1).transform.position + move;
        Vector3 v3 = right.transform.GetChild(2).transform.position + move;





        GameObject clone = Instantiate(Resources.Load("RidgeCc"), v1, Quaternion.identity) as GameObject;

        ting = clone;
        
        //clone.transform.parent = transform;
        clone.transform.parent = GameObject.Find("build").transform;



        clone.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform.position = v1*70;
		clone.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).transform.position = v2 * 70;
		clone.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).transform.position = v3 * 70;
        /*
        clone.transform.GetChild(0).GetChild(2).GetChild(0).transform.position = new Vector3(v1.x, v1.y + 1f, v1.z);
        clone.transform.GetChild(0).GetChild(2).transform.position = new Vector3(v1.x, v1.y + 1f, v1.z);
        */
        /*
        clone.transform.GetChild(0).GetChild(2).GetChild(0).transform.position = new Vector3(v1.x, v1.y , v1.z);
        clone.transform.GetChild(0).GetChild(2).transform.position = new Vector3(v1.x, v1.y , v1.z);
        */

        clone.transform.GetChild(1).GetComponent<roofcontrol>().setnumberslidervalue();
        clone.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<uppoint>().selffix(clone.transform.GetChild(0).GetChild(2).GetChild(0).gameObject, clone.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RidgeControl>());



        ControlPoint1 = clone.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        ControlPoint2 =  clone.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject;

    }









}
