using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotatingWheel : MonoBehaviour {

	public bool allowRotation = true;

	Vector3 nextRotation;

	void Start(){
		nextRotation = transform.rotation.eulerAngles;
	}

	public void TouchSwipeBehaviour(Vector2 swipeLength){
//		int dir = 1;
//
//		if (swipeLength.x < 0) {
//			dir = -1;
//		}
		if (allowRotation) {
			if (swipeLength.magnitude > 0) {
				float swipeModifier = 200f;

				float swipeRatio = (swipeLength.magnitude / Screen.width);

				MasterInput.instance.DebugText ("" + swipeRatio);

				Vector3 strength = Vector3.forward * swipeRatio * GetRotationDirection (swipeLength) * swipeModifier;

				transform.Rotate (strength);
			}
		}
	}

	public void TouchEndBehaviour(bool withTouch, Vector2 swipeLength){
		float dir = 1;
		if (withTouch) {
			Touch touch = Input.touches [0];

			Vector2 positionOnScreen = (Camera.main.WorldToScreenPoint (transform.position));

//			if (swipeLength.magnitude > MasterInput.DEAD_SWIPE) {
//				if (Mathf.Abs (swipeLength.x) > Mathf.Abs (swipeLength.y)) {
//					if ((positionOnScreen - touch.position).y > 0) {
//						dir = Mathf.Sign (swipeLength.x);
//					} else {
//						dir = -Mathf.Sign (swipeLength.x);
//					}
//				} else {
//					if ((positionOnScreen - touch.position).x > 0) {
//						dir = -Mathf.Sign (swipeLength.y);
//					} else {
//						dir = Mathf.Sign (swipeLength.y);
//					}
//				}
//			} else {
//				if ((positionOnScreen - touch.position).x > 0) {
//					dir = 1;
//				} else {
//					dir = -1;
//				}
//			}

			dir = GetRotationDirection (swipeLength);

			if (LevelScript.instance != null) {
				LevelScript.instance.currentHighscore.numberOfRotations++;
			}

			//MasterInput.instance.DebugText ("Magnitude: " + swipeLength.magnitude.ToString () + "\ndirection: " + dir);
		}

		if (allowRotation) {
//			Debug.Log ("ROT" + Time.time);
//			float val = (transform.rotation.eulerAngles - nextRotation).z;

			float lastRotation = nextRotation.z;

			if (lastRotation <= 0) {
				lastRotation += 360;
			}
			MasterInput.instance.DebugText ("ROT: " + transform.rotation.eulerAngles.z + ", " + lastRotation);

			//ChangeNextRotation (Vector3.forward * 90 * dir);

			Vector3 rot = Vector3.forward * RoundUp ((int)transform.rotation.eulerAngles.z, 90);

			//iTween.RotateTo (gameObject, nextRotation, 0.25f);
			iTween.RotateTo (gameObject, rot, 0.25f);
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

	float GetRotationDirection(Vector2 swipeLength){
		float dir = 1;

		Touch touch = Input.touches [0];

		Vector2 positionOnScreen = (Camera.main.WorldToScreenPoint (transform.position));

//		if (swipeLength.magnitude > MasterInput.DEAD_SWIPE) {
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
//		} else {
//			if ((positionOnScreen - touch.position).x > 0) {
//				dir = 1;
//			} else {
//				dir = -1;
//			}
//		}
		return dir;
	}

	int RoundUp(int numToRound, int multiple)  
	{  
//		if(multiple == 0)  
//		{  
//			return numToRound;  
//		}  
//
//		int remainder = numToRound % multiple; 
//		if (remainder == 0)
//		{
//			return numToRound; 
//		}

		return ((numToRound + multiple/2) / multiple) * multiple;//numToRound + multiple - remainder; 
	}  
}
