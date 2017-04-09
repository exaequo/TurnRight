using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterController : MonoBehaviour {
	public static MasterController instance;

	public Canvas gameCanvas;

	public List<LevelScript> levelPrefabs = new List<LevelScript> ();

	public LevelScript currentLevel;
	public GameObject showOnStart;
	public ShowOnEndDisplay showOnEnd;
	public LevelSelectDisplay levelSelectDisplay;


	void Awake () {
		instance = this;
		for (int i = 0; i < levelPrefabs.Count; i++) {
			levelPrefabs [i].levelInfo.levelNumber = i;
		}
	}

	public void ExitGame (){
		Application.Quit ();
	}
		
	public void LoadLevel(LevelScript levelPref){
		levelSelectDisplay.gameObject.SetActive (false);
		LevelScript testLevel = (LevelScript)Instantiate (levelPref, gameCanvas.transform, false);
		currentLevel = testLevel;
		//testLevel.transform.SetSiblingIndex (0);

		testLevel.SetupLevel ();
		showOnStart.SetActive (true);
	}

	public void ExitLevel(){
		if (currentLevel != null) {
			Destroy (currentLevel.gameObject);
		}
	}

	public void OpenLevelSelectDisplay(){
		levelSelectDisplay.gameObject.SetActive (true);
		levelSelectDisplay.Init ();
	}

	public void SaveLevelInfo(LevelScript level){
		levelPrefabs [level.levelInfo.levelNumber].levelInfo.oldScore.ChangeStarScore(level.currentScore.stars);
	}

	public void ShowEndLevelScreen(bool[] starValues){
		showOnEnd.transform.SetAsLastSibling ();
		showOnEnd.gameObject.SetActive (true);
		showOnEnd.starAchievability = starValues;
		showOnEnd.GetComponent<Animator> ().SetTrigger ("Start");
	}

	public void LoadNextLevel(){
		int next = currentLevel.levelInfo.levelNumber + 1;
		Destroy (currentLevel.gameObject);

		if (next < levelPrefabs.Count) {
			LoadLevel (levelPrefabs [next]);
		}
	}
	public void ReloadLevel(){
		int num = currentLevel.levelInfo.levelNumber;
		Destroy (currentLevel.gameObject);

		LoadLevel (levelPrefabs [num]);

	}

	public void ResetProgress(){
		foreach (LevelScript level in levelPrefabs) {
			bool[] array = { false, false, false };
			level.levelInfo.oldScore.stars = array;
		}
		MasterInput.instance.DebugText ("PROGRES RESET");
	}
}
