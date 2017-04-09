using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelScript : MonoBehaviour {
	[System.Serializable]
	public class LevelInfo
	{
		public Score oldScore;
	}

	public static LevelScript instance;

	public int[] starLevelTimes = { 30, 20, 10 };
	public GameObject startScreen;

	//TODO kolejność kulek jaka ma być na wyjściu

	public Score currentScore;
	public float currentLevelTime;

	public LevelInfo levelInfo;
	public List<BallSpawner> ballSpawners = new List<BallSpawner>();

	List<BallScript> balls = new List<BallScript>();

	bool countTime = true;
	float startLevelTime;



	void Awake () {
		if (instance != null) {
			Debug.Log ("Turning " + instance.gameObject.name + " off");
			instance.gameObject.SetActive (false);
		}
		instance = this;
	}


	public void SetupLevel(){
		startScreen.SetActive (true);
		countTime = false;
		foreach (BallSpawner spawner in ballSpawners) {
			spawner.SpawnBall ();
		}
		MasterInput.instance.SetTimer (0);
		SetStarsToTrue ();
	}


	public void StartLevel(){
		foreach (BallSpawner spawner in ballSpawners) {
			spawner.StartBall ();
		}
		startLevelTime = Time.time;
		countTime = true;

	}

	public void EndLevel(){
		countTime = false;
		MasterInput.instance.DebugText ("END: " + currentLevelTime);
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
		
		MasterInput.instance.DebugText ("BALL (" + ball.gameObject.name + ") finished");
		balls.Remove (ball);
		ball.End ();
		Destroy (ball.gameObject, 1);
		if (balls.Count == 0) {
			EndLevel ();
			StartCoroutine (WaitToEnd ());
		}
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
