using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTile : MapTile {


	public override void Setup() { }

	public override void SetupEditor() {
		rend.color = Color.white;
	}

	public override bool DoAction(BasicControls player, Direction direction){
		return false;
	}

}
