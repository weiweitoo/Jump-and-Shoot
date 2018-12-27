using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	public float jumpSpeed = 10f;
	public float fallSpeed = 20f;
	public float stunTime = 1f;
	public float screenWidth = 6f;
	public GameObject playerParent;
	public bool isDead = false;
	public GameObject shootEffectPrefab;

	Rigidbody2D rigidBody2DComponent;
	PlayerState currentState;
	BoxCollider2D boxCollider2D;
	float previousPosXParent;

	float hueValue;

	enum PlayerState{
		Standing,Jumping,Falling
	}
	
	void Awake () {
		rigidBody2DComponent = GetComponent<Rigidbody2D>();
		boxCollider2D = GetComponent<BoxCollider2D>();
		currentState = PlayerState.Jumping;
	}

	void Start(){
		hueValue = Random.Range(0,1f);
		ChangeBackgroundColor();
	}
	
	void Update () {
		GetInput();
		BounceAtWall();
		GetPreviousPositionOfParent();
		DeadCheck();
	}

	void GetPreviousPositionOfParent(){
		previousPosXParent = transform.parent.transform.position.x;
	}

	float ParentVelocity(){
		return (transform.parent.transform.position.x - previousPosXParent) / Time.deltaTime;
	}

	void BounceAtWall(){
		float radius = screenWidth/2;
		if(rigidBody2DComponent.position.x < -radius){
			rigidBody2DComponent.position = new Vector2(-radius,rigidBody2DComponent.position.y);
			rigidBody2DComponent.velocity = new Vector2(-rigidBody2DComponent.velocity.x,rigidBody2DComponent.velocity.y);
		}

		if(rigidBody2DComponent.position.x >= radius){
			rigidBody2DComponent.position = new Vector2(radius,rigidBody2DComponent.position.y);
			rigidBody2DComponent.velocity = new Vector2(-rigidBody2DComponent.velocity.x,rigidBody2DComponent.velocity.y);
		}
	}

	void GetInput(){
		if (Input.GetMouseButtonDown(0)){
			if(currentState == PlayerState.Standing){
				Jump();
			}
			else if(currentState == PlayerState.Jumping){
				StartCoroutine(Fall());
			}
		}
	}

	void Jump(){
		boxCollider2D.enabled = false;
		currentState = PlayerState.Jumping;

		rigidBody2DComponent.velocity = new Vector2(ParentVelocity(),jumpSpeed);
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

		currentState = PlayerState.Falling;
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

	void OnCollisionEnter2D(Collision2D target){
		rigidBody2DComponent.velocity = Vector2.zero;
		currentState = PlayerState.Standing;
		transform.SetParent(target.gameObject.transform);
		GetPreviousPositionOfParent();
		StartCoroutine(target.gameObject.GetComponent<GroundScript>().LandingEffect());
		GameObject.Find("ScoreManager").GetComponent<ScoreManagerScript>().AddScore();
	}

	void OnCollisionExit2D(Collision2D target){
		GameObject.Find("GroundManager").GetComponent<GroundManagerScript>().GenerateGround();
		Destroy(target.gameObject,0.1f);
	}
}
