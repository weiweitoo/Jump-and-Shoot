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
	public float minVelocity = 1f;
	public float maxVelocity = 2f;
	[ReadOnly] public int groundIndex = 0;
	
	void Start () {
		InitGround();
	}
	
	void Update () {

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
		// Random Type
		GroundScript.GroundType groundType = (GroundScript.GroundType)Random.Range(0, 3);
		float velocity = 0;
		if(groundType == GroundScript.GroundType.Normal || groundType == GroundScript.GroundType.JumpHigh){
			velocity = Random.Range(minVelocity,maxVelocity);
		}
		else if(groundType == GroundScript.GroundType.TimeBomb){
			velocity = Random.Range(minVelocity * 1.25f,maxVelocity * 1.25f);
		}

		obj.GetComponent<GroundScript>().SetGround(1,2,velocity,groundType);
	}

}
