using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterController : MonoBehaviour {
	public static MasterController instance;

	public Canvas gameCanvas;
	public LevelScript testLevelPrefab;
	public LevelScript currentLevel;

	void Awake () {
		instance = this;
	}

	public void ExitGame (){
		Application.Quit ();
	}

	public void TemporaryLoadLevel(){
		LoadLevel (testLevelPrefab);
	}

	public void LoadLevel(LevelScript level){
		LevelScript testLevel = (LevelScript)Instantiate (testLevelPrefab, gameCanvas.transform, false);
		currentLevel = testLevel;
		testLevel.transform.SetSiblingIndex (0);

		testLevel.SetupLevel ();
	}

	public void ExitLevel(){
		if (currentLevel != null) {
			Destroy (currentLevel.gameObject);
		}
	}
}
