using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoxCharacter : BasicControls {


	public override void Spawn(MapContainer map, MapTile tile) {
		mapContainer = map;
		currentTile = tile;
		posx = tile.posx;
		posy = tile.posy;
		moveDirection = tile.faceDirection;

		transform.position = currentTile.transform.position;
		
		tile.currentCharacter = this;
	}

	public override void DeathEffect() {
		
	}

	public void MoveForward(Direction dir) {
		if (paused.value)
			return;

		moveDirection = dir;
		Debug.Log("Push in dir:  " + dir);
		MapTile next = GetNextTile();		
		if (!next || !next.IsWalkable(type))
			return;
		
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
			default:
				return null;
		}
	}

}
