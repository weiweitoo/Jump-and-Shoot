using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MyReviveManagerScript : MonoBehaviour {

	public GameObject gameOverPanel;
	public GameObject revivePage;
	public GameObject finishGamePage;
	public TextMeshPro counterText;

	public int counter = 10;

	public void ReviveWithAds(){
		Debug.Log("Revive with ads");
		Revive();
	}
	public void ReviveWithGold(){
		Debug.Log("Revive with gold");
		Revive();
	}
	
	void Revive(){
		gameOverPanel.SetActive(false);
		// Switch to finish game page, so that wont show again
		revivePage.SetActive(false);
		finishGamePage.SetActive(true);
		GameObject.Find("_GameManager").GetComponent<GameManagerScript>().Revive();
	}
	public void StartCountingGameOver(){
		StartCoroutine(CountTimerCoroutine());
	}

	IEnumerator CountTimerCoroutine(){
		counterText.text = counter.ToString();
		while(true){
			yield return new WaitForSecondsRealtime(1);
			counter -= 1;
			counterText.text = counter.ToString();
			Debug.Log(counter);
			if(counter < 0){
				GameObject.Find("_GameManager").GetComponent<GameManagerScript>().GameOver();
				yield break;
			}
		}
	}
}
