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

	[Header("Main menu")]
	public BoolVariable lockControls;
	public BoolVariable isSinglePlayer;
	public GameObject backgroundSingle;
	public GameObject backgroundMulti;

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
	public Text scheme1Text;
	public Text scheme2Text;
	public Image scheme1Icon;
	public Image scheme2Icon;
	public IntVariable p1Index;
	public IntVariable p2Index;

	[Header("Menu Controls")]
	public MyButton[] mainButtons;
	private int menuMode;
	private int currentIndex;
	private int levelIndex;

	[Header("Events")]
	public UnityEvent nextLevelEvent;
	public UnityEvent startSaveEvent;


	private void Start () {
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
	}

	private void SetupMainMenu() {
		backgroundSingle.SetActive(isSinglePlayer.value);
		backgroundMulti.SetActive(!isSinglePlayer.value);
	}

	public void CreateLevelButtons() {
		Debug.Log("Buttons!");
		for (int i = 0; i < maxLevel.value; i++) {
			Transform t = Instantiate(buttonTemplate, levelParent);
			LevelButton lb = t.GetComponent<LevelButton>();
			lb.index = i;
			lb.text.text = (i+1).ToString();
			lb.button.interactable = (bestScore.value > i);
			levelButtons.Add(lb);
		}
		buttonTemplate.gameObject.SetActive(false);
	}

	private void SetupControllers() {
		scheme1Text.text = schemePool[p1Index.value].schemeName;
		scheme1Icon.sprite = schemePool[p1Index.value].schemeIcon;
		if (isSinglePlayer.value) {
			player2Side.SetActive(false);
		}
		else {
			player2Side.SetActive(true);
			scheme2Text.text = schemePool[p2Index.value].schemeName;
			scheme2Icon.sprite = schemePool[p2Index.value].schemeIcon;
		}
		player1ControllerScheme.value = schemePool[p1Index.value];
		player2ControllerScheme.value = schemePool[p2Index.value];
	}

	public void ChangeControllerP1(int dir) {
		do {
			p1Index.value = OPMath.FullLoop(0, schemePool.Length-1, p1Index.value + dir);
		} while (p1Index.value == p2Index.value);
		SetupControllers();
	}

	public void ChangeControllerP2(int dir) {
		do {
			p2Index.value = OPMath.FullLoop(0, schemePool.Length-1, p2Index.value + dir);
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

	//////
	/// INPUT
	//////

	public void UpdateButtons() {
		if (menuMode == 0) {
			for (int i = 0; i < mainButtons.Length; i++) {
				mainButtons[i].SetHighlight(i == currentIndex);
			}
		}
		else if (menuMode == 1) {
			for (int i = 0; i < levelButtons.Count; i++) {
				levelButtons[i].SetHighlight(i == levelIndex);
			}
		}
	}

	public void OnOK() {
		if (menuMode == 0) {
			mainButtons[currentIndex].Click();
		}
		else if (menuMode == 1) {
			if (levelIndex < maxLevel.value) {
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
			currentIndex--;
			if (currentIndex < 0)
				currentIndex = mainButtons.Length-1;
		}
		else if (menuMode == 1) {
			levelIndex = OPMath.FullLoop(0, levelButtons.Count-1, levelIndex -10);
		}

		UpdateButtons();
	}

	public void OnDown() {
		if (menuMode == 0) {
			currentIndex++;
			if (currentIndex > mainButtons.Length-1)
				currentIndex = 0;
		}
		else if (menuMode == 1) {
			levelIndex = OPMath.FullLoop(0, levelButtons.Count-1, levelIndex +10);
		}

		UpdateButtons();
	}

	public void OnLeft() {
		if (menuMode == 1) {
			levelIndex = OPMath.FullLoop(0, levelButtons.Count-1, levelIndex -1);
			UpdateButtons();
		}
		else if (menuMode == 2) {
			//ChangeControllerP1(-1);
		}
	}

	public void OnRight() {
		if (menuMode == 1) {
			levelIndex = OPMath.FullLoop(0, levelButtons.Count-1, levelIndex +1);
			UpdateButtons();
		}
		else if (menuMode == 2) {
			//ChangeControllerP1(1);
		}
	}
}
