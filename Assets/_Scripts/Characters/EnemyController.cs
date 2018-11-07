using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour {

	public BoolVariable paused;
	public BoolVariable lockControls;
	[HideInInspector] public MapContainer mapContainer;

	[Header("Patrollers")]
	public Transform patrolParent;
	public Transform patrolPrefab;
	public List<PatrolCharacter> patrollers = new List<PatrolCharacter>();

	[Header("Events")]
	public UnityEvent hitPlayerEvent;


	public void SpawnPatroller(PatrolTile tile) {
		PatrolCharacter patrol = Instantiate(patrolPrefab, patrolParent).GetComponent<PatrolCharacter>();
		patrol.enemyController = this;
		patrol.Spawn(mapContainer, tile);

		patrollers.Add(patrol);
	}

	public void OnPlayerMove(int direction) {
		bool player1 = (direction != 1 && direction != 2);
		for (int i = 0; i < patrollers.Count; i++) {
			patrollers[i].MoveForward((Direction)direction, player1);
		}
	}
}
