using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectDisplay : MonoBehaviour {
	public Transform content;
	public GridLayoutGroup layout;
	public LevelDisplay levelDisplayPrefab;

	public static float SCREEN_WIDTH_RATIO = 3.5f;

	public void Init(){
		if (layout != null) {
			float singleCell = Screen.width / SCREEN_WIDTH_RATIO;
			layout.cellSize = new Vector2 (singleCell, singleCell);
		}
		foreach (Transform child in content) {
			Destroy (child.gameObject);
		}

		foreach (LevelScript level in MasterController.instance.levelPrefabs) {
			LevelDisplay disp = (LevelDisplay)Instantiate (levelDisplayPrefab, content, false);

			disp.Init (level);
		}
	}
}
