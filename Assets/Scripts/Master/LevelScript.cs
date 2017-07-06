using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelScript : MonoBehaviour {
	[System.Serializable]
	public class LevelInfo
	{
		public int levelNumber = -1;
		public Score oldScore;

		public float time = float.MaxValue;
		public bool locked = true;

	}

	public bool newGameplayApplied = false;
	public bool withBallOrderDisplay = false;

	public static LevelScript instance;

	public int[] starLevelTimes = { 30, 20, 10 };

	public List<int> ballOrder = new List<int> ();
	public List<Color> ballOrderColorSav = new List<Color> ();

	public Score currentScore;
	public SingleHighscore currentHighscore = new SingleHighscore();
	public float currentLevelTime;

	public LevelInfo levelInfo;
	public List<BallSpawner> ballSpawners = new List<BallSpawner>();

	List<BallScript> balls = new List<BallScript>();

	bool countTime = true;
	float startLevelTime;

	public Sprite levelIcon;
//	public GameObject startScreen;
	BallOrderDisplay ballOrderDisp;
	bool newHighscoreAchieved = false;

	float speedChangeTimerStart = -1;

	void Awake () {
		if (instance != null) {
			Debug.Log ("Turning " + instance.gameObject.name + " off");
			instance.gameObject.SetActive (false);
		}
		instance = this;
		if (withBallOrderDisplay) {
			foreach (int num in ballOrder) {
				ballOrderColorSav.Add (MasterController.instance.ballColorCodes [num]);
			}
		} else {
			foreach (BallSpawner spawner in ballSpawners) {
				ballOrderColorSav.Add (MasterController.instance.ballColorCodes [spawner.ballToSpawn.ballNumber]);
			}
		}
		MasterController.instance.onGameSpeedChange.AddListener (OnGameSpeedChange);
	}

	void OnGameSpeedChange(float value){
		if (value != 1f) {
			
			speedChangeTimerStart = currentLevelTime;
		} else {
			if (speedChangeTimerStart != -1) {
				currentHighscore.speedUpTime += (currentLevelTime - speedChangeTimerStart);
				speedChangeTimerStart = -1;
			}
		}
	}

	public void SetupLevel(){
//		if (startScreen != null) {
//			startScreen.SetActive (true);
//		}
		countTime = false;
		foreach (BallSpawner spawner in ballSpawners) {
			spawner.SpawnBall ();
		}
		MasterInput.instance.SetTimer (0);
		SetStarsToTrue ();

		if (withBallOrderDisplay) {
			ballOrderDisp = MasterController.instance.ballOrderDisplay;
			ballOrderDisp.gameObject.SetActive (true);
			ballOrderDisp.Init (ballOrder);
		}

//		if (ballOrderDisplay != null) {
//			ballOrderDisplay.Init (ballOrder);
//		}
	}


	public void StartLevel(){
		foreach (BallSpawner spawner in ballSpawners) {
			spawner.StartBall ();
		}
		startLevelTime = Time.time;
		countTime = true;

	}

	public void EndLevel(bool won = true){
		MasterController.instance.GameSpeedChange (1);
		countTime = false;
		//levelInfo.oldScore = currentScore;
		if (won) {
			currentHighscore.time = currentLevelTime;
			currentHighscore.date = System.DateTime.Now.ToString ();
			currentHighscore.speedUpTime = (currentHighscore.speedUpTime / currentLevelTime) * 100;
			newHighscoreAchieved = MasterController.instance.SaveLevelInfo (this);
			MasterInput.instance.DebugText ("END: " + currentLevelTime);
		}
	}

	void Update(){
		if (countTime) {
			currentLevelTime = Time.time - startLevelTime;

			for (int i = 0; i < 3; i++) {
				currentScore.ChangeStar (i, currentLevelTime <= starLevelTimes [i]);
			}

			MasterInput.instance.SetTimer (currentLevelTime);
		}
	}

	public void BallCreatedNecessaryInvoke(BallScript ball){
		ball.onMazeFinish.AddListener (OnBallMazeFinish);
		balls.Add (ball);
	}

	void OnBallMazeFinish (BallScript ball){
		if (ballOrderDisp == null || ballOrder.Count > 0) {
			if (ballOrderDisp == null || ball.ballNumber == ballOrder [0]) {
//				MasterInput.instance.DebugText ("GUT BALL");
				if (ballOrderDisp != null) {
					ballOrder.RemoveAt (0);
					if (ballOrderDisp != null) {
						StartCoroutine (ballOrderDisp.DestroyFirst ());
					}
				}

				balls.Remove (ball);
				ball.End ();
				Destroy (ball.gameObject, 1);
				if (balls.Count == 0) {
					EndLevel ();
					StartCoroutine (WaitToEnd ());
				}

			} else {
//				MasterInput.instance.DebugText ("WRONG BALL");
				MasterController.instance.FailScreen (ball.ballNumber, ballOrder [0]);
				EndLevel (false);
				foreach(BallScript current in balls){
					Destroy (current.gameObject);
				}
			}
		}
		//MasterInput.instance.DebugText ("BALL (" + ball.gameObject.name + ") finished");

	}

	IEnumerator WaitToEnd(){
		yield return new WaitForSeconds (1);
		//MasterController.instance.ShowEndLevelScreen (currentScore.stars);
		transform.SetAsLastSibling();


		if (GetComponent<Animator> () != null) {
			GetComponent<Animator> ().SetTrigger ("End");
		}
	}

	public void InformMasterAboutEnding(){
		MasterController.instance.ShowEndLevelScreen (currentScore.stars, ballOrderColorSav, newHighscoreAchieved);
	}

	void SetStarsToTrue(){
		for (int i = 0; i < 3; i++) {
			currentScore.ChangeStar (i, true);
		}
	}
}
