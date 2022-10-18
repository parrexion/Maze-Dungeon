using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeverTile : MapTile {

	public Text groupText;
	public ButtonGroup group;
	public Sprite activeSprite;
	public Sprite deactiveSprite;
	public bool isPulled;


	public override void Setup() {
		groupText.text = "";
		isPulled = true;
		ChangeState();
	}

	public override void SetupEditor() {
		groupText.text = groupID.ToString();
		if (faceDirection == Direction.LEFT) {
			transform.localRotation = Quaternion.Euler(0, 0, 90);
			rend.flipX = true;
		}
		else if (faceDirection == Direction.RIGHT) {
			transform.localRotation = Quaternion.Euler(0, 0, -90);
		}
	}

	public override bool DoAction(BasicControls player, Direction direction) {
		ChangeState();
		group.UpdatePowerLevel();
		return true;
	}

	public void ChangeState() {
		isPulled = !isPulled;
		rend.sprite = (isPulled) ? activeSprite : deactiveSprite;
	}

}
