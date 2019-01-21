using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManagerScript : MonoBehaviour {
	public static PlayerStateManagerScript instance = null;

	/// <summary>
	/// Init for singleton
	/// </summary>
	void Awake(){
		if(instance == null){
			instance = this;
			SingletonInit();
		}
		else if (instance != null){
			Destroy(gameObject);
		}
	}

	// Code goes here...
	[ReadOnly] public int totalPoint;
	[ReadOnly] public int flashShootLevel;
	[ReadOnly] public int featherFallingLevel;

	void SingletonInit(){
		totalPoint = GetTotalPoint();
	}

	/* Total Point Getter Setter*/
	public int GetTotalPoint(){
		return PlayerPrefs.GetInt("TotalPoint",0);
	}
	public void AddTotalPoint(int obtainedPoint){
		totalPoint += obtainedPoint;
		PlayerPrefs.SetInt("TotalPoint",totalPoint);
	}
	public void SpendTotalPoint(int spentPoint){
		totalPoint -= spentPoint;
		PlayerPrefs.SetInt("TotalPoint",totalPoint);
	}

	/* Best Score Getter Setter*/
	public int GetBestScore(){
		return PlayerPrefs.GetInt("BestScore",0);
	}
	public void SetBestScore(int newBestScore){
		PlayerPrefs.SetInt("BestScore",newBestScore);
	}

	/* Best Score Getter Setter*/
	public int GetTotalScore(){
		return PlayerPrefs.GetInt("TotalScore",0);
	}
	public void SetTotalScore(int newTotalScore){
		PlayerPrefs.SetInt("TotalScore",newTotalScore);
	}

	/* Feather Falling Getter Setter */
	public int GetFeatherFalling(){
		return PlayerPrefs.GetInt("Upgrade_FeatherFalling",0);
	}
	public void AddFeatherFalling(){
		PlayerPrefs.SetInt("Upgrade_FeatherFalling",++featherFallingLevel);
	}

	/* Flash Shoot Getter Setter */
	public int GetFlashShoot(){
		return PlayerPrefs.GetInt("Upgrade_FlashShoot",0);
	}
	public void AddFlashShoot(){
		PlayerPrefs.SetInt("Upgrade_FlashShoot",++flashShootLevel);
	}
}
