using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchRayGrabber : MonoBehaviour {

	public bool allowRotation = true;

	Vector3 nextRotation;


	void Start(){
		nextRotation = transform.rotation.eulerAngles;
	}

	public void TouchBehaviour(bool withTouch, Vector2 swipeLength){
		float dir = 1;
		if (withTouch) {
			Touch touch = Input.touches [0];

			Vector2 positionOnScreen = (Camera.main.WorldToScreenPoint (transform.position));

			if (swipeLength.magnitude > MasterInput.DEAD_SWIPE) {
				if (Mathf.Abs (swipeLength.x) > Mathf.Abs (swipeLength.y)) {
					if ((positionOnScreen - touch.position).y > 0) {
						dir = Mathf.Sign (swipeLength.x);
					} else {
						dir = -Mathf.Sign (swipeLength.x);
					}
				} else {
					if ((positionOnScreen - touch.position).x > 0) {
						dir = -Mathf.Sign (swipeLength.y);
					} else {
						dir = Mathf.Sign (swipeLength.y);
					}
				}
			} else {
				if ((positionOnScreen - touch.position).x > 0) {
					dir = 1;
				} else {
					dir = -1;
				}
			}

			MasterInput.instance.DebugText ("Magnitude: " + swipeLength.magnitude.ToString () + "\ndirection: " + dir);
		}

		if (allowRotation) {
			Debug.Log ("ROT" + Time.time);
			ChangeNextRotation (Vector3.forward * 90 * dir);
			iTween.RotateTo (gameObject, nextRotation, 0.25f);
		}
	}


	void ChangeNextRotation(Vector3 value){
		nextRotation += value;

		if (nextRotation.x > 180) {
			nextRotation.x -= 360;
		}
		if (nextRotation.x < -180) {
			nextRotation.x += 360;
		}

		if (nextRotation.y > 180) {
			nextRotation.y -= 360;
		}
		if (nextRotation.y < -180) {
			nextRotation.y += 360;
		}

		if (nextRotation.z > 180) {
			nextRotation.z -= 360;
		}
		if (nextRotation.z < -180) {
			nextRotation.z += 360;
		}
	}


}
