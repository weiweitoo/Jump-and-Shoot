using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	public enum PlayerState{
		Standing,Jumping,Falling
	}
	
	public GameObject playerParent;
	public GameObject shootEffectPrefab;
	public GameObject landingEffectPrefab;
	public GameObject wallEffectPrefab;
	public float jumpSpeed = 10f;
	public float alwaysLeftSpeed = 3f;
	public float alwaysRightSpeed = 3f;
	public float throwSpeed = 1.5f;
	public float fallSpeed = 20f;
	public float stunTime = 1f;
	public float wallAdjustment;
	[ReadOnly] public bool isDead = false;
	[ReadOnly] public float previousPosXParent;
	[ReadOnly] public float ScreenRadiusInWorldX;
	[ReadOnly] public float hueValue;
	[ReadOnly] public PlayerState currentPlayerState;
	[ReadOnly] public GroundScript.GroundType standingGroundType;
	[ReadOnly] public bool initialCollide;
	[ReadOnly] public bool allowSlowMotion;
	Rigidbody2D rigidBody2DComponent;
	BoxCollider2D boxCollider2D;
	
	void Awake () {
		rigidBody2DComponent = GetComponent<Rigidbody2D>();
		boxCollider2D = GetComponent<BoxCollider2D>();
		currentPlayerState = PlayerState.Jumping;
	}

	void Start(){
		initialCollide = true;
		hueValue = Random.Range(0,1f);
		ChangeBackgroundColor();
	}
	
	void Update () {
		if(GameManagerScript.isPlaying()){
			GetInput();
			SlowMotion();
			BounceAtWall();
			GetPreviousPositionOfParent();
			DeadCheck();
		}
	}

	void OnCollisionEnter2D(Collision2D target){
		// Get Standing Groud Type
		GroundScript groundScriptComponent = target.gameObject.GetComponent<GroundScript>();
		standingGroundType = groundScriptComponent.groundType;

		rigidBody2DComponent.velocity = Vector2.zero;
		currentPlayerState = PlayerState.Standing;
		transform.SetParent(target.gameObject.transform);
		GetPreviousPositionOfParent();
		if(initialCollide == false){
			if(groundScriptComponent.GetStepped() == false){
				groundScriptComponent.Stepped();
				GameObject.Find("_ScoreManager").GetComponent<ScoreManagerScript>().AddScore();
			}
			
			GameObject.Find("_AudioManager").GetComponent<AudioManagerScript>().PlayCoinSound();
			StartCoroutine(target.gameObject.GetComponent<GroundScript>().LandingEffect());
			GameObject landingEffect = Instantiate(landingEffectPrefab,transform.position, Quaternion.identity);
			Destroy(landingEffect,0.1f);
		}
		else
		{
			initialCollide = false;
		}
	}

	IEnumerator OnCollisionExit2D(Collision2D target){
		yield return new WaitForSeconds(0.1f);
		
		if(currentPlayerState == PlayerState.Jumping){
			GameObject.Find("_GroundManager").GetComponent<GroundManagerScript>().GenerateGround();
			Destroy(target.gameObject,0.05f);
		}
		yield break;
	}

	void GetPreviousPositionOfParent(){
		previousPosXParent = transform.parent.transform.position.x;
	}

	float ParentVelocity(){
		return (transform.parent.transform.position.x - previousPosXParent) * throwSpeed / Time.deltaTime;
	}

	void BounceAtWall(){
		float screenWidth = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(new Vector2(Screen.width,0)).x - wallAdjustment;
		float transformX = rigidBody2DComponent.position.x;

		if(transformX < -screenWidth){
			rigidBody2DComponent.position = new Vector2(-screenWidth,rigidBody2DComponent.position.y);
			rigidBody2DComponent.velocity = new Vector2(-rigidBody2DComponent.velocity.x,rigidBody2DComponent.velocity.y);
			PlayWallBounceEffect();
		}

		if(transformX >= screenWidth){
			rigidBody2DComponent.position = new Vector2(screenWidth,rigidBody2DComponent.position.y);
			rigidBody2DComponent.velocity = new Vector2(-rigidBody2DComponent.velocity.x,rigidBody2DComponent.velocity.y);
			PlayWallBounceEffect();
		}
	}

	void GetInput(){
		if (Input.GetMouseButtonDown(0)){
			if(currentPlayerState == PlayerState.Jumping){
				StartCoroutine(Fall());
			}
			else if(currentPlayerState == PlayerState.Standing){
				Jump();
			}
		}
	}

	void Jump(){
		GameObject.Find("_AudioManager").GetComponent<AudioManagerScript>().PlayJumpSound();
		boxCollider2D.enabled = false;
		currentPlayerState = PlayerState.Jumping;
	
		if(standingGroundType == GroundScript.GroundType.JumpHigh){
			rigidBody2DComponent.velocity = new Vector2(ParentVelocity(),jumpSpeed * 1.08f);
		}else{
			rigidBody2DComponent.velocity = new Vector2(ParentVelocity(),jumpSpeed);
		}
		transform.SetParent(playerParent.transform);
	}

	void DeadCheck(){
		if(isDead == false && Camera.main.transform.position.y - transform.position.y > 10){
			isDead = true;
			StopPlayer();
			GameObject.Find("_GameManager").GetComponent<GameManagerScript>().Dead();
		}
	}

	void StopPlayer(){
		rigidBody2DComponent.isKinematic = true;
		rigidBody2DComponent.velocity = new Vector2(0,0);
	}

	public void RevivePlayer(){
		ContinuePlayer();
		isDead = false;
	}

	void ContinuePlayer(){
		rigidBody2DComponent.isKinematic = false;
	}

	void PlayWallBounceEffect(){
		if(currentPlayerState == PlayerState.Jumping){
			GameObject wallEffect = Instantiate(wallEffectPrefab,transform.position, Quaternion.identity);
			Destroy(wallEffect,0.2f);
		}
	}

	void SlowMotion(){
		if (allowSlowMotion == true && Input.GetMouseButton(0)){
			// TODO, this will caused laggy
			Time.timeScale = 0.05f;
			Time.fixedDeltaTime = 0.02F * Time.timeScale;
		}
		else{
			NormalMotion();
		}
	}

	void NormalMotion(){
		Time.timeScale = 1;
    	Time.fixedDeltaTime = 0.02F ;
	}

	IEnumerator Fall(){
		GameObject shootEffect = Instantiate(shootEffectPrefab,transform.position, Quaternion.identity);
		shootEffect.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.HSVToRGB(hueValue,0.6f,0.6f);

		currentPlayerState = PlayerState.Falling;
		boxCollider2D.enabled = true;

		rigidBody2DComponent.isKinematic = true;
		rigidBody2DComponent.velocity = new Vector2(0,0);

		allowSlowMotion = true;

		yield return new WaitForSeconds(stunTime);

		// Revert back
		allowSlowMotion = false;
		NormalMotion();

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
