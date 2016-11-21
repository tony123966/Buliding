using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AllInOne : MonoBehaviour
{


    


    

    public roofcontrol ucit;
	public GameObject center;

	public GameObject roof;
	public GameObject body;
	public GameObject platform;




	public List<GameObject> TingPartList = new List<GameObject>();
	public List<Vector3> TingPartPosition = new List<Vector3>();

    //tower

    public List<GameObject> tower = new List<GameObject>();
    public List<GameObject> tower_roof = new List<GameObject>();
    //public List<GameOB> tower_roof = new List<GameOB>();

	public int number;

	//column's percent
	public float percent;
	public float platpercent;
    public float percent2;

    public float columntall;


    public float Roof_Height_Percent;
    public float Roof_Wide_Percent;



    public float roof_tall;
    public float roof_long;

    public float ini_roof_tall;
    public float ini_roof_long;


    public bool Rectangle_Or_Not;


    public int inUseTab;



    void Awake()
    {
        Application.targetFrameRate = 30;

        inUseTab=0;
    }


	// Use this for initialization
	void Start()
	{

        tower_roof.Add(null);

		TingPartList.Add(roof);

		TingPartList.Add(body);
        tower.Add(body);
        //tower_roof.Add(roof);

		TingPartList.Add(platform);



		TingPartPosition.Add(roof.transform.position);
		TingPartPosition.Add(body.transform.position);
		TingPartPosition.Add(platform.transform.position);

		percent = 1 / body.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<ColumnControl>().EyeToColumn;

        percent2 = 1 / body.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<ColumnControl>().EyeToColumn;
        
        platpercent = platform.transform.GetChild(2).GetComponent<PlatForm>().percent;
        columntall = body.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<ColumnControl>().ColumnLong;


        Roof_Height_Percent = roof.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RidgeControl>().Height;
        Roof_Wide_Percent = roof.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RidgeControl>().Wide;

        Rectangle_Or_Not = false;

        GameObject Ridge_Nick = TingPartList[0].transform.GetChild(0).GetChild(0).gameObject;
        
        ini_roof_tall = Vector3.Distance(Ridge_Nick.transform.GetChild(0).GetChild(2).transform.position, Ridge_Nick.transform.GetChild(3).GetChild(2).transform.position)/2f;
        ini_roof_long = Vector3.Distance(Ridge_Nick.transform.GetChild(0).GetChild(2).transform.position, Ridge_Nick.transform.GetChild(1).GetChild(2).transform.position)/2f;

        GameObject.Find("bao-ding").transform.position = TingPartList[0].transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform.position;


	}

	// Update is called once per frame
	void Update()
	{

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{

            ChangeRoof(0, 0.5F, 0.5F);
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{

            ChangeRoof(2, 2, 0.7F);
		}

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{

            eaveupdate();
            rectangle_column();
            rectangle_Platform(roof_tall,roof_long);

		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{

			UpdateAll(4);
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
            //twolayer();
            Shanding(2f);

		}
		if (Input.GetKeyDown(KeyCode.Alpha6))
		{

			//UpdateAll(6);

            eaveupdate2();
		}

		if (Input.GetKeyDown(KeyCode.Alpha7))
		{

            upup(17.5f);
		}

		if (Input.GetKeyDown(KeyCode.Alpha8))
		{

			UpdateAll(8);
		}

		if (Input.GetKeyDown(KeyCode.Alpha9))
		{

			UpdateAll(9);
		}
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{

            Double_Eave();
		}

        


    }







    public void Double_Eave()
    {


        GameObject cc = TingPartList[1];

        Vector3 v1 = new Vector3(cc.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).transform.position.x, cc.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).transform.position.y - 15, cc.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).transform.position.z);


        GameObject clone2 = Instantiate(Resources.Load("RidgeCctwolayer"), v1, Quaternion.identity) as GameObject;


        clone2.transform.parent = transform;
        clone2.transform.GetChild(1).GetComponent<roofcontrol>().numberslider.value = ucit.numberslidervalue;

        tower_roof[inUseTab] = clone2;
     

    }



    public void Bodytruefalse(bool a)
    {

        TingPartList[1].transform.GetChild(1).GetComponent<UIcontrol>().isCb = a;

        TingPartList[1].transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<ColumnControl>().reset();
        TingPartList[1].transform.GetChild(7).GetComponent<ColumnBody>().reset();
    }











    //rectangle system


    //寶塔 *****************
    public void upup(float a)
    {
        columntall = tower[tower.Count-1].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<ColumnControl>().ColumnLong;
        a = columntall;


        TingPartList[0].transform.Translate(0,a,0);
       
        //GameObject clone = Instantiate(TingPartList[1], TingPartList[1].transform.position, Quaternion.identity) as GameObject;

        GameObject clone = Instantiate(tower[tower.Count - 1], tower[tower.Count - 1].transform.position, Quaternion.identity) as GameObject;
        
        clone.transform.parent = transform;
        clone.transform.Translate(0, a, 0);






        MoveTowerBody(clone);

        //clone.transform.GetChild(2).GetComponent<UIcontrol>().numberslidervalue = ucit.numberslidervalue;
        
        

        clone.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<catline>().ResetCatmullRom();
        clone.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<circlecut1>().reset();
        clone.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Columntile>().reset();


        clone.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<catline>().ResetCatmullRom();
        clone.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<circlecut1>().reset();
        clone.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Columntile>().reset();

        clone.transform.GetChild(1).GetComponent<UIcontrol>().setnumberslidervalue();
        /*
        GameObject cc = TingPartList[1];

        Vector3 v1 = new Vector3(cc.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).transform.position.x, cc.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).transform.position.y-15,cc.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).transform.position.z);



        
        GameObject clone2 = Instantiate(Resources.Load("RidgeCctwolayer"), v1, Quaternion.identity) as GameObject;
        
       
        clone2.transform.parent = transform;
        clone2.transform.GetChild(1).GetComponent<roofcontrol>().numberslider.value = ucit.numberslidervalue;
        */

       
        GameObject.Find("bao-ding").transform.position = TingPartList[0].transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform.position;
        
        // TingPartList[0].transform.GetChild(1).GetComponent<roofcontrol>().setnumberslidervalue();


        TingPartList[1] = clone;

        tower.Add(clone);
        tower_roof.Add(null);


        if (TingPartList[1].transform.GetChild(1).GetComponent<UIcontrol>().isCb == true)
        {
            clone.transform.GetChild(7).GetComponent<ColumnBody>().reset();
        }


        

    }



    public void changeLayer(int a)
    {
        TingPartList[1] = tower[a];
        inUseTab=a;


        columntall = TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<ColumnControl>().ColumnLong;

    }


    public void DeleteLayer(int a)
    {

        print("index: "+a);

        TingPartList[1] = tower[a];
        inUseTab = a;


        columntall = tower[a].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<ColumnControl>().ColumnLong;



        TingPartList[0].transform.Translate(0, -columntall, 0);



        for (int i = 0; i < tower.Count; i++)
        {
            if (TingPartList[1].name == tower[i].name)
            {

                for (int j = i + 1; j < tower.Count; j++)
                {
                    tower[j].transform.Translate(0, -columntall , 0);
                    if (tower_roof[j - 1] != null)
                        tower_roof[j - 1].transform.Translate(0, -columntall, 0);
                }


                TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).transform.Translate(0, (columntall) * a, 0);
                TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).transform.Translate(0, (columntall) * a, 0);

                TingPartList[1].transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).transform.Translate(0, (columntall) * a, 0);
                TingPartList[1].transform.GetChild(0).GetChild(2).GetChild(0).GetChild(1).transform.Translate(0, (columntall) * a, 0);



                break;
            }

        }





        for (int i = 0; i < tower.Count; i++)
        {
            if (TingPartList[1].name == tower[i].name)
            {

                for (int j = i + 1; j < tower.Count; j++)
                {
                    tower[j - 1] = tower[j];
                    if (tower_roof[j - 1] != null)
                        tower_roof[j - 2] = tower_roof[j - 1];
                }

                break;
            }

        }



        

        tower.Remove(tower[a]);

        //tower_roof.Remove(tower_roof[a-1]);


        Destroy(TingPartList[1].gameObject);
        GameObject.Find("bao-ding").transform.position = TingPartList[0].transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform.position;
        if(a!=0)
        { 

        TingPartList[1] = tower[a-1];
        inUseTab = a-1;
         }
        else
        {
            TingPartList[1] = tower[a];
            inUseTab = a;


        }
        //changeLayer(inUseTab);
    
    }












    public void MoveTowerBody(GameObject a)
    {


        
        a.transform.GetChild(0).GetChild(1).GetChild(0).transform.Translate(0, 0, 0);

        a.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<ColumnControl>().columnmanage[0].GetComponent<catline>().ResetCatmullRom();
        a.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<ColumnControl>().reset();
        

    }

    //**** ****************





    //歇山頂偷吃步



    public void Shanding(float a)
    {
        GameObject Ridge_Nick = TingPartList[0].transform.GetChild(0).GetChild(0).gameObject;


        Vector3 P1 = (Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[0].transform.GetChild(0).transform.position + Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[2].transform.GetChild(2).transform.position)/2;
        Vector3 P2 = (Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[0].transform.GetChild(1).transform.position + Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[2].transform.GetChild(1).transform.position) / 2;
        Vector3 P3 = (Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[0].transform.GetChild(2).transform.position + Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[2].transform.GetChild(0).transform.position) / 2;

        P1 = new Vector3(P1.x, P1.y+a, P1.z);
        P2 = new Vector3(P2.x, P2.y + a, P2.z);
        P3 = new Vector3(P3.x, P3.y + a, P3.z);


        Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[0].transform.GetChild(0).transform.position = P1;
        Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[2].transform.GetChild(2).transform.position = P1;

        Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[0].transform.GetChild(1).transform.position = P2;
        Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[2].transform.GetChild(1).transform.position = P2;

        Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[0].transform.GetChild(2).transform.position = P3;
        Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[2].transform.GetChild(0).transform.position = P3;



    
    }


    //長方形屋頂


    public void ChangeRoof(int n, float a, float b)
    {

       
       


        GameObject Ridge_Nick = TingPartList[0].transform.GetChild(0).GetChild(0).gameObject;



        if (n == 0)
        {
            /*
            Ridge_Nick.transform.GetChild(0).GetChild(n).transform.Translate(a, 0, a);
            Ridge_Nick.transform.GetChild(1).GetChild(n).transform.Translate(a, 0, -a);
            Ridge_Nick.transform.GetChild(2).GetChild(n).transform.Translate(a, 0, a);
            Ridge_Nick.transform.GetChild(3).GetChild(n).transform.Translate(a, 0, -a);
            */
            print(a+"   +    "+b);
            
            Ridge_Nick.transform.GetChild(0).GetChild(n).transform.Translate(a, 0, b);
            Ridge_Nick.transform.GetChild(1).GetChild(n).transform.Translate(-b, 0, -a);
            Ridge_Nick.transform.GetChild(2).GetChild(n).transform.Translate(-a, 0, -b);
            Ridge_Nick.transform.GetChild(3).GetChild(n).transform.Translate(b, 0, a);
            



            
            //特別
            Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[0].transform.GetChild(0).transform.Translate(a, 0, b);
            Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[0].transform.GetChild(2).transform.Translate(-b, 0, -a);

            
            Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[1].transform.GetChild(0).transform.Translate(a, 0, -b);
            //Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[1].transform.GetChild(2).transform.Translate(b, 0, -a);


            Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[2].transform.GetChild(0).transform.Translate(a, 0, b);
            Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[2].transform.GetChild(2).transform.Translate(-b, 0, -a);

            Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[3].transform.GetChild(0).transform.Translate(a, 0, -b);
           // Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[3].transform.GetChild(2).transform.Translate(b, 0, -a);
           







           
        }
        else if (n == 2)
        {
            Ridge_Nick.transform.GetChild(0).GetChild(n).transform.Translate(a, 0, b);
            Ridge_Nick.transform.GetChild(1).GetChild(n).transform.Translate(-b, 0, -a);
            Ridge_Nick.transform.GetChild(2).GetChild(n).transform.Translate(-a, 0, -b);
            Ridge_Nick.transform.GetChild(3).GetChild(n).transform.Translate(b, 0, a);
        }


        Ridge_Nick.transform.GetChild(0).GetChild(1).transform.position = con2posiotion(Ridge_Nick.transform.GetChild(0).GetChild(0).transform.position, Ridge_Nick.transform.GetChild(0).GetChild(2).transform.position);
        Ridge_Nick.transform.GetChild(1).GetChild(1).transform.position = con2posiotion(Ridge_Nick.transform.GetChild(1).GetChild(0).transform.position, Ridge_Nick.transform.GetChild(1).GetChild(2).transform.position);
        Ridge_Nick.transform.GetChild(2).GetChild(1).transform.position = con2posiotion(Ridge_Nick.transform.GetChild(2).GetChild(0).transform.position, Ridge_Nick.transform.GetChild(2).GetChild(2).transform.position);
        Ridge_Nick.transform.GetChild(3).GetChild(1).transform.position = con2posiotion(Ridge_Nick.transform.GetChild(3).GetChild(0).transform.position, Ridge_Nick.transform.GetChild(3).GetChild(2).transform.position);

        //Ridge重整
        Ridge_Nick.transform.GetChild(0).GetComponent<catline>().ResetCatmullRom();
        Ridge_Nick.transform.GetChild(1).GetComponent<catline>().ResetCatmullRom();
        Ridge_Nick.transform.GetChild(2).GetComponent<catline>().ResetCatmullRom();
        Ridge_Nick.transform.GetChild(3).GetComponent<catline>().ResetCatmullRom();

        Ridge_Nick.transform.GetChild(0).GetComponent<circlecut1>().reset();
        Ridge_Nick.transform.GetChild(1).GetComponent<circlecut1>().reset();
        Ridge_Nick.transform.GetChild(2).GetComponent<circlecut1>().reset();
        Ridge_Nick.transform.GetChild(3).GetComponent<circlecut1>().reset();

        Ridge_Nick.transform.GetChild(0).GetComponent<Ridgetile>().reset();
        Ridge_Nick.transform.GetChild(1).GetComponent<Ridgetile>().reset();
        Ridge_Nick.transform.GetChild(2).GetComponent<Ridgetile>().reset();
        Ridge_Nick.transform.GetChild(3).GetComponent<Ridgetile>().reset();

        


        //eaveupdate();



        //TingPartList[0].transform.GetChild(1).GetComponent<roofcontrol>().setnumberslidervalue();

        roof_tall = Vector3.Distance(Ridge_Nick.transform.GetChild(0).GetChild(2).transform.position, Ridge_Nick.transform.GetChild(3).GetChild(2).transform.position);
        roof_long = Vector3.Distance(Ridge_Nick.transform.GetChild(0).GetChild(2).transform.position, Ridge_Nick.transform.GetChild(1).GetChild(2).transform.position);





    }

    public void eaveupdate()
    {
        GameObject Ridge_Nick = TingPartList[0].transform.GetChild(0).GetChild(0).gameObject;

      

        //others

        
        upridge ups = Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>();




        ups.reset();

        Ridge_Nick.transform.parent.parent.GetChild(5).GetComponent<EaveControl>().reset();
        Ridge_Nick.transform.parent.parent.GetChild(2).GetComponent<roofsurcontrol>().reset();
        Ridge_Nick.transform.parent.parent.GetChild(3).GetComponent<roofsurcon2control>().reset();
        Ridge_Nick.transform.parent.parent.GetChild(4).GetComponent<roofsurcontrol2>().reset();

        /*
        
        ups.inig1(ups.transform.GetChild(1).transform.gameObject);
        ups.inig2(ups.transform.GetChild(3).transform.gameObject);

        ups.inig3(ups.transform.GetChild(2).transform.gameObject);
        ups.inig4(ups.transform.GetChild(0).transform.gameObject);

        



        //eave
        Ridge_Nick.transform.parent.parent.GetChild(5).GetComponent<EaveControl>().inig(Ridge_Nick.transform.parent.parent.GetChild(5).GetComponent<EaveControl>().eavemanage[0]);
        Ridge_Nick.transform.parent.parent.GetChild(5).GetComponent<EaveControl>().inig(Ridge_Nick.transform.parent.parent.GetChild(5).GetComponent<EaveControl>().eavemanage[2]);
        Ridge_Nick.transform.parent.parent.GetChild(5).GetComponent<EaveControl>().inig(Ridge_Nick.transform.parent.parent.GetChild(5).GetComponent<EaveControl>().eavemanage[1]);
        Ridge_Nick.transform.parent.parent.GetChild(5).GetComponent<EaveControl>().inig(Ridge_Nick.transform.parent.parent.GetChild(5).GetComponent<EaveControl>().eavemanage[3]);

        //rf
        Ridge_Nick.transform.parent.parent.GetChild(2).GetComponent<roofsurcontrol>().inig(Ridge_Nick.transform.parent.parent.GetChild(2).GetComponent<roofsurcontrol>().roofsurfacemanage[0]);
        Ridge_Nick.transform.parent.parent.GetChild(2).GetComponent<roofsurcontrol>().inig(Ridge_Nick.transform.parent.parent.GetChild(2).GetComponent<roofsurcontrol>().roofsurfacemanage[2]);
        Ridge_Nick.transform.parent.parent.GetChild(2).GetComponent<roofsurcontrol>().inig(Ridge_Nick.transform.parent.parent.GetChild(2).GetComponent<roofsurcontrol>().roofsurfacemanage[1]);
        Ridge_Nick.transform.parent.parent.GetChild(2).GetComponent<roofsurcontrol>().inig(Ridge_Nick.transform.parent.parent.GetChild(2).GetComponent<roofsurcontrol>().roofsurfacemanage[3]);

        //RRR
        Ridge_Nick.transform.parent.parent.GetChild(3).GetComponent<roofsurcon2control>().inig(Ridge_Nick.transform.parent.parent.GetChild(3).GetComponent<roofsurcon2control>().rrrRL[0]);
        Ridge_Nick.transform.parent.parent.GetChild(3).GetComponent<roofsurcon2control>().inig(Ridge_Nick.transform.parent.parent.GetChild(3).GetComponent<roofsurcon2control>().rrrRL[2]);
        Ridge_Nick.transform.parent.parent.GetChild(3).GetComponent<roofsurcon2control>().inig(Ridge_Nick.transform.parent.parent.GetChild(3).GetComponent<roofsurcon2control>().rrrRL[1]);
        Ridge_Nick.transform.parent.parent.GetChild(3).GetComponent<roofsurcon2control>().inig(Ridge_Nick.transform.parent.parent.GetChild(3).GetComponent<roofsurcon2control>().rrrRL[3]);
        //rf2


        Ridge_Nick.transform.parent.parent.GetChild(4).GetComponent<roofsurcontrol2>().inig(Ridge_Nick.transform.parent.parent.GetChild(4).GetComponent<roofsurcontrol2>().roofsurface2manage[0]);
        Ridge_Nick.transform.parent.parent.GetChild(4).GetComponent<roofsurcontrol2>().inig(Ridge_Nick.transform.parent.parent.GetChild(4).GetComponent<roofsurcontrol2>().roofsurface2manage[2]);
        Ridge_Nick.transform.parent.parent.GetChild(4).GetComponent<roofsurcontrol2>().inig(Ridge_Nick.transform.parent.parent.GetChild(4).GetComponent<roofsurcontrol2>().roofsurface2manage[1]);
        Ridge_Nick.transform.parent.parent.GetChild(4).GetComponent<roofsurcontrol2>().inig(Ridge_Nick.transform.parent.parent.GetChild(4).GetComponent<roofsurcontrol2>().roofsurface2manage[3]);


        */



    }




    public void eaveupdate2()
    {

        Rectangle_Or_Not = true;

        GameObject Ridge_Nick = TingPartList[0].transform.GetChild(0).GetChild(0).gameObject;



        //others


        upridge ups = Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>();

        
        ups.inig1(ups.transform.GetChild(1).transform.gameObject);
        ups.inig2(ups.transform.GetChild(3).transform.gameObject);
        


        ups.inig3(ups.transform.GetChild(2).transform.gameObject);
        ups.inig4(ups.transform.GetChild(0).transform.gameObject);


        //creat roofthxSanding

        GameObject sanding = new GameObject();

       

        GameObject Ridge1 = new GameObject();
        GameObject Ridge2 = new GameObject();
        GameObject Ridge3 = new GameObject();
        GameObject Ridge4 = new GameObject();


        Ridge1.transform.parent = sanding.transform;
        Ridge2.transform.parent = sanding.transform;
        Ridge3.transform.parent = sanding.transform;
        Ridge4.transform.parent = sanding.transform;

        GameObject R11 = Instantiate(Ridge_Nick.transform.GetChild(0).GetChild(0).gameObject, Ridge_Nick.transform.GetChild(0).GetChild(0).gameObject.transform.position, Quaternion.identity) as GameObject;
        GameObject R12 = Instantiate(Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[0].transform.GetChild(0).gameObject, Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[0].transform.GetChild(0).gameObject.transform.position, Quaternion.identity) as GameObject;

        R11.transform.parent = Ridge1.transform;
        R12.transform.parent = Ridge1.transform;

        Ridge1.AddComponent<catline>().AddControlPoint(R11);
        Ridge1.GetComponent<catline>().AddControlPoint(R12);
        Ridge1.GetComponent<catline>().ResetCatmullRom();

        GameObject R21 = Instantiate(Ridge_Nick.transform.GetChild(1).GetChild(0).gameObject, Ridge_Nick.transform.GetChild(1).GetChild(0).gameObject.transform.position, Quaternion.identity) as GameObject;
        GameObject R22 = Instantiate(Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[0].transform.GetChild(2).gameObject, Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[0].transform.GetChild(2).gameObject.transform.position, Quaternion.identity) as GameObject;

        R21.transform.parent = Ridge2.transform;
        R22.transform.parent = Ridge2.transform;

        Ridge2.AddComponent<catline>().AddControlPoint(R21);
        Ridge2.GetComponent<catline>().AddControlPoint(R22);
        Ridge2.GetComponent<catline>().ResetCatmullRom();

        GameObject R31 = Instantiate(Ridge_Nick.transform.GetChild(2).GetChild(0).gameObject, Ridge_Nick.transform.GetChild(2).GetChild(0).gameObject.transform.position, Quaternion.identity) as GameObject;
        GameObject R32 = Instantiate(Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[2].transform.GetChild(0).gameObject, Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[2].transform.GetChild(0).gameObject.transform.position, Quaternion.identity) as GameObject;

        R31.transform.parent = Ridge3.transform;
        R32.transform.parent = Ridge3.transform;

        Ridge3.AddComponent<catline>().AddControlPoint(R31);
        Ridge3.GetComponent<catline>().AddControlPoint(R32);
        Ridge3.GetComponent<catline>().ResetCatmullRom();

        GameObject R41 = Instantiate(Ridge_Nick.transform.GetChild(3).GetChild(0).gameObject, Ridge_Nick.transform.GetChild(3).GetChild(0).gameObject.transform.position, Quaternion.identity) as GameObject;
        GameObject R42 = Instantiate(Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[2].transform.GetChild(2).gameObject, Ridge_Nick.transform.parent.parent.GetChild(7).GetComponent<upridge>().upridgemanage[2].transform.GetChild(2).gameObject.transform.position, Quaternion.identity) as GameObject;

        R41.transform.parent = Ridge4.transform;
        R42.transform.parent = Ridge4.transform;

        Ridge4.AddComponent<catline>().AddControlPoint(R41);
        Ridge4.GetComponent<catline>().AddControlPoint(R42);
        Ridge4.GetComponent<catline>().ResetCatmullRom();




        Ridge1.AddComponent<circlecut1>().reset();
        Ridge2.AddComponent<circlecut1>().reset();
        Ridge3.AddComponent<circlecut1>().reset();
        Ridge4.AddComponent<circlecut1>().reset();


        Ridge1.AddComponent<Ridgetile>().reset();
        Ridge2.AddComponent<Ridgetile>().reset();
        Ridge3.AddComponent<Ridgetile>().reset();
        Ridge4.AddComponent<Ridgetile>().reset();










        //eave
        Ridge_Nick.transform.parent.parent.GetChild(5).GetComponent<EaveControl>().inig(Ridge_Nick.transform.parent.parent.GetChild(5).GetComponent<EaveControl>().eavemanage[0]);
        Ridge_Nick.transform.parent.parent.GetChild(5).GetComponent<EaveControl>().inig(Ridge_Nick.transform.parent.parent.GetChild(5).GetComponent<EaveControl>().eavemanage[2]);
        Ridge_Nick.transform.parent.parent.GetChild(5).GetComponent<EaveControl>().inig(Ridge_Nick.transform.parent.parent.GetChild(5).GetComponent<EaveControl>().eavemanage[1]);
        Ridge_Nick.transform.parent.parent.GetChild(5).GetComponent<EaveControl>().inig(Ridge_Nick.transform.parent.parent.GetChild(5).GetComponent<EaveControl>().eavemanage[3]);

        //rf
        Ridge_Nick.transform.parent.parent.GetChild(2).GetComponent<roofsurcontrol>().inig(Ridge_Nick.transform.parent.parent.GetChild(2).GetComponent<roofsurcontrol>().roofsurfacemanage[0]);
        Ridge_Nick.transform.parent.parent.GetChild(2).GetComponent<roofsurcontrol>().inig(Ridge_Nick.transform.parent.parent.GetChild(2).GetComponent<roofsurcontrol>().roofsurfacemanage[2]);
        Ridge_Nick.transform.parent.parent.GetChild(2).GetComponent<roofsurcontrol>().inig(Ridge_Nick.transform.parent.parent.GetChild(2).GetComponent<roofsurcontrol>().roofsurfacemanage[1]);
        Ridge_Nick.transform.parent.parent.GetChild(2).GetComponent<roofsurcontrol>().inig(Ridge_Nick.transform.parent.parent.GetChild(2).GetComponent<roofsurcontrol>().roofsurfacemanage[3]);

        //RRR
        Ridge_Nick.transform.parent.parent.GetChild(3).GetComponent<roofsurcon2control>().inig(Ridge_Nick.transform.parent.parent.GetChild(3).GetComponent<roofsurcon2control>().rrrRL[0]);
        Ridge_Nick.transform.parent.parent.GetChild(3).GetComponent<roofsurcon2control>().inig(Ridge_Nick.transform.parent.parent.GetChild(3).GetComponent<roofsurcon2control>().rrrRL[2]);
        Ridge_Nick.transform.parent.parent.GetChild(3).GetComponent<roofsurcon2control>().inig(Ridge_Nick.transform.parent.parent.GetChild(3).GetComponent<roofsurcon2control>().rrrRL[1]);
        Ridge_Nick.transform.parent.parent.GetChild(3).GetComponent<roofsurcon2control>().inig(Ridge_Nick.transform.parent.parent.GetChild(3).GetComponent<roofsurcon2control>().rrrRL[3]);
        //rf2


        Ridge_Nick.transform.parent.parent.GetChild(4).GetComponent<roofsurcontrol2>().inig(Ridge_Nick.transform.parent.parent.GetChild(4).GetComponent<roofsurcontrol2>().roofsurface2manage[0]);
        Ridge_Nick.transform.parent.parent.GetChild(4).GetComponent<roofsurcontrol2>().inig(Ridge_Nick.transform.parent.parent.GetChild(4).GetComponent<roofsurcontrol2>().roofsurface2manage[2]);
        Ridge_Nick.transform.parent.parent.GetChild(4).GetComponent<roofsurcontrol2>().inig(Ridge_Nick.transform.parent.parent.GetChild(4).GetComponent<roofsurcontrol2>().roofsurface2manage[1]);
        Ridge_Nick.transform.parent.parent.GetChild(4).GetComponent<roofsurcontrol2>().inig(Ridge_Nick.transform.parent.parent.GetChild(4).GetComponent<roofsurcontrol2>().roofsurface2manage[3]);

    }


    Vector3 con2posiotion(Vector3 a, Vector3 b)
    {

        return new Vector3((a.x + b.x) / 2, ((a.y + b.y) / 2)-2f, (a.z + b.z) / 2);

    }

    public void rectangle_column()
    {

        GameObject Ridge_Nick = TingPartList[0].transform.GetChild(0).GetChild(0).gameObject;
        GameObject column_nick = TingPartList[1].transform.GetChild(0).GetChild(1).gameObject;

        GameObject column_BODY_nick = TingPartList[1].transform.GetChild(0).GetChild(2).gameObject;

        print("oeoeoeoeoeoe");
        for(int i=0;i<4;i++)
        {


            for(int j=0;j<4;j++)
            {
                
                column_nick.transform.GetChild(i).GetChild(j).transform.position = new Vector3(Ridge_Nick.transform.GetChild(i).GetChild(1).transform.position.x, column_nick.transform.GetChild(i).GetChild(j).transform.position.y, Ridge_Nick.transform.GetChild(i).GetChild(1).transform.position.z);

                if (TingPartList[1].transform.GetChild(1).GetComponent<UIcontrol>().isCb == true)
                {
                    column_BODY_nick.transform.GetChild(i).GetChild(j).transform.position = new Vector3(Ridge_Nick.transform.GetChild(i).GetChild(1).transform.position.x, column_BODY_nick.transform.GetChild(i).GetChild(j).transform.position.y, Ridge_Nick.transform.GetChild(i).GetChild(1).transform.position.z);
                }
            }


            column_nick.transform.GetChild(i).GetComponent<catline>().ResetCatmullRom();
            column_nick.transform.GetChild(i).GetComponent<circlecut1>().reset();
            column_nick.transform.GetChild(i).GetComponent<Columntile>().reset();

            if (TingPartList[1].transform.GetChild(1).GetComponent<UIcontrol>().isCb == true)
            {
                column_BODY_nick.transform.GetChild(i).GetComponent<catline>().ResetCatmullRom();
                column_BODY_nick.transform.GetChild(i).GetComponent<circlecut1>().reset();
                column_BODY_nick.transform.GetChild(i).GetComponent<Columntile>().reset();
            }
            

        }



        column_nick.transform.GetChild(0).GetComponent<ColumnControl>().reset();



        if (TingPartList[1].transform.GetChild(1).GetComponent<UIcontrol>().isf == true)
        {
        TingPartList[1].transform.GetChild(3).GetComponent<FC>().reset();
        }
        if (TingPartList[1].transform.GetChild(1).GetComponent<UIcontrol>().isb == true)
        {
            TingPartList[1].transform.GetChild(2).GetComponent<BC>().reset();
        }


        //上梁
        TingPartList[1].transform.GetChild(6).GetComponent<bigbon>().reset();

        if (TingPartList[1].transform.GetChild(1).GetComponent<UIcontrol>().isCb == true)
        {
            TingPartList[1].transform.GetChild(7).GetComponent<ColumnBody>().reset();
        }





    }

    public void rectangle_Platform(float a,float b)
    {

        TingPartList[2].transform.GetChild(2).GetComponent<PlatForm>().para3reset(roof_tall, roof_long);

    
    }


    //rectangle_up

    public void twolayer()
    {


        Vector3 v1 = new Vector3(TingPartPosition[0].x + 17, TingPartPosition[0].y - 15, TingPartPosition[0].z);


        GameObject clone = Instantiate(Resources.Load("RidgeCctwolayer"), v1, Quaternion.identity) as GameObject;
        
        
        clone.transform.parent = transform;
        clone.transform.GetChild(1).GetComponent<roofcontrol>().numberslider.value = 4;
        
       // TingPartList[0].transform.GetChild(1).GetComponent<roofcontrol>().setnumberslidervalue();
    }




	public void ResetRoof(int a)
	{
		//Destroy(TingPartList[0]);


		Vector3 v1 = new Vector3(TingPartPosition[0].x + 20, TingPartPosition[0].y - 10, TingPartPosition[0].z);

		GameObject clone = Instantiate(Resources.Load("RidgeCc"), v1, Quaternion.identity) as GameObject;

		clone.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).transform.Translate(-3, 1, 0);
		clone.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).transform.Translate(-3, 1, 0);

		clone.transform.parent = transform;

		clone.transform.GetChild(1).GetComponent<roofcontrol>().numberslider.value = a;
		//clone.transform.GetChild(1).GetComponent<roofcontrol>().setnumberslidervalue();

		TingPartList[0] = clone;


        GameObject.Find("bao-ding").transform.position = TingPartList[0].transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform.position;



        GameObject Ridge_Nick = TingPartList[0].transform.GetChild(0).GetChild(0).gameObject;

        ini_roof_tall = Vector3.Distance(Ridge_Nick.transform.GetChild(0).GetChild(2).transform.position, Ridge_Nick.transform.GetChild(3).GetChild(2).transform.position) / 2f;
        ini_roof_long = Vector3.Distance(Ridge_Nick.transform.GetChild(0).GetChild(2).transform.position, Ridge_Nick.transform.GetChild(1).GetChild(2).transform.position) / 2f;




    }
	public void ResetBody(int a)
	{

		Destroy(TingPartList[1]);

		GameObject clone = Instantiate(Resources.Load("CC"), TingPartPosition[1], Quaternion.identity) as GameObject;
		clone.transform.parent = transform;





		clone.transform.GetChild(1).GetComponent<UIcontrol>().numberslider.value = a;
		TingPartList[1] = clone;





	}

	public void ResetPlatForm(int a)
	{

		Destroy(TingPartList[2]);

		GameObject clone = Instantiate(Resources.Load("Pt"), TingPartPosition[2], Quaternion.identity) as GameObject;
		clone.transform.parent = transform;

		clone.transform.GetChild(1).GetComponent<UIcontrol>().numberslider.value = a;
		TingPartList[2] = clone;
	}

	public void ResetAll(int a)
	{
		ResetRoof(a);
		ResetBody(a);
		ResetPlatForm(a);

        GameObject.Find("bao-ding").transform.position = TingPartList[0].transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform.position;

        GameObject Ridge_Nick = TingPartList[0].transform.GetChild(0).GetChild(0).gameObject;

        ini_roof_tall = Vector3.Distance(Ridge_Nick.transform.GetChild(0).GetChild(2).transform.position, Ridge_Nick.transform.GetChild(3).GetChild(2).transform.position) / 2f;
        ini_roof_long = Vector3.Distance(Ridge_Nick.transform.GetChild(0).GetChild(2).transform.position, Ridge_Nick.transform.GetChild(1).GetChild(2).transform.position) / 2f;


	}

	public void UpdateAll(int a)
	{
		TingPartList[0].transform.GetChild(1).GetComponent<roofcontrol>().numberslider.value = a;
		TingPartList[1].transform.GetChild(1).GetComponent<UIcontrol>().numberslider.value = a;
		TingPartList[2].transform.GetChild(1).GetComponent<UIcontrol>().numberslider.value = a;

        GameObject.Find("bao-ding").transform.position = TingPartList[0].transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform.position;

        GameObject Ridge_Nick = TingPartList[0].transform.GetChild(0).GetChild(0).gameObject;

        
            TingPartList[1].transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<ColumnControl>().reset();
         
            
        if (TingPartList[1].transform.GetChild(1).GetComponent<UIcontrol>().isCb == true)
        {
            TingPartList[1].transform.GetChild(7).GetComponent<ColumnBody>().reset();
        }



        ini_roof_tall = Vector3.Distance(Ridge_Nick.transform.GetChild(0).GetChild(2).transform.position, Ridge_Nick.transform.GetChild(3).GetChild(2).transform.position) / 2f;
        ini_roof_long = Vector3.Distance(Ridge_Nick.transform.GetChild(0).GetChild(2).transform.position, Ridge_Nick.transform.GetChild(1).GetChild(2).transform.position) / 2f;
    
    
    }



	/*
	public void MoveBody()
	{

		TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).transform.Translate(1, 0, 0);


       
		TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<ColumnControl>().columnmanage[0].GetComponent<catline>().ResetCatmullRom();
		TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<ColumnControl>().reset();


	}
	 * */
    /*
	public void MoveBody2()
	{

		TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).transform.Translate(-1, 0, 0);



		TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<ColumnControl>().columnmanage[0].GetComponent<catline>().ResetCatmullRom();
		TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<ColumnControl>().reset();


	}
    */


	//formfactor



	//roof
    public void ChangeRoof_Roofchild_one()
    {

        TingPartList[0].transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform.Translate(2, 0, 0);
        TingPartList[0].transform.GetChild(1).GetComponent<roofcontrol>().setnumberslidervalue();

    }




	public void UpdateRoof()
	{

       TingPartList[0].transform.GetChild(1).GetComponent<roofcontrol>().setnumberslidervalue();



	}

    public void MoveRoof_Cp1(Vector2 a)
    {

        TingPartList[0].transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform.Translate(Roof_Wide_Percent*a.x, Roof_Height_Percent * a.y, 0);

        TingPartList[0].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RidgeControl>().ridgemanage[0].GetComponent<catline>().ResetCatmullRom();
        TingPartList[0].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RidgeControl>().reset();
        //TingPartList[0].transform.GetChild(1).GetComponent<roofcontrol>().setnumberslidervalue();
    
    }
    public void MoveRoof_Cp2(Vector2 a)
    {

        TingPartList[0].transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).transform.Translate(Roof_Wide_Percent * a.x, Roof_Height_Percent * a.y, 0);

        TingPartList[0].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RidgeControl>().ridgemanage[0].GetComponent<catline>().ResetCatmullRom();
        TingPartList[0].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RidgeControl>().reset();
        

        //TingPartList[0].transform.GetChild(1).GetComponent<roofcontrol>().setnumberslidervalue();
    
    }
    public void MoveRoof_Cp3(Vector2 a)
    {
        //ridge
        TingPartList[0].transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).transform.Translate(Roof_Wide_Percent * a.x, Roof_Height_Percent * a.y, 0);
        //ridgetail
        TingPartList[0].transform.GetChild(0).GetChild(1).GetChild(0).transform.Translate(Roof_Wide_Percent * a.x, Roof_Height_Percent * a.y, 0);


        //ridge
        TingPartList[0].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RidgeControl>().ridgemanage[0].GetComponent<catline>().ResetCatmullRom();
        TingPartList[0].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RidgeControl>().reset();


        //ridgetail
        TingPartList[0].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<RidgetailControl>().ridgetailmanage[0].GetComponent<catline>().ResetCatmullRom();
        TingPartList[0].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<RidgetailControl>().reset();




        //TingPartList[0].transform.GetChild(1).GetComponent<roofcontrol>().setnumberslidervalue();
    
    }








	//body
	public void MoveBody(float a)
	{

		TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).transform.Translate((1 / percent) * a, 0, 0);
        //temp



       // TingPartList[1].transform.GetChild(0).GetChild(2).GetChild(0).transform.Translate((1 / percent) * a *1/3, 0, 0);
        //*************
		TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<ColumnControl>().columnmanage[0].GetComponent<catline>().ResetCatmullRom();
		TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<ColumnControl>().reset();


        
            TingPartList[1].transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<ColumnControl>().columnmanage[0].GetComponent<catline>().ResetCatmullRom();
            TingPartList[1].transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<ColumnControl>().reset();
 if (TingPartList[1].transform.GetChild(1).GetComponent<UIcontrol>().isCb == true)
        {
            TingPartList[1].transform.GetChild(7).GetComponent<ColumnBody>().reset();
        }

        TingPartList[1].transform.GetChild(1).GetComponent<UIcontrol>().setnumberslidervalue();

        //new


        if (TingPartList[1].transform.GetChild(1).GetComponent<UIcontrol>().isf == true)
        {
            TingPartList[1].transform.GetChild(3).GetComponent<FC>().reset();
        }

        if (TingPartList[1].transform.GetChild(1).GetComponent<UIcontrol>().isb == true)
        {
            TingPartList[1].transform.GetChild(2).GetComponent<BC>().reset();
        }

        //上梁

        
        TingPartList[1].transform.GetChild(6).GetComponent<bigbon>().reset();
       
        

	}


    public void Move_F(float a,float b)
    {

        if (TingPartList[1].transform.GetChild(1).GetComponent<UIcontrol>().isb == true)
        {
            TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).transform.position = new Vector3(TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).transform.position.x, TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).transform.position.y - ((a / b) * columntall), TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).transform.position.z);
        }

      }

    public void Move_B(float a, float b)
    {

        if (TingPartList[1].transform.GetChild(1).GetComponent<UIcontrol>().isf == true)
        {
            TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).transform.position = new Vector3(TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).transform.position.x, TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(3).transform.position.y + ((a / b) * columntall), TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).transform.position.z);
        }
    }




    public void UpdateBody_F(bool a)
    {

        print("#$@#$@#$@#$@$@#$@#$@$@#$@#$  "+a);

        TingPartList[1].transform.GetChild(1).GetComponent<UIcontrol>().isf = a;
        TingPartList[1].transform.GetChild(1).GetComponent<UIcontrol>().setnumberslidervalue();
        


    }
    public void UpdateBody_B(bool a)
    {

        TingPartList[1].transform.GetChild(1).GetComponent<UIcontrol>().isb = a;
        TingPartList[1].transform.GetChild(1).GetComponent<UIcontrol>().setnumberslidervalue();
        

    }



    public void moveBOBODY(float a)
    {

        if (TingPartList[1].transform.GetChild(1).GetComponent<UIcontrol>().isCb == true)
        {
            TingPartList[1].transform.GetChild(0).GetChild(2).GetChild(0).transform.Translate((1 / percent2) * a, 0, 0);
        }

        TingPartList[1].transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<ColumnControl>().columnmanage[0].GetComponent<catline>().ResetCatmullRom();
        TingPartList[1].transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<ColumnControl>().reset();
        if (TingPartList[1].transform.GetChild(1).GetComponent<UIcontrol>().isCb == true)
        {
            TingPartList[1].transform.GetChild(7).GetComponent<ColumnBody>().reset();
        }



      }



    public void bodytall(float a)
    {

        

        TingPartList[0].transform.Translate(0, (columntall)*a, 0);


        for (int i = 0; i < tower.Count; i++)
        {
            if (TingPartList[1].name == tower[i].name)
            {

                for (int j = i+1; j < tower.Count; j++)
                {
                    tower[j].transform.Translate(0, (columntall) * a, 0);
                    if (tower_roof[j - 1]!=null)
                    tower_roof[j-1].transform.Translate(0, (columntall) * a, 0);
                }


                TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).transform.Translate(0, (columntall) * a, 0);
                TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).transform.Translate(0, (columntall) * a, 0);

                TingPartList[1].transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).transform.Translate(0, (columntall) * a, 0);
                TingPartList[1].transform.GetChild(0).GetChild(2).GetChild(0).GetChild(1).transform.Translate(0, (columntall) * a, 0);



                break;
            }

        }

                //TingPartList[1].transform.Translate(0, (columntall) * a, 0);




        /*
        TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).transform.Translate(0, (columntall) * a, 0);
        TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).transform.Translate(0, (columntall) * a, 0);

        TingPartList[1].transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).transform.Translate(0, (columntall) * a, 0);
        TingPartList[1].transform.GetChild(0).GetChild(2).GetChild(0).GetChild(1).transform.Translate(0, (columntall) * a, 0);


        */


        //MoveTowerBody(TingPartList[1]);



        //clone.transform.GetChild(2).GetComponent<UIcontrol>().numberslidervalue = ucit.numberslidervalue;
        
        
        TingPartList[1].transform.GetChild(1).GetComponent<UIcontrol>().setnumberslidervalue();


        /*
        TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<catline>().ResetCatmullRom();
        TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<circlecut1>().reset();
        TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Columntile>().reset();

        TingPartList[1].transform.GetChild(7).GetComponent<ColumnBody>().reset();

        */

        columntall = TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<ColumnControl>().ColumnLong;


        /*
        GameObject cc = TingPartList[1];

        Vector3 v1 = new Vector3(cc.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).transform.position.x, cc.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).transform.position.y-15,cc.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).transform.position.z);



        
        GameObject clone2 = Instantiate(Resources.Load("RidgeCctwolayer"), v1, Quaternion.identity) as GameObject;
        
       
        clone2.transform.parent = transform;
        clone2.transform.GetChild(1).GetComponent<roofcontrol>().numberslider.value = ucit.numberslidervalue;
        */


        GameObject.Find("bao-ding").transform.position = TingPartList[0].transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform.position;

        // TingPartList[0].transform.GetChild(1).GetComponent<roofcontrol>().setnumberslidervalue();



    }








	//platform
	public void paraplat(float a, float b)
	{

		TingPartList[2].transform.GetChild(2).GetComponent<PlatForm>().parareset((platpercent) * a, (platpercent) * b);



	}









	void OnGUI()
	{




	}






}
