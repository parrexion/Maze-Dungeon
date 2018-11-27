using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PatrolCharacter : BasicControls {

	public EnemyController enemyController;
	public bool reactPlayer1;
	public Sprite reactP1Sprite;
	public Sprite reactP2Sprite;


	public override void Spawn(MapContainer map, MapTile tile) {
		mapContainer = map;
		currentTile = tile;
		posx = tile.posx;
		posy = tile.posy;
		moveDirection = tile.faceDirection;
		reactPlayer1 = tile.reversed;
		GetComponent<SpriteRenderer>().sprite = (reactPlayer1) ? reactP1Sprite : reactP2Sprite;

		transform.position = currentTile.transform.position;
		TurnAround();
		TurnAround();

		tile.currentCharacter = this;
	}

	public override void DeathEffect() {
		
	}

	public void MoveForward(Direction dir, bool player1) {
		if (paused.value || reactPlayer1 != player1)
			return;

		MapTile next = GetNextTile();
		if (!next || !next.IsWalkable(type)) {
			TurnAround();
			return;
		}
		if (next.currentCharacter && next.currentCharacter.type == CharacterType.PLAYER) {
			next.currentCharacter.DeathEffect();
			enemyController.hitPlayerEvent.Invoke();
		}
		currentTile.LeaveTile(this);
		currentTile = next;
		currentTile.EndOnTile(this);
		posx = next.posx;
		posy = next.posy;
		moveToPosition = currentTile.transform.position;
		StartCoroutine(Move());
	}

	private MapTile GetNextTile() {
		switch (moveDirection)
		{
			case Direction.UP:
				return mapContainer.GetTile(posx,posy+1);
			case Direction.LEFT:
				return mapContainer.GetTile(posx-1,posy);
			case Direction.RIGHT:
				return mapContainer.GetTile(posx+1,posy);
			case Direction.DOWN:
				return mapContainer.GetTile(posx,posy-1);
		}
		return null;
	}

	private void TurnAround() {
		switch (moveDirection)
		{
			case Direction.UP:
				moveDirection = Direction.DOWN;
				transform.rotation = Quaternion.Euler(0,0,-90);
				break;
			case Direction.LEFT:
				moveDirection = Direction.RIGHT;
				transform.rotation = Quaternion.Euler(0,0,0);
				break;
			case Direction.RIGHT:
				moveDirection = Direction.LEFT;
				transform.rotation = Quaternion.Euler(0,0,180);
				break;
			case Direction.DOWN:
				moveDirection = Direction.UP;
				transform.rotation = Quaternion.Euler(0,0,90);
				break;
		}
	}
}
