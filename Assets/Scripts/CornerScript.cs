using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CornerScript : MonoBehaviour {
	public static int RIGHT_DIRECTION = 0;
	public static int FORWARD_DIRECTION = 1;
	public static int LEFT_DIRECTION = 2;
	public static int BACK_DIRECTION = 3;

	public bool DEBUG_MODE = false;

	[Tooltip("Right,Up,Left,Bottom")]
	public SinglePath[] paths = {null, null, null, null};
	[Tooltip("Right,Up,Left,Bottom")]
	public bool[] pathsReversibility = {false, false, false, false};

	List<BallScript> ballsInside = new List<BallScript> ();
	CornerScript adjacentCorner;
//	[Tooltip("Ma być true kiedy to nie skrzyżowanie tylko pomiedzy okregami")]
//	public bool isBeetweenTwoWheels = true;

	void OnTriggerEnter(Collider col){
		CornerScript corner = col.GetComponent<CornerScript> ();
		if (corner != null) {
			adjacentCorner = corner;
			if (DEBUG_MODE) {
				GetComponent<MeshRenderer> ().material.color = Color.green;
			}
//			Debug.Log ("Corner attached");
		}

		BallScript ball = col.GetComponent<BallScript> ();
		if (ball != null) {
			if (!ballsInside.Contains (ball) && CheckParenting(ball)) { //&& !ball.locked) {

				//TODO porownac parenty
//				Debug.Log (gameObject.name + ": Collider entered" + Time.time);
				ballsInside.Add (ball);
				ball.onPathFinish.AddListener (OnBallPathFinish);
				ball.locked = true;
			}
		} 


	}

	bool CheckParenting(BallScript ball){

		return GetComponentInParent<RotatingWheel> () == ball.GetComponentInParent<RotatingWheel> ();
	}

	void OnTriggerExit(Collider col){
		CornerScript corner = col.GetComponent<CornerScript> ();
		if (corner != null) {
			if (corner == adjacentCorner) {
				adjacentCorner = null;
				if (DEBUG_MODE) {
					GetComponent<MeshRenderer> ().material.color = Color.red;
				}
//				Debug.Log ("Corner detached");
			}

		}

		BallScript ball = col.GetComponent<BallScript> ();
		if (ball != null && ballsInside.Contains (ball)) {
//			Debug.Log (gameObject.name + ": Collider exited" + Time.time);
			ballsInside.Remove (ball);
			ball.onPathFinish.RemoveListener (OnBallPathFinish);
			ball.locked = false;
		}
	}
		

	void OnBallPathFinish(BallScript ball){

		List<SinglePath> availPaths = new List<SinglePath> ();
		int originDirection = -1;
		int nextDirection = -1;
		bool backward = false;

		for (int i = 0; i < 4; i++) {
			SinglePath path = paths [i];

			if (path == null) {
				if (adjacentCorner != null) {
					path = adjacentCorner.paths [i];
				}
			}
			availPaths.Add (path);

			if (path == ball.currentPath) {
				originDirection = i;
			}
		}

		//Debug.Log ("originDir" + originDirection);
		string secondOne = "";
		foreach (SinglePath path in availPaths) {
			if (path != null) {
				secondOne += path.gameObject.name;
			} else {
				secondOne += "empty";
			}
				
			secondOne += ", ";
		}

		//Debug.Log (secondOne);

		if (originDirection < 0) {
			MasterInput.instance.DebugText ("No path to go from");
		} else {

			nextDirection = (originDirection + 1) % 4;

			while (availPaths [nextDirection] == null) {
				nextDirection = (nextDirection + 1) % 4;
			}
				
//			if (nextDirection == BACK_DIRECTION || nextDirection == LEFT_DIRECTION) {
//				backward = true;
//			}
			backward = pathsReversibility[nextDirection];
			if (adjacentCorner != null) {
				backward = backward || adjacentCorner.pathsReversibility [nextDirection];
			}

			ball.StartCoroutine (ball.FollowPath (availPaths [nextDirection], backward));
		}



	}
}

