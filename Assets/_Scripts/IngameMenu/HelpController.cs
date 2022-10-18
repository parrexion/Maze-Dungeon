using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpController : MonoBehaviour {

	public BoolVariable paused;
	public BoolVariable isSinglePlayer;

	[Header("Game Objects")]
	public GameObject pauseController;
	public GameObject helpObject;
	public GameObject p1ok;
	public GameObject p2ok;
	public GameObject p11ok;
	public GameObject singleImage;
	public GameObject multiImage;

	public SchemeReference player1Controls;
	public SchemeReference player2Controls;
	public TMPro.TextMeshProUGUI player1Text;
	public TMPro.TextMeshProUGUI player2Text;

	public bool player1ok;
	public bool player2ok;
	public bool player11ok;


	private void Start () {
		p1ok.SetActive(false);
		p2ok.SetActive(false);
		p11ok.SetActive(false);
		singleImage.SetActive(isSinglePlayer.value);
		multiImage.SetActive(!isSinglePlayer.value);
		ShowHelp(true);
		ControlSubstitution();
	}

	private void ControlSubstitution() {
		if (!isSinglePlayer.value) {
			string text = player1Text.text;
			string text2 = text.Replace("%UP", player1Controls.value.GetKeyName("%UP"))
						.Replace("%LEFT", player1Controls.value.GetKeyName("%LEFT"))
						.Replace("%RIGHT", player1Controls.value.GetKeyName("%RIGHT"))
						.Replace("%DOWN", player1Controls.value.GetKeyName("%DOWN"))
						.Replace("%ACTION", player1Controls.value.GetKeyName("%ACTION"))
						.Replace("%SWITCH", player1Controls.value.GetKeyName("%SWITCH"))
						.Replace("%S1", player1Controls.value.GetKeyName("%S1"))
						.Replace("%S2", player1Controls.value.GetKeyName("%S2"))
						.Replace("%S3", player1Controls.value.GetKeyName("%S3"))
						.Replace("%S4", player1Controls.value.GetKeyName("%S4"));
			player1Text.text = text2;

			text = player2Text.text;
			text2 = text.Replace("%UP", player2Controls.value.GetKeyName("%UP"))
						.Replace("%LEFT", player2Controls.value.GetKeyName("%LEFT"))
						.Replace("%RIGHT", player2Controls.value.GetKeyName("%RIGHT"))
						.Replace("%DOWN", player2Controls.value.GetKeyName("%DOWN"))
						.Replace("%ACTION", player2Controls.value.GetKeyName("%ACTION"))
						.Replace("%SWITCH", player2Controls.value.GetKeyName("%SWITCH"))
						.Replace("%S1", player2Controls.value.GetKeyName("%S1"))
						.Replace("%S2", player2Controls.value.GetKeyName("%S2"))
						.Replace("%S3", player2Controls.value.GetKeyName("%S3"))
						.Replace("%S4", player2Controls.value.GetKeyName("%S4"));
			player2Text.text = text2;
		}
		else {
			TMPro.TextMeshProUGUI[] texts = singleImage.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
			for (int i = 0; i < texts.Length; i++) {
				string text = texts[i].text;
				string text2 = text.Replace("%UP", player1Controls.value.GetKeyName("%UP"))
							.Replace("%LEFT", player1Controls.value.GetKeyName("%LEFT"))
							.Replace("%RIGHT", player1Controls.value.GetKeyName("%RIGHT"))
							.Replace("%DOWN", player1Controls.value.GetKeyName("%DOWN"))
							.Replace("%ACTION", player1Controls.value.GetKeyName("%ACTION"))
							.Replace("%SWITCH", player1Controls.value.GetKeyName("%SWITCH"))
							.Replace("%S1", player1Controls.value.GetKeyName("%S1"))
							.Replace("%S2", player1Controls.value.GetKeyName("%S2"))
							.Replace("%S3", player1Controls.value.GetKeyName("%S3"))
							.Replace("%S4", player1Controls.value.GetKeyName("%S4"));
				texts[i].text = text2;
			}
		}
	}

	private void ShowHelp(bool state) {
		paused.value = state;
		pauseController.SetActive(!state);
		helpObject.SetActive(state);
	}

	public void Player1Accept() {
		if (isSinglePlayer.value) {
			player11ok = true;
			p11ok.SetActive(true);
		}
		else {
			player1ok = true;
			p1ok.SetActive(true);
		}
		if (player1ok && player2ok || player11ok)
			ShowHelp(false);
	}

	public void Player2Accept() {
		if (isSinglePlayer.value)
			return;
			
		player2ok = true;
		p2ok.SetActive(true);
		if (player1ok && player2ok)
			ShowHelp(false);
	}
}
