using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using System.Xml.Serialization;
using System.IO;

public class MasterController : MonoBehaviour {
	


	public Progress progress = new Progress ();
	string progressPath = "/progress.xml";
	public static MasterController instance;

	public Canvas gameCanvas;

	public List<LevelScript> levelPrefabs = new List<LevelScript> ();

	public LevelScript currentLevel;
	public GameObject showOnStart;
	public ShowOnEndDisplay showOnEnd;
	public ShowOnFailDisplay showOnFail;
	public LevelSelectDisplay levelSelectDisplay;
	public BallOrderDisplay ballOrderDisplay;
	public List<Color> ballColorCodes = new List<Color> ();
	public GameObject showOnStartStartScreen;


	void Awake () {
		instance = this;
		if (!LoadProgressFromFile ()) {
			progress.levelInfos.Clear ();
			for (int i = 0; i < levelPrefabs.Count; i++) {
				progress.levelInfos.Add (levelPrefabs [i].levelInfo);
			}
			SaveProgressToFile ();
		}


		for (int i = 0; i < levelPrefabs.Count; i++) {
			levelPrefabs [i].levelInfo.levelNumber = i;
		}

		MasterInput.instance.DebugText (Application.persistentDataPath);
	}

	public void ExitGame (){
		Application.Quit ();
	}
		
	public void LoadLevel(LevelScript levelPref){
		levelSelectDisplay.gameObject.SetActive (false);
		LevelScript testLevel = (LevelScript)Instantiate (levelPref, gameCanvas.transform, false);
		currentLevel = testLevel;
		testLevel.transform.SetSiblingIndex (0);

		testLevel.SetupLevel ();
		showOnStart.SetActive (true);
		showOnStartStartScreen.SetActive (true);
	}

	public void ExitLevel(){
		if (currentLevel != null) {
			Destroy (currentLevel.gameObject);
		}
		ballOrderDisplay.gameObject.SetActive (false);
	}

	public void OpenLevelSelectDisplay(){
		levelSelectDisplay.gameObject.SetActive (true);
		levelSelectDisplay.Init ();
	}

	public void SaveLevelInfo(LevelScript level){
//		Debug.Log ("LEVEL NUM: " + level.levelInfo.levelNumber);

		levelPrefabs [level.levelInfo.levelNumber].levelInfo.oldScore.ChangeStarScore(level.currentScore.stars);
		progress.levelInfos[level.levelInfo.levelNumber].oldScore.ChangeStarScore(level.currentScore.stars);
		SaveProgressToFile ();
	}

	public void ShowEndLevelScreen(bool[] starValues){
		ballOrderDisplay.gameObject.SetActive (false);

		showOnStart.SetActive (false);
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
//		foreach (LevelScript level in levelPrefabs) {
//			bool[] array = { false, false, false };
//			level.levelInfo.oldScore.stars = array;
//		}
		for (int i = 0; i < levelPrefabs.Count; i++) {
			bool[] array = { false, false, false };
			levelPrefabs[i].levelInfo.oldScore.stars = array;
			progress.levelInfos [i] = levelPrefabs [i].levelInfo;
		}

		SaveProgressToFile ();
//		MasterInput.instance.DebugText ("PROGRES RESET");
	}

	public void StartCurrentLevel(){
		currentLevel.StartLevel ();
	}

	public void FailScreen(int was, int shouldve){
		showOnFail.gameObject.SetActive (true);
		showOnFail.Init (was, shouldve);
	}

	public bool LoadProgressFromFile(){
		if (File.Exists(Application.persistentDataPath + progressPath))
		{
			XmlSerializer SerializerObj = new XmlSerializer(typeof(Progress));

			FileStream ReadFileStream = new FileStream(Application.persistentDataPath + progressPath, FileMode.Open, FileAccess.Read, FileShare.Read);

			Progress loaded = (Progress)SerializerObj.Deserialize(ReadFileStream);

			if (loaded.levelInfos.Count != levelPrefabs.Count) {
				return false;
			}

			progress.Replace (loaded);

			ReadFileStream.Close();	

			for (int i = 0; i < levelPrefabs.Count; i++) {
				levelPrefabs [i].levelInfo = progress.levelInfos [i];
			}
			MasterInput.instance.DebugText ("LOAD: " + progress.PrintInfo ());
			return true;
		}
		return false;
	}

	public void SaveProgressToFile(){
		XmlSerializer SerializerObj = new XmlSerializer(typeof(Progress));


		if (Directory.Exists(Application.persistentDataPath) == false)
		{
			Directory.CreateDirectory(Application.persistentDataPath);
		}

		TextWriter WriteFileStream = new StreamWriter(Application.persistentDataPath + progressPath);
		SerializerObj.Serialize(WriteFileStream, progress);

		WriteFileStream.Close();
		MasterInput.instance.DebugText ("SAVE: " + this.progress.PrintInfo ());
//		MasterInput.instance.DebugText ("Progress saved");
	}
}
