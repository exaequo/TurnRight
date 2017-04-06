using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable()]public class BallEvent : UnityEvent<BallScript>{}

public class BallScript : MonoBehaviour {
	//[HideInInspector]
	public int followedPaths = 0;
	public SinglePath currentPath;

//	public CornerScript currentCorner;

	//public SinglePath path;//TODO wypierdolic <-
	public bool locked = false;

	public float speed = 1;
	public float epsilon = 0.5f;
	[HideInInspector]
	public BallEvent onPathFinish;

	// Use this for initialization
	void Start () {
		//StartCoroutine(FollowPath (path));
	}
	
	// Update is called once per frame
	void Update () {
		
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
		//

		transform.position = path [0].position;
		transform.parent = singlePath.transform;

		int targetPoint = 1;
		while (targetPoint < path.Length) {
			//transform.Translate ((path [targetPoint].localPosition - transform.localPosition).normalized * Time.deltaTime * speed);

			transform.position += (path [targetPoint].position - transform.position).normalized * Time.deltaTime * speed;

			if ((transform.position - path [targetPoint].position).magnitude < epsilon) {
				targetPoint++;
			}
			yield return null;
		}
		onPathFinish.Invoke (this);
		followedPaths--;
		//Debug.Log ("Invoked" + Time.deltaTime);
	}
}
