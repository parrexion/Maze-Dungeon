using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTile : MapTile {

	public Text groupText;
	public ButtonGroup group;

	public bool activated;
	public Sprite deactivatedColor;
	public Sprite waitingColor;
	public Sprite activatedColor;


	public override void Setup() {
		groupText.text = "";
	}

	public override void SetupEditor() {
		rend.sprite = deactivatedColor;
		groupText.text = groupID.ToString();
		if (multiSolve) groupText.text += "M";
	}
	
	public override bool DoAction(BasicControls player, Direction direction) {
		return false;
	}

	public override void LeaveTile(BasicControls basic) {
		base.LeaveTile(basic);
		if (!multiSolve && group.oneTimeSolve)
			return;
		activated = false;
		group.UpdatePowerLevel();
		rend.sprite = deactivatedColor;
	}

	public override void EndOnTile(BasicControls basic) {
		base.EndOnTile(basic);
		activated = true;
		rend.sprite = activatedColor;
		group.UpdatePowerLevel();
	}

}
