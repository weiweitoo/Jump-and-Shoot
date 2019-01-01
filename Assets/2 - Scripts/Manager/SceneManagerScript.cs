using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour {

	public void GoToGame(){
		SceneManager.LoadScene("MainScene");
	}

	public void GoToMenu(){
		SceneManager.LoadScene("Menu");
	}
}
