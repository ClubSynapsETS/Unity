using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSC_receiver : MonoBehaviour {

	private float value;
	private Transform goTransform; 
	private Vector3 position;
	private float initialValue;

	// Use this for initialization
	void Start () {
		//Transform  
        this.goTransform = GetComponent<Transform>();  
		initialValue = goTransform.localPosition.y;
		value = initialValue;
	}
	
	// Update is called once per frame
	void Update () {
		position = goTransform.localPosition;
		position.y = initialValue + value;
		goTransform.localPosition = position;
	}

	public void setValue(float val){
	value = val;
	}

}
