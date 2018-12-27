using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour {

	public float velocity = 1f;
	public float distance = 3f;
	public float angle = 0;
	public float shakeEffect = 0.4f;


	PlayerScript playerScript;
	void Awake(){
		playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
	}

	void Update () {
		if(playerScript.isDead) return;
		Move();
	}

	void Move(){
		transform.position = new Vector2(Mathf.Sin(angle) * distance, transform.position.y);
		angle += velocity / 100f;
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
