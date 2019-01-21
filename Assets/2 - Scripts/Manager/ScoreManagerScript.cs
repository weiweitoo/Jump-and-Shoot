using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManagerScript : MonoBehaviour {
	public int currentScore = 0;
	public int totalScore = 0;
	public TextMeshPro totalScoreText;
	public TextMeshPro currentScoreText;
	public TextMeshPro bestScoreText;
	public TextMeshPro best;
	[ReadOnly] public float bestScore;

	void Start () {
		bestScore = PlayerStateManagerScript.instance.GetBestScore();
		totalScore = PlayerStateManagerScript.instance.GetTotalScore();
		bestScoreText.text = bestScore.ToString();
		currentScoreText.text = currentScore.ToString();
	}

	public void UpdateTotalScore(){
		totalScoreText.text = totalScore.ToString() + " Gold Left";
	}

	public void SaveScore(){
		totalScore += currentScore;
		Debug.Log("Add " + currentScore);
		PlayerStateManagerScript.instance.SetTotalScore(totalScore);
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
