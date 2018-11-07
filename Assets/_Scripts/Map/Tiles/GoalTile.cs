using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoalTile : MapTile {

	[Header("Addition manual sets")]
	public UnityEvent goalReachedEvent;


	public override void Setup() { }

	public override void SetupEditor() { }
	
	public override bool DoAction(BasicControls player, Direction direction){
		return false;
	}

	public override void LeaveTile(BasicControls basic) {
		base.LeaveTile(basic);
		if (basic.type == CharacterType.PLAYER)
			((PlayerCharacter)basic).reachedGoal = false;
	}

	public override void EndOnTile(BasicControls basic) {
		base.EndOnTile(basic);
		if (basic.type == CharacterType.PLAYER) {
			((PlayerCharacter)basic).reachedGoal = true;
			goalReachedEvent.Invoke();
		}
	}

}
