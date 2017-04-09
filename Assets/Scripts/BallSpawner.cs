using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour {
	public Transform spawnPoint;
	public SinglePath pathToStart;

	public BallScript ballToSpawn;
	BallScript ball;

	public void SpawnBall(){
		ball = (BallScript)Instantiate (ballToSpawn);
		ball.transform.parent = pathToStart.transform;
		ball.transform.position = spawnPoint.position;
		LevelScript.instance.BallCreatedNecessaryInvoke (ball);

	}

	public void StartBall(){
		if (ball != null) {
			ball.StartCoroutine (ball.FollowPath (pathToStart));
		} else {
			MasterInput.instance.DebugText ("BALL IS NULL");
		}
	}

//	void Start(){
//		SpawnBall ();
//		StartBall ();
//	}
}
