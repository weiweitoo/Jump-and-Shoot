using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	public enum PlayerState{
		Standing,Jumping,Falling
	}
	
	public GameObject playerParent;
	public GameObject shootEffectPrefab;
	public float jumpSpeed = 10f;
	public float alwaysLeftSpeed = 3f;
	public float alwaysRightSpeed = 3f;
	public float fallSpeed = 20f;
	public float stunTime = 1f;
	public float wallAdjustment;
	[ReadOnly] public bool isDead = false;
	[ReadOnly] public float previousPosXParent;
	[ReadOnly] public float ScreenRadiusInWorldX;
	[ReadOnly] public float hueValue;
	[ReadOnly] public PlayerState currentPlayerState;
	[ReadOnly] public GroundScript.GroundType standingGroundType;
	
	Rigidbody2D rigidBody2DComponent;
	BoxCollider2D boxCollider2D;
	
	void Awake () {
		rigidBody2DComponent = GetComponent<Rigidbody2D>();
		boxCollider2D = GetComponent<BoxCollider2D>();
		currentPlayerState = PlayerState.Jumping;
	}

	void Start(){
		hueValue = Random.Range(0,1f);
		ChangeBackgroundColor();
	}
	
	void Update () {
		if(GameManagerScript.isPlaying()){
			GetInput();
			BounceAtWall();
			GetPreviousPositionOfParent();
			DeadCheck();
		}
	}

	void OnCollisionEnter2D(Collision2D target){
		// Get Standing Groud Type
		standingGroundType = target.gameObject.GetComponent<GroundScript>().groundType;

		rigidBody2DComponent.velocity = Vector2.zero;
		currentPlayerState = PlayerState.Standing;
		transform.SetParent(target.gameObject.transform);
		GetPreviousPositionOfParent();
		StartCoroutine(target.gameObject.GetComponent<GroundScript>().LandingEffect());
		GameObject.Find("ScoreManager").GetComponent<ScoreManagerScript>().AddScore();
	}

	IEnumerator OnCollisionExit2D(Collision2D target){
		yield return new WaitForSeconds(0.1f);
		
		if(currentPlayerState == PlayerState.Jumping){
			GameObject.Find("GroundManager").GetComponent<GroundManagerScript>().GenerateGround();
			Destroy(target.gameObject,0.1f);
		}
		yield break;
	}

	void GetPreviousPositionOfParent(){
		previousPosXParent = transform.parent.transform.position.x;
	}

	float ParentVelocity(){
		return (transform.parent.transform.position.x - previousPosXParent) / Time.deltaTime;
	}

	void BounceAtWall(){
		float screenWidth = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(new Vector2(Screen.width,0)).x - wallAdjustment;
		float transformX = rigidBody2DComponent.position.x;

		if(transformX < -screenWidth){
			rigidBody2DComponent.position = new Vector2(-screenWidth,rigidBody2DComponent.position.y);
			rigidBody2DComponent.velocity = new Vector2(-rigidBody2DComponent.velocity.x,rigidBody2DComponent.velocity.y);
		}

		if(transformX >= screenWidth){
			rigidBody2DComponent.position = new Vector2(screenWidth,rigidBody2DComponent.position.y);
			rigidBody2DComponent.velocity = new Vector2(-rigidBody2DComponent.velocity.x,rigidBody2DComponent.velocity.y);
		}
	}

	void GetInput(){
		if (Input.GetMouseButtonDown(0)){
			if(currentPlayerState == PlayerState.Jumping){
				StartCoroutine(Fall());
			}
			else{
				Jump();
			}
		}
	}

	void Jump(){
		boxCollider2D.enabled = false;
		currentPlayerState = PlayerState.Jumping;

		if(standingGroundType == GroundScript.GroundType.JumpHigh){
			rigidBody2DComponent.velocity = new Vector2(ParentVelocity(),jumpSpeed * 1.27f);
		}else{
			rigidBody2DComponent.velocity = new Vector2(ParentVelocity(),jumpSpeed);
		}

		transform.SetParent(playerParent.transform);
	}

	void DeadCheck(){
		if(isDead == false && Camera.main.transform.position.y - transform.position.y > 10){
			isDead = true;
			StopPlayer();
			GameObject.Find("GameManager").GetComponent<GameManagerScript>().GameOver();
		}
	}

	void StopPlayer(){
		rigidBody2DComponent.isKinematic = true;
		rigidBody2DComponent.velocity = new Vector2(0,0);
	}

	IEnumerator Fall(){
		GameObject shootEffect = Instantiate(shootEffectPrefab,transform.position, Quaternion.identity);
		shootEffect.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.HSVToRGB(hueValue,0.6f,0.6f);

		currentPlayerState = PlayerState.Falling;
		boxCollider2D.enabled = true;

		rigidBody2DComponent.isKinematic = true;
		rigidBody2DComponent.velocity = new Vector2(0,0);

		yield return new WaitForSeconds(stunTime);

		rigidBody2DComponent.isKinematic = false;
		rigidBody2DComponent.velocity = new Vector2(0,-fallSpeed);
		Destroy(shootEffect);
		ChangeBackgroundColor();
		yield break;
	}

	void ChangeBackgroundColor(){
		Camera.main.backgroundColor = Color.HSVToRGB(hueValue,0.6f,0.6f);
		hueValue += Random.Range(0.1f,0.2f);
		if(hueValue >= 1f){
			hueValue = 0;
		}
	}

}
