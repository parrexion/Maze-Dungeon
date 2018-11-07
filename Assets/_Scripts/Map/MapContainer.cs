using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class MapContainer : MonoBehaviour {

	private PlayerController playerController;
	[HideInInspector] public InteractController interactController;

	public int sizeX, sizeY;

	//Dictionaries
	public Dictionary<int,MapTile> spawnTiles = new Dictionary<int, MapTile>();
	public Dictionary<int,MapTile> tiles = new Dictionary<int, MapTile>();
	public Dictionary<int,ButtonGroup> buttonGroup = new Dictionary<int, ButtonGroup>();


	public MapTile GetTile(int x, int y) {
		if (x < 0 || y < 0 || x > sizeX || y > sizeY) {
			Debug.Log("OUT OF RANGE   " + x + " , " + y);
			return null;
		}
		try {
			return tiles[y * 1000 + x];
		}
		catch (System.Exception) {
			Debug.Log("Failed at pos:  " + x + " , " + y);
			throw;
		}
	}

	public bool IsWalkable(CharacterType type, int x, int y) {
		MapTile tile = GetTile(x, y);
		if (!tile || !tile.IsWalkable(type))
			return false;
		return true;
	}

	public bool IsDangerous(int x, int y) {
		BasicControls basic = GetTile(x, y).currentCharacter;
		Debug.Log("Checking dangeours at  " + x + " , " + y + "  :  " + (basic!=null));
		return (basic && basic.type == CharacterType.ENEMY);
	}

	public bool InteractTile(Direction dir, int x, int y) {
		return interactController.OnPlayerPush(dir, x, y);
	}

}
