using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatrolTile : MapTile {

	public Canvas canvas;
	public Text groupText;
	
	private Color p1Color = new Color(0.6f, 0.2f, 0.4f);
	private Color p2Color = new Color(0.2f, 0.4f, 0.6f);


	public override void Setup() {
		Destroy(canvas.gameObject);
		rend.color = Color.white;
	}

	public override void SetupEditor() {
		rend.color = (reversed) ? p2Color : p1Color;
		switch (faceDirection)
		{
			case Direction.UP:
				groupText.text = "UP";
				break;
			case Direction.DOWN:
				groupText.text = "DOWN";
				break;
			case Direction.LEFT:
				groupText.text = "LEFT";
				break;
			case Direction.RIGHT:
				groupText.text = "RIGHT";
				break;
		}
	}
	
	public override bool DoAction(BasicControls player, Direction direction) {
		return false;
	}

}
