using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManagerScript : MonoBehaviour {

	public int currentScore = 0;
	public TextMeshPro currentScoreText;
	public TextMeshPro bestScoreText;
	public TextMeshPro best;

	void Start () {
		bestScoreText.text = PlayerPrefs.GetInt("BestScore",0).ToString();
		currentScoreText.text = currentScore.ToString();
	}
	
	public void AddScore(){
		currentScore++;
		currentScoreText.text = currentScore.ToString();

		if(currentScore > PlayerPrefs.GetInt("BestScore",0)){
			bestScoreText.text = currentScore.ToString();
			PlayerPrefs.SetInt("BestScore",currentScore);
		}
	}

	public void ChangeColorToWhite(){
		bestScoreText.color = Color.white;
		currentScoreText.color = Color.white;
		best.color = Color.white;
	}

	public void ShowBestScoreOnly(){
		currentScoreText.enabled = false;
		bestScoreText.enabled = true;
		best.enabled = true;
	}

	public void ShowBestScoreAndCurrScore(){
		currentScoreText.enabled = true;
		bestScoreText.enabled = true;
		best.enabled = true;
	}
}
