using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManagerScript : MonoBehaviour {
	public int currentScore = 0;
	public TextMeshPro currentScoreText;
	public TextMeshPro bestScoreText;
	public TextMeshPro best;
	[ReadOnly] public float bestScore;

	void Start () {
		bestScore = PlayerStateManagerScript.instance.GetBestScore();
		bestScoreText.text = bestScore.ToString();
		currentScoreText.text = currentScore.ToString();
	}
	
	public void AddScore(){
		currentScore++;
		currentScoreText.text = currentScore.ToString();

		if(currentScore > bestScore){
			bestScoreText.text = currentScore.ToString();
			PlayerStateManagerScript.instance.SetBestScore(currentScore);
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
