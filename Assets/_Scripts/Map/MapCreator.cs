using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class MapCreator : MonoBehaviour {

	public Tilemap tilemap;
	public MapContainer mapContainer;
	private PlayerController playerController;
	private EnemyController enemyController;
	private InteractController interactController;


	private void Awake() {
		mapContainer.sizeX = mapContainer.sizeY = 0;
		playerController = GameObject.FindObjectOfType<PlayerController>();
		playerController.mapContainer = mapContainer;
		enemyController = GameObject.FindObjectOfType<EnemyController>();
		enemyController.mapContainer = mapContainer;
		interactController = GameObject.FindObjectOfType<InteractController>();
		interactController.mapContainer = mapContainer;
		mapContainer.interactController = interactController;
		IndexTilemap();
		SpawnPlayers();
	}

	private void IndexTilemap() {
		MapTile[] allTiles = tilemap.GetComponentsInChildren<MapTile>();
		for (int i = 0; i < allTiles.Length; i++) {
			int x = Mathf.FloorToInt(allTiles[i].transform.position.x);
			int y = Mathf.FloorToInt(allTiles[i].transform.position.y);
			allTiles[i].posx = x;
			allTiles[i].posy = y;
			allTiles[i].map = mapContainer;
			allTiles[i].Setup();

			try {
				if (x < 0 || y < 0)
					allTiles[i].gameObject.SetActive(false);
				else {
					mapContainer.tiles.Add(y * 1000 + x, allTiles[i]);
				}
			}
			catch (System.Exception) {
				Debug.Log("Duplicate on position " + x + " , " + y, allTiles[i]);
				throw;
			}

			mapContainer.sizeX = Mathf.Max(mapContainer.sizeX, x);
			mapContainer.sizeY = Mathf.Max(mapContainer.sizeY, y);
			
			AdditionalFeatures(allTiles[i]);
		}
	}

	private void AdditionalFeatures(MapTile tile) {
		ButtonGroup g;
		int id = tile.groupID;
		switch (tile.type)
		{
			case TileType.BUTTON:
				ButtonTile button = (ButtonTile)tile;
				if (!mapContainer.buttonGroup.ContainsKey(id))
					mapContainer.buttonGroup.Add(id, new ButtonGroup());
				g = mapContainer.buttonGroup[id];
				g.buttons.Add(button);
				button.group = g;
				break;
			case TileType.LEVER:
				LeverTile lever = (LeverTile)tile;
				if (!mapContainer.buttonGroup.ContainsKey(id))
					mapContainer.buttonGroup.Add(id, new ButtonGroup());
				g = mapContainer.buttonGroup[id];
				g.levers.Add(lever);
				lever.group = g;
				break;
			case TileType.GATE:
				GateTile gate = (GateTile)tile;
				if (!mapContainer.buttonGroup.ContainsKey(id))
					mapContainer.buttonGroup.Add(id, new ButtonGroup());
				g = mapContainer.buttonGroup[id];
				g.gates.Add(gate);
				gate.group = g;
				break;
			case TileType.FALL:
				FallTile fall = (FallTile)tile;
				if (!mapContainer.buttonGroup.ContainsKey(id))
					mapContainer.buttonGroup.Add(id, new ButtonGroup());
				g = mapContainer.buttonGroup[id];
				g.fallPits.Add(fall);
				Debug.Log("Added fall");
				fall.group = g;
				break;
			case TileType.PATROL:
				enemyController.SpawnPatroller((PatrolTile)tile);
				break;
			case TileType.BOX:
				interactController.SpawnBox((BoxTile)tile);
				break;
			case TileType.START:
				mapContainer.spawnTiles.Add(id,tile);
				break;
		}
	}

	private void SpawnPlayers() {
		for (int i = 0; i < playerController.players.Count; i++) {
			if (mapContainer.spawnTiles.ContainsKey(i)) {
				MapTile tile = mapContainer.spawnTiles[i];
				playerController.Spawn(i, tile);
			}
			else {
				playerController.Spawn(i, null);
			}
		}
		playerController.StartGame();
	}

}
