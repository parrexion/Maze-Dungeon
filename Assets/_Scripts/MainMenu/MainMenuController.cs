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
		mainMenu.SetActive(true);
		levelSelect.SetActive(false);
		controllerSelect.SetActive(false);
	}

	public void GoToLevelSelect() {
		mainMenu.SetActive(false);
		levelSelect.SetActive(true);
		controllerSelect.SetActive(false);
	}

	public void GoToControllerSelect() {
		mainMenu.SetActive(false);
		levelSelect.SetActive(false);
		controllerSelect.SetActive(true);
	}
}
