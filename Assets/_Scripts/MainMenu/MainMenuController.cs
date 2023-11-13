using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	public IntVariable currentLevelIndex;
	public GameObject mainMenu;
	public GameObject levelSelect;
	public GameObject controllerSelect;
	public IntVariable musicVolume;
	private AudioController audioController;

	[Header("Main menu")]
	public BoolVariable lockControls;
	public BoolVariable isSinglePlayer;
	public GameObject backgroundSingle;
	public GameObject backgroundMulti;
	public GameObject exitButton;

	[Header("Level select")]
	public Transform levelParent;
	public Transform buttonTemplate;
	public IntVariable maxLevel;
	public IntVariable bestScore;
	private List<LevelButton> levelButtons = new List<LevelButton>();

	[Header("Controller Setup")]
	public ControllerScheme[] schemePool;
	public SchemeReference player1ControllerScheme;
	public SchemeReference player2ControllerScheme;
	public GameObject player2Side;
	public GameObject scheme1SwapModeEntry;
	[Space(10)]
	public Text scheme1Text;
	public Image scheme1Icon;
	public IntVariable p1Index;
	public Text scheme1TextMovement;
	public Text scheme1TextCharacters;
	public Text scheme1TextAction;
	public Text scheme1TextPause;
	public Text scheme1TextMode;
	[Space(10)]
	public Text scheme2Text;
	public Image scheme2Icon;
	public IntVariable p2Index;
	public Text scheme2TextMovement;
	public Text scheme2TextCharacters;
	public Text scheme2TextAction;
	public Text scheme2TextPause;

	[Header("Menu Controls")]
	public MyButton[] mainButtons;
	public MyButton musicSlider;
	public MyButton changeButton;
	public Slider musicSliderSlider;
	private int menuMode;
	private int currentIndex;
	private int levelIndex;

	[Header("Events")]
	public UnityEvent nextLevelEvent;
	public UnityEvent startSaveEvent;


	private void Start() {
		currentLevelIndex.value = 0;
		mainMenu.SetActive(true);
		levelSelect.SetActive(false);
		controllerSelect.SetActive(false);
		SetupMainMenu();
		SetupControllers();
		CreateLevelButtons();

		menuMode = 0;
		currentIndex = 0;
		UpdateButtons();
		audioController = FindObjectOfType<AudioController>();
		musicSliderSlider.value = musicVolume.value;
		//Debug.Log("Best score: " + bestScore.value);
#if UNITY_WEBGL
		System.Array.Resize(ref mainButtons, mainButtons.Length - 1);
		exitButton.SetActive(false);
#endif
	}

	private void SetupMainMenu() {
		backgroundSingle.SetActive(isSinglePlayer.value);
		backgroundMulti.SetActive(!isSinglePlayer.value);
	}

	public void CreateLevelButtons() {
		for (int i = 0; i < maxLevel.value; i++) {
			Transform t = Instantiate(buttonTemplate, levelParent);
			LevelButton lb = t.GetComponent<LevelButton>();
			lb.index = i;
			lb.text.text = (i + 1).ToString();
			lb.button.interactable = (bestScore.value > i);
			levelButtons.Add(lb);
		}
		buttonTemplate.gameObject.SetActive(false);
	}

	private void SetupControllers() {
		scheme1Text.text = schemePool[p1Index.value].schemeName;
		scheme1Icon.sprite = schemePool[p1Index.value].schemeIcon;
		scheme1TextMovement.text = schemePool[p1Index.value].descButtonDir;
		scheme1TextCharacters.text = schemePool[p1Index.value].descCharacterSel;
		scheme1TextAction.text = schemePool[p1Index.value].GetKeyName("%ACTION");
		scheme1TextPause.text = schemePool[p1Index.value].GetKeyName("%PAUSE");
		if (isSinglePlayer.value) {
			player2Side.SetActive(false);
			scheme1SwapModeEntry.SetActive(true);
			scheme1TextMode.text = schemePool[p1Index.value].GetKeyName("%SWITCH");
		}
		else {
			player2Side.SetActive(true);
			scheme1SwapModeEntry.SetActive(false);

			scheme2Text.text = schemePool[p2Index.value].schemeName;
			scheme2Icon.sprite = schemePool[p2Index.value].schemeIcon;
			scheme2TextMovement.text = schemePool[p2Index.value].descButtonDir;
			scheme2TextCharacters.text = schemePool[p2Index.value].descCharacterSel;
			scheme2TextAction.text = schemePool[p2Index.value].GetKeyName("%ACTION");
			scheme2TextPause.text = schemePool[p2Index.value].GetKeyName("%PAUSE");
		}
		player1ControllerScheme.value = schemePool[p1Index.value];
		player2ControllerScheme.value = schemePool[p2Index.value];
	}

	public void ChangeControllerP1(int dir) {
		if (isSinglePlayer.value) {
			int nextIndex = OPMath.FullLoop(0, schemePool.Length - 1, p1Index.value + dir);
			if (nextIndex == p2Index.value)
				p2Index.value = p1Index.value;
			p1Index.value = nextIndex;
		}
		else {
			do {
				p1Index.value = OPMath.FullLoop(0, schemePool.Length - 1, p1Index.value + dir);
			} while (p1Index.value == p2Index.value);
		}
		SetupControllers();
	}

	public void ChangeControllerP2(int dir) {
		do {
			p2Index.value = OPMath.FullLoop(0, schemePool.Length - 1, p2Index.value + dir);
		} while (p1Index.value == p2Index.value);
		SetupControllers();
	}

	public void StartGame() {
		lockControls.value = false;
		nextLevelEvent.Invoke();
	}

	public void TogglePlayerCount() {
		isSinglePlayer.value = !isSinglePlayer.value;
		SetupMainMenu();
		SetupControllers();
	}

	public void GoToMainMenu() {
		menuMode = 0;
		mainMenu.SetActive(true);
		levelSelect.SetActive(false);
		controllerSelect.SetActive(false);
		UpdateButtons();
	}

	public void GoToLevelSelect() {
		menuMode = 1;
		levelIndex = 0;
		mainMenu.SetActive(false);
		levelSelect.SetActive(true);
		controllerSelect.SetActive(false);
		UpdateButtons();
	}

	public void GoToControllerSelect() {
		menuMode = 2;
		mainMenu.SetActive(false);
		levelSelect.SetActive(false);
		controllerSelect.SetActive(true);
		UpdateButtons();
	}

	public void QuitGame() {
		Application.Quit();
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}

	//////
	/// INPUT
	//////

	public void UpdateButtons() {
		Debug.Log("Current index  " + currentIndex);
		if (menuMode == 0) {
			for (int i = 0; i < mainButtons.Length; i++) {
				mainButtons[i].SetHighlight(i == currentIndex);
			}
			musicSlider.SetHighlight(currentIndex == -1);
			changeButton.SetHighlight(currentIndex == 10);
		}
		else if (menuMode == 1) {
			for (int i = 0; i < levelButtons.Count; i++) {
				levelButtons[i].SetHighlight(i == levelIndex);
			}
		}
	}

	public void OnOK() {
		Debug.Log("Current index  " + currentIndex);
		if (menuMode == 0) {
			if (currentIndex == 10)
				changeButton.Click();
			else if (currentIndex != -1)
				mainButtons[currentIndex].Click();
		}
		else if (menuMode == 1) {
			if (levelIndex < maxLevel.value && levelIndex < bestScore.value) {
				levelButtons[levelIndex].Clicked();
			}
		}
	}

	public void OnBack() {
		if (menuMode == 1) {
			GoToMainMenu();
		}
	}

	public void OnUp() {
		if (menuMode == 0) {
			if (currentIndex == -1) {
				if (musicVolume.value < 6) {
					musicVolume.value++;
					musicSliderSlider.value = musicVolume.value;
					audioController.UpdateVolume();
				}
			}
			else if (currentIndex < 10) {
				currentIndex--;
				if (currentIndex < 0)
					currentIndex = mainButtons.Length - 1;
			}
		}
		else if (menuMode == 1) {
			levelIndex = OPMath.FullLoop(0, levelButtons.Count - 1, levelIndex - 10);
		}

		UpdateButtons();
	}

	public void OnDown() {
		if (menuMode == 0) {
			if (currentIndex == -1) {
				if (musicVolume.value > 0) {
					musicVolume.value--;
					musicSliderSlider.value = musicVolume.value;
					audioController.UpdateVolume();
				}
			}
			else if (currentIndex < 10) {
				currentIndex++;
				if (currentIndex > mainButtons.Length - 1)
					currentIndex = 0;
			}
		}
		else if (menuMode == 1) {
			levelIndex = OPMath.FullLoop(0, levelButtons.Count - 1, levelIndex + 10);
		}

		UpdateButtons();
	}

	public void OnLeft() {
		if (menuMode == 0) {
			if (currentIndex == 10)
				currentIndex = 2;
			else
				currentIndex = -1;
			UpdateButtons();
		}
		else if (menuMode == 1) {
			levelIndex = OPMath.FullLoop(0, levelButtons.Count - 1, levelIndex - 1);
			UpdateButtons();
		}
		else if (menuMode == 2) {
			//ChangeControllerP1(-1);
		}
	}

	public void OnRight() {
		if (menuMode == 0) {
			if (currentIndex == -1)
				currentIndex = 2;
			else
				currentIndex = 10;
			UpdateButtons();
		}
		else if (menuMode == 1) {
			levelIndex = OPMath.FullLoop(0, levelButtons.Count - 1, levelIndex + 1);
			UpdateButtons();
		}
		else if (menuMode == 2) {
			//ChangeControllerP1(1);
		}
	}

	public void SliderVolume(float value) {
		musicVolume.value = Mathf.RoundToInt(value);
		audioController.UpdateVolume();
	}
}
