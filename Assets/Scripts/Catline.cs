using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Catline : MonoBehaviour
{
	public List<GameObject> controlPointList = new List<GameObject>();
   
    public int numberOfPoints =20;
    public List<Vector3> innerPointList = new List<Vector3>();
	
	public float tiledLength = 0.5f;
	public List<Vector3> anchorpointlist = new List<Vector3>(); 

    public void SetCatmullRom()
    {
        DisplayCatmullromSpline();
    }

    Vector3 ReturnCatmullRomPos(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Vector3 pos = 0.5f * ((2f * p1) + (-p0 + p2) * t + (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t + (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t);
        return pos;
    }
    void DisplayCatmullromSpline()
    {
        innerPointList.Clear();
        Vector3 p0, p1, p2, p3;

        if (controlPointList.Count < 2) return;
        else if (controlPointList.Count == 2)
        {
            p0 = controlPointList[0].transform.position;
            p1 = controlPointList[0].transform.position;
            p2 = controlPointList[1].transform.position;
            p3 = controlPointList[1].transform.position;


            float segmentation = 1 / (float)numberOfPoints;
            float t = 0;
            for (int i = 0; i < numberOfPoints; i++)
            {
                Vector3 newPos = ReturnCatmullRomPos(t, p0, p1, p2, p3);
                innerPointList.Add(newPos);
                t += segmentation;
            }
        }
        else
        {
            for (int index = 0; index < controlPointList.Count - 1; index++)
            {
                if (index == 0)
                {
                    p0 = controlPointList[0].transform.position;
                    p1 = controlPointList[0].transform.position;
                    p2 = controlPointList[1].transform.position;
                    p3 = controlPointList[2].transform.position;
                }
                else if (index == controlPointList.Count - 2)
                {
                    p0 = controlPointList[index - 1].transform.position;
                    p1 = controlPointList[index].transform.position;
                    p2 = controlPointList[index + 1].transform.position;
                    p3 = controlPointList[index + 1].transform.position;
                }
                else
                {
                    p0 = controlPointList[index - 1].transform.position;
                    p1 = controlPointList[index].transform.position;
                    p2 = controlPointList[index + 1].transform.position;
                    p3 = controlPointList[index + 2].transform.position;
                }

                float segmentation = 1 / (float)numberOfPoints;
                float t = 0;
                for (int i = 0; i < numberOfPoints; i++)
                {
                    Vector3 newPos = ReturnCatmullRomPos(t, p0, p1, p2, p3);
                    innerPointList.Add(newPos);
                    t += segmentation;
                }
            }
        }
    }
 
    public void AddControlPoint(GameObject obj)
    {
		controlPointList.Add(obj);
    }
    public void MoveControlPoint(GameObject obj, Vector3 point)
    {
        obj.transform.position = point;
    }
    public void RemoveControlPoint(GameObject obj)
    {

        for (int i = 0; i < controlPointList.Count; i++)
        {
            if (controlPointList[i] == obj.transform)
            {
                controlPointList.Remove(controlPointList[i]);
                Destroy(obj);
                break;
            }

        }
    }
    public void ShowOrHideControlPoint(bool isShow)
    {
		for (int i = 0; i < controlPointList.Count; i++)
        {
			if (controlPointList[i].gameObject.GetComponent<SphereCollider>()) controlPointList[i].gameObject.GetComponent<SphereCollider>().enabled = isShow;
			if (controlPointList[i].gameObject.GetComponent<MeshRenderer>()) controlPointList[i].gameObject.GetComponent<MeshRenderer>().enabled = isShow;
        }
    }
	public void SetCutpoint()
	{
		if(innerPointList.Count<2)return;
		
		anchorpointlist.Add(innerPointList[0]);
		float t = 0;
		for (int i = 0; i < innerPointList.Count-1; i++)
        {
			t += Vector3.Distance(innerPointList[i], innerPointList[i+1]);
			if(t>=tiledLength)
			{
				t = 0;
				anchorpointlist.Add(innerPointList[i]);
			}

		}
		if (anchorpointlist[anchorpointlist.Count - 1] != innerPointList[innerPointList.Count - 1]) anchorpointlist.Add(innerPointList[innerPointList.Count - 1]);
	}
}
