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
	public Coroutine countTimerCoroutine;
	public void ReviveWithAds(){
		StopCoroutine(countTimerCoroutine);
		GameObject.Find("_AudioManager").gameObject.GetComponent<AudioManagerScript>().StopDeadSound();
		GameObject.Find("UnityAds").gameObject.GetComponent<UnityAdsPlacementScript>().ShowAd();
	}
	public void ReviveWithGold(){
		StopCoroutine(countTimerCoroutine);
		Revive();
	}
	
	public void Revive(){
		gameOverPanel.SetActive(false);
		// Switch to finish game page, so that wont show again
		revivePage.SetActive(false);
		finishGamePage.SetActive(true);
		GameObject.Find("_AudioManager").gameObject.GetComponent<AudioManagerScript>().StopDeadSound();
		GameObject.Find("_GameManager").GetComponent<GameManagerScript>().Revive();
	}
	public void StartCountingGameOver(){
		countTimerCoroutine = StartCoroutine(CountTimerCoroutine());
	}

	void GameOverScreen(){
		revivePage.SetActive(false);
		finishGamePage.SetActive(true);
		GameObject.Find("_GameManager").GetComponent<GameManagerScript>().GameOver();
	}

	IEnumerator CountTimerCoroutine(){
		counterText.text = counter.ToString();
		// If dead before then ignore the 5 sec
		if(GameObject.Find("_GameManager").GetComponent<GameManagerScript>().lost == true){
			GameOverScreen();
		}

		while(true){
			yield return new WaitForSecondsRealtime(1f);
			if(--counter < 0){
				GameOverScreen();
				yield break;
			}
			counterText.text = counter.ToString();
		}
	}
}
