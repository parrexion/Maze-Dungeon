﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallTile : MapTile {

	public static int speed = 2;

	public ButtonGroup group;
	public Text groupText;
	public Transform leftGate;
	public Transform rightGate;

	private bool active;


	public override void Setup() {
		active = reversed;
		groupText.text = "";
		if (reversed) {
			leftGate.localScale = new Vector3(0.1f,1,1);
			rightGate.localScale = new Vector3(0.1f,1,1);
		}
		else {
			leftGate.localScale = new Vector3(0.5f,1,1);
			rightGate.localScale = new Vector3(0.5f,1,1);
		}
	}

	public override void SetupEditor() {
		groupText.text = groupID.ToString();
		if (reversed) groupText.text += "R";
		leftGate.localScale = new Vector3(0.5f,1,1);
		rightGate.localScale = new Vector3(0.5f,1,1);
	}
	
	public override bool DoAction(BasicControls player, Direction direction){
		return false;
	}

	public void ChangeState(bool fulfilled) {
		bool success = (fulfilled != reversed);
		if (success != active) {
			StopAllCoroutines();
			active = success;
			StartCoroutine(Animate(active));
			if (currentCharacter != null) {
				Debug.Log("Oh oh");
				EndOnTile(currentCharacter);
			}
		}
	}

	private IEnumerator Animate(bool open) {
		Vector3 startSize = (open) ? new Vector3(0.5f,1,1) : new Vector3(0.1f,1,1);
		Vector3 endSize = (open) ? new Vector3(0.1f,1,1) : new Vector3(0.5f,1,1);
		float f = 0;
		while (f < 1) {
			f += Time.deltaTime * speed;
			leftGate.localScale = Vector3.Lerp(startSize, endSize, f);
			rightGate.localScale = Vector3.Lerp(startSize, endSize, f);
			yield return null;
		}
	}

	public override void EndOnTile(BasicControls basic) {
		base.EndOnTile(basic);
		if (!active || basic.type != CharacterType.PLAYER)
			return;

		PlayerCharacter player = (PlayerCharacter)basic;
		player.DeathEffect();
		killPlayerEvent.Invoke();
	}
}
