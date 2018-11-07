using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BrushWindow : EditorWindow {

	public PrefabBrush brush;
	public string[] tileTypes = { "EMPTY", "WALL", "START", "GOAL", "SPIKE", "BUTTON", "GATE", "LEVER", "PATROL", "BOX", "LASER", "FALL PIT" };


	[MenuItem("Window/BrushPalette")]
	public static void ShowWindow() {
		GetWindow<BrushWindow>("Brush Palette");
	}


	private void OnGUI() {
		DrawSettings();
	}

	private void DrawSettings() {
		GUILayout.Label("Brush Settings", EditorStyles.boldLabel);
		brush.zPos = EditorGUILayout.IntField("Z Position", brush.zPos);

		GUILayout.Space(10);
		
		brush.currentBrush = (TileType)GUILayout.SelectionGrid((int)brush.currentBrush, tileTypes, 3);

		GUILayout.Space(10);
		GUILayout.Label("Specific settings", EditorStyles.boldLabel);
		
		switch (brush.currentBrush)
		{
			case TileType.START:
				brush.groupIndex = EditorGUILayout.IntField("Spawn ID", brush.groupIndex);
				break;
			case TileType.BUTTON:
				brush.groupIndex = EditorGUILayout.IntField("Group ID", brush.groupIndex);
				brush.multiSolve = EditorGUILayout.Toggle("Multi Solve", brush.multiSolve);
				break;
			case TileType.GATE:
				brush.groupIndex = EditorGUILayout.IntField("Group ID", brush.groupIndex);
				brush.reversed = EditorGUILayout.Toggle("Reversed", brush.reversed);
				break;
			case TileType.LEVER:
				brush.groupIndex = EditorGUILayout.IntField("Group ID", brush.groupIndex);
				brush.faceDirection = (Direction)EditorGUILayout.EnumPopup("Face direction", brush.faceDirection);
				break;
			case TileType.PATROL:
				brush.reversed = EditorGUILayout.Toggle("Reacts to Player 2", brush.reversed);
				brush.faceDirection = (Direction)EditorGUILayout.EnumPopup("Start direction", brush.faceDirection);
				break;
			case TileType.LASER:
				brush.faceDirection = (Direction)EditorGUILayout.EnumPopup("Fire direction", brush.faceDirection);
				brush.percent = EditorGUILayout.IntSlider("Start percent", brush.percent, 0, 100);
				break;
			case TileType.FALL:
				brush.groupIndex = EditorGUILayout.IntField("Group ID", brush.groupIndex);
				brush.reversed = EditorGUILayout.Toggle("Reversed", brush.reversed);
				break;

			default:
				GUILayout.Label("Nothing for type " + brush.currentBrush);
				break;
		}
	}
}
