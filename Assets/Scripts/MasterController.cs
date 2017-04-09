using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterController : MonoBehaviour {
	public static MasterController instance;

	public Canvas gameCanvas;

	public List<LevelScript> levelPrefabs = new List<LevelScript> ();

	public LevelScript currentLevel;
	public ShowOnEndDisplay showOnEnd;

	int currentLevelNumberTMP = 0;


	void Awake () {
		instance = this;
	}

	public void ExitGame (){
		Application.Quit ();
	}

	public void TemporaryLoadLevel(){
		LoadLevel (levelPrefabs[currentLevelNumberTMP]);
//		currentLevelNumberTMP = 0;
	}

	public void LoadLevel(LevelScript level){
		LevelScript testLevel = (LevelScript)Instantiate (level, gameCanvas.transform, false);
		currentLevel = testLevel;
		//testLevel.transform.SetSiblingIndex (0);

		testLevel.SetupLevel ();
	}

	public void ExitLevel(){
		if (currentLevel != null) {
			Destroy (currentLevel.gameObject);
		}
	}

	public void ShowEndLevelScreen(bool[] starValues){
		showOnEnd.transform.SetAsLastSibling ();
		showOnEnd.gameObject.SetActive (true);
		showOnEnd.starAchievability = starValues;
		showOnEnd.GetComponent<Animator> ().SetTrigger ("Start");
	}

	public void LoadNextLevel(){
		Destroy (currentLevel.gameObject);
		if (currentLevelNumberTMP + 1 < levelPrefabs.Count) {
			++currentLevelNumberTMP;
			LoadLevel (levelPrefabs [currentLevelNumberTMP]);
		}
	}
}
