using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable()]public class BallEvent : UnityEvent<BallScript>{}

public class BallScript : MonoBehaviour {
	
	public int followedPaths = 0;
	public SinglePath currentPath;
	public Color ballColor = Color.red;

	public bool locked = false;

	public float speed = 1;
	public float epsilon = 0.5f;
	[HideInInspector]
	public BallEvent onPathFinish;
	[HideInInspector]
	public BallEvent onMazeFinish;

	void Start () {
		GetComponent<SpriteRenderer> ().color = ballColor;
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

			if ((transform.position - path [targetPoint].position).magnitude < epsilon) {
				targetPoint++;
			}
			yield return null;
		}
		if (singlePath.IsEnd) {
			onMazeFinish.Invoke (this);
		} else {
			onPathFinish.Invoke (this);
		}
		followedPaths--;
	}
}
