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
	public Canvas mainMenuCanvas;


	public List<LevelScript> levelPrefabs = new List<LevelScript> ();

	public LevelScript currentLevel;
	public GameObject showOnStart;
	public ShowOnEndDisplay showOnEnd;
	public ShowOnFailDisplay showOnFail;
//	public CarPaintDisplay carPaintDisplay;
	public LevelSelectDisplay levelSelectDisplay;
	public BallOrderDisplay ballOrderDisplay;
	public List<Color> ballColorCodes = new List<Color> ();
	public GameObject showOnStartStartScreen;
	public Transform levelParent;

	public Image lastLevelImage;
	public Text playLevelText;

	public static float GAME_SPEED = 1;

	bool canLoadLevel = true;
	public bool CanLoadLevel{
		get{ return canLoadLevel; }
	}


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
		LevelScript testLevel = (LevelScript)Instantiate (levelPref, gameCanvas.GetComponentInChildren<BackgroundAnimatorControl>().transform, false);
		currentLevel = testLevel;
		testLevel.transform.SetSiblingIndex (0);

		testLevel.SetupLevel ();
		showOnStart.SetActive (true);
		showOnStartStartScreen.SetActive (true);

		if (levelParent != null) {
			foreach (Transform child in levelParent) {
				Destroy (child.gameObject);
			}
			testLevel.transform.SetParent (levelParent);
		}
	}

	public void ExitLevel(int time = 0){
//		BackgroundAnimatorControl bgAnim = mainMenuCanvas.GetComponentInChildren<BackgroundAnimatorControl> ();
//		if (bgAnim != null) {
//			bgAnim.StartAnimation ();
//		}
		StartCoroutine(WaitToDestroyLevel(time));

	}
	IEnumerator WaitToDestroyLevel(int time){
		yield return new WaitForSeconds (time);
		if (currentLevel != null) {
			Destroy (currentLevel.gameObject);
		}
		ballOrderDisplay.gameObject.SetActive (false);
//		carPaintDisplay.gameObject.SetActive (false);
	}


	public void OpenLevelSelectDisplay(){
		BackgroundAnimatorControl bgAnim = mainMenuCanvas.GetComponentInChildren<BackgroundAnimatorControl> ();
		if (bgAnim != null) {
			bgAnim.EndAnimation ();
		}

		levelSelectDisplay.gameObject.SetActive (true);

		levelSelectDisplay.Init ();
	}

	public bool SaveLevelInfo(LevelScript level){
//		Debug.Log ("LEVEL NUM: " + level.levelInfo.levelNumber);
		bool value = false;

		levelPrefabs [level.levelInfo.levelNumber].levelInfo.oldScore.ChangeStarScore(level.currentScore.stars);
		progress.levelInfos[level.levelInfo.levelNumber].oldScore.ChangeStarScore(level.currentScore.stars);
		if (progress.levelInfos [level.levelInfo.levelNumber].time > level.currentLevelTime) {
			value = true;
			progress.levelInfos [level.levelInfo.levelNumber].time = level.currentLevelTime;
		}

		if (progress.lastWonLevel < level.levelInfo.levelNumber) {
			progress.lastWonLevel = level.levelInfo.levelNumber;
		}

		SaveProgressToFile ();

		UpdateOnMenuItems ();

		return value;
	}

	public void ShowEndLevelScreen(bool[] starValues, List<Color> ballColors, bool withHighscore){
		ballOrderDisplay.gameObject.SetActive (false);

		showOnStart.SetActive (false);
		showOnEnd.transform.SetAsLastSibling ();
		showOnEnd.gameObject.SetActive (true);
		showOnEnd.starAchievability = starValues;
		showOnEnd.showHighscore = withHighscore;
		showOnEnd.GetComponent<Animator> ().SetTrigger ("Start");

//		carPaintDisplay.gameObject.SetActive (true);
//		carPaintDisplay.Init (ballColors);

		//TODO here dać highscore display

	}

	public void LoadNextLevel(){
		int next = currentLevel.levelInfo.levelNumber + 1;
		Destroy (currentLevel.gameObject);

		if (next < levelPrefabs.Count) {
			LoadLevel (levelPrefabs [next]);
		} else {
			ExitLevel ();
			mainMenuCanvas.gameObject.SetActive (true);
		}
//		carPaintDisplay.gameObject.SetActive (false);
	}
	public void ReloadLevel(){
		int num = currentLevel.levelInfo.levelNumber;
		Destroy (currentLevel.gameObject);

		LoadLevel (levelPrefabs [num]);
//		carPaintDisplay.gameObject.SetActive (false);
	}

	public void ResetProgress(){
//		foreach (LevelScript level in levelPrefabs) {
//			bool[] array = { false, false, false };
//			level.levelInfo.oldScore.stars = array;
//		}
		for (int i = 0; i < levelPrefabs.Count; i++) {
			bool[] array = { false, false, false };
			levelPrefabs[i].levelInfo.oldScore.stars = array;
			levelPrefabs [i].levelInfo.time = float.MaxValue;
			levelPrefabs [i].levelInfo.locked = true;
			progress.levelInfos [i] = levelPrefabs [i].levelInfo;
		}

		progress.lastWonLevel = -1;
		SaveProgressToFile ();

		UpdateOnMenuItems ();
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

			UpdateOnMenuItems ();

			return true;
		}
		return false;
	}

	void UpdateOnMenuItems(){
		if (lastLevelImage != null) {
			int which = progress.lastWonLevel + 1;
			if (which >= levelPrefabs.Count) {
				which = levelPrefabs.Count - 1;
			}
			lastLevelImage.overrideSprite = levelPrefabs [which].levelIcon;
		}

		if (playLevelText != null) {
			string text = "CONTINUE";
			if (progress.lastWonLevel < 0) {
				text = "NEW GAME";
			}
			playLevelText.text = text;
		}
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

	public void GameSpeedChange(float value){
//		Time.timeScale = value;
		GAME_SPEED = value;

		BallScript.EPSILON = BallScript.DEFAULT_EPSILON * value;
	}

	public LevelScript GetNextLevelToLoad(){
		return levelPrefabs [progress.lastWonLevel + 1];
	}
	public void WaitToTurnOffMainMenuCanvas(float time){
		canLoadLevel = false;
		StartCoroutine (WaitToTurnOffMainMenuCanvasCoroutine (time));	
	}

	IEnumerator WaitToTurnOffMainMenuCanvasCoroutine(float time){
		yield return new WaitForSeconds (time);
		canLoadLevel = true;
		mainMenuCanvas.gameObject.SetActive (false);
	}

//	public void ContinueNextLevel(){
//		LoadLevel (levelPrefabs [Mathf.Clamp (progress.lastWonLevel + 1, 0, levelPrefabs.Count - 1)]);
//	}
}
