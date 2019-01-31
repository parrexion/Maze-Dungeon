using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MiddleDisplayController : MonoBehaviour {

	public BoolVariable paused;
	public BoolVariable lockControls;

	[Header("Game End")]
	public GameObject gameEndScreen;
	public Text gameOverText;

	[Header("Pause Menu")]
	public GameObject pauseButtonsView;
	public MyButton[] pauseButtons;
	private int pauseIndex;

	[Header("Events")]
	public UnityEvent mainMenuEvent;
	public UnityEvent nextLevelEvent;
	public UnityEvent restartLevelEvent;


	private void Start () {
		gameEndScreen.SetActive(false);
		pauseButtonsView.SetActive(false);
		gameOverText.text = "";
	}

	public void ShowPauseMenu() {
		Debug.Log("Show Pause");
		paused.value = true;
		pauseIndex = 0;
		UpdateButtons();
		//lockControls.value = true;
		pauseButtonsView.SetActive(true);
	}

	public void HidePauseMenu() {
		Debug.Log("Hide Pause");
		paused.value = false;
		//lockControls.value = false;
		pauseButtonsView.SetActive(false);
	}

	public void ReturnToMain() {
		Debug.Log("Go to main");
		mainMenuEvent.Invoke();
	}

	public void GameOver() {
		gameEndScreen.SetActive(true);
		gameOverText.text = "YOU DIED!";
		paused.value = true;
		lockControls.value = true;
		StartCoroutine(GameOverDelay(false));
	}

	public IEnumerator GameOverDelay(bool win) {
		yield return new WaitForSeconds(2f);
		if (win)
			nextLevelEvent.Invoke();
		else
			restartLevelEvent.Invoke();
	}

	public void OnReachedGoal() {
		gameEndScreen.SetActive(true);
		gameOverText.text = "You Win!";
		paused.value = true;
		lockControls.value = true;
		StartCoroutine(GameOverDelay(true));
	}

	
	//////
	/// INPUT
	//////

	public void UpdateButtons() {
		for (int i = 0; i < pauseButtons.Length; i++) {
			pauseButtons[i].SetHighlight(i == pauseIndex);
		}
	}

	public void OnOK() {
		if (!paused.value)
			return;
		pauseButtons[pauseIndex].Click();
	}

	public void OnBack() {
		if (!paused.value)
			return;
		HidePauseMenu();
	}

	public void OnUp() {
		if (!paused.value)
			return;
		pauseIndex = OPMath.FullLoop(0, pauseButtons.Length-1, pauseIndex -1);

		UpdateButtons();
	}

	public void OnDown() {
		if (!paused.value)
			return;
		pauseIndex = OPMath.FullLoop(0, pauseButtons.Length-1, pauseIndex +1);

		UpdateButtons();
	}
	
}
