using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Direction { UP, LEFT, RIGHT, DOWN }
public enum TileType { EMPTY, WALL, START, GOAL, SPIKE, BUTTON, GATE, LEVER, PATROL, BOX, LASER, FALL }

public abstract class MapTile : MonoBehaviour {

	[Header("Set these values")]
	public TileType type;
	public SpriteRenderer rend;
	public bool blocked;

	[HideInInspector] public MapContainer map;
	[Header("Automatically set")]
	public BasicControls currentCharacter;
	public int posx, posy;
	public int groupID;
	public bool reversed;
	public Direction faceDirection;
	public bool multiSolve;
	public float percent;

	[Header("Events")]
	public UnityEvent killPlayerEvent;


	public abstract void Setup();

	public abstract void SetupEditor();

	public bool IsWalkable(CharacterType moveType) {
		if (blocked)
			return false;
		if (!currentCharacter)
			return true;
		switch (moveType)
		{
			case CharacterType.PLAYER:
				return (currentCharacter.type == CharacterType.ENEMY);
			case CharacterType.ENEMY:
				return (currentCharacter.type == CharacterType.PLAYER);
			case CharacterType.INTERACT:
				return false;
			case CharacterType.CRUSHING:
				return (currentCharacter.type != CharacterType.INTERACT);
			default:
				return false;
		}
	}

	public abstract bool DoAction(BasicControls player, Direction direction);

	public virtual void LeaveTile(BasicControls basic) {
		currentCharacter = null;
	}

	public virtual void EndOnTile(BasicControls basic) {
		currentCharacter = basic;
	}

}
