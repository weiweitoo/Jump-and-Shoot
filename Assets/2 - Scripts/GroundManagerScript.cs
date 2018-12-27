using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManagerScript : MonoBehaviour {

	public GameObject groundPrefab;
	public GameObject groundHolder;

	public int initOffSet = -4;
	public int distanceToNextGround = 8;
	public int initialGround = 5;
	public float groundWidth = 3;
	public float groundHeight = 0.6f;

	int groundIndex = 0;
	
	void Start () {
		InitGround();
	}
	
	void Update () {
		// GenerateGround();	
	}

	void InitGround(){
		for(int i =0; i < initialGround;i++){
			GenerateGround();
		}
	}

	public void GenerateGround(){
		Vector2 position = new Vector2(0,initOffSet + groundIndex * distanceToNextGround);
		GameObject newGroundObj = Instantiate(groundPrefab,position,Quaternion.identity);
		newGroundObj.transform.SetParent(groundHolder.transform);
		newGroundObj.transform.localScale = new Vector2(groundWidth,groundHeight);

		SetSpeed(newGroundObj);

		groundIndex++;
	}

	void SetSpeed(GameObject obj){
		obj.GetComponent<GroundScript>().velocity = Random.Range(-2.5f,2.5f);
	}

}
