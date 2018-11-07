using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserTile : MapTile {

	public static float maxCharge = 5f;

	public BoolVariable paused;
	public Text groupText;
	public SpriteRenderer beam;
	public Image chargeMeter;

	private float charge;


	public override void Setup() {
		groupText.text = "";
		beam.enabled = false;
		charge = maxCharge * percent;
		chargeMeter.fillAmount = charge;
		StartCoroutine(LaserLoop());
	}

	public override void SetupEditor() {
		switch (faceDirection)
		{
			case Direction.UP:
				groupText.text = "UP";
				beam.transform.rotation = Quaternion.Euler(0,0,90);
				break;
			case Direction.DOWN:
				groupText.text = "DOWN";
				beam.transform.rotation = Quaternion.Euler(0,0,-90);
				break;
			case Direction.LEFT:
				groupText.text = "LEFT";
				beam.transform.rotation = Quaternion.Euler(0,0,180);
				break;
			case Direction.RIGHT:
				groupText.text = "RIGHT";
				beam.transform.rotation = Quaternion.Euler(0,0,0);
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
				chargeMeter.fillAmount = (charge / maxCharge);
			}
			yield return null;
		}
	}

	private IEnumerator FireLaser() {
		int x = posx, y = posy;
		int moveX = 0, moveY = 0;
		SetDirection(out moveX, out moveY);
		MapTile nextTile = map.GetTile(x,y);
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

		float size = 0.5f + (Mathf.Abs(x - posx) + Mathf.Abs(y - posy));
		beam.transform.localScale = new Vector3(size, 0.25f, 1f);
		beam.enabled = true;
		
		yield return new WaitForSeconds(0.5f);
		beam.enabled = false;
	}

	private void SetDirection(out int x, out int y) {
		switch (faceDirection)
		{
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
