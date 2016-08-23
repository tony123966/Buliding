using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Circlecut : MonoBehaviour {

    public Catline catline;

    public static float tiledLength = 0.5f;
    public List<Vector3> anchorpointlist = new List<Vector3>();
	void Awake() 
	{
		catline = transform.GetComponent<Catline>();
	}
    public void SetCutpoint()
    {
        Vector3 born = new Vector3(0, 0, 0);
        Vector3 mid = new Vector3(0, 0, 0);

		for (int k = 0; k < catline.innerPointList.Count; k++)
        {
            if (k == 0)
            {
				Vector3 fi = catline.innerPointList[k];
                
                anchorpointlist.Add(fi);

            }
            float min = 1000;
			Vector3 ori = catline.innerPointList[k];

            int h = k;

			for (int j = h; j < catline.innerPointList.Count; j++)
            {
				Vector3 dot = catline.innerPointList[j];
                Vector3 miid = (ori + dot) / 2;

                float iwant = Vector3.Distance(ori, dot);

				if (Mathf.Abs((iwant - tiledLength)) < min) {
					min = Mathf.Abs((iwant - tiledLength));
                    born = dot;
                    mid = miid;
                    k = j;
                }
                else { 
					break; 
				}

            }
            anchorpointlist.Add(born);
        }
        
    }

	public void ResetCutpoint()
    {
        anchorpointlist.Clear();
		SetCutpoint();
    }

}
