using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserTile : MapTile {

	public static float maxCharge = 5f;

	public BoolVariable paused;
	public SpriteRenderer beam;
	public SpriteRenderer beamSplash;
	public SpriteRenderer[] chargeImages;

	private float stepSize;
	private float charge;


	public override void Setup() {
		beam.enabled = false;
		beamSplash.enabled = false;
		stepSize = 1f / (chargeImages.Length + 1);
		charge = maxCharge * percent;
		RefreshCharge(percent);
		StartCoroutine(LaserLoop());
	}

	public override void SetupEditor() {
		switch (faceDirection) {
			case Direction.UP:
				transform.rotation = Quaternion.Euler(0, 0, 90);
				break;
			case Direction.DOWN:
				transform.rotation = Quaternion.Euler(0, 0, -90);
				break;
			case Direction.LEFT:
				transform.rotation = Quaternion.Euler(0, 0, 180);
				break;
			case Direction.RIGHT:
				transform.rotation = Quaternion.Euler(0, 0, 0);
				break;
		}
	}

	public override bool DoAction(BasicControls player, Direction direction) {
		return false;
	}

	private IEnumerator LaserLoop() {
		while (true) {
			if (!paused.value) {
				charge += Time.deltaTime;
				if (charge >= maxCharge) {
					StartCoroutine(FireLaser());
					charge -= maxCharge;
				}
				RefreshCharge(charge / maxCharge);
			}
			yield return null;
		}
	}

	private void RefreshCharge(float fillAmount) {
		if (fillAmount < stepSize) {
			for (int i = 0; i < chargeImages.Length; i++) {
				chargeImages[i].enabled = true;
			}
		}
		else {
			float tot = stepSize;
			for (int i = 0; i < chargeImages.Length; i++) {
				tot += stepSize;
				chargeImages[i].enabled = (fillAmount >= tot);
			}
		}
	}

	private IEnumerator FireLaser() {
		int x = posx, y = posy;
		SetDirection(out int moveX, out int moveY);
		MapTile nextTile = map.GetTile(x, y);
		do {
			if (nextTile.currentCharacter && nextTile.currentCharacter.type == CharacterType.PLAYER) {
				nextTile.currentCharacter.DeathEffect();
				killPlayerEvent.Invoke();
			}
			x += moveX;
			y += moveY;
			nextTile = map.GetTile(x, y);
		} while (nextTile != null && nextTile.IsWalkable(CharacterType.CRUSHING));

		x -= moveX;
		y -= moveY;
		beamSplash.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0f);

		float size = 0.06f + (Mathf.Abs(x - posx) + Mathf.Abs(y - posy));
		beam.transform.localScale = new Vector3(size * 0.5f, 0.5f, 1f);
		beam.enabled = true;
		beamSplash.enabled = true;

		yield return new WaitForSeconds(0.4f);
		beam.enabled = false;
		beamSplash.enabled = false;
	}

	private void SetDirection(out int x, out int y) {
		switch (faceDirection) {
			case Direction.UP:
				x = 0;
				y = 1;
				break;
			case Direction.LEFT:
				x = -1;
				y = 0;
				break;
			case Direction.RIGHT:
				x = 1;
				y = 0;
				break;
			case Direction.DOWN:
				x = 0;
				y = -1;
				break;
			default:
				x = 0;
				y = 0;
				break;
		}
	}
}
