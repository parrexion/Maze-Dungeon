using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerDebug : MonoBehaviour {

	public Text controllerText;
	public ControllerScheme scheme;


	private void Update() {
		controllerText.text = "Controller\nVertical:  " + 
					Input.GetAxis(scheme.vertical) + "\nHorizontal:  " +
					Input.GetAxis(scheme.horizontal);
	}

}
