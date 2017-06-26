using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectDisplay : MonoBehaviour {
	public Transform content;
	public GridLayoutGroup layout;
	public LevelDisplay levelDisplayPrefab;
	public BackgroundAnimatorControl backgroundAnimator;

	public static float SCREEN_WIDTH_RATIO = 5.5f;
	LevelScript childLevelPrefabToLoad;

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

			disp.Init (level, this);
		}
	
//		if (backgroundAnimator != null) {
//			backgroundAnimator.StartAnimation (true);
//		}
	}

	public void LoadChildLevel(LevelScript levelPrefab){
		if (backgroundAnimator != null) {
			childLevelPrefabToLoad = levelPrefab;
			backgroundAnimator.onShouldStartLoading.AddListener (OnShouldLoadLevel);
			//backgroundAnimator.anim.SetTrigger ("Select");
			backgroundAnimator.SelectAnimation();
		} else {
			MasterController.instance.LoadLevel (levelPrefab);
		}
	}

	void OnShouldLoadLevel(){
		if (childLevelPrefabToLoad != null) {
			MasterController.instance.LoadLevel (childLevelPrefabToLoad);
		} else {
			Debug.Log ("CHILD LEVEL PREFAB IS NULL");
		}
	}
}
