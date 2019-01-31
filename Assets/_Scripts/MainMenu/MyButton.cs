using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MyButton : MonoBehaviour {

	public Image highlight;
	public Text buttonText;
	public UnityEvent clickEvent;


	public void SetHighlight(bool state) {
		highlight.color = state ? Color.green : Color.white;
	}

	public void Click() {
		clickEvent.Invoke();
	}
}
