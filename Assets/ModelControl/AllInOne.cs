using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class AllInOne : MonoBehaviour {

    public GameObject center;

    public GameObject roof;
    public GameObject body;
    public GameObject platform;

    public List<GameObject> TingPartList = new List<GameObject>();
    public List<Vector3> TingPartPosition = new List<Vector3>();

    public int number;


	// Use this for initialization
	void Start () 
    
    
    {


        TingPartList.Add(roof);

        TingPartList.Add(body);

        TingPartList.Add(platform);

        TingPartPosition.Add(roof.transform.position);
        TingPartPosition.Add(body.transform.position);
        TingPartPosition.Add(platform.transform.position);


	
	}
	
	// Update is called once per frame
	void Update () 
    
    {
	
	}



    public void ResetRoof(int a)
    {
        Destroy(TingPartList[0]);

        GameObject clone = Instantiate(Resources.Load("RidgeCc"), TingPartPosition[0], Quaternion.identity) as GameObject;
        
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

	public void UpdateAll()
	{
		TingPartList[0].transform.GetChild(1).GetComponent<roofcontrol>().numberslider.value = number;
		TingPartList[1].transform.GetChild(1).GetComponent<UIcontrol>().numberslider.value = number;
		TingPartList[2].transform.GetChild(1).GetComponent<UIcontrol>().numberslider.value = number;
	}




    void OnGUI()
    {

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






}
