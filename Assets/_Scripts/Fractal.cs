using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour {
	
	public int maxDepth;
	public int dungeonScale;
	public int levelMax;

	private int depth;
	private float moveRate, nextMove;

	private Material mat_down, mat_up, mat_left, mat_right;
	private GameObject asset;

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
		//GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		GameObject cube = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/modular_asset"));
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
		/*
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
		*/

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


		//GameObject cube1 =  GameObject.CreatePrimitive (PrimitiveType.Cube);
		/*GameObject cube1 = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/modular_asset"));
		cube1.transform.localScale = scale;
		cube1.transform.position = position;
		cube1.transform.SetParent(container.transform,true);
		cube1.tag = "Fractal";*/

		GameObject cube1 = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Corridor_X"));
		cube1.transform.localScale = scale;
		float sizeObject = cube1.GetComponent<Collider> ().bounds.size.x;
		Debug.Log (sizeObject);
		cube1.transform.position = position;
		cube1.transform.SetParent(container.transform,true);
		cube1.tag = "Fractal";


		//GameObject cube2 =  GameObject.CreatePrimitive (PrimitiveType.Cube);
		GameObject cube2 = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Corridor_X"));
		cube2.transform.localScale = scale;
		cube2.transform.position = position + (Vector3.forward * sizeObject);
		cube2.GetComponent<MeshRenderer> ().material = mat_up;
		cube2.transform.SetParent(container.transform,true);
		cube2.tag = "Fractal";

		closeOpening (cube2, scale, Vector3.forward);
		closeOpening (cube2, scale, Vector3.left);
		closeOpening (cube2, scale, Vector3.right);



		//GameObject cube3 =  GameObject.CreatePrimitive (PrimitiveType.Cube);
		GameObject cube3 = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Corridor_X"));
		cube3.transform.localScale = scale;
		cube3.transform.position = position + (Vector3.back * sizeObject);
		cube3.GetComponent<MeshRenderer> ().material = mat_down;
		cube3.transform.SetParent(container.transform,true);
		cube3.tag = "Fractal";

		closeOpening (cube3, scale, Vector3.back);
		closeOpening (cube3, scale, Vector3.left);
		closeOpening (cube3, scale, Vector3.right);
		
		//GameObject cube4 =  GameObject.CreatePrimitive (PrimitiveType.Cube);
		GameObject cube4 = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Corridor_X"));
		cube4.transform.localScale = scale;
		cube4.transform.position = position + (Vector3.left * sizeObject);
		cube4.GetComponent<MeshRenderer> ().material = mat_left;
		cube4.transform.SetParent(container.transform,true);
		cube4.tag = "Fractal";

		closeOpening (cube4, scale, Vector3.left);
		closeOpening (cube4, scale, Vector3.back);
		closeOpening (cube4, scale, Vector3.forward);

		//GameObject cube5 =  GameObject.CreatePrimitive (PrimitiveType.Cube);
		GameObject cube5 = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Corridor_X"));
		cube5.transform.localScale = scale;
		cube5.transform.position =  position +(Vector3.right * sizeObject);
		cube5.GetComponent<MeshRenderer> ().material = mat_right;
		cube5.transform.SetParent(container.transform,true);
		cube5.tag = "Fractal";

		closeOpening (cube5, scale, Vector3.right);
		closeOpening (cube5, scale, Vector3.back);
		closeOpening (cube5, scale, Vector3.forward);

		//parent.GetComponent<Renderer>().enabled = false;
		Destroy(cube1.GetComponent<Collider>());
		Destroy(cube2.GetComponent<Collider>());
		Destroy(cube3.GetComponent<Collider>());
		Destroy(cube4.GetComponent<Collider>());
		Destroy(cube5.GetComponent<Collider>());

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
				gameObj.transform.RotateAround (gameObj.gameObject.transform.parent.position, Vector3.up, 45f);
			}

		}
	}


	//Close an opening of Corridor_X with 3 prefabs (Left, Right, Center)
	private void closeOpening(GameObject obj, Vector3 scale, Vector3 direction){

		float sizeObject = obj.GetComponent<Collider> ().bounds.size.x;

		GameObject doorBlock_center = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/DoorBlock"));
		doorBlock_center.transform.localScale = scale;
		doorBlock_center.transform.position = obj.transform.position + (direction * 0.5f *sizeObject);
		doorBlock_center.transform.LookAt(obj.transform.position);
		doorBlock_center.transform.SetParent(obj.transform,true);

	}


}
