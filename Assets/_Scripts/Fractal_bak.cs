using UnityEngine;
using System.Collections;

public class Fractal_bak: MonoBehaviour {
	
	public int maxDepth;
	public int dungeonScale;
	public int levelMax;
	
	private int depth;
	private float moveRate, nextMove;
	
	private Material mat_down, mat_up, mat_left, mat_right;
	
	// Use this for initialization
	void Start () {
		
		//Load materials
		mat_down = Resources.Load ("Materials/Fractal Down") as Material;
		mat_up = Resources.Load ("Materials/Fractal Up") as Material;
		mat_left = Resources.Load ("Materials/Fractal Left") as Material;
		mat_right = Resources.Load ("Materials/Fractal Right") as Material;
		
		
		//For rotation
		moveRate = 0.5f;
		
		
		//Create the initial cube
		GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube.transform.localScale = Vector3.one * dungeonScale;
		cube.transform.parent = this.transform;
		cube.tag = "Fractal";
		
		
		//Iterations
		GameObject[] arrayofcubes;
		
		while (depth <= maxDepth) {
			
			arrayofcubes = GameObject.FindGameObjectsWithTag ("Fractal");
			
			foreach (GameObject go in arrayofcubes) {
				if (go.transform.localScale.x > 0.1)
					Vicsek (go, depth);
			}
			depth += 1;
			
		}
		
		
		
	}
	
	
	void Update(){
		
		if (Input.GetKey(KeyCode.RightArrow) && Time.time > nextMove){
			
			//Fractal level rotation
			nextMove = Time.time + moveRate;
			RotateLevel (1);
		}
		
		if (Input.GetKey(KeyCode.UpArrow) && Time.time > nextMove){
			
			//Fractal level rotation
			nextMove = Time.time + moveRate;
			RotateLevel (2);
		}
		
		
	}
	
	
	
	
	// Vicsek fractal iteration
	private void Vicsek(GameObject parent, int depth) {
		
		GameObject parent_container = parent.transform.parent.gameObject;
		
		GameObject container = new GameObject("Level " + depth);
		container.transform.position = parent.transform.position;
		container.transform.rotation = parent.transform.rotation;
		container.transform.parent = parent_container.transform;
		
		Vector3 position = parent.transform.position;
		Vector3 scale = parent.transform.localScale * 1/3;
		
		GameObject cube1 =  GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube1.transform.localScale = scale;
		cube1.transform.position = position;
		cube1.transform.SetParent(container.transform,true);
		cube1.tag = "Fractal";
		
		GameObject cube2 =  GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube2.transform.localScale = scale;
		cube2.transform.position = position + (Vector3.up * scale.y );
		cube2.GetComponent<MeshRenderer> ().material = mat_up;
		cube2.transform.SetParent(container.transform,true);
		cube2.tag = "Fractal";
		
		GameObject cube3 =  GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube3.transform.localScale = scale;
		cube3.transform.position = position + (Vector3.down * scale.y);
		cube3.GetComponent<MeshRenderer> ().material = mat_down;
		cube3.transform.SetParent(container.transform,true);
		cube3.tag = "Fractal";
		
		GameObject cube4 =  GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube4.transform.localScale = scale;
		cube4.transform.position = position + (Vector3.left * scale.x);
		cube4.GetComponent<MeshRenderer> ().material = mat_left;
		cube4.transform.SetParent(container.transform,true);
		cube4.tag = "Fractal";
		
		GameObject cube5 =  GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube5.transform.localScale = scale;
		cube5.transform.position =  position +(Vector3.right * scale.x);
		cube5.GetComponent<MeshRenderer> ().material = mat_right;
		cube5.transform.SetParent(container.transform,true);
		cube5.tag = "Fractal";
		
		//parent.GetComponent<Renderer>().enabled = false;
		DestroyImmediate (parent);
		
	}
	
	
	// Array of gameObject for one fractal level
	private GameObject[] SelectLevel(int level){
		
		Transform[] ts = this.transform.GetComponentsInChildren<Transform> (true);
		int size = (int)Mathf.Pow (5, (float)level);
		GameObject[] selectorLevel = new GameObject[size];
		
		int cpt = 0;
		foreach (Transform t in ts) 
		if (t.gameObject.name == "Level " + level){
			selectorLevel[cpt] = t.gameObject;
			cpt += 1;
		}
		
		return selectorLevel;
	}
	
	
	// Rotate a fractal level
	private void RotateLevel(int level){
		
		GameObject[] selectorLevel = SelectLevel (level);
		GameObject gameObj;
		int istart = (int)Mathf.Pow (5, (float)(level - 1));
		
		for (int i=istart; i<selectorLevel.Length; i++) {
			if (i % 5 != 0) {
				gameObj = selectorLevel [i];
				gameObj.transform.RotateAround (gameObj.gameObject.transform.parent.position, Vector3.forward, 45f);
			}
			
		}
	}
	
}
