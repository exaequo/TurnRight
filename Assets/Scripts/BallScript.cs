﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable()]public class BallEvent : UnityEvent<BallScript>{}

public class BallScript : MonoBehaviour {
	public bool newSystemBall = false;

	[Range(0,10)]
	public int ballNumber = 0;
	public int followedPaths = 0;
	public SinglePath currentPath;
//	public Color ballColor = Color.red;

	public bool locked = false;

	public float speed = 1;
	public static float EPSILON = 1f;
	public static float DEFAULT_EPSILON = 1f;
	public float epsilon = 0.01f;
	[HideInInspector]
	public BallEvent onPathFinish;
	[HideInInspector]
	public BallEvent onMazeFinish;
	SinglePath lastPath;

//	Vector3 lastPos;
//	int stationaryCount;
//	bool lastPathReversity;
//
//	static float RESTART_EPSILON = 0.0001f;

	void Start () {

		if (ballNumber < MasterController.instance.ballColorCodes.Count) {
			
			ChangeColor (MasterController.instance.ballColorCodes [ballNumber]);

		} else {
			throw new UnityException ("Za duży numer: " + ballNumber +"/" + MasterController.instance.ballColorCodes.Count);
		}
		iTween.ScaleFrom (gameObject, Vector3.zero, 0.5f);

		DEFAULT_EPSILON = epsilon;
		EPSILON = epsilon;
//		lastPos = transform.position;
	}

	public void End(){
		iTween.ScaleTo (gameObject, Vector3.zero, 0.5f);
	}

//	void FixedUpdate(){
//		if ((lastPos - transform.position).magnitude <= RESTART_EPSILON) {
//			stationaryCount++;
//			CheckStationaryCount ();
//		}
//		lastPos = transform.position;
//	}
//
//	void CheckStationaryCount(){
//		if (stationaryCount > 30) {
//			stationaryCount = 0;
//			lastPos = transform.position;
//			StartCoroutine (FollowPath (currentPath, !lastPathReversity));
//		}
//	}

	public IEnumerator FollowPath(SinglePath singlePath, bool reversed = false){
		if (singlePath != null) {
//			lastPathReversity = reversed;
			followedPaths++;
			currentPath = singlePath;
			Transform[] path = new Transform[0];
			if (!reversed) {
				path = singlePath.GetPath ();
			} else {
				path = singlePath.GetPathReversed ();
			}

			transform.position = path [0].position;
			transform.parent = singlePath.transform;

			int targetPoint = 1;
			while (targetPoint < path.Length) {
			
				transform.position += (path [targetPoint].position - transform.position).normalized * Time.deltaTime * speed * MasterController.GAME_SPEED;

				if ((transform.position - path [targetPoint].position).magnitude < (EPSILON * Time.deltaTime)) {
					targetPoint++;
				}
				yield return null;
			}
			if (singlePath.IsEnd) {
				SoundManager.instance.PlaySingleSound ("ballFall");
				onMazeFinish.Invoke (this);
			} else {
				onPathFinish.Invoke (this);
			}
			followedPaths--;
		}
		yield return null;
	}

	void ChangeColor(Color color){
		if (!newSystemBall) {
			GetComponent<SpriteRenderer> ().color = color;
		} else {
			GetComponentInChildren<MeshRenderer> ().material.color = color;
			GetComponentInChildren<MeshRenderer> ().material.SetColor ("Shine Color", color);
		}
	}
}
