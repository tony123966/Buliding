using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIcontrol : MonoBehaviour 
{
    /*
    public RidgeControl ridgecontrol;
   
    public EaveControl eavecontrol;
    public roofsurcontrol roofcontrol;
    public roofsurcon2control roofcontrol2;
    public roofsurcontrol2 roofcontrolS;

    public RidgetailControl rtc;
    
    */

    public ColumnControl columncontrol;
    public BC bc;
    public FC fc;
    public PlatForm plat;
   // public RidgetailControl rtc;

   // public upridge up;

   // public bool upridge = false;

    public Slider numberslider;
    public Slider tiledwideslider;
    public Slider ohoh;

    public int numberslidervalue;
    public int twvalue;
    public int ohohv;

    public  GameObject center;


	// Use this for initialization
	void Start () 
    {
        center = transform.parent.parent.GetChild(0).gameObject;
        numberslidervalue = (int)numberslider.value;
        //ohohv = (int)ohoh.value;

        setnumberslidervalue();
	}
	
	// Update is called once per frame
	void Update () 
    {
        /*
        if (Input.GetKeyDown(KeyCode.A))
        {

            print(transform.name + " : ");
            print(numberslider.value);



            numberslider.value = numberslider.value + 1;


            setnumberslidervalue();




        }
        if (Input.GetKeyDown(KeyCode.S))
        {

            print(transform.name + " : ");
            print(numberslider.value);


            numberslider.value = numberslider.value - 1;

            setnumberslidervalue();



        }
        */
	}

    public void setnumberslidervalue()
    {
       
        numberslidervalue = (int)numberslider.value;

        







        /*
        if (transform.parent.GetChild(0).GetChild(0).GetChild(0).GetComponent<catline>())
        {
            transform.parent.GetChild(0).GetChild(0).GetChild(0).GetComponent<catline>().ResetCatmullRom();
        }


        if (transform.parent.GetChild(0).childCount>2)
        {
                transform.parent.GetChild(0).GetChild(1).GetChild(0).GetComponent<catline>().ResetCatmullRom();
        }
        

        if (ridgecontrol)
        {

            ridgecontrol.reset();

            if (upridge == true)
            {
                up.reset();
            }

            
            if (rtc)
            {
                rtc.reset();
            }
             
            eavecontrol.reset();
            roofcontrol.reset();
            roofcontrol2.reset();
            roofcontrolS.reset();
        }
         
         */
        if (columncontrol)
        {
            columncontrol.reset();


            if (bc)
            {
                bc.reset();
            }

            if (fc)
            {
                fc.reset();
            }
            
        }

        
        if (plat)
        {




            
            //if (ridgecontrol)
            //{
                plat.reset();
            //}
             
            //else
            //{


            
            plat.reset();


               // print(plat.parayn);



                if (plat.parayn == true)
                {
                    
                    print("~~~");
                    

                    Destroy(plat.platform[0]);
                    plat.platform.Clear();
                    
                    plat.para2reset();
                    //plat.parareset(plat.looog,plat.doot);
                     
                }
                


            //}
           
        }
        

            
    }





    public void settwvalue()
    {
        /*
        twvalue = (int)tiledwideslider.value;

        roofcontrol.withoutinireset();
        roofcontrol2.reset();
        roofcontrolS.reset();
         */ 

    }




    public void setohohvalue()
    {
        ohohv = (int)ohoh.value;

        fc.reset();
        bc.reset();

    }

    /*
    void OnGUI()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {

            print(transform.name + " : ");
            print(numberslider.value);



            numberslider.value = numberslider.value+1;
              

               setnumberslidervalue();




        }
        if (Input.GetKeyDown(KeyCode.S))
        {

            print(transform.name + " : ");
            print(numberslider.value);


            numberslider.value = numberslider.value-1;

               setnumberslidervalue();


            
        }

    }

    */



}
