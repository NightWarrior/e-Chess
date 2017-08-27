using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour {
	public GameObject canvas;
	public bool AIGame = false;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void StartGame(string name){
		PlayerPrefs.SetInt ("AIGame", 0);
		SceneManager.LoadScene (name);
	}

	public void StartAIGame(string name){
		PlayerPrefs.SetInt ("AIGame", 1);
		SceneManager.LoadScene (name);
	}

	public void ExitGame(){
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else 
		Application.Exit;
		#endif 

	}
}
