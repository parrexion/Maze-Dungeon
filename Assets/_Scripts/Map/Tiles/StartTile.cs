using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTile : MapTile {

	public Canvas canvas;
	public Text startNumber;

	public override void Setup() {
		Destroy(canvas.gameObject);
	}

	public override void SetupEditor() {
		startNumber.text = groupID.ToString();
	}
	
	public override bool DoAction(BasicControls player, Direction direction){
		return false;
	}

}
