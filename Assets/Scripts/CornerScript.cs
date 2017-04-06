using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CornerScript : MonoBehaviour {
	public static int RIGHT_DIRECTION = 0;
	public static int FORWARD_DIRECTION = 1;
	public static int LEFT_DIRECTION = 2;
	public static int BACK_DIRECTION = 3;


	[Tooltip("Right,Up,Left,Bottom")]
	public SinglePath[] paths = {null, null, null, null};

	List<BallScript> ballsInside = new List<BallScript> ();
	CornerScript adjacentCorner;
//	[Tooltip("Ma być true kiedy to nie skrzyżowanie tylko pomiedzy okregami")]
//	public bool isBeetweenTwoWheels = true;

	void OnTriggerEnter(Collider col){
		CornerScript corner = col.GetComponent<CornerScript> ();
		if (corner != null) {
			adjacentCorner = corner;
			Debug.Log ("Corner attached");
		}

		BallScript ball = col.GetComponent<BallScript> ();
		if (ball != null) {
			if (!ballsInside.Contains (ball) && !ball.locked) {
				Debug.Log ("Collider entered" + Time.time);
				ballsInside.Add (ball);
				ball.onPathFinish.AddListener (OnBallPathFinish);
				ball.locked = true;
			}
		} 


	}

	void OnTriggerExit(Collider col){
		CornerScript corner = col.GetComponent<CornerScript> ();
		if (corner != null) {
			if (corner == adjacentCorner) {
				adjacentCorner = null;
				Debug.Log ("Corner detached");
			}

		}

		BallScript ball = col.GetComponent<BallScript> ();
		if (ball != null && ballsInside.Contains (ball)) {
			Debug.Log ("Collider exited" + Time.time);
			ballsInside.Remove (ball);
			ball.onPathFinish.RemoveListener (OnBallPathFinish);
			ball.locked = false;
		}
	}
		

	void OnBallPathFinish(BallScript ball){
		Debug.Log ("Grabbed event from ball" + Time.time);
//		if (isBeetweenTwoWheels) {
//			if (adjacentCorner != null) {
//				//TODO dodać kierunek
//			} else {
//				if (paths [FORWARD_DIRECTION] == null) {
//					Debug.Log ("GO back");
//					ball.StartCoroutine (ball.FollowPath (paths [BACK_DIRECTION], true));
//				} else {
//					Debug.Log ("GO FRONT");
//					ball.StartCoroutine (ball.FollowPath (paths [FORWARD_DIRECTION]));
//				}
//				//paths [Direction.BACK];
//			}
//		}
		Debug.Log("1, " + Time.time);
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

		Debug.Log ("originDir" + originDirection);
		string secondOne = "";
		foreach (SinglePath path in availPaths) {
			if (path != null) {
				secondOne += path.gameObject.name;
			} else {
				secondOne += "empty";
			}
				
			secondOne += ", ";
		}

		Debug.Log (secondOne);

		if (originDirection < 0) {
			MasterInput.instance.DebugText ("No path to go from");
		} else {

			nextDirection = (originDirection + 1) % 4;

			while (availPaths [nextDirection] == null) {
				nextDirection = (nextDirection + 1) % 4;
			}

			Debug.Log ("3");
//
			if (nextDirection == BACK_DIRECTION || nextDirection == LEFT_DIRECTION) {
				backward = true;
			}

			ball.StartCoroutine (ball.FollowPath (availPaths [nextDirection], backward));
		}



	}
}
