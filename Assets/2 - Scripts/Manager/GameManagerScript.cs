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
	public GameObject groundHolder;
	public GameObject newGround;
	[ReadOnly] GroundScript newGroundScript;
	AudioManagerScript audioManager;
	AudioSource backgroundMusic;
	public bool lost;
	void Awake(){
		lost = false;
		Time.timeScale = 1f;
		gameState = GameState.Menu;
		audioManager = GameObject.Find("_AudioManager").GetComponent<AudioManagerScript>();
		backgroundMusic = GameObject.Find("_BackgroundMusic").GetComponent<AudioSource>();
	}

	void Update(){
		if (GameManagerScript.gameState == GameState.Menu && Input.GetMouseButtonDown(0)){
			StartCoroutine(StartGame());
		}
	}

	IEnumerator StartGame(){
		yield return new WaitForSeconds(0.2f);
		menuPanel.GetComponent<Animator>().SetBool("InMenu",false);
		// player.GetComponent<Animator>().SetBool("PlayerActive",true);
		GameManagerScript.gameState = GameState.Playing;

		yield break;
	}

	public void Dead(){
		StartCoroutine(DeadCoroutine());
	}

	public void GameOver(){
		StartCoroutine(GameOverScreenCoroutine());
	}

	public void Revive(){
		audioManager.StopDeadSound();
		backgroundMusic.Play();
		Time.timeScale = 1f;
		newGround = groundHolder.transform.GetChild(0).gameObject;
		newGroundScript = newGround.GetComponent<GroundScript>();
		newGroundScript.velocity = 0;

		player.transform.position = new Vector3(newGround.transform.position.x,newGround.transform.position.y+1,newGround.transform.position.z);
		player.GetComponent<PlayerScript>().RevivePlayer();
	}

	IEnumerator DeadCoroutine(){
		audioManager.PlayDeadSound();
		backgroundMusic.Pause();
		Time.timeScale = 0.1f;
		GameObject.Find("_ScoreManager").GetComponent<ScoreManagerScript>().UpdateTotalScore();
		yield return new WaitForSecondsRealtime(0.5f);
		gameOverPanel.SetActive(true);
		GameObject.Find("_ReviveManager").GetComponent<MyReviveManagerScript>().StartCountingGameOver();
		lost = true;
	}

	IEnumerator GameOverScreenCoroutine(){
		if(lost == true){
			GameObject.Find("_ScoreManager").GetComponent<ScoreManagerScript>().SaveScore();
		}
		yield return new WaitForSecondsRealtime(0.5f);
		gameOverPanel.SetActive(true);
		GameObject.Find("_ScoreManager").GetComponent<ScoreManagerScript>().ChangeColorToWhite();
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
