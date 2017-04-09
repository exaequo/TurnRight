using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplay : MonoBehaviour {
	public Image levelIcon;
	public StarDisplay[] levelStars = new StarDisplay[3];
	public Image lockedIcon;
	LevelScript levelPrefab;


	public void Init(LevelScript level){
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

	}

	public void InvokeLoadLevel(){
		if (levelPrefab != null) {
			MasterController.instance.LoadLevel (levelPrefab);
		}
	}
}
