using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

	public IntVariable currentLevelIndex;
	public int index;
	public Text text;
	public Button button;

	public UnityEvent startLevelEvent;


	public void Clicked() {
		currentLevelIndex.value = index;
		startLevelEvent.Invoke();
	}

}
