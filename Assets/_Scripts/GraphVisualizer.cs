using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphVisualizer : MonoBehaviour {
	
	public Camera camera;
	private LineRenderer lRenderer;  
	private Vector3 linePos;
	private Transform goTransform;  
	private Vector3[] linePoints; 

	private ArrayList yValues;
	private int resolution = 200;

	void Awake ()  
        {  
            //LineRenderer  
            this.lRenderer = GetComponent<LineRenderer>();  
            //Transform  
            this.goTransform = GetComponent<Transform>();  
        }  

	// Use this for initialization
	void Start () {
		
		yValues = new ArrayList();
		// lRenderer.SetVertexCount(samples.Length);  
		lRenderer.positionCount = resolution;
		linePoints = new Vector3[resolution];  
		goTransform.position = new Vector3(-resolution/2,goTransform.position.y,goTransform.position.z);  

		Vector3 camPos = camera.GetComponent<Transform>().position;
		camPos.x = resolution/2;
		camPos.z = -10;
		camera.GetComponent<Transform>().position = camPos;

		 //For each sample  
		for(int i=0; i<resolution;i++)  
		{ 	
			linePos = new Vector3(goTransform.position.x + i, goTransform.position.y, goTransform.position.z);
			//Get the recently instantiated cube Transform component  
			yValues.Add(0.0f);
			linePoints[i] = linePos;  
		}  

		
		
	}
	
	// Update is called once per frame
	void Update () { 
		refreshPoints();
	}

	void refreshPoints() {

		for(int i=0; i<resolution;i++) {

            linePos = linePoints[i];
			linePos.y = (float)yValues[i];
			lRenderer.SetPosition(i, linePos- goTransform.position); 
		}


	}

	public void setNewValue(float newValue){
		yValues.RemoveAt(0);
		yValues.Add(newValue);
	}
}
