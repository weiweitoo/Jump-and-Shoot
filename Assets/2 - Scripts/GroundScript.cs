using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour {
	public enum GroundType{
		Normal,JumpHigh,TimeBomb
	}

	public float minDistanceFromScreen = 1f;
	public float maxDistanceFromScreen = 2f;
	public float wallAdjustment = 0.5f;
	public float shakeEffect = 0.4f;
	public GroundType groundType;
	[ReadOnly] public float velocity = 1f;
	[ReadOnly] public float distance;
	[ReadOnly] public float screenWidth;
	[ReadOnly] public float angle = 0;
	PlayerScript playerScript;
	void Awake(){
		playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
	}

	void Start(){
		screenWidth = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(new Vector2(Screen.width,0)).x - wallAdjustment;
		distance = Random.Range(screenWidth - minDistanceFromScreen,screenWidth + maxDistanceFromScreen);
	}

	void Update () {
		if(playerScript.isDead) return;
		Move();
	}

	void Move(){
		transform.position = new Vector2(Mathf.Sin(angle) * distance, transform.position.y);
		angle += velocity / 100f;
	}

	public void SetGround(float minDistanceFromScreen, float maxDistanceFromScreen, float velocity, GroundType groundType){
		this.minDistanceFromScreen = minDistanceFromScreen;
		this.maxDistanceFromScreen = maxDistanceFromScreen;
		this.velocity = velocity;
		this.groundType = groundType;
	}

	public IEnumerator LandingEffect(){
		Vector2 originalPosition = transform.position;
		float yChangeValue = shakeEffect;

		while(yChangeValue > 0){
			yChangeValue -= 0.1f;
			yChangeValue = Mathf.Clamp(yChangeValue,0,shakeEffect);
			transform.position = new Vector2(transform.position.x,originalPosition.y - yChangeValue);
			yield return 0;
		}

		yield break;
	}


}
