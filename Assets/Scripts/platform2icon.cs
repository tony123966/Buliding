﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class platform2icon : MonoBehaviour
{


    public List<GameObject> pf_cplist = new List<GameObject>();





    public DragItemController dragitemcontroller;
    public Movement movement;


    Mesh mesh;
    Vector3[] verts;
    Vector3[] iniverts;


    //for ratio
    float ini_platdis_x;
    float ini_platdis_y;
    public float ratio_platdis_w;
    public float ratio_platdis_h;


    void Awake()
    {

        mesh = GetComponent<MeshFilter>().mesh;
        movement = GameObject.Find("Movement").GetComponent<Movement>();

        if (!gameObject.GetComponent<MeshFilter>()) gameObject.AddComponent<MeshFilter>();
        if (!gameObject.GetComponent<MeshRenderer>()) gameObject.AddComponent<MeshRenderer>();





        if (pf_cplist.Count == 4)
        {
            mesh.vertices = new Vector3[] {
    pf_cplist [0].transform.localPosition,
    pf_cplist [1].transform.localPosition,
    pf_cplist [2].transform.localPosition,
    pf_cplist [3].transform.localPosition
   };
            mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
            verts = mesh.vertices;
            movement.freelist.AddRange(pf_cplist);
            iniverts = new Vector3[pf_cplist.Count];
            //inidis
            //   ini_platdis_h = pf_cplist [1].transform.localPosition.x - pf_cplist [0].transform.localPosition.x;
            //   ini_platdis_h = Mathf.Abs (ini_platdis_h);
            //   ini_platdis_h = ini_platdis_h / 2;
            //
            //   ini_platdis_w = pf_cplist [1].transform.localPosition.y - pf_cplist [2].transform.localPosition.y;
            //   ini_platdis_w = Mathf.Abs (ini_platdis_w);
            //   ini_platdis_w = ini_platdis_w / 2;







        }





    }

    // Use this for initialization
    void Start()
    {
        dragitemcontroller = GameObject.Find("DragItemController").GetComponent<DragItemController>();
        for (int i = 0; i < pf_cplist.Count; i++)
        {
            iniverts[i] = pf_cplist[i].transform.localPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            print("Update verts array");
            for (int i = 0; i < pf_cplist.Count; i++)
            {
                verts[i] = pf_cplist[i].transform.localPosition;

            }
        }

    }
    void adjMesh()
    {
        for (int i = 0; i < pf_cplist.Count; i++)
        {
            //Debug.Log (controlPointList[i].transform.localPosition);
            verts[i] = pf_cplist[i].transform.localPosition;
        }
        mesh.vertices = verts;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

    }
    public void adjPos()
    {


        for (int i = 0; i < pf_cplist.Count; i++)
        {
            if (dragitemcontroller.chooseObj == pf_cplist[i])
            {
                if (pf_cplist.Count == 4)
                {
                    float offset_x, offset_y;
                    Vector3 mouse_pos = dragitemcontroller.chooseObj.transform.localPosition;
                    Vector3 ori_mouse_pos = verts[i]; //找x和他一樣的
                    offset_x = mouse_pos.x - ori_mouse_pos.x;
                    offset_y = mouse_pos.y - ori_mouse_pos.y;
                    //
                    ini_platdis_x = mouse_pos.x - iniverts[i].x;
                    ini_platdis_y = mouse_pos.y - iniverts[i].y;
                    ratio_platdis_w = ini_platdis_x;
                    ratio_platdis_h = ini_platdis_y;

                    for (int j = 0; j < pf_cplist.Count; j++)
                    {
                        if ((ori_mouse_pos.x == pf_cplist[j].transform.localPosition.x) && (i != j))
                        { //* i->choose obj *////2.adjust other point.
                            pf_cplist[j].transform.localPosition =
                              new Vector3(mouse_pos.x,
                             verts[j].y - (offset_y),
                             verts[j].z);
                        }
                        else if ((ori_mouse_pos.y == pf_cplist[j].transform.localPosition.y) && (i != j))
                        {
                            pf_cplist[j].transform.localPosition =
                              new Vector3(verts[j].x - (offset_x),
                             mouse_pos.y,
                             verts[j].z);
                        }
                        else if (i != j)
                        {
                            pf_cplist[j].transform.localPosition =
                              new Vector3(verts[j].x - (offset_x),
                             verts[j].y - (offset_y),
                             verts[j].z);
                        }
                    }

                }

            }
        }

        for (int x = 0; x < pf_cplist.Count; x++)
        {
            verts[x] = pf_cplist[x].transform.localPosition;
        }
        adjMesh();
    }
    public void addpoint()
    {

        movement.freelist.AddRange(pf_cplist);


    }
}