using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCharacter : BasicControls {

	[Header("References")]
	public BoolVariable isSinglePlayer;
	public bool reachedGoal;

	[Header("Selection indication")]
	public int characterIndex;
	public IntVariable p1SelectedCharacter;
	public IntVariable p2SelectedCharacter;
	public GameObject p1SelectionIndication;
	public GameObject p2SelectionIndication;

	[Header("Actions")]
	public BoolVariable isPulling1;
	public BoolVariable isPulling2;

	[Header("Rotation")]
	public Transform rotateTransform;
	public Transform rotateTransform2a;
	public Transform rotateTransform2b;
	private float rotateSpeed = 75f;

	[Header("Events")]
	public UnityEvent killPlayerEvent;
	public UnityEvent goalReachedEvent;
	public UnityEvent playerMoveUp;
	public UnityEvent playerMoveLeft;
	public UnityEvent playerMoveRight;
	public UnityEvent playerMoveDown;


	private void Update() {
		if (paused.value)
			return;
		rotateTransform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
		rotateTransform2a.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
		rotateTransform2b.Rotate(Vector3.back, rotateSpeed * Time.deltaTime * 1.1f);
	}

	public override void Spawn(MapContainer map, MapTile tile) {
		mapContainer = map;
		posx = tile.posx;
		posy = tile.posy;
		currentTile = mapContainer.GetTile(posx, posy);
		transform.position = currentTile.transform.position;

		tile.currentCharacter = this;
	}

	public override void DeathEffect() {
		deathEffect.Play();
	}

	public void SingleMoveLeft() {
		if (isSinglePlayer.value)
			MoveLeft();
	}

	public void SingleMoveRight() {
		if (isSinglePlayer.value)
			MoveRight();
	}

	public void MoveUp() {
		if (isMoving || p1SelectedCharacter.value != characterIndex || paused.value)
			return;

		MapTile next = mapContainer.GetTile(posx, posy+1);
		if (!next)
			return;
		
		if (isPulling1.value && next.DoAction(this, Direction.UP))
			return;

		bool res = true;
		if (isPulling1.value)
			res = mapContainer.InteractTile(Direction.UP, posx, posy+1);

		if (!mapContainer.IsWalkable(type, posx, posy+1))
			return;
		
		currentTile.LeaveTile(this);
		currentTile = next;
		// currentTile.EndOnTile(this);

		if (!res)
			res = mapContainer.InteractTile(Direction.UP, posx, posy-1);
			
		posy++;
		moveToPosition = currentTile.transform.position;
		StartCoroutine(Move());
		if (mapContainer.IsDangerous(posx, posy)) {
			Debug.Log("DANGER TIME!");
			DeathEffect();
			killPlayerEvent.Invoke();
		}
		playerMoveUp.Invoke();
	}

	public void MoveLeft() {
		if (isMoving || p2SelectedCharacter.value != characterIndex || paused.value)
			return;

		MapTile next = mapContainer.GetTile(posx-1, posy);
		if (!next)
			return;

		bool pull = (isPulling2.value || (isPulling1.value && isSinglePlayer.value));
		if (pull && next.DoAction(this, Direction.LEFT))
			return;

		bool res = true;
		if (pull)
			res = mapContainer.InteractTile(Direction.LEFT, posx-1, posy);

		if (!mapContainer.IsWalkable(type, posx-1, posy))
			return;
			
		currentTile.LeaveTile(this);
		currentTile = next;
		// currentTile.EndOnTile(this);

		if (!res) 
			res = mapContainer.InteractTile(Direction.LEFT, posx+1, posy);

		posx--;
		moveToPosition = currentTile.transform.position;
		StartCoroutine(Move());
		if (mapContainer.IsDangerous(posx, posy)) {
			Debug.Log("DANGER TIME!");
			DeathEffect();
			killPlayerEvent.Invoke();
		}
		playerMoveLeft.Invoke();
	}

	public void MoveRight() {
		if (isMoving || p2SelectedCharacter.value != characterIndex || paused.value)
			return;
			
		MapTile next = mapContainer.GetTile(posx+1, posy);
		if (!next)
			return;
		
		bool pull = (isPulling2.value || (isPulling1.value && isSinglePlayer.value));
		if (pull && next.DoAction(this, Direction.RIGHT))
			return;

		bool res = true;
		if (pull)
			res = mapContainer.InteractTile(Direction.RIGHT, posx+1, posy);

		if (!mapContainer.IsWalkable(type, posx+1, posy))
			return;
			
		currentTile.LeaveTile(this);
		currentTile = next;
		// currentTile.EndOnTile(this);

		if (!res) 
			res = mapContainer.InteractTile(Direction.RIGHT, posx-1, posy);

		posx++;
		moveToPosition = currentTile.transform.position;
		StartCoroutine(Move());
		if (mapContainer.IsDangerous(posx, posy)) {
			Debug.Log("DANGER TIME!");
			DeathEffect();
			killPlayerEvent.Invoke();
		}
		playerMoveRight.Invoke();
	}

	public void MoveDown() {
		if (isMoving || p1SelectedCharacter.value != characterIndex || paused.value)
			return;

		MapTile next = mapContainer.GetTile(posx, posy-1);
		if (!next)
			return;
		
		if (isPulling1.value && next.DoAction(this, Direction.DOWN))
			return;

		bool res = true;
		if (isPulling1.value)
			res = mapContainer.InteractTile(Direction.DOWN, posx, posy-1);

		if (!mapContainer.IsWalkable(type, posx, posy-1))
			return;
		
		currentTile.LeaveTile(this);
		currentTile = next;
		// currentTile.EndOnTile(this);
		
		if (!res)
			res = mapContainer.InteractTile(Direction.DOWN, posx, posy+1);

		posy--;
		moveToPosition = currentTile.transform.position;
		StartCoroutine(Move());
		if (mapContainer.IsDangerous(posx, posy)) {
			Debug.Log("DANGER TIME!");
			DeathEffect();
			killPlayerEvent.Invoke();
		}
		playerMoveDown.Invoke();
	}

	public void SelectionChanged(bool visible) {
		p1SelectionIndication.SetActive(visible && p1SelectedCharacter.value == characterIndex);
		p2SelectionIndication.SetActive(visible && p2SelectedCharacter.value == characterIndex);
	}

}
