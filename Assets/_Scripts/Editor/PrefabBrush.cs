using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditor
{
	[CreateAssetMenu]
	[CustomGridBrush(false, true, false, "Prefab Brush")]
	public class PrefabBrush : GridBrushBase
	{
		[Header("Brush Settings")]
		public TileType currentBrush;
		public int groupIndex;
		public bool reversed;
		public Direction faceDirection;
		public bool multiSolve;
		public int percent;

		[Header("Prefabs")]
		public GameObject[] m_Prefabs;

		[Header("Z - position")]
		public int zPos;


		public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
		{
			// Do not allow editing palettes
			if (brushTarget.layer == 31)
				return;

			Erase(grid, brushTarget, position);

			GameObject prefab = m_Prefabs[(int)currentBrush];
			GameObject instance = (GameObject) PrefabUtility.InstantiatePrefab(prefab);
			Undo.RegisterCreatedObjectUndo((Object)instance, "Paint Prefabs");
			if (instance != null)
			{
				instance.transform.SetParent(brushTarget.transform);
				instance.transform.position = grid.LocalToWorld(grid.CellToLocalInterpolated(new Vector3Int(position.x, position.y, zPos) + new Vector3(.5f, .5f, .5f)));
				MapTile tile = instance.GetComponent<MapTile>();
				tile.groupID = groupIndex;
				tile.reversed = reversed;
				tile.faceDirection = faceDirection;
				tile.multiSolve = multiSolve;
				tile.percent = percent * 0.01f;
				tile.SetupEditor();
			}
		}

		public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position)
		{
			// Do not allow editing palettes
			if (brushTarget.layer == 31)
				return;

			Transform erased = GetObjectInCell(grid, brushTarget.transform, new Vector3Int(position.x, position.y, zPos));
			if (erased != null)
				Undo.DestroyObjectImmediate(erased.gameObject);
		}

		private static Transform GetObjectInCell(GridLayout grid, Transform parent, Vector3Int position)
		{
			int childCount = parent.childCount;
			Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));
			Vector3 max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));
			Bounds bounds = new Bounds((max + min)*.5f, max - min);

			for (int i = 0; i < childCount; i++)
			{
				Transform child = parent.GetChild(i);
				if (bounds.Contains(child.position))
					return child;
			}
			return null;
		}
	}

	[CustomEditor(typeof(PrefabBrush))]
	public class PrefabBrushEditor : UnityEditor.Tilemaps.GridBrushEditorBase
	{
		private PrefabBrush prefabBrush { get { return target as PrefabBrush; } }

		private SerializedProperty m_Prefabs;
		private SerializedObject m_SerializedObject;

		protected void OnEnable()
		{
			m_SerializedObject = new SerializedObject(target);
			m_Prefabs = m_SerializedObject.FindProperty("m_Prefabs");
		}

		public override void OnPaintInspectorGUI()
		{
			m_SerializedObject.UpdateIfRequiredOrScript();
			prefabBrush.currentBrush = (TileType)EditorGUILayout.EnumPopup("Brush Type", prefabBrush.currentBrush);
				
			EditorGUILayout.PropertyField(m_Prefabs, true);
			m_SerializedObject.ApplyModifiedPropertiesWithoutUndo();
		}
	}
}
