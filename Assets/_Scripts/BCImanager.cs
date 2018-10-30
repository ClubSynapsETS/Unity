using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BCImanager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float x = Mathf.Abs(Mathf.Sin(Time.time/2));
		
		Debug.Log(x);
	}
}
