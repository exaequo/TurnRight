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
		public bool locked = true;

	}

	public static LevelScript instance;

	public int[] starLevelTimes = { 30, 20, 10 };

	public List<int> ballOrder = new List<int> ();

	public Score currentScore;
	public float currentLevelTime;

	public LevelInfo levelInfo;
	public List<BallSpawner> ballSpawners = new List<BallSpawner>();

	List<BallScript> balls = new List<BallScript>();

	bool countTime = true;
	float startLevelTime;

	public Sprite levelIcon;
//	public GameObject startScreen;
	public BallOrderDisplay ballOrderDisplay;


	void Awake () {
		if (instance != null) {
			Debug.Log ("Turning " + instance.gameObject.name + " off");
			instance.gameObject.SetActive (false);
		}
		instance = this;
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

		if (ballOrderDisplay != null) {
			ballOrderDisplay.Init (ballOrder);
		}
	}


	public void StartLevel(){
		foreach (BallSpawner spawner in ballSpawners) {
			spawner.StartBall ();
		}
		startLevelTime = Time.time;
		countTime = true;

	}

	public void EndLevel(bool won = true){
		countTime = false;
		//levelInfo.oldScore = currentScore;
		if (won) {
			MasterController.instance.SaveLevelInfo (this);
			MasterInput.instance.DebugText ("END: " + currentLevelTime);
		}
	}

	void Update(){
		if (countTime) {
			currentLevelTime = Time.time - startLevelTime;

//			if (currentScore.ThirdStar && currentLevelTime > starLevelTime) {
//				currentScore.ThirdStar = false;
//			}
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
		if (ballOrderDisplay == null || ballOrder.Count > 0) {
			if (ballOrderDisplay == null || ball.ballNumber == ballOrder [0]) {
				MasterInput.instance.DebugText ("GUT BALL");
				if (ballOrderDisplay != null) {
					ballOrder.RemoveAt (0);
					if (ballOrderDisplay != null) {
						StartCoroutine (ballOrderDisplay.DestroyFirst ());
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
				MasterInput.instance.DebugText ("WRONG BALL");
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
		MasterController.instance.ShowEndLevelScreen (currentScore.stars);

	}

	void SetStarsToTrue(){
		for (int i = 0; i < 3; i++) {
			currentScore.ChangeStar (i, true);
		}
	}
}
