using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour {

	[HideInInspector] public MapContainer mapContainer;
	public BoolVariable paused;
	public BoolVariable lockControls;

	[Header("Selection of players")]
	public BoolVariable isSinglePlayer;
	public IntVariable p1SelectedCharacter;
	public IntVariable p2SelectedCharacter;
	public List<PlayerCharacter> players;

	[Header("Events")]
	public UnityEvent ClearedMapEvent;

	private bool isPlayer1 = true;
	private int spawnCount;


	public void StartGame() {
		lockControls.value = false;
		paused.value = false;
		p1SelectedCharacter.value = 1;
		p2SelectedCharacter.value = (isSinglePlayer.value || spawnCount > 1) ? -1 : 1;
		UpdateSelections();
	}
	
	public void Spawn(int index, MapTile tile) {
		if (!tile) {
			players[index].reachedGoal = true;
			players[index].gameObject.SetActive(false);
		}
		else {
			players[index].Spawn(mapContainer, tile);
			spawnCount++;
		}
	}

	public void SelectCharacter(int index) {
		if (!players[index-1].gameObject.activeSelf)
			return;

		if (isSinglePlayer.value) {
			p1SelectedCharacter.value = (isPlayer1) ? index : -1;
			p2SelectedCharacter.value = (isPlayer1) ? -1 : index;
		}
		else {
			p1SelectedCharacter.value = index;
		}

		if (spawnCount > 1 && p1SelectedCharacter.value == p2SelectedCharacter.value){
			p2SelectedCharacter.value = -1;
		}
		UpdateSelections();
	}

	public void SelectCharacterP2(int index) {
		if (!players[index-1].gameObject.activeSelf || isSinglePlayer.value)
			return;

		p2SelectedCharacter.value = index;
		if (spawnCount > 1 && p1SelectedCharacter.value == p2SelectedCharacter.value){
			p1SelectedCharacter.value = -1;
		}
		UpdateSelections();
	}

	public void ChangePlayer() {
		if (!isSinglePlayer.value)
			return;
		isPlayer1 = !isPlayer1;
		int temp = p1SelectedCharacter.value;
		p1SelectedCharacter.value = p2SelectedCharacter.value;
		p2SelectedCharacter.value = temp;
		UpdateSelections();
	}

	private void UpdateSelections() {
		bool visible = !(spawnCount == 1 && !isSinglePlayer.value);
		for (int i = 0; i < players.Count; i++) {
			players[i].SelectionChanged(visible);
		}
	}

	public void OnReachedGoal() {
		UpdateSelections();
		for (int i = 0; i < players.Count; i++) {
			if (!players[i].reachedGoal){
				return;
			}
		}
		ClearedMapEvent.Invoke();
	}

}
