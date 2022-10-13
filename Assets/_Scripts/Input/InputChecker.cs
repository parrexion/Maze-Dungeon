using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputChecker : MonoBehaviour {

	public Image[] lights;
	private List<int> power = new List<int>();


	private void Awake() {
		for (int i = 0; i < lights.Length; i++) {
			power.Add(0);
		}
	}

	private void Update() {
		for (int i = 0; i < power.Count; i++) {
			power[i]--;
			lights[i].enabled = (power[i] > 0);
		}
	}

	public void TriggerButton(int index) {
		power[index] = 30;
	}
}
