using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MasterInput : MonoBehaviour {

	public static MasterInput instance;

	public Text debugText;

	TouchRayGrabber current;

	void Awake () {
		if (instance == null) {
			instance = this;
		}
	}

	void Update() {
		if (Input.touchCount > 0){
			if (Input.GetTouch(0).phase == TouchPhase.Began) {
				current = CastRayFromPosition(Input.GetTouch(0).position, true);
				DebugText ("Casting ray " + Input.GetTouch (0).position);
			}
			if (Input.GetTouch(0).phase == TouchPhase.Ended){// && Input.GetTouch(0).deltaPosition.magnitude > 0) {
//				CastRayFromPosition(Input.GetTouch(0).position, true);
				DebugText ("Ending ray " + Input.GetTouch (0).deltaPosition);
				if (current != null) {
					current.TouchBehaviour (true);
				}
			}
		}

//		if (Input.GetMouseButtonDown (0)) {
////			CastRayFromPosition (Input.mousePosition, false);
//		}
	}
		

	TouchRayGrabber CastRayFromPosition(Vector2 pos, bool withTouch){
		PointerEventData cursor = new PointerEventData (EventSystem.current);
		cursor.position = pos;
		List<RaycastResult> objectsHit = new List<RaycastResult> ();
		EventSystem.current.RaycastAll(cursor, objectsHit);

		float cameraWidth = Camera.main.ScreenToWorldPoint (new Vector2(Screen.width,0)).x - Camera.main.ScreenToWorldPoint (new Vector2(0,0)).x;


		for (int i = 0; i < objectsHit.Count; i++) {

			RectTransform rectTransform = objectsHit [i].gameObject.GetComponent<RectTransform> ();

			TouchRayGrabber grabber = objectsHit [i].gameObject.GetComponent<TouchRayGrabber> ();

			if (grabber != null) {
				Vector2 worldPos = Camera.main.ScreenToWorldPoint (pos);
				Vector2 objCenterPos = objectsHit [i].gameObject.transform.position;
				float radius = rectTransform.rect.width / Screen.width * cameraWidth / 2;
				//				Debug.Log ("W: " + worldPos  + ", O:" + objCenterPos);
				float distance = (worldPos - objCenterPos).magnitude;

				//				Debug.Log ("D: " + distance + ", R:" + radius);
				if(distance < radius){

					return grabber;
				}
			}
		}
		return null;
	}

	public void DebugText(string text){
		Debug.Log (text);
		debugText.text = text;
	}
}
