using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchRayGrabber : MonoBehaviour {
//	public Text text;
	public bool allowRotation = true;
//	RectTransform rectTransform;
	void Start(){
//		rectTransform = GetComponent<RectTransform> ();
	}

	public void TouchBehaviour(bool withTouch){
		float dir = 1;
		if (withTouch) {
			Touch touch = Input.touches [0];

			Debug.Log (touch.deltaPosition.magnitude.ToString ());

			Vector2 position = (Camera.main.WorldToScreenPoint (transform.position));
			if (Mathf.Abs (touch.deltaPosition.x) > Mathf.Abs (touch.deltaPosition.y)) {
				if ((position - touch.position).y > 0) {
					dir = Mathf.Sign (touch.deltaPosition.x);
				} else {
					dir = -Mathf.Sign (touch.deltaPosition.x);
				}
			} else {
				if ((position - touch.position).x > 0) {
					dir = -Mathf.Sign (touch.deltaPosition.y);
				} else {
					dir = Mathf.Sign (touch.deltaPosition.y);
				}
			}

			MasterInput.instance.DebugText ("TOUCH B: " + Input.GetTouch (0).deltaPosition + "\ndirection: " + dir);
		}

		if (allowRotation) {
			iTween.RotateBy (gameObject, Vector3.forward * 0.25f * dir, 0.25f);
		}
	}
}
