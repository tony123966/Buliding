using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class AllInOne : MonoBehaviour
{

	public GameObject center;

	public GameObject roof;
	public GameObject body;
	public GameObject platform;




	public List<GameObject> TingPartList = new List<GameObject>();
	public List<Vector3> TingPartPosition = new List<Vector3>();

	public int number;

	//column's percent
	public float percent;
	public float platpercent;
    public float columntall;


    public float Roof_Height_Percent;
    public float Roof_Wide_Percent;

	void Awake()
	{ 
	    Application.targetFrameRate = 60;
	}

	// Use this for initialization
	void Start()
	{


		TingPartList.Add(roof);

		TingPartList.Add(body);

		TingPartList.Add(platform);

		TingPartPosition.Add(roof.transform.position);
		TingPartPosition.Add(body.transform.position);
		TingPartPosition.Add(platform.transform.position);

		percent = 1 / body.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<ColumnControl>().EyeToColumn;
		platpercent = platform.transform.GetChild(2).GetComponent<PlatForm>().percent;
        columntall = body.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<ColumnControl>().ColumnLong;


        Roof_Height_Percent = roof.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RidgeControl>().Height;
        Roof_Wide_Percent = roof.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RidgeControl>().Wide;

	}

	// Update is called once per frame
	void Update()
	{

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{

            ChangeRoof();
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
            twolayer();
			
		}

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{

			UpdateAll(3);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{

			UpdateAll(4);
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			UpdateAll(5);

		}
		if (Input.GetKeyDown(KeyCode.Alpha6))
		{

			UpdateAll(6);
		}

		if (Input.GetKeyDown(KeyCode.Alpha7))
		{

			UpdateAll(7);
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

			UpdateAll(50);
		}




	}


    public void twolayer()
    {


        Vector3 v1 = new Vector3(TingPartPosition[0].x + 17, TingPartPosition[0].y - 15, TingPartPosition[0].z);




        GameObject clone = Instantiate(Resources.Load("RidgeCctwolayer"), v1, Quaternion.identity) as GameObject;
        
        
        clone.transform.parent = transform;
        
        
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
	}

	public void UpdateAll(int a)
	{
		TingPartList[0].transform.GetChild(1).GetComponent<roofcontrol>().numberslider.value = a;
		TingPartList[1].transform.GetChild(1).GetComponent<UIcontrol>().numberslider.value = a;
		TingPartList[2].transform.GetChild(1).GetComponent<UIcontrol>().numberslider.value = a;
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
    public void ChangeRoof()
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



		TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<ColumnControl>().columnmanage[0].GetComponent<catline>().ResetCatmullRom();
		TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<ColumnControl>().reset();


	}


    public void Move_F(float a,float b)
    {
        
            TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).transform.position = new Vector3(TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).transform.position.x, TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).transform.position.y - ((a / b) * columntall), TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).transform.position.z);
      

      }

    public void Move_B(float a, float b)
    {
       
            TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).transform.position = new Vector3(TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).transform.position.x, TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(3).transform.position.y + ((a / b) * columntall), TingPartList[1].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).transform.position.z);
        
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



	//platform
	public void paraplat(float a, float b)
	{

		TingPartList[2].transform.GetChild(2).GetComponent<PlatForm>().parareset((platpercent) * a, (platpercent) * b);



	}









	void OnGUI()
	{




	}






}
