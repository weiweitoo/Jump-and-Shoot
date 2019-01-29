using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour {
	public enum GroundType{
		Normal,JumpHigh,TimeBomb,Static
	}

	public float minDistanceFromScreen = 0f;
	public float maxDistanceFromScreen = 0f;
	public float wallAdjustment = 0.5f;
	public float shakeEffect = 0.4f;
	public GroundType groundType;
	[ReadOnly] public float velocity = 1f;
	[ReadOnly] public float distance;
	[ReadOnly] public float screenWidth;
	[ReadOnly] public float angle = 0;
	[ReadOnly] private bool stepped = false;
	PlayerScript playerScript;
	void Awake(){
		playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
	}

	void Start(){
		screenWidth = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(new Vector2(Screen.width,0)).x - wallAdjustment;
		distance = Random.Range(screenWidth - minDistanceFromScreen - wallAdjustment - 0.5f,screenWidth + maxDistanceFromScreen - wallAdjustment - 0.5f);
	}

	void Update () {
		if(playerScript.isDead || groundType == GroundType.Static) return;
		Move();
	}

	void Move(){
		transform.position = new Vector2(Mathf.Sin(angle) * distance, transform.position.y);
		angle += (velocity / 1f) * Time.deltaTime;
	}

	public void SetGround(Vector2 scale,float minDistanceFromScreen, float maxDistanceFromScreen, float velocity, GroundType groundType){
		this.transform.localScale = scale;
		this.minDistanceFromScreen = minDistanceFromScreen;
		this.maxDistanceFromScreen = maxDistanceFromScreen;
		this.velocity = velocity;
		this.groundType = groundType;
	}

	public void SetColor(byte red,byte green,byte blue){
		this.transform.gameObject.GetComponent<SpriteRenderer>().color = new Color32(red,green,blue,255);
	}

	public void Stepped(){
		stepped = true;
	}

	public bool GetStepped(){
		return stepped;
	}

	public IEnumerator LandingEffect(){
		Vector2 originalPosition = transform.position;
		float yChangeValue = shakeEffect;

		while(yChangeValue > 0){
			yChangeValue -= 0.05f;
			yChangeValue = Mathf.Clamp(yChangeValue,0,shakeEffect);
			transform.position = new Vector2(transform.position.x,originalPosition.y - yChangeValue);
			yield return 0;
		}

		yield break;
	}
}
