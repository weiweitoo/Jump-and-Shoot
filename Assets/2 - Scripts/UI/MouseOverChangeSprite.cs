using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverChangeSprite : MonoBehaviour {

	public Sprite newSprite;

	void OnMouseOver()
	{
		transform.GetComponent<SpriteRenderer>().sprite = newSprite;
		Debug.Log("changing sprite");
	}
}
