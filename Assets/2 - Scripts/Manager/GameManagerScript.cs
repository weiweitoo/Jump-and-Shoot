using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {

	public enum GameState{
		Menu,Playing,Shop
	};
	public static GameState gameState;
	public GameObject gameOverPanel;
	public GameObject menuPanel;
	public GameObject player;

	void Awake(){
		Time.timeScale = 1f;
		gameState = GameState.Menu;
	}

	void Update(){
		if (GameManagerScript.gameState == GameState.Menu && Input.GetMouseButtonDown(0)){
			StartCoroutine(StartGame());
		}
	}

	IEnumerator StartGame(){
		yield return new WaitForSeconds(0.2f);
		menuPanel.GetComponent<Animator>().SetBool("InMenu",false);
		player.GetComponent<Animator>().SetBool("PlayerActive",true);
		GameManagerScript.gameState = GameState.Playing;

		yield break;
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

	public void RestartGame(){
		Scene scene = SceneManager.GetActiveScene(); 
		SceneManager.LoadScene(scene.name);
	}

	public static bool isPlaying(){
		return GameManagerScript.gameState == GameManagerScript.GameState.Playing;
	}

	public static bool isMenu(){
		return GameManagerScript.gameState == GameManagerScript.GameState.Menu;
	}

	public static bool isShop(){
		return GameManagerScript.gameState == GameManagerScript.GameState.Shop;
	}
}
