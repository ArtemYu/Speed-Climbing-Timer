using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;


public class TimerScript : MonoBehaviour {
	public Text TimerText;
	public Text firstTimeText;
	public Button startButton;

	public Toggle toggle;
	private float startTime;
	public bool isRunning = false;
	public bool soundPlayed = false;
	public Sprite start, stop;
	public AudioSource audio;
	public bool firstIsDone = false;
	private bool showed = false;
	private bool secondIsDone = false;
	private string min1 = " ";
	private string sec1= " ";
	private Save sv = new Save();
	private string path;

	private float duration;






	// Use this for initialization
	void Start () {
		Button btn = startButton.GetComponent<Button> ();
		btn.onClick.AddListener (TaskOnClick);
		#if UNITY_ANDROID && !UNITY_EDITOR
		path = Path.Combine(Application.persistentDataPath, "Save.json");
		#else 
		path = Path.Combine(Application.dataPath, "Save.json");
		#endif

		if (File.Exists(path))
		{
			sv = JsonUtility.FromJson<Save>(File.ReadAllText(path));

		}



	}



	public void changeSprite(){
		if (!toggle.isOn) {
			if (startButton.image.sprite == start) {
				startButton.image.sprite = stop;
				TimerText.text = "0:0.00";
			} else {
				startButton.image.sprite = start;
				if (!soundPlayed) {
					audio.Stop ();
				}
			}
		} else {
			if (startButton.image.sprite == start) {
				startButton.image.sprite = stop;
				TimerText.text = "0:0.00";
				firstTimeText.text = "0:0.00";

			} else if (secondIsDone && startButton.image.sprite == stop){
				startButton.image.sprite = start;
				if (!soundPlayed) {
					audio.Stop ();
				}
			}
		}
	}


	public void showText(){
		if (toggle.isOn) {
			firstTimeText.text = "0:0.00";
		} else {
			firstTimeText.text = " ";
		}

	}




	public void playSound(){
		if (!toggle.isOn) {
			soundPlayed = false;
			if (isRunning) {
				audio.Play ();
				duration = audio.clip.length;
				StartCoroutine (waitForSound ());
			}
		} else {
				if (isRunning && !firstIsDone) {
				soundPlayed = false;
				audio.Play ();
				duration = audio.clip.length;
				StartCoroutine (waitForSound ());
			}
		}
	}




	IEnumerator waitForSound(){
		yield return new WaitForSeconds (duration);
		soundPlayed = true;
		startTime = Time.time;
	 
		}

	public List<String> getFirstList(){
		return sv.firstTimerList;
	}

	public List<String> getSecondList(){
		return sv.secondTimerList;
	}



	public void setDeafault(){
		TimerText.text = "0:0.00";

	}

	public void setRunning(){

		if (!toggle.isOn) {
			if (!isRunning) {
				firstTimeText.text = " ";
				isRunning = true;
				toggle.interactable = false;
			} else {
				isRunning = false;
				toggle.interactable = true;
				if (TimerText.text != "0:0.00") {
					sv.firstTimerList.Add (TimerText.text);
					sv.secondTimerList.Add (" ");
					File.WriteAllText (path, JsonUtility.ToJson (sv));
				}
			}
		} else {
			if (!isRunning) {
				firstTimeText.text = "0:0.00";
				toggle.interactable = false;
				showed = false;
				firstIsDone = false;
				secondIsDone = false;
				isRunning = true;
			} else if (isRunning && !firstIsDone) {
				firstIsDone = true;
				showed = true;
			} else if (isRunning && firstIsDone) {
				secondIsDone = true;
				isRunning = false;
				showed = false;
				toggle.interactable = true;
				if (firstTimeText.text != "0:0.00") {
					sv.secondTimerList.Add (TimerText.text);
					sv.firstTimerList.Add (firstTimeText.text);
					File.WriteAllText (path, JsonUtility.ToJson (sv));
				}


			}

		}



	}
	private void TaskOnClick(){
		if (!toggle.isOn) {
			if (isRunning && soundPlayed) {
				startTime = Time.time;
			}
		}
		else if (isRunning && soundPlayed && !firstIsDone) {
			startTime = Time.time;
		}
		}


	
	// Update is called once per frame
	void Update () {

		if (toggle.isOn && isRunning && soundPlayed && !audio.isPlaying && !secondIsDone) {
			float t = Time.time - startTime;

			string minutes = ((int)t / 60).ToString ();
			string seconds = (t % 60).ToString ("f2");

			TimerText.text = minutes + ":" + seconds;


			min1 = minutes;
			sec1 = seconds;
			if (!firstIsDone) {
				firstTimeText.text = min1 + ":" + sec1;
			} 



		} else if (isRunning && soundPlayed && !audio.isPlaying && !toggle.isOn) {
			float t = Time.time - startTime;

			string minutes = ((int)t / 60).ToString ();
			string seconds = (t % 60).ToString ("f2");

			TimerText.text = minutes + ":" + seconds;
		} 

		
	}
	#if UNITY_ANDROID && !UNITY_EDITOR
    private void OnApplicationPause(bool pause)
    {
        if (pause) File.WriteAllText(path, JsonUtility.ToJson(sv));
    }
#endif
    private void OnApplicationQuit()
    {
        File.WriteAllText(path, JsonUtility.ToJson(sv));
    }

}
	

	[Serializable]
	public class Save{
		public List<String> firstTimerList = new List<String> ();
		public List<String> secondTimerList = new List<String> ();

	}


