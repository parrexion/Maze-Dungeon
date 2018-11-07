using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Levelcontroller : MonoBehaviour {

#region Singleton

	private static Levelcontroller instance = null;
	private void Start() {
		if (instance != null){
			Destroy(gameObject);
		}
		else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}
#endregion


	public IntVariable currentLevel;
	public IntVariable bestScore;
	public IntVariable maxLevel;

	public UnityEvent startSaveEvent;


	public void ClearLevel() {
		currentLevel.value++;
		if (bestScore.value < currentLevel.value) {
			bestScore.value = currentLevel.value;
			startSaveEvent.Invoke();
		}
		if (currentLevel.value > maxLevel.value){
			ReturnToMain();
		}
		else {
			SceneManager.LoadScene("Level " + currentLevel.value);
		}
	}

	public void ReturnToMain() {
		SceneManager.LoadScene("Main Menu");
	}

	public void RestartLevel() {
		SceneManager.LoadScene("Level " + currentLevel.value);
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.G))
			ClearLevel();
	}

}
