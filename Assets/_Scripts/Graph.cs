using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {

	public Transform pointPrefab;
	
	public int resolution = 200;
	private Transform[] points;
	private ArrayList yValues;

	void Awake() {
		float step = 2f / resolution;
		Vector3 scale = Vector3.one * step;
		Vector3 position = Vector3.zero;		
		points = new Transform[resolution];
		yValues = new ArrayList();
	
		for (int i=0; i<resolution; i++) {
			Transform point = Instantiate(pointPrefab);
			position.x = (i + 0.5f) * step - 1f;
			point.localPosition = position;
			point.localScale = scale;
			point.SetParent(transform, false);
			points[i] = point;
			yValues.Add(0.0f);
			
		}

	}

	void Update() {

		refreshPoints();
	}

	void refreshPoints() {
		for (int i=0; i<resolution; i++) {

			Transform point = points[i]; 
			Vector3 position = point.localPosition;
			position.y = (float)yValues[i];
			point.localPosition = position;
		}
		
		
	}

	public void setNewValue(float newValue){
		yValues.RemoveAt(0);
		yValues.Add(newValue);
	}

}
