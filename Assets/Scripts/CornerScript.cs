using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CornerScript : MonoBehaviour {
	public enum Direction
	{
		RIGHT,
		FRONT,
		LEFT,
		BACK
	}

	[Tooltip("Right,Up,Left,Bottom")]
	public SinglePath[] paths = {null, null, null, null};

	List<BallScript> ballsInside = new List<BallScript> ();
	CornerScript adjacentCorner;
	[Tooltip("Ma być true kiedy to nie skrzyżowanie tylko pomiedzy okregami")]
	public bool isBeetweenTwoWheels = true;

	void OnTriggerEnter(Collider col){
		CornerScript corner = col.GetComponent<CornerScript> ();
		if (corner != null) {
			adjacentCorner = corner;
			Debug.Log ("Corner attached");
		}

		BallScript ball = col.GetComponent<BallScript> ();
		if (ball != null) {
			if (!ballsInside.Contains (ball)) {
				Debug.Log ("Collider entered" + Time.time);
				ballsInside.Add (ball);
				ball.onPathFinish.AddListener (OnBallPathFinish);
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
		}
	}
		

	void OnBallPathFinish(BallScript ball){
		Debug.Log ("Grabbed event from ball" + Time.time);
		if (isBeetweenTwoWheels) {
			if (adjacentCorner != null) {
				//TODO dodać kierunek
			} else {
				if (paths [0] == null) {
					Debug.Log ("GO back");
					ball.StartCoroutine (ball.FollowPath (paths [(int)Direction.BACK], true));
				} else {
					Debug.Log ("GO FRONT");
					ball.StartCoroutine (ball.FollowPath (paths [0]));
				}
				//paths [Direction.BACK];
			}
		}
	}
}
