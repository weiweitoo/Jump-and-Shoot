using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {

	public GameObject gameOverPanel;

	void Awake(){
		Time.timeScale = 1f;
	}

	public void GameOver(){
		StartCoroutine(GameOverCoroutine());
	}

	IEnumerator GameOverCoroutine(){
		Time.timeScale = 0.1f;
		yield return new WaitForSecondsRealtime(0.5f);
		gameOverPanel.SetActive(true);
		GameObject.Find("ScoreManager").GetComponent<ScoreManagerScript>().ChangeColorToWhite();
	}

	public void Restart(){
		Debug.Log(123);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
