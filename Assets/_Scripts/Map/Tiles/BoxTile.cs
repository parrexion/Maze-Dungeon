using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxTile : MapTile {

	private Color editorColor = new Color(0.6f, 0.6f, 0.3f);
	private Color ingameColor = new Color(1f, 1f, 1f);


	public override void Setup() {
		rend.color = ingameColor;
	}

	public override void SetupEditor() {
		rend.color = editorColor;
	}

	public override bool DoAction(BasicControls player, Direction direction) {
		return false;
	}

}
