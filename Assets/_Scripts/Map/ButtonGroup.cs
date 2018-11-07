using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGroup {

	public bool oneTimeSolve;
	public bool fulfilled;

	public List<GateTile> gates = new List<GateTile>();
	public List<FallTile> fallPits = new List<FallTile>();
	public List<ButtonTile> buttons = new List<ButtonTile>();
	public List<LeverTile> levers = new List<LeverTile>();

	private int powerLevel;


	public void UpdatePowerLevel() {
		powerLevel = 0;
		for (int i = 0; i < buttons.Count; i++) {
			if (buttons[i].activated)
				powerLevel++;
		}
		for (int i = 0; i < levers.Count; i++) {
			if (levers[i].isPulled)
				powerLevel++;
		}

		fulfilled = (powerLevel == (buttons.Count + levers.Count));
		if (fulfilled)
			oneTimeSolve = true;

		for (int i = 0; i < gates.Count; i++) {
			gates[i].ChangeState(fulfilled);
		}
		for (int i = 0; i < fallPits.Count; i++) {
			fallPits[i].ChangeState(fulfilled);
			Debug.Log("Updated fall");
		}
		// Debug.Log("Power level is now:   " + powerLevel + "/" + (buttons.Count + levers.Count));
	}

	public bool IsCleared() {
		return fulfilled;
	}
}
