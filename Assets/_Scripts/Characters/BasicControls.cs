using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CharacterType { PLAYER, ENEMY, INTERACT, CRUSHING }

public abstract class BasicControls : MonoBehaviour {

	public BoolVariable paused;
	public MapContainer mapContainer;
	public CharacterType type;
	public ParticleSystem deathEffect;

	[Header("Movement")]
	public float speed = 1;
	public MapTile currentTile;
	public int posx, posy;
	public Direction moveDirection;
	protected Vector3 moveToPosition;
	protected bool isMoving;


	public abstract void Spawn(MapContainer map, MapTile tile);
	public abstract void DeathEffect();

	protected IEnumerator Move() {
		isMoving = true;
		Vector3 startPosition = transform.position;
		float f = 0;
		while (f < 1) {
			f += Time.deltaTime * speed;
			transform.position = Vector3.Lerp(startPosition, moveToPosition, f);
			yield return null;
		}
		isMoving = false;
	}

}
