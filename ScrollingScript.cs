using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
public class ScrollingScript : MonoBehaviour {

	public RectTransform prefab;
	public RectTransform content;
	private String path;
	public Button deleteAllButton;
	public Save sv = new Save ();
	private bool changed = true;
	private int del;
	int prev = -1;

	public class TimeView {

		public Text FirstText;
		public Text SecondText;
		public Button deleteButton;

		public TimeView(Transform rootView){
			FirstText = rootView.Find("FirstText").GetComponent<Text>();
			SecondText = rootView.Find("SecondText").GetComponent<Text>();
			deleteButton = rootView.Find("DeleteButton").GetComponent<Button>();
			}


	}


	void Start(){
		#if UNITY_ANDROID && !UNITY_EDITOR
		path = Path.Combine(Application.persistentDataPath, "Save.json");
		#else 
		path = Path.Combine(Application.dataPath, "Save.json");
		#endif

		Button btn = deleteAllButton.GetComponent<Button> ();
		btn.onClick.AddListener (deleteAll);

		if (File.Exists(path))
		{
			sv = JsonUtility.FromJson<Save>(File.ReadAllText(path));

		}



	}

	public void deleteAll(){
		
		File.Delete (path);
		sv.firstTimerList.Clear ();
		sv.secondTimerList.Clear();
		changed = true;
	}


	void OnReceivedTimes(TimeRecord[] records){

		foreach (Transform child in content) {
			Destroy (child.gameObject);
		}

		foreach (var record in records){
			var instance = GameObject.Instantiate (prefab.gameObject) as GameObject;
			instance.transform.SetParent (content, false);
			InitializeTimeRecordView (instance, record);
		}

	}



	void InitializeTimeRecordView(GameObject viewGameObject, TimeRecord record){
		TimeView view = new TimeView (viewGameObject.transform);
		view.FirstText.text = record.FirstTimerString;
		view.SecondText.text = record.SecondTimerString;

		view.deleteButton.onClick.AddListener (
			() =>
			{
				del = record.Index;
				deleteThisTime(del);
				//Debug.Log("pri");
			
			}

		);
		}


	public void deleteThisTime(int i){
		
		sv.firstTimerList.RemoveAt (i);
		sv.secondTimerList.RemoveAt (i);
		File.WriteAllText(path, JsonUtility.ToJson(sv));
		changed = true;

		//Debug.Log ("privet");

	}



	public TimeRecord[] GetTimes(){
		
		List<string> FirstTimes = sv.firstTimerList;
		List<string> SecondTimes = sv.secondTimerList;


		var results = new TimeRecord[FirstTimes.Count];
		if (prev != FirstTimes.Count) {
			for (int i = 0; i < FirstTimes.Count; i++) {
				
				results [i] = new TimeRecord ();
				results [i].FirstTimerString = FirstTimes [i]; 
				results [i].SecondTimerString = SecondTimes [i]; 
				results [i].Index = i;

			}
			changed = true;
			prev = FirstTimes.Count;
		}

		return results;
	}
	void Update(){
		if (changed) {
			//Debug.Log ("sdasd");
			OnReceivedTimes (GetTimes ());
			changed = false;
		}

	}


	public class TimeRecord{
		public string FirstTimerString;
		public string SecondTimerString;
		public int Index;
	}
}
