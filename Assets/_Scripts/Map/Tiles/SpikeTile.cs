using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTile : MapTile {


	public override void Setup() { }

	public override void SetupEditor() { }

	public override bool DoAction(BasicControls player, Direction direction){
		return false;
	}

	public override void EndOnTile(BasicControls basic) {
		base.EndOnTile(basic);
		if (basic.type == CharacterType.PLAYER) {
			basic.DeathEffect();
			killPlayerEvent.Invoke();
		}
	}

}
