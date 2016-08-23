using UnityEngine;
using System.Collections;

[AddComponentMenu("")]
public class NodeCreator : MonoBehaviour 
{
	public Spline spline;
	public SplineMesh splineMesh;
	
	// Update is called once per frame
	void Update () 
	{
		Move( );
		
		//Insert a new node if space is pressed
		if( Input.GetKeyDown( KeyCode.Space ) )
		{




            publicvar.number++;
			//Create a new spline node at the spline's end and store the created gameObject in a variable.
			GameObject newSplineNode = spline.AddSplineNode( );
			
			//Set the new node's position to the current position of the character. 
			newSplineNode.transform.position = this.transform.position;
			
			//Increase the segment count of the spline mesh, so that it doesn't look edgy when 
			//the spline gets very long
			splineMesh.segmentCount += 3;
		}
		
		//Delete the first node when X is pressed
		if( Input.GetKeyDown( KeyCode.X ) )
		{
            
			//Get the array of nodes
			SplineNode[] splineNodes = spline.SplineNodes;
			
			//If there are no spline nodes left, return
			if( splineNodes.Length < 1 )
            { 
            Destroy(GameObject.Find("dot0"));
            spline.RemoveSplineNode(GameObject.Find("dot0"));
            publicvar.number=0;
            
			return;
            }
            
			//Get the spline's first node

            //�쥻��
            //SplineNode firstNode = splineNodes[0];
            SplineNode firstNode = splineNodes[publicvar.number-1];
            
			
			//Remove it from the spline
			//spline.RemoveSplineNode( firstNode );
            spline.RemoveSplineNode(firstNode);
			
			splineMesh.segmentCount -= 3;

            Destroy(GameObject.Find("dot" + (publicvar.number - 1)));
            publicvar.number--;
		}
	}
	
	void Move( )
	{
		if( Input.GetKey( KeyCode.W ) )
			transform.position = transform.position + Vector3.forward * Time.deltaTime * 4f;
		if( Input.GetKey( KeyCode.S ) )
			transform.position = transform.position + Vector3.back * Time.deltaTime * 4f;
		
		if( Input.GetKey( KeyCode.A ) )
			transform.position = transform.position + Vector3.left * Time.deltaTime * 4f;
		if( Input.GetKey( KeyCode.D ) )
			transform.position = transform.position + Vector3.right * Time.deltaTime * 4f;
	}
}
