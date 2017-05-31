using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplay : MonoBehaviour {
	public Image levelIcon;
	public StarDisplay[] levelStars = new StarDisplay[3];
	public Image lockedIcon;
	LevelScript levelPrefab;
	public GameObject ballOrderContent;
	public GameObject ballOrderImagePrefab;
	LevelSelectDisplay master;

	public void Init(LevelScript level, LevelSelectDisplay initer){
		master = initer;
		levelPrefab = level;
		if (levelIcon != null) {
			levelIcon.overrideSprite = level.levelIcon;
		}
//		if (lockedIcon != null) {
//			lockedIcon.gameObject.SetActive(level.levelInfo.locked);
//		}
		for (int i = 0; i < 3; i++) {
			if (levelStars [i] != null) {
				levelStars [i].Init (level.levelInfo.oldScore.stars [i]);
			}
		}

		if (ballOrderContent != null) {
			if (level.withBallOrderDisplay) {
				ballOrderContent.SetActive (true);

				foreach (Transform child in ballOrderContent.transform) {
					Destroy (child.gameObject);
				}


				GridLayoutGroup grid = ballOrderContent.GetComponent<GridLayoutGroup> ();

				float cellSize = Screen.width / LevelSelectDisplay.SCREEN_WIDTH_RATIO / (2 + level.ballOrder.Count);
//					//ballOrderContent.GetComponent<RectTransform> ().rect.max.y - ballOrderContent.GetComponent<RectTransform> ().;
//
				MasterInput.instance.DebugText("H: " + cellSize);
//
				grid.cellSize = new Vector2 (cellSize, cellSize);
				grid.padding.left = (int)cellSize / 4;
				grid.spacing = new Vector2(cellSize / 4, 0); 

				foreach (int number in level.ballOrder) {
					GameObject ballImage = Instantiate (ballOrderImagePrefab, grid.transform, false) as GameObject;

					ballImage.GetComponent<Image>().color = MasterController.instance.ballColorCodes [number];
				}
			} else {
				ballOrderContent.SetActive (false);
			}
		}

	}

	public void InvokeLoadLevel(){
		if (levelPrefab != null && master!=null) {
			master.LoadChildLevel (levelPrefab);
			//MasterController.instance.LoadLevel (levelPrefab);
		}
	}
}
