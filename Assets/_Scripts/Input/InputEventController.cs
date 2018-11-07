using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputEventController : MonoBehaviour {

#region Singleton
	private static InputEventController instance = null;
	private void Start() {
		if (instance != null) {
			Destroy(gameObject);
		}
		else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}
#endregion

	private enum InputType { UP, LEFT, RIGHT, DOWN, SEL1, SEL2, SEL3, SEL4, ACTION, SWITCH, START, HOLD_ACT }

	public int holdDelay = 25;
	public int scrollSpeed = 5;

	[Header("Input schemes")]
	public SchemeReference player1ControllerScheme;
	public SchemeReference player2ControllerScheme;

	[Header("Control locks")]
	public BoolVariable lockAllControls;

	[Header("Move events")]
	public UnityEvent upArrowEvent;
	public UnityEvent downArrowEvent;
	public UnityEvent leftArrowEvent;
	public UnityEvent rightArrowEvent;
	[Space(10)]
	public UnityEvent upArrowP2Event;
	public UnityEvent downArrowP2Event;
	public UnityEvent leftArrowP2Event;
	public UnityEvent rightArrowP2Event;
	
	[Header("Button Events")]
	public UnityEvent sel1ButtonEvent;
	public UnityEvent sel2ButtonEvent;
	public UnityEvent sel3ButtonEvent;
	public UnityEvent sel4ButtonEvent;
	public UnityEvent actionButtonEvent;
	public UnityEvent switchButtonEvent;
	public UnityEvent startButtonEvent;
	[Space(10)]
	public UnityEvent sel1ButtonP2Event;
	public UnityEvent sel2ButtonP2Event;
	public UnityEvent sel3ButtonP2Event;
	public UnityEvent sel4ButtonP2Event;
	public UnityEvent actionButtonP2Event;
	public UnityEvent switchButtonP2Event;
	public UnityEvent startButtonP2Event;

	[Header("Hold down buttons")]
	public BoolVariable holdDownActionButton;
	public BoolVariable holdDownActionP2Button;

	private bool axisUp;
	private bool axisDown;
	private bool axisLeft;
	private bool axisRight;


	private void Update() {
		if (lockAllControls.value)
			return;

		MenuMode(player1ControllerScheme.value, true);
		MenuMode(player2ControllerScheme.value, false);
	}

	private void MenuMode(ControllerScheme scheme, bool isPlayer1) {

		if (scheme.useStick) {
			// Stick releases
			if (Input.GetAxis(scheme.vertical) == 0) {
				axisUp = false;
				axisDown = false;
			}
			if (Input.GetAxis(scheme.horizontal) == 0) {
				axisLeft = false;
				axisRight = false;
			}
			// Stick presses
			if (!axisUp && Input.GetAxis(scheme.vertical) == -1) {
				CallEvent(InputType.UP, isPlayer1);
				axisUp = true;
			}
			if (!axisLeft && Input.GetAxis(scheme.horizontal) == -1) {
				CallEvent(InputType.LEFT, isPlayer1);
				axisLeft = true;
			}
			if (!axisRight && Input.GetAxis(scheme.horizontal) == 1) {
				CallEvent(InputType.RIGHT, isPlayer1);
				axisRight = true;
			}
			if (!axisDown && Input.GetAxis(scheme.vertical) == 1) {
				CallEvent(InputType.DOWN, isPlayer1);
				axisDown = true;
			}
		}
		else {
			// Arrow presses
			if (Input.GetKeyDown(scheme.up)) {
				CallEvent(InputType.UP, isPlayer1);
				axisUp = true;
			}
			if (Input.GetKeyDown(scheme.left)) {
				CallEvent(InputType.LEFT, isPlayer1);
				axisLeft = true;
			}
			if (Input.GetKeyDown(scheme.right)) {
				CallEvent(InputType.RIGHT, isPlayer1);
				axisRight = true;
			}
			if (Input.GetKeyDown(scheme.down)) {
				CallEvent(InputType.DOWN, isPlayer1);
				axisDown = true;
			}
		}

		// Button presses
		if (Input.GetKeyDown(scheme.select1)) {
			CallEvent(InputType.SEL1, isPlayer1);
		}
		if (Input.GetKeyDown(scheme.select2)) {
			CallEvent(InputType.SEL2, isPlayer1);
		}
		if (Input.GetKeyDown(scheme.select3)) {
			CallEvent(InputType.SEL3, isPlayer1);
		}
		if (Input.GetKeyDown(scheme.select4)) {
			CallEvent(InputType.SEL4, isPlayer1);
		}
		if (Input.GetKeyDown(scheme.action)) {
			CallEvent(InputType.ACTION, isPlayer1);
		}
		if (Input.GetKeyDown(scheme.mode)) {
			CallEvent(InputType.SWITCH, isPlayer1);
		}
		if (Input.GetKeyDown(scheme.start)) {
			CallEvent(InputType.START, isPlayer1);
		}
		
		SetButtonHold(InputType.HOLD_ACT, isPlayer1, Input.GetKey(scheme.action));
	}

	private void InGameMode() {

	}

	private void CallEvent(InputType type, bool isPlayer1) {
		switch (type)
		{
			case InputType.UP:
				if (isPlayer1) upArrowEvent.Invoke();
				else upArrowP2Event.Invoke();
				break;
			case InputType.LEFT:
				if (isPlayer1) leftArrowEvent.Invoke();
				else leftArrowP2Event.Invoke();
				break;
			case InputType.RIGHT:
				if (isPlayer1) rightArrowEvent.Invoke();
				else rightArrowP2Event.Invoke();
				break;
			case InputType.DOWN:
				if (isPlayer1) downArrowEvent.Invoke();
				else downArrowP2Event.Invoke();
				break;
			case InputType.SEL1:
				if (isPlayer1) sel1ButtonEvent.Invoke();
				else sel1ButtonP2Event.Invoke();
				break;
			case InputType.SEL2:
				if (isPlayer1) sel2ButtonEvent.Invoke();
				else sel2ButtonP2Event.Invoke();
				break;
			case InputType.SEL3:
				if (isPlayer1) sel3ButtonEvent.Invoke();
				else sel3ButtonP2Event.Invoke();
				break;
			case InputType.SEL4:
				if (isPlayer1) sel4ButtonEvent.Invoke();
				else sel4ButtonP2Event.Invoke();
				break;
			case InputType.ACTION:
				if (isPlayer1) actionButtonEvent.Invoke();
				else actionButtonP2Event.Invoke();
				break;
			case InputType.SWITCH:
				if (isPlayer1) switchButtonEvent.Invoke();
				else switchButtonP2Event.Invoke();
				break;
			case InputType.START:
				if (isPlayer1) startButtonEvent.Invoke();
				else startButtonP2Event.Invoke();
				break;
			default:
				Debug.LogError("Invalid input!");
				break;
		}
	}

	private void SetButtonHold(InputType type, bool isPlayer1, bool hold) {
		switch (type)
		{
			case InputType.HOLD_ACT:
				if (isPlayer1) holdDownActionButton.value = hold;
				else holdDownActionP2Button.value = hold;
				break;
			default:
				Debug.LogError("Invalid input!");
				break;
		}
	}
}
