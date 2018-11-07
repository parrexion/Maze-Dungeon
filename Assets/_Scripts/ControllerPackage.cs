using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPackage : MonoBehaviour {


	private void Awake() {
		while (transform.childCount > 0) {
			transform.GetChild(0).SetParent(null);
		}
		Destroy(gameObject);
	}

}
