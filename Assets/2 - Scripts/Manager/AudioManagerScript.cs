using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour {

	public AudioClip JumpSound;
	public AudioClip CoinSound;
	AudioSource audioSourceComponent;

	void Awake(){
		audioSourceComponent = GetComponent<AudioSource>();
	}

	public void PlayJumpSound(){
		audioSourceComponent.PlayOneShot(JumpSound,1f);
	}

	public void PlayCoinSound(){
		audioSourceComponent.PlayOneShot(CoinSound,1f);
	}
}
