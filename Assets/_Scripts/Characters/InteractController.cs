using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractController : MonoBehaviour {

	[HideInInspector] public MapContainer mapContainer;
	public BoolVariable paused;
	public BoolVariable lockControls;
	public BoolVariable p1HoldButton;
	public BoolVariable p2HoldButton;

	[Header("Boxes")]
	public Transform boxParent;
	public Transform boxPrefab;
	public List<BoxCharacter> boxes = new List<BoxCharacter>();

	
	public void SpawnBox(BoxTile tile) {
		BoxCharacter box = Instantiate(boxPrefab, boxParent).GetComponent<BoxCharacter>();
		box.Spawn(mapContainer, tile);
		boxes.Add(box);
	}

	private BoxCharacter GetBox(int x, int y) {
		for (int i = 0; i < boxes.Count; i++) {
			if (boxes[i].posx == x && boxes[i].posy == y)
				return boxes[i];
		}
		return null;
	}

	public bool OnPlayerPush(Direction dir, int x, int y) {
		BoxCharacter box = GetBox(x, y);
		if (!box)
			return false;

		box.MoveForward(dir);
		return true;
	}
}
