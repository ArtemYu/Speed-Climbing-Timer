using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LOTbuttonScript : MonoBehaviour {
	public Button LOTbutton;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void loadTimes(){
		Application.LoadLevel ("times");
	}

	public void loadTimer(){
		Application.LoadLevel("mainScene");
	}
}
