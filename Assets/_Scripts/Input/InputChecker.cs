using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputChecker : MonoBehaviour {

	public Image[] lights;
	private List<int> power = new List<int>();


	private void Start() {
		for (int i = 0; i < lights.Length; i++) {
			power.Add(0);
		}
	}

	private void Update() {
		for (int i = 0; i < power.Count; i++) {
			power[i]--;
			lights[i].color = (power[i] > 0) ? Color.green : Color.white;
		}
	}

	public void TriggerButton(int index) {
		power[index] = 20;
	}
}
