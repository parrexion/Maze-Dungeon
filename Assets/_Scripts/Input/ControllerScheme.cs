using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ControllerScheme : ScriptableObject {

	[Header("Scheme Info")]
	public string schemeName;
	public Sprite schemeIcon;
	public bool useStick;

	[Header("Stick directions")]
	public string descStickDir;
	public string vertical;
	public string horizontal;

	[Header("Button directions")]
	public string descButtonDir;
	public KeyCode up;
	public KeyCode left;
	public KeyCode right;
	public KeyCode down;

	[Header("Character select")]
	public string descCharacterSel;
	public KeyCode select1;
	public KeyCode select2;
	public KeyCode select3;
	public KeyCode select4;

	[Header("Other buttons")]
	public KeyCode action;
	public KeyCode mode;
	public KeyCode start;


	public string GetKeyName(string input) {
		switch (input)
		{
			case "%UP":
				return (useStick) ? "[UP]" : "[" + up + "]";
			case "%LEFT":
				return (useStick) ? "[LEFT]" : "[" + left + "]";
			case "%RIGHT":
				return (useStick) ? "[RIGHT]" : "[" + right + "]";
			case "%DOWN":
				return (useStick) ? "[DOWN]" : "[" + down + "]";
			case "%ACTION":
				return (useStick) ? "[RB]" : "[" + action + "]";
			case "%SWITCH":
				return (useStick) ? "[LB]" : "[" + mode + "]";
			case "%S1":
				return (useStick) ? "[A]" : "[" + select1 + "]";
			case "%S2":
				return (useStick) ? "[B]" : "[" + select2 + "]";
			case "%S3":
				return (useStick) ? "[X]" : "[" + select3 + "]";
			case "%S4":
				return (useStick) ? "[Y]" : "[" + select4 + "]";
			case "%PAUSE":
				return (useStick) ? "[OPTIONS]" : "[" + start + "]";
			default:
				return "ERROR";
		}
	}
}
