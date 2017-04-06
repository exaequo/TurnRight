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

	public float starLevelTime = 60;
	public GameObject startScreen;

	//TODO kolejność kulek jaka ma być na wyjściu

	public Score currentScore;
	public float currentLevelTime;

	public LevelInfo levelInfo;
	public List<BallSpawner> ballSpawners = new List<BallSpawner>();

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
		foreach (BallSpawner spawner in ballSpawners) {
			spawner.SpawnBall ();
		}
	}


	public void StartLevel(){
		foreach (BallSpawner spawner in ballSpawners) {
			spawner.StartBall ();
		}
		startLevelTime = Time.time;
	}

	public void EndLevel(){
		countTime = false;
	}

	void Update(){
		if (countTime) {
			currentLevelTime = Time.time - startLevelTime;

			if (currentScore.ThirdStar && currentLevelTime > starLevelTime) {
				currentScore.ThirdStar = false;
			}
		}

	}
}
