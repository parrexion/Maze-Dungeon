using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerAdjuster : MonoBehaviour {

	public BoolVariable isSinglePlayer;
	public GameObject[] indications;


	private void Start () {
		for (int i = 0; i < indications.Length; i++) {
			indications[i].SetActive(isSinglePlayer.value);
		}
	}

}
