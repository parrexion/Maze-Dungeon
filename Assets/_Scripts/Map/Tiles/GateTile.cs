﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GateTile : MapTile {

	public static int speed = 1;

	public ButtonGroup group;
	public Transform leftGate;
	public Transform rightGate;
	public Canvas canvas;
	public Text groupText;

	private bool active;


	public override void Setup() {
		active = reversed;
		blocked = !active;
		Destroy(canvas.gameObject);
		if (reversed) {
			leftGate.localScale = new Vector3(0f,1,1);
			rightGate.localScale = new Vector3(0f,1,1);
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
		}
		blocked = !active;
		if (blocked && currentCharacter) {
			if (currentCharacter.type == CharacterType.PLAYER) {
				currentCharacter.DeathEffect();
				killPlayerEvent.Invoke();
			}
		}
	}

	private IEnumerator Animate(bool open) {
		Vector3 startSize = (open) ? new Vector3(0.5f,1,1) : new Vector3(0f,1,1);
		Vector3 endSize = (open) ? new Vector3(0f,1,1) : new Vector3(0.5f,1,1);
		float f = 0;
		while (f < 1) {
			f += Time.deltaTime * speed;
			leftGate.localScale = Vector3.Lerp(startSize, endSize, f);
			rightGate.localScale = Vector3.Lerp(startSize, endSize, f);
			yield return null;
		}
	}
}
