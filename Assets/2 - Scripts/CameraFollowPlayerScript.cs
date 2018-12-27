using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayerScript : MonoBehaviour {

	public GameObject player;
	public int yOffSet = 5;
	float smooth = 0.3f;
	Vector2 velocity = Vector2.zero;

	
	void Start () {
			
	}
	
	void Update () {	
		Follow();
	}

	void Follow(){
		Vector2 targetPosition = new Vector2(0, player.transform.position.y + yOffSet);
		if(targetPosition.y < transform.position.y) return;

		transform.position = Vector2.SmoothDamp(transform.position,targetPosition,ref velocity,smooth);
		transform.position = new Vector3(transform.position.x,transform.position.y,-10);
	}	
}
