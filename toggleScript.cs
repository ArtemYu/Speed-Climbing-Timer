using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class toggleScript : MonoBehaviour {

	public Sprite on, off;
	public Toggle toggle;

	// Use this for initialization
	void Start () {
		
	}

	public void changeSprite(){
		if (toggle.image.sprite == on) {
			toggle.image.sprite = off;
		} else {
			toggle.image.sprite = on;
		}
	
	}
}