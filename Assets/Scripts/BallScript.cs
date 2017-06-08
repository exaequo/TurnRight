﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable()]public class BallEvent : UnityEvent<BallScript>{}

public class BallScript : MonoBehaviour {
	[Range(0,10)]
	public int ballNumber = 0;
	public int followedPaths = 0;
	public SinglePath currentPath;
//	public Color ballColor = Color.red;

	public bool locked = false;

	public float speed = 1;
	float epsilon = 0.01f;
	[HideInInspector]
	public BallEvent onPathFinish;
	[HideInInspector]
	public BallEvent onMazeFinish;

	void Start () {
		if (ballNumber < MasterController.instance.ballColorCodes.Count) {
			GetComponent<SpriteRenderer> ().color = MasterController.instance.ballColorCodes [ballNumber];
		} else {
			throw new UnityException ("Za duży numer: " + ballNumber +"/" + MasterController.instance.ballColorCodes.Count);
		}
		iTween.ScaleFrom (gameObject, Vector3.zero, 0.5f);


	}

	public void End(){
		iTween.ScaleTo (gameObject, Vector3.zero, 0.5f);
	}

	public IEnumerator FollowPath(SinglePath singlePath, bool reversed = false){
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
			
			transform.position += (path [targetPoint].position - transform.position).normalized * Time.deltaTime * speed;

			if ((transform.position - path [targetPoint].position).magnitude < (epsilon / Time.deltaTime) / 60) {
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
}
